using System;

namespace GBX.NET;

[AttributeUsage(AttributeTargets.Class)]
public class NodeAttribute : Attribute
{
    public uint ID { get; }

    public NodeAttribute(uint id)
    {
        ID = id;
    }
}
