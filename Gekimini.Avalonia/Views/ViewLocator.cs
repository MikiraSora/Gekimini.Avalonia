using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Gekimini.Avalonia.Utils;
using Gekimini.Avalonia.ViewModels;
using Injectio.Attributes;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Views;

[RegisterSingleton<ViewLocator>]
public class ViewLocator : IDataTemplate
{
    private readonly ILogger<ViewLocator> logger;
    private readonly IServiceProvider serviceProvider;

    public ViewLocator(ILogger<ViewLocator> logger, IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
    }

    public Control Build(object viewModelObject)
    {
        if (viewModelObject is not IViewModel viewModel)
        {
            logger.LogErrorEx($"viewModel is null: {new Exception().StackTrace}");
            return null;
        }

        logger.LogDebugEx($"viewModel fullName: {viewModel.GetType().FullName}");
        var viewTypeName = GetViewTypeName(viewModel.GetType());
        logger.LogDebugEx($"viewTypeName: {viewTypeName}");

        //create new view
        var view = CreateView(viewTypeName);
        if (view == null)
        {
            var msg = $"<resolve type object {viewTypeName} failed; model type:{viewModel.GetType().FullName}>";
#if DEBUG
            throw new Exception(msg);
#else
				return new TextBlock { Text = msg };
#endif
        }

        if (view is Control control)
        {
            control.Loaded += (a, aa) => { viewModel.OnViewAfterLoaded(control); };
            control.Unloaded += (a, aa) =>
            {
                viewModel.OnViewBeforeUnload(control);
                control.DataContext = null;
            };

            control.DataContext = viewModel;
            return control;
        }

        {
            //todo log it
            var msg = $"<resolve type object {viewTypeName} is not Control; model type:{viewModel.GetType().FullName}>";
#if DEBUG
            throw new Exception(msg);
#else
            return new TextBlock { Text = msg };
#endif
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Match(object data)
    {
        return data is IViewModel;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object Create(object viewModel)
    {
        return Build(viewModel);
    }

    private string GetViewTypeName(Type viewModelType)
    {
        if (viewModelType is null)
            return null;
        var name = string.Join(".", viewModelType.FullName.Split(".").Select(x =>
        {
            if (x == "ViewModels")
                return "Views";
            if (x.Length > "ViewModel".Length && x.EndsWith("ViewModel"))
                return x.Substring(0, x.Length - "Model".Length);
            return x;
        }));
        return name;
    }


    public IView CreateView(string fullClassName)
    {
        if (!TypeCollectedActivatorHelper<IView>.TryCreateInstance(serviceProvider, fullClassName, out var instance))
        {
            logger.LogErrorEx($"failed creating view for {fullClassName}");
            return default;
        }

        return instance;
    }
}