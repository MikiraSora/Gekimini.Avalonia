using System;
using System.Collections.Generic;
using System.Linq;
using Gekimini.Avalonia.Modules.Toolbox.Models;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Toolbox.Services;

[RegisterSingleton<IToolboxService>]
public class ToolboxService : IToolboxService
{
    private readonly Dictionary<string, IEnumerable<ToolboxItem>> _toolboxItems;

    public ToolboxService(IServiceProvider serviceProvider, IEnumerable<ToolboxItem> toolboxItems)
    {
        _toolboxItems = toolboxItems
            .GroupBy(x => x.DocumentType)
            .ToDictionary(x => x.Key, x => x.AsEnumerable());
    }

    public IEnumerable<ToolboxItem> GetToolboxItems(Type documentType)
    {
        IEnumerable<ToolboxItem> result;
        if (_toolboxItems.TryGetValue(documentType.FullName, out result))
            return result;
        return new List<ToolboxItem>();
    }
}