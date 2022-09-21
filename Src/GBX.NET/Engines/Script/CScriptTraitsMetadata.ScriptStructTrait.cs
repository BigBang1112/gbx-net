using System.Text;

namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
{
    /// <summary>
    /// A simplified variant of <c>ScriptTrait&lt;IDictionary&lt;string, ScriptTrait&gt;&gt;</c>
    /// </summary>
    public class ScriptStructTrait : ScriptTrait<IDictionary<string, ScriptTrait>>
    {
        public ScriptStructTrait(ScriptStructType type, string name, IDictionary<string, ScriptTrait> value)
            : base(type, name, value)
        {
        }
        
        public static ScriptStructTraitBuilder Create(string structName, string traitName) => new(structName, traitName);

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
            builder.Append(" members)");

            return builder.ToString();
        }
    }
}
