using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Gekimini.Avalonia.Modules.Dialogs.ViewModels.CommonMessage;
using Gekimini.Avalonia.Modules.Dialogs.Views;
using Injectio.Attributes;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Framework.Dialogs.DefaultImpl;

[RegisterSingleton<IDialogManager>]
public class DefaultDialogManager : IDialogManager
{
    private readonly Stack<Window> dialogWindowStack = new();
    private readonly ILogger<DefaultDialogManager> logger;
    private readonly IServiceProvider serviceProvider;

    public DefaultDialogManager(ILogger<DefaultDialogManager> logger, IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
    }

    public Task<T> ShowDialog<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : DialogViewModelBase
    {
        var viewModel = serviceProvider.Resolve<T>();
        return ShowDialogInternal(viewModel);
    }

    public async Task ShowDialog(DialogViewModelBase dialogViewModel)
    {
        await ShowDialogInternal(dialogViewModel);
    }

    public async Task ShowMessageDialog(string content, DialogMessageType messageType = DialogMessageType.Info)
    {
        var vm = new CommonMessageDialogViewModel(messageType, content);
        await ShowDialogInternal(vm);
    }

    public async Task<bool> ShowComfirmDialog(string content, string yesButtonContent = "确认",
        string noButtonContent = "取消")
    {
        var vm = new CommonComfirmDialogViewModel(content, yesButtonContent, noButtonContent);
        await ShowDialogInternal(vm);
        return vm.ComfirmResult;
    }

    private Task<T> ShowDialogInternal<T>(T viewModel) where T : DialogViewModelBase
    {
        return Dispatcher.UIThread.InvokeAsync(async () =>
        {
            logger.LogInformationEx($"dialog {viewModel.DialogIdentifier} started.");
            var view = new TemplateDialogView(viewModel);
            await view.ShowAsync();
            logger.LogInformationEx($"dialog {viewModel.DialogIdentifier} finished");
            return viewModel;
        });
    }
}