namespace Gekimini.Avalonia.Utils.MethodExtensions;

public static class StringEx
{
    public static string FormatEx(this string str, params object[] args)
    {
        return string.IsNullOrWhiteSpace(str) ? str : string.Format(str, args);
    }
}