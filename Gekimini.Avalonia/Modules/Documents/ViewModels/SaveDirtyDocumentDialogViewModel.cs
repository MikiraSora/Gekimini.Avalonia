using CommunityToolkit.Mvvm.Input;
using Gekimini.Avalonia.Modules.Dialogs.ViewModels;
using Gekimini.Avalonia.Modules.Documents.Models;

namespace Gekimini.Avalonia.Modules.Documents.ViewModels;

public partial class SaveDirtyDocumentDialogViewModel : DialogViewModelBase
{
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