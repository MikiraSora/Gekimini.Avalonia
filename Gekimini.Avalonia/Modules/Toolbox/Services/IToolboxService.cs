using System;
using System.Collections.Generic;
using Gekimini.Avalonia.Modules.Toolbox.Models;

namespace Gekimini.Avalonia.Modules.Toolbox.Services
{
    public interface IToolboxService
    {
        IEnumerable<ToolboxItem> GetToolboxItems(Type documentType);
    }
}