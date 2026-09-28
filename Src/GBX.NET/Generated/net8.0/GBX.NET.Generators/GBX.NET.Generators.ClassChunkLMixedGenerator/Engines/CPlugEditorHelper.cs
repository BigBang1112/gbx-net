namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0917B000</remarks>
[Class(0x0917B000)]
public partial class CPlugEditorHelper : CMwNod, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x0917B000;




    private CPlugPrefab? prefab;
    public CPlugPrefab? Prefab { get => prefabFile?.GetNode(ref prefab) ?? prefab; set => prefab = value; }
    private Components.GbxRefTableFile? prefabFile;
    public Components.GbxRefTableFile? PrefabFile { get => prefabFile; set => prefabFile = value; }
    public CPlugPrefab? GetPrefab(GbxReadSettings settings = default, bool exceptions = false) => prefabFile?.GetNode(ref prefab, settings, exceptions) ?? prefab;

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.VersionInt32(this);
        rw.NodeRef<CPlugPrefab>(ref prefab, ref prefabFile);
    }




}
