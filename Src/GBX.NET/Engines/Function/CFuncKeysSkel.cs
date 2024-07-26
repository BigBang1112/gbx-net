namespace GBX.NET.Engines.Function;

public partial class CFuncKeysSkel
{
    public partial class Chunk05006001
    {
        public Loc[][]? U01;

        public override void Read(CFuncKeysSkel n, GbxReader r)
        {
            var count = r.ReadInt32();
            U01 = new Loc[count][];

            for (var i = 0; i < count; i++)
            {
                var count2 = n.skel?.Bones?.Length ?? 0;
                U01[i] = new Loc[count2];

                for (var j = 0; j < count2; j++)
                {
                    var loc = new Loc();
                    loc.Read(r);
                    U01[i][j] = loc;
                }
            }
        }

        public override void Write(CFuncKeysSkel n, GbxWriter w)
        {
            w.Write(U01?.Length ?? 0);

            if (U01 is null)
            {
                return;
            }

            foreach (var locs in U01)
            {
                if (locs.Length != n.skel?.Bones?.Length)
                {
                    throw new InvalidOperationException("Invalid bone count.");
                }

                foreach (var loc in locs)
                {
                    loc.Write(w);
                }
            }
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class Loc;
}
