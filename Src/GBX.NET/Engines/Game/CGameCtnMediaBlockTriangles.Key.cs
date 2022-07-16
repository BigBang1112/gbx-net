namespace GBX.NET.Engines.Game;

public abstract partial class CGameCtnMediaBlockTriangles
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private readonly CGameCtnMediaBlockTriangles node;

        private Vec3[] positions;

        public Vec3[] Positions
        {
            get => positions;
            set
            {
                if (value.Length != positions.Length)
                {
                    Array.Resize(ref node.vertices, value.Length);

                    foreach (var k in node.keys)
                        if (k != this)
                            Array.Resize(ref k.positions, value.Length);

                    node.RemoveTrianglesOutOfRange();
                }

                positions = value;
            }
        }

        public Key(CGameCtnMediaBlockTriangles node)
        {
            this.node = node;
            positions = new Vec3[node.vertices?.Length ?? 0];
        }
    }
}
