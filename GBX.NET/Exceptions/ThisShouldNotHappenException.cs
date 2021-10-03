using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Exceptions
{
    public class ThisShouldNotHappenException : Exception
    {
        public ThisShouldNotHappenException() : base("This should not happen.")
        {

        }
    }
}
