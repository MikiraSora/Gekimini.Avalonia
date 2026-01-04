using System.Threading.Tasks;
using Avalonia.Input;

namespace Gekimini.Avalonia.Framework.DragDrops;

public interface IDragDropManager
{
    Task StartDragDropEvent(PointerEventArgs e, object state, DragDropEffects move);
    bool TryGetDragData(DragEventArgs e, out object state);
    void EndDragDropEvent(DragEventArgs e);
}