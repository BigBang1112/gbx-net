using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Exceptions
{
    /// <summary>
    /// The exception that is thrown when an internal property of GBX.NET is null when it shouldn't be.
    /// </summary>
    public class PropertyNullException : Exception
    {
        public PropertyNullException(string propertyName) : base($"Property '{propertyName}' is null.")
        {
        }
    }
}
