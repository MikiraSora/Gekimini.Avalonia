using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Injectio.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<ICommandHandler>]
public class SwitchToDocumentListCommandHandler : CommandListHandlerBase<SwitchToDocumentCommandListDefinition>
{
    private readonly IServiceProvider _serviceProvider;

    public SwitchToDocumentListCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override void Populate(Command command, List<Command> commands)
    {
        var _shell = _serviceProvider.GetService<IShell>();

        for (var i = 0; i < _shell.Documents.Count(); i++)
        {
            var document = _shell.Documents.ElementAtOrDefault(i);
            commands.Add(new Command(command.CommandDefinition)
            {
                Checked = _shell.ActiveDocument == document,
                Text = string.Format("_{0} {1}", i + 1, document.Title.Text /*todo document.DisplayName*/).ToLocalizedStringByRawText(),
                Tag = document
            });
        }
    }

    public override Task Run(Command command)
    {
        var _shell = _serviceProvider.GetService<IShell>();

        return _shell.OpenDocumentAsync((IDocumentViewModel) command.Tag);
    }
}