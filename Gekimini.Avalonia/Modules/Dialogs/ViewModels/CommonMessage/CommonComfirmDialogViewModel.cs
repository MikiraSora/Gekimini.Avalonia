using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gekimini.Avalonia.Framework.Dialogs;
using Gekimini.Avalonia.Utils;

namespace Gekimini.Avalonia.Modules.Dialogs.ViewModels.CommonMessage;

public partial class CommonComfirmDialogViewModel : DialogViewModelBase
{
    [ObservableProperty]
    private bool comfirmResult;

    [ObservableProperty]
    private string content;

    [ObservableProperty]
    private string noButtonContent;

    [ObservableProperty]
    private string yesButtonContent;

    public CommonComfirmDialogViewModel()
    {
        DesignModeHelper.CheckOnlyForDesignMode();
    }

    public CommonComfirmDialogViewModel(string content, string yesButtonContent, string noButtonContent)
    {
        this.content = content;
        this.yesButtonContent = yesButtonContent;
        this.noButtonContent = noButtonContent;
    }

    public override string DialogIdentifier => nameof(CommonComfirmDialogViewModel);

    public override string Title => "Comfirm";

    [RelayCommand]
    private void Yes()
    {
        ComfirmResult = true;
        CloseDialog();
    }

    [RelayCommand]
    private void No()
    {
        ComfirmResult = false;
        CloseDialog();
    }
}