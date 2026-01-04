using Gekimini.Avalonia.Utils.MethodExtensions;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Example;

public abstract class ExampleApp : App
{
    protected override void RegisterServices(IServiceCollection serviceCollection)
    {
        base.RegisterServices(serviceCollection);

        serviceCollection.AddGekiminiAvaloniaExample();

        serviceCollection.AddTypeCollectedActivator(ViewTypeCollectedActivator.Default);

        serviceCollection.AddTypeCollectedActivator(ToolViewModelTypeCollectedActivator.Default);
    }
}