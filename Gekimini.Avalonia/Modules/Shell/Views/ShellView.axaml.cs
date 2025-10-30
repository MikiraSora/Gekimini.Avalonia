using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Interactivity;
using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Views;

namespace Gekimini.Avalonia.Modules.Shell.Views;

public partial class ShellView : ViewBase, IShellView
{
    private readonly ViewLocator viewLocator;

    public ShellView(ViewLocator viewLocator,IServiceProvider serviceProvider)
    {
        this.viewLocator = viewLocator;
        InitializeComponent();
    }

    public void LoadLayout(Stream stream, Action<ITool> addToolCallback, Action<IDocument> addDocumentCallback,
        Dictionary<string, ILayoutItem> itemsState)
    {
        //todo
        throw new NotImplementedException();
    }

    public void SaveLayout(Stream stream)
    {
        //todo
        throw new NotImplementedException();
    }

    public void UpdateFloatingWindows(bool isFloatingWindows)
    {
        //todo show other floating dock windows
    }

    private void Control_OnLoaded(object sender, RoutedEventArgs e)
    {
        
    }
}