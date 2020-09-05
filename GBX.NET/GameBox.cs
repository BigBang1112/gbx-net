using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GBX.NET
{
    public class GameBox<T> : GameBox, IDisposable where T : Node
    {
        public override short Version => Header.Result.Parameters.Version;
        public override char? ByteFormat => Header.Result.Parameters.ByteFormat;
        public override char? RefTableCompression => Header.Result.Parameters.RefTableCompression;
        public override char? BodyCompression => Header.Result.Parameters.BodyCompression;
        public override char? UnknownByte => Header.Result.Parameters.UnknownByte;
        public override uint? ClassID => Header.Result.Parameters.ClassID;
        public override int? NumNodes => Header.Result.Parameters.NumNodes;

        public new Task<GameBoxHeader<T>> Header { get; private set; }
        public GameBoxRefTable RefTable { get; private set; }
        public GameBoxBody<T> Body { get; private set; }
        public ClassIDRemap Game { get; set; }

        public T MainNode { get; internal set; }

        public GameBox()
        {
            Game = ClassIDRemap.ManiaPlanet;
        }

        public TChunk CreateHeaderChunk<TChunk>() where TChunk : HeaderChunk<T>
        {
            return (TChunk)Header.Result.Chunks.Create<TChunk>();
        }

        public void RemoveAllHeaderChunks()
        {
            Header.Result.Chunks.Clear();
        }

        public bool RemoveHeaderChunk<TChunk>() where TChunk : HeaderChunk<T>
        {
            return Header.Result.Chunks.Remove(typeof(TChunk).GetCustomAttribute<ChunkAttribute>().ID);
        }

        public TChunk CreateBodyChunk<TChunk>(byte[] data) where TChunk : Chunk<T>
        {
            return Body.Chunks.Create<TChunk>(data);
        }

        public TChunk CreateBodyChunk<TChunk>() where TChunk : Chunk<T>
        {
            return CreateBodyChunk<TChunk>(new byte[0]);
        }

        public void RemoveAllBodyChunks()
        {
            Body.Chunks.Clear();
        }

        public bool RemoveBodyChunk<TChunk>() where TChunk : Chunk<T>
        {
            return Body.Chunks.Remove<TChunk>();
        }

        public void DiscoverAllChunks()
        {
            foreach (var chunk in Header.Result.Chunks.Values)
                chunk.Discover();
            foreach (ISkippableChunk chunk in Body.Chunks.Values)
                chunk.Discover();
        }

        public override bool Read(Stream stream, bool withoutBody)
        {
            using var gbxr = new GameBoxReader(stream);

            // Header

            Log.Write("Reading the header...");

            var parameters = new GameBoxHeaderParameters();
            if (!parameters.Read(gbxr)) return false;

            Log.Write("Working out the header chunks in the background...");

            Header = Task.Run(() => new GameBoxHeader<T>(this, parameters));
            Header.ContinueWith(x =>
            {
                if (x.IsCompletedSuccessfully)
                    Log.Write("Header chunks parsed without any exceptions.", ConsoleColor.Green);
                else
                    Log.Write("Header chunks parsed with exceptions.", ConsoleColor.Red);
            });

            // Reference table

            Log.Write("Reading the reference table...");

            var numExternalNodes = gbxr.ReadInt32();

            if (numExternalNodes > 0)
            {
                var ancestorLevel = gbxr.ReadInt32();

                GameBoxRefTableFolder rootFolder = new GameBoxRefTableFolder("Root");

                var numSubFolders = gbxr.ReadInt32();
                ReadRefTableFolders(numSubFolders, ref rootFolder);

                void ReadRefTableFolders(int n, ref GameBoxRefTableFolder folder)
                {
                    for (var i = 0; i < n; i++)
                    {
                        var name = gbxr.ReadString();
                        var numSubFolders = gbxr.ReadInt32();

                        var f = new GameBoxRefTableFolder(name, folder);
                        folder.Folders.Add(f);

                        ReadRefTableFolders(numSubFolders, ref f);
                    }
                }

                var externalNodes = new ExternalNode[numExternalNodes];

                for (var i = 0; i < numExternalNodes; i++)
                {
                    string fileName = null;
                    int? resourceIndex = null;
                    bool? useFile = null;
                    int? folderIndex = null;

                    var flags = gbxr.ReadInt32();

                    if ((flags & 4) == 0)
                        fileName = gbxr.ReadString();
                    else
                        resourceIndex = gbxr.ReadInt32();

                    var nodeIndex = gbxr.ReadInt32();

                    if (parameters.Version >= 5)
                        useFile = gbxr.ReadBoolean();

                    if ((flags & 4) == 0)
                        folderIndex = gbxr.ReadInt32();

                    var extNode = new ExternalNode(flags, fileName, resourceIndex, nodeIndex, useFile, folderIndex);
                    externalNodes[i] = extNode;
                }

                var refTable = new GameBoxRefTable(rootFolder, externalNodes);
                RefTable = refTable;
            }
            else
            {
                Log.Write("No external nodes found, reference table completed.", ConsoleColor.Green);
            }

            // Body

            if (!withoutBody)
            {
                Log.Write("Reading the body...");

                switch (parameters.BodyCompression)
                {
                    case 'C':
                        var uncompressedSize = gbxr.ReadInt32();
                        var compressedSize = gbxr.ReadInt32();

                        var data = gbxr.ReadBytes(compressedSize);

                        Body = GameBoxBody<T>.DecompressAndConstruct(this, parameters.ClassID.GetValueOrDefault(), data, compressedSize, uncompressedSize);
                        break;
                    case 'U':
                        var uncompressedData = gbxr.ReadBytes((int)(stream.Length - stream.Position));
                        Body = new GameBoxBody<T>(this, parameters.ClassID.GetValueOrDefault(), uncompressedData, null, uncompressedData.Length);
                        break;
                    default:
                        Task.WaitAll(Header);
                        return false;
                }

                Log.Write("Body completed!");

                Task.WaitAll(Header);
            }
            else
                Task.WaitAll(Header);
            
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
            using var ms = new MemoryStream();
            using var w = new GameBoxWriter(ms);
            Write(w, remap);
            ms.Position = 0;
            File.WriteAllBytes(fileName, ms.ToArray());
        }

        public void Save(string fileName)
        {
            Save(fileName, ClassIDRemap.Latest);
        }

        public void Dispose()
        {
            Header = null;
            RefTable = null;
            Body = null;
        }
    }

    public class GameBox : IGameBox
    {
        public ClassIDRemap Game { get; set; }

        public virtual short Version => Header.Result.Parameters.Version;
        public virtual char? ByteFormat => Header.Result.Parameters.ByteFormat;
        public virtual char? RefTableCompression => Header.Result.Parameters.RefTableCompression;
        public virtual char? BodyCompression => Header.Result.Parameters.BodyCompression;
        public virtual char? UnknownByte => Header.Result.Parameters.UnknownByte;
        public virtual uint? ClassID => Header.Result.Parameters.ClassID;
        public virtual int? NumNodes => Header.Result.Parameters.NumNodes;

        public Task<GameBoxHeader> Header { get; private set; }

        public string FileName { get; set; }

        public virtual bool Read(Stream stream, bool withoutBody)
        {
            using var gbxr = new GameBoxReader(stream);

            var parameters = new GameBoxHeaderParameters();
            if (!parameters.Read(gbxr)) return false;

            Log.Write("Working out the header chunks in the background...");

            Header = Task.Run(() => new GameBoxHeader(this, parameters));
            Header.ContinueWith(x =>
            {
                if(x.IsCompletedSuccessfully)
                    Log.Write("Header chunks parsed without any exceptions.");
                else
                    Log.Write("Header chunks parsed with exceptions.");
            });

            return true;
        }

        public bool Read(Stream stream)
        {
            return Read(stream, false);
        }

        public bool Load(string fileName)
        {
            Log.Write($"Loading {fileName}...");

            FileName = fileName;

            using var fs = File.OpenRead(fileName);
            var success = Read(fs);

            if(success) Log.Write($"Loaded {fileName}!", ConsoleColor.Green);
            else Log.Write($"File {fileName} has't loaded successfully.", ConsoleColor.Red);

            return success;
        }

        public bool Load(string fileName, bool loadToMemory)
        {
            if (loadToMemory)
            {
                using var ms = new MemoryStream(File.ReadAllBytes(fileName));
                return Read(ms);
            }

            return Load(fileName);
        }

        public static Type GetGameBoxType(Stream stream)
        {
            var gbxr = new GameBoxReader(stream);

            var parameters = new GameBoxHeaderParameters();
            parameters.Read(gbxr);

            if (parameters.Version >= 3)
            {
                var modernID = parameters.ClassID.GetValueOrDefault();
                if (Node.Mappings.TryGetValue(parameters.ClassID.GetValueOrDefault(), out uint newerClassID))
                    modernID = newerClassID;

                Debug.WriteLine("GetGameBoxType: " + modernID.ToString("x8"));

                var availableClass = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass
                        && x.Namespace.StartsWith("GBX.NET.Engines") && GetBaseType(x) == typeof(Node)
                        && x.GetCustomAttribute<NodeAttribute>().ID == modernID).FirstOrDefault();

                if (availableClass == null) return null;

                return typeof(GameBox<>).MakeGenericType(availableClass);
            }

            static Type GetBaseType(Type t)
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
