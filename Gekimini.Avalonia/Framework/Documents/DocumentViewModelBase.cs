using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Dock.Model.Mvvm.Controls;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.UndoRedo;
using Gekimini.Avalonia.Modules.Shell.Commands;
using Gekimini.Avalonia.Modules.UndoRedo.Commands;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Framework.Documents;

public abstract partial class DocumentViewModelBase : Document, IDocumentViewModel,
    ICommandHandler<RedoCommandDefinition>,
    ICommandHandler<UndoCommandDefinition>,
    ICommandHandler<SaveFileCommandDefinition>,
    ICommandHandler<SaveFileAsCommandDefinition>
{
    public DocumentViewModelBase()
    {
        Id = Guid.NewGuid().ToString();
        Title = GetType().Name;
    }

    [GetServiceLazy]
    private partial IUndoRedoManagerFactory UndoRedoManagerFactory { get; }
    
    [GetServiceLazy]
    private partial ILogger<DocumentViewModelBase> Logger { get; }

    public virtual void OnViewAfterLoaded(Control view)
    {
        ViewAfterLoaded?.Invoke(view);
    }

    public virtual void OnViewBeforeUnload(Control view)
    {
        ViewBeforeUnload?.Invoke(view);
    }

    public event Action<Control> ViewAfterLoaded;
    public event Action<Control> ViewBeforeUnload;
    public IUndoRedoManager UndoRedoManager => field ??= UndoRedoManagerFactory.Create();

    [GenerateCommandUpdateDispatcher<SaveFileAsCommandDefinition>]
    private void UpdateSaveFileAsCommand(Command command)
    {
        command.Enabled = this is IPersistedDocumentViewModel;
    }

    [GenerateCommandRunDispatcher<SaveFileAsCommandDefinition>]
    private Task RunSaveFileAsCommand(Command command)
    {
        if (this is IPersistedDocumentViewModel persistedDocumentViewModel)
            return persistedDocumentViewModel.SaveAs();
        return Task.CompletedTask;
    }

    [GenerateCommandUpdateDispatcher<SaveFileCommandDefinition>]
    private void UpdateSaveFileCommand(Command command)
    {
        command.Enabled = this is IPersistedDocumentViewModel;
    }

    [GenerateCommandRunDispatcher<SaveFileCommandDefinition>]
    private Task RunSaveFileCommand(Command command)
    {
        if (this is IPersistedDocumentViewModel persistedDocumentViewModel)
            return persistedDocumentViewModel.Save();
        return Task.CompletedTask;
    }

    [GenerateCommandUpdateDispatcher<UndoCommandDefinition>]
    protected void UpdateUndoCommand(Command command)
    {
        command.Enabled = UndoRedoManager.CanUndo;
        Logger.LogInformationEx($"Called. UndoCommand.Enabled = {command.Enabled}");
    }

    [GenerateCommandUpdateDispatcher<RedoCommandDefinition>]
    protected void UpdateRedoCommand(Command command)
    {
        command.Enabled = UndoRedoManager.CanRedo;
    }

    [GenerateCommandRunDispatcher<RedoCommandDefinition>]
    protected Task RunRedoCommand(Command command)
    {
        UndoRedoManager.Redo(1);
        return Task.CompletedTask;
    }

    [GenerateCommandRunDispatcher<UndoCommandDefinition>]
    protected Task RunUndoCommand(Command command)
    {
        UndoRedoManager.Undo(1);
        return Task.CompletedTask;
    }
}