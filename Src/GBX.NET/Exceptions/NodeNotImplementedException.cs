using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Exceptions;

public class NodeNotImplementedException : Exception
{
    public NodeNotImplementedException(uint classId) : base(GetMessage(classId))
    {
        
    }

    private static string GetMessage(uint id)
    {
        var name = NodeCacheManager.Names
            .Where(x => x.Key == id)
            .Select(x => x.Value)
            .FirstOrDefault() ?? "unknown class";

        return $"Node with ID 0x{id:X8} is not implemented. ({name})";
    }
}
