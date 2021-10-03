using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Exceptions
{
    public class PropertyNullException : Exception
    {
        public PropertyNullException(string propertyName) : base($"Property '{propertyName}' is null.")
        {
        }
    }
}
