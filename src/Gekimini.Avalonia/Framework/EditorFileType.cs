using System;

namespace Gekimini.Avalonia.Framework;

public class EditorFileType
{
    public EditorFileType(string name , Uri iconSource = null)
    {
        Name = name;
        IconSource = iconSource;
    }

    public string Name { get; set; }
    public Uri IconSource { get; set; }
}