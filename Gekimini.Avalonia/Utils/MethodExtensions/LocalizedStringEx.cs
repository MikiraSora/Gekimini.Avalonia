using Gekimini.Avalonia.Framework.Languages;

namespace Gekimini.Avalonia.Utils.MethodExtensions;

public static class LocalizedStringEx
{
    public static LocalizedString ToLocalizedStringByRawText(this string rawText)
    {
        return LocalizedString.CreateFromRawText(rawText);
    }
}