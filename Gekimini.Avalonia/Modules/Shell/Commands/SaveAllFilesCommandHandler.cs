using System.Linq;
using System.Threading.Tasks;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Modules.StatusBar;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<ICommandHandler>]
public partial class SaveAllFilesCommandHandler : CommandHandlerBase<SaveAllFilesCommandDefinition>
{
    [GetServiceLazy]
    private partial IShell Shell { get; }

    [GetServiceLazy]
    private partial IStatusBar StatusBar { get; }

    public override void Update(Command command)
    {
        command.Enabled = Shell?.Documents.OfType<IPersistedDocumentViewModel>().Any() ?? false;
    }

    public override async Task Run(Command command)
    {
        var statusBarItem = StatusBar?.GetApplicationGlobalStatusBarItem();

        //force save all documents.
        foreach (var document in Shell.Documents.OfType<IPersistedDocumentViewModel>() /*.Where(x => x.IsDirty)*/)
            if (!await document.Save())
            {
                //one document save failed, just stop remains.
                statusBarItem?.Message = "A document save failed, cancel remains.";
                return;
            }

        statusBarItem?.Message = "All documents are saved.";
    }
}