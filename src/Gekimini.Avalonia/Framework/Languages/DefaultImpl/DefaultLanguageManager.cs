using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using CommunityToolkit.Mvvm.Messaging;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Models.Events;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Injectio.Attributes;
using Microsoft.Extensions.Logging;
using SimpleTypedLocalizer;

namespace Gekimini.Avalonia.Framework.Languages.DefaultImpl;

[RegisterSingleton<ILanguageManager>]
public partial class DefaultLanguageManager : ILanguageManager
{
    private List<string> cachedAvaliableLanguages;

    private string currentLanguage;

    static DefaultLanguageManager()
    {
        _ = ProgramLanguages.AdvancedSliderCommitErrorFormat;
    }

    [GetServiceLazy]
    public partial ISettingManager SettingManager { get; }

    [GetServiceLazy]
    public partial ILogger<DefaultLanguageManager> Logger { get; }

    public IEnumerable<string> GetAvaliableLanguageNames()
    {
        if (cachedAvaliableLanguages is not null)
            return cachedAvaliableLanguages;
        cachedAvaliableLanguages = new List<string>();

        var providers = ProgramLanguages.LocalizerManager.Providers;

        foreach (var culture in providers.Select(x => x.CultureInfo).DistinctBy(x => x.Name))
        {
            cachedAvaliableLanguages.Add(string.IsNullOrWhiteSpace(culture.Name) ? "Default" : culture.Name);
            Logger.LogInformationEx($"available language found: {culture.Name}");
        }

        return cachedAvaliableLanguages;
    }

    public string GetCurrentLanguage()
    {
        return currentLanguage;
    }

    public string GetTranslatedText(string resKey)
    {
        return GetTranslatedText(resKey, LocalizerManager.CurrentDefaultCultureInfo);
    }

    public void Initialize()
    {
        SetLanguage(SettingManager.GetSetting(GekiminiSetting.JsonTypeInfo).LanguageCode);
    }

    public void SetLanguage(string languageName)
    {
        var isDefaultRequest = string.IsNullOrWhiteSpace(languageName) ||
                               languageName.Equals("Default", StringComparison.OrdinalIgnoreCase);
        var culture = isDefaultRequest
            ? CultureInfo.InvariantCulture
            : CultureInfo.GetCultureInfo(languageName);
        /*
        var uiCulture = isDefaultRequest
            ? CultureInfo.InvariantCulture
            : CultureInfo.GetCultureInfo(languageName);

        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = uiCulture;
        */
        LocalizerManager.CurrentDefaultCultureInfo = culture;

        WeakReferenceMessenger.Default.Send(new CurrentCultureInfoChangedEvent(culture));

        currentLanguage = isDefaultRequest ? "Default" : languageName;

        Logger.LogInformationEx($"set current language {languageName}, cultureInfo:{culture.Name}");
    }

    private string GetTranslatedText(string resKey, CultureInfo culture)
    {
        return LocalizerManager.GetLocalizedStringGlobally(resKey, culture);
    }
}