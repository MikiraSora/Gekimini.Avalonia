using System.Runtime.InteropServices.JavaScript;

namespace Gekimini.Avalonia.Browser.Platforms.Services.Settings;

public partial class LocalStorageInterop
{
    [JSImport("globalThis.LocalStorageInterop.load")]
    public static partial string Load([JSMarshalAs<JSType.String>] string key);

    [JSImport("globalThis.LocalStorageInterop.save")]
    public static partial void Save([JSMarshalAs<JSType.String>] string key, [JSMarshalAs<JSType.String>] string value);
}