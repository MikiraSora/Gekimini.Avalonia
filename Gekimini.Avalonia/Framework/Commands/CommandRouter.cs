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
    private static readonly Type CommandHandlerInterfaceType = typeof(ICommandHandler<>);
    private static readonly Type CommandListHandlerInterfaceType = typeof(ICommandListHandler<>);
    private readonly Dictionary<Type, HashSet<Type>> _commandHandlerTypeToCommandDefinitionTypesLookup;

    private readonly Dictionary<Type, CommandHandlerWrapper> _globalCommandHandlerWrappers;
    private readonly IServiceProvider _serviceProvider;
    private readonly ViewLocator viewLocator;

    public CommandRouter(IEnumerable<ICommandHandler> globalCommandHandlers, ViewLocator viewLocator,
        IServiceProvider serviceProvider)
    {
        this.viewLocator = viewLocator;
        _serviceProvider = serviceProvider;
        _commandHandlerTypeToCommandDefinitionTypesLookup = new Dictionary<Type, HashSet<Type>>();
        _globalCommandHandlerWrappers = BuildCommandHandlerWrappers(globalCommandHandlers.ToArray());
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
        if (!_globalCommandHandlerWrappers.TryGetValue(commandDefinition.GetType(), out commandHandler))
            return null;

        return commandHandler;
    }

    private Dictionary<Type, CommandHandlerWrapper> BuildCommandHandlerWrappers(ICommandHandler[] commandHandlers)
    {
        var commandHandlersList = SortCommandHandlers(commandHandlers);

        // Command handlers are either ICommandHandler<T> or ICommandListHandler<T>.
        // We need to extract T, and use it as the key in our dictionary.

        var result = new Dictionary<Type, CommandHandlerWrapper>();

        foreach (var commandHandler in commandHandlersList)
        {
            var commandHandlerType = commandHandler.GetType();
            EnsureCommandHandlerTypeToCommandDefinitionTypesPopulated(commandHandlerType);
            var commandDefinitionTypes = _commandHandlerTypeToCommandDefinitionTypesLookup[commandHandlerType];
            foreach (var commandDefinitionType in commandDefinitionTypes)
                result[commandDefinitionType] = CreateCommandHandlerWrapper(commandDefinitionType, commandHandler);
        }

        return result;
    }

    private static List<ICommandHandler> SortCommandHandlers(ICommandHandler[] commandHandlers)
    {
        return commandHandlers
            //.OrderBy(h => bootstrapper.PriorityAssemblies.Contains(h.GetType().Assembly) ? 1 : 0)
            .ToList();
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
            if (visualObject != null)
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

    private bool IsCommandHandlerForCommandDefinitionType(
        object commandHandler, Type commandDefinitionType)
    {
        var commandHandlerType = commandHandler.GetType();
        EnsureCommandHandlerTypeToCommandDefinitionTypesPopulated(commandHandlerType);
        var commandDefinitionTypes = _commandHandlerTypeToCommandDefinitionTypesLookup[commandHandlerType];
        return commandDefinitionTypes.Contains(commandDefinitionType);
    }

    private void EnsureCommandHandlerTypeToCommandDefinitionTypesPopulated(Type commandHandlerType)
    {
        if (!_commandHandlerTypeToCommandDefinitionTypesLookup.ContainsKey(commandHandlerType))
        {
            var commandDefinitionTypes = _commandHandlerTypeToCommandDefinitionTypesLookup[commandHandlerType] =
                new HashSet<Type>();

            foreach (var handledCommandDefinitionType in GetAllHandledCommandedDefinitionTypes(commandHandlerType,
                         CommandHandlerInterfaceType))
                commandDefinitionTypes.Add(handledCommandDefinitionType);

            foreach (var handledCommandDefinitionType in GetAllHandledCommandedDefinitionTypes(commandHandlerType,
                         CommandListHandlerInterfaceType))
                commandDefinitionTypes.Add(handledCommandDefinitionType);
        }
    }

    private static IEnumerable<Type> GetAllHandledCommandedDefinitionTypes(
        Type type, Type genericInterfaceType)
    {
        var result = new List<Type>();

        while (type != null)
        {
            result.AddRange(type.GetInterfaces()
                .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterfaceType)
                .Select(x => x.GetGenericArguments().First()));

            type = type.BaseType;
        }

        return result;
    }
}