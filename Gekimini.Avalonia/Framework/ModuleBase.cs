using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Dock.Model.Controls;

namespace Gekimini.Avalonia.Framework;

public abstract class ModuleBase : IModule
{
    public virtual IEnumerable<ResourceDictionary> GlobalResourceDictionaries
    {
        get { yield break; }
    }

    public virtual IEnumerable<IDocument> DefaultDocuments
    {
        get { yield break; }
    }

    public virtual IEnumerable<Type> DefaultTools
    {
        get { yield break; }
    }

    public virtual void PreInitialize()
    {
    }

    public virtual void Initialize()
    {
    }

    public virtual Task PostInitializeAsync()
    {
        return Task.CompletedTask;
    }
}