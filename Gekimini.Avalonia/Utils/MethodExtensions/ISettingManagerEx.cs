using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;
using Gekimini.Avalonia.Platforms.Services.Settings;

namespace Gekimini.Avalonia.Utils.MethodExtensions;

public static class ISettingManagerEx
{
    public static void LoadAndSave<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(
        this ISettingManager manager, JsonTypeInfo<T> jsonTypeInfo,
        Action<T> action) where T : new()
    {
        var setting = manager.GetSetting(jsonTypeInfo);
        action.Invoke(setting);
        manager.SaveSetting(setting, jsonTypeInfo);
    }
}