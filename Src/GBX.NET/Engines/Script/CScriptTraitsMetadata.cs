using GBX.NET.Builders.Engines.Script;
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
    [AppliedWithChunk<Chunk11002000>]
    public IDictionary<string, ScriptTrait> Traits { get; set; }

    internal CScriptTraitsMetadata()
	{
#if NET6_0_OR_GREATER
        Traits = global::System.Collections.Immutable.ImmutableDictionary.Create<string, ScriptTrait>();
#else
        Traits = null!;
#endif
    }

    public static CScriptTraitsMetadataBuilder Create() => new();

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGet(string name, [NotNullWhen(true)] out ScriptTrait? trait)
#else
    public bool TryGet(string name, out ScriptTrait trait)
#endif
    {
        return Traits.TryGetValue(name, out trait!);
    }

    public ScriptTrait? Get(string name)
    {
        return TryGet(name, out var trait) ? trait : null;
    }

    public bool Remove(string name)
    {
        return Traits.Remove(name);
    }

    public void ClearMetadata()
    {
        Traits.Clear();
    }

    /// <summary>
    /// Declares a metadata variable as <c>Struct</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="valueBuilder">A value of Struct builder.</param>
    public void Declare(string name, ScriptStructTraitBuilder valueBuilder)
    {
        Declare(name, valueBuilder.Build());
    }

    /// <summary>
    /// Declares a metadata variable as <c>Struct</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">A value of Struct.</param>
    public void Declare(string name, ScriptStructTrait value)
    {
        Traits[name] = value;
    }

    /// <summary>
    /// Declares a metadata array variable as <c>Struct[Void]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any enumerable of Struct. It is always reconstructed into a new list.</param>
    public void Declare(string name, IEnumerable<ScriptStructTrait> value)
    {
        Traits[name] = new ScriptArrayTrait(
            new ScriptArrayType(new ScriptType(EScriptType.Void), new ScriptType(EScriptType.Struct)),
            value.Select(x => (ScriptTrait)x).ToList());
    }

    /// <summary>
    /// Declares a metadata array variable as <c>Struct[Void]</c>.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">Any enumerable of Struct builder. It is always reconstructed into a new list.</param>
    public void Declare(string name, IEnumerable<ScriptStructTraitBuilder> value)
    {
        Declare(name, value.Select(x => x.Build()));
    }

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

    public static ScriptStructTypeBuilder DefineStruct(string name) => ScriptStructType.Create(name);
    public static ScriptStructTraitBuilder CreateStruct(string name) => ScriptStructTrait.Create(name);

    #region 0x000 chunk

    /// <summary>
    /// CScriptTraitsMetadata 0x000 chunk
    /// </summary>
    [Chunk(0x11002000)]
    public class Chunk11002000 : Chunk<CScriptTraitsMetadata>, IVersionable
    {
        public int Version { get; set; } = 5;

        public override void Read(CScriptTraitsMetadata n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            if (Version < 2 || Version > 6)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            // CScriptTraitsGenericContainer::Archive (version = Version - 2)

            // ArchiveWithTypeBuffer
            var typeOrTraitCount = Version >= 3 ? r.ReadSmallLen() : r.ReadInt32();

            if (Version < 5)
            {
                n.Traits = new Dictionary<string, ScriptTrait>(typeOrTraitCount);

                for (var i = 0; i < typeOrTraitCount; i++)
                {
                    var traitName = Version >= 3 ? r.ReadSmallString() : r.ReadString();
                    var type = ReadType(r);
                    n.Traits.Add(traitName, ReadContents(r, type, noContent: false));
                }

                return;
            }

            var types = r.ReadArray(typeOrTraitCount, ReadType);

            var traitCount = r.ReadSmallLen();
            n.Traits = new Dictionary<string, ScriptTrait>(traitCount);

            for (var i = 0; i < traitCount; i++)
            {
                var traitName = r.ReadSmallString();
                var typeIndex = r.ReadSmallLen();
                n.Traits.Add(traitName, ReadContents(r, types[typeIndex], noContent: false));
            }
        }

        public override void Write(CScriptTraitsMetadata n, GameBoxWriter w)
        {
            w.Write(Version);

            if (Version < 2 || Version > 6)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }
            
            if (Version < 5)
            {
                if (Version >= 3)
                {
                    w.WriteSmallLen(n.Traits.Count);
                }
                else
                {
                    w.Write(n.Traits.Count);
                }

                foreach (var trait in n.Traits)
                {
                    if (Version >= 3)
                    {
                        w.WriteSmallString(trait.Key);
                    }
                    else
                    {
                        w.Write(trait.Key);
                    }
                    
                    WriteType(w, trait.Value.Type);
                    WriteContents(w, trait.Value);
                }

                return;
            }

            var uniqueTypes = new Dictionary<IScriptType, int>();
            
            foreach (var type in n.Traits.Select(x => x.Value.Type).Distinct())
            {
                uniqueTypes.Add(type, uniqueTypes.Count);
            }

            w.WriteSmallLen(uniqueTypes.Count);

            foreach (var type in uniqueTypes)
            {
                WriteType(w, type.Key);
            }
            
            w.WriteSmallLen(n.Traits.Count);
            
            foreach (var trait in n.Traits)
            {
                w.WriteSmallString(trait.Key);
                w.WriteSmallLen(uniqueTypes[trait.Value.Type]);
                WriteContents(w, trait.Value);
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

                    var members = new Dictionary<string, ScriptTrait>(memberCount);

                    for (var i = 0; i < memberCount; i++)
                    {
                        var memberName = r.ReadString();
                        var memberType = ReadType(r);
                        
                        members.Add(memberName, ReadContents(r, memberType, noContent: Version >= 6));
                    }

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
                    
                    w.Write((byte)structType.Members.Count); // CScriptType::StructMemberCount
                    w.Write(structType.Name);
                    
                    foreach (var member in structType.Members)
                    {
                        w.Write(member.Key);
                        WriteType(w, member.Value.Type);

                        if (Version < 6)
                        {
                            WriteContents(w, member.Value);
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// CScriptTraitsGenericContainer::ChunkContents
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        private ScriptTrait ReadContents(GameBoxReader r, IScriptType type, bool noContent) => type.Type switch
        {
            EScriptType.Boolean => new ScriptTrait<bool>(type, !noContent && r.ReadBoolean(asByte: Version >= 3)),
            EScriptType.Integer => new ScriptTrait<int>(type, noContent ? default : r.ReadInt32()),
            EScriptType.Real => new ScriptTrait<float>(type, noContent ? default : r.ReadSingle()),
            EScriptType.Text => new ScriptTrait<string>(type, noContent ? "" : (Version >= 3 ? r.ReadSmallString() : r.ReadString())),
            EScriptType.Array => ReadScriptArray(r, type, noContent),
            EScriptType.Vec2 => new ScriptTrait<Vec2>(type, noContent ? default : r.ReadVec2()),
            EScriptType.Vec3 => new ScriptTrait<Vec3>(type, noContent ? default : r.ReadVec3()),
            EScriptType.Int3 => new ScriptTrait<Int3>(type, noContent ? default : r.ReadInt3()),
            EScriptType.Int2 => new ScriptTrait<Int2>(type, noContent ? default : r.ReadInt2()),
            EScriptType.Struct => ReadScriptStruct(r, type, noContent),
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
                    if (Version >= 3)
                    {
                        w.WriteSmallString(stringTrait.Value);
                    }
                    else
                    {
                        w.Write(stringTrait.Value);
                    }
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

        private ScriptTrait ReadScriptArray(GameBoxReader r, IScriptType type, bool noContent)
        {
            if (type is not ScriptArrayType arrayType)
            {
                throw new Exception("EScriptType.Array not matching ScriptArrayType");
            }

            var arrayFieldCount = Version >= 3 ? r.ReadSmallLen() : r.ReadInt32();
            var isRegularArray = arrayType.KeyType.Type == EScriptType.Void;

            if (isRegularArray)
            {
                var array = new ScriptTrait[arrayFieldCount];

                for (var i = 0; i < arrayFieldCount; i++)
                {
                    var valueContents = ReadContents(r, arrayType.ValueType, noContent);

                    array[i] = valueContents;
                }

                return new ScriptArrayTrait(arrayType, array);
            }

            var dictionary = new Dictionary<ScriptTrait, ScriptTrait>(arrayFieldCount);

            for (var i = 0; i < arrayFieldCount; i++)
            {
                var keyContents = ReadContents(r, arrayType.KeyType, noContent);
                var valueContents = ReadContents(r, arrayType.ValueType, noContent);

                dictionary[keyContents] = valueContents;
            }

            return new ScriptDictionaryTrait(arrayType, dictionary);
        }

        private void WriteScriptArray(GameBoxWriter w, ScriptArrayTrait arrayTrait)
        {
            if (Version >= 3)
            {
                w.WriteSmallLen(arrayTrait.Value.Count);
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
                w.WriteSmallLen(dictionaryTrait.Value.Count);
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

        private ScriptStructTrait ReadScriptStruct(GameBoxReader r, IScriptType type, bool noContent)
        {
            if (Version < 4)
            {
                throw new StructsNotSupportedException();
            }

            if (type is not ScriptStructType structType)
            {
                throw new Exception("EScriptType.Struct not matching ScriptStructType");
            }
            
            var dictionary = new Dictionary<string, ScriptTrait>(structType.Members.Count);

            foreach (var member in structType.Members)
            {
                dictionary[member.Key] = ReadContents(r, member.Value.Type, noContent);
            }

            return new ScriptStructTrait(structType, dictionary);
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
