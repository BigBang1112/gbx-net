using System.Text;

namespace GBX.NET.Engines.Script;

public sealed class CScriptTraitsMetadata_Old
{
    /// <summary>
    /// Type of the variable supported by ManiaScript.
    /// </summary>
    public enum ScriptType
    {
        Void,
        Boolean,
        Integer,
        Real,
        /// <summary>
        /// Not allowed for metadata.
        /// </summary>
        Class,
        Text,
        Enum,
        Array,
        ParamArray,
        Vec2,
        Vec3,
        Int3,
        /// <summary>
        /// Not allowed for metadata.
        /// </summary>
        Iso4,
        /// <summary>
        /// Not allowed for metadata.
        /// </summary>
        Ident,
        Int2,
        Struct
    }

    public int Version { get; set; } = 5;

    [Hexadecimal]
    public uint ClassId { get; set; } = 0x11002000;

    public List<ScriptVariable> Metadata { get; set; } = new List<ScriptVariable>();

    public void Declare(string name, bool value)
    {
        Remove(name);
        Metadata.Add(new ScriptVariable(ScriptType.Boolean) { Name = name, Value = value });
    }

    public void Declare(string name, int value)
    {
        Remove(name);
        Metadata.Add(new ScriptVariable(ScriptType.Integer) { Name = name, Value = value });
    }

    public void Declare(string name, float value)
    {
        Remove(name);
        Metadata.Add(new ScriptVariable(ScriptType.Real) { Name = name, Value = value });
    }

    public void Declare(string name, string value)
    {
        Remove(name);
        Metadata.Add(new ScriptVariable(ScriptType.Text) { Name = name, Value = value });
    }

    public void Declare(string name, Vec2 value)
    {
        Remove(name);
        Metadata.Add(new ScriptVariable(ScriptType.Vec2) { Name = name, Value = value });
    }

    public void Declare(string name, Vec3 value)
    {
        Remove(name);
        Metadata.Add(new ScriptVariable(ScriptType.Vec3) { Name = name, Value = value });
    }

    public void Declare(string name, Int3 value)
    {
        Remove(name);
        Metadata.Add(new ScriptVariable(ScriptType.Int3) { Name = name, Value = value });
    }

    public void Declare(string name, Int2 value)
    {
        Remove(name);
        Metadata.Add(new ScriptVariable(ScriptType.Int3) { Name = name, Value = value });
    }

    public ScriptVariable? Get(string name)
    {
        return Metadata.Find(x => x.Name == name);
    }

    public bool Remove(string name)
    {
        return Metadata.RemoveAll(x => x.Name == name) > 0;
    }

    public void ClearMetadata()
    {
        Metadata.Clear();
    }

    public void Read(GameBoxReader r)
    {
        ClassId = r.ReadUInt32();

        Version = r.ReadInt32();

        if (Version < 3) return;

        var typeCount = r.ReadByte();
        var types = r.ReadArray(typeCount, r =>
        {
            var varType = r.ReadByte(); // Int32 in version 0

            return (ScriptType)varType switch
            {
                ScriptType.Array => ReadScriptArray(r),
                ScriptType.Struct => ReadScriptStruct(out int _, r),
                _ => new ScriptVariable((ScriptType)varType),
            };
        });

        var varCount = r.ReadByte();
        var metadata = new ScriptVariable[varCount];

        for (var i = 0; i < varCount; i++)
        {
            var metadataVarName = r.ReadString(StringLengthPrefix.Byte);
            var typeIndex = r.ReadByte();

            var type = types[typeIndex];
            metadata[i] = ReadType(type.Clone(), r);
            metadata[i].Name = metadataVarName;
        }

        Metadata = metadata.ToList();

        var facade = r.ReadUInt32();
    }

    private static ScriptArray ReadScriptArray(GameBoxReader r)
    {
        var indexType = r.ReadByte(); // index
        var indexVar = (ScriptType)indexType == ScriptType.Struct
            ? ReadScriptStruct(out int _, r)
            : new ScriptVariable((ScriptType)indexType);

        var arrayType = r.ReadByte(); // value

        var valueVar = (ScriptType)arrayType switch
        {
            ScriptType.Array => ReadScriptArray(r),
            ScriptType.Struct => ReadScriptStruct(out int _, r),
            _ => new ScriptVariable((ScriptType)arrayType),
        };

        var array = new ScriptArray(new KeyValuePair<ScriptVariable, ScriptVariable>(indexVar, valueVar));

        int counterArray = 0;
        while (r.ReadByte() == 0)
            counterArray++;
        r.BaseStream.Position -= 1;

        array.Unknown = counterArray;

        return array;
    }

    private static ScriptVariable ReadType(ScriptVariable type, GameBoxReader r)
    {
        switch (type.Type)
        {
            case ScriptType.Boolean:
                type.Value = r.ReadBoolean(true);
                break;
            case ScriptType.Integer:
                type.Value = r.ReadInt32();
                break;
            case ScriptType.Real:
                type.Value = r.ReadSingle();
                break;
            case ScriptType.Text:
                type.Value = r.ReadString(StringLengthPrefix.Byte);
                break;
            case ScriptType.Vec2:
                type.Value = r.ReadVec2();
                break;
            case ScriptType.Vec3:
                type.Value = r.ReadVec3();
                break;
            case ScriptType.Int3:
                type.Value = r.ReadInt3();
                break;
            case ScriptType.Int2:
                type.Value = r.ReadInt2();
                break;
            case ScriptType.Array:
                var array = (ScriptArray)type;

                var numElements = r.ReadByte();
                if (numElements > 0)
                {
                    ScriptVariable key;
                    if (array.Reference.Key.Type == ScriptType.Void)
                    {
                        for (var i = 0; i < numElements; i++)
                            array.Elements[new ScriptVariable(ScriptType.Void) { Value = i }] = ReadType(array.Reference.Value.Clone(), r);
                    }
                    else
                    {
                        key = ReadType(array.Reference.Key.Clone(), r);
                        for (var i = 0; i < numElements; i++)
                            array.Elements[key] = ReadType(array.Reference.Value.Clone(), r);
                    }
                }
                break;
            case ScriptType.Struct:
                var strc = (ScriptStruct)type;
                for (var i = 0; i < strc.Members.Length; i++)
                    strc.Members[i] = ReadType(strc.Members[i], r);
                break;
            default:
                throw new Exception(type.Type.ToString());
        }

        return type;
    }

    private static ScriptStruct ReadScriptStruct(out int defaultLength, GameBoxReader r)
    {
        var strc = new ScriptStruct();

        var numMembers = r.ReadByte();
        var structName = r.ReadString();

        strc.StructName = structName;
        strc.Members = new ScriptVariable[numMembers];

        defaultLength = 0;

        for (var i = 0; i < numMembers; i++)
        {
            var memberName = r.ReadString();
            var memberType = r.ReadByte();

            ScriptVariable member;
            
            switch ((ScriptType)memberType)
            {
                case ScriptType.Array:
                    member = ReadScriptArray(r);
                    break;
                case ScriptType.Struct:
                    member = ReadScriptStruct(out int defLength, r);
                    defaultLength += defLength;
                    break;
                default:
                    member = new ScriptVariable((ScriptType)memberType);
                    break;
            }

            switch (member.Type)
            {
                case ScriptType.Integer:
                    r.ReadInt32();
                    defaultLength += 4;
                    break;
                case ScriptType.Real:
                    r.ReadSingle();
                    defaultLength += 4;
                    break;
                case ScriptType.Vec2:
                    r.ReadVec2();
                    defaultLength += 8;
                    break;
                case ScriptType.Vec3:
                    r.ReadVec3();
                    defaultLength += 12;
                    break;
                case ScriptType.Int3:
                    r.ReadInt3();
                    defaultLength += 12;
                    break;
                case ScriptType.Int2:
                    r.ReadInt2();
                    defaultLength += 8;
                    break;
                case ScriptType.Array:
                    break;
                case ScriptType.Struct:
                    break;
                default:
                    r.ReadByte();
                    defaultLength += 1;
                    break;
            }

            member.Name = memberName;

            strc.Members[i] = member;
        }

        int counter = 0;
        while (r.ReadByte() == 0)
            counter++;
        r.BaseStream.Position -= 1;

        //int counter = 0;
        //while (r.ReadByte() == 0) counter++; // probably size of the struct in byte count?
        //r.BaseStream.Position -= 1;
        strc.Size = defaultLength + counter; //
        strc.Unknown = counter;

        //Progress += defaultLength;

        return strc;
    }

    public void Write(GameBoxWriter w)
    {
        w.Write(ClassId);
        w.Write(Version);

        var listOfTypes = new List<ScriptVariable>();
        var typeIndicies = new int[Metadata.Count];

        for (var i = 0; i < Metadata.Count; i++)
        {
            var type = Metadata[i].Clone();
            type.Name = null!;
            type.Clear();

            bool exists = false;
            for (var j = 0; j < listOfTypes.Count; j++)
            {
                exists = type.TypeEquals(listOfTypes[j]);
                if (exists)
                {
                    typeIndicies[i] = j;
                    break;
                }
            }
            if (!exists)
            {
                listOfTypes.Add(type);
                if (i == 0) typeIndicies[i] = 0;
                else typeIndicies[i] = typeIndicies.Max() + 1;
            }
        }

        w.Write((byte)listOfTypes.Count);

        foreach (var t in listOfTypes)
        {
            w.Write((byte)t.Type);
            if (t.Type == ScriptType.Array)
                WriteScriptArray((ScriptArray)t);
            else if (t.Type == ScriptType.Struct)
                WriteScriptStruct((ScriptStruct)t);
        }

        w.Write((byte)Metadata.Count);

        for (var i = 0; i < Metadata.Count; i++)
        {
            var m = Metadata[i];
            w.Write(m.Name, StringLengthPrefix.Byte);
            w.Write((byte)typeIndicies[i]);
            WriteType(m);
        }

        void WriteScriptArray(ScriptArray variable)
        {
            var reference = variable.Reference;

            w.Write((byte)reference.Key.Type);

            if (reference.Key.Type == ScriptType.Struct)
                WriteScriptStruct((ScriptStruct)reference.Key);

            w.Write((byte)reference.Value.Type);

            if (reference.Value.Type == ScriptType.Array)
                WriteScriptArray((ScriptArray)reference.Value);
            else if (reference.Value.Type == ScriptType.Struct)
                WriteScriptStruct((ScriptStruct)reference.Value);

            for (var i = 0; i < variable.Unknown; i++)
                w.Write((byte)0);
        }

        void WriteScriptStruct(ScriptStruct variable)
        {
            w.Write((byte)variable.Members.Length);
            w.Write(variable.StructName);

            foreach (var member in variable.Members)
            {
                w.Write(member.Name);
                w.Write((byte)member.Type);

                switch (member.Type)
                {
                    case ScriptType.Array:
                        WriteScriptArray((ScriptArray)member);
                        break;
                    case ScriptType.Struct:
                        WriteScriptStruct((ScriptStruct)member);
                        break;
                }

                switch (member.Type)
                {
                    case ScriptType.Integer:
                        w.Write(0);
                        break;
                    case ScriptType.Real:
                        w.Write(0f);
                        break;
                    case ScriptType.Vec2:
                        w.Write(new Vec2());
                        break;
                    case ScriptType.Vec3:
                        w.Write(new Vec3());
                        break;
                    case ScriptType.Int3:
                        w.Write(new Int3());
                        break;
                    case ScriptType.Int2:
                        w.Write(new Int2());
                        break;
                    case ScriptType.Array:
                        break;
                    case ScriptType.Struct:
                        break;
                    default:
                        w.Write((byte)0);
                        break;
                }
            }

            for (var i = 0; i < variable.Unknown; i++)
                w.Write((byte)0);
        }

        void WriteType(ScriptVariable type)
        {
            switch (type.Type)
            {
                case ScriptType.Boolean:
                    w.Write((bool)type.Value, true);
                    break;
                case ScriptType.Integer:
                    w.Write((int)type.Value);
                    break;
                case ScriptType.Real:
                    w.Write((float)type.Value);
                    break;
                case ScriptType.Text:
                    w.Write((string)type.Value, StringLengthPrefix.Byte);
                    break;
                case ScriptType.Vec2:
                    w.Write((Vec2)type.Value);
                    break;
                case ScriptType.Vec3:
                    w.Write((Vec3)type.Value);
                    break;
                case ScriptType.Int3:
                    w.Write((Int3)type.Value);
                    break;
                case ScriptType.Int2:
                    w.Write((Int2)type.Value);
                    break;
                case ScriptType.Array:
                    var array = (ScriptArray)type;

                    w.Write((byte)array.Elements.Count);

                    if (array.Elements.Count > 0)
                    {
                        if (array.Reference.Key.Type != ScriptType.Void)
                            WriteType(array.Reference.Key);

                        foreach (var e in array.Elements)
                            WriteType(e.Value);
                    }
                    break;
                case ScriptType.Struct:
                    var strc = (ScriptStruct)type;
                    foreach (var m in strc.Members)
                        WriteType(m);
                    break;
                default:
                    throw new Exception(type.Type.ToString());
            }
        }

        w.Write(0xFACADE01);
    }

    [Serializable]
    public class ScriptVariable
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public ScriptType Type { get; }

        public ScriptVariable(ScriptType type)
        {
            Name = null!;
            Value = null!;
            Type = type;
        }

        public virtual ScriptVariable Clone()
        {
            return this.Copy()!;
        }

        /// <summary>
        /// Checks the script variable type equality. This function hasn't been tested with complex structs and arrays.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public bool TypeEquals(ScriptVariable variable)
        {
            // If the actual major type does not equal
            if (Type != variable.Type)
            {
                return false;
            }
            
            // If both types are some kind of array
            if (this is ScriptArray a && variable is ScriptArray b)
            {
                // Recursively check the array if both types aren't different in their inner based on Reference property
                if (!a.Reference.Key.TypeEquals(b.Reference.Key) || !a.Reference.Value.TypeEquals(b.Reference.Value))
                {
                    return false; // If some are found different, then it's clearly not equal
                }
            }
            // If both types are some kind of struct
            else if (this is ScriptStruct c && variable is ScriptStruct d)
            {
                // If the struct name is the same
                if (c.StructName == d.StructName)
                {
                    // Each member from the first struct meets the other member from the second struct
                    foreach (var m1 in c.Members)
                    {
                        foreach (var m2 in d.Members)
                        {
                            // If the member names don't match or the type doesn't match
                            if (m1.Name != m2.Name && !m1.TypeEquals(m2))
                            {
                                return false; // Not equal
                            }
                        }
                    }
                }
                else
                {
                    return false; // Struct with different name isn't equal
                }
            }
            
            return true; // If everything appeared to be equal, returns true
        }

        /// <summary>
        /// Clears the value of the variable safely.
        /// </summary>
        public void Clear()
        {
            switch (this)
            {
                case ScriptArray v:
                    v.Elements.Clear();
                    break;
                case ScriptStruct s:
                    foreach (var m in s.Members)
                    {
                        m.Clear();
                    }
                    break;
                default:
                    Value = null!;
                    break;
            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name) && Value != null)
            {
                if (Type == ScriptType.Text)
                {
                    return $"{Type} {Name} = \"{Value}\"";
                }
                
                return $"{Type} {Name} = {Value}";
            }

            if (!string.IsNullOrEmpty(Name))
            {
                return $"{Type} {Name}";
            }
            
            if (Value != default)
            {
                return Value?.ToString() ?? string.Empty;
            }

            return Type.ToString();
        }
    }

    [Serializable]
    public sealed class ScriptArray : ScriptVariable
    {
        public KeyValuePair<ScriptVariable, ScriptVariable> Reference { get; }
        public Dictionary<ScriptVariable, ScriptVariable> Elements { get; }

        public int Unknown { get; set; }

        public ScriptArray(KeyValuePair<ScriptVariable, ScriptVariable> reference) : base(ScriptType.Array)
        {
            Reference = reference;
            Elements = new Dictionary<ScriptVariable, ScriptVariable>();
        }

        public override string ToString()
        {
            var result = new StringBuilder(Reference.Value.ToString());
            
            if (Reference.Key is ScriptArray)
            {
                result.Append($"[{Reference.Key as ScriptArray}]");
            }
            else
            {
                result.Append($"[{Reference.Key}]");
            }

            if (!string.IsNullOrEmpty(Name))
            {
                result.Append($" {Name}");
            }
            
            return result.ToString();
        }
    }

    [Serializable]
    public sealed class ScriptStruct : ScriptVariable
    {
        public string StructName { get; set; }
        public ScriptVariable[] Members { get; set; }
        public int Size { get; set; }

        public int Unknown { get; set; }

        public ScriptStruct() : base(ScriptType.Struct)
        {
            StructName = "";
            Members = Array.Empty<ScriptVariable>();
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name)
                ? $"Struct({StructName})"
                : $"Struct({StructName}) {Name}";
        }
    }
}
