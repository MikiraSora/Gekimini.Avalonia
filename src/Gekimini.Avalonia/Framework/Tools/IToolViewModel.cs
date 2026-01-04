using Dock.Model.Controls;
using Dock.Model.Core;
using Gekimini.Avalonia.Framework.Documents;
using Gekimini.Avalonia.ViewModels;

namespace Gekimini.Avalonia.Framework;

public interface IToolViewModel : IDockableViewModel
{
    DockMode Dock { get; set; }
}