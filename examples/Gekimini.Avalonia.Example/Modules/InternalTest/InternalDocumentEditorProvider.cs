using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Example.Modules.InternalTest.ViewModels.Documents;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.RecentFiles;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Example.Modules.InternalTest;

[RegisterSingleton<IEditorProvider>]
public partial class InternalDocumentEditorProvider : IEditorProvider
{
    public static EditorFileType InternalDocumentEditorFileType { get; } = new("Internal Document File");

    [GetServiceLazy]
    private partial IServiceProvider ServiceProvider { get; }

    public IEnumerable<EditorFileType> FileTypes { get; } =
    [
        InternalDocumentEditorFileType
    ];

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
}