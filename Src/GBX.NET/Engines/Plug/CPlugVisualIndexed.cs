﻿namespace GBX.NET.Engines.Plug;

[Node(0x0906A000)]
public class CPlugVisualIndexed : CPlugVisual3D
{
    private ushort[] indices;

    public ushort[] Indices
    {
        get => indices;
        set => indices = value;
    }

    protected CPlugVisualIndexed()
    {
        indices = null!;
    }

    [Chunk(0x0906A001)]
    public class Chunk0906A001 : Chunk<CPlugVisualIndexed>
    {
        public bool U01;
        public int Flags;

        public override void ReadWrite(CPlugVisualIndexed n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);

            rw.UInt32(0x9057000);
            rw.Int32(ref Flags);
            rw.Array<ushort>(ref n.indices!);
            rw.UInt32(0xFACADE01);
        }
    }
}
