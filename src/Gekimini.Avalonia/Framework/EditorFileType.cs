using System;
using Gekimini.Avalonia.Framework.Languages;

namespace Gekimini.Avalonia.Framework;

public class EditorFileType
{
    public EditorFileType(string id, LocalizedString name, Uri iconSource = null)
    {
        Id = id;
        Name = name;
        IconSource = iconSource;
    }

    public string Id { get; set; }
    public LocalizedString Name { get; set; }
    public string[] Patterns { get; set; } = [];
    public string[] MimeTypes { get; set; } = [];
    public Uri IconSource { get; set; }
}