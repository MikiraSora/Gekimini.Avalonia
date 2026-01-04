using System;

namespace Gekimini.Avalonia.Framework.RecentFiles;

public record RecentRecordInfo(
    string editorFileTypeName,
    string Name,
    string LocationDescription,
    DateTime? LastAccessTime = default);