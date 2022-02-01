namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a private constructor is not found in GBX node classes.
/// </summary>
public class PrivateConstructorNotFoundException : Exception
{
    public Type Type { get; }

    public PrivateConstructorNotFoundException(Type type) : base(GetMessage(type))
    {
        Type = type;
    }

    private static string GetMessage(Type type)
    {
        return $"{type.Name} doesn't have a private constructor.";
    }
}
