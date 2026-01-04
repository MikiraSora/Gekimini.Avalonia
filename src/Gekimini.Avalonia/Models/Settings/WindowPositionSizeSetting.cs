using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Utils;

namespace Gekimini.Avalonia.Models.Settings;

public partial class WindowPositionSizeSetting : ObservableObject
{
    public static JsonTypeInfo<WindowPositionSizeSetting> JsonTypeInfo => JsonSourceGenerateContext.Default.WindowPositionSizeSetting;

    [ObservableProperty]
    public partial Dictionary<string, ControlPositionSize> WindowPositionSizeMap { get; set; } = new();
}