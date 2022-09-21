using System.Text;

namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
{
    public class ScriptTrait<T> : ScriptTrait where T : notnull
    {
        public T Value { get; set; }

        public ScriptTrait(IScriptType type, string name, T value) : base(type, name)
        {
            Value = value;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + Value.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is ScriptTrait<T> other
                && Type.Equals(other.Type)
                && Name.Equals(other.Name)
                && EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override string ToString()
        {
            var builder = new StringBuilder(base.ToString());

            builder.Append(" = ");

            if (Value is string str)
            {
                builder.Append('"');
                builder.Append(str);
                builder.Append('"');
            }
            else
            { 
                builder.Append(Value?.ToString() ?? "null");
            }

            return builder.ToString();
        }
    }
}
