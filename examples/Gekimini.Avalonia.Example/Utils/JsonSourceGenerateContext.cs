using System.Text.Json.Serialization;
using Gekimini.Avalonia.Example.Modules.InternalTest.Models;

[JsonSerializable(typeof(InternalTestValueStoreData))]
[JsonSerializable(typeof(InternalTestRecentInfoData))]
[JsonSerializable(typeof(Dictionary<string, string>))]
public partial class JsonSourceGenerateContext : JsonSerializerContext
{
}