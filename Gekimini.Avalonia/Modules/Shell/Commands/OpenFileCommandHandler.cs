using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Dock.Model.Controls;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands
{
    [RegisterSingleton<ICommandHandler>]
    public class OpenFileCommandHandler : CommandHandlerBase<OpenFileCommandDefinition>
    {
        private readonly IEnumerable<IEditorProvider> _editorProviders;

        public OpenFileCommandHandler(IEnumerable<IEditorProvider> editorProviders)
        {
            _editorProviders = editorProviders.ToArray();
        }

        public override void Update(Command command)
        {
            base.Update(command);

            command.Enabled = _editorProviders != null && _editorProviders.Any();
        }

        public override async Task Run(Command command)
        {
            /*
            var dialog = new OpenFileDialog();

            string filter = null;

            filter = "All Supported Files|" + string.Join(";", _editorProviders
                .SelectMany(x => x.FileTypes).Select(x => "*" + x.FileExtension));

            filter += "|" + string.Join("|", _editorProviders
                .SelectMany(x => x.FileTypes)
                .Select(x => x.Name + "|*" + x.FileExtension));

            dialog.Filter = filter;

            if (dialog.ShowDialog() == true)
                await _shell.OpenDocumentAsync(await GetEditor(dialog.FileName));
            */
        }
    }
}
