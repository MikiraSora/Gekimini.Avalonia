namespace Gekimini.Avalonia.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class GenerateCommandRunDispatcherAttribute<TCommandDefinition> : Attribute
{
}