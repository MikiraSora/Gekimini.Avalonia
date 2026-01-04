using System.Collections.Generic;
using System.ComponentModel;

namespace Gekimini.Avalonia.Framework.Themes;

public interface IThemeManager : INotifyPropertyChanged
{
    IEnumerable<IColorTheme> AvaliableColorThemes { get; }
    IColorTheme CurrentColorTheme { get; set; }

    IEnumerable<IControlTheme> AvaliableControlThemes { get; }
    IControlTheme CurrentControlTheme { get; set; }

    void Initalize();
}