using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Framework.UndoRedo;
using Gekimini.Avalonia.Modules.Shell.Commands;
using Gekimini.Avalonia.Modules.UndoRedo.Commands;
using Gekimini.Avalonia.ViewModels;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Framework.Documents;

public abstract partial class DocumentViewModelBase : ViewModelBase, IDocumentViewModel,
    ICommandHandler<RedoCommandDefinition>,
    ICommandHandler<UndoCommandDefinition>,
    ICommandHandler<SaveFileCommandDefinition>,
    ICommandHandler<SaveFileAsCommandDefinition>
{
    [ObservableProperty]
    private LocalizedString title;

    public DocumentViewModelBase()
    {
        Title = LocalizedString.CreateFromRawText(GetType().Name);
    }

    [GetServiceLazy]
    private partial IUndoRedoManagerFactory UndoRedoManagerFactory { get; }

    [GetServiceLazy]
    private partial ILogger<DocumentViewModelBase> Logger { get; }

    public virtual IEnumerable<Type> SupportCommandDefinitionTypes =>
    [
        typeof(UndoCommandDefinition),
        typeof(RedoCommandDefinition),
        typeof(SaveFileCommandDefinition),
        typeof(SaveFileAsCommandDefinition)
    ];

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