using GBX.NET.Exceptions;
using GBX.NET.Managers;

namespace GBX.NET.Engines.Plug;

public partial class CPlugPrefab : IVersionable
{
    private DateTime? fileWriteTime;
    private string url = "";
    private int u01;
    private int u02;
    private EntRef[] ents = [];

    public int Version { get; set; } = 11;

    public DateTime? FileWriteTime { get => fileWriteTime; set => fileWriteTime = value; }
    public string Url { get => url; set => url = value; }
    public int U01 { get => u01; set => u01 = value; }
    public int U02 { get => u02; set => u02 = value; }
    public EntRef[] Ents { get => ents; set => ents = value; }

    public override void ReadWrite(GbxReaderWriter rw)
    {
        rw.VersionInt32(this);

        if (Version >= 7)
        {
            rw.FileTime(ref fileWriteTime);
            rw.String(ref url);
            rw.Int32(ref u01);
        }

        var entsLength = rw.Int32(ents.Length);

        if (Version >= 5)
        {
            rw.Int32(ref u02);
        }

        rw.ArrayReadableWritable<EntRef>(ref ents, entsLength, Version);
    }

    public sealed partial class EntRef
    {

        public CMwNod? Model { get => modelRef?.Resolve(); set => NodeRef.SetEmbedded(ref modelRef, value); }

        private Quat rotation;
        public Quat Rotation { get => rotation; set => rotation = value; }

        private Vec3 position;
        public Vec3 Position { get => position; set => position = value; }

        private SMetaPtr? @params;
        public SMetaPtr? Params { get => @params; set => @params = value; }

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private float legacyPhase01 = -1;
        /// <summary>
        /// Prefab versions 2 to 9 store the dynamic object phase separately. Native readers apply a positive value to <see cref="NPlugDynaObjectModel_SInstanceParams.Phase01"/>.
        /// </summary>
        public float LegacyPhase01 { get => legacyPhase01; set => legacyPhase01 = value; }

        private byte u02;
        public byte U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.NodeRef(ref modelRef);

            if (v == 0)
            {
                throw new VersionNotSupportedException(v);
            }

            rw.Quat(ref rotation);
            rw.Vec3(ref position);

            if (v is >= 2 and < 10)
            {
                rw.Single(ref legacyPhase01);
            }

            if (v is >= 3 and < 11)
            {
                rw.Byte(ref u02);
            }

            if (v >= 4)
            {
                // should be replaced with rw.MetaRef<SMetaPtr> in the future or something
                var classId = rw.UInt32(@params is null
                    ? uint.MaxValue
                    : ClassManager.GetId(@params.GetType())
                        .GetValueOrDefault(uint.MaxValue));

                @params = classId switch
                {
                    // Legacy dynamic object params store only the period and texture.
                    0x2F0B6000 when v < 8 => ReadWriteLegacyInstanceParams(rw, (NPlugDynaObjectModel_SInstanceParams?)@params),
                    // Legacy constraint params omit the archive version.
                    0x2F0C8000 when v == 8 => ReadWriteLegacyConstraintParams(rw, (NPlugDyna_SPrefabConstraintParams?)@params),
                    0x2F0A9000 => rw.Node((NPlugItemPlacement_SPlacement?)@params),
                    0x2F0B6000 => rw.Node((NPlugDynaObjectModel_SInstanceParams?)@params),
                    0x2F0C8000 => rw.Node((NPlugDyna_SPrefabConstraintParams?)@params),
                    0x2F0D8000 => rw.Node((NPlugItemPlacement_SPlacementGroup?)@params),
                    0x2F0D9000 => rw.Node((NPlugStaticObjectModel_SInstanceParams?)@params),
                    uint.MaxValue => null,
                    _ => throw new NotImplementedException($"Unknown classId: 0x{classId:X8} ({ClassManager.GetName(classId)})"),
                };
            }

            if (v >= 6)
            {
                rw.String(ref u01);
            }
        }

        private static NPlugDynaObjectModel_SInstanceParams ReadWriteLegacyInstanceParams(GbxReaderWriter rw, NPlugDynaObjectModel_SInstanceParams? value)
        {
            value ??= new();
            value.PeriodSc = rw.Single(value.PeriodSc);
            value.TextureId = rw.Int32(value.TextureId);
            return value;
        }

        private static NPlugDyna_SPrefabConstraintParams ReadWriteLegacyConstraintParams(GbxReaderWriter rw, NPlugDyna_SPrefabConstraintParams? value)
        {
            value ??= new();
            value.Ent1 = rw.Int32(value.Ent1);
            value.Ent2 = rw.Int32(value.Ent2);
            value.Pos1 = rw.Vec3(value.Pos1);
            value.Pos2 = rw.Vec3(value.Pos2);
            return value;
        }
    }
}
