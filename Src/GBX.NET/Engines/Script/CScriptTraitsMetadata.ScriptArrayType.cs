using System.Text;

namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
{
    public readonly record struct ScriptArrayType(IScriptType KeyType, IScriptType ValueType) : IScriptType
    {
        public EScriptType Type => EScriptType.Array;

        public override string ToString()
        {
            var result = new StringBuilder(ValueType.ToString());
            result.Append('[');
            result.Append(KeyType.ToString());
            result.Append(']');
            return result.ToString();
        }
    }
}
