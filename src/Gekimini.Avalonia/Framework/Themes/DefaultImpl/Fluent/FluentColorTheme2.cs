using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;

namespace Gekimini.Avalonia.Framework.Themes.DefaultImpl.Fluent;

public class FluentColorTheme2<T> : IColorTheme where T : ColorPaletteResources, new()
{
    private readonly ThemeVariant overrideVariant;
    private FluentTheme cachedNewFluent;
    private WeakReference<Application> cachedApplication;

    private FluentTheme prevFluent;
    private ThemeVariant prevVariant;
    private WeakReference<Application> prevApplication;

    public FluentColorTheme2(string name, ThemeVariant overrideVariant)
    {
        this.overrideVariant = overrideVariant;
        Name = name;
    }

    public string Name { get; }

    public void ApplyColorTheme()
    {
        var app = Application.Current;
        if (app is null)
            return;

        prevFluent = app.Styles.OfType<FluentTheme>().FirstOrDefault();
        prevApplication = new WeakReference<Application>(app);
        var newFluent = GetOrCreateFluent(app);

        if (prevFluent is not null)
            app.Styles.Remove(prevFluent);
        app.Styles.Insert(0, newFluent);

        prevVariant = app.RequestedThemeVariant;
        app.RequestedThemeVariant = overrideVariant;
    }

    public void RevertColorTheme()
    {
        var app = Application.Current;
        if (app is null)
            return;

        var newFluent = GetOrCreateFluent(app);

        app.Styles.Remove(newFluent);

        // 静态主题实例可能被多个 Application 复用（headless 测试隔离、设计器预览）。
        // prevFluent 只属于应用它的那个 Application，跨应用回插会抛
        // "The Styles already has a owner."，此时跳过回插即可。
        if (prevFluent is not null &&
            prevApplication is { } prevAppRef &&
            prevAppRef.TryGetTarget(out var prevApp) &&
            ReferenceEquals(prevApp, app))
            app.Styles.Insert(0, prevFluent);

        prevFluent = default;
        app.RequestedThemeVariant = prevVariant;
        prevVariant = default;
    }

    private FluentTheme GetOrCreateFluent(Application app)
    {
        // 缓存的 FluentTheme 归属其他 Application 的 Styles 时必须重建，
        // 否则插入会抛 "The Styles already has a owner."。
        if (cachedNewFluent is not null &&
            cachedApplication is { } cachedAppRef &&
            cachedAppRef.TryGetTarget(out var cachedApp) &&
            ReferenceEquals(cachedApp, app))
            return cachedNewFluent;

        cachedNewFluent = new FluentTheme();
        cachedNewFluent.Palettes[overrideVariant] = new T();
        cachedApplication = new WeakReference<Application>(app);

        return cachedNewFluent;
    }

}