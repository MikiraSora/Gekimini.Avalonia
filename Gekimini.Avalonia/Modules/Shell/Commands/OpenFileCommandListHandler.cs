using System.Collections.Generic;
using System.Threading.Tasks;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<ICommandHandler>]
public partial class OpenFileCommandListHandler : CommandListHandlerBase<OpenFileCommandListDefinition>
{
    [GetServiceLazy]
    private partial IShell Shell { get; }

    [GetServiceLazy]
    private partial IEnumerable<IEditorProvider> EditorProviders { get; }

    public override void Populate(Command command, List<Command> commands)
    {
        foreach (var editorProvider in EditorProviders)
        {
            if (!editorProvider.CanCreateNew)
                continue;

            foreach (var editorFileType in editorProvider.FileTypes)
                commands.Add(new Command(command.CommandDefinition)
                {
                    Text = editorFileType.Name,
                    IconSource = editorFileType.IconSource,
                    Tag = new OpenFileTag
                    {
                        EditorProvider = editorProvider
                    }
                });
        }
    }

    public override void Update(Command command)
    {
    }

    public override async Task Run(Command command)
    {
        var tag = (OpenFileTag) command.Tag;
        var editor = tag.EditorProvider.Create();

        var shouldShow = await tag.EditorProvider.TryOpen(editor);
        if (shouldShow)
            await Shell.OpenDocumentAsync(editor);
    }

    public class OpenFileTag
    {
        public IEditorProvider EditorProvider;
    }
}