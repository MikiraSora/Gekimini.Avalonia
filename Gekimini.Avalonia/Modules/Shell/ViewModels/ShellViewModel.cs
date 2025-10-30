using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Core.Events;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.Services;
using Gekimini.Avalonia.Modules.InternalTest.ViewModels;
using Gekimini.Avalonia.Modules.Shell.Models;
using Gekimini.Avalonia.Modules.Shell.Views;
using Gekimini.Avalonia.ViewModels;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.ViewModels;

[RegisterSingleton<IShell>]
public partial class ShellViewModel : ViewModelBase, IShell
{
    private readonly IEnumerable<IModule> _modules;

    private readonly List<IDocumentViewModel> addedDocuments = new();
    private readonly List<IToolViewModel> addTools = new();
    private readonly IServiceProvider serviceProvider;

    private IShellView _shellView;

    [ObservableProperty]
    private ShellDockFactory factory;

    [ObservableProperty]
    private IRootDock layout;

    [ObservableProperty]
    private bool showFloatingWindowsInTaskbar;

    public ShellViewModel(IServiceProvider serviceProvider, IEnumerable<IModule> modules)
    {
        this.serviceProvider = serviceProvider;
        _modules = modules;

        Factory = this.serviceProvider.Resolve<ShellDockFactory>();

        var l = factory.CreateLayout();
        factory.InitLayout(l);
        Layout = l;

        Factory.DockableAdded += FactoryOnDockableAdded;
        Factory.DockableRemoved += FactoryOnDockableRemoved;
    }

    public IEnumerable<IDocumentViewModel> Documents => addedDocuments;

    public IEnumerable<IToolViewModel> Tools => addTools;

    public void ShowTool<TTool>()
        where TTool : IToolViewModel
    {
        ShowTool(serviceProvider.Resolve<TTool>());
    }

    public void ShowTool(IToolViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        Factory.AddTool(model);
    }

    public void HideTool<TTool>() where TTool : IToolViewModel
    {
        HideTool(Tools.OfType<TTool>().FirstOrDefault());
    }

    public void HideTool(IToolViewModel model)
    {
        if (model is null)
            return;
        Factory.RemoveTool(model);
    }

    public Task OpenDocumentAsync(IDocumentViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        Factory.AddDocument(model);
        return Task.CompletedTask;
    }

    public Task CloseDocumentAsync(IDocumentViewModel model)
    {
        if (model is null)
            return Task.CompletedTask;

        Factory.RemoveDocument(model);

        //todo return DeactivateItemAsync(document, true, CancellationToken.None);
        return Task.CompletedTask;
    }

    public void Close()
    {
    }

    private void FactoryOnDockableRemoved(object sender, DockableRemovedEventArgs e)
    {
        switch (e.Dockable)
        {
            case IDocumentViewModel document:
                addedDocuments.Remove(document);
                break;
            case IToolViewModel tool:
                addTools.Remove(tool);
                break;
        }
    }

    private void FactoryOnDockableAdded(object sender, DockableAddedEventArgs e)
    {
        switch (e.Dockable)
        {
            case IDocumentViewModel document:
                addedDocuments.Add(document);
                break;
            case IToolViewModel tool:
                addTools.Add(tool);
                break;
        }
    }

    public override void OnViewAfterLoaded(Control view)
    {
        base.OnViewAfterLoaded(view);

        foreach (var module in _modules)
        foreach (var globalResourceDictionary in module.GlobalResourceDictionaries)
            Application.Current.Resources.MergedDictionaries.Add(globalResourceDictionary);

        foreach (var module in _modules)
            module.PreInitialize();
        foreach (var module in _modules)
            module.Initialize();

        _shellView = view as IShellView;

        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            foreach (var module in _modules)
                await module.PostInitializeAsync();
        });
    }

    [RelayCommand]
    private async void AddDocument()
    {
        var document = serviceProvider.Resolve<InternalTestDocumentViewModel>();
        document.Text = Guid.NewGuid().ToString();
        await OpenDocumentAsync(document);
    }

    [RelayCommand]
    private async void AddTool(string dockEnum)
    {
        var tool = serviceProvider.Resolve<InternalTestToolViewModel>();
        tool.Dock = Enum.Parse<DockMode>(dockEnum);
        ShowTool(tool);
    }

    [RelayCommand]
    private async void RemoveLastCreatedTool()
    {
        HideTool(Tools.LastOrDefault());
    }

    [RelayCommand]
    private async void RemoveLastCreatedDocument()
    {
        CloseDocumentAsync(Documents.LastOrDefault());
    }

    partial void OnShowFloatingWindowsInTaskbarChanged(bool value)
    {
        _shellView?.UpdateFloatingWindows(ShowFloatingWindowsInTaskbar);
    }
}