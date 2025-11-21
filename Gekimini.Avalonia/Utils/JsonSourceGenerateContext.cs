using System.Text.Json.Serialization;
using Gekimini.Avalonia.Framework.RecentFiles.DefaultImpl;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Modules.InternalTest.Models;
using Gekimini.Avalonia.Modules.Toolbox.Models;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Gekimini.Avalonia.Utils.MethodExtensions;
using RecentRecordDataStoreSetting = Gekimini.Avalonia.Models.Settings.RecentRecordDataStoreSetting;
using RecentRecordInfoStoreSetting = Gekimini.Avalonia.Models.Settings.RecentRecordInfoStoreSetting;

namespace Gekimini.Avalonia.Utils;

[JsonSerializable(typeof(GekiminiSetting))]
[JsonSerializable(typeof(WindowPositionSizeSetting))]
[JsonSerializable(typeof(RecentRecordInfoStoreSetting))]
[JsonSerializable(typeof(RecentRecordDataStoreSetting))]
[JsonSerializable(typeof(InternalTestValueStoreData))]
[JsonSerializable(typeof(InternalTestRecentInfoData))]
[JsonSerializable(typeof(IEditorRecentFilesManagerEx.StringStore))]
public partial class JsonSourceGenerateContext: JsonSerializerContext
{
    
}