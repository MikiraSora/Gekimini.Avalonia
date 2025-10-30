using CommunityToolkit.Mvvm.ComponentModel;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.Documents.UndoRedo;

namespace Gekimini.Avalonia.Modules.InternalTest.ViewModels;

public partial class InternalTestDocumentViewModel : DocumentViewModelBase
{
    public InternalTestDocumentViewModel(IUndoRedoManagerFactory undoRedoManagerFactory) : base(undoRedoManagerFactory)
    {
    }
    
    [ObservableProperty]
    private string text;
}