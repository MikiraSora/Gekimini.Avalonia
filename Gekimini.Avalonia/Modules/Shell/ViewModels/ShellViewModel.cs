using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Core.Events;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.Dialogs;
using Gekimini.Avalonia.Models.Events;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Modules.Documents.Models;
using Gekimini.Avalonia.Modules.Documents.ViewModels;
using Gekimini.Avalonia.Modules.MainMenu;
using Gekimini.Avalonia.Modules.Shell.Models;
using Gekimini.Avalonia.Modules.Shell.Views;
using Gekimini.Avalonia.Modules.StatusBar;
using Gekimini.Avalonia.Modules.ToolBars;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Gekimini.Avalonia.Platforms.Services.Window;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Gekimini.Avalonia.ViewModels;
using Injectio.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace Gekimini.Avalonia.Modules.Shell.ViewModels;

[RegisterSingleton<IShell>]
public partial class ShellViewModel : ViewModelBase, IShell,
    IRecipient<ApplicationAskQuitEvent>,
    IRecipient<ApplicationQuitEvent>
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
    private readonly IWindowManager windowManager;

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
        IWindowManager windowManager,
        IDialogManager dialogManager,
        ILogger<ShellViewModel> logger)
    {
        this.serviceProvider = serviceProvider;
        this.dockSerializer = dockSerializer;
        this.memoryStreamManager = memoryStreamManager;
        this.settingManager = settingManager;
        _modules = modules;
        this.windowManager = windowManager;
        this.dialogManager = dialogManager;
        this.logger = logger;

        Factory = this.serviceProvider.Resolve<ShellDockFactory>();
        StatusBar = statusBar;
        ToolBars = toolBars;
        MainMenu = mainMenu;
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

    public void ShowTool<TTool>()
        where TTool : IToolViewModel
    {
        var toolViewModel = default(IToolViewModel);
        if (typeof(ITool).IsAbstract || typeof(ITool).IsInterface)
            toolViewModel = serviceProvider.GetService<TTool>();
        else
            toolViewModel = serviceProvider.Resolve<TTool>();
        ShowTool(toolViewModel);
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
        logger.LogInformationEx($"Hide tool {model.Id}: {model.GetType().Name}");
        Factory.RemoveTool(model);
    }

    public Task OpenDocumentAsync(IDocumentViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (addedDocuments.Contains(model))
        {
            //exist, just active it.
            logger.LogInformationEx($"Active&Focus existed document {model.Id}: {model.GetType().Name}");
            Factory.ActiveAndFocus(model);
        }
        else
        {
            logger.LogInformationEx($"Open new document {model.Id}: {model.GetType().Name}");
            Factory.AddDocument(model);
        }

        return Task.CompletedTask;
    }

    public Task CloseDocumentAsync(IDocumentViewModel model)
    {
        if (model is null)
            return Task.CompletedTask;

        logger.LogInformationEx($"Close document {model.Id}: {model.GetType().Name}");
        Factory.RemoveDocument(model);

        return Task.CompletedTask;
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
        if (prevDocumentId != document?.Id)
        {
            logger.LogDebugEx($"active document changed: [{document?.Id}] {document?.Title}");
            ActiveDocument = document;
            prevDocumentId = document?.Id;
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
        if (e.Dockable is IDocumentViewModel documentViewModel)
            CheckIfRaiseActiveDocumentChanged(sender, documentViewModel);
    }

    private void FactoryOnActiveDockableChanged(object sender, ActiveDockableChangedEventArgs e)
    {
        /*
         * todo: improve which Document or Tool were actived/deactived.
         */

        logger.LogDebugEx($"active dockable changed: [{e.Dockable?.Id}] {e.Dockable?.Title}");
        ActiveDockable = e.Dockable;
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
                if (ActiveDocument == document)
                    ActiveDocument = default;

                break;
            case IToolViewModel tool:
                addTools.Remove(tool);
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

    private async Task<bool> OnApplicationAskQuit(ApplicationAskQuitEvent message)
    {
        foreach (var document in Documents.OfType<IPersistedDocumentViewModel>().Where(x => x.IsDirty))
        {
            var dialog = new SaveDirtyDocumentDialogViewModel
            {
                DocumentName = document.Title
            };
            await dialogManager.ShowDialog(dialog);

            switch (dialog.Result)
            {
                case DialogResult.Yes:
                    var isSaveSuccess = await document.Save();
                    if (!isSaveSuccess)
                    {
                        await dialogManager.ShowMessageDialog(
                            $"Document {document.Title} saving is failed/canceled, application exit has been canceled.");
                        return false;
                    }

                    break;
                case DialogResult.No:
                    //user cancel save this document, just continue.
                    break;
                case DialogResult.Cancel:
                default:
                    //user cancel application exit process.
                    return false;
            }
        }

        return true;
    }

    private void SaveLayout()
    {
        var json = dockSerializer.Serialize(Layout);
        settingManager.LoadAndSave(GekiminiSetting.JsonTypeInfo, setting => setting.ShellLayout = json);
        logger.LogDebugEx($"Saved setting.ShellLayout Hex: {Convert.ToHexString(Encoding.UTF8.GetBytes(json))}");
    }

    private void LoadLayout()
    {
        var setting = settingManager.GetSetting(GekiminiSetting.JsonTypeInfo);
        logger.LogDebugEx(
            $"loaded setting.ShellLayout Hex: {Convert.ToHexString(Encoding.UTF8.GetBytes(setting.ShellLayout))}");
        var dockable = dockSerializer.Deserialize<IRootDock>(setting.ShellLayout);
        if (dockable is null)
        {
            logger.LogErrorEx("Deserialize layout file failed, deserialized dockable object is null.");
            return;
        }

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