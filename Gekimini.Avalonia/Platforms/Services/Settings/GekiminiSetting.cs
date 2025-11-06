using System.Text.Json.Serialization.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Utils;

namespace Gekimini.Avalonia.Platforms.Services.Settings;

public partial class GekiminiSetting : ObservableObject
{
    public static JsonTypeInfo<GekiminiSetting> JsonTypeInfo => JsonSourceGenerateContext.Default.GekiminiSetting;
    
    [ObservableProperty]
    public partial string ShellLayout { get; set; }
}