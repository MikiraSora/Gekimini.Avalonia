using Gekimini.Avalonia.Attributes;

namespace Gekimini.Avalonia.Framework.Languages;

public partial class ResourceLocalizedString(string resourceKey) : LocalizedString(true)
{
    [GetServiceLazy]
    private partial ILanguageManager LanguageManager { get; }

    protected override string GetText()
    {
        return LanguageManager?.GetTranslatedText(resourceKey) ?? $"<i18n {resourceKey}>";
    }

    public override string ToString()
    {
        return $"<resourceKey:{resourceKey} Text:{Text}>";
    }
}