using GBX.NET.Attributes;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GbxExplorerOld.Client.Components.ValueRenderers;

public abstract class IntegerValueRenderer<T> : ValueRenderer where T : struct, IBinaryInteger<T>
{
    private bool IsHexadecimal => Property?.GetCustomAttribute<HexadecimalAttribute>(inherit: true) is not null;

    protected string? FormattedValue
    {
        get
        {
            if (Value is not T number)
            {
                return null;
            }

            return IsHexadecimal
                ? $"0x{number.ToString($"X{Unsafe.SizeOf<T>() * 2}", CultureInfo.InvariantCulture)}"
                : number.ToString();
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                SetAndUpdate(null, parent: true);
                return;
            }

            var text = value.Trim();
            var style = NumberStyles.Integer;
            var culture = CultureInfo.CurrentCulture;

            if (IsHexadecimal)
            {
                if (text.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    text = text[2..];
                }

                style = NumberStyles.HexNumber;
                culture = CultureInfo.InvariantCulture;
            }

            if (T.TryParse(text, style, culture, out var number))
            {
                SetAndUpdate(number, parent: true);
            }
        }
    }
}
