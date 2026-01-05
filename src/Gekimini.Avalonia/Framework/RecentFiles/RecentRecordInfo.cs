using System;

namespace Gekimini.Avalonia.Framework.RecentFiles;

public record RecentRecordInfo(
    string EditorFileTypeId,
    string Name,
    string LocationDescription,
    DateTime? LastAccessTime = default);