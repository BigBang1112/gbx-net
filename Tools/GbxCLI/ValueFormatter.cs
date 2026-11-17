using GBX.NET;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Serialization.Chunking;
using System.Collections;
using System.Globalization;
using System.Text;

namespace GbxCLI;

/// <summary>
/// Describes values on one line, the same way in <c>inspect</c> and in the browser.
/// </summary>
internal static class ValueFormatter
{
    public const int TextLimit = 240;

    /// <summary>
    /// Describes the value without escaping. Strings are returned as they are.
    /// </summary>
    public static string Describe(object? value) => value switch
    {
        null => "(null)",
        string text => text.Length == 0 ? "\"\"" : text,
        byte[] { Length: 0 } => "0 bytes",
        byte[] bytes => $"{bytes.Length:N0} bytes (hex): {Convert.ToHexString(bytes.AsSpan(0, Math.Min(bytes.Length, 32)))}{(bytes.Length > 32 ? " ..." : "")}",
        NodeRef { Kind: NodeRefKind.None } => "(null)",
        NodeRef { Kind: NodeRefKind.Embedded } => "Embedded node",
        NodeRef reference => $"{reference.Kind}, {reference.State}: {reference.Descriptor}",
        CMwNod node => node.GetType().Name,
        IChunk chunk => $"0x{chunk.Id:X8} ({chunk.GetType().Name}){(chunk is IVersionable versionable ? $", version {versionable.Version}" : "")}",
        ICollection collection => $"{collection.Count:N0} items",
        IEnumerable => "Collection",
        Exception exception => exception.Message,
        _ => DescribeOther(value)
    };

    private static string DescribeOther(object value)
    {
        var type = value.GetType();
        var culture = CultureInfo.CurrentCulture;
        string? text;

        // Vectors and other structs format their numbers with the current culture, where a decimal comma would blur their separators.
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        try
        {
            text = Convert.ToString(value, CultureInfo.InvariantCulture);
        }
        finally
        {
            CultureInfo.CurrentCulture = culture;
        }

        // Without its own ToString, a class only gives its full type name, such as Namespace.Outer+Inner.
        if (text is null || text == type.ToString())
        {
            return type.IsNested ? $"{GetTypeName(type.DeclaringType!)}.{GetTypeName(type)}" : GetTypeName(type);
        }

        // An empty Id, for example, would otherwise leave nothing after the name.
        return text.Length == 0 ? "\"\"" : text;
    }

    /// <summary>
    /// Makes control characters visible, so a value stays on one line, and shortens it to <paramref name="limit"/> characters.
    /// </summary>
    public static string Clean(string text, int limit = TextLimit)
    {
        text = text.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t").Replace("\u001b", "\\u001b");

        return text.Length > limit ? $"{text[..limit]} ... ({text.Length:N0} characters)" : text;
    }

    /// <summary>
    /// A readable type name, such as <c>List&lt;CGameCtnBlock&gt;</c> instead of <c>List`1</c>.
    /// </summary>
    public static string GetTypeName(Type type)
    {
        var tick = type.Name.IndexOf('`');

        // Types nested in a generic type are generic without a tick in their own name.
        if (!type.IsGenericType || tick < 0)
        {
            return type.Name;
        }

        var name = new StringBuilder(type.Name[..tick]);
        name.Append('<').AppendJoin(", ", type.GetGenericArguments().Select(GetTypeName)).Append('>');

        return name.ToString();
    }
}
