using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace Gekimini.Avalonia.Platforms.Services.Settings;

public interface ISettingManager
{
    Task Save<T>(T obj, JsonTypeInfo<T> jsonTypeInfo);
    Task<T> Load<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]T>(JsonTypeInfo<T> jsonTypeInfo) where T : new();
}