using System.Linq;
using System.Threading.Tasks;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Dialogs;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<ICommandHandler>]
public partial class ResetLayoutCommandHandler : CommandHandlerBase<ResetLayoutCommandDefinition>
{
    [GetServiceLazy]
    private partial IShell Shell { get; }

    [GetServiceLazy]
    private partial IDialogManager DialogManager { get; }

    public override async Task Run(Command command)
    {
        if (!await DialogManager.ShowComfirmDialog(ProgramLanguages.AskResetLayout))
            return;

        //make sure no document opened
        if (Shell.Documents.Any())
        {
            await DialogManager.ShowMessageDialog(ProgramLanguages.ResetLayoutRequestUserCloseAllDocuments,
                DialogMessageType.Error);
            return;
        }

        await Shell.ResetLayout();
        await DialogManager.ShowMessageDialog(ProgramLanguages.ResetLayoutSuccessfully);
    }
}