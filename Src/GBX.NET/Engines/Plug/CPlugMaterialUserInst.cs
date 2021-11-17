namespace GBX.NET.Engines.Plug;

[Node(0x090FD000)]
public sealed class CPlugMaterialUserInst : CMwNod
{
    private string? materialFile;

    public string? MaterialFile
    {
        get => materialFile;
        set => materialFile = value;
    }

    private CPlugMaterialUserInst()
    {

    }

    public override string ToString()
    {
        return MaterialFile ?? "";
    }

    [Chunk(0x090FD000)]
    public class Chunk090FD000 : Chunk<CPlugMaterialUserInst>, IVersionable
    {
        private int version;

        public CMwNod? U01;
        public CMwNod? U02;
        public int U03;
        public byte U04;
        public CMwNod? U05;

        /// <summary>
        /// Version 10: TM®, version 8/9: ManiaPlanet
        /// </summary>
        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CPlugMaterialUserInst n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef(ref U01);
            rw.NodeRef(ref U02);
            rw.Int32(ref U03);
            rw.Byte(ref U04);
            if (Version < 9)
                rw.Id(ref n.materialFile);
            if (Version >= 9)
            {
                if (Version >= 10)
                    rw.Byte();
                rw.String(ref n.materialFile);
            }
            rw.Int32();
            rw.Int32();
            rw.Int32();
            rw.Int32();
            rw.Int32();
            rw.NodeRef(ref U05);
        }
    }

    [Chunk(0x090FD001)]
    public class Chunk090FD001 : Chunk<CPlugMaterialUserInst>, IVersionable
    {
        private int version;

        public CMwNod? U01;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CPlugMaterialUserInst n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef(ref U01);
            rw.Int32();
            rw.Int32();
            rw.Single();
            rw.Int32();
            rw.Int32();
        }
    }

    [Chunk(0x090FD002)]
    public class Chunk090FD002 : Chunk<CPlugMaterialUserInst>
    {
        public override void ReadWrite(CPlugMaterialUserInst n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Int32();
        }
    }
}
