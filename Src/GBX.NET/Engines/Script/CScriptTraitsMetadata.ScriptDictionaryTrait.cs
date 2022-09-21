using System.Text;

namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
{
    /// <summary>
    /// A simplified variant of <c>ScriptTrait&lt;IDictionary&lt;ScriptTrait, ScriptTrait&gt;&gt;</c>
    /// </summary>
    public class ScriptDictionaryTrait : ScriptTrait<IDictionary<ScriptTrait, ScriptTrait>>
    {
        public ScriptDictionaryTrait(ScriptArrayType type, IDictionary<ScriptTrait, ScriptTrait> value)
            : base(type, value)
        {
        }

        public override string ToString()
        {
            var builder = new StringBuilder(Type.ToString());

            builder.Append(" (");
            builder.Append(Value.Count);
            builder.Append(" elements)");

            return builder.ToString();
        }
    }
}
