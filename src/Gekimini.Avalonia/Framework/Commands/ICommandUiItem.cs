namespace Gekimini.Avalonia.Framework.Commands
{
    public interface ICommandUiItem
    {
        CommandDefinitionBase CommandDefinition { get; }
        global::System.Threading.Tasks.Task Update(CommandHandlerWrapper commandHandler);
    }
}
