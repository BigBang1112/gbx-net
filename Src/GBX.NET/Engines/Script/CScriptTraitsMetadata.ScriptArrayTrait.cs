using System.Text;

namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
{
    /// <summary>
    /// A simplified variant of <c>ScriptTrait&lt;IList&lt;ScriptTrait&gt;&gt;</c>
    /// </summary>
    public class ScriptArrayTrait : ScriptTrait<IList<ScriptTrait>>
    {
        public ScriptArrayTrait(ScriptArrayType type, IList<ScriptTrait> value) : base(type, value)
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
