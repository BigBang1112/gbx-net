using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a string length is outside of a valid range.
/// </summary>
public class StringLengthOutOfRangeException : Exception
{
    public StringLengthOutOfRangeException(int length) : base("The string was outside of the valid range: " + length)
    {
    }
}
