using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Core.Events;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.Dialogs;
using Gekimini.Avalonia.Framework.Documents;
using Gekimini.Avalonia.Models.Events;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Modules.MainMenu;
using Gekimini.Avalonia.Modules.Shell.Views;
using Gekimini.Avalonia.Modules.StatusBar;
using Gekimini.Avalonia.Modules.ToolBars;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Gekimini.Avalonia.ViewModels;
using Gekimini.Avalonia.Views;
using Injectio.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Modules.Shell.ViewModels;

[RegisterSingleton<IShell>]
public partial class ShellViewModel : ViewModelBase, IShell,
    IRecipient<ApplicationAskQuitEvent>,
    IRecipient<ApplicationQuitEvent>
{
    private readonly IEnumerable<IModule> _modules;

    private readonly List<IDocumentViewModel> addedDocuments = new();
    private readonly List<IToolViewModel> addTools = new();

    private readonly Dictionary<string, DocumentContainerViewModel> cachedIdToDocumentContainerMap = new();
    private readonly Dictionary<string, ToolContainerViewModel> cachedIdToToolContainerMap = new();
    private readonly IDialogManager dialogManager;
    private readonly IDockSerializer dockSerializer;
    private readonly ILogger<ShellViewModel> logger;
    private readonly ILogger printLogger;
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
        ISettingManager settingManager,
        IEnumerable<IModule> modules,
        IStatusBar statusBar,
        IToolBars toolBars,
        IMenu mainMenu,
        IDialogManager dialogManager,
        ILoggerFactory loggerFactory,
        ILogger<ShellViewModel> logger)
    {
        this.serviceProvider = serviceProvider;
        this.dockSerializer = dockSerializer;
        this.settingManager = settingManager;
        _modules = modules;
        this.dialogManager = dialogManager;
        this.logger = logger;

        Factory = this.serviceProvider.Resolve<ShellDockFactory>();
        StatusBar = statusBar;
        ToolBars = toolBars;
        MainMenu = mainMenu;
        printLogger = loggerFactory.CreateLogger("DumpDock");
    }

    public void Receive(ApplicationAskQuitEvent message)
    {
        message.Reply(OnApplicationAskQuit(message));
    }

    public void Receive(ApplicationQuitEvent message)
    {
        SaveLayout();
    }

    public event EventHandler<IDocumentViewModel> ActiveDocumentChanged;

    public IEnumerable<IDocumentViewModel> Documents => addedDocuments;

    public IEnumerable<IToolViewModel> Tools => addTools;

    public void ShowTool(IToolViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        var id = GetId(model);

        if (Tools.Contains(model))
        {
            logger.LogWarningEx($"can't show tool multi times, tool {id}: {model.GetType().Name}");
            return;
        }

        var toolContainer = new ToolContainerViewModel
        {
            Id = id, Context = model
        };
        //Factory.ContextLocator![id] = () => model;
        Factory.AddTool(toolContainer);
        cachedIdToToolContainerMap[id] = toolContainer;
    }

    public void HideTool<TTool>() where TTool : IToolViewModel
    {
        HideTool(Tools.OfType<TTool>().FirstOrDefault());
    }

    public void HideTool(IToolViewModel model)
    {
        if (model is null)
            return;
        var id = GetId(model);

        if (cachedIdToToolContainerMap.TryGetValue(id, out var toolContainer))
        {
            logger.LogInformationEx($"Hide tool {id}: {model.GetType().Name}");
            Factory.RemoveTool(toolContainer);
            cachedIdToToolContainerMap.Remove(id);
        }
        else
        {
            logger.LogWarningEx($"Hide tool {id} but can't find its container.");
        }
    }

    public Task OpenDocumentAsync(IDocumentViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var id = GetId(model);

        if (addedDocuments.Contains(model))
        {
            //exist, just active it.
            logger.LogInformationEx($"Active&Focus existed document {id}: {model.GetType().Name}");
            if (cachedIdToDocumentContainerMap.TryGetValue(id, out var documentContainer))
                Factory.ActiveAndFocus(documentContainer);
            else
                logger.LogWarningEx($"Active&Focus existed document {id} but can't find its container.");
        }
        else
        {
            logger.LogInformationEx($"Open new document {id}: {model.GetType().Name}");

            var documentContainer = new DocumentContainerViewModel
            {
                Id = id, Context = model
            };
            //Factory.ContextLocator![id] = () => model;
            Factory.AddDocument(documentContainer);
            cachedIdToDocumentContainerMap[id] = documentContainer;
        }

        return Task.CompletedTask;
    }

    public Task CloseDocumentAsync(IDocumentViewModel model)
    {
        if (model is null)
            return Task.CompletedTask;
        var id = GetId(model);

        if (cachedIdToDocumentContainerMap.TryGetValue(id, out var documentContainer))
        {
            logger.LogInformationEx($"Close document {id}: {model.GetType().Name}");
            Factory.RemoveDocument(documentContainer);
            cachedIdToDocumentContainerMap.Remove(id);
        }
        else
        {
            logger.LogWarningEx($"Close document {id} but can't find its container.");
        }

        return Task.CompletedTask;
    }

    public async Task ResetLayout()
    {
        foreach (var tool in Tools.ToArray())
            HideTool(tool);
        foreach (var document in Documents.ToArray())
            await CloseDocumentAsync(document);

        var newLayout = Factory.CreateLayout();
        Factory.InitLayout(newLayout);
        Layout = newLayout;
    }

    public void ShowTool<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TTool>(
        bool allowDuplicate = false)
        where TTool : IToolViewModel
    {
        if (Tools.OfType<TTool>().Any())
            if (!allowDuplicate)
            {
                logger.LogWarningEx($"can't show more same tool, tool {typeof(TTool).Name}");
                return;
            }

        IToolViewModel toolViewModel;
        if (typeof(ITool).IsAbstract || typeof(ITool).IsInterface)
            toolViewModel = serviceProvider.GetService<TTool>();
        else
            toolViewModel = serviceProvider.Resolve<TTool>();
        ShowTool(toolViewModel);
    }

    private string GetId(IDockableViewModel dockableViewModel)
    {
        ArgumentNullException.ThrowIfNull(dockableViewModel);
        return dockableViewModel.GetType().FullName;
    }

    private async void InitLayout()
    {
        logger.LogInformationEx("Begin initialize shell dock layout");
        try
        {
            LoadLayout();
            logger.LogInformationEx("Initialize shell dock layout successfully.");
        }
        catch (Exception e)
        {
            logger.LogErrorEx(e, $"Initialize shell dock layout failed: {e.Message}");

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
        var id = GetId(document);
        if (prevDocumentId != id)
        {
            logger.LogDebugEx($"active document changed: [{id}] {document?.Title.Text}");
            ActiveDocument = document;
            prevDocumentId = id;
        }
    }

    partial void OnActiveDocumentChanged(IDocumentViewModel value)
    {
        ActiveDocumentChanged?.Invoke(this, ActiveDocument);
    }

    private void FactoryOnFocusedDockableChanged(object sender, FocusedDockableChangedEventArgs e)
    {
        /*
         * todo: improve which Document or Tool were focused/unfocused.
         */

        logger.LogDebugEx($"focus dockable changed: [{e.Dockable?.Id}] {e.Dockable?.Title}");
        ActiveDockable = e.Dockable;
        if (e.Dockable is IDocument {Context: IDocumentViewModel documentViewModel})
            CheckIfRaiseActiveDocumentChanged(sender, documentViewModel);
    }

    private void FactoryOnActiveDockableChanged(object sender, ActiveDockableChangedEventArgs e)
    {
        /*
         * todo: improve which Document or Tool were actived/deactived.
         */

        logger.LogDebugEx($"active dockable changed: [{e.Dockable?.Id}] {e.Dockable?.Title}");
        ActiveDockable = e.Dockable;
        if (e.Dockable is IDocument {Context: IDocumentViewModel documentViewModel})
            CheckIfRaiseActiveDocumentChanged(sender, documentViewModel);
    }

    private void FactoryOnDockableRemoved(object sender, DockableRemovedEventArgs e)
    {
        logger.LogDebugEx($"dockable removed: [{e.Dockable?.Id}] {e.Dockable?.Title}");
        switch (e.Dockable)
        {
            case IDocument {Context: IDocumentViewModel documentViewModel}:
                addedDocuments.Remove(documentViewModel);
                if (ActiveDocument == documentViewModel)
                    ActiveDocument = default;

                break;
            case ITool {Context: IToolViewModel toolViewModel}:
                addTools.Remove(toolViewModel);
                break;
        }

        if (ActiveDockable == e.Dockable)
            ActiveDockable = default;
    }

    private void FactoryOnDockableAdded(object sender, DockableAddedEventArgs e)
    {
        logger.LogDebugEx($"dockable added: [{e.Dockable?.Id}] {e.Dockable?.Title}");
        switch (e.Dockable)
        {
            case IDocument {Context: IDocumentViewModel documentViewModel}:
                addedDocuments.Add(documentViewModel);
                break;
            case ITool {Context: IToolViewModel toolViewModel}:
                addTools.Add(toolViewModel);
                break;
        }
    }

    public override void OnViewAfterLoaded(IView view)
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

    private async Task<bool> OnApplicationAskQuit(ApplicationAskQuitEvent message)
    {
        foreach (var document in Documents)
        {
            var id = GetId(document);
            if (cachedIdToDocumentContainerMap.TryGetValue(id, out var documentContainer))
            {
                if (!await Factory.CanCloseDocument(documentContainer))
                    return false;
            }
            else
            {
                logger.LogWarningEx($"can't find document container {id} to ask quit.");
            }
        }

        return true;
    }

    private void SaveLayout()
    {
        var json = dockSerializer.Serialize(Layout);
        settingManager.LoadAndSave(GekiminiSetting.JsonTypeInfo, setting => setting.ShellLayout = json);
        //logger.LogDebugEx($"Saved setting.ShellLayout Hex: {Convert.ToHexString(Encoding.UTF8.GetBytes(json))}");
    }

    private void LoadLayout()
    {
        var setting = settingManager.GetSetting(GekiminiSetting.JsonTypeInfo);
        //logger.LogDebugEx($"loaded setting.ShellLayout Hex: {Convert.ToHexString(Encoding.UTF8.GetBytes(setting.ShellLayout))}");
        var dockable = dockSerializer.Deserialize<IRootDock>(setting.ShellLayout);
        if (dockable is null)
        {
            logger.LogErrorEx("Deserialize layout file failed, deserialized dockable object is null.");
            return;
        }

        void printDock(IDockable dockable, int stack = 0)
        {
            printLogger.LogDebugEx($"stack: {stack}");
            printLogger.LogDebugEx($"id: {dockable.Id}");
            printLogger.LogDebugEx($"title: {dockable.Title}");
            printLogger.LogDebugEx($"dockMode: {dockable.Dock}");
            printLogger.LogDebugEx($"owner: {dockable.Owner?.Title}");
            printLogger.LogDebugEx($"originalOwner: {dockable.OriginalOwner?.Title}");

            if (dockable is IDock dock)
            {
                printLogger.LogDebugEx($"isActive: {dock.IsActive}");
                printLogger.LogDebugEx($"openedDockablesCount: {dock.OpenedDockablesCount}");
                printLogger.LogDebugEx($"defaultDockable: {dock.DefaultDockable?.Title}");
                printLogger.LogDebugEx($"focusedDockable: {dock.FocusedDockable?.Title}");
                printLogger.LogDebugEx($"activeDockable: {dock.ActiveDockable?.Title}");
                if (dock.VisibleDockables.Any())
                {
                    printLogger.LogDebugEx("children:");
                    foreach (var childDockable in dock.VisibleDockables)
                        printDock(childDockable, stack + 1);
                }
            }
        }

        printDock(dockable);

        Factory.InitLayout(dockable);
        Layout = dockable;

        addTools.Clear();
        addedDocuments.Clear();

        addTools.AddRange(Factory.Find(_ => true).OfType<IToolViewModel>());
        logger.LogDebugEx(
            $"Deserialized tools: {string.Join(", ", addTools.Select(x => x.GetType().Name))}");
        addedDocuments.AddRange(Factory.Find(_ => true).OfType<IDocumentViewModel>());
        logger.LogDebugEx(
            $"Deserialized documents: {string.Join(", ", addedDocuments.Select(x => x.GetType().Name))}");
    }

    partial void OnShowFloatingWindowsInTaskbarChanged(bool value)
    {
        //todo not support
        _shellView?.UpdateFloatingWindows(ShowFloatingWindowsInTaskbar);
    }
}