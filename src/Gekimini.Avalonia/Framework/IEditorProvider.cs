using System.Collections.Generic;
using System.Threading.Tasks;
using Gekimini.Avalonia.Framework.RecentFiles;

namespace Gekimini.Avalonia.Framework;

public interface IEditorProvider
{
    IEnumerable<EditorFileType> FileTypes { get; }

    bool CanCreateNew { get; }

    /// <summary>
    ///     create an implemented empty document object.
    /// </summary>
    /// <returns></returns>
    IDocumentViewModel Create();

    /// <summary>
    ///     Document try create something， document implment can ask user what's they should be configure/create.
    /// </summary>
    /// <param name="document"></param>
    /// <returns>if return true, document will be shown in shell.</returns>
    Task<bool> TryNew(IDocumentViewModel document);

    /// <summary>
    ///     Document try open something, document implement can ask what should be load.
    /// </summary>
    /// <param name="document"></param>
    /// <returns>if return true, document will be shown in shell.</returns>
    Task<bool> TryOpen(IDocumentViewModel document);

    /// <summary>
    ///     Document try open something from recent info
    /// </summary>
    /// <param name="document"></param>
    /// <returns>if return true, document will be shown in shell.</returns>
    Task<bool> TryOpen(IDocumentViewModel document, RecentRecordInfo recordInfo);

    /// <summary>
    ///     Checks whether a recent record can still be opened by this provider.
    /// </summary>
    /// <param name="recordInfo">The recent record to validate.</param>
    /// <returns><see langword="true"/> when the record is still valid.</returns>
    Task<bool> CheckIsValid(RecentRecordInfo recordInfo);
}
