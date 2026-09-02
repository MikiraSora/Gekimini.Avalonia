using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ToolBar.Utils;

namespace Avalonia.Controls.ToolBar.Controls;

public partial class ToolBarOverflowPanel : Panel
{
    private double _wrapWidth;
    private Size _panelSize;

    private ToolBar? ToolBar => TemplatedParent as ToolBar;
    private ToolBarPanel? ToolBarPanel => ToolBar?.ToolBarPanel;

    protected override Size MeasureOverride(Size constraint)
    {
        var curLineSize = new Size();
        _panelSize = new Size();
        _wrapWidth = double.IsNaN(WrapWidth) ? constraint.Width : WrapWidth;
        var childrenCount = Children.Count;

        var toolBarPanel = ToolBarPanel;
        if (toolBarPanel != null)
        {
            var generatedItemsCollection = toolBarPanel.GeneratedItemsCollection;
            var generatedItemsCount = generatedItemsCollection?.Count ?? 0;
            var childrenIndex = 0;
            for (var i = 0; i < generatedItemsCount; i++)
            {
                var child = generatedItemsCollection?[i];
                if (child != null && ToolBar.GetIsOverflowItem(child) && child is not Separator)
                {
                    if (childrenIndex < childrenCount)
                    {
                        if (Children[childrenIndex] != child)
                        {
                            Children.Insert(childrenIndex, child);
                            childrenCount++;
                        }
                    }
                    else
                    {
                        Children.Add(child);
                        childrenCount++;
                    }

                    childrenIndex++;
                }
            }
        }

        for (var i = 0; i < childrenCount; i++)
        {
            var child = Children[i];
            child.Measure(constraint);
            var childDesiredSize = child.DesiredSize;
            if (DoubleUtil.GreaterThan(childDesiredSize.Width, _wrapWidth))
                _wrapWidth = childDesiredSize.Width;
        }

        _wrapWidth = Math.Min(_wrapWidth, constraint.Width);

        foreach (var child in Children)
        {
            var size = child.DesiredSize;
            if (DoubleUtil.GreaterThan(curLineSize.Width + size.Width, _wrapWidth))
            {
                _panelSize = _panelSize.WithWidth(Math.Max(curLineSize.Width, _panelSize.Width));
                _panelSize = _panelSize.WithHeight(_panelSize.Height + curLineSize.Height);
                curLineSize = size;

                if (DoubleUtil.GreaterThan(size.Width, _wrapWidth))
                {
                    _panelSize = _panelSize.WithWidth(Math.Max(size.Width, _panelSize.Width));
                    _panelSize = _panelSize.WithHeight(_panelSize.Height + size.Height);
                    curLineSize = new Size();
                }
            }
            else
            {
                curLineSize = curLineSize.WithWidth(curLineSize.Width + size.Width);
                curLineSize = curLineSize.WithHeight(Math.Max(size.Height, curLineSize.Height));
            }
        }

        _panelSize = _panelSize.WithWidth(Math.Max(curLineSize.Width, _panelSize.Width));
        _panelSize = _panelSize.WithHeight(_panelSize.Height + curLineSize.Height);
        return _panelSize;
    }

    protected override Size ArrangeOverride(Size arrangeBounds)
    {
        var firstInLine = 0;
        var curLineSize = new Size();
        var accumulatedHeight = 0d;
        _wrapWidth = Math.Min(_wrapWidth, arrangeBounds.Width);

        for (var i = 0; i < Children.Count; i++)
        {
            var size = Children[i].DesiredSize;
            if (DoubleUtil.GreaterThan(curLineSize.Width + size.Width, _wrapWidth))
            {
                ArrangeLine(accumulatedHeight, curLineSize.Height, firstInLine, i);
                accumulatedHeight += curLineSize.Height;
                firstInLine = i;
                curLineSize = size;
            }
            else
            {
                curLineSize = curLineSize.WithWidth(curLineSize.Width + size.Width);
                curLineSize = curLineSize.WithHeight(Math.Max(size.Height, curLineSize.Height));
            }
        }

        ArrangeLine(accumulatedHeight, curLineSize.Height, firstInLine, Children.Count);
        return _panelSize;
    }

    private void ArrangeLine(double y, double lineHeight, int start, int end)
    {
        double x = 0;
        for (var i = start; i < end; i++)
        {
            var child = Children[i];
            child.Arrange(new Rect(x, y, child.DesiredSize.Width, lineHeight));
            x += child.DesiredSize.Width;
        }
    }
}
