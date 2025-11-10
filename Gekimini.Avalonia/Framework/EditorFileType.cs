using System;

namespace Gekimini.Avalonia.Framework;

public class EditorFileType
{
    public EditorFileType(string name, string fileExtension, Uri iconSource = null)
    {
        Name = name;
        FileExtension = fileExtension;
        IconSource = iconSource;
    }

    public string Name { get; set; }
    public string FileExtension { get; set; }
    public Uri IconSource { get; set; }
}