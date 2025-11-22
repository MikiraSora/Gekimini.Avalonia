using CommunityToolkit.Mvvm.Input;
using Gekimini.Avalonia.Modules.Dialogs.ViewModels;

namespace Gekimini.Avalonia.Modules.Documents.ViewModels;

public partial class SaveDirtyDocumentDialogViewModel : DialogViewModelBase
{
    public enum DialogResult
    {
        Yes,
        No,
        Cancel
    }

    public override string DialogIdentifier => nameof(SaveDirtyDocumentDialogViewModel);
    public override string Title => "提醒";

    public DialogResult Result { get; set; }

    public string DocumentName { get; set; }

    [RelayCommand]
    private void Close(DialogResult result)
    {
        Result = result;
        CloseDialog();
    }
}