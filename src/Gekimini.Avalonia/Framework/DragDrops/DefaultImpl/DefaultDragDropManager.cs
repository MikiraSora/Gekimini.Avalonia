using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Input;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.DragDrops.DefaultImpl;

[RegisterSingleton<IDragDropManager>]
public class DefaultDragDropManager : IDragDropManager
{
    private readonly Dictionary<string, object> stateStoreMap = new();

    public static DataFormat<string> DragDropDataFormat { get; } =
        DataFormat.CreateStringApplicationFormat("GekiminiDefaultDragDropManager");

    public async Task StartDragDropEvent(PointerEventArgs e, object state, DragDropEffects move)
    {
        var dataTransfer = new DataTransfer();
        var dataTransferItem = new DataTransferItem();
        var guid = Guid.NewGuid().ToString();
        dataTransferItem.Set(DragDropDataFormat, guid);
        dataTransfer.Add(dataTransferItem);

        stateStoreMap[guid] = state;

        await DragDrop.DoDragDropAsync(e, dataTransfer, move);
    }

    public bool TryGetDragData(DragEventArgs e, out object state)
    {
        state = default;
        if (e.DataTransfer.TryGetValue(DragDropDataFormat) is not { } token)
            return false;

        return stateStoreMap.TryGetValue(token, out state);
    }

    public void EndDragDropEvent(DragEventArgs e)
    {
        if (e.DataTransfer.TryGetValue(DragDropDataFormat) is not { } token)
            return;

        stateStoreMap.Remove(token);
    }
}