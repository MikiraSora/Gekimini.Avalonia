using System.Text.Json.Serialization;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Modules.Toolbox.Models;
using Gekimini.Avalonia.Platforms.Services.Settings;

namespace Gekimini.Avalonia.Utils;

[JsonSerializable(typeof(GekiminiSetting))]
[JsonSerializable(typeof(WindowPositionSizeSetting))]
public partial class JsonSourceGenerateContext: JsonSerializerContext
{
    
}