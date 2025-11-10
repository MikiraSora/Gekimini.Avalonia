using System.Threading.Tasks;

namespace Gekimini.Avalonia.Framework;

public interface IPersistedDocumentViewModel : IDocumentViewModel
{
    bool IsNew { get; }
    string FileName { get; }
    string FilePath { get; }

    Task New(string fileName);
    Task Load(string filePath);
    Task Save(string filePath);
}