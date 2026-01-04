using System;
using Avalonia.Input;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework.Commands;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Shell.Commands;

[RegisterSingleton<CommandDefinitionBase>]
public class OpenFileCommandListDefinition : CommandListDefinition
{
    public const string CommandName = "File.OpenFile";

    public override string Name => CommandName;
}