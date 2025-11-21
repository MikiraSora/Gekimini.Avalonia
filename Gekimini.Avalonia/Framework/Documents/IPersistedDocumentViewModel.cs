using System.Threading.Tasks;
using Gekimini.Avalonia.Framework.RecentFiles;

namespace Gekimini.Avalonia.Framework;

public interface IPersistedDocumentViewModel : IDocumentViewModel
{
    bool IsNew { get; }
    bool IsDirty { get; }

    Task<bool> New();
    Task<bool> Load();
    Task<bool> Load(RecentRecordInfo info);
    Task Save();
    Task SaveAs();
}