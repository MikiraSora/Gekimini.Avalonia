using Gekimini.Avalonia.Framework.Languages;
using SimpleTypedLocalizer;

namespace Gekimini.Avalonia.Utils.MethodExtensions;

public static class LocalizedStringEx
{
    public static LocalizedString ToLocalizedStringByRawText(this string rawText)
    {
        return LocalizedString.CreateFromRawText(rawText);
    }

    extension(ILocalizedTextSource source)
    {
        public LocalizedString ToLocalizedString()
        {
            return LocalizedString.CreateFromTemplateFunc(() => source.Text);
        }

        public LocalizedString ToFormatLocalizedString(params object[] args)
        {
            return LocalizedString.CreateFromTemplateFunc(() => source.Text.FormatEx(args));
        }
    }
}