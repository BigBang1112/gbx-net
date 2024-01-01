namespace GBX.NET.Exceptions;

public class SerializationModeNotSupportedException(SerializationMode mode)
    : NotSupportedException($"Serialization mode {mode} is not supported.") { }