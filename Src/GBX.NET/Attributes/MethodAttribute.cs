namespace GBX.NET.Attributes;

/// <summary>
/// Associates a property with a public, parameterless method or an extension method on its owner.
/// </summary>
/// <remarks>The method must return a value that can be displayed using <see cref="object.ToString()"/>.</remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class MethodAttribute(string methodName) : Attribute
{
    public string MethodName { get; } = methodName;

    /// <summary>
    /// Assembly-qualified name of the type declaring an external static or extension method, or null to use the owning type.
    /// </summary>
    public string? DeclaringTypeName { get; set; }

    /// <summary>
    /// Copies the result to the clipboard instead of displaying it in an explorer.
    /// </summary>
    public bool Protected { get; set; }
}
