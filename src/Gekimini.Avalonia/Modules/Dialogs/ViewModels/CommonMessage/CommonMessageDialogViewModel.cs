using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Dialogs;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Utils;
using Gekimini.Avalonia.Utils.MethodExtensions;

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
        DialogMessageType.Error => ProgramLanguages.Error,
        DialogMessageType.Info or _ => ProgramLanguages.Notify
    };

    [RelayCommand]
    private void Close()
    {
        CloseDialog();
    }
}