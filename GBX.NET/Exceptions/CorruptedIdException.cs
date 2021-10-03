using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Exceptions
{
    public class CorruptedIdException : Exception
    {
        public CorruptedIdException(uint index) : base($"The index ({index}) in Id is not matching any known values.")
        {

        }
    }
}
