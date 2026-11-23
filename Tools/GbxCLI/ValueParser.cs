using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace GbxCLI;

/// <summary>
/// Converts text from the command line to a value of a member's type, for <c>set</c>.
/// </summary>
/// <remarks>
/// Strings are taken as they are. Enums take names, numbers take invariant culture and <c>0x</c> for hexadecimal integers,
/// and types with a <c>Parse</c> method use it, such as <c>1:23.456</c> for a time. Other types are constructed from values
/// separated by commas, such as <c>1,2,3</c> for a vector or <c>Id,Collection,Author</c> for an ident.
/// </remarks>
internal static class ValueParser
{
    private const string RootedJustification = "GBX.NET and TmEssentials are rooted with TrimmerRootAssembly, so their constructors and Parse methods are kept.";

    /// <summary>
    /// Text that sets a nullable value or a reference other than a string to null.
    /// </summary>
    public const string Null = "null";

    /// <exception cref="FormatException">The text is not a valid value of <paramref name="type"/>.</exception>
    /// <exception cref="NotSupportedException">Values of <paramref name="type"/> cannot be written as text.</exception>
    public static object? Parse(string text, Type type)
    {
        var underlyingType = Nullable.GetUnderlyingType(type);

        if (text == Null && type != typeof(string) && (!type.IsValueType || underlyingType is not null))
        {
            return null;
        }

        type = underlyingType ?? type;

        if (type == typeof(string))
        {
            return text;
        }

        if (type.IsEnum)
        {
            return ParseEnum(text, type);
        }

        if (type == typeof(bool))
        {
            return bool.TryParse(text, out var boolean) ? boolean : throw Invalid(text, type, "Use true or false.");
        }

        if (type.IsPrimitive || type == typeof(decimal))
        {
            return ParseNumber(text, type);
        }

        if (type.IsAbstract || type.IsArray || type == typeof(object) || typeof(IEnumerable).IsAssignableFrom(type))
        {
            throw new NotSupportedException($"A {ValueFormatter.GetTypeName(type)} cannot be set from text.");
        }

        return FindParseMethod(type) is { } parse ? InvokeParse(parse, text, type) : Construct(text, type);
    }

    private static object ParseEnum(string text, Type type)
    {
        var isFlags = type.IsDefined(typeof(FlagsAttribute), inherit: false);

        // A number is accepted for flags, since their combinations have no single name.
        if (Enum.TryParse(type, text, ignoreCase: true, out var value) && (isFlags || Enum.IsDefined(type, value!)))
        {
            return value!;
        }

        throw Invalid(text, type, $"Use one of: {string.Join(", ", Enum.GetNames(type))}.");
    }

    private static object ParseNumber(string text, Type type)
    {
        try
        {
            if (type != typeof(char) && text.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                var bits = ulong.Parse(text.AsSpan(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                return Convert.ChangeType(bits, type, CultureInfo.InvariantCulture);
            }

            return Convert.ChangeType(text, type, CultureInfo.InvariantCulture);
        }
        catch (Exception exception) when (exception is FormatException or OverflowException or InvalidCastException)
        {
            throw Invalid(text, type, exception.Message);
        }
    }

    /// <summary>
    /// Finds a static <c>Parse(string, IFormatProvider)</c> or <c>Parse(string)</c> that returns <paramref name="type"/>.
    /// </summary>
    [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = RootedJustification)]
    private static MethodInfo? FindParseMethod(Type type)
        => type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, [typeof(string), typeof(IFormatProvider)]) is { } withProvider && withProvider.ReturnType == type
            ? withProvider
            : type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, [typeof(string)]) is { } method && method.ReturnType == type ? method : null;

    private static object InvokeParse(MethodInfo parse, string text, Type type)
    {
        try
        {
            return parse.Invoke(null, parse.GetParameters().Length == 2 ? [text, CultureInfo.InvariantCulture] : [text])!;
        }
        catch (TargetInvocationException exception) when (exception.InnerException is { } inner)
        {
            throw Invalid(text, type, inner.Message);
        }
    }

    /// <summary>
    /// Calls a public constructor with one parameter for each value separated by commas. A constructor that takes a string is tried last,
    /// so a number makes a numbered <c>Id</c>, such as a collection, and other text makes a string <c>Id</c>.
    /// </summary>
    [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = RootedJustification)]
    private static object Construct(string text, Type type)
    {
        var parts = text.Split(',', StringSplitOptions.TrimEntries);

        // A copy constructor of a record takes the type itself, which text cannot describe.
        var constructors = type.GetConstructors()
            .Where(constructor => constructor.GetParameters() is { Length: > 0 } parameters && parameters.All(parameter => parameter.ParameterType != type))
            .ToArray();

        var candidates = constructors
            .Where(constructor => constructor.GetParameters().Length == parts.Length)
            .OrderBy(constructor => constructor.GetParameters()[0].ParameterType == typeof(string) ? 1 : 0);

        Exception? lastError = null;

        foreach (var constructor in candidates)
        {
            try
            {
                var arguments = constructor.GetParameters().Select((parameter, i) => Parse(parts[i], parameter.ParameterType)).ToArray();
                return constructor.Invoke(arguments);
            }
            catch (Exception exception) when (exception is FormatException or NotSupportedException or TargetInvocationException)
            {
                lastError = exception is TargetInvocationException { InnerException: { } inner } ? inner : exception;
            }
        }

        if (lastError is not null)
        {
            throw Invalid(text, type, lastError.Message);
        }

        if (constructors.Length == 0)
        {
            throw new NotSupportedException($"A {ValueFormatter.GetTypeName(type)} cannot be set from text.");
        }

        var forms = constructors.Select(constructor => string.Join(",", constructor.GetParameters().Select(parameter => parameter.Name))).Distinct();
        throw Invalid(text, type, $"Use {string.Join(" or ", forms)}.");
    }

    private static FormatException Invalid(string text, Type type, string hint)
        => new($"'{text}' is not a valid {ValueFormatter.GetTypeName(type)}. {hint}");
}
