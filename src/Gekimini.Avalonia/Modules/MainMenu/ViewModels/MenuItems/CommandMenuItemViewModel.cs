using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Input;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.Languages;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Modules.MainMenu.ViewModels.MenuItems;

public class CommandMenuItemViewModel : StandardMenuItemViewModel, ICommandUiItem
{
    private readonly Command _command;
    private readonly KeyGesture _keyGesture;
    private readonly List<StandardMenuItemViewModel> _listItems;
    private readonly StandardMenuItemViewModel _parent;
    private readonly IServiceProvider _serviceProvider;

    public CommandMenuItemViewModel(Command command, StandardMenuItemViewModel parent, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _command = command;
        _keyGesture = _serviceProvider.GetService<ICommandKeyGestureService>()
            .GetPrimaryKeyGesture(_command.CommandDefinition);
        _parent = parent;

        _listItems = new List<StandardMenuItemViewModel>();

        _command.PropertyChanged += CommandPropertyChanged;
    }

    public override LocalizedString Text => _command.Text;

    public override Uri IconSource => _command.IconSource;

    public override string InputGestureText => _keyGesture == null
        ? string.Empty
        : _keyGesture.ToString();

    public override ICommand Command => _serviceProvider.GetService<ICommandService>().GetTargetableCommand(_command);

    public override bool IsChecked => _command.Checked;

    public override bool IsVisible => _command.Visible;

    private bool IsListItem { get; set; }

    CommandDefinitionBase ICommandUiItem.CommandDefinition => _command.CommandDefinition;

    void ICommandUiItem.Update(CommandHandlerWrapper commandHandler)
    {
        if (_command != null && _command.CommandDefinition.IsList && !IsListItem)
        {
            foreach (var listItem in _listItems)
                _parent.Children.Remove(listItem);

            _listItems.Clear();

            var listCommands = new List<Command>();
            commandHandler.Populate(_command, listCommands);

            _command.Visible = false;

            var startIndex = _parent.Children.IndexOf(this) + 1;

            foreach (var command in listCommands)
            {
                var newMenuItem = new CommandMenuItemViewModel(command, _parent, _serviceProvider)
                {
                    IsListItem = true
                };
                _parent.Children.Insert(startIndex++, newMenuItem);
                _listItems.Add(newMenuItem);
            }
        }
    }

    private void CommandPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_command.Visible):
                OnPropertyChanged(nameof(IsVisible));
                break;

            case nameof(_command.Checked):
                OnPropertyChanged(nameof(IsChecked));
                break;

            case nameof(_command.Text):
            case nameof(_command.IconSource):
                OnPropertyChanged(e.PropertyName);
                break;
        }
    }
}