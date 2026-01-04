using System;
using System.Threading.Tasks;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Modules.Shell;
using Injectio.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Modules.UndoRedo.Commands
{
    [RegisterSingleton<ICommandHandler>]
    public class ViewHistoryCommandHandler : CommandHandlerBase<ViewHistoryCommandDefinition>
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewHistoryCommandHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override Task Run(Command command)
        {
            _serviceProvider.GetService<IShell>().ShowTool<IHistoryTool>();
            return Task.CompletedTask;
        }
    }
}