using System;
using System.Collections.Generic;
using System.Linq;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.RecentFiles.DefaultImpl;

[RegisterSingleton<IEditorRecentFilesManager>]
public partial class DefaultEditorRecentFilesManager : IEditorRecentFilesManager
{
    private readonly RecentRecordDataStoreSetting dataSetting;
    private readonly List<RecentRecordInfo> recentRecordInfos = new();
    private RecentRecordInfoStoreSetting setting;

    public DefaultEditorRecentFilesManager()
    {
        LoadRecordOpenedList();
        dataSetting = SettingManager.GetSetting(RecentRecordDataStoreSetting.JsonTypeInfo);
        setting = SettingManager.GetSetting(RecentRecordInfoStoreSetting.JsonTypeInfo);
    }

    [GetServiceLazy]
    private partial ISettingManager SettingManager { get; }

    public IEnumerable<RecentRecordInfo> RecentRecordInfos => recentRecordInfos;

    public RecentRecordInfo PostRecent(EditorFileType editorFileType, string name, string locationDescription)
    {
        var info = new RecentRecordInfo(editorFileType.Name, name, locationDescription, DateTime.Now);

        foreach (var deleteInfo in recentRecordInfos
                     .Where(x => x.Name == info.Name && x.LocationDescription == info.LocationDescription).ToArray())
        {
            recentRecordInfos.Remove(deleteInfo);
            ClearData(deleteInfo);
        }

        recentRecordInfos.Insert(0, info);
        SaveRecordOpenedList();

        return info;
    }

    public void ClearAllRecordsAndDatas()
    {
        foreach (var info in recentRecordInfos)
            ClearData(info);
        recentRecordInfos.Clear();
        SaveRecordOpenedList();
    }

    public void ClearData(RecentRecordInfo info)
    {
        dataSetting.RecordInfoDataMap.Remove(GetRecordInfoDataKey(info));
        SettingManager.SaveSetting(dataSetting, RecentRecordDataStoreSetting.JsonTypeInfo);
    }

    public byte[] ReadData(RecentRecordInfo info)
    {
        var key = GetRecordInfoDataKey(info);
        return dataSetting.RecordInfoDataMap.GetValueOrDefault(key);
    }

    public void WriteData(RecentRecordInfo info, byte[] data)
    {
        dataSetting.RecordInfoDataMap[GetRecordInfoDataKey(info)] = data;
        SettingManager.SaveSetting(dataSetting, RecentRecordDataStoreSetting.JsonTypeInfo);
    }

    private string GetRecordInfoDataKey(RecentRecordInfo info)
    {
        return HashCode.Combine(info.Name, info.LocationDescription).ToString();
    }

    private void SaveRecordOpenedList()
    {
        var list = recentRecordInfos.Take(setting.RecordMaxCount).ToList();
        setting.RecentRecordInfoList = list;

        SettingManager.SaveSetting(setting, RecentRecordInfoStoreSetting.JsonTypeInfo);
    }

    private void LoadRecordOpenedList()
    {
        recentRecordInfos.Clear();
        setting = SettingManager.GetSetting(RecentRecordInfoStoreSetting.JsonTypeInfo);
        recentRecordInfos.AddRange(setting.RecentRecordInfoList);
    }
}