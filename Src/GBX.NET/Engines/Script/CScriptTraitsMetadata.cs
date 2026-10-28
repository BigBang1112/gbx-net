using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Engines.Script;

public partial class CScriptTraitsMetadata
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

    public IDictionary<string, ScriptTrait> Traits { get; set; } = new Dictionary<string, ScriptTrait>();

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

    public partial class Chunk11002000 : IVersionable
    {
        public int Version { get; set; } = 5;

        public override void Read(CScriptTraitsMetadata n, GbxReader r)
        {
            Version = r.ReadInt32();

            if (Version < 2 || Version > 6)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            // CScriptTraitsGenericContainer::Archive (version = Version - 2)
            n.Traits = ReadTraits(r, Version - 2);
        }

        public override void Write(CScriptTraitsMetadata n, GbxWriter w)
        {
            w.Write(Version);

            if (Version < 2 || Version > 6)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            WriteTraits(w, Version - 2, n.Traits);
        }
    }

    // CScriptTraitsGenericContainer::Archive
    internal static Dictionary<string, ScriptTrait> ReadTraits(GbxReader r, int version)
    {
        Dictionary<string, ScriptTrait> traits;

        // ArchiveWithTypeBuffer
        var typeOrTraitCount = version >= 1 ? r.ReadSmallLen() : r.ReadInt32();

        if (version < 3)
        {
            traits = new Dictionary<string, ScriptTrait>(typeOrTraitCount);

            for (var i = 0; i < typeOrTraitCount; i++)
            {
                var traitName = version >= 1 ? r.ReadSmallString() : r.ReadString();
                var type = ReadType(r, version);
                traits.Add(traitName, ReadContents(r, type, noContent: false, version));
            }

            return traits;
        }

        var types = new IScriptType[typeOrTraitCount];
        for (var i = 0; i < typeOrTraitCount; i++)
        {
            types[i] = ReadType(r, version);
        }

        var traitCount = r.ReadSmallLen();
        traits = new Dictionary<string, ScriptTrait>(traitCount);

        for (var i = 0; i < traitCount; i++)
        {
            var traitName = r.ReadSmallString();
            var typeIndex = r.ReadSmallLen();
            traits.Add(traitName, ReadContents(r, types[typeIndex], noContent: false, version));
        }

        return traits;
    }

    /// <summary>
    /// CScriptTraitsGenericContainer::ChunkContents
    /// </summary>
    /// <exception cref="NotSupportedException"></exception>
    internal static ScriptTrait ReadContents(GbxReader r, IScriptType type, bool noContent, int version) => type.Type switch
    {
        EScriptType.Boolean => new ScriptTrait<bool>(type, !noContent && r.ReadBoolean(asByte: version >= 1)),
        EScriptType.Integer => new ScriptTrait<int>(type, noContent ? default : r.ReadInt32()),
        EScriptType.Real => new ScriptTrait<float>(type, noContent ? default : r.ReadSingle()),
        EScriptType.Text => new ScriptTrait<string>(type, noContent ? "" : (version >= 1 ? r.ReadSmallString() : r.ReadString())),
        EScriptType.Array => ReadScriptArray(r, type, noContent, version),
        EScriptType.Vec2 => new ScriptTrait<Vec2>(type, noContent ? default : r.ReadVec2()),
        EScriptType.Vec3 => new ScriptTrait<Vec3>(type, noContent ? default : r.ReadVec3()),
        EScriptType.Int3 => new ScriptTrait<Int3>(type, noContent ? default : r.ReadInt3()),
        EScriptType.Int2 => new ScriptTrait<Int2>(type, noContent ? default : r.ReadInt2()),
        EScriptType.Struct => ReadScriptStruct(r, type, noContent, version),
        _ => throw new NotSupportedException($"{type} is not supported.")
    }; 
    
    private static ScriptTrait ReadScriptArray(GbxReader r, IScriptType type, bool noContent, int version)
    {
        if (type is not ScriptArrayType arrayType)
        {
            throw new Exception("EScriptType.Array not matching ScriptArrayType");
        }

        var arrayFieldCount = noContent ? 0 : (version >= 1 ? r.ReadSmallLen() : r.ReadInt32());
        var isRegularArray = arrayType.KeyType.Type == EScriptType.Void;

        if (isRegularArray)
        {
            var array = new ScriptTrait[arrayFieldCount];

            for (var i = 0; i < arrayFieldCount; i++)
            {
                var valueContents = ReadContents(r, arrayType.ValueType, noContent, version);

                array[i] = valueContents;
            }

            return new ScriptArrayTrait(arrayType, array);
        }

        var dictionary = new Dictionary<ScriptTrait, ScriptTrait>(arrayFieldCount);

        for (var i = 0; i < arrayFieldCount; i++)
        {
            var keyContents = ReadContents(r, arrayType.KeyType, noContent, version);
            var valueContents = ReadContents(r, arrayType.ValueType, noContent, version);

            dictionary[keyContents] = valueContents;
        }

        return new ScriptDictionaryTrait(arrayType, dictionary);
    }

    /// <summary>
    /// CScriptTraitsGenericContainer::ChunkType
    /// </summary>
    internal static IScriptType ReadType(GbxReader r, int version)
    {
        var type = (EScriptType)(version >= 1 ? r.ReadByte() : r.ReadInt32());

        switch (type)
        {
            case EScriptType.Array:
                var key = ReadType(r, version); // CScriptType::KeyType
                var value = ReadType(r, version); // CScriptType::ValueType
                return new ScriptArrayType(key, value);
            case EScriptType.Struct:

                if (version < 2) throw new StructsNotSupportedException();

                var memberCount = r.ReadByte(); // CScriptType::StructMemberCount
                var structName = r.ReadString();

                var members = new Dictionary<string, ScriptTrait>(memberCount);

                for (var i = 0; i < memberCount; i++)
                {
                    var memberName = r.ReadString();
                    var memberType = ReadType(r, version);

                    members.Add(memberName, ReadContents(r, memberType, noContent: version >= 4, version));
                }

                return new ScriptStructType(structName, members);
        }

        return new ScriptType(type);
    }

    private static ScriptStructTrait ReadScriptStruct(GbxReader r, IScriptType type, bool noContent, int version)
    {
        if (version < 2)
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
            dictionary[member.Key] = ReadContents(r, member.Value.Type, noContent, version);
        }

        return new ScriptStructTrait(structType, dictionary);
    }

    // CScriptTraitsGenericContainer::Archive
    internal static void WriteTraits(GbxWriter w, int version, IDictionary<string, ScriptTrait> traits)
    {
        if (version < 3)
        {
            if (version >= 1)
            {
                w.WriteSmallLen(traits?.Count ?? 0);
            }
            else
            {
                w.Write(traits?.Count ?? 0);
            }

            if (traits is null) return;

            foreach (var trait in traits)
            {
                if (version >= 1)
                {
                    w.WriteSmallString(trait.Key);
                }
                else
                {
                    w.Write(trait.Key);
                }

                WriteType(w, trait.Value.Type, version);
                WriteContents(w, trait.Value, version);
            }

            return;
        }

        var uniqueTypes = new Dictionary<IScriptType, int>();

        if (traits is not null)
        {
            foreach (var type in traits.Select(x => x.Value.Type).Distinct())
            {
                uniqueTypes.Add(type, uniqueTypes.Count);
            }
        }

        w.WriteSmallLen(uniqueTypes.Count);

        foreach (var type in uniqueTypes)
        {
            WriteType(w, type.Key, version);
        }

        w.WriteSmallLen(traits?.Count ?? 0);

        if (traits is null) return;

        foreach (var trait in traits)
        {
            w.WriteSmallString(trait.Key);
            w.WriteSmallLen(uniqueTypes[trait.Value.Type]);
            WriteContents(w, trait.Value, version);
        }
    }

    internal static void WriteType(GbxWriter w, IScriptType type, int version)
    {
        if (version >= 1)
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
                WriteType(w, arrayType.KeyType, version); // CScriptType::KeyType
                WriteType(w, arrayType.ValueType, version); // CScriptType::ValueType
                break;
            case ScriptStructType structType:

                if (version < 2) throw new StructsNotSupportedException();

                w.Write((byte)structType.Members.Count); // CScriptType::StructMemberCount
                w.Write(structType.Name);

                foreach (var member in structType.Members)
                {
                    w.Write(member.Key);
                    WriteType(w, member.Value.Type, version);

                    if (version < 4)
                    {
                        WriteContents(w, member.Value, version);
                    }
                }

                break;
        }
    }

    internal static void WriteContents(GbxWriter w, ScriptTrait trait, int version)
    {
        switch (trait)
        {
            case ScriptTrait<bool> boolTrait:
                w.Write(boolTrait.Value, asByte: version >= 1);
                break;
            case ScriptTrait<int> intTrait:
                w.Write(intTrait.Value);
                break;
            case ScriptTrait<float> floatTrait:
                w.Write(floatTrait.Value);
                break;
            case ScriptTrait<string> stringTrait:
                if (version >= 1)
                {
                    w.WriteSmallString(stringTrait.Value);
                }
                else
                {
                    w.Write(stringTrait.Value);
                }
                break;
            case ScriptArrayTrait arrayTrait:
                WriteScriptArray(w, arrayTrait, version);
                break;
            case ScriptDictionaryTrait dictionaryTrait:
                WriteScriptDictionary(w, dictionaryTrait, version);
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
                WriteScriptStruct(w, structTrait, version);
                break;
            default:
                throw new NotSupportedException($"{trait.Type.Type} is not supported.");
        }
    }



    private static void WriteScriptArray(GbxWriter w, ScriptArrayTrait arrayTrait, int version)
    {
        if (version >= 1)
        {
            w.WriteSmallLen(arrayTrait.Value.Count);
        }
        else
        {
            w.Write(arrayTrait.Value.Count);
        }

        foreach (var trait in arrayTrait.Value)
        {
            WriteContents(w, trait, version);
        }
    }

    private static void WriteScriptDictionary(GbxWriter w, ScriptDictionaryTrait dictionaryTrait, int version)
    {
        if (version >= 1)
        {
            w.WriteSmallLen(dictionaryTrait.Value.Count);
        }
        else
        {
            w.Write(dictionaryTrait.Value.Count);
        }

        foreach (var pair in dictionaryTrait.Value)
        {
            WriteContents(w, pair.Key, version);
            WriteContents(w, pair.Value, version);
        }
    }

    private static void WriteScriptStruct(GbxWriter w, ScriptStructTrait structTrait, int version)
    {
        if (version < 2)
        {
            throw new StructsNotSupportedException();
        }

        foreach (var member in structTrait.Value)
        {
            WriteContents(w, member.Value, version);
        }
    }
}
