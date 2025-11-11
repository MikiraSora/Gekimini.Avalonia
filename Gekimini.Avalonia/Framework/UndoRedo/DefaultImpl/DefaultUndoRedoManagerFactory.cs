using System;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.UndoRedo.DefaultImpl;

[RegisterSingleton<IUndoRedoManagerFactory>]
internal class DefaultUndoRedoManagerFactory : IUndoRedoManagerFactory
{
    private readonly IServiceProvider serviceProvider;

    public DefaultUndoRedoManagerFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    
    public IUndoRedoManager Create()
    {
        return serviceProvider.Resolve<DefaultUndoRedoManager>();
    }
}