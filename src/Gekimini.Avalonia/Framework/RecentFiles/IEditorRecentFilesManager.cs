using System;
using System.Collections.Generic;

namespace Gekimini.Avalonia.Framework.RecentFiles;

public interface IEditorRecentFilesManager
{
    IEnumerable<RecentRecordInfo> RecentRecordInfos { get; }

    RecentRecordInfo PostRecent(
        EditorFileType editorFileType,
        string name,
        string locationDescription,
        byte[] data = null);

    RecentRecordInfo UpdateRecent(
        Guid recordId,
        string name,
        string locationDescription,
        byte[] data = null);

    void ClearAllRecordsAndDatas();

    byte[] ReadData(RecentRecordInfo info);
    void WriteData(RecentRecordInfo info, byte[] data);
    void ClearData(RecentRecordInfo info);

    bool IsMarkedInvalid(RecentRecordInfo info);
    void SetMarkedInvalid(RecentRecordInfo info, bool isInvalid);
}
