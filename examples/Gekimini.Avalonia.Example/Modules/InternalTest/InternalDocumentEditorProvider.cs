using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Example.Modules.InternalTest.ViewModels.Documents;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.RecentFiles;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Example.Modules.InternalTest;

[RegisterSingleton<IEditorProvider>]
public partial class InternalDocumentEditorProvider : IEditorProvider
{
    public static EditorFileType[] SupportFileTypes { get; } =
    [
        new("InternalDocumentFileType", "Internal Document File".ToLocalizedStringByRawText())
        {
            Patterns = ["*.internal"],
            MimeTypes =  ["application/unknown"],
        }
    ];

    [GetServiceLazy]
    private partial IServiceProvider ServiceProvider { get; }

    public IEnumerable<EditorFileType> FileTypes => SupportFileTypes;

    public bool CanCreateNew => true;

    public IDocumentViewModel Create()
    {
        return ServiceProvider.Resolve<InternalTestDocumentViewModel>();
    }

    public async Task<bool> TryNew(IDocumentViewModel document)
    {
        if (document is not InternalTestDocumentViewModel internalTestDocumentViewModel)
            return false;
        return await internalTestDocumentViewModel.New();
    }

    public async Task<bool> TryOpen(IDocumentViewModel document)
    {
        if (document is not InternalTestDocumentViewModel internalTestDocumentViewModel)
            return false;
        return await internalTestDocumentViewModel.Load();
    }

    public async Task<bool> TryOpen(IDocumentViewModel document, RecentRecordInfo recordInfo)
    {
        if (document is not InternalTestDocumentViewModel internalTestDocumentViewModel)
            return false;
        return await internalTestDocumentViewModel.Load(recordInfo);
    }

    public async Task<bool> CheckIsValid(RecentRecordInfo recordInfo)
    {
        var storageProvider = (App.Current as App)?.TopLevel?.StorageProvider;
        if (storageProvider is null || string.IsNullOrWhiteSpace(recordInfo.LocationDescription))
            return false;

        return await storageProvider.OpenFileBookmarkAsync(recordInfo.LocationDescription) is not null;
    }
}
