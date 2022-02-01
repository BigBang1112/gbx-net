namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a read Id has not a valid index. 
/// </summary>
public class CorruptedIdException : Exception
{
    public uint Index { get; }

    public CorruptedIdException(uint index) : base(GetMessage(index))
    {
        Index = index;
    }

    private static string GetMessage(uint index)
    {
        return $"The index ({index}) in Id is not matching any known values.";
    }
}
