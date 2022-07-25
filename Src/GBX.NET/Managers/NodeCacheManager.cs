using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace GBX.NET.Managers;

public static class NodeCacheManager
{
    private static readonly Assembly assembly = typeof(NodeCacheManager).Assembly;

    public static bool ClassesAreCached { get; internal set; }

    public static Dictionary<uint, string> Names { get; }
    public static Dictionary<uint, uint> Mappings { get; } // key: older, value: newer
    public static ILookup<uint, uint> ReverseMappings { get; } // key: newer, value: older
    public static Dictionary<uint, string> Extensions { get; }
    public static Dictionary<int, string> CollectionIds { get; }
    public static ConcurrentDictionary<uint, string> GbxExtensions { get; }

    public static ConcurrentDictionary<uint, Type> ClassTypesById { get; }
    public static ConcurrentDictionary<Type, uint> ClassIdsByType { get; }

    public static ConcurrentDictionary<uint, Type> ChunkTypesById { get; }
    public static ConcurrentDictionary<Type, uint> ChunkIdsByType { get; }

    public static ConcurrentDictionary<uint, Type> HeaderChunkTypesById { get; }

    public static ConcurrentDictionary<Type, IEnumerable<Attribute>> ClassAttributesByType { get; }
    public static ConcurrentDictionary<uint, IEnumerable<Attribute>> ChunkAttributesById { get; }
    public static ConcurrentDictionary<uint, IEnumerable<Attribute>> HeaderChunkAttributesById { get; }
    public static ConcurrentDictionary<Type, IEnumerable<Attribute>> ChunkAttributesByType { get; }
    public static ConcurrentDictionary<uint, Func<Node>> ClassConstructors { get; }
    public static ConcurrentDictionary<uint, Func<Chunk>> ChunkConstructors { get; }
    public static ConcurrentDictionary<uint, Func<IHeaderChunk>> HeaderChunkConstructors { get; }

    public static bool ChunksAreCurrentlyBeingCached { get; private set; }

    public static ConcurrentDictionary<Type, byte> ClassTypesWithCachedChunks { get; }
    public static ConcurrentDictionary<Type, byte> SkippableChunks { get; }

    public static ConcurrentDictionary<uint, byte> AsyncChunksById { get; }
    public static ConcurrentDictionary<uint, byte> ReadAsyncChunksById { get; }
    public static ConcurrentDictionary<uint, byte> WriteAsyncChunksById { get; }
    public static ConcurrentDictionary<uint, byte> ReadWriteAsyncChunksById { get; }

    static NodeCacheManager()
    {
        Names = new Dictionary<uint, string>();
        Mappings = new Dictionary<uint, uint>();
        Extensions = new Dictionary<uint, string>();
        CollectionIds = new Dictionary<int, string>();
        GbxExtensions = new ConcurrentDictionary<uint, string>();

        ClassTypesById = new ConcurrentDictionary<uint, Type>();
        ClassIdsByType = new ConcurrentDictionary<Type, uint>();
        ChunkTypesById = new ConcurrentDictionary<uint, Type>();
        ChunkIdsByType = new ConcurrentDictionary<Type, uint>();
        HeaderChunkTypesById = new ConcurrentDictionary<uint, Type>();

        ClassAttributesByType = new ConcurrentDictionary<Type, IEnumerable<Attribute>>();
        ChunkAttributesById = new ConcurrentDictionary<uint, IEnumerable<Attribute>>();
        HeaderChunkAttributesById = new ConcurrentDictionary<uint, IEnumerable<Attribute>>();
        ChunkAttributesByType = new ConcurrentDictionary<Type, IEnumerable<Attribute>>();

        ClassConstructors = new ConcurrentDictionary<uint, Func<Node>>();
        ChunkConstructors = new ConcurrentDictionary<uint, Func<Chunk>>();
        HeaderChunkConstructors = new ConcurrentDictionary<uint, Func<IHeaderChunk>>();

        ClassTypesWithCachedChunks = new ConcurrentDictionary<Type, byte>();
        SkippableChunks = new ConcurrentDictionary<Type, byte>();

        AsyncChunksById = new ConcurrentDictionary<uint, byte>();
        ReadAsyncChunksById = new ConcurrentDictionary<uint, byte>();
        WriteAsyncChunksById = new ConcurrentDictionary<uint, byte>();
        ReadWriteAsyncChunksById = new ConcurrentDictionary<uint, byte>();

        DefineNames2(Names, Extensions);
        DefineMappings2(Mappings);
        ReverseMappings = Mappings.ToLookup(x => x.Value, x => x.Key);
        DefineCollectionIds(CollectionIds);
    }

    private static void DefineCollectionIds(IDictionary<int, string> collectionIds)
    {
        using var reader = new StringReader(Resources.CollectionID);

        while (true)
        {
            var stringLine = reader.ReadLine();

            if (stringLine is null)
            {
                break;
            }

            var line = stringLine.AsSpan();

            var spaceAtIndex = line.IndexOf(' ');

            if (spaceAtIndex == -1)
            {
                continue;
            }

            var key = line.Slice(0, spaceAtIndex);
            var value = line.Slice(spaceAtIndex + 1);

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            collectionIds[int.Parse(key)] = value.ToString();
#else
            collectionIds[int.Parse(key.ToString())] = value.ToString();
#endif
        }
    }

    public static string? GetNodeExtension(uint classId)
    {
        CacheClassTypesIfNotCached();

        GbxExtensions.TryGetValue(classId, out var extension);

        return extension;
    }

    /// <summary>
    /// Gets the cached private constructor of the node. The node can potentially have null values in non-nullable properties and fields.
    /// </summary>
    /// <typeparam name="T">Type of the node to instantiate.</typeparam>
    /// <param name="classId">Class ID.</param>
    /// <returns>The instantiated node.</returns>
    /// <exception cref="NodeNotInstantiableException">Node instance cannot be created from this class ID.</exception>
    internal static T GetNodeInstance<T>(uint classId) where T : Node
    {
        CacheClassTypesIfNotCached();

        return (T)GetClassConstructor(classId)();
    }

    /// <summary>
    /// Gets the cached private constructor of the node with an additional of allocating the class ID from <typeparamref name="T"/>. The node can potentially have null values in non-nullable properties and fields.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    internal static T GetNodeInstance<T>() where T : Node
    {
        CacheClassTypesIfNotCached();

        var classId = GetClassIdByType(typeof(T)) ?? throw new Exception("Node cannot be instantiated.");

        return (T)GetClassConstructor(classId)();
    }

    public static void CacheClassTypesIfNotCached()
    {
        if (ClassesAreCached)
        {
            return;
        }

        IEnumerable<Type> types;

        try
        {
            types = assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            types = e.Types.Where(x => x is not null)!;
        }

        foreach (var type in types)
        {
            if (!IsClassFromEngines(type))
            {
                continue;
            }

            var attributes = CustomAttributeExtensions.GetCustomAttributes(type, inherit: false);

            var nodeAttribute = default(NodeAttribute);
            var nodeExtensionAttribute = default(NodeExtensionAttribute);
            var moreNodeAttributes = default(List<NodeAttribute>);

            foreach (var attribute in attributes)
            {
                switch (attribute)
                {
                    case NodeAttribute na:

                        if (nodeAttribute is not null)
                        {
                            moreNodeAttributes ??= new();
                            moreNodeAttributes.Add(na);
                            continue;
                        }

                        nodeAttribute = na;

                        break;
                    case NodeExtensionAttribute nea:
                        nodeExtensionAttribute = nea;
                        break;
                }
            }

            if (nodeAttribute is null)
            {
                continue;
            }

            var privateConstructor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Array.Empty<Type>(), null);

            if (privateConstructor is null)
            {
                throw new PrivateConstructorNotFoundException(type);
            }

            var nodeId = nodeAttribute.ID;
            ClassTypesById[nodeId] = type;
            ClassIdsByType[type] = nodeId;
            ClassAttributesByType[type] = attributes;

            if (nodeExtensionAttribute is not null)
            {
                GbxExtensions[nodeId] = nodeExtensionAttribute.Extension;
            }

            if (moreNodeAttributes is not null)
            {
                foreach (var att in moreNodeAttributes)
                {
                    ClassTypesById[att.ID] = type;
                }
            }
        }

        ClassesAreCached = true;
    }

    internal static Type? GetClassTypeById(uint id)
    {
        CacheClassTypesIfNotCached();

        if (ClassTypesById.TryGetValue(id, out var cachedType))
        {
            return cachedType;
        }

        return null;
    }

    internal static uint? GetClassIdByType(Type type)
    {
        CacheClassTypesIfNotCached();

        if (ClassIdsByType.TryGetValue(type, out var cachedId))
        {
            return cachedId;
        }

        return null;
    }

    internal static Type? GetHeaderChunkTypeById(Type classType, uint chunkId)
    {
        return GetChunkTypeById(classType, chunkId, HeaderChunkTypesById);
    }

    internal static Type? GetChunkTypeById(Type classType, uint chunkId)
    {
        return GetChunkTypeById(classType, chunkId, ChunkTypesById);
    }

    internal static Type? GetChunkTypeById(Type classType, uint chunkId, IDictionary<uint, Type> chunkTypesById)
    {
        if (chunkTypesById.TryGetValue(chunkId, out var cachedChunkType))
        {
            return cachedChunkType;
        }

        if (CacheChunkTypesIfNotCached(classType))
        {
            chunkTypesById.TryGetValue(chunkId, out var chunkType);
            return chunkType;
        }

        return null;
    }

    internal static uint GetChunkIdByType(Type chunkType)
    {
        return GetChunkIdByType(chunkType, ChunkIdsByType);
    }

    internal static uint GetChunkIdByType(Type chunkType, IDictionary<Type, uint> chunkIdsByType)
    {
        if (chunkIdsByType.TryGetValue(chunkType, out var cachedChunkId))
        {
            return cachedChunkId;
        }

        if (chunkType.DeclaringType is null)
        {
            throw new Exception("Wrongly defined chunk class.");
        }

        CacheChunkTypesIfNotCached(chunkType.DeclaringType);

        return ChunkIdsByType[chunkType];
    }

    internal static uint GetChunkIdByType(Type classType, Type chunkType)
    {
        return GetChunkIdByType(classType, chunkType, ChunkIdsByType);
    }

    internal static uint GetChunkIdByType(Type classType, Type chunkType, IDictionary<Type, uint> chunkIdsByType)
    {
        if (chunkIdsByType.TryGetValue(chunkType, out var cachedChunkId))
        {
            return cachedChunkId;
        }
        
        CacheChunkTypesIfNotCached(classType);

        return ChunkIdsByType[chunkType];
    }

    /// <summary>
    /// Cache chunk types based on its <paramref name="classType"/>.
    /// </summary>
    /// <param name="classType"></param>
    /// <returns>False if chunks have been already cached, otherwise true.</returns>
    private static bool CacheChunkTypesIfNotCached(Type classType)
    {
        // Unknown skippable chunk solution
        if (ClassTypesWithCachedChunks.ContainsKey(classType))
        {
            return false;
        }

        var tempClassType = classType;

        while (tempClassType != typeof(CMwNod))
        {
            var nestedTypes = tempClassType.GetNestedTypes(BindingFlags.Instance | BindingFlags.Public);

            foreach (var type in nestedTypes)
            {
                CacheChunk(type);
            }

            var baseType = tempClassType.BaseType;

            if (baseType is null)
            {
                throw new ThisShouldNotHappenException();
            }

            tempClassType = baseType;
        }

        ClassTypesWithCachedChunks[classType] = 1;

        return true;
    }

    private static void CacheChunk(Type type)
    {
        // If the chunk was already cached
        if (ChunkAttributesByType.ContainsKey(type))
        {
            return;
        }

        if (!type.IsSubclassOf(typeof(Chunk)))
        {
            return;
        }

        var attributes = type.GetCustomAttributes();

        if (attributes.FirstOrDefault(x => x is ChunkAttribute) is not ChunkAttribute chunkAttribute)
        {
            throw new Exception($"Chunk {type.FullName} doesn't have ChunkAttribute.");
        }

        var chunkId = chunkAttribute.ID;

        ChunkIdsByType[type] = chunkId;
        ChunkAttributesByType[type] = attributes;

        if (type.GetInterface(nameof(IHeaderChunk)) is null)
        {
            var constructor = CreateConstructor<Chunk>(type);

            ChunkAttributesById[chunkId] = attributes;
            ChunkTypesById[chunkId] = type;
            ChunkConstructors[chunkId] = constructor;
            
            CheckAsyncReadWrite(type, chunkId);
        }
        else
        {
            var constructor = CreateConstructor<IHeaderChunk>(type);

            HeaderChunkAttributesById[chunkId] = attributes;
            HeaderChunkTypesById[chunkId] = type;
            HeaderChunkConstructors[chunkId] = constructor;
        }

        if (type.BaseType?.GetGenericTypeDefinition() == typeof(SkippableChunk<>))
        {
            SkippableChunks[type] = 1;
        }
    }

    private static void CheckAsyncReadWrite(Type type, uint chunkId)
    {
        CheckMethod(type, chunkId, nameof(IReadableWritableChunk.ReadAsync), ReadAsyncChunksById);
        CheckMethod(type, chunkId, nameof(IReadableWritableChunk.WriteAsync), WriteAsyncChunksById);
        CheckMethod(type, chunkId, nameof(IReadableWritableChunk.ReadWriteAsync), ReadWriteAsyncChunksById);
    }

    private static void CheckMethod(Type type, uint chunkId, string name, IDictionary<uint, byte> dictionary)
    {
        var methods = type.GetMethods().Where(x => x.Name == name);

        foreach (var method in methods)
        {
            if (method.GetBaseDefinition().DeclaringType == method.DeclaringType)
            {
                continue;
            }

            dictionary[chunkId] = 1;
            AsyncChunksById[chunkId] = 1;
        }
    }

    internal static Func<Node> GetClassConstructor(uint id)
    {
        if (ClassConstructors.TryGetValue(id, out var cachedConstructor))
        {
            return cachedConstructor;
        }

        if (!ClassTypesById.TryGetValue(id, out var cachedType))
        {
            throw new ThisShouldNotHappenException();
        }

        var constructor = CreateConstructor<Node>(cachedType);

        ClassConstructors[id] = constructor;

        return constructor;
    }

    private static Func<T> CreateConstructor<T>(Type type)
    {
        var newExp = Expression.New(type);
        var lambda = Expression.Lambda<Func<T>>(newExp);
        return lambda.Compile();
    }

    internal static void DefineNames(IDictionary<uint, string> names, IDictionary<uint, string> extensions)
    {
        using var reader = new StringReader(Resources.ClassID);

        var en = "";
        var engineName = "";

        while (true)
        {
            var line = reader.ReadLine();

            if (line is null)
            {
                break;
            }

            var ch = "000";

            var className = "";

            if (!line.StartsWith("  "))
            {
                en = line.Substring(0, 2);
                if (line.Length - 3 > 0) engineName = line.Substring(3);
                continue;
            }

            var cl = line.Substring(2, 3);
            if (line.Length - 6 > 0) className = line.Substring(6);

            var classIDString = $"{en}{cl}{ch}";

            var extension = default(string);

            var classNameSplit = className.Split(' ');

            if (classNameSplit.Length > 1)
            {
                className = classNameSplit[0];
                extension = classNameSplit[1];
            }

            if (uint.TryParse(classIDString, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint classID))
            {
                names[classID] = engineName + "::" + className;
                if (extension != null)
                    extensions[classID] = extension;

                continue;
            }

            //Debug.WriteLine($"Invalid class ID {classIDString}, skipping");
        }

        //Debug.WriteLine("Classes named in " + watch.Elapsed.TotalMilliseconds + "ms");
    }

    internal static void DefineNames2(IDictionary<uint, string> names, IDictionary<uint, string> extensions)
    {
        var watch = Stopwatch.StartNew();

        using var reader = new StringReader(Resources.ClassID);

        var classIdSpan = new Span<char>(new[] { '0', '0', '0', '0', '0', '0', '0', '0' });
        var engineNameSpan = new ReadOnlySpan<char>();

        while (true)
        {
            var stringLine = reader.ReadLine();

            if (stringLine is null)
            {
                break;
            }

            var line = stringLine.AsSpan();

#if NET462 || NETSTANDARD2_0
            if (!line.StartsWith(new ReadOnlySpan<char>(new char[] { ' ', ' ' })))
#else
            if (!line.StartsWith("  "))
#endif
            {
                var engine = line.Slice(0, 2);
                classIdSpan[0] = engine[0];
                classIdSpan[1] = engine[1];

                if (line.Length > 3)
                {
                    engineNameSpan = line.Slice(3, line.Length - 3);
                }

                continue;
            }

            var classIdPart = line.Slice(2, 3);

            for (var i = 0; i < 3; i++)
            {
                classIdSpan[2 + i] = classIdPart[i];
            }

            var classNameWithExtensionSpan = new ReadOnlySpan<char>();

            if (line.Length <= 6)
            {
                continue;
            }

            classNameWithExtensionSpan = line.Slice(6, line.Length - 6);
            var classNameWithExtensionSpaceIndex = classNameWithExtensionSpan.IndexOf(' ');
            var noExtension = classNameWithExtensionSpaceIndex == -1;

            var classNameSpan = noExtension
                ? classNameWithExtensionSpan
                : classNameWithExtensionSpan.Slice(0, classNameWithExtensionSpaceIndex);

            var extensionSpan = noExtension
                ? new ReadOnlySpan<char>()
                : classNameWithExtensionSpan.Slice(classNameWithExtensionSpaceIndex + 1,
                    length: classNameWithExtensionSpan.Length - classNameWithExtensionSpaceIndex - 1);

#if NET462 || NETSTANDARD2_0
            if (!uint.TryParse(classIdSpan.ToString(), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint classID))
#else
            if (!uint.TryParse(classIdSpan, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint classID))
#endif
            {
                continue;
            }

#if NET6_0_OR_GREATER
            var fullName = string.Concat(engineNameSpan, "::", classNameSpan); // .NET Core 3+
#else
            var fullNameSpan = new Span<char>(new char[engineNameSpan.Length + 2 + classNameSpan.Length]);
                
            var ii = 0;
            engineNameSpan.CopyTo(fullNameSpan.Slice(ii, engineNameSpan.Length));
            ii += engineNameSpan.Length;

            new ReadOnlySpan<char>(new char[] { ':', ':' }).CopyTo(fullNameSpan.Slice(ii, 2));
            ii += 2;

            classNameSpan.CopyTo(fullNameSpan.Slice(ii, classNameSpan.Length));

            var fullName = fullNameSpan.ToString();
#endif

            names.Add(classID, fullName);

            if (!extensionSpan.IsEmpty)
                extensions.Add(classID, extensionSpan.ToString());

            //Debug.WriteLine($"Invalid class ID {classIdSpan}, skipping");
        }

        //Debug.WriteLine("Classes named in " + watch.Elapsed.TotalMilliseconds + "ms");
    }

    internal static void DefineMappings(Dictionary<uint, uint> mappings)
    {
        using var reader = new StringReader(Resources.ClassIDMappings);

        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            var valueKey = line.Split(new string[] { " -> " }, StringSplitOptions.None);

            if (valueKey.Length != 2)
                continue;

            if (!uint.TryParse(valueKey[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint key)
            || !uint.TryParse(valueKey[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint value))
                continue;

            if (mappings.ContainsValue(key)) // Virtual Skipper solution
                mappings[mappings.FirstOrDefault(x => x.Value == key).Key] = value;
            mappings[key] = value;
        }
    }

    internal static void DefineMappings2(Dictionary<uint, uint> mappings)
    {
        using var reader = new StringReader(Resources.ClassIDMappings);

        while (true)
        {
            var stringLine = reader.ReadLine();

            if (stringLine is null)
            {
                break;
            }

            if (stringLine == "")
            {
                continue;
            }

            var line = stringLine.AsSpan();

            var from = line.Slice(0, 8);
            var to = line.Slice(12, 8);

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
            if (!uint.TryParse(from, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint key)
            || !uint.TryParse(to, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint value))
            {
                continue;
            }
#else
            if (!uint.TryParse(from.ToString(), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint key)
            || !uint.TryParse(to.ToString(), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint value))
            {
                continue;
            }
#endif

            mappings.Add(key, value);
        }
    }

    private static bool IsClassFromEngines(Type t)
    {
        return t?.IsClass == true && t.Namespace?.StartsWith("GBX.NET.Engines") == true;
    }
}
