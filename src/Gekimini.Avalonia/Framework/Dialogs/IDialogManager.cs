using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Gekimini.Avalonia.Modules.Dialogs.ViewModels;

namespace Gekimini.Avalonia.Framework.Dialogs;

public interface IDialogManager
{
    Task<T> ShowDialog<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : DialogViewModelBase;

    Task ShowDialog(DialogViewModelBase dialogViewModel);
    Task ShowMessageDialog(string content, DialogMessageType messageType = DialogMessageType.Info);

    Task<bool> ShowComfirmDialog(string content, string title = null, string yesButtonContent = null,
        string noButtonContent = null);
}