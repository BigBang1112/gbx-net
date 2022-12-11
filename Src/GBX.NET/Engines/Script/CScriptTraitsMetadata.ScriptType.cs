namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
{
    public readonly record struct ScriptType(EScriptType Type) : IScriptType
    {
        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
