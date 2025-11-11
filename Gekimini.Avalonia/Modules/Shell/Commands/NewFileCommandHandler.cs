using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Modules.Shell;
using Injectio.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Gemini.Modules.Shell.Commands;

[RegisterSingleton<ICommandHandler>]
public class NewFileCommandHandler : ICommandListHandler<NewFileCommandListDefinition>
{
    private readonly IServiceProvider _serviceProvider;
    private int _newFileCounter = 1;

    public NewFileCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Populate(Command command, List<Command> commands)
    {
        foreach (var editorProvider in _serviceProvider.GetServices<IEditorProvider>())
        {
            if (!editorProvider.CanCreateNew)
                continue;

            foreach (var editorFileType in editorProvider.FileTypes)
                commands.Add(new Command(command.CommandDefinition)
                {
                    Text = editorFileType.Name,
                    IconSource = editorFileType.IconSource,
                    Tag = new NewFileTag
                    {
                        EditorProvider = editorProvider,
                        FileType = editorFileType
                    }
                });
        }
    }

    public void Update(Command command)
    {
        
    }

    public async Task Run(Command command)
    {
        var tag = (NewFileTag) command.Tag;
        var editor = tag.EditorProvider.Create();

        var viewAware = editor;
        viewAware.ViewAfterLoaded += frameworkElement =>
        {
            EventHandler<RoutedEventArgs> loadedHandler = null;
            loadedHandler = async (sender2, e2) =>
            {
                frameworkElement.Loaded -= loadedHandler;
                await tag.EditorProvider.New(editor,
                    string.Format(Resources.FileNewUntitled, _newFileCounter++ + tag.FileType.FileExtension));
            };
            frameworkElement.Loaded += loadedHandler;
        };

        await _serviceProvider.GetService<IShell>().OpenDocumentAsync(editor);
    }

    public class NewFileTag
    {
        public IEditorProvider EditorProvider;
        public EditorFileType FileType;
    }
}