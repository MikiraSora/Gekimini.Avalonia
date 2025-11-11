using System.Collections.Generic;
using System.ComponentModel;

namespace Gekimini.Avalonia.Framework.Themes;

public interface IThemeManager : INotifyPropertyChanged
{
    IEnumerable<ColorTheme> AvaliableColorThemes { get; }
    ColorTheme CurrentColorTheme { get; set; }

    IEnumerable<ControlTheme> AvaliableControlThemes { get; }
    ControlTheme CurrentControlTheme { get; set; }

    void InitalizeThemes();
}