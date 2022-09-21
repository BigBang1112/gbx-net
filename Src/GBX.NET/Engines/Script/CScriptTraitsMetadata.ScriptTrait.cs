namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
{
    public abstract class ScriptTrait
    {
        public IScriptType Type { get; }

        public ScriptTrait(IScriptType type)
        {
            Type = type;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() * -1521134295;
        }

        public override bool Equals(object? obj)
        {
            return obj is ScriptTrait other && Type.Equals(other.Type);
        }

        public override string ToString()
        {
            return Type.ToString() ?? "";
        }
    }
}
