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
        private ChunkSet? chunks;

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

        public ChunkSet? Chunks
        {
            get
            {
                if (this is MainNodeModel)
                {
                    if (chunks is null)
                    {
                        var node = Node!;
                        var gbx = node.GBX!;

                        var headerProp = gbx.GetType().GetProperty("Header", typeof(GameBoxHeader<>).MakeGenericType(node.GetType()));
                        if (headerProp is null)
                            return node.Chunks;

                        var headerVal = headerProp.GetValue(gbx);
                        if (headerVal is null)
                            return node.Chunks;

                        var chunksProp = headerVal.GetType().GetProperty("Chunks");
                        if (chunksProp is null)
                            return node.Chunks;

                        var headerChunkSet = chunksProp.GetValue(headerVal) as ChunkSet;
                        if (headerChunkSet is null)
                            return node.Chunks;

                        var chunkSet = new ChunkSet(node);

                        foreach (var headerChunk in headerChunkSet)
                        {
                            chunkSet.Add(headerChunk);
                        }

                        foreach (var chunk in node.Chunks)
                        {
                            chunkSet.Add(chunk);
                        }

                        chunks = chunkSet;

                        return chunks;
                    }

                    return chunks;
                }

                return Node?.Chunks;
            }
        }
    }
}
