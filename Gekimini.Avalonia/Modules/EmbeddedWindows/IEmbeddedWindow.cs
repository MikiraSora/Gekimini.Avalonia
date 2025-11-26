using Gekimini.Avalonia.Modules.Window.ViewModels;

namespace Gekimini.Avalonia.Modules.EmbeddedWindows;

public interface IEmbeddedWindow
{
    void AddWindow(WindowViewModelBase window);
    void RemoveWindow(WindowViewModelBase window);
    void MakeFrontShow(WindowViewModelBase window);
}