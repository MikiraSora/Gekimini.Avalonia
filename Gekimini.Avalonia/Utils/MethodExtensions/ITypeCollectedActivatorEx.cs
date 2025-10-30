using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Utils.MethodExtensions;

public static class ITypeCollectedActivatorEx
{
    public static IServiceCollection AddTypeCollectedActivator<T>(this IServiceCollection services,
        ITypeCollectedActivator<T> activator)
    {
        services.AddSingleton(_ => activator);

        return services;
    }
}