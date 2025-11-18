using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Gekimini.Avalonia.Models;

namespace Gekimini.Avalonia.UI.ValueConverters;

public class ControlSizeConverter : IValueConverter
{
    public static ControlSizeConverter Instance { get; } = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not ControlSize controlSize)
            return null;

        if (controlSize.SizeValueType == SizeValueType.Fixed)
            return controlSize.Value;

        var percent = Math.Max(0, Math.Min(controlSize.Value, 1));
        if ((Application.Current as App)?.TopLevel is not { } topLevel)
            return null;

        var total = parameter?.ToString()?.ToLower() == "height"
            ? topLevel.Bounds.Height
            : topLevel.Bounds.Width;

        return total * percent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double d)
            return new ControlSize(d);
        return new ControlSize(0);
    }
}