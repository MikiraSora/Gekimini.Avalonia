using System;
using System.ComponentModel;
using Dock.Model.Mvvm.Controls;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.ViewModels;
using Gekimini.Avalonia.Views;

namespace Gekimini.Avalonia.Modules.Shell.ViewModels;

public class ToolContainerViewModel : Tool, IViewModel
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

            if (Context is IToolViewModel toolViewModel)
            {
                //register all prop bindings
                toolViewModel.PropertyChanged += ToolViewModelOnPropertyChanged;
                unregisterContextPropertyChangedEventAction = () =>
                {
                    toolViewModel.PropertyChanged -= ToolViewModelOnPropertyChanged;
                };

                Title = toolViewModel.Title.Text;
                Dock = toolViewModel.Dock;
            }
        }
    }

    private void ToolViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        //todo 感觉需要更优化的方式，可能得重新实现了
        if (sender is not IToolViewModel toolViewModel)
            return;

        switch (e.PropertyName)
        {
            case nameof(IToolViewModel.Title):
                Title = toolViewModel.Title.Text;
                break;
            case nameof(IToolViewModel.Dock):
                Dock = toolViewModel.Dock;
                break;
        }
    }
}