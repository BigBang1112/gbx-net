namespace GBX.NET.Attributes;

/// <summary>
/// Associates a property with a public, parameterless method on its owning type.
/// </summary>
/// <remarks>The method must return a value that can be displayed using <see cref="object.ToString()"/>.</remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class MethodAttribute(string methodName) : Attribute
{
    public string MethodName { get; } = methodName;

    /// <summary>
    /// Copies the result to the clipboard instead of displaying it in an explorer.
    /// </summary>
    public bool Protected { get; set; }
}
