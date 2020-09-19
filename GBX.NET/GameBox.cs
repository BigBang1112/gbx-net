using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GBX.NET
{
    public class GameBox<T> : GameBox where T : Node
    {
        public new Task<GameBoxHeader<T>> Header { get; private set; }
        public GameBoxBody<T> Body { get; private set; }

        public T MainNode { get; internal set; }

        public GameBox()
        {
            Game = ClassIDRemap.ManiaPlanet;
        }

        public TChunk CreateHeaderChunk<TChunk>() where TChunk : HeaderChunk<T>
        {
            return Header.Result.Chunks.Create<TChunk>();
        }

        public void RemoveAllHeaderChunks()
        {
            Header.Result.Chunks.Clear();
        }

        public bool RemoveHeaderChunk<TChunk>() where TChunk : HeaderChunk<T>
        {
            return Header.Result.Chunks.RemoveWhere(x => x.ID == typeof(TChunk).GetCustomAttribute<ChunkAttribute>().ID) > 0;
        }

        public TChunk CreateBodyChunk<TChunk>(byte[] data) where TChunk : Chunk<T>
        {
            return MainNode.Chunks.Create<TChunk>(data);
        }

        public TChunk CreateBodyChunk<TChunk>() where TChunk : Chunk<T>
        {
            return CreateBodyChunk<TChunk>(new byte[0]);
        }

        public void RemoveAllBodyChunks()
        {
            MainNode.Chunks.Clear();
        }

        public bool RemoveBodyChunk<TChunk>() where TChunk : Chunk<T>
        {
            return MainNode.Chunks.Remove<TChunk>();
        }

        public void DiscoverAllChunks()
        {
            //foreach (var chunk in Header.Result.Chunks.Values)
            //    chunk.Discover();
            foreach (var chunk in MainNode.Chunks)
                if(chunk is ISkippableChunk s)
                    s.Discover();
        }

        public override bool ReadHeader(GameBoxReader reader)
        {
            var parameters = new GameBoxHeaderParameters(this);
            if (!parameters.Read(reader)) return false; // Should already throw an exception

            Log.Write("Working out the header chunks in the background...");

            Header = Task.Run(() => new GameBoxHeader<T>(this, parameters.UserData));
            Header.ContinueWith(x =>
            {
                if (x.Exception == null)
                    Log.Write("Header chunks parsed without any exceptions.", ConsoleColor.Green);
                else
                    Log.Write("Header chunks parsed with exceptions.", ConsoleColor.Red);
            });

            return true;
        }

        public override bool Read(GameBoxReader reader)
        {
            // Header

            Log.Write("Reading the header...");

            if (!ReadHeader(reader))
                return false;

            // Reference table

            Log.Write("Reading the reference table...");

            ReadRefTable(reader);

            // Body

            Log.Write("Reading the body...");

            switch (BodyCompression)
            {
                case 'C':
                    var uncompressedSize = reader.ReadInt32();
                    var compressedSize = reader.ReadInt32();

                    var data = reader.ReadBytes(compressedSize);

                    Body = GameBoxBody<T>.DecompressAndConstruct(this, ClassID.GetValueOrDefault(), data, compressedSize, uncompressedSize);
                    break;
                case 'U':
                    var uncompressedData = reader.ReadToEnd();
                    Body = new GameBoxBody<T>(this, ClassID.GetValueOrDefault(), uncompressedData, null, uncompressedData.Length);
                    break;
                default:
                    return false;
            }

            Log.Write("Body completed!");

            return true;
        }

        public void Write(GameBoxWriter w, ClassIDRemap remap)
        {
            (Header.Result as ILookbackable).LookbackWritten = false;
            (Header.Result as ILookbackable).LookbackStrings.Clear();
            Header.Result.Write(w, Body.AuxilaryNodes.Count + 1, remap);

            if (RefTable == null)
                w.Write(0);
            else
                RefTable.Write(w);

            (Body as ILookbackable).LookbackWritten = false;
            (Body as ILookbackable).LookbackStrings.Clear();
            Body.AuxilaryNodes.Clear();

            Body.Write(w, remap);
        }

        public void Write(GameBoxWriter w)
        {
            Write(w, ClassIDRemap.Latest);
        }

        public void Save(string fileName, ClassIDRemap remap)
        {
            using (var ms = new MemoryStream())
            using (var w = new GameBoxWriter(ms))
            {
                Write(w, remap);
                ms.Position = 0;
                File.WriteAllBytes(fileName, ms.ToArray());
            }
        }

        public void Save(string fileName)
        {
            Save(fileName, ClassIDRemap.Latest);
        }
    }

    public class GameBox : IGameBox
    {
        public ClassIDRemap Game { get; set; }

        public short Version { get; set; }
        public char? ByteFormat { get; set; }
        public char? RefTableCompression { get; set; }
        public char? BodyCompression { get; set; }
        public char? UnknownByte { get; set; }
        public uint? ClassID { get; internal set; }
        public int? NumNodes { get; internal set; }

        public Task<GameBoxHeader> Header { get; private set; }
        public GameBoxRefTable RefTable { get; private set; }

        public string FileName { get; set; }

        public virtual bool ReadHeader(GameBoxReader reader)
        {
            var parameters = new GameBoxHeaderParameters(this);
            if (!parameters.Read(reader)) return false;

            Log.Write("Working out the header chunks in the background...");

            Header = Task.Run(() => new GameBoxHeader(this, parameters.UserData));
            Header.ContinueWith(x =>
            {
                if (x.Exception == null)
                    Log.Write("Header chunks parsed without any exceptions.");
                else
                    Log.Write("Header chunks parsed with exceptions.");
            });

            return true;
        }

        public bool ReadRefTable(GameBoxReader reader)
        {
            var numExternalNodes = reader.ReadInt32();

            if (numExternalNodes > 0)
            {
                var ancestorLevel = reader.ReadInt32();

                GameBoxRefTableFolder rootFolder = new GameBoxRefTableFolder("Root");

                var numSubFolders = reader.ReadInt32();
                ReadRefTableFolders(numSubFolders, ref rootFolder);

                void ReadRefTableFolders(int n, ref GameBoxRefTableFolder folder)
                {
                    for (var i = 0; i < n; i++)
                    {
                        var name = reader.ReadString();
                        var numSubSubFolders = reader.ReadInt32();

                        var f = new GameBoxRefTableFolder(name, folder);
                        folder.Folders.Add(f);

                        ReadRefTableFolders(numSubSubFolders, ref f);
                    }
                }

                var externalNodes = new ExternalNode[numExternalNodes];

                for (var i = 0; i < numExternalNodes; i++)
                {
                    string fileName = null;
                    int? resourceIndex = null;
                    bool? useFile = null;
                    int? folderIndex = null;

                    var flags = reader.ReadInt32();

                    if ((flags & 4) == 0)
                        fileName = reader.ReadString();
                    else
                        resourceIndex = reader.ReadInt32();

                    var nodeIndex = reader.ReadInt32();

                    if (Version >= 5)
                        useFile = reader.ReadBoolean();

                    if ((flags & 4) == 0)
                        folderIndex = reader.ReadInt32();

                    var extNode = new ExternalNode(flags, fileName, resourceIndex, nodeIndex, useFile, folderIndex);
                    externalNodes[i] = extNode;
                }

                var refTable = new GameBoxRefTable(rootFolder, externalNodes);
                RefTable = refTable;
            }
            else
                Log.Write("No external nodes found, reference table completed.", ConsoleColor.Green);

            return true;
        }

        public virtual bool Read(GameBoxReader reader)
        {
            return ReadHeader(reader) && ReadRefTable(reader);
        }

        public bool Read(Stream stream)
        {
            using (GameBoxReader reader = new GameBoxReader(stream))
                return Read(reader);
        }

        [Obsolete]
        public bool Load(string fileName)
        {
            Log.Write($"Loading {fileName}...");

            FileName = fileName;

            using (var fs = File.OpenRead(fileName))
            {
                var success = Read(fs);

                if (success) Log.Write($"Loaded {fileName}!", ConsoleColor.Green);
                else Log.Write($"File {fileName} has't loaded successfully.", ConsoleColor.Red);

                return success;
            }
        }

        [Obsolete]
        public bool Load(string fileName, bool loadToMemory)
        {
            if (loadToMemory)
            {
                using (var ms = new MemoryStream(File.ReadAllBytes(fileName)))
                    return Read(ms);
            }

            return Load(fileName);
        }

        public static GameBox<T> Parse<T>(string fileName) where T : Node
        {
            GameBox<T> gbx = new GameBox<T>();
            using (var fs = File.OpenRead(fileName))
            {
                gbx.FileName = fileName;
                if (!gbx.Read(fs))
                    return null;
            }
            return gbx;
        }

        public static uint? ReadClassID(GameBoxReader reader)
        {
            uint? classID = null;

            reader.ReadString("GBX".Length);

            var version = reader.ReadInt16(); // Version

            if (version >= 3)
            {
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();

                if (version >= 4)
                    reader.ReadByte();

                classID = reader.ReadUInt32();
            }

            return classID;
        }

        public static uint? ReadClassID(Stream stream)
        {
            using (GameBoxReader r = new GameBoxReader(stream))
                return ReadClassID(r);
        }

        public static uint? ReadClassID(string fileName)
        {
            using (var fs = File.OpenRead(fileName))
                return ReadClassID(fs);
        }

        public static GameBox Parse(string fileName)
        {
            using (var fs = File.OpenRead(fileName))
            using (var r = new GameBoxReader(fs))
            {
                var classID = ReadClassID(r);

                if (classID.HasValue)
                {
                    var modernID = classID.GetValueOrDefault();
                    if (Node.Mappings.TryGetValue(classID.GetValueOrDefault(), out uint newerClassID))
                        modernID = newerClassID;

                    Debug.WriteLine("Parse: " + modernID.ToString("x8"));

                    Node.AvailableClasses.TryGetValue(modernID, out Type availableClass);

                    GameBox gbx;

                    if (availableClass == null)
                        gbx = new GameBox();
                    else
                        gbx = (GameBox)Activator.CreateInstance(typeof(GameBox<>).MakeGenericType(availableClass));

                    fs.Seek(0, SeekOrigin.Begin);

                    gbx.FileName = fileName;

                    if (gbx.Read(fs))
                        return gbx;
                }
            }

            Type GetBaseType(Type t)
            {
                if (t == null)
                    return null;
                if (t.BaseType == typeof(Node))
                    return t.BaseType;
                return GetBaseType(t.BaseType);
            }

            return null;
        }

        public static Type GetGameBoxType(Stream stream)
        {
            using (var r = new GameBoxReader(stream))
                return GetGameBoxType(r);
        }

        public static Type GetGameBoxType(GameBoxReader reader)
        {
            var classID = ReadClassID(reader);

            if (classID.HasValue)
            {
                var modernID = classID.GetValueOrDefault();
                if (Node.Mappings.TryGetValue(classID.GetValueOrDefault(), out uint newerClassID))
                    modernID = newerClassID;

                Debug.WriteLine("GetGameBoxType: " + modernID.ToString("x8"));

                var availableClass = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass
                        && x.Namespace.StartsWith("GBX.NET.Engines") && GetBaseType(x) == typeof(Node)
                        && x.GetCustomAttribute<NodeAttribute>().ID == modernID).FirstOrDefault();

                if (availableClass == null) return null;

                return typeof(GameBox<>).MakeGenericType(availableClass);
            }

            Type GetBaseType(Type t)
            {
                if (t == null)
                    return null;
                if (t.BaseType == typeof(Node))
                    return t.BaseType;
                return GetBaseType(t.BaseType);
            }

            return null;
        }
    }
}
