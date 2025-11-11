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
using Gekimini.Avalonia.Framework.Dialogs;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Modules.InternalTest.ViewModels;
using Gekimini.Avalonia.Modules.MainMenu;
using Gekimini.Avalonia.Modules.Shell.Models;
using Gekimini.Avalonia.Modules.Shell.Views;
using Gekimini.Avalonia.Modules.StatusBar;
using Gekimini.Avalonia.Modules.ToolBars;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Gekimini.Avalonia.Utils.MethodExtensions;
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
    private readonly IDialogManager dialogManager;
    private readonly IDockSerializer dockSerializer;
    private readonly ILogger<ShellViewModel> logger;
    private readonly RecyclableMemoryStreamManager memoryStreamManager;
    private readonly IServiceProvider serviceProvider;
    private readonly ISettingManager settingManager;

    private IShellView _shellView;

    [ObservableProperty]
    private IDockable activeDockable;

    [ObservableProperty]
    private IDocumentViewModel activeDocument;

    [ObservableProperty]
    private ShellDockFactory factory;

    [ObservableProperty]
    private IRootDock layout;

    [ObservableProperty]
    private IMenu mainMenu;

    private string prevDocumentId;

    [ObservableProperty]
    private bool showFloatingWindowsInTaskbar;

    [ObservableProperty]
    private IStatusBar statusBar;

    [ObservableProperty]
    private IToolBars toolBars;

    public ShellViewModel(IServiceProvider serviceProvider, IDockSerializer dockSerializer,
        RecyclableMemoryStreamManager memoryStreamManager,
        ISettingManager settingManager,
        IEnumerable<IModule> modules,
        IStatusBar statusBar,
        IToolBars toolBars,
        IMenu mainMenu,
        IDialogManager dialogManager,
        ILogger<ShellViewModel> logger)
    {
        this.serviceProvider = serviceProvider;
        this.dockSerializer = dockSerializer;
        this.memoryStreamManager = memoryStreamManager;
        this.settingManager = settingManager;
        _modules = modules;
        this.dialogManager = dialogManager;
        this.logger = logger;

        Factory = this.serviceProvider.Resolve<ShellDockFactory>();
        StatusBar = statusBar;
        ToolBars = toolBars;
        MainMenu = mainMenu;
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

    private async void InitLayout()
    {
        try
        {
            await LoadLayout();
        }
        catch (Exception e)
        {
            logger.LogErrorEx(e, e.Message);

            var l = Factory.CreateLayout();
            Factory.InitLayout(l);
            Layout = l;

            await dialogManager.ShowMessageDialog("Load stored layout failed, we will use default layout.",
                DialogMessageType.Error);
        }
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

        if (Layout == null)
            InitLayout();
    }

    public override void OnViewBeforeUnload(Control view)
    {
        base.OnViewBeforeUnload(view);
        SaveLayout().NoWait();
    }

    [RelayCommand]
    private async Task AddDocument()
    {
        var document = serviceProvider.Resolve<InternalTestDocumentViewModel>();
        document.Text = Guid.NewGuid().ToString();
        await OpenDocumentAsync(document);
    }

    [RelayCommand]
    private void AddTool(string dockEnum)
    {
        var tool = serviceProvider.Resolve<InternalTestToolViewModel>();
        tool.Dock = Enum.Parse<DockMode>(dockEnum);
        ShowTool(tool);
    }

    [RelayCommand]
    private void RemoveLastCreatedTool()
    {
        HideTool(Tools.LastOrDefault());
    }

    [RelayCommand]
    private void RemoveLastCreatedDocument()
    {
        CloseDocumentAsync(Documents.LastOrDefault());
    }

    [RelayCommand]
    private async Task SaveLayout()
    {
        if ((App.Current as App)?.TopLevel?.StorageProvider is not { } storageProvider)
            return;

        var json = dockSerializer.Serialize(Layout);

        await settingManager.LoadAndSave(GekiminiSetting.JsonTypeInfo, setting => setting.ShellLayout = json);
    }

    [RelayCommand]
    private async Task LoadLayout()
    {
        if ((App.Current as App)?.TopLevel?.StorageProvider is not { } storageProvider)
            return;

        var setting = settingManager.GetSetting(GekiminiSetting.JsonTypeInfo);
        var dockable = dockSerializer.Deserialize<IRootDock>(setting.ShellLayout);
        if (dockable is null)
            //todo log
            return;

        Factory.InitLayout(dockable);
        Layout = dockable;

        addTools.Clear();
        addedDocuments.Clear();

        addTools.AddRange(Factory.Find(_ => true).OfType<IToolViewModel>());
        addedDocuments.AddRange(Factory.Find(_ => true).OfType<IDocumentViewModel>());
    }

    partial void OnShowFloatingWindowsInTaskbarChanged(bool value)
    {
        _shellView?.UpdateFloatingWindows(ShowFloatingWindowsInTaskbar);
    }
}