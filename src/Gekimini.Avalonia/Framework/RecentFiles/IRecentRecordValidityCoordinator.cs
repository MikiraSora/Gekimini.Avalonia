using System;
using System.Threading.Tasks;

namespace Gekimini.Avalonia.Framework.RecentFiles;

public interface IRecentRecordValidityCoordinator
{
    long BeginValidationGeneration();

    Task<bool> GetOrCheckAsync(
        RecentRecordInfo recordInfo,
        Func<Task<bool>> checkFactory);

    Task<bool> CheckFreshAsync(
        RecentRecordInfo recordInfo,
        Func<Task<bool>> checkFactory);

    void Invalidate(Guid recordId);
}
