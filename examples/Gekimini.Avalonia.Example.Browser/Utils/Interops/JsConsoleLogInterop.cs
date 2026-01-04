using System.Runtime.InteropServices.JavaScript;

namespace Gekimini.Avalonia.Example.Browser.Utils.Interops;

public partial class JsConsoleLogInterop
{
    [JSImport("globalThis.console.log")]
    public static partial void Log([JSMarshalAs<JSType.String>] string message);
}