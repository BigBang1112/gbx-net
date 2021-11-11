using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Exceptions;

/// <summary>
/// The exception that is thrown when a private constructor is not found in GBX node classes.
/// </summary>
public class PrivateConstructorNotFoundException : Exception
{
    public PrivateConstructorNotFoundException(Type type)
        : base(type.Name + " doesn't have a private constructor.")
    {

    }
}
