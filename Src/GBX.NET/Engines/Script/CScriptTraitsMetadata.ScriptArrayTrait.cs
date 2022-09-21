using System.Text;

namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
{
    /// <summary>
    /// A simplified variant of <c>ScriptTrait&lt;IList&lt;ScriptTrait&gt&gt;</c>
    /// </summary>
    public class ScriptArrayTrait : ScriptTrait<IList<ScriptTrait>>
    {
        public ScriptArrayTrait(ScriptArrayType type, string name, IList<ScriptTrait> value)
            : base(type, name, value)
        {
        }

        public override string ToString()
        {
            var builder = new StringBuilder(Type.ToString());

            if (!string.IsNullOrWhiteSpace(Name))
            {
                builder.Append(' ');
                builder.Append(Name);
            }

            builder.Append(" (");
            builder.Append(Value.Count);
            builder.Append(" elements)");

            return builder.ToString();
        }
    }
}
