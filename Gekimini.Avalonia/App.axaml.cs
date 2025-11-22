using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.Events;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Framework.Themes;
using Gekimini.Avalonia.Models.Events;
using Gekimini.Avalonia.Modules.MainView;
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

#if DEBUG
        this.AttachDeveloperTools();
#endif
    }

    public override void OnFrameworkInitializationCompleted()
    {
        InitailizeServices();

        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        ServiceProvider.GetService<IThemeManager>().Initalize();
        ServiceProvider.GetService<ILanguageManager>().Initalize();

        var viewLocator = ServiceProvider.GetService<ViewLocator>();
        DataTemplates.Add(viewLocator);

        var mainViewModel = ServiceProvider.GetService<IMainView>();
        mainView = ServiceProvider.GetService<ViewLocator>().Build(mainViewModel);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                Content = mainView
            };
            desktop.Exit += OnExit;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = mainView;
        }

        base.OnFrameworkInitializationCompleted();
    }

    protected virtual void OnExit(object sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        WeakReferenceMessenger.Default.Send(new ApplicationQuitEvent());
    }

    protected virtual void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(new RecyclableMemoryStreamManager(new RecyclableMemoryStreamManager.Options
        {
            ThrowExceptionOnToArray = true
        }));

        serviceCollection.AddSingleton(this);

        serviceCollection.AddLogging(o => { o.SetMinimumLevel(LogLevel.Debug); });

        serviceCollection.AddTypeCollectedActivator(ViewTypeCollectedActivator.Default);

        serviceCollection.AddTypeCollectedActivator(ToolViewModelTypeCollectedActivator.Default);

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

    /// <summary>
    ///     Application try to exit
    /// </summary>
    /// <returns></returns>
    public virtual async Task<bool> TryExit()
    {
        //raise event to ask registered handlers if we could exit safety,
        foreach (var askTask in serviceProvider
                     .GetService<IWeakReferenceEventManager>()
                     .SendMessageAndGetResponses(new ApplicationAskQuitEvent()))
        {
            var result = await askTask;

            //for example Shell not allow Application quit because user canceled SaveAllDialog when there are some documents not saved.
            if (!result)
                return false;
        }

        await PrepareExit();

        DoExit();
        return true;
    }

    /// <summary>
    ///     Do something to prepare application exit.
    /// </summary>
    /// <returns></returns>
    protected virtual Task PrepareExit()
    {
        //notify handlers to do something for preparing application exit. such as save log, shell layout and application settings.
        serviceProvider
            .GetService<IWeakReferenceEventManager>()
            .SendMessage(new ApplicationQuitEvent());

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Actual exit without any confirming/requesting/waiting.
    /// </summary>
    /// <param name="exitCode"></param>
    protected abstract void DoExit(int exitCode = 0);
}