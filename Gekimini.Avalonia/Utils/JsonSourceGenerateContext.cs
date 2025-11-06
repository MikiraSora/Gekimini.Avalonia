using System.Text.Json.Serialization;
using Gekimini.Avalonia.Platforms.Services.Settings;

namespace Gekimini.Avalonia.Utils;

[JsonSerializable(typeof(GekiminiSetting))]
public partial class JsonSourceGenerateContext: JsonSerializerContext
{
    
}