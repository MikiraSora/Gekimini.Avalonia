using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Dock.Model.Controls;
using Gekimini.Avalonia.Framework.Documents;

namespace Gekimini.Avalonia.Framework;

public interface IModule
{
    IEnumerable<ResourceDictionary> GlobalResourceDictionaries { get; }
    IEnumerable<IDocument> DefaultDocuments { get; }
    IEnumerable<Type> DefaultTools { get; }

    void PreInitialize();
    void Initialize();
    Task PostInitializeAsync();
}