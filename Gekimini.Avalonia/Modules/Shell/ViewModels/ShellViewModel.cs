using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace Gekimini.Avalonia.Modules.Shell.ViewModels;

[RegisterSingleton<IShell>]
public partial class ShellViewModel : ViewModelBase, IShell
{
    private readonly IEnumerable<IModule> _modules;

    private readonly List<IDocumentViewModel> addedDocuments = new();
    private readonly List<IToolViewModel> addTools = new();
    private readonly IDockSerializer dockSerializer;
    private readonly RecyclableMemoryStreamManager memoryStreamManager;
    private readonly ILogger<ShellViewModel> logger;
    private readonly IServiceProvider serviceProvider;

    private IShellView _shellView;

    [ObservableProperty]
    private ShellDockFactory factory;

    [ObservableProperty]
    private IRootDock layout;

    private string prevDocumentId;

    [ObservableProperty]
    private bool showFloatingWindowsInTaskbar;

    public ShellViewModel(IServiceProvider serviceProvider, IDockSerializer dockSerializer,
        RecyclableMemoryStreamManager memoryStreamManager,
        IEnumerable<IModule> modules,
        ILogger<ShellViewModel> logger)
    {
        this.serviceProvider = serviceProvider;
        this.dockSerializer = dockSerializer;
        this.memoryStreamManager = memoryStreamManager;
        _modules = modules;
        this.logger = logger;

        Factory = this.serviceProvider.Resolve<ShellDockFactory>();

        var l = factory.CreateLayout();
        factory.InitLayout(l);
        Layout = l;
    }

    public event EventHandler<IDocumentViewModel> ActiveDocumentChanged;

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
        //todo
    }

    partial void OnFactoryChanged(ShellDockFactory oldValue, ShellDockFactory newValue)
    {
        if (oldValue != null)
        {
            oldValue.DockableAdded -= FactoryOnDockableAdded;
            oldValue.DockableRemoved -= FactoryOnDockableRemoved;
            oldValue.ActiveDockableChanged -= FactoryOnActiveDockableChanged;
            oldValue.FocusedDockableChanged -= FactoryOnFocusedDockableChanged;
        }

        if (newValue != null)
        {
            newValue.DockableAdded += FactoryOnDockableAdded;
            newValue.DockableRemoved += FactoryOnDockableRemoved;
            newValue.ActiveDockableChanged += FactoryOnActiveDockableChanged;
            newValue.FocusedDockableChanged += FactoryOnFocusedDockableChanged;
        }
    }

    private void CheckIfRaiseActiveDocumentChanged(object sender, IDocumentViewModel document)
    {
        if (prevDocumentId != document?.Id)
        {
            logger.LogDebugEx($"active document changed: [{document?.Id}] {document?.Title}");
            ActiveDocumentChanged?.Invoke(sender, document);
            prevDocumentId = document?.Id;
        }
    }

    private void FactoryOnFocusedDockableChanged(object sender, FocusedDockableChangedEventArgs e)
    {
        logger.LogDebugEx($"focus dockable changed: [{e.Dockable?.Id}] {e.Dockable?.Title}");
        if (e.Dockable is IDocumentViewModel documentViewModel)
            CheckIfRaiseActiveDocumentChanged(sender, documentViewModel);
    }

    private void FactoryOnActiveDockableChanged(object sender, ActiveDockableChangedEventArgs e)
    {
        logger.LogDebugEx($"active dockable changed: [{e.Dockable?.Id}] {e.Dockable?.Title}");
        if (e.Dockable is IDocumentViewModel documentViewModel)
            CheckIfRaiseActiveDocumentChanged(sender, documentViewModel);
    }

    private void FactoryOnDockableRemoved(object sender, DockableRemovedEventArgs e)
    {
        logger.LogDebugEx($"dockable removed: [{e.Dockable?.Id}] {e.Dockable?.Title}");
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
        logger.LogDebugEx($"dockable added: [{e.Dockable?.Id}] {e.Dockable?.Title}");
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

    [RelayCommand]
    private async void SaveLayout()
    {
        //todo
        using var stream = memoryStreamManager.GetStream();
        dockSerializer.Save(stream, Layout);
        var result = Encoding.UTF8.GetString(stream.GetReadOnlySequence());
    }

    [RelayCommand]
    private async void LoadLayout()
    {
        //todo
    }

    partial void OnShowFloatingWindowsInTaskbarChanged(bool value)
    {
        _shellView?.UpdateFloatingWindows(ShowFloatingWindowsInTaskbar);
    }
}