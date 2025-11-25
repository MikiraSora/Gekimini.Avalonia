namespace Gekimini.Avalonia.Framework.Languages;

public class RawTextString(string rawText) : LocalizedString(false)
{
    protected override string GetText()
    {
        return rawText;
    }
}