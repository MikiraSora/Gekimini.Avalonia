using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Gekimini.Avalonia.Utils.MethodExtensions;

public static class ObservableCollectionEx
{
    public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
            collection.Add(item);
    }

    public static void AddRange<T>(this ObservableCollection<T> collection, params T[] items)
    {
        AddRange(collection, items.AsEnumerable());
    }
}