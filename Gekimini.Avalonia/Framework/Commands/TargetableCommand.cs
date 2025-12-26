using System;
using System.Windows.Input;
using Avalonia.Threading;

namespace Gekimini.Avalonia.Framework.Commands;

public class TargetableCommand : ICommand
{
    private readonly Command _command;
    private readonly ICommandRouter _commandRouter;

    public TargetableCommand(ICommandRouter commandRouter, Command command)
    {
        _command = command;
        _commandRouter = commandRouter;
    }

    public bool CanExecute(object parameter)
    {
        var commandHandler = _commandRouter.GetCommandHandler(_command.CommandDefinition);
        if (commandHandler == null)
            return false;

        commandHandler.Update(_command);

        return _command.Enabled;
    }

    public async void Execute(object parameter)
    {
        var commandHandler = _commandRouter.GetCommandHandler(_command.CommandDefinition);
        if (commandHandler == null)
            return;
        
        await Dispatcher.UIThread.InvokeAsync(()=>commandHandler.Run(_command));
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}