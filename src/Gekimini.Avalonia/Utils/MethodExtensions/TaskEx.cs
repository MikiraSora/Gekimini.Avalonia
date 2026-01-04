using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Gekimini.Avalonia;

internal static class TaskEx
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NoWait(this Task task)
    {
        //ignore.
    }
}