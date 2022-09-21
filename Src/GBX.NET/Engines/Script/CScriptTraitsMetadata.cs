using System.Text;

namespace GBX.NET.Engines.Script;

[Node(0x11002000)]
public class CScriptTraitsMetadata : CMwNod
{
    /// <summary>
    /// Type of the variable supported by ManiaScript.
    /// </summary>
    public enum EScriptType
    {
        Void,
        Boolean,
        Integer,
        Real,
        Class,
        Text,
        Enum,
        Array,
        ParamArray,
        Vec2,
        Vec3,
        Int3,
        Iso4,
        Ident,
        Int2,
        Struct,
        ValueNotComputed
    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk11002000))]
    public IList<ScriptTrait> Traits { get; set; }

    protected CScriptTraitsMetadata()
	{
        Traits = Array.Empty<ScriptTrait>();
    }
    
    public void Declare(string name, bool value)
    {
        Remove(name);
        Traits.Add(new ScriptTrait<bool>(new ScriptType(EScriptType.Boolean), name, value));
    }

    public void Declare(string name, int value)
    {
        Remove(name);
        Traits.Add(new ScriptTrait<int>(new ScriptType(EScriptType.Integer), name, value));
    }

    public void Declare(string name, float value)
    {
        Remove(name);
        Traits.Add(new ScriptTrait<float>(new ScriptType(EScriptType.Real), name, value));
    }

    public void Declare(string name, string value)
    {
        Remove(name);
        Traits.Add(new ScriptTrait<string>(new ScriptType(EScriptType.Text), name, value));
    }

    public void Declare(string name, Vec2 value)
    {
        Remove(name);
        Traits.Add(new ScriptTrait<Vec2>(new ScriptType(EScriptType.Vec2), name, value));
    }

    public void Declare(string name, Vec3 value)
    {
        Remove(name);
        Traits.Add(new ScriptTrait<Vec3>(new ScriptType(EScriptType.Vec3), name, value));
    }

    public void Declare(string name, Int3 value)
    {
        Remove(name);
        Traits.Add(new ScriptTrait<Int3>(new ScriptType(EScriptType.Int3), name, value));
    }

    public void Declare(string name, Int2 value)
    {
        Remove(name);
        Traits.Add(new ScriptTrait<Int2>(new ScriptType(EScriptType.Int2), name, value));
    }

    public ScriptTrait? Get(string name)
    {
        return Traits.FirstOrDefault(x => x.Name == name);
    }

    public bool Remove(string name)
    {
        return Traits.RemoveAll(x => x.Name == name) > 0;
    }

    public void ClearMetadata()
    {
        Traits.Clear();
    }

    #region 0x000 chunk

    /// <summary>
    /// CScriptTraitsMetadata 0x000 chunk
    /// </summary>
    [Chunk(0x11002000)]
    public class Chunk11002000 : Chunk<CScriptTraitsMetadata>, IVersionable
    {
        public int Version { get; set; }

        public override void Read(CScriptTraitsMetadata n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            if (Version < 2)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            // CScriptTraitsGenericContainer::Archive (version = Version - 2)

            // ArchiveWithTypeBuffer
            var typeOrTraitCount = Version >= 3 ? r.ReadByte() : r.ReadInt32();

            if (Version < 5)
            {
                n.Traits = new List<ScriptTrait>(typeOrTraitCount);

                for (var i = 0; i < typeOrTraitCount; i++)
                {
                    var traitName = r.ReadString(Version >= 3 ? StringLengthPrefix.Byte : StringLengthPrefix.Int32);
                    var type = ReadType(r);
                    n.Traits.Add(ReadContents(r, traitName, type));
                }

                return;
            }

            var types = r.ReadArray(typeOrTraitCount, ReadType);

            var traitCount = r.ReadByte();
            n.Traits = new List<ScriptTrait>(traitCount);

            for (var i = 0; i < traitCount; i++)
            {
                var traitName = r.ReadString(StringLengthPrefix.Byte);
                var typeIndex = r.ReadByte();
                n.Traits.Add(ReadContents(r, traitName, types[typeIndex]));
            }
        }

        public override void Write(CScriptTraitsMetadata n, GameBoxWriter w)
        {
            w.Write(Version);

            if (Version < 2)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }
            
            if (Version < 5)
            {
                if (Version >= 3)
                {
                    w.Write((byte)n.Traits.Count);
                }
                else
                {
                    w.Write(n.Traits.Count);
                }

                foreach (var trait in n.Traits)
                {
                    w.Write(trait.Name, Version >= 3 ? StringLengthPrefix.Byte : StringLengthPrefix.Int32);
                    WriteType(w, trait.Type);
                    WriteContents(w, trait);
                }

                return;
            }

            var uniqueTypes = new Dictionary<IScriptType, int>();
            
            foreach (var type in n.Traits.Select(x => x.Type).Distinct())
            {
                uniqueTypes.Add(type, uniqueTypes.Count);
            }

            w.Write((byte)uniqueTypes.Count);

            foreach (var type in uniqueTypes)
            {
                WriteType(w, type.Key);
            }
            
            w.Write((byte)n.Traits.Count);
            
            foreach (var trait in n.Traits)
            {
                w.Write(trait.Name, StringLengthPrefix.Byte);
                w.Write((byte)uniqueTypes[trait.Type]);
                WriteContents(w, trait);
            }
        }

        /// <summary>
        /// CScriptTraitsGenericContainer::ChunkType
        /// </summary>
        private IScriptType ReadType(GameBoxReader r)
        {
            var type = (EScriptType)(Version >= 3 ? r.ReadByte() : r.ReadInt32());

            switch (type)
            {
                case EScriptType.Array:
                    var key = ReadType(r); // CScriptType::KeyType
                    var value = ReadType(r); // CScriptType::ValueType
                    return new ScriptArrayType(key, value);
                case EScriptType.Struct:
                    
                    if (Version < 4) throw new StructsNotSupportedException();
                    
                    var memberCount = r.ReadByte(); // CScriptType::StructMemberCount
                    var structName = r.ReadString();
                    var members = r.ReadArray(memberCount, r =>
                    {
                        var memberName = r.ReadString();
                        var memberType = ReadType(r);
                        return ReadContents(r, memberName, memberType);
                    });

                    return new ScriptStructType(structName, members);
            }
            
            return new ScriptType(type);
        }

        private void WriteType(GameBoxWriter w, IScriptType type)
        {
            if (Version >= 3)
            {
                w.Write((byte)type.Type);
            }
            else
            {
                w.Write((int)type.Type);
            }
            
            switch (type)
            {
                case ScriptArrayType arrayType:
                    WriteType(w, arrayType.KeyType); // CScriptType::KeyType
                    WriteType(w, arrayType.ValueType); // CScriptType::ValueType
                    break;
                case ScriptStructType structType:
                    
                    if (Version < 4) throw new StructsNotSupportedException();
                    
                    w.Write((byte)structType.Members.Length); // CScriptType::StructMemberCount
                    w.Write(structType.Name);
                    
                    foreach (var member in structType.Members)
                    {
                        w.Write(member.Name);
                        WriteType(w, member.Type);
                        WriteContents(w, member);
                    }

                    break;
            }
        }

        /// <summary>
        /// CScriptTraitsGenericContainer::ChunkContents
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        private ScriptTrait ReadContents(GameBoxReader r, string name, IScriptType type) => type.Type switch
        {
            EScriptType.Boolean => new ScriptTrait<bool>(type, name, r.ReadBoolean(asByte: Version >= 3)),
            EScriptType.Integer => new ScriptTrait<int>(type, name, r.ReadInt32()),
            EScriptType.Real => new ScriptTrait<float>(type, name, r.ReadSingle()),
            EScriptType.Text => new ScriptTrait<string>(type, name, r.ReadString(Version >= 3 ? StringLengthPrefix.Byte : StringLengthPrefix.Int32)),
            EScriptType.Array => ReadScriptArray(r, name, type),
            EScriptType.Vec2 => new ScriptTrait<Vec2>(type, name, r.ReadVec2()),
            EScriptType.Vec3 => new ScriptTrait<Vec3>(type, name, r.ReadVec3()),
            EScriptType.Int3 => new ScriptTrait<Int3>(type, name, r.ReadInt3()),
            EScriptType.Int2 => new ScriptTrait<Int2>(type, name, r.ReadInt2()),
            EScriptType.Struct => ReadScriptStruct(r, name, type),
            _ => throw new NotSupportedException($"{type} is not supported.")
        };

        private void WriteContents(GameBoxWriter w, ScriptTrait trait)
        {
            switch (trait)
            {
                case ScriptTrait<bool> boolTrait:
                    w.Write(boolTrait.Value, asByte: Version >= 3);
                    break;
                case ScriptTrait<int> intTrait:
                    w.Write(intTrait.Value);
                    break;
                case ScriptTrait<float> floatTrait:
                    w.Write(floatTrait.Value);
                    break;
                case ScriptTrait<string> stringTrait:
                    w.Write(stringTrait.Value, Version >= 3 ? StringLengthPrefix.Byte : StringLengthPrefix.Int32);
                    break;
                case ScriptArrayTrait arrayTrait:
                    WriteScriptArray(w, arrayTrait);
                    break;
                case ScriptDictionaryTrait dictionaryTrait:
                    WriteScriptDictionary(w, dictionaryTrait);
                    break;
                case ScriptTrait<Vec2> vec2Trait:
                    w.Write(vec2Trait.Value);
                    break;
                case ScriptTrait<Vec3> vec3Trait:
                    w.Write(vec3Trait.Value);
                    break;
                case ScriptTrait<Int3> int3Trait:
                    w.Write(int3Trait.Value);
                    break;
                case ScriptTrait<Int2> int2Trait:
                    w.Write(int2Trait.Value);
                    break;
                case ScriptStructTrait structTrait:
                    WriteScriptStruct(w, structTrait);
                    break;
                default:
                    throw new NotSupportedException($"{trait.Type.Type} is not supported.");
            }
        }

        private ScriptTrait ReadScriptArray(GameBoxReader r, string name, IScriptType type)
        {
            if (type is not ScriptArrayType arrayType)
            {
                throw new Exception("EScriptType.Array not matching ScriptArrayType");
            }

            var arrayFieldCount = Version >= 3 ? r.ReadByte() : r.ReadInt32();
            var isRegularArray = arrayType.KeyType.Type == EScriptType.Void;

            if (isRegularArray)
            {
                var array = new ScriptTrait[arrayFieldCount];

                for (var i = 0; i < arrayFieldCount; i++)
                {
                    var valueContents = ReadContents(r, name: "", arrayType.ValueType);

                    array[i] = valueContents;
                }

                return new ScriptArrayTrait(type, name, array);
            }

            var dictionary = new Dictionary<ScriptTrait, ScriptTrait>(arrayFieldCount);

            for (var i = 0; i < arrayFieldCount; i++)
            {
                var keyContents = ReadContents(r, name: "", arrayType.KeyType);
                var valueContents = ReadContents(r, name: "", arrayType.ValueType);

                dictionary[keyContents] = valueContents;
            }

            return new ScriptDictionaryTrait(type, name, dictionary);
        }

        private void WriteScriptArray(GameBoxWriter w, ScriptArrayTrait arrayTrait)
        {
            if (Version >= 3)
            {
                w.Write((byte)arrayTrait.Value.Length);
            }
            else
            {
                w.Write(arrayTrait.Value.Length);
            }

            foreach (var trait in arrayTrait.Value)
            {
                WriteContents(w, trait);
            }
        }

        private void WriteScriptDictionary(GameBoxWriter w, ScriptDictionaryTrait dictionaryTrait)
        {
            if (Version >= 3)
            {
                w.Write((byte)dictionaryTrait.Value.Count);
            }
            else
            {
                w.Write(dictionaryTrait.Value.Count);
            }

            foreach (var pair in dictionaryTrait.Value)
            {
                WriteContents(w, pair.Key);
                WriteContents(w, pair.Value);
            }
        }

        private ScriptStructTrait ReadScriptStruct(GameBoxReader r, string name, IScriptType type)
        {
            if (Version < 4)
            {
                throw new StructsNotSupportedException();
            }

            if (type is not ScriptStructType structType)
            {
                throw new Exception("EScriptType.Struct not matching ScriptStructType");
            }
            
            var dictionary = new Dictionary<string, ScriptTrait>(structType.Members.Length);

            foreach (var member in structType.Members)
            {
                dictionary[member.Name] = ReadContents(r, member.Name, member.Type);
            }

            return new ScriptStructTrait(type, name, dictionary);
        }

        private void WriteScriptStruct(GameBoxWriter w, ScriptStructTrait structTrait)
        {
            if (Version < 4)
            {
                throw new StructsNotSupportedException();
            }

            foreach (var member in structTrait.Value)
            {
                WriteContents(w, member.Value);
            }
        }
    }

    #endregion

    public interface IScriptType
    {
        EScriptType Type { get; }
    }

    public readonly record struct ScriptType(EScriptType Type) : IScriptType
    {
        public override string ToString()
        {
            return Type.ToString();
        }
    }

    public readonly record struct ScriptArrayType(IScriptType KeyType, IScriptType ValueType) : IScriptType
    {
        public EScriptType Type => EScriptType.Array;

        public override string ToString()
        {
            var result = new StringBuilder(ValueType.ToString());
            result.Append('[');
            result.Append(KeyType.ToString());
            result.Append(']');
            return result.ToString();
        }
    }

    public sealed class ScriptStructType : IScriptType
    {
        public EScriptType Type => EScriptType.Struct;
        
        public string Name { get; }
        public ScriptTrait[] Members { get; }

        public ScriptStructType(string name, ScriptTrait[] members)
        {
            Name = name;
            Members = members;
        }

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

    /// <summary>
    /// A simplified variant of <c>ScriptTrait&lt;Dictionary&lt;string, ScriptTrait&gt;&gt;</c>
    /// </summary>
    public class ScriptStructTrait : ScriptTrait<Dictionary<string, ScriptTrait>>
    {
        public ScriptStructTrait(IScriptType type, string name, Dictionary<string, ScriptTrait> value)
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
            builder.Append(" members)");

            return builder.ToString();
        }
    }

    /// <summary>
    /// A simplified variant of <c>ScriptTrait&lt;Dictionary&lt;ScriptTrait, ScriptTrait&gt;&gt;</c>
    /// </summary>
    public class ScriptDictionaryTrait : ScriptTrait<Dictionary<ScriptTrait, ScriptTrait>>
    {
        public ScriptDictionaryTrait(IScriptType type, string name, Dictionary<ScriptTrait, ScriptTrait> value)
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

    /// <summary>
    /// A simplified variant of <c>ScriptTrait&lt;ScriptTrait[]&gt;</c>
    /// </summary>
    public class ScriptArrayTrait : ScriptTrait<ScriptTrait[]>
    {
        public ScriptArrayTrait(IScriptType type, string name, ScriptTrait[] value)
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
            builder.Append(Value.Length);
            builder.Append(" elements)");

            return builder.ToString();
        }
    }
}
