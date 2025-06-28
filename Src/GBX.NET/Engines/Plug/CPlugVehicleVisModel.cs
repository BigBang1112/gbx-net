namespace GBX.NET.Engines.Plug;

public partial class CPlugVehicleVisModel
{
    private int surfVersion;
    public int SurfVersion { get => surfVersion; set => surfVersion = value; }

    public partial class Chunk090E7000 : IVersionable
    {
        public int Version { get; set; }

        public CPlugVehicleVisModelShared? U01;
        public Components.GbxRefTableFile? U01File;
        public CMwNod? U02;
        public CMwNod? U03;
        public CMwNod? U04;
        public CPlugLocatedSound? U06;
        public CPlugLocatedSound? U07;
        public CPlugLocatedSound? U08;
        public CPlugLocatedSound? U09;
        public CPlugLocatedSound? U10;
        public CPlugLocatedSound? U11;
        public CPlugLocatedSound? U12;
        public CPlugLocatedSound? U13;
        public CPlugLocatedSound? U14;
        public CPlugLocatedSound? U15;
        public CPlugLocatedSound? U16;
        public CPlugLocatedSound? U17;
        public CPlugLocatedSound? U18;
        public CPlugLocatedSound? U19;
        public float U20;
        public float U21;
        public float U22;
        public float U23;
        public CPlugSolid2Model? U24;
        public bool U25;
        public CPlugSolid2Model? U26;
        public CPlugSurface.ISurf? U27;
        public CPlugSolid2Model? U28;
        public CPlugSurface.ISurf? U29;
        public CPlugSolid2Model? U30;
        public CPlugSurface.ISurf? U31;
        public CPlugSolid2Model? U32;
        public CPlugSurface.ISurf? U33;
        public short[]? U34;
        public Vec3[]? U35;
        public string? U36;
        public string? U37;
        public short[]? U38;
        public Vec3[]? U39;
        public string? U40;
        public string? U41;
        public short[]? U42;
        public Vec3[]? U43;
        public string? U44;
        public string? U45;
        public short[]? U46;
        public Vec3[]? U47;
        public string? U48;
        public string? U49;
        public short[]? U50;
        public Vec3[]? U51;
        public string? U52;
        public string? U53;
        public Vec3 U54;
        public Vec3 U55;
        public Vec3 U56;
        public Vec3 U57;
        public Vec3 U58;
        public Vec3 U59;
        public Vec3 U60;
        public Vec3 U61;
        public Vec3 U62;
        public string? U63;
        public CPlugSolid2Model? U64;

        public override void ReadWrite(CPlugVehicleVisModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugVehicleVisModelShared>(ref U01, ref U01File);
            rw.NodeRef<CMwNod>(ref U02);
            rw.NodeRef<CMwNod>(ref U03);
            rw.NodeRef<CMwNod>(ref U04);
            if (Version >= 5)
            {
                rw.Int32(ref n.surfVersion);
            }
            rw.NodeRef<CPlugLocatedSound>(ref U06);
            rw.NodeRef<CPlugLocatedSound>(ref U07);
            rw.NodeRef<CPlugLocatedSound>(ref U08);
            rw.NodeRef<CPlugLocatedSound>(ref U09);
            rw.NodeRef<CPlugLocatedSound>(ref U10);
            rw.NodeRef<CPlugLocatedSound>(ref U11);
            rw.NodeRef<CPlugLocatedSound>(ref U12);
            rw.NodeRef<CPlugLocatedSound>(ref U13);
            rw.NodeRef<CPlugLocatedSound>(ref U14);
            rw.NodeRef<CPlugLocatedSound>(ref U15);
            rw.NodeRef<CPlugLocatedSound>(ref U16);
            if (Version >= 2)
            {
                rw.NodeRef<CPlugLocatedSound>(ref U17);
                if (Version >= 6)
                {
                    rw.NodeRef<CPlugLocatedSound>(ref U18);
                    if (Version >= 8)
                    {
                        rw.NodeRef<CPlugLocatedSound>(ref U19);
                    }
                }
            }
            if (Version >= 1)
            {
                rw.Single(ref U20);
                rw.Single(ref U21);
                if (Version >= 9)
                {
                    rw.Single(ref U22);
                    rw.Single(ref U23);
                }
                if (Version >= 3)
                {
                    rw.NodeRef<CPlugSolid2Model>(ref U24);
                    rw.Boolean(ref U25);

                    rw.NodeRef<CPlugSolid2Model>(ref U26);
                    var wtf = rw.Reader.ReadArray<int>(13);
                    if (rw.Reader is not null) U27 = CPlugSurface.ReadSurf(rw.Reader, n.surfVersion);
                    if (rw.Writer is not null) CPlugSurface.WriteSurf(U27, rw.Writer, n.surfVersion);

                    rw.NodeRef<CPlugSolid2Model>(ref U28);
                    if (rw.Reader is not null)
                    {
                        U29 = CPlugSurface.ReadSurf(rw.Reader, n.surfVersion);
                    }

                    if (rw.Writer is not null)
                    {
                        CPlugSurface.WriteSurf(U29, rw.Writer, n.surfVersion);
                    }

                    /*rw.NodeRef<CPlugSolid2Model>(ref U30);

                    if (rw.Reader is not null)
                    {
                        U31 = CPlugSurface.ReadSurf(rw.Reader, n.surfVersion);
                    }

                    if (rw.Writer is not null)
                    {
                        CPlugSurface.WriteSurf(U31, rw.Writer, n.surfVersion);
                    }

                    rw.NodeRef<CPlugSolid2Model>(ref U32);

                    if (rw.Reader is not null)
                    {
                        U33 = CPlugSurface.ReadSurf(rw.Reader, n.surfVersion);
                    }

                    if (rw.Writer is not null)
                    {
                        CPlugSurface.WriteSurf(U33, rw.Writer, n.surfVersion);
                    }*/

                    rw.Array<short>(ref U34!);
                    rw.Array<Vec3>(ref U35!);
                    rw.Id(ref U36);
                    rw.Id(ref U37);
                    rw.Array<short>(ref U38!);
                    rw.Array<Vec3>(ref U39!);
                    rw.Id(ref U40);
                    rw.Id(ref U41);
                    rw.Array<short>(ref U42!);
                    rw.Array<Vec3>(ref U43!);
                    rw.Id(ref U44);
                    rw.Id(ref U45);
                    rw.Array<short>(ref U46!);
                    rw.Array<Vec3>(ref U47!);
                    rw.Id(ref U48);
                    rw.Id(ref U49);
                    rw.Array<short>(ref U50!);
                    rw.Array<Vec3>(ref U51!);
                    rw.Id(ref U52);
                    rw.Id(ref U53);
                    rw.Vec3(ref U54);
                    rw.Vec3(ref U55);
                    rw.Vec3(ref U56);
                    rw.Vec3(ref U57);
                    rw.Vec3(ref U58);
                    rw.Vec3(ref U59);
                    rw.Vec3(ref U60);
                    rw.Vec3(ref U61);
                    rw.Vec3(ref U62);
                    if (Version >= 7)
                    {
                        rw.String(ref U63);
                        if (U63 == null || U63 == "")
                        {
                            rw.NodeRef<CPlugSolid2Model>(ref U64);
                        }
                    }
                }
            }
        }
    }
}
