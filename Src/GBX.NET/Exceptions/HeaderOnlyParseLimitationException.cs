using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Exceptions;

public class HeaderOnlyParseLimitationException : Exception
{
    public HeaderOnlyParseLimitationException() : this("This action is forbidden in GameBox where only the header was parsed.")
    {
    }

    public HeaderOnlyParseLimitationException(string? message) : base(message)
    {
    }

    public HeaderOnlyParseLimitationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected HeaderOnlyParseLimitationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
