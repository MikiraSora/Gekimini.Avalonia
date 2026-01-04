using System;
using System.ComponentModel;
using Avalonia.Controls;
using Gekimini.Avalonia.Views;

namespace Gekimini.Avalonia.ViewModels;

public interface IViewModel : INotifyPropertyChanged
{
    void OnViewAfterLoaded(IView view);
    void OnViewBeforeUnload(IView view);

    event Action<IView> ViewAfterLoaded;
    event Action<IView> ViewBeforeUnload;
}