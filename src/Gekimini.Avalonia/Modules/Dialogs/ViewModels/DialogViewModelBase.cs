using AvaloniaDialogs.Views;
using Gekimini.Avalonia.ViewModels;

namespace Gekimini.Avalonia.Modules.Dialogs.ViewModels;

public abstract class DialogViewModelBase : ViewModelBase
{
    private BaseDialog dialogView;
    public abstract string DialogIdentifier { get; }
    public abstract string Title { get; }

    public void CloseDialog()
    {
        dialogView?.Close();
    }

    internal void SetDialogView(BaseDialog bd)
    {
        dialogView = bd;
    }
}