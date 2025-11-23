using System;
using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Input;
using Gekimini.Avalonia.Framework.Commands;
using Gekimini.Avalonia.Framework.ToolBars;
using Microsoft.Extensions.DependencyInjection;

namespace Gekimini.Avalonia.Modules.ToolBars.ViewModels;

public class CommandToolBarItemViewModel : ToolBarItemViewModelBase, ICommandUiItem
{
    private readonly Command _command;
    private readonly ToolBarItemDefinition _toolBarItem;

    public CommandToolBarItemViewModel(ToolBarItemDefinition toolBarItem, Command command)
    {
        _toolBarItem = toolBarItem;
        _command = command;
    }

    public KeyGesture KeyGesture => field ??= (App.Current as App)?.ServiceProvider
        .GetService<ICommandKeyGestureService>().GetPrimaryKeyGesture(_command.CommandDefinition);

    public string Text => TrimMnemonics(_command.Text);

    public ToolBarItemDisplay Display => _toolBarItem.Display;

    public Uri IconSource => _command.IconSource;

    public string ToolTip
    {
        get
        {
            var inputGestureText = KeyGesture != null
                ? string.Format(" ({0})", KeyGesture.ToString())
                : string.Empty;

            return string.Format("{0}{1}", _command.ToolTip, inputGestureText).Trim();
        }
    }

    public bool HasToolTip => !string.IsNullOrWhiteSpace(ToolTip);

    public ICommand Command => (App.Current as App)?.ServiceProvider.GetService<ICommandService>()
        .GetTargetableCommand(_command);

    public bool IsChecked => _command.Checked;

    public bool Visibility => _command.Visible;

    CommandDefinitionBase ICommandUiItem.CommandDefinition => _command.CommandDefinition;

    void ICommandUiItem.Update(CommandHandlerWrapper commandHandler)
    {
        // TODO?
    }

    public override void OnViewAfterLoaded(Control view)
    {
        base.OnViewAfterLoaded(view);
        _command.PropertyChanged += OnCommandPropertyChanged;
    }

    public override void OnViewBeforeUnload(Control view)
    {
        base.OnViewBeforeUnload(view);
        _command.PropertyChanged -= OnCommandPropertyChanged;
    }

    private void OnCommandPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Framework.Commands.Command.Text):
                OnPropertyChanged(nameof(Text));
                break;
            case nameof(Framework.Commands.Command.IconSource):
                OnPropertyChanged(nameof(IconSource));
                break;
            case nameof(Framework.Commands.Command.ToolTip):
                OnPropertyChanged(nameof(ToolTip));
                OnPropertyChanged(nameof(HasToolTip));
                break;
            case nameof(Framework.Commands.Command.Checked):
                OnPropertyChanged(nameof(IsChecked));
                break;
            case nameof(Framework.Commands.Command.Visible):
                OnPropertyChanged(nameof(Visibility));
                break;
        }

        base.OnPropertyChanged(e);
    }

    /// <summary>
    ///     Remove mnemonics underscore used by menu from text.
    ///     Also replace escaped/double underscores by a single underscore.
    ///     Displayed text will be the same than with a menu item.
    /// </summary>
    private static string TrimMnemonics(string text)
    {
        var resultArray = new char[text.Length];

        var resultLength = 0;
        var previousWasUnderscore = false;
        var mnemonicsFound = false;

        for (var textIndex = 0; textIndex < text.Length; textIndex++)
        {
            var c = text[textIndex];

            if (c == '_')
            {
                if (!previousWasUnderscore)
                {
                    // If previous character was not an underscore but the current is one, we set the flag.
                    previousWasUnderscore = true;

                    // Also, if mnemonics mark was not found yet, we also skip that underscore in result.
                    if (!mnemonicsFound)
                        continue;
                }
                else
                {
                    // If both current and previous character are underscores, it is an escaped underscore.
                    // We will include that second underscore in result and restore the flag.
                    previousWasUnderscore = false;

                    // If mnemonics mark was already found, previous underscore was included in result so we can escape this one.
                    if (mnemonicsFound)
                        continue;
                }
            }
            else
            {
                // If previous character was an underscore and the current is not one, we found the mnemonics mark.
                // We will stop to search and include all the following characters, except escaped underscores, in result.
                if (!mnemonicsFound && previousWasUnderscore)
                    mnemonicsFound = true;

                previousWasUnderscore = false;
            }

            resultArray[resultLength++] = c;
        }

        // If last character was an underscore and mnemonics mark was not found, it should be included in result.
        if (previousWasUnderscore && !mnemonicsFound)
            resultArray[resultLength++] = '_';

        return new string(resultArray, 0, resultLength);
    }
}