using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Exceptions
{
    public class StringLengthOutOfRangeException : Exception
    {
        public StringLengthOutOfRangeException(int length) : base("The string was outside of the valid range: " + length)
        {
        }
    }
}
