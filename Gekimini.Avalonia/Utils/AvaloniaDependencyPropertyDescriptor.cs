using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;

namespace Gekimini.Avalonia.Utils;

internal class AvaloniaDependencyPropertyDescriptor
{
    private static readonly Dictionary<(Type, AvaloniaProperty), AvaloniaDependencyPropertyDescriptor>
        cachedDescriptorMap = new();

    private readonly Dictionary<Control, List<Action<object, EventArgs>>> registeredEventMap = new();
    private bool isRegisterGlobal;

    private readonly AvaloniaProperty property;
    private int referenceCount;
    private IDisposable registerHandler;
    private Type type;

    public AvaloniaDependencyPropertyDescriptor(AvaloniaProperty property, Type type)
    {
        this.property = property;
        this.type = type;
    }

    internal static AvaloniaDependencyPropertyDescriptor FromProperty(AvaloniaProperty property, Type type)
    {
        var key = (type, property);
        if (!cachedDescriptorMap.TryGetValue(key, out var descriptor))
            cachedDescriptorMap[key] = descriptor = new AvaloniaDependencyPropertyDescriptor(property, type);
        return descriptor;
    }

    public void AddValueChanged<T>(T element, Action<object, EventArgs> handler) where T : Control
    {
        if (!registeredEventMap.TryGetValue(element, out var list))
            list = registeredEventMap[element] = new List<Action<object, EventArgs>>();

        list.Add(handler);
        referenceCount++;

        if (!isRegisterGlobal)
        {
            registerHandler = property.Changed.AddClassHandler<T>(OnPropertyValueChanged);
            isRegisterGlobal = true;
        }
    }

    private void OnPropertyValueChanged<T>(T sender, AvaloniaPropertyChangedEventArgs e) where T : Control
    {
        if (registeredEventMap.TryGetValue(sender, out var list))
            foreach (var callback in list)
                callback?.Invoke(sender, e);
    }

    public void RemoveValueChanged(Control element, Action<object, EventArgs> handler)
    {
        if (!registeredEventMap.TryGetValue(element, out var list))
            return;

        list.Remove(handler);
        referenceCount--;

        if (isRegisterGlobal && referenceCount <= 0)
        {
            registerHandler?.Dispose();
            isRegisterGlobal = false;
        }
    }
}