using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.ChunkExplorer.Exceptions
{
    public class NodeIsNullException : Exception
    {
        public NodeIsNullException() : base("Node is null here when it shouldn't. Please, report this issue together with the file.")
        {

        }
    }
}
