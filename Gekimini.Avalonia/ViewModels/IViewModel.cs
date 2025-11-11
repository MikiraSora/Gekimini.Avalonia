using System;
using System.ComponentModel;
using Avalonia.Controls;

namespace Gekimini.Avalonia.ViewModels;

public interface IViewModel : INotifyPropertyChanged
{
    void OnViewAfterLoaded(Control view);
    void OnViewBeforeUnload(Control view);

    event Action<Control> ViewAfterLoaded;
    event Action<Control> ViewBeforeUnload;
}