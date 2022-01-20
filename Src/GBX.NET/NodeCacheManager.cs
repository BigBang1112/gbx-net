using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace GBX.NET;

public static class NodeCacheManager
{
    private static readonly Assembly assembly = typeof(NodeCacheManager).Assembly;

    public static bool TypesDefined { get; internal set; }
    public static bool ClassesAreCached { get; internal set; }

    public static Dictionary<uint, string> Names { get; }
    public static Dictionary<uint, uint> Mappings { get; } // key: older, value: newer
    public static ILookup<uint, uint> ReverseMappings { get; } // key: newer, value: older
    public static Dictionary<uint, string> Extensions { get; }

    public static Dictionary<Type, uint> TypeWithClassId { get; }
    public static Dictionary<uint, Type> ClassIdWithType { get; }

    public static Dictionary<uint, Type> ChunkTypesById { get; }
    public static Dictionary<Type, uint> ChunkIdsByType { get; }

    public static Dictionary<uint, Type> HeaderChunkTypesById { get; }
    public static Dictionary<Type, uint> HeaderChunkIdsByType { get; }

    public static Dictionary<Type, IEnumerable<Attribute>> ClassAttributesByType { get; }
    public static Dictionary<uint, IEnumerable<Attribute>> ChunkAttributesById { get; }
    public static Dictionary<uint, IEnumerable<Attribute>> HeaderChunkAttributesById { get; }
    public static Dictionary<Type, IEnumerable<Attribute>> ChunkAttributesByType { get; }
    public static Dictionary<uint, Func<CMwNod>> ClassConstructors { get; }
    public static Dictionary<uint, Func<Chunk>> ChunkConstructors { get; }
    public static Dictionary<uint, Func<Chunk>> HeaderChunkConstructors { get; }

    public static bool ChunksAreCurrentlyGettingCached { get; private set; }

    public static HashSet<Type> ClassTypesWithCachedChunks { get; }
    public static HashSet<Type> SkippableChunks { get; }

    static NodeCacheManager()
    {
        Names = new Dictionary<uint, string>();
        Mappings = new Dictionary<uint, uint>();
        Extensions = new Dictionary<uint, string>();

        TypeWithClassId = new Dictionary<Type, uint>();
        ClassIdWithType = new Dictionary<uint, Type>();
        ChunkTypesById = new Dictionary<uint, Type>();
        ChunkIdsByType = new Dictionary<Type, uint>();
        HeaderChunkTypesById = new Dictionary<uint, Type>();
        HeaderChunkIdsByType = new Dictionary<Type, uint>();

        ClassTypesWithCachedChunks = new HashSet<Type>();

        ClassAttributesByType = new Dictionary<Type, IEnumerable<Attribute>>();
        ChunkAttributesById = new Dictionary<uint, IEnumerable<Attribute>>();
        HeaderChunkAttributesById = new Dictionary<uint, IEnumerable<Attribute>>();
        ChunkAttributesByType = new Dictionary<Type, IEnumerable<Attribute>>();

        ClassConstructors = new Dictionary<uint, Func<CMwNod>>();
        ChunkConstructors = new Dictionary<uint, Func<Chunk>>();
        HeaderChunkConstructors = new Dictionary<uint, Func<Chunk>>();

        SkippableChunks = new HashSet<Type>();

        DefineNames2(Names, Extensions);
        DefineMappings2(Mappings);
        ReverseMappings = Mappings.ToLookup(x => x.Value, x => x.Key);
    }

    /// <summary>
    /// Gets the cached private constructor of the node. The node can potentially have null values in non-nullable properties and fields.
    /// </summary>
    /// <typeparam name="T">Type of the node to instantiate.</typeparam>
    /// <param name="classId">Class ID.</param>
    /// <returns>The instantiated node.</returns>
    /// <exception cref="NodeNotInstantiableException">Node instance cannot be created from this class ID.</exception>
    internal static T GetNodeInstance<T>(uint classId) where T : CMwNod
    {
        if (ClassConstructors.TryGetValue(classId, out Func<CMwNod>? constructor))
        {
            var node = (T)constructor();
            node.SetIDAndChunks();
            return node;
        }

        throw new NodeNotInstantiableException(classId);
    }

    internal static void CacheClassTypesIfNotCached()
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

            var attributes = type.GetCustomAttributes();

            if (attributes.FirstOrDefault(x => x is NodeAttribute) is not NodeAttribute nodeAttribute)
            {
                continue;
            }

            ClassIdWithType.Add(nodeAttribute.ID, type);
            TypeWithClassId.Add(type, nodeAttribute.ID);
            ClassAttributesByType.Add(type, attributes);
        }

        ClassesAreCached = true;
    }

    internal static Type? GetClassTypeById(uint id)
    {
        CacheClassTypesIfNotCached();

        if (ClassIdWithType.TryGetValue(id, out var cachedType))
        {
            return cachedType;
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

        var nullableChunkId = (uint?)chunkId;
        var chunkType = default(Type);

        CacheChunkTypesIfNotCached(classType, ref nullableChunkId, ref chunkType);

        return chunkType;
    }

    internal static uint GetHeaderChunkIdByType(Type classType, Type chunkType)
    {
        return GetChunkIdByType(classType, chunkType, HeaderChunkIdsByType);
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

        var nullableChunkId = default(uint?);

        CacheChunkTypesIfNotCached(classType, ref nullableChunkId, ref chunkType!);

        if (nullableChunkId is null)
        {
            throw new Exception();
        }

        return nullableChunkId.Value;
    }

    /// <summary>
    /// Cache chunk types based on its <paramref name="classType"/>.
    /// </summary>
    /// <param name="classType"></param>
    /// <param name="chunkId">If not null, this chunk ID is used to return the <paramref name="chunkType"/>.</param>
    /// <param name="chunkType">If not null, this chunk type is used to return the <paramref name="chunkId"/>.</param>
    private static void CacheChunkTypesIfNotCached(Type classType, ref uint? chunkId, ref Type? chunkType)
    {
        // Unknown skippable chunk solution
        if (ClassTypesWithCachedChunks.Contains(classType))
        {
            return;
        }

        ClassTypesWithCachedChunks.Add(classType);

        // If this section is running while the same classType arrives, it should wait until its completed
        // and then return it from AvailableChunkTypes/AvailableHeaderChunkTypes
        // The solution is probably with async
        // This would mainly fix performance issues with parallel gbx parsing

        // Current solution doesnt allow multiple classType processing but doesnt require concurrent collection for now
        // Would be better to replace with an async option
        if (ChunksAreCurrentlyGettingCached)
        {
            Thread.Sleep(1);
        }

        ChunksAreCurrentlyGettingCached = true;

        while (classType != typeof(CMwNod))
        {
            var nestedTypes = classType.GetNestedTypes(BindingFlags.Instance | BindingFlags.Public);

            foreach (var type in nestedTypes)
            {
                CacheChunk(type, ref chunkId, ref chunkType);
            }

            var baseType = classType.BaseType;

            if (baseType is null)
            {
                throw new ThisShouldNotHappenException();
            }

            classType = baseType;
        }

        ChunksAreCurrentlyGettingCached = false;
    }

    private static void CacheChunk(Type type, ref uint? chunkId, ref Type? chunkType)
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

        ChunkAttributesByType.Add(type, attributes);

        if (chunkId is null && chunkType is not null && chunkType == type)
        {
            chunkId = chunkAttribute.ID;
        }

        if (chunkId is not null && chunkType is null && chunkAttribute.ID == chunkId)
        {
            chunkType = type;
        }

        var constructor = CreateConstructor<Chunk>(type);

        if (type.GetInterface(nameof(IHeaderChunk)) is null)
        {
            ChunkAttributesById.Add(chunkAttribute.ID, attributes);
            ChunkTypesById.Add(chunkAttribute.ID, type);
            ChunkIdsByType.Add(type, chunkAttribute.ID);
            ChunkConstructors.Add(chunkAttribute.ID, constructor);
        }
        else
        {
            HeaderChunkAttributesById.Add(chunkAttribute.ID, attributes);
            HeaderChunkTypesById.Add(chunkAttribute.ID, type);
            HeaderChunkIdsByType.Add(type, chunkAttribute.ID);
            HeaderChunkConstructors.Add(chunkAttribute.ID, constructor);
        }

        if (type.BaseType?.GetGenericTypeDefinition() == typeof(SkippableChunk<>))
        {
            SkippableChunks.Add(type);
        }
    }

    internal static Func<CMwNod> GetClassConstructor(uint id)
    {
        if (ClassConstructors.TryGetValue(id, out var cachedConstructor))
        {
            return cachedConstructor;
        }

        if (!ClassIdWithType.TryGetValue(id, out var cachedType))
        {
            throw new ThisShouldNotHappenException();
        }

        var constructor = CreateConstructor<CMwNod>(cachedType);

        ClassConstructors.Add(id, constructor);

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

    /*internal static void DefineTypes()
    {
        DefineTypes(AvailableClasses, AvailableInheritanceClasses, AvailableChunkTypes,
                    AvailableHeaderChunkTypes, AvailableClassAttributes, AvailableChunkAttributes,
                    AvailableChunkAttributesByType, AvailableClassConstructors, AvailableChunkConstructors,
                    AvailableHeaderChunkConstructors, SkippableChunks);
    }

    internal static void DefineTypes(IDictionary<uint, Type> availableClasses,
        IDictionary<Type, List<uint>> availableInheritanceClasses,
        IDictionary<uint, Type> availableChunkTypes,
        IDictionary<uint, Type> availableHeaderChunkTypes,
        IDictionary<Type, IEnumerable<Attribute>> availableClassAttributes,
        IDictionary<uint, IEnumerable<Attribute>> availableChunkAttributes,
        IDictionary<Type, IEnumerable<Attribute>> availableChunkAttributesByType,
        IDictionary<uint, Func<CMwNod>> availableClassConstructors,
        IDictionary<uint, Func<Chunk>> availableChunkConstructors,
        IDictionary<uint, Func<Chunk>> availableHeaderChunkConstructors,
        HashSet<Type> skippableChunks)
    {
        if (TypesDefined)
        {
            return;
        }

        var assembly = Assembly.GetExecutingAssembly();

        IEnumerable<Type> types;

        try
        {
            types = assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            types = e.Types.Where(x => x is not null)!;
        }

        var engineRelatedTypes = types.Where(IsClassFromEngines);

        var availableClassesByType = new Dictionary<Type, uint>();

        foreach (var type in engineRelatedTypes)
        {
            if (!type.IsSubclassOf(typeof(CMwNod)) && type != typeof(CMwNod)) // Engine types
                continue;

            var id = type.GetCustomAttribute<NodeAttribute>()?.ID;

            if (!id.HasValue)
                throw new Exception($"{type.Name} misses NodeAttribute.");

            availableClasses.Add(id.Value, type);
            availableClassesByType.Add(type, id.Value);
        }

        foreach (var typePair in availableClasses)
        {
            var id = typePair.Key;
            var type = typePair.Value;

            var classes = new List<uint>();

            var currentType = type.BaseType!;

            while (currentType != typeof(Node))
            {
                classes.Add(availableClassesByType[currentType]);

                currentType = currentType.BaseType!;
            }

            availableInheritanceClasses.Add(type, classes);

            var chunks = type.GetNestedTypes().Where(x => x.IsSubclassOf(typeof(Chunk)));

            var baseType = type.BaseType!;

            while (baseType!.IsSubclassOf(typeof(CMwNod)))
            {
                chunks = chunks.Concat(baseType.GetNestedTypes().Where(x => x.IsSubclassOf(typeof(Chunk))));

                baseType = baseType.BaseType;
            }

            var availableChunks = new Dictionary<uint, Type>();
            var availableHeaderChunks = new Dictionary<uint, Type>();

            foreach (var chunk in chunks)
            {
                var chunkAttribute = chunk.GetCustomAttribute<ChunkAttribute>();

                if (chunkAttribute == null)
                    throw new Exception($"Chunk {chunk.FullName} doesn't have ChunkAttribute.");

                if (chunk.GetInterface(nameof(IHeaderChunk)) == null)
                {
                    availableChunks.Add(chunkAttribute.ID, chunk);
                }
                else
                {
                    availableHeaderChunks.Add(chunkAttribute.ID, chunk);
                }

                if (chunk.BaseType?.GetGenericTypeDefinition() == typeof(SkippableChunk<>))
                {
                    skippableChunks.Add(chunk);
                }
            }

            availableChunkTypesPerClass.Add(type, availableChunks);
            availableHeaderChunkTypesPerClass.Add(type, availableHeaderChunks);
        }

        foreach (var idClassPair in availableClasses)
        {
            var id = idClassPair.Key;
            var classType = idClassPair.Value;

            availableClassAttributes.Add(classType, classType.GetCustomAttributes());
        }

        foreach (var classChunksPair in availableChunkTypesPerClass.Concat(availableHeaderChunkTypesPerClass))
        {
            var attributeDictionary = new Dictionary<uint, IEnumerable<Attribute>>();

            foreach (var chunkClassIdTypePair in classChunksPair.Value)
            {
                var id = chunkClassIdTypePair.Key;
                var chunkClass = chunkClassIdTypePair.Value;

                var attributes = chunkClass.GetCustomAttributes();

                attributeDictionary.Add(id, attributes);

                availableChunkAttributesByType[chunkClass] = attributes; // some duplicates
            }

            availableChunkAttributes[classChunksPair.Key] = attributeDictionary; // some duplicates
        }

        foreach (var idClassPair in availableClasses)
        {
            var id = idClassPair.Key;
            var classType = idClassPair.Value;

            var privateConstructor = classType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Array.Empty<Type>(), null);

            if (privateConstructor is null)
                throw new PrivateConstructorNotFoundException(classType);

            if (classType.IsAbstract)
                continue;

            var newExp = Expression.New(privateConstructor);
            var lambda = Expression.Lambda<Func<CMwNod>>(newExp);
            var compiled = lambda.Compile();

            availableClassConstructors.Add(id, compiled);
        }

        foreach (var classChunksPair in availableChunkTypesPerClass)
        {
            var constructors = GetChunkConstructors(classChunksPair);
            if (constructors is not null)
                availableChunkConstructors.Add(classChunksPair.Key, constructors);
        }

        foreach (var classChunksPair in availableHeaderChunkTypesPerClass)
        {
            var constructors = GetChunkConstructors(classChunksPair);
            if (constructors is not null)
                availableHeaderChunkConstructors.Add(classChunksPair.Key, constructors);
        }

        TypesDefined = true;
    }*/

    private static bool IsClassFromEngines(Type t)
    {
        return t?.IsClass == true && t.Namespace?.StartsWith("GBX.NET.Engines") == true;
    }

    private static Dictionary<uint, Func<Chunk>>? GetChunkConstructors(
        KeyValuePair<Type, Dictionary<uint, Type>> classChunksPair)
    {
        if (classChunksPair.Value.Count == 0) return null;

        var constructorDictionary = new Dictionary<uint, Func<Chunk>>();

        foreach (var chunkClassIdTypePair in classChunksPair.Value)
        {
            var id = chunkClassIdTypePair.Key;
            var chunkClass = chunkClassIdTypePair.Value;

            var newExp = Expression.New(chunkClass);
            var lambda = Expression.Lambda<Func<Chunk>>(newExp);
            var compiled = lambda.Compile();

            constructorDictionary.Add(id, compiled);
        }

        return constructorDictionary;
    }
}
