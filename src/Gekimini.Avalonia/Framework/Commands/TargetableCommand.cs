using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gekimini.Avalonia.Framework.Commands;

public class TargetableCommand : ICommand
{
    private readonly Command _command;
    private readonly ICommandRouter _commandRouter;
    private bool _hasUpdateResult;
    private Task _pendingUpdateTask;

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

        if (_pendingUpdateTask is null)
            BeginUpdate(commandHandler);

        return _hasUpdateResult && _command.Enabled;
    }

    public async void Execute(object parameter)
    {
        var commandHandler = _commandRouter.GetCommandHandler(_command.CommandDefinition);
        if (commandHandler is null)
            return;

        if (_pendingUpdateTask is not null)
            await _pendingUpdateTask;
        else
            await commandHandler.Update(_command);

        _hasUpdateResult = true;
        if (_command.Enabled)
            await commandHandler.Run(_command);
    }

    private void BeginUpdate(CommandHandlerWrapper commandHandler)
    {
        var wasEnabled = _command.Enabled;
        var hadUpdateResult = _hasUpdateResult;
        var updateTask = commandHandler.Update(_command);

        if (updateTask.IsCompletedSuccessfully)
        {
            _hasUpdateResult = true;
            return;
        }

        _pendingUpdateTask = updateTask;
        ObserveUpdate(updateTask, hadUpdateResult, wasEnabled);
    }

    private async void ObserveUpdate(Task updateTask, bool hadUpdateResult, bool wasEnabled)
    {
        try
        {
            await updateTask;
        }
        catch (Exception e)
        {
            _command.Enabled = false;
            Trace.TraceError($"Failed to update command state for {_command.CommandDefinition.GetType().FullName}: {e}");
        }
        finally
        {
            _hasUpdateResult = true;
            if (ReferenceEquals(_pendingUpdateTask, updateTask))
                _pendingUpdateTask = null;

            if (!hadUpdateResult || wasEnabled != _command.Enabled)
                CommandManager.InvalidateRequerySuggested("commandStateUpdated");
        }
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}
