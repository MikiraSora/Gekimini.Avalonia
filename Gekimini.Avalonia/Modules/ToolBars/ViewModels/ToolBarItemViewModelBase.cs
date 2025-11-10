using Gekimini.Avalonia.ViewModels;

namespace Gekimini.Avalonia.Modules.ToolBars.ViewModels
{
	public class ToolBarItemViewModelBase : ViewModelBase
	{
		public virtual string Name
		{
			get { return "-"; }
		}
	}
}