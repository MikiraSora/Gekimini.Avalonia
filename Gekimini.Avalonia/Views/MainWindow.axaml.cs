using Avalonia.Controls;

namespace Gekimini.Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override async void OnClosing(WindowClosingEventArgs e)
    {
        base.OnClosing(e);
        e.Cancel = true;

        await (App.Current as App)?.TryExit();
    }
}