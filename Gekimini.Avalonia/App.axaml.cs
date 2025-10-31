using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Gekimini.Avalonia.Framework.Services;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Gekimini.Avalonia.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace Gekimini.Avalonia;

public abstract class App : Application
{
    private Control mainView;
    private IServiceProvider serviceProvider;

    public IServiceProvider ServiceProvider =>
        serviceProvider ?? throw new InvalidOperationException("DI has not been initialized.");

    public TopLevel TopLevel => TopLevel.GetTopLevel(mainView) ??
                                throw new InvalidOperationException("View has not been initialized");

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        InitailizeServices();

        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        var viewLocator = ServiceProvider.GetService<ViewLocator>();
        DataTemplates.Add(viewLocator);

        var mainViewModel = ServiceProvider.GetService<IMainView>();
        mainView = ServiceProvider.GetService<ViewLocator>().Build(mainViewModel);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow
            {
                Content = mainView
            };
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            singleViewPlatform.MainView = mainView;

        base.OnFrameworkInitializationCompleted();
    }

    protected virtual void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<RecyclableMemoryStreamManager>(new RecyclableMemoryStreamManager(new RecyclableMemoryStreamManager.Options()
        {
            ThrowExceptionOnToArray = true
        }));

        serviceCollection.AddSingleton(this);

        serviceCollection.AddLogging(o => { o.SetMinimumLevel(LogLevel.Debug); });

        serviceCollection.AddTypeCollectedActivator(ViewTypeCollectedActivator.Default);

        serviceCollection.AddGekiminiAvalonia();
    }

    private void InitailizeServices()
    {
        if (serviceProvider is not null)
            throw new Exception("InitServiceProvider() has been called.");

        var serviceCollection = new ServiceCollection();

        RegisterServices(serviceCollection);

        serviceProvider = serviceCollection.BuildServiceProvider();
    }
}