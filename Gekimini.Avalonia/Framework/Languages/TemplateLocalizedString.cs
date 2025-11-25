using System;

namespace Gekimini.Avalonia.Framework.Languages;

public class TemplateLocalizedString(Func<string> templateFunc) : LocalizedString(true)
{
    protected override string GetText()
    {
        return templateFunc?.Invoke();
    }
    
    public override string ToString()
    {
        return $"<templated Text:{Text}>";
    }
}