using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;

namespace Gekimini.Avalonia.Utils;

[JsonSourceGenerationOptions(
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals)]
[JsonSerializable(typeof(LayoutDockable))]
internal partial class LayoutJsonSourceGeneratorContext : JsonSerializerContext
{
    
}