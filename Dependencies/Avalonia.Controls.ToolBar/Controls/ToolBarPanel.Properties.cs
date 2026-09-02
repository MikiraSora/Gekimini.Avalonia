using Avalonia;
using Avalonia.Controls;

namespace Avalonia.Controls.ToolBar.Controls;

public partial class ToolBarPanel
{
    #region ItemIsOwnContainer Property

    private static readonly AttachedProperty<bool> ItemIsOwnContainerProperty =
        AvaloniaProperty.RegisterAttached<ToolBar, Control, bool>("ItemIsOwnContainer");

    #endregion
}