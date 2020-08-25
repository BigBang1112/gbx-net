using System;

namespace GBX.NET
{
    public class NodeAttribute : Attribute
    {
        public uint ID { get; }

        public NodeAttribute(uint id)
        {
            ID = id;
        }
    }
}