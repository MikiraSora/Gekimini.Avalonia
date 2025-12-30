using System;
using System.ComponentModel;
using Dock.Model.Mvvm.Controls;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.ViewModels;
using Gekimini.Avalonia.Views;

namespace Gekimini.Avalonia.Modules.Shell.ViewModels;

public class DocumentContainerViewModel : Document, IViewModel
{
    private Action unregisterContextPropertyChangedEventAction;

    public void OnViewAfterLoaded(IView view)
    {
        ViewAfterLoaded?.Invoke(view);
    }

    public void OnViewBeforeUnload(IView view)
    {
        ViewBeforeUnload?.Invoke(view);
    }

    public event Action<IView> ViewAfterLoaded;
    public event Action<IView> ViewBeforeUnload;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        //Context is changed.
        if (e.PropertyName == nameof(Context))
        {
            //unregister prev prop bindings
            unregisterContextPropertyChangedEventAction?.Invoke();
            unregisterContextPropertyChangedEventAction = default;

            if (Context is IDocumentViewModel documentViewModel)
            {
                //register all prop bindings
                documentViewModel.PropertyChanged += DocumentViewModelOnPropertyChanged;
                unregisterContextPropertyChangedEventAction = () =>
                {
                    documentViewModel.PropertyChanged -= DocumentViewModelOnPropertyChanged;
                };

                Title = documentViewModel.Title.Text;
            }
        }
    }

    private void DocumentViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        //todo 感觉需要更优化的方式，可能得重新实现了
        if (sender is not IDocumentViewModel documentViewModel)
            return;

        switch (e.PropertyName)
        {
            case nameof(IDocumentViewModel.Title):
                Title = documentViewModel.Title.Text;
                break;
        }

        if (sender is not IPersistedDocumentViewModel persistedDocumentViewModel)
            return;

        switch (e.PropertyName)
        {
            case nameof(IPersistedDocumentViewModel.IsDirty):
                IsModified = persistedDocumentViewModel.IsDirty;
                break;
        }
    }
}