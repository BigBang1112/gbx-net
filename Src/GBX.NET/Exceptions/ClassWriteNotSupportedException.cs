using GBX.NET.Managers;

namespace GBX.NET.Exceptions;

public class ClassWriteNotSupportedException(uint classId) : NotSupportedException($"Class ID 0x{classId:X8} ({ClassManager.GetName(classId)}) write is not supported.");