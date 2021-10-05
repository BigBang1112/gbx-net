using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Exceptions
{
    public class PrivateConstructorNotFoundException : Exception
    {
        public PrivateConstructorNotFoundException(Type type)
            : base(type.Name + " doesn't have a private constructor.")
        {

        }
    }
}
