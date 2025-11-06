using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gekimini.Avalonia.Framework.Dialogs;
using Gekimini.Avalonia.Utils;

namespace Gekimini.Avalonia.Modules.Dialogs.ViewModels.CommonMessage;

public partial class CommonMessageDialogViewModel : DialogViewModelBase
{
    [ObservableProperty]
    private string content;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Title))]
    private DialogMessageType dialogMessageType;

    public CommonMessageDialogViewModel()
    {
        DesignModeHelper.CheckOnlyForDesignMode();
    }

    public CommonMessageDialogViewModel(DialogMessageType dialogMessageType, string content)
    {
        this.dialogMessageType = dialogMessageType;
        this.content = content;
    }

    public override string DialogIdentifier => nameof(CommonMessageDialogViewModel);

    public override string Title => DialogMessageType switch
    {
        DialogMessageType.Error => "Error",
        DialogMessageType.Info or _ => "Info"
    };

    [RelayCommand]
    private void Close()
    {
        CloseDialog();
    }
}