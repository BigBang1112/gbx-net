namespace GBX.NET.Engines.Game;

public partial class CGameCtnChallenge
{
    public partial class BlockOctreeData
    {
        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref rootNodeSize);
            rw.Int3(ref size);

            var count = rw.Int32(nodes?.Length ?? 0);
            if (count < 0)
            {
                throw new InvalidDataException("Block octree node count cannot be negative.");
            }

            if (rw.Reader is not null)
            {
                nodes = new BlockOctreeNode[count];
            }

            var nodeSizes = new int[count];
            for (var i = 0; i < count; i++)
            {
                var node = nodes![i] ??= new BlockOctreeNode();
                node.ParentIndex = rw.Int32(node.ParentIndex);
                var nodeSize = node.ParentIndex >= 0 && node.ParentIndex < i
                    ? nodeSizes[node.ParentIndex] >> 1
                    : rootNodeSize;
                nodeSizes[i] = nodeSize;

                if (nodeSize == 1)
                {
                    node.Value = rw.Byte(node.Value);
                    if (v == 0)
                    {
                        node.LegacyValue = rw.Int32(node.LegacyValue);
                    }
                }
                else
                {
                    node.Children = rw.Array(node.Children, 8);
                }
            }
        }
    }
}
