using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Exceptions
{
    public class NodeNotFoundException : Exception
    {
        public NodeNotFoundException(uint classId) : base($"Node with a class ID {classId:X8} was not found.")
        {

        }
    }
}
