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
    
    public static ConcurrentDictionary<uint, IEnumerable<string>> GbxExtensions { get; }

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
        GbxExtensions = new();
        
        ChunkTypesById = new();
        ChunkIdsByType = new();
        HeaderChunkTypesById = new();

        ClassAttributesByType = new();
        ChunkAttributesById = new();
        HeaderChunkAttributesById = new();
        ChunkAttributesByType = new();

        ClassConstructors = new();
        ChunkConstructors = new();
        HeaderChunkConstructors = new();

        ClassTypesWithCachedChunks = new();
        SkippableChunks = new();

        AsyncChunksById = new();
        ReadAsyncChunksById = new();
        WriteAsyncChunksById = new();
        ReadWriteAsyncChunksById = new();
    }

    public static IEnumerable<string> GetNodeExtensions(uint classId)
    {
        CacheClassTypesIfNotCached();

        GbxExtensions.TryGetValue(classId, out var extension);

        return extension ?? Enumerable.Empty<string>();
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

        if (!NodeManager.ClassIdsByType.TryGetValue(typeof(T), out uint classId))
        {
            throw new Exception("Node cannot be instantiated.");
        }

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
            var nodeExtensions = default(List<string>);
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
                        nodeExtensions ??= new();
                        nodeExtensions.Add(nea.Extension);
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
            ClassAttributesByType[type] = attributes;

            if (nodeExtensions is not null)
            {
                GbxExtensions[nodeId] = nodeExtensions;
            }
        }

        ClassesAreCached = true;
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

        var attributes = CustomAttributeExtensions.GetCustomAttributes(type, inherit: false);

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

            return;
        }

        if (type.GetInterface(nameof(ISkippableChunk)) is not null)
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

        var cachedType = NodeManager.GetClassTypeById(id) ?? throw new ThisShouldNotHappenException();

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

    internal static void DefineMappings2(Dictionary<uint, uint> mappings)
    {
        
    }

    private static bool IsClassFromEngines(Type t)
    {
        return t?.IsClass == true && t.Namespace?.StartsWith("GBX.NET.Engines") == true;
    }
}
