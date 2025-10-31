using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Dock.Model.Controls;
using Dock.Model.Core;
using Gekimini.Avalonia.Modules.Shell.Serializations.Layouts;
using Injectio.Attributes;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Modules.Shell.Serializations;

[RegisterSingleton<IDockSerializer>]
public class LayoutJsonSerializer : IDockSerializer
{
    private readonly ILogger<LayoutJsonSerializer> logger;
    private readonly IServiceProvider serviceProvider;

    public LayoutJsonSerializer(ILogger<LayoutJsonSerializer> logger, IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
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
            case IDocument document:
                return SerializeToLayoutObject<LayoutDocument>(document);
            case ITool tool:
                return SerializeToLayoutObject<LayoutTool>(tool);
        }

        return null;
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
        return JsonSerializer.Serialize(layoutData);
    }

    public T Deserialize<T>(string text)
    {
        throw new NotImplementedException();
    }

    public T Load<T>(Stream stream)
    {
        throw new NotImplementedException();
    }

    public void Save<T>(Stream stream, T value)
    {
        var json = Serialize(value);
        var writer = new StreamWriter(stream, Encoding.UTF8);
        writer.Write(json);
    }

    #endregion
}