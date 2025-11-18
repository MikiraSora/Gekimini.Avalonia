using System;

namespace Gekimini.Avalonia.Models;

public class ControlSize(double value, SizeValueType sizeValueType = SizeValueType.Fixed)
{
    public double Value { get; } = value;
    public SizeValueType SizeValueType { get; } = sizeValueType;

    public static implicit operator ControlSize(double value)
    {
        return new ControlSize(value);
    }

    public static implicit operator ControlSize(string s)
    {
        if (s is null)
            throw new ArgumentNullException(nameof(s));

        s = s.Trim();

        if (s.EndsWith("%"))
        {
            var numberPart = s.Substring(0, s.Length - 1).Trim();

            if (!double.TryParse(numberPart, out var v))
                throw new FormatException($"Invalid percent value: \"{s}\"");

            return new ControlSize(v / 100.0, SizeValueType.PercentBaseOnRoot);
        }

        if (!double.TryParse(s, out var fixedValue))
            throw new FormatException($"Invalid fixed value: \"{s}\"");

        return new ControlSize(fixedValue);
    }

    public override string ToString()
    {
        return SizeValueType switch
        {
            SizeValueType.Fixed => Value.ToString(),
            SizeValueType.PercentBaseOnRoot => Value * 100 + "%",
            _ => Value.ToString()
        };
    }
}