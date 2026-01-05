using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Dialogs;
using Gekimini.Avalonia.Modules.Shell;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Injectio.Attributes;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Framework.RecentFiles.Commands;

[RegisterSingleton<ICommandHandler>]
public partial class OpenRecentFileCommandHandler : CommandListHandlerBase<OpenRecentFileCommandListDefinition>
{
    [GetServiceLazy]
    private partial IEditorRecentFilesManager RecentOpenedManager { get; }

    [GetServiceLazy]
    private partial IShell Shell { get; }

    [GetServiceLazy]
    private partial IEnumerable<IEditorProvider> EditorProviders { get; }

    [GetServiceLazy]
    private partial ILogger<OpenRecentFileCommandHandler> Logger { get; }

    [GetServiceLazy]
    private partial IDialogManager DialogManager { get; }

    public override void Populate(Command command, List<Command> commands)
    {
        var recentOpened = RecentOpenedManager.RecentRecordInfos;

        for (var i = 0; i < recentOpened.Count(); i++)
        {
            var item = recentOpened.ElementAtOrDefault(i);
            commands.Add(new Command(command.CommandDefinition)
            {
                Text = $"_{i + 1} {item.Name} ({item.LocationDescription})".ToLocalizedStringByRawText(),
                Tag = item,
                Enabled = PickDocumentProvider(item) is not null
            });
        }
    }

    public override async Task Run(Command command)
    {
        var info = command.Tag as RecentRecordInfo;
        Logger.LogDebugEx($"OpenRecentFileCommandHandler.Run() try open recent: {info}");

        await OpenRecentFileByDocument(info);
    }

    private IEditorProvider PickDocumentProvider(RecentRecordInfo info)
    {
        return EditorProviders.FirstOrDefault(x =>
            x.FileTypes.Any(t =>
                t.Id.Equals(info.EditorFileTypeId, StringComparison.OrdinalIgnoreCase)));
    }

    private async Task OpenRecentFileByDocument(RecentRecordInfo info)
    {
        var documentProvider = PickDocumentProvider(info);

        if (documentProvider is null)
        {
            await DialogManager.ShowMessageDialog(ProgramLanguages.NoDocumentSupportOpenRecentInfo, DialogMessageType.Error);
            return;
        }

        var doc = documentProvider.Create();

        var shouldShow = await documentProvider.TryOpen(doc, info);
        if (shouldShow)
            await Shell.OpenDocumentAsync(doc);
    }
}