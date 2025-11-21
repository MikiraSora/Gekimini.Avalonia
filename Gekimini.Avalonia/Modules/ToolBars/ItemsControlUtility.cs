using Avalonia.Controls;

namespace Gekimini.Avalonia.Modules.ToolBars;

public static class ItemsControlUtility
{
    public static void UpdateSeparatorsVisibility(ItemsControl itemsControl)
    {
        Separator lastSeparator = null;
        var foundItemsBefore = false;
        var foundItemsAfter = false;

        var itemsCount = itemsControl.Items.Count;
        for (var i = 0; i < itemsCount; i++)
        {
            var container = itemsControl.ContainerFromIndex(i);
            switch (container)
            {
                case Separator newSeparator:
                    if (lastSeparator != null)
                    {
                        lastSeparator.IsVisible = foundItemsBefore && foundItemsAfter;

                        // If last separator is not visible, items found before it should still be considered as item found before next separator.
                        if (!lastSeparator.IsVisible)
                        {
                            foundItemsBefore = foundItemsBefore || foundItemsAfter;
                            foundItemsAfter = false;
                            lastSeparator = newSeparator;
                            break;
                        }
                    }

                    foundItemsBefore = foundItemsAfter;
                    foundItemsAfter = false;
                    lastSeparator = newSeparator;
                    break;

                case Control uiElement when uiElement.IsVisible:
                    foundItemsAfter = true;
                    break;
            }
        }

        if (lastSeparator != null)
            lastSeparator.IsVisible = foundItemsBefore && foundItemsAfter;
    }
}