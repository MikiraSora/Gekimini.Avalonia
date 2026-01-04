using System.Text.Json.Serialization.Metadata;
using Gekimini.Avalonia.Utils;

namespace Gekimini.Avalonia.Example.Modules.InternalTest.Models;

public class InternalTestRecentInfoData
{
    public static JsonTypeInfo<InternalTestRecentInfoData> JsonTypeInfo =>
        JsonSourceGenerateContext.Default.InternalTestRecentInfoData;

    public string Bookmark { get; set; }
}