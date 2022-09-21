using System.Text;

namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
{
    /// <summary>
    /// A simplified variant of <c>ScriptTrait&lt;IDictionary&lt;string, ScriptTrait&gt;&gt;</c>
    /// </summary>
    public class ScriptStructTrait : ScriptTrait<IDictionary<string, ScriptTrait>>
    {
        public ScriptStructTrait(ScriptStructType type, IDictionary<string, ScriptTrait> value)
            : base(type, value)
        {
        }
        
        public static ScriptStructTraitBuilder Create(string structName) => new(structName);

        public override string ToString()
        {
            var builder = new StringBuilder(Type.ToString());

            builder.Append(" (");
            builder.Append(Value.Count);
            builder.Append(" members)");

            return builder.ToString();
        }
    }
}
