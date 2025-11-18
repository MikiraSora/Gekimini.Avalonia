using Avalonia;
using AvaloniaDialogs.Views;
using Gekimini.Avalonia.Framework.Dialogs;
using Gekimini.Avalonia.Modules.Dialogs.ViewModels;
using Gekimini.Avalonia.Utils;

namespace Gekimini.Avalonia.Modules.Dialogs.Views;

public partial class TemplateDialogView : BaseDialog
{
    public static readonly StyledProperty<DialogViewModelBase> MainPageContentProperty =
        AvaloniaProperty.Register<TemplateDialogView, DialogViewModelBase>(nameof(MainPageContent));

    public TemplateDialogView()
    {
        DesignModeHelper.CheckOnlyForDesignMode();
    }

    public TemplateDialogView(DialogViewModelBase dialogViewModelBase)
    {
        MainPageContent = dialogViewModelBase;
        dialogViewModelBase?.SetDialogView(this);
        InitializeComponent();
        DataContext = this;
    }

    public DialogViewModelBase MainPageContent
    {
        get => GetValue(MainPageContentProperty);
        set => SetValue(MainPageContentProperty, value);
    }
}