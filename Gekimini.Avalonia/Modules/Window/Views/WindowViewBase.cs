using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Gekimini.Avalonia.Views;
using Iciclecreek.Avalonia.WindowManager;

namespace Gekimini.Avalonia.Modules.Window.Views;

public class WindowViewBase : ManagedWindow, IView
{
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        //there is a bug in PART_SystemMenu, just hide it
        if (e.NameScope.Find<Control>("PART_SystemMenu") is { } systemMenu)
            systemMenu.IsVisible = false;
    }
}