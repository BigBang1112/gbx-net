namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
{
    public class ScriptStructTypeBuilder
    {        
        public string Name { get; }
        public Dictionary<string, ScriptTrait> Members { get; }

        public ScriptStructTypeBuilder(string name)
        {
            Name = name;
            Members = new();
        }

        public ScriptStructType Build()
        {
            return new ScriptStructType(Name, Members.Select(x => x.Value).ToArray());
        }

        public ScriptStructTraitBuilder Set(string traitName) => new(this, traitName);

        public ScriptStructTypeBuilder WithBoolean(string name)
        {
            if (Members.TryGetValue(name, out var trait) && trait.Type.Type == EScriptType.Boolean) return this;
            Members.Add(name, new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), name, default));
            return this;
        }

        public ScriptStructTypeBuilder WithInteger(string name)
        {
            if (Members.TryGetValue(name, out var trait) && trait.Type.Type == EScriptType.Integer) return this;
            Members.Add(name, new ScriptTrait<int>(new ScriptType(EScriptType.Integer), name, default));
            return this;
        }

        public ScriptStructTypeBuilder WithReal(string name)
        {
            if (Members.TryGetValue(name, out var trait) && trait.Type.Type == EScriptType.Real) return this;
            Members.Add(name, new ScriptTrait<float>(new ScriptType(EScriptType.Real), name, default));
            return this;
        }

        public ScriptStructTypeBuilder WithText(string name)
        {
            if (Members.TryGetValue(name, out var trait) && trait.Type.Type == EScriptType.Text) return this;
            Members.Add(name, new ScriptTrait<string>(new ScriptType(EScriptType.Text), name, ""));
            return this;
        }

        public ScriptStructTypeBuilder WithVec2(string name)
        {
            if (Members.TryGetValue(name, out var trait) && trait.Type.Type == EScriptType.Vec2) return this;
            Members.Add(name, new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), name, default));
            return this;
        }

        public ScriptStructTypeBuilder WithVec3(string name)
        {
            if (Members.TryGetValue(name, out var trait) && trait.Type.Type == EScriptType.Vec3) return this;
            Members.Add(name, new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), name, default));
            return this;
        }

        public ScriptStructTypeBuilder WithInt3(string name)
        {
            if (Members.TryGetValue(name, out var trait) && trait.Type.Type == EScriptType.Int3) return this;
            Members.Add(name, new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), name, default));
            return this;
        }
        
        public ScriptStructTypeBuilder WithInt2(string name)
        {
            if (Members.TryGetValue(name, out var trait) && trait.Type.Type == EScriptType.Int2) return this;
            Members.Add(name, new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), name, default));
            return this;
        }

        public ScriptStructTypeBuilder WithStruct(string name, ScriptStructTypeBuilder structTypeBuilder)
        {
            return WithStruct(name, structTypeBuilder.Build());
        }

        public ScriptStructTypeBuilder WithStruct(string name, ScriptStructType structType)
        {
            if (Members.TryGetValue(name, out var trait) && trait.Type.Type == EScriptType.Struct) return this;
            Members.Add(name, new ScriptStructTrait(structType, name, structType.Members.ToDictionary(x => x.Name)));
            return this;
        }
    }
}
