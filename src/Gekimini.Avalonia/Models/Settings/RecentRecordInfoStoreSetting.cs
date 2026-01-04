using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Framework.RecentFiles;
using Gekimini.Avalonia.Utils;

namespace Gekimini.Avalonia.Models.Settings;

public partial class RecentRecordInfoStoreSetting : ObservableObject
{
    public static JsonTypeInfo<RecentRecordInfoStoreSetting> JsonTypeInfo =>
        JsonSourceGenerateContext.Default.RecentRecordInfoStoreSetting;

    [ObservableProperty]
    public partial int RecordMaxCount { get; set; } = 10;

    [ObservableProperty]
    public partial List<RecentRecordInfo> RecentRecordInfoList { get; set; } = new();
}