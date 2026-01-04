using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia;

public static class IServiceProviderEx
{
    public static T Resolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(
        this IServiceProvider serviceProvider)
    {
        return ActivatorUtilities.CreateInstance<T>(serviceProvider);
    }

    public static object Resolve(this IServiceProvider serviceProvider,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type resolveType)
    {
        return ActivatorUtilities.CreateInstance(serviceProvider, resolveType);
    }
}