using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.ViewModels;

namespace Gekimini.Avalonia.Framework.Documents;

public interface IDockableViewModel : IViewModel
{
    LocalizedString Title { get; set; }
}