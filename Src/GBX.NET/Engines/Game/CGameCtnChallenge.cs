using System.IO.Compression;
using System.Text;

namespace GBX.NET.Engines.Game;

public sealed partial class CGameCtnChallenge :
    CGameCtnChallenge.ITM2020
{
    /// <summary>
    /// The map's UID.
    /// </summary>
    public string MapUid
    {
        get => mapInfo.Id;
        set
        {
            mapInfo = new Ident(value, mapInfo.Collection, mapInfo.Author);

            ComputeCrc32();
        }
    }

    private byte[]? thumbnail;
    public byte[]? Thumbnail { get => hashedPassword; set => thumbnail = value; }

    public Id? Collection => mapInfo?.Collection;

    public int? NbBlocks => Blocks?.Count(x => x.Name != "Unassigned1");

    private byte[]? hashedPassword;
    public byte[]? HashedPassword
    {
        get => hashedPassword;
        set
        {
            hashedPassword = value;

            ComputeCrc32();
        }
    }

    private IList<CGameCtnAnchoredObject>? anchoredObjects;
    public IList<CGameCtnAnchoredObject>? AnchoredObjects { get => anchoredObjects; set => anchoredObjects = value; }

    private IList<CGameCtnZoneGenealogy>? zoneGenealogy;
    public IList<CGameCtnZoneGenealogy>? ZoneGenealogy { get => zoneGenealogy; set => zoneGenealogy = value; }

    private CScriptTraitsMetadata? scriptMetadata;
    public CScriptTraitsMetadata? ScriptMetadata { get => scriptMetadata; set => scriptMetadata = value; }

    public int? NbBakedBlocks => bakedBlocks?.Count(x => x.Name != "Unassigned1");

    private IList<CGameCtnBlock>? bakedBlocks;
    public IList<CGameCtnBlock>? BakedBlocks { get => bakedBlocks; set => bakedBlocks = value; }

    public IList<SBakedClipsAdditionalData>? BakedClipsAdditionalData { get; set; }

    public byte[]? EmbeddedDataZip { get; set; }

    private IList<string>? Textures { get; set; }

    public IList<MacroblockInstance>? MacroblockInstances { get; set; }

    string ITM2020.MapUid
    {
        get => MapUid ?? throw new Exception("MapUid not available");
        set => MapUid = value;
    }

    public interface ITM2020 : IClassVersion<CGameCtnChallenge>
    {
        string MapUid { get; set; }
    }

    public string GetEnvironment()
    {
        return Collection ?? throw new Exception("Environment not available");
    }

    public IEnumerable<CGameCtnBlock> GetBlocks(bool includeUnassigned1 = true)
    {
        if (includeUnassigned1)
        {
            return blocks ?? [];
        }

        return blocks?.Where(x => x.Name != "Unassigned1") ?? [];
    }

    public IEnumerable<CGameCtnAnchoredObject> GetAnchoredObjects()
    {
        return anchoredObjects ?? [];
    }

    public IEnumerable<CGameCtnBlock> GetBakedBlocks()
    {
        return bakedBlocks ?? [];
    }

    public ZipArchive OpenReadEmbeddedDataZip()
    {
        if (EmbeddedDataZip is null)
        {
            throw new Exception("Embedded data zip is not available.");
        }

        var ms = new MemoryStream(EmbeddedDataZip);
        return new ZipArchive(ms);
    }

    public void UpdateEmbeddedDataZip(Action<ZipArchive> update)
    {
        EmbeddedDataZip ??= [];

        using var ms = new MemoryStream(EmbeddedDataZip);
        using (var zip = new ZipArchive(ms, ZipArchiveMode.Update))
        {
            update(zip);
        }

        EmbeddedDataZip = ms.ToArray();
    }

    public async Task UpdateEmbeddedDataZipAsync(Func<ZipArchive, Task> update)
    {
        EmbeddedDataZip ??= [];

        using var ms = new MemoryStream(EmbeddedDataZip);
        using (var zip = new ZipArchive(ms, ZipArchiveMode.Update))
        {
            await update(zip);
        }

        EmbeddedDataZip = ms.ToArray();
    }

    /// <summary>
    /// Calculates the CRC32 of the map.
    /// </summary>
    public void ComputeCrc32()
    {
        string toHash;

        if (hashedPassword is null)
        {
            toHash = $"0x00000000000000000000000000000000???{MapUid}";
        }
        else
        {
            Span<char> hex = stackalloc char[hashedPassword.Length * 2];
            TryHex(hashedPassword, hex);
            toHash = $"0x{hex.ToString()}???{MapUid}";
        }

        Crc32 = Gbx.CRC32?.Hash(Encoding.ASCII.GetBytes(toHash)) ?? throw new Exception("CRC32 is not imported (ICrc32).");

        static void TryHex(ReadOnlySpan<byte> value, Span<char> chars)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var hex1 = HexIntToChar((byte)(value[value.Length - 1 - i] % 16));
                var hex2 = HexIntToChar((byte)(value[value.Length - 1 - i] / 16));

                chars[i * 2 + 1] = hex1;
                chars[i * 2] = hex2;
            }

            return;

            static char HexIntToChar(byte v)
            {
                if (v < 10)
                {
                    return (char)(v + 48);
                }

                return (char)(v + 55);
            }
        }
    }

    [ChunkGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class HeaderChunk03043007 : IVersionable
    {
        public int Version { get; set; } = 1;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version == 0)
            {
                return;
            }

            var thumbnailSize = rw.Int32(n.thumbnail?.Length ?? 0);
            rw.Marker("<Thumbnail.jpg>");
            rw.Data(ref n.thumbnail, thumbnailSize);
            rw.Marker("</Thumbnail.jpg>");
            rw.Marker("<Comments>");
            rw.String(ref n.comments);
            rw.Marker("</Comments>");
        }
    }

    [ChunkGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class Chunk03043013;

    public partial class Chunk0304301F : IVersionable
    {
        public int Version { get; set; } = 6;

        public bool U01;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            n.mapInfo = r.ReadIdent();
            n.mapName = r.ReadString();
            n.decoration = r.ReadIdent();
            n.size = r.ReadInt3();
            U01 = r.ReadBoolean();
            Version = r.ReadInt32();

            var nbBlocks = r.ReadInt32();
            n.blocks = new List<CGameCtnBlock>(nbBlocks);

            for (var i = 0; i < nbBlocks; i++)
            {
                var block = r.ReadReadable<CGameCtnBlock>(Version);
                n.blocks.Add(block);

                if (block.Flags == -1)
                {
                    i--;
                }
            }

            while ((r.PeekUInt32() & 0xC0000000) > 0)
            {
                n.blocks.Add(r.ReadReadable<CGameCtnBlock>(Version));
            }
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(n.mapInfo);
            w.Write(n.mapName);
            w.Write(n.decoration);
            w.Write(n.size);
            w.Write(U01);
            w.Write(Version);

            w.Write(n.NbBlocks.GetValueOrDefault());

            if (n.blocks is null)
            {
                return;
            }

            foreach (var block in n.blocks)
            {
                w.WriteWritable(block, Version);
            }
        }
    }

    public partial class Chunk03043040
    {
        public int U01;
        public int[]? U02;

        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; } = 4;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            Version = r.ReadInt32();
            U01 = r.ReadInt32(); // always 0
            var size = r.ReadInt32();

            using var _ = new Encapsulation(r);

            n.anchoredObjects = r.ReadListNodeRef_deprec<CGameCtnAnchoredObject>()!;

            if (Version >= 1 && Version != 5)
            {
                // defines which (second element) items are deleted together with other (first element) item?
                var itemsOnItem = r.ReadArray<Int2>();

                foreach (var item in itemsOnItem)
                {
                    n.anchoredObjects[item.Y].PlacedOnItem = n.anchoredObjects[item.X];
                }
            }

            if (Version >= 5)
            {
                var blockIndexes = r.ReadArray<int>(); // block indexes, -1 means itemIndexes will have the value instead
                var usedBlocks = new CGameCtnBlock?[blockIndexes.Length];
                var blocksWithoutUnassigned = n.blocks!.Where(x => x.Flags != -1).ToArray();

                for (var i = 0; i < blockIndexes.Length; i++)
                {
                    var index = blockIndexes[i];

                    if (index > -1)
                    {
                        usedBlocks[i] = blocksWithoutUnassigned[index];
                    }
                }

                var snapItemGroups = Version < 7 ? r.ReadArray<int>() : null; // snap item group - only some snapped items will delete on a block. they are consistent numbers

                var usedItems = default(CGameCtnAnchoredObject[]);

                if (Version >= 6)
                {
                    var itemIndexes = r.ReadArray<int>(); // item indexes
                    usedItems = new CGameCtnAnchoredObject[itemIndexes.Length];

                    for (var i = 0; i < itemIndexes.Length; i++)
                    {
                        var index = itemIndexes[i];

                        if (index > -1)
                        {
                            usedItems[i] = n.anchoredObjects![index];
                        }
                    }
                }

                snapItemGroups ??= Version >= 7 ? r.ReadArray<int>() : null;

                if (Version != 6)
                {
                    var U07 = r.ReadArray<int>();

                    if (U07.Any(x => x != -1))
                    {
                        throw new NotSupportedException("U07 has something else than -1");
                    }
                }

                // always the same count as anchoredObjects
                var snappedIndexes = r.ReadArray<int>(); // "snapped onto block/item" indexes

                for (var i = 0; i < snappedIndexes.Length; i++)
                {
                    var snappedIndex = snappedIndexes[i];

                    if (snappedIndex <= -1)
                    {
                        continue;
                    }

                    var usedBlock = usedBlocks[snappedIndex];

                    if (usedBlock is not null)
                    {
                        n.anchoredObjects[i].SnappedOnBlock = usedBlock;
                    }

                    var usedItem = usedItems?[snappedIndex];

                    if (usedItem is not null)
                    {
                        n.anchoredObjects[i].SnappedOnItem = usedItem;
                    }

                    n.anchoredObjects[i].SnappedOnGroup = snapItemGroups?[snappedIndex] ?? 0;
                }

                if (Version >= 8)
                {
                    throw new ChunkVersionNotSupportedException(Version);
                }
            }
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(Version);
            w.Write(U01);

            using var itemMs = new MemoryStream();
            using var itemW = new GbxWriter(itemMs);
            using var _ = new Encapsulation(itemW);

            itemW.WriteListNodeRef_deprec((n.anchoredObjects ?? [])!);

            var itemDict = new Dictionary<CGameCtnAnchoredObject, int>();

            if (n.anchoredObjects is not null)
            {
                for (var i = 0; i < n.anchoredObjects.Count; i++)
                {
                    itemDict[n.anchoredObjects[i]] = i;
                }
            }

            if (Version >= 1 && Version != 5)
            {
                var pairs = new List<Int2>();

                if (n.anchoredObjects is not null)
                {
                    for (var i = 0; i < n.anchoredObjects.Count; i++)
                    {
                        var placedOnItem = n.anchoredObjects[i].PlacedOnItem;

                        if (placedOnItem is not null && itemDict.TryGetValue(placedOnItem, out int index))
                        {
                            pairs.Add((index, i));
                        }
                    }
                }

                itemW.WriteList(pairs);
            }

            if (Version >= 5)
            {
                var blockDict = new Dictionary<CGameCtnBlock, int>();

                if (n.blocks is not null)
                {
                    for (var i = 0; i < n.blocks.Count; i++)
                    {
                        blockDict[n.blocks[i]] = i;
                    }
                }

                var usedBlockIndexHashSet = new HashSet<(int blockIndex, int group)>();
                var usedBlockIndexList = new List<(int blockIndex, int group)>();
                var usedItemIndexHashSet = new HashSet<(int itemIndex, int group)>();
                var usedItemIndexList = new List<(int itemIndex, int group)>();

                var indicesOnUsedBlocksAndItems = new Dictionary<(int index, int group), int>();
                var snappedOnIndices = new List<int>(n.anchoredObjects?.Count ?? 0);

                foreach (var item in n.GetAnchoredObjects())
                {
                    if (item.SnappedOnBlock is null && item.SnappedOnItem is null)
                    {
                        snappedOnIndices.Add(-1);
                        continue;
                    }

                    var groupIndex = item.SnappedOnGroup ?? 0;
                    var unique = (-1, groupIndex);

                    if (item.SnappedOnBlock is not null)
                    {
                        var blockIndex = blockDict[item.SnappedOnBlock];

                        unique = (blockIndex, groupIndex);

                        if (!usedBlockIndexHashSet.Contains(unique))
                        {
                            usedBlockIndexList.Add(unique);
                            usedBlockIndexHashSet.Add(unique);

                            if (item.SnappedOnItem is null)
                            {
                                usedItemIndexList.Add((-1, groupIndex));
                            }
                        }
                    }

                    if (item.SnappedOnItem is not null)
                    {
                        var itemIndex = itemDict[item.SnappedOnItem];

                        unique = (itemIndex, groupIndex);

                        if (!usedItemIndexHashSet.Contains(unique))
                        {
                            usedItemIndexList.Add(unique);
                            usedItemIndexHashSet.Add(unique);

                            if (item.SnappedOnBlock is null)
                            {
                                usedBlockIndexList.Add((-1, groupIndex));
                            }
                        }
                    }

                    if (indicesOnUsedBlocksAndItems.TryGetValue(unique, out int indexOfBlockOrItemIndex))
                    {
                        snappedOnIndices.Add(indexOfBlockOrItemIndex);
                    }
                    else
                    {
                        indicesOnUsedBlocksAndItems[unique] = indicesOnUsedBlocksAndItems.Count;
                        snappedOnIndices.Add(indicesOnUsedBlocksAndItems.Count - 1);
                    }
                }

                itemW.WriteArray(usedBlockIndexList.Select(x => x.blockIndex).ToArray());

                if (Version < 7)
                {
                    itemW.WriteArray(usedBlockIndexList.Select(x => x.group).ToArray());
                }

                if (Version >= 6)
                {
                    itemW.WriteArray(usedItemIndexList.Select(x => x.itemIndex).ToArray());
                }

                if (Version >= 7)
                {
                    itemW.WriteArray(usedBlockIndexList.Select(x => x.group).ToArray());
                }

                if (Version != 6)
                {
                    itemW.WriteArray(Enumerable.Repeat(-1, usedBlockIndexList.Count).ToArray());
                }

                itemW.WriteArray(snappedOnIndices.ToArray());
            }

            w.Write((int)itemMs.Length);
            w.Write(itemMs.ToArray());
        }
    }

    public partial class Chunk03043043
    {
        public int U01;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            U01 = r.ReadInt32(); // always 0
            var size = r.ReadInt32();

            using var _ = new Encapsulation(r);

            n.zoneGenealogy = r.ReadListNodeRef<CGameCtnZoneGenealogy>()!;
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(U01);

            using var ms = new MemoryStream();
            using var wBuffer = new GbxWriter(ms);
            using var _ = new Encapsulation(w);

            wBuffer.WriteListNodeRef((n.zoneGenealogy ?? [])!);

            w.Write((int)ms.Length);
            w.Write(ms.ToArray());
        }
    }

    public partial class Chunk03043044
    {
        public int U01;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            U01 = r.ReadInt32(); // always 0
            var size = r.ReadInt32();

            using var _ = new Encapsulation(r);

            n.scriptMetadata = r.ReadNode<CScriptTraitsMetadata>()!;
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(U01);

            using var ms = new MemoryStream();
            using var wBuffer = new GbxWriter(ms);
            using var _ = new Encapsulation(w);

            wBuffer.WriteNode(n.scriptMetadata!);

            w.Write((int)ms.Length);
            w.Write(ms.ToArray());
        }
    }

    public partial class Chunk03043048 : IVersionable
    {
        public int Version { get; set; }
        public int BlocksVersion { get; set; } = 6;
        public int U01;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            Version = r.ReadInt32();

            if (Version >= 1)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            BlocksVersion = r.ReadInt32();

            var nbBakedBlocks = r.ReadInt32();
            n.bakedBlocks = new List<CGameCtnBlock>(nbBakedBlocks);

            for (var i = 0; i < nbBakedBlocks; i++)
            {
                var block = r.ReadReadable<CGameCtnBlock>(BlocksVersion);
                n.bakedBlocks.Add(block);

                if (block.Flags == -1)
                {
                    i--;
                }
            }

            while ((r.PeekUInt32() & 0xC0000000) > 0)
            {
                n.bakedBlocks.Add(r.ReadReadable<CGameCtnBlock>(BlocksVersion));
            }

            U01 = r.ReadInt32();

            n.BakedClipsAdditionalData = r.ReadListReadable<SBakedClipsAdditionalData>(Version);
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(Version);
            w.Write(BlocksVersion);

            w.Write(n.NbBakedBlocks.GetValueOrDefault());

            if (n.bakedBlocks is not null)
            {
                foreach (var block in n.bakedBlocks)
                {
                    w.WriteWritable(block, BlocksVersion);
                }
            }

            w.Write(U01);

            w.WriteListWritable(n.BakedClipsAdditionalData, Version);
        }
    }

    [ArchiveGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
    public partial class SBakedClipsAdditionalData;

    public partial class Chunk03043054 : IVersionable
    {
        public int Version { get; set; }

        public int U01;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            Version = r.ReadInt32();
            U01 = r.ReadInt32(); // always 0
            var size = r.ReadInt32();

            using var _ = new Encapsulation(r);

            var embeddedItemModels = r.ReadArrayIdent(); // ignored, could be used for validation

            n.EmbeddedDataZip = r.ReadData();

            if (Version >= 1)
            {
                n.Textures = r.ReadListString();
            }
        }
    }

    public partial class Chunk0304305F : IVersionable
    {
        /// <summary>
        /// Version of the chunk.
        /// </summary>
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            foreach (var block in n.GetBlocks().Concat(n.GetBakedBlocks()).Where(x => x.IsFree))
            {
                block.AbsolutePositionInMap = rw.Vec3(block.AbsolutePositionInMap);
                block.PitchYawRoll = rw.Vec3(block.PitchYawRoll);
            }
        }
    }

    public partial class Chunk03043062 : IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            foreach (var block in n.GetBlocks())
            {
                block.Color = (DifficultyColor)rw.Byte((byte)block.Color);
            }

            foreach (var block in n.GetBakedBlocks())
            {
                block.Color = (DifficultyColor)rw.Byte((byte)block.Color);
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                item.Color = (DifficultyColor)rw.Byte((byte)item.Color);
            }
        }
    }

    public partial class Chunk03043063 : IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                item.AnimPhaseOffset = (CGameCtnAnchoredObject.EPhaseOffset)rw.Byte((byte)item.AnimPhaseOffset);
            }
        }
    }

    public partial class Chunk03043065 : IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                var hasForegroundPackDesc = rw.Boolean(item.ForegroundPackDesc is not null, asByte: true);

                if (hasForegroundPackDesc)
                {
                    item.ForegroundPackDesc = rw.PackDesc(item.ForegroundPackDesc);
                }
            }
        }
    }

    public partial class Chunk03043068 : IVersionable
    {
        // It has not been 100% validated if this is lightmap quality per block/object or not, but a lot of things hint towards it

        public int Version { get; set; }

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 1)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            foreach (var block in n.GetBlocks())
            {
                block.LightmapQuality = (LightmapQuality)rw.Byte((byte)block.LightmapQuality);
            }

            foreach (var block in n.GetBakedBlocks())
            {
                block.LightmapQuality = (LightmapQuality)rw.Byte((byte)block.LightmapQuality);
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                item.LightmapQuality = (LightmapQuality)rw.Byte((byte)item.LightmapQuality);
            }
        }
    }

    public partial class Chunk03043069
    {
        public int Version { get; set; }

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            Version = r.ReadInt32();

            if (Version > 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            var dict = new Dictionary<int, MacroblockInstance>();

            foreach (var block in n.GetBlocks())
            {
                var macroblockId = r.ReadInt32();

                if (macroblockId == -1)
                {
                    continue;
                }

                if (!dict.TryGetValue(macroblockId, out var instance))
                {
                    instance = new MacroblockInstance();
                    dict[macroblockId] = instance;
                }

                block.MacroblockReference = instance;
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                var macroblockId = r.ReadInt32();

                if (macroblockId == -1)
                {
                    continue;
                }

                if (!dict.TryGetValue(macroblockId, out var instance))
                {
                    instance = new MacroblockInstance();
                    dict[macroblockId] = instance;
                }

                item.MacroblockReference = instance;
            }

            var idFlagsPair = r.ReadArray<(int, int)>();

            foreach (var (id, flags) in idFlagsPair)
            {
                dict[id].Flags = flags;
            }

            n.MacroblockInstances = dict.Values.ToList();
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(Version);

            var dict = new Dictionary<MacroblockInstance, int>();

            if (n.MacroblockInstances is not null)
            {
                for (var i = 0; i < n.MacroblockInstances.Count; i++)
                {
                    dict[n.MacroblockInstances[i]] = i;
                }
            }

            foreach (var block in n.GetBlocks())
            {
                if (block.MacroblockReference is not null && dict.TryGetValue(block.MacroblockReference, out int index))
                {
                    w.Write(index);
                }
                else
                {
                    w.Write(-1);
                }
            }

            foreach (var item in n.GetAnchoredObjects())
            {
                if (item.MacroblockReference is not null && dict.TryGetValue(item.MacroblockReference, out int index))
                {
                    w.Write(index);
                }
                else
                {
                    w.Write(-1);
                }
            }

            for (var i = 0; i < n.MacroblockInstances?.Count; i++)
            {
                w.Write(i);
                w.Write(n.MacroblockInstances[i].Flags);
            }
        }
    }
}
