using GBX.NET.Managers;

namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a class can be read but not written.
/// </summary>
public class ClassWriteNotSupportedException(uint classId)
    : NotSupportedException($"Class ID 0x{classId:X8} ({ClassManager.GetName(classId)}) write is not supported.")
{
    /// <summary>
    /// The class ID that cannot be written.
    /// </summary>
    public uint ClassId { get; } = classId;
}
