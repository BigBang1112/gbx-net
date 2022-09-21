using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Engines.Script;

/// <remarks>ID: 0x11002000</remarks>
[Node(0x11002000)]
public partial class CScriptTraitsMetadata : CMwNod
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

    public void Declare(ScriptStructTraitBuilder valueBuilder)
    {
        Declare(valueBuilder.Build());
    }

    public void Declare(ScriptStructTrait value)
    {
        Remove(value.Name);
        Traits.Add(value);
    }

    public void Declare(string name, IEnumerable<ScriptStructTrait> value)
    {{
        Remove(name);
        Traits.Add(new ScriptArrayTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Void), new ScriptType(EScriptType.Struct)),
            name, value.Select(x => (ScriptTrait)x).ToList()));
    }}

    public ScriptStructTrait? GetStruct(string name)
    {
        return Get(name) as ScriptStructTrait;
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetStruct(string name, [NotNullWhen(true)] out ScriptStructTrait? value)
#else
    public bool TryGetStruct(string name, out ScriptStructTrait? value)
#endif
    {
        var val = GetStruct(name);
        value = val ?? default!;
        return val is not null;
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

                return new ScriptArrayTrait(arrayType, name, array);
            }

            var dictionary = new Dictionary<ScriptTrait, ScriptTrait>(arrayFieldCount);

            for (var i = 0; i < arrayFieldCount; i++)
            {
                var keyContents = ReadContents(r, name: "", arrayType.KeyType);
                var valueContents = ReadContents(r, name: "", arrayType.ValueType);

                dictionary[keyContents] = valueContents;
            }

            return new ScriptDictionaryTrait(arrayType, name, dictionary);
        }

        private void WriteScriptArray(GameBoxWriter w, ScriptArrayTrait arrayTrait)
        {
            if (Version >= 3)
            {
                w.Write((byte)arrayTrait.Value.Count);
            }
            else
            {
                w.Write(arrayTrait.Value.Count);
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

            return new ScriptStructTrait(structType, name, dictionary);
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
}
