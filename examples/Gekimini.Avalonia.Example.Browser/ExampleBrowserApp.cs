using Gekimini.Avalonia.Example.Browser.Utils;
using Gekimini.Avalonia.Example.Browser.Utils.Interops;
using Gekimini.Avalonia.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Example.Browser;

public class ExampleBrowserApp : ExampleApp
{
    private ILogger<ExampleBrowserApp> logger;

    protected override void RegisterServices(IServiceCollection serviceCollection)
    {
        base.RegisterServices(serviceCollection);

#if LLVM_BUILD
        serviceCollection.AddGekiminiAvaloniaExampleBrowserLLVM();
#else
        serviceCollection.AddGekiminiAvaloniaExampleBrowser();
#endif

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

        logger = ServiceProvider.GetService<ILogger<ExampleBrowserApp>>();
    }

    protected override void DoExit(int exitCode = 0)
    {
        logger.LogInformationEx($"bye. exitCode={exitCode}");
        JsApplicationInterop.Exit();
    }
}