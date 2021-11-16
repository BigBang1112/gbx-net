using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Attributes;

[Obsolete]
public class ChunkDescriptionAttribute : Attribute
{
    public string Description { get; }

    public ChunkDescriptionAttribute(string description)
    {
        Description = description;
    }
}
