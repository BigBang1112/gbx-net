using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Exceptions;

public class VersionNotSupportedException : Exception
{
    public VersionNotSupportedException(int version) : base($"Version {version} is not supported.")
    {
    }
}
