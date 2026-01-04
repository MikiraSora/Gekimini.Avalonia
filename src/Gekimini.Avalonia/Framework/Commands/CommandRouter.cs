using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Gekimini.Avalonia.Modules.Shell;
using Gekimini.Avalonia.Utils.MethodExtensions;
using Gekimini.Avalonia.Views;
using Injectio.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Framework.Commands;

[RegisterSingleton<ICommandRouter>]
public class CommandRouter : ICommandRouter
{
    private readonly IServiceProvider _serviceProvider;

    private readonly Dictionary<Type, CommandHandlerWrapper> cachedCommandDefinitionToGloablHandlerMap = new();
    private readonly Dictionary<Type, HashSet<Type>> cachedHandlerSupportDefinitionTypesMap = new();
    private readonly ICommandHandler[] globalCommandHandlers;
    private readonly ViewLocator viewLocator;

    public CommandRouter(IEnumerable<ICommandHandler> globalCommandHandlers, ViewLocator viewLocator,
        IServiceProvider serviceProvider)
    {
        this.globalCommandHandlers = globalCommandHandlers.ToArray();
        this.viewLocator = viewLocator;
        _serviceProvider = serviceProvider;
    }

    public CommandHandlerWrapper GetCommandHandler(CommandDefinitionBase commandDefinition)
    {
        CommandHandlerWrapper commandHandler;

        var shell = _serviceProvider.GetService<IShell>();
        var activeItemViewModel = shell.ActiveDockable;
        if (activeItemViewModel != null)
        {
            commandHandler = GetCommandHandlerForLayoutItem(commandDefinition, activeItemViewModel);
            if (commandHandler != null)
                return commandHandler;
        }

        var activeDocumentViewModel = shell.ActiveDocument;
        if (activeDocumentViewModel != null && activeDocumentViewModel != activeItemViewModel)
        {
            commandHandler = GetCommandHandlerForLayoutItem(commandDefinition, activeDocumentViewModel);
            if (commandHandler != null)
                return commandHandler;
        }

        // If none of the objects in the DataContext hierarchy handle the command,
        // fallback to the global handler.
        if (!TryGetGlobalCommandHandler(commandDefinition.GetType(), out commandHandler))
            return null;

        return commandHandler;
    }

    private bool TryGetGlobalCommandHandler(Type commandDefinitionType, out CommandHandlerWrapper commandHandler)
    {
        if (cachedCommandDefinitionToGloablHandlerMap.TryGetValue(commandDefinitionType, out commandHandler))
            return true;

        var handler = globalCommandHandlers.FirstOrDefault(x => x.SupportCommandDefinitionTypes.Contains(commandDefinitionType));
        if (handler is null)
            return false;
        var wrapper = CreateCommandHandlerWrapper(commandDefinitionType, handler);
        cachedCommandDefinitionToGloablHandlerMap[commandDefinitionType] = wrapper;

        commandHandler = wrapper;
        return true;
    }

    private bool IsCommandHandlerForCommandDefinitionType(object obj, Type commandDefinitionType)
    {
        var handlerType = obj.GetType();
        if (cachedHandlerSupportDefinitionTypesMap.TryGetValue(handlerType, out var supports))
            return supports.Contains(commandDefinitionType);

        if (obj is not ICommandHandler commandHandler)
            return false;

        var s = commandHandler.SupportCommandDefinitionTypes.ToHashSet();
        cachedHandlerSupportDefinitionTypesMap[handlerType] = s;
        return s.Contains(commandDefinitionType);
    }

    private CommandHandlerWrapper GetCommandHandlerForLayoutItem(CommandDefinitionBase commandDefinition,
        object activeItemViewModel)
    {
        var activeItemView = viewLocator.LocateForModel(activeItemViewModel) as Control;
        if (activeItemView == null)
            return null;

        var startElement = activeItemView.GetFocusedElement() ?? activeItemView;

        // First, we look at the currently focused element, and iterate up through
        // the tree, giving each DataContext a chance to handle the command.
        return FindCommandHandlerInVisualTree(commandDefinition, startElement);
    }

    private CommandHandlerWrapper FindCommandHandlerInVisualTree(CommandDefinitionBase commandDefinition,
        IInputElement target)
    {
        var visualObject = target as Visual;
        if (visualObject == null)
            return null;

        object previousDataContext = null;
        do
        {
            var dataContext = visualObject.DataContext;
            if (dataContext != null && !ReferenceEquals(dataContext, previousDataContext))
            {
                if (dataContext is ICommandRerouter)
                {
                    var commandRerouter = (ICommandRerouter) dataContext;
                    var commandTarget = commandRerouter.GetHandler(commandDefinition);
                    if (commandTarget != null)
                    {
                        if (IsCommandHandlerForCommandDefinitionType(commandTarget, commandDefinition.GetType()))
                            return CreateCommandHandlerWrapper(commandDefinition.GetType(), commandTarget);
                        throw new InvalidOperationException(
                            "This object does not handle the specified command definition.");
                    }
                }

                if (IsCommandHandlerForCommandDefinitionType(dataContext, commandDefinition.GetType()))
                    return CreateCommandHandlerWrapper(commandDefinition.GetType(), dataContext);

                previousDataContext = dataContext;
            }

            visualObject = visualObject.GetVisualParent();
        } while (visualObject != null);

        return null;
    }

    private static CommandHandlerWrapper CreateCommandHandlerWrapper(
        Type commandDefinitionType, object commandHandler)
    {
        if (commandHandler is ICommandListHandler handler)
            return CommandHandlerWrapper.FromCommandListHandler(handler);
        if (commandHandler is ICommandHandler handler2)
            return CommandHandlerWrapper.FromCommandHandler(handler2);
        throw new InvalidOperationException();
    }
}