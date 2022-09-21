using System.Text;

namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
{
    public abstract class ScriptTrait
    {
        public IScriptType Type { get; }
        public string Name { get; }

        public ScriptTrait(IScriptType type, string name)
        {
            Type = type;
            Name = name;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() * -1521134295 + Name.GetHashCode() * -1521134295;
        }

        public override bool Equals(object? obj)
        {
            return obj is ScriptTrait other
                && Type.Equals(other.Type)
                && Name.Equals(other.Name);
        }

        public override string ToString()
        {
            var builder = new StringBuilder(Type.ToString());

            if (!string.IsNullOrWhiteSpace(Name))
            {
                builder.Append(' ');
                builder.Append(Name);
            }

            return builder.ToString();
        }
    }
}
