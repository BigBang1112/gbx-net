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
        public string NullStringIfNull => this is not ElementNodeModel && !AuxNodes.Any() && Node is null ? " null" : string.Empty;
        public bool HasNodeWithChunksInside { get; set; }
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
