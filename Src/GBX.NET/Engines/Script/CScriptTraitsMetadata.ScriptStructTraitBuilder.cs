using System.Xml.Linq;

namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
{
    public class ScriptStructTraitBuilder
    {
        private readonly ScriptStructTypeBuilder typeBuilder;
        
        public Dictionary<string, ScriptTrait> Members { get; }

        public ScriptStructTraitBuilder(ScriptStructTypeBuilder typeBuilder)
        {
            this.typeBuilder = typeBuilder;

            Members = new();
        }

        public ScriptStructTraitBuilder(string structName) : this(new ScriptStructTypeBuilder(structName))
        {
        }

        public ScriptStructTraitBuilder WithBoolean(string name, bool value)
        {
            typeBuilder.WithBoolean(name);
            Members.Add(name, new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), value));
            return this;
        }

        public ScriptStructTraitBuilder WithInteger(string name, int value)
        {
            typeBuilder.WithInteger(name);
            Members.Add(name, new ScriptTrait<int>(new ScriptType(EScriptType.Integer), value));
            return this;
        }

        public ScriptStructTraitBuilder WithReal(string name, float value)
        {
            typeBuilder.WithReal(name);
            Members.Add(name, new ScriptTrait<float>(new ScriptType(EScriptType.Real), value));
            return this;
        }

        public ScriptStructTraitBuilder WithText(string name, string value)
        {
            typeBuilder.WithText(name);
            Members.Add(name, new ScriptTrait<string>(new ScriptType(EScriptType.Text), value));
            return this;
        }

        public ScriptStructTraitBuilder WithVec2(string name, Vec2 value)
        {
            typeBuilder.WithVec2(name);
            Members.Add(name, new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), value));
            return this;
        }

        public ScriptStructTraitBuilder WithVec3(string name, Vec3 value)
        {
            typeBuilder.WithVec3(name);
            Members.Add(name, new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), value));
            return this;
        }

        public ScriptStructTraitBuilder WithInt3(string name, Int3 value)
        {
            typeBuilder.WithInt3(name);
            Members.Add(name, new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), value));
            return this;
        }

        public ScriptStructTraitBuilder WithInt2(string name, Int2 value)
        {
            typeBuilder.WithInt2(name);
            Members.Add(name, new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), value));
            return this;
        }

        public ScriptStructTraitBuilder WithStruct(string name, ScriptStructTraitBuilder structTraitBuilder)
        {
            return WithStruct(name, structTraitBuilder.Build());
        }

        public ScriptStructTraitBuilder WithStruct(string name, ScriptStructTrait structTrait)
        {
            typeBuilder.WithStruct(name, (ScriptStructType)structTrait.Type);
            Members.Add(name, structTrait);
            return this;
        }

        public ScriptStructTrait Build()
        {
            return new ScriptStructTrait(typeBuilder.Build(), Members);
        }
    }
}
