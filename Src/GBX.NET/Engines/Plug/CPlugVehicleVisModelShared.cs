namespace GBX.NET.Engines.Plug;

public partial class CPlugVehicleVisModelShared
{
    public VisualVehicle[] VisualVehicles { get; set; } = [];

    public partial class Chunk090E8006
    {
        public override void Read(CPlugVehicleVisModelShared n, GbxReader r)
        {
            n.VisualVehicles = new VisualVehicle[r.ReadInt32()];
        }

        public override void Write(CPlugVehicleVisModelShared n, GbxWriter w)
        {
            w.Write(n.VisualVehicles.Length);
        }
    }

    public partial class Chunk090E8009
    {
        public override void Read(CPlugVehicleVisModelShared n, GbxReader r)
        {
            for (int i = 0; i < n.VisualVehicles.Length; i++)
            {
                n.VisualVehicles[i] ??= new VisualVehicle();
                n.VisualVehicles[i].VisualArms = r.ReadArrayReadable<VisualArm>();
            }
        }

        public override void Write(CPlugVehicleVisModelShared n, GbxWriter w)
        {
            for (int i = 0; i < n.VisualVehicles.Length; i++)
            {
                w.WriteArrayWritable(n.VisualVehicles[i].VisualArms);
            }
        }
    }

    public partial class Chunk090E800A
    {
        public override void Read(CPlugVehicleVisModelShared n, GbxReader r)
        {
            for (int i = 0; i < n.VisualVehicles.Length; i++)
            {
                n.VisualVehicles[i] ??= new VisualVehicle();
                n.VisualVehicles[i].VisualLights = r.ReadArrayReadable<VisualLight>();
            }
        }

        public override void Write(CPlugVehicleVisModelShared n, GbxWriter w)
        {
            for (int i = 0; i < n.VisualVehicles.Length; i++)
            {
                w.WriteArrayWritable(n.VisualVehicles[i].VisualLights);
            }
        }
    }

    public partial class Chunk090E800C
    {
        public int[][]? U01;

        public override void Read(CPlugVehicleVisModelShared n, GbxReader r)
        {
            var count = r.ReadInt32();
            U01 = new int[count][];
            for (int i = 0; i < count; i++)
            {
                U01[i] = r.ReadArray<int>();
            }
        }

        public override void Write(CPlugVehicleVisModelShared n, GbxWriter w)
        {
            if (U01 is null)
            {
                w.Write(0);
                return;
            }

            w.Write(U01.Length);
            for (int i = 0; i < U01.Length; i++)
            {
                w.WriteArray(U01[i]);
            }
        }
    }

    public partial class Chunk090E800D
    {
        public override void Read(CPlugVehicleVisModelShared n, GbxReader r)
        {
            for (int i = 0; i < n.VisualVehicles.Length; i++)
            {
                n.VisualVehicles[i] ??= new VisualVehicle();
                n.VisualVehicles[i].Emitters = r.ReadArrayReadable<Emitter>();
            }
        }

        public override void Write(CPlugVehicleVisModelShared n, GbxWriter w)
        {
            for (int i = 0; i < n.VisualVehicles.Length; i++)
            {
                w.WriteArrayWritable(n.VisualVehicles[i].Emitters);
            }
        }
    }

    public partial class Chunk090E800F
    {
        public override void Read(CPlugVehicleVisModelShared n, GbxReader r)
        {
            for (int i = 0; i < n.VisualVehicles.Length; i++)
            {
                n.VisualVehicles[i] ??= new VisualVehicle();
                n.VisualVehicles[i].VisualWheels = r.ReadArrayReadable<VisualWheel>();
            }
        }

        public override void Write(CPlugVehicleVisModelShared n, GbxWriter w)
        {
            for (int i = 0; i < n.VisualVehicles.Length; i++)
            {
                w.WriteArrayWritable(n.VisualVehicles[i].VisualWheels);
            }
        }
    }

    public partial class Chunk090E8010
    {
        public override void Read(CPlugVehicleVisModelShared n, GbxReader r)
        {
            for (int i = 0; i < n.VisualVehicles.Length; i++)
            {
                n.VisualVehicles[i] ??= new VisualVehicle();
                n.VisualVehicles[i].U01 = r.ReadReadable<VisualId>();
                n.VisualVehicles[i].U02 = r.ReadReadable<VisualId>();
                n.VisualVehicles[i].U03 = r.ReadReadable<VisualId>();
                n.VisualVehicles[i].U04 = r.ReadReadable<VisualId>();
                n.VisualVehicles[i].U05 = r.ReadInt32();
            }
        }

        public override void Write(CPlugVehicleVisModelShared n, GbxWriter w)
        {
            for (int i = 0; i < n.VisualVehicles.Length; i++)
            {
                w.WriteWritable(n.VisualVehicles[i].U01);
                w.WriteWritable(n.VisualVehicles[i].U02);
                w.WriteWritable(n.VisualVehicles[i].U03);
                w.WriteWritable(n.VisualVehicles[i].U04);
                w.Write(n.VisualVehicles[i].U05);
            }
        }
    }

    public partial class SimulationWheel
    {
        public override string ToString()
        {
            return $"{Name} (U01: {U01}, U02: {U01})";
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class VisualId
    {
        public override string ToString()
        {
            return $"{Name} (U01: {U01})";
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class VisualArm
    {
        public override string ToString()
        {
            return $"{U01}, {U02}, {U03} [U04: {U04}, U05: {U05}, U06: {U06}]";
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class VisualLight
    {
        public override string ToString()
        {
            return $"{U01} [U02: {U02}]";
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class VisualWheel
    {
        public override string ToString()
        {
            return $"{U01}, {U02}, {U03}, {U04} [U05: {U05}, U06: {U06}]";
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class Emitter;

    public sealed class VisualVehicle
    {
        public VisualArm[] VisualArms { get; set; } = [];
        public VisualLight[] VisualLights { get; set; } = [];
        public VisualWheel[] VisualWheels { get; set; } = [];
        public Emitter[] Emitters { get; set; } = [];
        public VisualId? U01 { get; set; }
        public VisualId? U02 { get; set; }
        public VisualId? U03 { get; set; }
        public VisualId? U04 { get; set; }
        public int U05 { get; set; }
    }
}
