using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Modules.Window.ViewModels;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Modules.Settings.ViewModels;

[RegisterTransient<SettingsViewModel>]
public partial class SettingsViewModel : WindowViewModelBase
{
    private readonly IEnumerable<ISettingsEditor> _settingsEditors;

    public SettingsViewModel(IEnumerable<ISettingsEditor> settingsEditors)
    {
        Title = Resources.SettingsDisplayName;
        DefaultWidth = 1000;
        DefaultHeight = 600;

        var pages = new List<SettingsPageViewModel>();
        _settingsEditors = settingsEditors;

        foreach (var settingsEditor in _settingsEditors)
        {
            var parentCollection = GetParentCollection(settingsEditor, pages);

            var page = parentCollection.FirstOrDefault(m => m.Name == settingsEditor.SettingsPageName);

            if (page == null)
            {
                page = new SettingsPageViewModel
                {
                    Name = settingsEditor.SettingsPageName
                };
                parentCollection.Add(page);
            }

            page.Editors.Add(settingsEditor);
        }

        Pages = pages;
        SelectedPage = GetFirstLeafPageRecursive(pages);
    }

    public List<SettingsPageViewModel> Pages { get; internal set; }

    [ObservableProperty]
    public partial SettingsPageViewModel SelectedPage { get; set; }

    private static SettingsPageViewModel GetFirstLeafPageRecursive(List<SettingsPageViewModel> pages)
    {
        if (!pages.Any())
            return null;

        var firstPage = pages.First();
        if (!firstPage.Children.Any())
            return firstPage;

        return GetFirstLeafPageRecursive(firstPage.Children);
    }

    private List<SettingsPageViewModel> GetParentCollection(ISettingsEditor settingsEditor,
        List<SettingsPageViewModel> pages)
    {
        if (string.IsNullOrEmpty(settingsEditor.SettingsPagePath))
            return pages;

        var path = settingsEditor.SettingsPagePath.Split(new[] {'\\'}, StringSplitOptions.RemoveEmptyEntries);

        foreach (var pathElement in path)
        {
            var page = pages.FirstOrDefault(s => s.Name == pathElement);

            if (page == null)
            {
                page = new SettingsPageViewModel {Name = pathElement};
                pages.Add(page);
            }

            pages = page.Children;
        }

        return pages;
    }

    [RelayCommand]
    private async Task SaveChanges()
    {
        foreach (var settingsEditor in _settingsEditors)
            settingsEditor.ApplyChanges();

        await TryCloseAsync(true);
    }

    [RelayCommand]
    private Task Cancel()
    {
        return TryCloseAsync(false);
    }
}