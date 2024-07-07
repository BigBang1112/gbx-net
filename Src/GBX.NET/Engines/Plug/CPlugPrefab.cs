using GBX.NET.Managers;

namespace GBX.NET.Engines.Plug;

public partial class CPlugPrefab : IVersionable
{
    private DateTime fileWriteTime;
    private string url = "";
    private int u01;
    private int u02;
    private EntRef[] ents = [];

    public int Version { get; set; }

    public DateTime FileWriteTime { get => fileWriteTime; set => fileWriteTime = value; }
    public string Url { get => url; set => url = value; }
    public int U01 { get => u01; set => u01 = value; }
    public int U02 { get => u02; set => u02 = value; }
    public EntRef[] Ents { get => ents; set => ents = value; }

#if NET8_0_OR_GREATER
    static void IClass.Read<T>(T node, GbxReaderWriter rw)
    {
        node.ReadWrite(rw);
    }
#endif

    public override void ReadWrite(GbxReaderWriter rw)
    {
        rw.VersionInt32(this);
        rw.FileTime(ref fileWriteTime);
        rw.String(ref url);
        rw.Int32(ref u01);
        var entsLength = rw.Int32(ents.Length);
        rw.Int32(ref u02);
        rw.ArrayReadableWritable<EntRef>(ref ents, entsLength);
    }

    public sealed partial class EntRef
    {
        private CMwNod? model;
        public CMwNod? Model { get => modelFile?.GetNode(ref model) ?? model; set => model = value; }
        private Components.GbxRefTableFile? modelFile;
        public Components.GbxRefTableFile? ModelFile { get => modelFile; set => modelFile = value; }
        public CMwNod? GetModel(GbxReadSettings settings = default) => modelFile?.GetNode(ref model, settings);

        private Quat rotation;
        public Quat Rotation { get => rotation; set => rotation = value; }

        private Vec3 position;
        public Vec3 Position { get => position; set => position = value; }

        private SMetaPtr? @params;
        public SMetaPtr? Params { get => @params; set => @params = value; }

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.NodeRef(ref model, ref modelFile);
            rw.Quat(ref rotation);
            rw.Vec3(ref position);

            if (model is not null || modelFile is not null)
            {
                // should be replaced with rw.MetaRef<SMetaPtr> in the future or something
                var classId = rw.UInt32(@params is null
                    ? uint.MaxValue
                    : ClassManager.GetId(@params.GetType())
                        .GetValueOrDefault(uint.MaxValue));

                @params = classId switch
                {
                    0x2F0B6000 => rw.Node((NPlugDynaObjectModel_SInstanceParams?)@params),
                    0x2F0C8000 => rw.Node((NPlugDyna_SPrefabConstraintParams?)@params),
                    _ => throw new NotImplementedException($"Unknown classId: 0x{classId:X8} ({ClassManager.GetName(classId)})"),
                };
            }

            rw.String(ref u01);
        }
    }
}
