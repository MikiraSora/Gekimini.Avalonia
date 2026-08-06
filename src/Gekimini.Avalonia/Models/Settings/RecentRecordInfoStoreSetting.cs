using System;
using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Framework.RecentFiles;
using Gekimini.Avalonia.Utils;

namespace Gekimini.Avalonia.Models.Settings;

public partial class RecentRecordInfoStoreSetting : ObservableObject
{
    public const int CurrentVersion = 2;

    public static JsonTypeInfo<RecentRecordInfoStoreSetting> JsonTypeInfo =>
        JsonSourceGenerateContext.Default.RecentRecordInfoStoreSetting;

    [ObservableProperty]
    public partial int Version { get; set; }

    [ObservableProperty]
    public partial int RecordMaxCount { get; set; } = 10;

    [ObservableProperty]
    public partial List<RecentRecordInfo> RecentRecordInfoList { get; set; } = new();

    [ObservableProperty]
    public partial Dictionary<Guid, byte[]> RecordInfoDataMap { get; set; } = new();

    [ObservableProperty]
    public partial HashSet<Guid> InvalidRecordIds { get; set; } = new();
}
