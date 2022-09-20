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
    public ScriptTrait[] Traits { get; set; }

    protected CScriptTraitsMetadata()
	{
        Traits = Array.Empty<ScriptTrait>();
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
                return; // temporary
            }

            // CScriptTraitsGenericContainer::Archive (version = Version - 2)
            
            if (Version < 5)
            {
                return; // temporary
            }

            // ArchiveWithTypeBuffer
            var typeCount = r.ReadByte();
            var types = r.ReadArray(typeCount, ReadType);

            var traitCount = r.ReadByte();
            n.Traits = new ScriptTrait[traitCount];

            for (var i = 0; i < traitCount; i++)
            {
                var traitName = r.ReadString(StringLengthPrefix.Byte);
                var typeIndex = r.ReadByte();
                n.Traits[i] = ReadContents(r, traitName, types[typeIndex]);
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
        
        private ScriptTrait ReadContents(GameBoxReader r, string name, IScriptType type)
        {
            return new ScriptTrait(type, name, ReadContents(r, type));
        }

        private object ReadContents(GameBoxReader r, IScriptType type) => type.Type switch
        {
            EScriptType.Boolean => r.ReadBoolean(asByte: Version >= 3),
            EScriptType.Integer => r.ReadInt32(),
            EScriptType.Real => r.ReadSingle(),
            EScriptType.Text => r.ReadString(Version >= 3 ? StringLengthPrefix.Byte : StringLengthPrefix.Int32),
            EScriptType.Array => ReadScriptArray(r, type),
            EScriptType.Vec2 => r.ReadVec2(),
            EScriptType.Vec3 => r.ReadVec3(),
            EScriptType.Int3 => r.ReadInt3(),
            EScriptType.Int2 => r.ReadInt2(),
            EScriptType.Struct => ReadScriptStruct(r, type),
            _ => throw new NotSupportedException($"{type} is not supported.")
        };

        private object ReadScriptArray(GameBoxReader r, IScriptType type)
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

                return array;
            }
            else
            {
                var dictionary = new Dictionary<ScriptTrait, ScriptTrait>(arrayFieldCount);

                for (var i = 0; i < arrayFieldCount; i++)
                {
                    var keyContents = ReadContents(r, name: "", arrayType.KeyType);
                    var valueContents = ReadContents(r, name: "", arrayType.ValueType);

                    dictionary[keyContents] = valueContents;
                }

                return dictionary;
            }
        }

        private object ReadScriptStruct(GameBoxReader r, IScriptType type)
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

            return dictionary;
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

    public sealed class ScriptTrait
    {
        public IScriptType Type { get; }
        public string Name { get; }
        public object Value { get; set; }

        public ScriptTrait(IScriptType type, string name, object value)
        {
            Type = type;
            Name = name;
            Value = value;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() * -1521134295 + Name.GetHashCode() * -1521134295 + Value.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is ScriptTrait other
                && Type.Equals(other.Type)
                && Name.Equals(other.Name)
                && Value.Equals(other.Value);
        }

        public override string ToString()
        {
            var builder = new StringBuilder(Type.ToString());

            if (!string.IsNullOrWhiteSpace(Name))
            {
                builder.Append(' ');
                builder.Append(Name);
            }

            builder.Append(" = ");
            builder.Append(Value?.ToString() ?? "null");

            return builder.ToString();
        }
    }
}
