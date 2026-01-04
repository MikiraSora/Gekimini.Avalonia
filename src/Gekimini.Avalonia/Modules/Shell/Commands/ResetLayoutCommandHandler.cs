using System.Linq;
using System.Threading.Tasks;
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
        if (!await DialogManager.ShowComfirmDialog("是否重置应用布局? 你的工具栏和文档栏的位置分布将会被清除!"))
            return;

        //make sure no document opened
        if (Shell.Documents.Any())
        {
            await DialogManager.ShowMessageDialog("请先关闭所有文档再进行操作", DialogMessageType.Error);
            return;
        }

        await Shell.ResetLayout();
        await DialogManager.ShowMessageDialog("重置应用布局成功");
    }
}