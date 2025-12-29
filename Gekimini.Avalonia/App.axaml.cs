using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Framework;
using Gekimini.Avalonia.Framework.Languages;
using Gekimini.Avalonia.Framework.Themes;
using Gekimini.Avalonia.Models.Events;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Modules.MainView;
using Gekimini.Avalonia.Platforms.Services.MainWindow;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Gekimini.Avalonia.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace Gekimini.Avalonia;

public abstract class App : Application
{
    private ILogger<App> logger;
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

        logger = ServiceProvider.GetService<ILogger<App>>();

        BindingPlugins.DataValidators.Clear();

        ServiceProvider.GetService<IThemeManager>().Initalize();
        ServiceProvider.GetService<ILanguageManager>().Initialize();

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

        RestoreMainWindowLocationAndSize();

        base.OnFrameworkInitializationCompleted();
    }

    private void RestoreMainWindowLocationAndSize()
    {
        var platformMainWindow = ServiceProvider.GetService<IPlatformMainWindow>();
        var setting = ServiceProvider.GetService<ISettingManager>().GetSetting(GekiminiSetting.JsonTypeInfo);

        platformMainWindow.WindowRect = new Rect(new Point(setting.MainWindowRectTop, setting.MainWindowRectTop),
            new Size(setting.MainWindowRectWidth, setting.MainWindowRectHeight));
        platformMainWindow.IsFullScreen = setting.IsFullScreen;
    }

    private void SaveMainWindowLocationAndSize()
    {
        var platformMainWindow = ServiceProvider.GetService<IPlatformMainWindow>();
        var settingManager = ServiceProvider.GetService<ISettingManager>();
        var setting = settingManager.GetSetting(GekiminiSetting.JsonTypeInfo);

        if (platformMainWindow.WindowRect is { } rect)
        {
            setting.MainWindowRectTop = rect.Top;
            setting.MainWindowRectLeft = rect.Left;
            setting.MainWindowRectWidth = rect.Width;
            setting.MainWindowRectHeight = rect.Height;
        }

        setting.IsFullScreen = platformMainWindow.IsFullScreen;

        settingManager.SaveSetting(setting, GekiminiSetting.JsonTypeInfo);
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
    ///     check if Application can exit safety
    /// </summary>
    /// <returns></returns>
    public virtual async Task<bool> CanExit()
    {
        //raise event to ask registered handlers if we could exit safety,
        await using var itor = WeakReferenceMessenger.Default.Send<ApplicationAskQuitEvent>().GetAsyncEnumerator();
        while (await itor.MoveNextAsync())
        {
            var canExit = itor.Current;
            //for example Shell not allow Application quit because user canceled SaveAllDialog when there are some documents not saved.
            if (!canExit)
                return false;
        }

        return true;
    }

    /// <summary>
    ///     Application try to exit
    /// </summary>
    /// <returns></returns>
    public virtual async Task<bool> TryExit()
    {
        logger.LogInformationEx("Begin.");

        var canExit = await CanExit();
        logger.LogInformationEx($"CanExit() canExit: {canExit}");
        if (!canExit)
            return false;

        await PrepareExit();
        logger.LogInformationEx("PrepareExit() pass");

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
        WeakReferenceMessenger.Default.Send<ApplicationQuitEvent>();

        SaveMainWindowLocationAndSize();

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Actual exit without any confirming/requesting/waiting.
    /// </summary>
    /// <param name="exitCode"></param>
    protected abstract void DoExit(int exitCode = 0);
}