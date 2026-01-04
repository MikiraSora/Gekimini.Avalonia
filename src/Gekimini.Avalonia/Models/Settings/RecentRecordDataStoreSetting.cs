using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Utils;

namespace Gekimini.Avalonia.Models.Settings;

public partial class RecentRecordDataStoreSetting : ObservableObject
{
    public static JsonTypeInfo<RecentRecordDataStoreSetting> JsonTypeInfo =>
        JsonSourceGenerateContext.Default.RecentRecordDataStoreSetting;

    [ObservableProperty]
    public partial Dictionary<string, byte[]> RecordInfoDataMap { get; set; } = new();
}