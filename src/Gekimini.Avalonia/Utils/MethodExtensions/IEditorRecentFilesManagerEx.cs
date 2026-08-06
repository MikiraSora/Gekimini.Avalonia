using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Gekimini.Avalonia.Framework.RecentFiles;

namespace Gekimini.Avalonia.Utils.MethodExtensions;

public static class IEditorRecentFilesManagerEx
{
    public static string ReadDataAsString(this IEditorRecentFilesManager editorRecentFilesManager,
        RecentRecordInfo recordInfo)
    {
        var data = editorRecentFilesManager.ReadData(recordInfo);
        var json = JsonSerializer.Deserialize(data, StringStore.JsonTypeInfo);
        return json.Value;
    }

    public static void WriteDataAsString(this IEditorRecentFilesManager editorRecentFilesManager,
        RecentRecordInfo recordInfo, string str)
    {
        editorRecentFilesManager.WriteData(recordInfo, EncodeDataAsString(str));
    }

    public static byte[] EncodeDataAsString(string str) =>
        JsonSerializer.SerializeToUtf8Bytes(new StringStore {Value = str}, StringStore.JsonTypeInfo);

    public class StringStore
    {
        public static JsonTypeInfo<StringStore> JsonTypeInfo => JsonSourceGenerateContext.Default.StringStore;

        public string Value { get; set; }
    }
}
