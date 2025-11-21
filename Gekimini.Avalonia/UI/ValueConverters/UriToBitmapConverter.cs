using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Gekimini.Avalonia.UI.ValueConverters;

public class UriToBitmapConverter : IValueConverter
{
    public static UriToBitmapConverter Instance { get; } = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Uri s)
            return default;

        try
        {
            using var stream = AssetLoader.Open(s); 
            return new Bitmap(stream);
        }
        catch
        {
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}