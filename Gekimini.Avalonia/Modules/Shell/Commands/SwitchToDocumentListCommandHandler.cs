using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Modules.Shell;
using Injectio.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Gemini.Modules.Shell.Commands;

[RegisterSingleton<ICommandHandler>]
public class SwitchToDocumentListCommandHandler : ICommandListHandler<SwitchToDocumentCommandListDefinition>
{
    private readonly IServiceProvider _serviceProvider;

    public SwitchToDocumentListCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Populate(Command command, List<Command> commands)
    {
        var _shell = _serviceProvider.GetService<IShell>();

        for (var i = 0; i < _shell.Documents.Count(); i++)
        {
            var document = _shell.Documents.ElementAtOrDefault(i);
            commands.Add(new Command(command.CommandDefinition)
            {
                Checked = _shell.ActiveDocument == document,
                Text = string.Format("_{0} {1}", i + 1, document.Title /*todo document.DisplayName*/),
                Tag = document
            });
        }
    }

    public void Update(Command command)
    {
        
    }

    public Task Run(Command command)
    {
        var _shell = _serviceProvider.GetService<IShell>();

        return _shell.OpenDocumentAsync((IDocumentViewModel) command.Tag);
    }
}