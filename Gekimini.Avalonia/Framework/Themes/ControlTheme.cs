namespace Gekimini.Avalonia.Framework.Themes;

public interface IControlTheme
{
    string Name { get; }

    void ApplyControlTheme();
    void RevertControlTheme();
}