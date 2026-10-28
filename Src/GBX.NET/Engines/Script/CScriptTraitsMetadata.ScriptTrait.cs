namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
{
    public abstract class ScriptTrait : IDeepCloneable
    {
        public IScriptType Type { get; private set; }

        public ScriptTrait(IScriptType type)
        {
            Type = type;
        }

        object IDeepCloneable.DeepClone(DeepCloneContext context)
        {
            var clone = (ScriptTrait)MemberwiseClone();
            context.Register(this, clone);
            clone.Type = context.Clone(Type)!;
            DeepCloneFields(clone, context);
            return clone;
        }

        internal virtual void DeepCloneFields(ScriptTrait clone, DeepCloneContext context) { }

        public override int GetHashCode()
        {
            return Type.GetHashCode() * -1521134295;
        }

        public override bool Equals(object? obj)
        {
            return obj is ScriptTrait other && Type.Equals(other.Type);
        }

        public abstract object GetValue();

        public override string ToString()
        {
            return Type.ToString() ?? "";
        }
    }
}
