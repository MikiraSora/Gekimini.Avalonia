using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Threading;
using Gekimini.Avalonia.Assets.Languages;
using Gekimini.Avalonia.Attributes;
using Gekimini.Avalonia.Models.Settings;
using Gekimini.Avalonia.Platforms.Services.Settings;
using Injectio.Attributes;
using Microsoft.Extensions.Logging;

namespace Gekimini.Avalonia.Framework.Languages.DefaultImpl;

[RegisterSingleton<ILanguageManager>]
public partial class DefaultLanguageManager : ILanguageManager
{
    private readonly Dictionary<int, TranslationSource> cachedSources = new();
    private List<string> cachedAvaliableLanguages;

    private string currentLanguage;

    [GetServiceLazy]
    public partial ISettingManager SettingManager { get; }

    [GetServiceLazy]
    public partial ILogger<DefaultLanguageManager> Logger { get; }

    public IEnumerable<string> GetAvaliableLanguageNames()
    {
        if (cachedAvaliableLanguages is not null)
            return cachedAvaliableLanguages;
        cachedAvaliableLanguages = new List<string>();
        var rm = new ResourceManager(typeof(Resources));

        var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        foreach (var culture in cultures)
        {
            var rs = rm.GetResourceSet(culture, true, false);
            if (rs == null)
                continue;

            cachedAvaliableLanguages.Add(string.IsNullOrWhiteSpace(culture.Name) ? "Default" : culture.Name);
            Logger.LogInformationEx($"available language found: {culture.Name}");
        }

        return cachedAvaliableLanguages;
    }

    public string GetCurrentLanguage()
    {
        return currentLanguage;
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

    public void Initalize()
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
        var uiCulture = isDefaultRequest
            ? CultureInfo.InvariantCulture
            : CultureInfo.GetCultureInfo(languageName);

        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = uiCulture;

        foreach (var source in cachedSources.Values)
            source.Refresh();

        currentLanguage = isDefaultRequest ? "Default" : languageName;

        Logger.LogInformationEx($"set current language {languageName}, cultureInfo:{culture.Name}");
    }

    private string GetTranslatedText(string resKey, CultureInfo culture)
    {
        return Resources.ResourceManager.GetString(resKey, culture);
    }
}