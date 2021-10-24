using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.ChunkExplorer.Mapping
{
    internal static class MappingManager
    {
        internal static void Configure()
        {
            var adapters = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.Namespace == "GBX.NET.ChunkExplorer.Mapping.Adapters" && !x.IsNested);

            foreach (var adapter in adapters)
            {
                var method = adapter.GetMethod("Configure");

                if (method is null)
                    throw new Exception(adapter.FullName + " is missing a Configure method.");
                
                _ = method.Invoke(null, null);
            }
        }
    }
}
