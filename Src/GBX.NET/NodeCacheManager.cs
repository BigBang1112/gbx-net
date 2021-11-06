using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Plug;
using GBX.NET.Exceptions;

namespace GBX.NET
{
    public static class NodeCacheManager
    {
        public static Dictionary<uint, string> Names { get; }
        public static Dictionary<uint, uint> Mappings { get; } // key: older, value: newer
        public static Dictionary<uint, string> Extensions { get; }

        public static Dictionary<uint, Type> AvailableClasses { get; }
        public static Dictionary<Type, List<uint>> AvailableInheritanceClasses { get; }
        public static Dictionary<Type, Dictionary<uint, Type>> AvailableChunkClasses { get; }
        public static Dictionary<Type, Dictionary<uint, Type>> AvailableHeaderChunkClasses { get; }

        public static Dictionary<Type, IEnumerable<Attribute>> AvailableClassAttributes { get; }
        public static Dictionary<Type, Dictionary<uint, IEnumerable<Attribute>>> AvailableChunkAttributes { get; }
        public static Dictionary<Type, IEnumerable<Attribute>> AvailableChunkAttributesByType { get; }

        public static Dictionary<uint, Func<CMwNod>> AvailableClassConstructors { get; }
        public static Dictionary<Type, Dictionary<uint, Func<Chunk>>> AvailableChunkConstructors { get; }
        public static Dictionary<Type, Dictionary<uint, Func<Chunk>>> AvailableHeaderChunkConstructors { get; }

        static NodeCacheManager()
        {
            Names = new Dictionary<uint, string>();
            Mappings = new Dictionary<uint, uint>();
            Extensions = new Dictionary<uint, string>();

            AvailableClasses = new Dictionary<uint, Type>();
            AvailableInheritanceClasses = new Dictionary<Type, List<uint>>();
            AvailableChunkClasses = new Dictionary<Type, Dictionary<uint, Type>>();
            AvailableHeaderChunkClasses = new Dictionary<Type, Dictionary<uint, Type>>();

            AvailableClassAttributes = new Dictionary<Type, IEnumerable<Attribute>>();
            AvailableChunkAttributes = new Dictionary<Type, Dictionary<uint, IEnumerable<Attribute>>>();
            AvailableChunkAttributesByType = new Dictionary<Type, IEnumerable<Attribute>>();

            AvailableClassConstructors = new Dictionary<uint, Func<CMwNod>>();
            AvailableChunkConstructors = new Dictionary<Type, Dictionary<uint, Func<Chunk>>>();
            AvailableHeaderChunkConstructors = new Dictionary<Type, Dictionary<uint, Func<Chunk>>>();

            DefineNames();
            DefineMappings();
            DefineTypes();
        }

        internal static T GetNodeInstance<T>(uint classId) where T : CMwNod
        {
            if (AvailableClassConstructors.TryGetValue(classId, out Func<CMwNod> constructor))
                return (T)constructor();
            throw new NodeNotFoundException(classId);
        }

        private static void DefineNames()
        {
            var watch = Stopwatch.StartNew();

            using var reader = new StringReader(Resources.ClassID);

            var en = "";
            var engineName = "";

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var ch = "000";

                var className = "";

                if (line.StartsWith("  "))
                {
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
                        Names[classID] = engineName + "::" + className;
                        if (extension != null)
                            Extensions[classID] = extension;
                    }
                    else
                    {
                        Debug.WriteLine($"Invalid class ID {classIDString}, skipping");
                    }
                }
                else
                {
                    en = line.Substring(0, 2);
                    if (line.Length - 3 > 0) engineName = line.Substring(3);
                }
            }

            Debug.WriteLine("Classes named in " + watch.Elapsed.TotalMilliseconds + "ms");
        }

        private static void DefineMappings()
        {
            var watch = Stopwatch.StartNew();

            using var reader = new StringReader(Resources.ClassIDMappings);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var valueKey = line.Split(new string[] { " -> " }, StringSplitOptions.None);
                if (valueKey.Length == 2)
                {
                    if (uint.TryParse(valueKey[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint key)
                    && uint.TryParse(valueKey[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint value))
                    {
                        if (Mappings.ContainsValue(key)) // Virtual Skipper solution
                            Mappings[Mappings.FirstOrDefault(x => x.Value == key).Key] = value;
                        Mappings[key] = value;
                    }
                }
            }

            Debug.WriteLine("Mappings defined in " + watch.Elapsed.TotalMilliseconds + "ms");
        }

        private static void DefineTypes()
        {
            var watch = Stopwatch.StartNew();

            var assembly = Assembly.GetExecutingAssembly();

            IEnumerable<Type> types;

            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                types = e.Types;
            }

            var engineRelatedTypes = types.Where(t =>
                 t?.IsClass == true
              && t.Namespace?.StartsWith("GBX.NET.Engines") == true);

            var availableClassesByType = new Dictionary<Type, uint>();

            foreach (var type in engineRelatedTypes)
            {
                if (type.IsSubclassOf(typeof(CMwNod)) || type == typeof(CMwNod)) // Engine types
                {
                    var id = type.GetCustomAttribute<NodeAttribute>()?.ID;

                    if (id.HasValue)
                    {
                        AvailableClasses.Add(id.Value, type);
                        availableClassesByType.Add(type, id.Value);
                    }
                    else
                    {
                        throw new Exception($"{type.Name} misses NodeAttribute.");
                    }
                }
            }

            foreach (var typePair in AvailableClasses)
            {
                var id = typePair.Key;
                var type = typePair.Value;

                var classes = new List<uint>();

                Type currentType = type.BaseType;

                while (currentType != typeof(object))
                {
                    classes.Add(availableClassesByType[currentType]);

                    currentType = currentType.BaseType;
                }

                AvailableInheritanceClasses[type] = classes;

                var chunks = type.GetNestedTypes().Where(x => x.IsSubclassOf(typeof(Chunk)));

                var baseType = type.BaseType;

                while (baseType.IsSubclassOf(typeof(CMwNod)))
                {
                    chunks = chunks.Concat(baseType.GetNestedTypes().Where(x => x.IsSubclassOf(typeof(Chunk))));

                    baseType = baseType.BaseType;
                }
                
                var availableChunkClasses = new Dictionary<uint, Type>();
                var availableHeaderChunkClasses = new Dictionary<uint, Type>();

                foreach (var chunk in chunks)
                {
                    var chunkAttribute = chunk.GetCustomAttribute<ChunkAttribute>();

                    if (chunkAttribute == null)
                        throw new Exception($"Chunk {chunk.FullName} doesn't have ChunkAttribute.");

                    if (chunk.GetInterface(nameof(IHeaderChunk)) == null)
                    {
                        availableChunkClasses.Add(chunkAttribute.ID, chunk);
                    }
                    else
                    {
                        availableHeaderChunkClasses.Add(chunkAttribute.ID, chunk);
                    }
                }

                AvailableChunkClasses.Add(type, availableChunkClasses);
                AvailableHeaderChunkClasses.Add(type, availableHeaderChunkClasses);
            }

            foreach (var idClassPair in AvailableClasses)
            {
                var id = idClassPair.Key;
                var classType = idClassPair.Value;

                AvailableClassAttributes.Add(classType, classType.GetCustomAttributes());
            }

            foreach (var classChunksPair in AvailableHeaderChunkClasses.Concat(AvailableChunkClasses))
            {
                var attributeDictionary = new Dictionary<uint, IEnumerable<Attribute>>();

                foreach (var chunkClassIdTypePair in classChunksPair.Value)
                {
                    var id = chunkClassIdTypePair.Key;
                    var chunkClass = chunkClassIdTypePair.Value;

                    var attributes = chunkClass.GetCustomAttributes();

                    attributeDictionary[id] = attributes;

                    AvailableChunkAttributesByType[chunkClass] = attributes;
                }

                AvailableChunkAttributes[classChunksPair.Key] = attributeDictionary;
            }

            foreach (var idClassPair in AvailableClasses)
            {
                var id = idClassPair.Key;
                var classType = idClassPair.Value;

                var privateConstructor = classType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);

                if (privateConstructor is null)
                    throw new PrivateConstructorNotFoundException(classType);
                
                var newExp = Expression.New(privateConstructor);
                var lambda = Expression.Lambda<Func<CMwNod>>(newExp);
                var compiled = lambda.Compile();

                AvailableClassConstructors.Add(id, compiled);
            }

            foreach (var classChunksPair in AvailableChunkClasses)
            {
                var constructors = GetChunkConstructors(classChunksPair);
                if (constructors != null)
                    AvailableChunkConstructors[classChunksPair.Key] = constructors;
            }

            foreach (var classChunksPair in AvailableHeaderChunkClasses)
            {
                var constructors = GetChunkConstructors(classChunksPair);
                if (constructors != null)
                    AvailableHeaderChunkConstructors[classChunksPair.Key] = constructors;
            }

            Debug.WriteLine("Types defined in " + watch.Elapsed.TotalMilliseconds + "ms");
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
}
