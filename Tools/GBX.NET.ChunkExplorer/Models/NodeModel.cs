using GBX.NET.Engines.MwFoundations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GBX.NET.ChunkExplorer.Models
{
    public abstract class NodeModel
    {
        public string TypeName { get; set; } = string.Empty;
        public Type? Type { get; set; }
        public CMwNod? Node { get; set; }
        public string Value
        {
            get
            {
                if (AuxNodes.Any()) // Has some form of node inside
                {
                    if (Node is null) // Is not a node - likely a list
                    {
                        var supportedCount = AuxNodes.Take(100).Count();

                        if (supportedCount == 100)
                        {
                            return "(99+)";
                        }

                        return $"({supportedCount})";
                    }                    

                    return string.Empty;
                }

                if (IsCollection)
                {
                    return "(empty)";
                }

                if (Node is null) // Is a single node without any content inside
                {
                    return "null";
                }

                return string.Empty;
            }
        }
        public bool HasNodeWithChunksInside { get; set; }
        public bool IsCollection { get; set; }
        public SolidColorBrush NodeBackgroundBrush
        {
            get
            {
                if (Node is null && AuxNodes.Any())
                    return Brushes.SandyBrown;
                if (Node is null || Node.Chunks.Count == 0)
                    return Brushes.Gray;
                if (HasNodeWithChunksInside)
                    return Brushes.DarkOrange;
                return Brushes.CornflowerBlue;
            }
        }

        public IEnumerable<AuxNodeModel> AuxNodes { get; set; } = Enumerable.Empty<AuxNodeModel>();
        public ChunkSet? Chunks => Node?.Chunks;
    }
}
