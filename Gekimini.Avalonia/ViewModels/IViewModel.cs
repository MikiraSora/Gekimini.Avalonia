using System.ComponentModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gekimini.Avalonia.ViewModels;

public interface IViewModel : INotifyPropertyChanged
{
    void OnViewAfterLoaded(Control view);
    void OnViewBeforeUnload(Control view);
}
