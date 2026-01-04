using System;
using Gekimini.Avalonia.Example.Browser.Utils;
using Gekimini.Avalonia.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Example.Browser;

public class BrowserApp : App
{
    private ILogger<BrowserApp> logger;

    protected override void RegisterServices(IServiceCollection serviceCollection)
    {
        base.RegisterServices(serviceCollection);

        serviceCollection.AddGekiminiAvaloniaExampleBrowser();

#if DEBUG
        if (DesignModeHelper.IsDesignMode)
            return;
#endif

        serviceCollection.AddLogging(o =>
        {
            o.SetMinimumLevel(LogLevel.Debug);
            o.AddProvider(new ConsoleLoggerProvider());
            o.AddDebug();
        });
    }

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();

        logger = ServiceProvider.GetService<ILogger<BrowserApp>>();
    }

    protected override void DoExit(int exitCode = 0)
    {
        logger.LogInformationEx($"bye. exitCode={exitCode}");
        Utils.Interops.JsApplicationInterop.Exit();
    }
}