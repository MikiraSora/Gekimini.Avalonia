using System;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Gekimini.Avalonia.Platforms.Services.Settings;

namespace Gekimini.Avalonia.Utils.MethodExtensions;

public static class ISettingManagerEx
{
    public static void LoadAndSave<T>(this ISettingManager manager, JsonTypeInfo<T> jsonTypeInfo,
        Action<T> action) where T : new()
    {
        var setting = manager.GetSetting(jsonTypeInfo);
        action.Invoke(setting);
        manager.SaveSetting(setting, jsonTypeInfo);
    }
}