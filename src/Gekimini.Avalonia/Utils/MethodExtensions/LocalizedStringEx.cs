using System.Collections.Generic;
using Gekimini.Avalonia.Framework.Languages;
using SimpleTypedLocalizer;

namespace Gekimini.Avalonia.Utils.MethodExtensions;

public static class LocalizedStringEx
{
    private static readonly Dictionary<ILocalizedTextSource, LocalizedString> cachedTextSourceToLocalizedStringMap =
        new();

    public static LocalizedString ToLocalizedStringByRawText(this string rawText)
    {
        return LocalizedString.CreateFromRawText(rawText);
    }

    extension(ILocalizedTextSource source)
    {
        public LocalizedString ToLocalizedString()
        {
            if (!cachedTextSourceToLocalizedStringMap.TryGetValue(source, out var localizedString))
                localizedString = cachedTextSourceToLocalizedStringMap[source] =
                    LocalizedString.CreateFromTemplateFunc(() => source.Text);
            return localizedString;
        }

        public LocalizedString ToFormatLocalizedString(params object[] args)
        {
            return LocalizedString.CreateFromTemplateFunc(() => source.Text.FormatEx(args));
        }
    }
}