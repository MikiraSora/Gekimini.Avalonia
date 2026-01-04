using Avalonia.Input;

namespace Gekimini.Avalonia.Modules.Toolbox;

public static class ToolboxDragDrop
{
    public static DataFormat<string> DragDropDataFormat { get; } =
        DataFormat.CreateStringApplicationFormat("GekiminiToolboxItem");
}