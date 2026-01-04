using System.Runtime.InteropServices.JavaScript;

namespace Gekimini.Avalonia.Example.Browser.Utils.Interops;

public partial class JsApplicationInterop
{
    [JSImport("globalThis.JsApplication.exit")]
    public static partial void Exit();
}