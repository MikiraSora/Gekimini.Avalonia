using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.RecentFiles.DefaultImpl;

[RegisterSingleton<IRecentRecordValidityCoordinator>]
public sealed class RecentRecordValidityCoordinator : IRecentRecordValidityCoordinator
{
    private readonly object syncRoot = new();
    private readonly Dictionary<(Guid RecordId, long Generation), Task<bool>> checks = new();
    private long generation = 1;

    public long BeginValidationGeneration()
    {
        lock (syncRoot)
        {
            generation++;
            checks.Clear();
            return generation;
        }
    }

    public Task<bool> GetOrCheckAsync(
        RecentRecordInfo recordInfo,
        Func<Task<bool>> checkFactory)
    {
        ArgumentNullException.ThrowIfNull(recordInfo);
        ArgumentNullException.ThrowIfNull(checkFactory);

        if (recordInfo.RecordId == Guid.Empty)
            return InvokeFactory(checkFactory);

        lock (syncRoot)
        {
            var key = (recordInfo.RecordId, generation);
            if (checks.TryGetValue(key, out var existing))
                return existing;

            var created = InvokeFactory(checkFactory);
            checks[key] = created;
            return created;
        }
    }

    public Task<bool> CheckFreshAsync(
        RecentRecordInfo recordInfo,
        Func<Task<bool>> checkFactory)
    {
        ArgumentNullException.ThrowIfNull(recordInfo);
        ArgumentNullException.ThrowIfNull(checkFactory);

        if (recordInfo.RecordId == Guid.Empty)
            return InvokeFactory(checkFactory);

        lock (syncRoot)
        {
            var created = InvokeFactory(checkFactory);
            checks[(recordInfo.RecordId, generation)] = created;
            return created;
        }
    }

    public void Invalidate(Guid recordId)
    {
        if (recordId == Guid.Empty)
            return;

        lock (syncRoot)
        {
            foreach (var key in checks.Keys.Where(x => x.RecordId == recordId).ToArray())
                checks.Remove(key);
        }
    }

    private static Task<bool> InvokeFactory(Func<Task<bool>> checkFactory)
    {
        try
        {
            return checkFactory() ?? Task.FromResult(false);
        }
        catch (Exception exception)
        {
            return Task.FromException<bool>(exception);
        }
    }
}
