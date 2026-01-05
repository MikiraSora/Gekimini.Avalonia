using System.Collections.Generic;
using System.Linq;
using Avalonia.Platform.Storage;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework;

namespace Gekimini.Avalonia.Utils;

public static class FileTypeHelper
{
    public const string UNKNOWN_MIMETYPE = "application/unknown";

    public static FilePickerFileType[] BuildFileTypeFilters(
        this IEnumerable<EditorFileType> supportFileTypes,
        bool includeAllSupportTypeFilter = true)
    {
        if (supportFileTypes == null)
            return [];

        var result = new List<FilePickerFileType>();

        foreach (var fileType in supportFileTypes)
        {
            if (fileType == null)
                continue;

            var patterns = new List<string>();
            var mimeTypes = new List<string>();

            for (var i = 0; i < fileType.Patterns.Length; i++)
            {
                var pattern = fileType.Patterns[i];
                var mimeType = fileType.MimeTypes.ElementAtOrDefault(i) ?? UNKNOWN_MIMETYPE;
                patterns.Add(pattern);
                mimeTypes.Add(mimeType);
            }

            var pickerType = new FilePickerFileType(fileType.Name.Text)
            {
                Patterns = patterns,
                MimeTypes = mimeTypes
            };

            result.Add(pickerType);
        }

        if (includeAllSupportTypeFilter)
        {
            var pickerType = new FilePickerFileType(ProgramLanguages.AllSupportedFiles)
            {
                Patterns = result.SelectMany(x => x.Patterns).ToArray(),
                MimeTypes = result.SelectMany(x => x.MimeTypes).ToArray()
            };

            result.Insert(0, pickerType);
        }

        return result.ToArray();
    }
}