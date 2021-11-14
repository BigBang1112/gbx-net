using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Exceptions;

public class TextFormatNotSupportedException : Exception
{
    public TextFormatNotSupportedException() : base("Text-formatted GBX files are not supported.")
    {

    }
}
