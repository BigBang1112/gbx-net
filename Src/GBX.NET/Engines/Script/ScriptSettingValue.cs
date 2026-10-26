using GBX.NET.Serialization;

namespace GBX.NET.Engines.Script;

/// <summary>
/// A typed script setting value. The version is the script type version.
/// </summary>
public sealed class ScriptSettingValue : IReadableWritable, IReadable, IWritable
{
    public CScriptTraitsMetadata.ScriptTrait? Value { get; set; }

    public void Read(GbxReader r, int v = 0) => ReadWrite(new GbxReaderWriter(r), v);

    public void Write(GbxWriter w, int v = 0) => ReadWrite(new GbxReaderWriter(w), v);

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        if (rw.Reader is not null)
        {
            var type = CScriptTraitsMetadata.ReadType(rw.Reader, v);
            Value = CScriptTraitsMetadata.ReadContents(rw.Reader, type, noContent: false, v);
        }

        if (rw.Writer is not null)
        {
            if (Value is null)
            {
                throw new InvalidOperationException("Typed script setting value is missing.");
            }

            CScriptTraitsMetadata.WriteType(rw.Writer, Value.Type, v);
            CScriptTraitsMetadata.WriteContents(rw.Writer, Value, v);
        }
    }
}
