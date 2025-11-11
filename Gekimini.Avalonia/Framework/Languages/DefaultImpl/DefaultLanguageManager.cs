using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Threading;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Injectio.Attributes;

namespace Gekimini.Avalonia.Framework.Languages.DefaultImpl;

[RegisterSingleton<ILanguageManager>]
internal class DefaultLanguageManager : ILanguageManager
{
    private readonly Dictionary<int, TranslationSource> cachedSources = new();
    private readonly ISettingManager settingManager;
    private readonly GekiminiSetting settings;
    private List<string> cachedAvaliableLanguages;

    public DefaultLanguageManager(ISettingManager settingManager)
    {
        this.settingManager = settingManager;
        settings = settingManager?.GetSetting(GekiminiSetting.JsonTypeInfo);
    }

    public IEnumerable<string> GetAvaliableLanguageNames()
    {
        if (cachedAvaliableLanguages is null)
            cachedAvaliableLanguages = new List<string>();
        var rm = new ResourceManager(typeof(Resources));

        var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        foreach (var culture in cultures)
        {
            var rs = rm.GetResourceSet(culture, true, false);
            if (rs != null)
                cachedAvaliableLanguages.Add(culture.Name);
        }

        return cachedAvaliableLanguages;
    }

    public string GetCurrentLanguage()
    {
        return settings.LanguageCode;
    }

    public INotifyPropertyChanged GetTranslationSource(Func<string, CultureInfo, string> callback)
    {
        var key = callback.GetHashCode();
        if (!cachedSources.TryGetValue(key, out var source))
            cachedSources[key] =
                source = new TranslationSource(key => callback(key, Thread.CurrentThread.CurrentUICulture));
        return source;
    }

    public INotifyPropertyChanged GetTranslationSource(string resKey)
    {
        var key = resKey.GetHashCode();
        if (!cachedSources.TryGetValue(key, out var source))
            cachedSources[key] =
                source = new TranslationSource(key => GetTranslatedText(key, Thread.CurrentThread.CurrentUICulture));
        return source;
    }

    public void SetLanguage(string languageName)
    {
        var culture = string.IsNullOrWhiteSpace(languageName)
            ? CultureInfo.InvariantCulture
            : CultureInfo.GetCultureInfo(languageName);
        var uiCulture = string.IsNullOrWhiteSpace(languageName)
            ? CultureInfo.InvariantCulture
            : CultureInfo.GetCultureInfo(languageName);

        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = uiCulture;

        settings.LanguageCode = languageName;
        settingManager.SaveSetting(settings, GekiminiSetting.JsonTypeInfo);

        foreach (var source in cachedSources.Values)
            source.Refresh();
    }

    private string GetTranslatedText(string resKey, CultureInfo culture)
    {
        return Resources.ResourceManager.GetString(resKey, culture);
    }
}