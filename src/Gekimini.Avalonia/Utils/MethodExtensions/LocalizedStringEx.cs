using Gekimini.Avalonia.Framework.Languages;
using SimpleTypedLocalizer;

namespace Gekimini.Avalonia.Utils.MethodExtensions;

public static class LocalizedStringEx
{
    public static LocalizedString ToLocalizedStringByRawText(this string rawText)
    {
        return LocalizedString.CreateFromRawText(rawText);
    }

    public static LocalizedString ToLocalizedString(this ILocalizedTextSource rawText)
    {
        return LocalizedString.CreateFromTemplateFunc(() => rawText.Text);
    }
}