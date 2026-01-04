using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Framework.UndoRedo;
using Gekimini.Avalonia.Modules.Shell.Commands;
using Gekimini.Avalonia.Modules.UndoRedo.Commands;
using Gekimini.Avalonia.ViewModels;
using Gekimini.Avalonia.Views;
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

    public override void OnViewAfterLoaded(IView view)
    {
        base.OnViewAfterLoaded(view);

        UndoRedoManager.PropertyChanged += UndoRedoManagerOnPropertyChanged;
    }

    public override void OnViewBeforeUnload(IView view)
    {
        base.OnViewBeforeUnload(view);

        UndoRedoManager.PropertyChanged -= UndoRedoManagerOnPropertyChanged;
    }

    [GenerateCommandUpdateDispatcher<SaveFileAsCommandDefinition>]
    protected void UpdateSaveFileAsCommand(Command command)
    {
        command.Enabled = this is IPersistedDocumentViewModel;
    }

    [GenerateCommandRunDispatcher<SaveFileAsCommandDefinition>]
    protected Task RunSaveFileAsCommand(Command command)
    {
        if (this is IPersistedDocumentViewModel persistedDocumentViewModel)
            return persistedDocumentViewModel.SaveAs();
        return Task.CompletedTask;
    }

    [GenerateCommandUpdateDispatcher<SaveFileCommandDefinition>]
    protected void UpdateSaveFileCommand(Command command)
    {
        command.Enabled = this is IPersistedDocumentViewModel;
    }

    [GenerateCommandRunDispatcher<SaveFileCommandDefinition>]
    protected Task RunSaveFileCommand(Command command)
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

    private static void UndoRedoManagerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(IUndoRedoManager.CanRedo):
            case nameof(IUndoRedoManager.CanUndo):
                CommandManager.InvalidateRequerySuggested(); 
                break;
        }
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