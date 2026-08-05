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

    public override async Task Populate(Command command, List<Command> commands)
    {
        var recentOpened = RecentOpenedManager.RecentRecordInfos.ToArray();

        for (var i = 0; i < recentOpened.Length; i++)
        {
            var item = recentOpened[i];
            var documentProvider = PickDocumentProvider(item);
            commands.Add(new Command(command.CommandDefinition)
            {
                Text = $"_{i + 1} {item.Name} ({item.LocationDescription})".ToLocalizedStringByRawText(),
                Tag = item,
                Enabled = documentProvider is not null && await CheckIsValid(documentProvider, item)
            });
        }
    }

    public override async Task Update(Command command)
    {
        if (command.Tag is not RecentRecordInfo info)
        {
            await base.Update(command);
            return;
        }

        var documentProvider = PickDocumentProvider(info);
        command.Enabled = documentProvider is not null && await CheckIsValid(documentProvider, info);
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

    private async Task<bool> CheckIsValid(IEditorProvider documentProvider, RecentRecordInfo info)
    {
        try
        {
            return await documentProvider.CheckIsValid(info);
        }
        catch (Exception e)
        {
            Logger.LogWarning(e, "Failed to check recent record validity: {RecentRecord}", info);
            return false;
        }
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
