using System;
using System.Collections.Generic;
using System.Linq;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.RecentFiles.DefaultImpl;

[RegisterSingleton<IEditorRecentFilesManager>]
public sealed class DefaultEditorRecentFilesManager : IEditorRecentFilesManager
{
    private readonly ISettingManager settingManager;
    private readonly IRecentRecordValidityCoordinator validityCoordinator;
    private readonly object syncRoot = new();
    private RecentRecordInfoStoreSetting setting;

    public DefaultEditorRecentFilesManager(
        ISettingManager settingManager,
        IRecentRecordValidityCoordinator validityCoordinator = null)
    {
        this.settingManager = settingManager ?? throw new ArgumentNullException(nameof(settingManager));
        this.validityCoordinator = validityCoordinator;

        var loaded = settingManager.GetSetting(RecentRecordInfoStoreSetting.JsonTypeInfo);
        var clearLegacyData = false;
        if (loaded.Version != RecentRecordInfoStoreSetting.CurrentVersion)
        {
            setting = new RecentRecordInfoStoreSetting
            {
                Version = RecentRecordInfoStoreSetting.CurrentVersion,
                RecordMaxCount = loaded.RecordMaxCount > 0 ? loaded.RecordMaxCount : 10
            };

            // V2 is an intentional direct cut. Legacy location-keyed data cannot carry folder capabilities.
            clearLegacyData = true;
        }
        else
        {
            setting = Clone(loaded);
            Normalize(setting);
        }

        settingManager.SaveSetting(setting, RecentRecordInfoStoreSetting.JsonTypeInfo);
        if (clearLegacyData)
            settingManager.SaveSetting(new RecentRecordDataStoreSetting(), RecentRecordDataStoreSetting.JsonTypeInfo);
    }

    public IEnumerable<RecentRecordInfo> RecentRecordInfos
    {
        get
        {
            lock (syncRoot)
                return setting.RecentRecordInfoList.ToArray();
        }
    }

    public RecentRecordInfo PostRecent(
        EditorFileType editorFileType,
        string name,
        string locationDescription,
        byte[] data = null)
    {
        ArgumentNullException.ThrowIfNull(editorFileType);
        var info = new RecentRecordInfo(
            editorFileType.Id,
            name ?? string.Empty,
            locationDescription ?? string.Empty,
            DateTime.Now,
            Guid.NewGuid());

        Commit(next =>
        {
            next.RecentRecordInfoList.Insert(0, info);
            if (data is not null)
                next.RecordInfoDataMap[info.RecordId] = data.ToArray();
        });

        return info;
    }

    public RecentRecordInfo UpdateRecent(
        Guid recordId,
        string name,
        string locationDescription,
        byte[] data = null)
    {
        if (recordId == Guid.Empty)
            throw new ArgumentException("A recent record ID cannot be empty.", nameof(recordId));

        RecentRecordInfo updated = null;
        Commit(next =>
        {
            var index = next.RecentRecordInfoList.FindIndex(x => x.RecordId == recordId);
            if (index < 0)
                throw new KeyNotFoundException($"Recent record '{recordId:N}' was not found.");

            var current = next.RecentRecordInfoList[index];
            updated = current with
            {
                Name = name ?? string.Empty,
                LocationDescription = locationDescription ?? string.Empty,
                LastAccessTime = DateTime.Now
            };

            next.RecentRecordInfoList.RemoveAt(index);
            next.RecentRecordInfoList.Insert(0, updated);
            next.InvalidRecordIds.Remove(recordId);
            if (data is not null)
                next.RecordInfoDataMap[recordId] = data.ToArray();
        });
        validityCoordinator?.Invalidate(recordId);

        return updated;
    }

    public bool RemoveRecent(Guid recordId)
    {
        if (recordId == Guid.Empty)
            return false;

        lock (syncRoot)
        {
            if (setting.RecentRecordInfoList.All(x => x.RecordId != recordId))
                return false;
        }

        Commit(next =>
        {
            next.RecentRecordInfoList.RemoveAll(x => x.RecordId == recordId);
            next.RecordInfoDataMap.Remove(recordId);
            next.InvalidRecordIds.Remove(recordId);
        });
        validityCoordinator?.Invalidate(recordId);
        return true;
    }

    public void ClearAllRecordsAndDatas()
    {
        Commit(next =>
        {
            next.RecentRecordInfoList.Clear();
            next.RecordInfoDataMap.Clear();
            next.InvalidRecordIds.Clear();
        });
        validityCoordinator?.BeginValidationGeneration();
    }

    public byte[] ReadData(RecentRecordInfo info)
    {
        ArgumentNullException.ThrowIfNull(info);
        lock (syncRoot)
        {
            return setting.RecordInfoDataMap.TryGetValue(info.RecordId, out var data)
                ? data.ToArray()
                : null;
        }
    }

    public void WriteData(RecentRecordInfo info, byte[] data)
    {
        ArgumentNullException.ThrowIfNull(info);
        ArgumentNullException.ThrowIfNull(data);
        EnsureKnownRecord(info.RecordId);
        Commit(next => next.RecordInfoDataMap[info.RecordId] = data.ToArray());
        validityCoordinator?.Invalidate(info.RecordId);
    }

    public void ClearData(RecentRecordInfo info)
    {
        ArgumentNullException.ThrowIfNull(info);
        Commit(next => next.RecordInfoDataMap.Remove(info.RecordId));
        validityCoordinator?.Invalidate(info.RecordId);
    }

    public bool IsMarkedInvalid(RecentRecordInfo info)
    {
        ArgumentNullException.ThrowIfNull(info);
        lock (syncRoot)
            return setting.InvalidRecordIds.Contains(info.RecordId);
    }

    public void SetMarkedInvalid(RecentRecordInfo info, bool isInvalid)
    {
        ArgumentNullException.ThrowIfNull(info);
        EnsureKnownRecord(info.RecordId);
        Commit(next =>
        {
            if (isInvalid)
                next.InvalidRecordIds.Add(info.RecordId);
            else
                next.InvalidRecordIds.Remove(info.RecordId);
        });
        validityCoordinator?.Invalidate(info.RecordId);
    }

    private void EnsureKnownRecord(Guid recordId)
    {
        lock (syncRoot)
        {
            if (recordId == Guid.Empty || setting.RecentRecordInfoList.All(x => x.RecordId != recordId))
                throw new KeyNotFoundException($"Recent record '{recordId:N}' was not found.");
        }
    }

    private void Commit(Action<RecentRecordInfoStoreSetting> mutation)
    {
        lock (syncRoot)
        {
            var next = Clone(setting);
            mutation(next);
            Normalize(next);
            settingManager.SaveSetting(next, RecentRecordInfoStoreSetting.JsonTypeInfo);
            setting = next;
        }
    }

    private static RecentRecordInfoStoreSetting Clone(RecentRecordInfoStoreSetting source)
    {
        return new RecentRecordInfoStoreSetting
        {
            Version = RecentRecordInfoStoreSetting.CurrentVersion,
            RecordMaxCount = source.RecordMaxCount,
            RecentRecordInfoList = source.RecentRecordInfoList?.ToList() ?? [],
            RecordInfoDataMap = source.RecordInfoDataMap?.ToDictionary(
                pair => pair.Key,
                pair => pair.Value?.ToArray() ?? []) ?? new Dictionary<Guid, byte[]>(),
            InvalidRecordIds = source.InvalidRecordIds is null
                ? []
                : [.. source.InvalidRecordIds]
        };
    }

    private static void Normalize(RecentRecordInfoStoreSetting target)
    {
        target.Version = RecentRecordInfoStoreSetting.CurrentVersion;
        target.RecordMaxCount = Math.Max(0, target.RecordMaxCount);
        target.RecentRecordInfoList ??= [];
        target.RecordInfoDataMap ??= new Dictionary<Guid, byte[]>();
        target.InvalidRecordIds ??= [];

        var seen = new HashSet<Guid>();
        target.RecentRecordInfoList.RemoveAll(info =>
            info is null || info.RecordId == Guid.Empty || !seen.Add(info.RecordId));

        while (target.RecentRecordInfoList.Count > target.RecordMaxCount)
        {
            var removed = target.RecentRecordInfoList[^1];
            target.RecentRecordInfoList.RemoveAt(target.RecentRecordInfoList.Count - 1);
            target.RecordInfoDataMap.Remove(removed.RecordId);
            target.InvalidRecordIds.Remove(removed.RecordId);
        }

        var retainedIds = target.RecentRecordInfoList.Select(x => x.RecordId).ToHashSet();
        foreach (var orphanId in target.RecordInfoDataMap.Keys.Where(x => !retainedIds.Contains(x)).ToArray())
            target.RecordInfoDataMap.Remove(orphanId);
        target.InvalidRecordIds.RemoveWhere(x => !retainedIds.Contains(x));
    }
}
