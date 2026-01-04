namespace Gekimini.Avalonia.Attributes;

/// <summary>
///     Lazy inject service though (App.Current as App)?.ServiceProvider.GetService globally
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class GetServiceLazyAttribute : Attribute
{
}