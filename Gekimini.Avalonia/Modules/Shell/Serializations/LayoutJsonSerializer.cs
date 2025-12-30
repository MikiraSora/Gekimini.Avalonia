using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using Dock.Model.Controls;
using Dock.Model.Core;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;
using Gekimini.Avalonia.Modules.Shell.ViewModels;
using Gekimini.Avalonia.Utils;
using Injectio.Attributes;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Modules.Shell.Serializations;

[RegisterSingleton<IDockSerializer>]
public class LayoutJsonSerializer : IDockSerializer
{
    private readonly IFactory factory;
    private readonly ILogger<LayoutJsonSerializer> logger;
    private readonly IServiceProvider serviceProvider;

    public LayoutJsonSerializer(ILogger<LayoutJsonSerializer> logger, IServiceProvider serviceProvider,
        IFactory factory)
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
        this.factory = factory;
    }

    private LayoutDockable SerializeLayout(IDockable dockable)
    {
        switch (dockable)
        {
            case IProportionalDock proportionalDock:
                return SerializeToLayoutObject<LayoutProportionalDock>(proportionalDock);
            case IDockDock dockDock:
                return SerializeToLayoutObject<LayoutDockDock>(dockDock);
            case IRootDock rootDock:
                return SerializeToLayoutObject<LayoutRootDock>(rootDock);
            case IStackDock stackDock:
                return SerializeToLayoutObject<LayoutStackDock>(stackDock);
            case IGridDock gridDock:
                return SerializeToLayoutObject<LayoutGridDock>(gridDock);
            case IWrapDock wrapDock:
                return SerializeToLayoutObject<LayoutWrapDock>(wrapDock);
            case IUniformGridDock uniformGridDock:
                return SerializeToLayoutObject<LayoutUniformGridDock>(uniformGridDock);
            case IProportionalDockSplitter proportionalDockSplitter:
                return SerializeToLayoutObject<LayoutProportionalDockSplitter>(proportionalDockSplitter);
            case IGridDockSplitter gridDockSplitter:
                return SerializeToLayoutObject<LayoutGridDockSplitter>(gridDockSplitter);
            case IToolDock toolDock:
                return SerializeToLayoutObject<LayoutToolDock>(toolDock);
            case IDocumentDock documentDock:
                return SerializeToLayoutObject<LayoutDocumentDock>(documentDock);
            //--------------------------------
            // spcial process
            case IDocument {Context: IDocumentViewModel} document:
                return SerializeToLayoutObject<LayoutDocument>(document);
            case ITool {Context: IToolViewModel} tool:
                return SerializeToLayoutObject<LayoutTool>(tool);
        }

        return null;
    }

    private T SerializeToLayoutObject<T>(IRootDock dock)
        where T : LayoutRootDock, new()
    {
        var layout = SerializeToLayoutObject<T>(dock as IDock);

        foreach (var window in dock.Windows ?? [])
        {
            var layoutWindow = new LayoutDockWindow();
            layoutWindow.CopyFrom(window);
            layout.Windows.Add(layoutWindow);
        }

        if (dock.Window is { } mainWindow)
        {
            var layoutWindow = new LayoutDockWindow();
            layoutWindow.CopyFrom(mainWindow);
            layout.Window = layoutWindow;
        }

        return layout;
    }

    private T SerializeToLayoutObject<T>(IDock dock)
        where T : LayoutDock, new()
    {
        var layout = new T();
        layout.CopyFrom(dock);

        foreach (var childDockable in dock.VisibleDockables)
        {
            var childLayout = SerializeLayout(childDockable);
            layout.VisibleDockables.Add(childLayout);
        }

        return layout;
    }

    private T SerializeToLayoutObject<T>(IDocument dockable)
        where T : LayoutDocument, new()
    {
        var layout = SerializeToLayoutObject<T>(dockable as IDockable);
        layout.ContextType = dockable?.Context?.GetType().FullName;
        return layout;
    }

    private T SerializeToLayoutObject<T>(ITool dockable)
        where T : LayoutTool, new()
    {
        var layout = SerializeToLayoutObject<T>(dockable as IDockable);
        layout.ContextType = dockable?.Context?.GetType().FullName;
        return layout;
    }

    private T SerializeToLayoutObject<T>(IDockable dockable)
        where T : LayoutDockable, new()
    {
        var layout = new T();
        layout.CopyFrom(dockable);
        return layout;
    }

    #region IDockSerializer implementation

    public string Serialize<T>(T value)
    {
        if (value is not IDockable dockable)
            throw new NotSupportedException("LayoutJsonSerializer only supports IDockable");
        var layoutData = SerializeLayout(dockable);
        return JsonSerializer.Serialize(layoutData, typeof(LayoutDockable),
            LayoutJsonSourceGeneratorContext.Default);
    }

    public T Deserialize<T>(string json)
    {
        var layout = JsonSerializer.Deserialize(json, LayoutJsonSourceGeneratorContext.Default.LayoutDockable);
        if (layout is null)
            throw new InvalidDataException("json is not a LayoutDockable json");
        var dockable = DeserializeLayout(layout);
        return (T) dockable;
    }

    private IDockable DeserializeLayout(LayoutDockable layout)
    {
        switch (layout)
        {
            case LayoutProportionalDock proportionalDock:
                return DeserializeToDockableObject(factory.CreateProportionalDock(), proportionalDock);
            case LayoutDockDock dockDock:
                return DeserializeToDockableObject(factory.CreateDockDock(), dockDock);
            case LayoutRootDock rootDock:
                return DeserializeToDockableObject(factory.CreateRootDock(), rootDock);
            case LayoutStackDock stackDock:
                return DeserializeToDockableObject(factory.CreateStackDock(), stackDock);
            case LayoutGridDock gridDock:
                return DeserializeToDockableObject(factory.CreateGridDock(), gridDock);
            case LayoutWrapDock wrapDock:
                return DeserializeToDockableObject(factory.CreateWrapDock(), wrapDock);
            case LayoutUniformGridDock uniformGridDock:
                return DeserializeToDockableObject(factory.CreateUniformGridDock(), uniformGridDock);
            case LayoutProportionalDockSplitter proportionalDockSplitter:
                return DeserializeToDockableObject(factory.CreateProportionalDockSplitter(), proportionalDockSplitter);
            case LayoutGridDockSplitter gridDockSplitter:
                return DeserializeToDockableObject(factory.CreateGridDockSplitter(), gridDockSplitter);
            case LayoutToolDock toolDock:
                return DeserializeToDockableObject(factory.CreateToolDock(), toolDock);
            case LayoutDocumentDock documentDock:
                return DeserializeToDockableObject(factory.CreateDocumentDock(), documentDock);
            // spcial process
            case LayoutDocument document:
                //ignore
                return null;
            case LayoutTool tool:
                return TryOpenTool(tool);
        }

        return null;
    }

    private IDockable TryOpenTool(LayoutTool layoutTool)
    {
        if (factory.DockableLocator?.TryGetValue(layoutTool.ContextType, out var toolCreateFactory) ?? false)
        {
            var tool = toolCreateFactory.Invoke();
            layoutTool.CopyTo(tool);
            return tool;
        }

        if (TypeCollectedActivatorHelper<IToolViewModel>.TryCreateInstance(serviceProvider, layoutTool.ContextType,
                out var toolViewModel))
        {
            var toolContainer = new ToolContainerViewModel
            {
                Context = toolViewModel
            };
            layoutTool.CopyTo(toolContainer);
            return toolContainer;
        }

        logger.LogWarningEx(
            $"Can't find deserialize layoutTool to dockable: layoutTool.ToolType = {layoutTool.ContextType}");

        return default;
    }

    private IRootDock DeserializeToDockableObject(IRootDock rootDock,
        LayoutRootDock layoutRootDock)
    {
        DeserializeToDockableObject(rootDock, layoutRootDock as LayoutDock);
        rootDock.Windows ??= new ObservableCollection<IDockWindow>();

        foreach (var layoutDockWindow in layoutRootDock.Windows)
        {
            var dockWindow = factory.CreateDockWindow();
            if (dockWindow != null)
            {
                layoutDockWindow.CopyTo(dockWindow);
                rootDock.Windows.Add(dockWindow);
            }
        }

        if (layoutRootDock.Window is { } defaultLayoutDockWindow)
        {
            var dockWindow = factory.CreateDockWindow();
            if (dockWindow != null)
            {
                defaultLayoutDockWindow.CopyTo(dockWindow);
                rootDock.Window = dockWindow;
            }
        }

        return rootDock;
    }

    private IDock DeserializeToDockableObject(IDock dock,
        LayoutDock layoutDock)
    {
        DeserializeToDockableObject(dock, layoutDock as LayoutDockable);
        dock.VisibleDockables ??= new ObservableCollection<IDockable>();

        foreach (var childLayoutDockable in layoutDock.VisibleDockables)
        {
            var childDockable = DeserializeLayout(childLayoutDockable);
            if (childDockable != null)
                dock.VisibleDockables.Add(childDockable);
        }

        return dock;
    }

    private IDockable DeserializeToDockableObject(IDockable dockable,
        LayoutDockable layoutDockable)
    {
        layoutDockable.CopyTo(dockable);
        return dockable;
    }

    public T Load<T>(Stream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8);
        var text = reader.ReadToEnd();
        return Deserialize<T>(text);
    }

    public void Save<T>(Stream stream, T value)
    {
        var json = Serialize(value);
        var layout = JsonSerializer.Deserialize(json, LayoutJsonSourceGeneratorContext.Default.LayoutDockable);
        var dockable = Deserialize<IDockable>(json);
        var writer = new StreamWriter(stream, Encoding.UTF8);
        writer.Write(json);
        writer.Flush();
    }

    #endregion
}