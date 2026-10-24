namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
{
    public sealed class ScriptStructType : IScriptType, IDeepCloneable
    {
        public EScriptType Type => EScriptType.Struct;
        
        public string Name { get; }
        public IDictionary<string, ScriptTrait> Members { get; private set; }

        public ScriptStructType(string name, IDictionary<string, ScriptTrait> members)
        {
            Name = name;
            Members = members;
        }

        object IDeepCloneable.DeepClone(DeepCloneContext context)
        {
            var clone = new ScriptStructType(Name, new Dictionary<string, ScriptTrait>());
            context.Register(this, clone);
            clone.Members = context.CloneDictionary(Members)!;
            return clone;
        }

        public static ScriptStructTypeBuilder Create(string name) => new(name);

        // May not be any useful
        public override int GetHashCode()
        {
            return Type.GetHashCode() * -1521134295
                 + Name.GetHashCode() * -1521134295
                 + Members.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is ScriptStructType other
                && Type.Equals(other.Type)
                && Name.Equals(other.Name)
                && Members.SequenceEqual(other.Members);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
