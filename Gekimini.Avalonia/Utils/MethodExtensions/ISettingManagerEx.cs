using System;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Gekimini.Avalonia.Platforms.Services.Settings;

namespace Gekimini.Avalonia.Utils.MethodExtensions;

public static class ISettingManagerEx
{
    public static async Task LoadAndSave<T>(this ISettingManager manager, JsonTypeInfo<T> jsonTypeInfo,
        Action<T> action) where T : new()
    {
        var setting = await manager.Load(jsonTypeInfo);
        action.Invoke(setting);
        await manager.Save(setting, jsonTypeInfo);
    }
}