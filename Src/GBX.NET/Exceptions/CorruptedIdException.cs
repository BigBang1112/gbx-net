using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a read <see cref="Id"/> has not a valid index. 
    /// </summary>
    public class CorruptedIdException : Exception
    {
        public CorruptedIdException(uint index) : base($"The index ({index}) in Id is not matching any known values.")
        {

        }
    }
}
