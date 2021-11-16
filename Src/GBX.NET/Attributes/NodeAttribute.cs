using System;

namespace GBX.NET.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class NodeAttribute : Attribute
{
    public uint ID { get; }

    public NodeAttribute(uint id)
    {
        ID = id;
    }
}
