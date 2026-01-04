using System;
using System.Collections.Generic;
using System.IO;
using Dock.Model.Controls;
using Gekimini.Avalonia.Framework;

namespace Gekimini.Avalonia.Modules.Shell.Views;

public interface IShellView
{
    void LoadLayout(Stream stream, Action<ITool> addToolCallback, Action<IDocument> addDocumentCallback,
        Dictionary<string, ILayoutItem> itemsState);

    void SaveLayout(Stream stream);

    void UpdateFloatingWindows(bool isFloating);
}