using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<ICommandHandler>]
public class SaveAllFilesCommandHandler : CommandHandlerBase<SaveAllFilesCommandDefinition>
{
    private readonly IServiceProvider _serviceProvider;

    public SaveAllFilesCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override async Task Run(Command command)
    {
        var shell = _serviceProvider.GetService<IShell>();
        var tasks = new List<Task<Tuple<IPersistedDocumentViewModel, bool>>>();

        foreach (var document in shell.Documents.OfType<IPersistedDocumentViewModel>().Where(x => !x.IsNew))
            await document.Save(document.FilePath);

        // TODO: display "Item(s) saved" in statusbar
    }
}