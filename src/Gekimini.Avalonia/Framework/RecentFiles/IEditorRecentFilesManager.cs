using System.Collections.Generic;

namespace Gekimini.Avalonia.Framework.RecentFiles;

public interface IEditorRecentFilesManager
{
    IEnumerable<RecentRecordInfo> RecentRecordInfos { get; }
    RecentRecordInfo PostRecent(EditorFileType editorFileType, string name, string locationDescription);

    void ClearAllRecordsAndDatas();

    byte[] ReadData(RecentRecordInfo info);
    void WriteData(RecentRecordInfo info, byte[] data);
    void ClearData(RecentRecordInfo info);
}