using GBX.NET.Managers;

namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a class ID is unknown or cannot be instantiated.
/// </summary>
public class ClassNotSupportedException(uint classId)
    : NotSupportedException($"Class ID 0x{classId:X8} ({ClassManager.GetName(classId) ?? "unknown class name"}) is not supported.")
{
    /// <summary>
    /// The class ID that is not supported.
    /// </summary>
    public uint ClassId { get; } = classId;
}
