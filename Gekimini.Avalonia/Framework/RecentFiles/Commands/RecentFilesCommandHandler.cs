using System.Linq;
using System.Threading.Tasks;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.RecentFiles.Commands
{
	[RegisterSingleton<ICommandHandler>]
	public class RecentFilesCommandHandler : CommandHandlerBase<RecentFilesCommandDefinition>
	{
		private readonly IEditorRecentFilesManager recentOpenedManager;

		public RecentFilesCommandHandler(IEditorRecentFilesManager recentOpenedManager)
		{
			this.recentOpenedManager = recentOpenedManager;
		}

		public override void Update(Command command)
		{
			command.Enabled = recentOpenedManager.RecentRecordInfos.Any();
		}

		public override Task Run(Command command)
		{
			return Task.CompletedTask;
		}
	}
}
