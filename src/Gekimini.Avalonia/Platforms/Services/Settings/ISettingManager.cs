using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace Gekimini.Avalonia.Platforms.Services.Settings;

public interface ISettingManager
{
    void SaveSetting<T>(T obj, JsonTypeInfo<T> jsonTypeInfo);
    T GetSetting<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]T>(JsonTypeInfo<T> jsonTypeInfo) where T : new();
}