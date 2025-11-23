using System;
using Avalonia.Controls.ApplicationLifetimes;
using Gekimini.Avalonia.Desktop.Utils.Logging;
using Gekimini.Avalonia.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Desktop;

public class DesktopApp : App
{
    private ILogger<DesktopApp> logger;

    protected override void RegisterServices(IServiceCollection serviceCollection)
    {
        base.RegisterServices(serviceCollection);

        serviceCollection.AddGekiminiAvaloniaDesktop();

#if DEBUG
        if (DesignModeHelper.IsDesignMode)
            return;
#endif
        serviceCollection.AddLogging(o =>
        {
            o.SetMinimumLevel(LogLevel.Debug);
            o.AddProvider(new FileLoggerProvider());
            o.AddDebug();
            o.AddConsole();
        });
    }

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();

        logger = ServiceProvider.GetService<ILogger<DesktopApp>>();
    }
    
    protected override void DoExit(int exitCode = 0)
    {
        /*
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        desktop.Shutdown(exitCode);
        */
        logger.LogInformationEx("bye.");
        System.Environment.Exit(exitCode);
    }
}