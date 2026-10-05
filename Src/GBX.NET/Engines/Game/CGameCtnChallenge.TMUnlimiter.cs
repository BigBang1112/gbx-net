using GBX.NET.Components;

namespace GBX.NET.Engines.Game;

public partial class CGameCtnChallenge
{
    private Dictionary<int, CGameCtnMediaClip.TMUnlimiter>? tempOldTMUnlimiterClipData;

    internal override void Read(GbxReaderWriter rw)
    {
        base.Read(rw);

        if (tempOldTMUnlimiterClipData is not null && clipGroupInGame is not null)
        {
            foreach (var clipData in tempOldTMUnlimiterClipData)
            {
                clipGroupInGame.Clips[clipData.Key].Clip.TMUnlimiterData = clipData.Value;
            }

            tempOldTMUnlimiterClipData = null;
        }
    }

    public partial class Chunk03043055
    {
        public Chunk3F001001? TMUnlimiterChunk;

        // empty => sets classic clips to true? related to TMCanyon?
        // if unskippable = odd unlimiter chunk

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            if (TMUnlimiterChunk is null)
            {
                return;
            }

            n.TMUnlimiterData = new TMUnlimiter();

            var versionByte = r.ReadByte();
            TMUnlimiterChunk.Version = versionByte switch
            {
                1 => 4,
                2 => 5,
                _ => throw new NotSupportedException($"Unlimiter chunk version {versionByte} not supported.")
            };

            n.TMUnlimiterData.DecorationOffset = r.ReadInt3();
            n.TMUnlimiterData.SkyDecorationVisibility = (TMUnlimiter.DecorationVisibility)r.ReadByte();

            var blockCount = r.ReadInt32();

            if (n.blocks is null) throw new InvalidOperationException("Blocks are null.");

            for (var i = 0; i < blockCount; i++)
            {
                var block = n.blocks[r.ReadInt32()];

                block.TMUnlimiterData = new CGameCtnBlock.TMUnlimiter
                {
                    OverOverSizeChunk = r.ReadByte3(),
                    IsInverted = r.ReadBoolean(asByte: true),
                    Offset = r.ReadInt3(),
                    Rotation = TMUnlimiterChunk.Version >= 5 ? r.ReadInt3() : r.ReadByte3()
                };
            }

            var mediaClipMappingCount = r.ReadInt32();
            n.tempOldTMUnlimiterClipData = new(mediaClipMappingCount);

            for (var i = 0; i < mediaClipMappingCount; i++)
            {
                // this appears before the mediatracker chunk, so this has to be stored in a private dictionary
                var mediaClipIndex = r.ReadInt32();
                var resourceType = r.ReadByte();

                CGameCtnMediaClip.TMUnlimiter.LegacyResource resource;

                switch (resourceType)
                {
                    case 0: // Parameter Set
                        var parameterSet = new CGameCtnMediaClip.TMUnlimiter.LegacyParameterSet
                        {
                            Parameters = new CGameCtnMediaClip.TMUnlimiter.Parameter[4]
                        };

                        for (var parameterIndex = 0; parameterIndex < parameterSet.Parameters.Length; parameterIndex++)
                        {
                            parameterSet.Parameters[parameterIndex] = new CGameCtnMediaClip.TMUnlimiter.Parameter
                            {
                                CatalogIndex = r.ReadByte(),
                                FunctionIndex = r.ReadByte(),
                                Value = r.ReadSingle()
                            };
                        }

                        resource = parameterSet;

                        break;
                    case 1: // Legacy Script
                        resource = new CGameCtnMediaClip.TMUnlimiter.LegacyScript
                        {
                            ByteCode = r.ReadData()
                        };
                        break;
                    default: // Unknown
                        throw new NotSupportedException($"Media clip mapping resource type {resourceType} not supported.");
                }

                n.tempOldTMUnlimiterClipData[mediaClipIndex] = new CGameCtnMediaClip.TMUnlimiter
                {
                    Resource = resource
                };
            }

            if (r.ReadUInt32() != 0xFACADE01)
            {
                throw new InvalidDataException("Unlimiter chunk did not end properly.");
            }
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            if (TMUnlimiterChunk is null)
            {
                return;
            }

            w.Write((byte)(TMUnlimiterChunk.Version == 4 ? 1 : 2));

            w.Write(n.TMUnlimiterData?.DecorationOffset ?? Vec3.Zero);
            w.Write((byte)(n.TMUnlimiterData?.SkyDecorationVisibility ?? TMUnlimiter.DecorationVisibility.Everything));

            var blocks = n.blocks ?? throw new InvalidOperationException("Blocks are null.");

            w.Write(blocks.Count(x => x.TMUnlimiterData is not null));

            foreach (var (block, i) in blocks
                .Select((block, i) => (block, i))
                .Where(x => x.block.TMUnlimiterData is not null))
            {
                w.Write(i);
                w.Write(block.TMUnlimiterData!.OverOverSizeChunk);
                w.Write(block.TMUnlimiterData.IsInverted, asByte: true);
                w.Write(block.TMUnlimiterData.Offset);

                if (TMUnlimiterChunk.Version >= 5)
                {
                    w.Write(block.TMUnlimiterData.Rotation);
                }
                else
                {
                    w.Write((Byte3)(Int3)(block.TMUnlimiterData.Rotation));
                }
            }

            w.Write(n.clipGroupInGame?.Clips.Count(x => x.Clip.TMUnlimiterData is not null) ?? 0);

            foreach (var (clip, i) in n.clipGroupInGame?.Clips
                .Select((clip, i) => (clip.Clip, i))
                .Where(x => x.Clip.TMUnlimiterData is not null) ?? [])
            {
                w.Write(i);

                switch (clip.TMUnlimiterData!.Resource)
                {
                    case CGameCtnMediaClip.TMUnlimiter.LegacyParameterSet parameterSet:
                        w.Write((byte)0); // Parameter Set
                        foreach (var parameter in parameterSet.Parameters)
                        {
                            w.Write(parameter.CatalogIndex);
                            w.Write(parameter.FunctionIndex);
                            w.Write(parameter.Value);
                        }
                        break;

                    case CGameCtnMediaClip.TMUnlimiter.LegacyScript legacyScript:
                        w.Write((byte)1); // Legacy Script
                        w.WriteData(legacyScript.ByteCode);
                        break;
                    default:
                        throw new InvalidOperationException("Unsupported TM Unlimiter media clip resource type.");
                }
            }

            w.Write(0xFACADE01);
        }
    }

    public partial class Chunk3F001000 : IVersionable
    {
        public int Version { get; set; }

        public int U01;

        [Flags]
        private enum ChallengeFlags
        {
            DecorationVisibility_SkyOnly = 1 << 0,
            IsDecorationMoved = 1 << 1,
            DecorationVisibility_Nothing = 1 << 2,
            IsDecorationScaled = 1 << 3,
            IsTrackBaseEmpty = 1 << 4,
            IsVanillaMode = 1 << 5,
            DecorationVisibility_Warp = 1 << 8,
            IsPylonsDisabled = 1 << 9,
            ReservedBit = 1 << 15,
        }

        [Flags]
        private enum BlockFlags
        {
            IsOutsideBoundaries = 1 << 0,
            IsMoved = 1 << 1,
            IsRotated = 1 << 2,
            IsScaled = 1 << 3,
            IsInverted = 1 << 4,
            IsVanillaTerrain = 1 << 5,
            IsSpawnPointFixEnabled = 1 << 6,
            IsDynamic = 1 << 7,
            IsInvisible = 1 << 8,
            IsCollisionDisabled = 1 << 9,
            IsClassicMode = 1 << 10,
            IsClassicTerrain = 1 << 11,
            HasIdentifier = 1 << 14,
            Reserved = 1 << 15,
        }

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            using var ms = new MemoryStream(Crypt(r.ReadToEnd()));
            using var decryptedReader = new GbxReader(ms);
            decryptedReader.LoadFrom(r);
            ReadDecrypted(n, decryptedReader);
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            using var ms = new MemoryStream();
            using var decryptedWriter = new GbxWriter(ms);
            decryptedWriter.LoadFrom(w);
            WriteDecrypted(n, decryptedWriter);

            w.Write(Crypt(ms.ToArray()));
        }

        private void ReadDecrypted(CGameCtnChallenge n, GbxReader r)
        {
            n.TMUnlimiterData = new TMUnlimiter();

            var challengeFlags = (ChallengeFlags)r.ReadUInt16();
            n.TMUnlimiterData.SkyDecorationVisibility = (TMUnlimiter.DecorationVisibility)((int)challengeFlags & 0x105);
            var isDecorationMoved = (challengeFlags & ChallengeFlags.IsDecorationMoved) != 0;
            var isDecorationScaled = (challengeFlags & ChallengeFlags.IsDecorationScaled) != 0;
            n.TMUnlimiterData.IsTrackBaseEmpty = (challengeFlags & ChallengeFlags.IsTrackBaseEmpty) != 0;
            n.TMUnlimiterData.IsVanillaMode = (challengeFlags & ChallengeFlags.IsVanillaMode) != 0;
            n.TMUnlimiterData.IsPylonsDisabled = (challengeFlags & ChallengeFlags.IsPylonsDisabled) != 0;

            // throw if any flags outside the enum are set
            if ((challengeFlags & ~(
                  ChallengeFlags.DecorationVisibility_SkyOnly
                | ChallengeFlags.IsDecorationMoved
                | ChallengeFlags.DecorationVisibility_Nothing
                | ChallengeFlags.IsDecorationScaled
                | ChallengeFlags.IsTrackBaseEmpty
                | ChallengeFlags.IsVanillaMode
                | ChallengeFlags.DecorationVisibility_Warp
                | ChallengeFlags.IsPylonsDisabled)) != 0)
            {
                throw new InvalidDataException($"Unknown challenge flags set: {(ushort)challengeFlags & 0xFF7F}");
            }

            if (n.TMUnlimiterData.SkyDecorationVisibility != TMUnlimiter.DecorationVisibility.Nothing)
            {
                if (isDecorationMoved)
                {
                    n.TMUnlimiterData.DecorationOffset = r.ReadVec3();
                }

                if (isDecorationScaled)
                {
                    n.TMUnlimiterData.DecorationScale = r.ReadVec3();
                }
            }

            var blockCount = r.ReadInt32();

            if (n.blocks is null) throw new InvalidOperationException("Blocks are null.");

            for (var i = 0; i < blockCount; i++)
            {
                var block = n.blocks![r.ReadInt32()];
                block.TMUnlimiterData = new CGameCtnBlock.TMUnlimiter();
                var flags = (BlockFlags)r.ReadUInt16();

                if (flags.HasFlag(BlockFlags.IsOutsideBoundaries))
                {
                    block.TMUnlimiterData.OverOverSizeChunk = r.ReadByte3();
                }

                block.TMUnlimiterData.IsInverted = flags.HasFlag(BlockFlags.IsInverted);
                block.TMUnlimiterData.IsVanillaTerrain = flags.HasFlag(BlockFlags.IsVanillaTerrain);
                block.TMUnlimiterData.IsSpawnPointFixEnabled = flags.HasFlag(BlockFlags.IsSpawnPointFixEnabled);
                block.TMUnlimiterData.IsDynamic = flags.HasFlag(BlockFlags.IsDynamic);
                block.TMUnlimiterData.IsInvisible = flags.HasFlag(BlockFlags.IsInvisible);
                block.TMUnlimiterData.IsCollisionDisabled = flags.HasFlag(BlockFlags.IsCollisionDisabled);
                block.TMUnlimiterData.IsClassicMode = flags.HasFlag(BlockFlags.IsClassicMode);
                block.TMUnlimiterData.IsClassicTerrain = flags.HasFlag(BlockFlags.IsClassicTerrain);

                // throw if any flags outside the enum are set
                if ((flags & ~(
                      BlockFlags.IsOutsideBoundaries
                    | BlockFlags.IsMoved
                    | BlockFlags.IsRotated
                    | BlockFlags.IsScaled
                    | BlockFlags.IsInverted
                    | BlockFlags.IsVanillaTerrain
                    | BlockFlags.IsSpawnPointFixEnabled
                    | BlockFlags.IsDynamic
                    | BlockFlags.IsInvisible
                    | BlockFlags.IsCollisionDisabled
                    | BlockFlags.IsClassicMode
                    | BlockFlags.IsClassicTerrain
                    | BlockFlags.HasIdentifier)) != 0)
                {
                    throw new InvalidDataException($"Unknown block flags set: {(ushort)flags & 0x7F7F}");
                }

                if (block.TMUnlimiterData.IsVanillaTerrain)
                {
                    continue;
                }

                if (flags.HasFlag(BlockFlags.IsMoved))
                {
                    block.TMUnlimiterData.Offset = r.ReadVec3();
                }

                if (flags.HasFlag(BlockFlags.IsRotated))
                {
                    block.TMUnlimiterData.Rotation = r.ReadVec3();
                }

                if (flags.HasFlag(BlockFlags.IsScaled))
                {
                    block.TMUnlimiterData.Scale = r.ReadVec3();
                }

                if (flags.HasFlag(BlockFlags.HasIdentifier))
                {
                    block.TMUnlimiterData.Group = r.ReadString();
                }
            }

            U01 = r.ReadInt32();
        }

        private void WriteDecrypted(CGameCtnChallenge n, GbxWriter w)
        {
            var challengeFlags = (ChallengeFlags)0;
            if (n.TMUnlimiterData is not null)
            {
                challengeFlags |= (ChallengeFlags)((int)n.TMUnlimiterData.SkyDecorationVisibility & 0x105);
                if (n.TMUnlimiterData.IsDecorationMoved) challengeFlags |= ChallengeFlags.IsDecorationMoved;
                if (n.TMUnlimiterData.IsDecorationScaled) challengeFlags |= ChallengeFlags.IsDecorationScaled;
                if (n.TMUnlimiterData.IsTrackBaseEmpty) challengeFlags |= ChallengeFlags.IsTrackBaseEmpty;
                if (n.TMUnlimiterData.IsVanillaMode) challengeFlags |= ChallengeFlags.IsVanillaMode;
                if (n.TMUnlimiterData.IsPylonsDisabled) challengeFlags |= ChallengeFlags.IsPylonsDisabled;
            }

            w.Write((ushort)challengeFlags);

            if (n.TMUnlimiterData is not null && n.TMUnlimiterData.SkyDecorationVisibility != TMUnlimiter.DecorationVisibility.Nothing)
            {
                if (n.TMUnlimiterData.IsDecorationMoved)
                {
                    w.Write(n.TMUnlimiterData.DecorationOffset);
                }

                if (n.TMUnlimiterData.IsDecorationScaled)
                {
                    w.Write(n.TMUnlimiterData.DecorationScale);
                }
            }

            var blocks = n.blocks ?? throw new InvalidOperationException("Blocks are null.");

            w.Write(blocks.Count(x => x.TMUnlimiterData is not null));

            foreach (var (block, i) in blocks
                .Select((block, i) => (block, i))
                .Where(x => x.block.TMUnlimiterData is not null))
            {
                w.Write(i);

                var flags = (BlockFlags)0;
                if (block.TMUnlimiterData!.IsOutsideBoundaries) flags |= BlockFlags.IsOutsideBoundaries;
                if (block.TMUnlimiterData.IsMoved) flags |= BlockFlags.IsMoved;
                if (block.TMUnlimiterData.IsRotated) flags |= BlockFlags.IsRotated;
                if (block.TMUnlimiterData.IsScaled) flags |= BlockFlags.IsScaled;
                if (block.TMUnlimiterData.IsInverted) flags |= BlockFlags.IsInverted;
                if (block.TMUnlimiterData.IsVanillaTerrain) flags |= BlockFlags.IsVanillaTerrain;
                if (block.TMUnlimiterData.IsSpawnPointFixEnabled) flags |= BlockFlags.IsSpawnPointFixEnabled;
                if (block.TMUnlimiterData.IsDynamic) flags |= BlockFlags.IsDynamic;
                if (block.TMUnlimiterData.IsInvisible) flags |= BlockFlags.IsInvisible;
                if (block.TMUnlimiterData.IsCollisionDisabled) flags |= BlockFlags.IsCollisionDisabled;
                if (block.TMUnlimiterData.IsClassicMode) flags |= BlockFlags.IsClassicMode;
                if (block.TMUnlimiterData.IsClassicTerrain) flags |= BlockFlags.IsClassicTerrain;
                if (block.TMUnlimiterData.HasIdentifier) flags |= BlockFlags.HasIdentifier;

                w.Write((ushort)flags);

                if (block.TMUnlimiterData.IsVanillaTerrain)
                {
                    continue;
                }

                if (block.TMUnlimiterData.IsMoved)
                {
                    w.Write(block.TMUnlimiterData.Offset);
                }

                if (block.TMUnlimiterData.IsRotated)
                {
                    w.Write(block.TMUnlimiterData.Rotation);
                }

                if (block.TMUnlimiterData.IsScaled)
                {
                    w.Write(block.TMUnlimiterData.Scale);
                }

                if (block.TMUnlimiterData.HasIdentifier)
                {
                    w.Write(block.TMUnlimiterData.Group);
                }
            }

            w.Write(U01);
        }

        private static byte[] Crypt(byte[] cryptedChunkData)
        {
            for (uint offset = 0; offset < cryptedChunkData.Length; offset++)
            {
                uint data = cryptedChunkData[offset];
                uint hash = (uint)(cryptedChunkData.Length * ((cryptedChunkData.Length * 2) - offset));

                hash ^= 0xEAD9C8B3;
                hash += offset * 3 % 0x7F;

                if (offset % 5 < 2)
                {
                    hash = ~hash;
                }

                cryptedChunkData[offset] = (byte)~(data ^ hash);
            }

            return cryptedChunkData;
        }
    }

    public partial class Chunk3F001001 : IVersionable
    {
        public int Version { get; set; }

        [Flags]
        private enum ChallengeFlags
        {
            DecorationVisibility_SkyOnly = 1 << 0,
            DecorationVisibility_Background = 1 << 1,
            DecorationVisibility_Nothing = 3,
            IsDecorationMoved = 1 << 2,
            IsDecorationScaled = 1 << 3,
            IsPylonsDisabled = 1 << 4,
            IsTrackBaseEmpty = 1 << 5,
            EnableVehicleCollisions = 1 << 6,
            EnableRandomStartLine = 1 << 7,
            EnableServerCommunication = 1 << 8,
            IgnoreMultiplayerTimeSyncForMotionBlocks = 1 << 9,
        }

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw) => ReadWrite(n, rw, ver: 0);

        protected void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw, int ver)
        {
            rw.VersionByte(this);

            if (Version == 0)
            {
                // vanilla
                return;
            }

            if (Version == 1)
            {
                if (rw.Reader is not null) ReadV1(n, rw.Reader);
                if (rw.Writer is not null) WriteV1(n, rw.Writer);

                return;
            }

            if (Version == 6)
            {
                if (rw.Reader is not null) ReadV6(n, rw.Reader);
                if (rw.Writer is not null) WriteV6(n, rw.Writer);

                return;
            }

            if (Version == 7)
            {
                if (rw.Reader is not null) ReadV7(n, rw.Reader, ver);
                if (rw.Writer is not null) WriteV7(n, rw.Writer, ver);

                return;
            }

            throw new ChunkVersionNotSupportedException(Version);
        }

        private static void ReadV1(CGameCtnChallenge n, GbxReader r)
        {
            n.TMUnlimiterData = new()
            {
                FakeBlocks = n.blocks
            };

            var blockCount = r.ReadInt32();
            n.blocks = new List<CGameCtnBlock>(blockCount);

            for (var i = 0; i < blockCount; i++)
            {
                n.blocks.Add(ReadGameBlock(r, byteCoord: true));
            }
        }

        private static void ReadV6(CGameCtnChallenge n, GbxReader r)
        {
            var flags = r.ReadByte();
            var isDecorationOffsetApplied = (flags & (1 << 0)) != 0;
            var isDecorationScaleApplied = (flags & (1 << 1)) != 0;
            var decorationVisibility = (TMUnlimiter.DecorationVisibility)((flags >> 2) & 3);
            var isPylonsDisabled = (flags & (1 << 4)) != 0;
            var isTrackBaseEmpty = (flags & (1 << 5)) != 0;

            n.TMUnlimiterData = new()
            {
                SkyDecorationVisibility = decorationVisibility,
                IsPylonsDisabled = isPylonsDisabled,
                IsTrackBaseEmpty = isTrackBaseEmpty
            };

            if (n.TMUnlimiterData.SkyDecorationVisibility != TMUnlimiter.DecorationVisibility.Nothing)
            {
                if (isDecorationOffsetApplied)
                {
                    n.TMUnlimiterData.DecorationOffset = r.ReadVec3();
                }

                if (isDecorationScaleApplied)
                {
                    n.TMUnlimiterData.DecorationScale = r.ReadVec3();
                }
            }

            var legacyScripts = new CGameCtnMediaClip.TMUnlimiter.LegacyScript[r.ReadInt32()];
            for (var i = 0; i < legacyScripts.Length; i++)
            {
                legacyScripts[i] = new CGameCtnMediaClip.TMUnlimiter.LegacyScript
                {
                    Name = r.ReadString(),
                    ByteCode = r.ReadData()
                };
            }

            var parameterSets = new CGameCtnMediaClip.TMUnlimiter.LegacyParameterSet[r.ReadInt32()];
            for (var i = 0; i < parameterSets.Length; i++)
            {
                parameterSets[i] = new CGameCtnMediaClip.TMUnlimiter.LegacyParameterSet
                {
                    Name = r.ReadString(),
                };

                var parameters = new CGameCtnMediaClip.TMUnlimiter.Parameter[r.ReadInt32()];
                parameterSets[i].Parameters = parameters;

                for (var j = 0; j < parameters.Length; j++)
                {
                    var functionIndex = r.ReadByte();

                    parameters[j] = new CGameCtnMediaClip.TMUnlimiter.Parameter
                    {
                        FunctionIndex = functionIndex
                    };

                    if (CGameCtnMediaClip.TMUnlimiter.Parameter.IsStringParameter(functionIndex))
                    {
                        parameters[j].StringValue = r.ReadString(StringLengthPrefix.Byte);
                    }
                    else
                    {
                        parameters[j].Value = r.ReadSingle();
                    }
                }
            }

            var mediaClipMappingCount = r.ReadInt32();

            for (var i = 0; i < mediaClipMappingCount; i++)
            {
                var mediaClipIndex = r.ReadInt32();
                var resourceType = r.ReadByte();

                switch (resourceType)
                {
                    case 0: // Parameter Set
                        var parameterSet = parameterSets[r.ReadInt32()];
                        n.clipGroupInGame?.Clips[mediaClipIndex].Clip.TMUnlimiterData = new CGameCtnMediaClip.TMUnlimiter
                        {
                            Resource = parameterSet
                        };
                        break;
                    case 1: // Legacy Script
                        var legacyScript = legacyScripts[r.ReadInt32()];
                        n.clipGroupInGame?.Clips[mediaClipIndex].Clip.TMUnlimiterData = new CGameCtnMediaClip.TMUnlimiter
                        {
                            Resource = legacyScript
                        };
                        break;
                    default:
                        throw new NotSupportedException($"Media clip mapping resource type {resourceType} not supported.");
                }
            }

            var blockGroups = r.ReadListReadable<TMUnlimiter.BlockGroup>();

            ReadBlocks(n, n.TMUnlimiterData, r, [], [], blockGroups);
        }

        private static void WriteV6(CGameCtnChallenge n, GbxWriter w)
        {
            var flags = (byte)0;
            if (n.TMUnlimiterData is not null)
            {
                if (n.TMUnlimiterData.IsDecorationMoved) flags |= 1 << 0;
                if (n.TMUnlimiterData.IsDecorationScaled) flags |= 1 << 1;
                flags |= (byte)(((int)n.TMUnlimiterData.SkyDecorationVisibility & 3) << 2);
                if (n.TMUnlimiterData.IsPylonsDisabled) flags |= 1 << 4;
                if (n.TMUnlimiterData.IsTrackBaseEmpty) flags |= 1 << 5;
            }

            w.Write(flags);

            if (n.TMUnlimiterData is not null && n.TMUnlimiterData.SkyDecorationVisibility != TMUnlimiter.DecorationVisibility.Nothing)
            {
                if (n.TMUnlimiterData.IsDecorationMoved)
                {
                    w.Write(n.TMUnlimiterData.DecorationOffset);
                }

                if (n.TMUnlimiterData.IsDecorationScaled)
                {
                    w.Write(n.TMUnlimiterData.DecorationScale);
                }
            }

            var clips = n.clipGroupInGame?.Clips ?? [];

            var legacyScripts = clips
                .Select(x => x.Clip.TMUnlimiterData?.Resource as CGameCtnMediaClip.TMUnlimiter.LegacyScript)
                .Where(x => x is not null)
                .Distinct()
                .ToList();

            w.Write(legacyScripts.Count);
            foreach (var script in legacyScripts)
            {
                w.Write(script!.Name ?? "");
                w.WriteData(script.ByteCode);
            }

            var parameterSets = clips
                .Select(x => x.Clip.TMUnlimiterData?.Resource as CGameCtnMediaClip.TMUnlimiter.LegacyParameterSet)
                .Where(x => x is not null)
                .Distinct()
                .ToList();

            w.Write(parameterSets.Count);
            foreach (var paramSet in parameterSets)
            {
                w.Write(paramSet!.Name ?? "");
                w.Write(paramSet.Parameters.Length);
                foreach (var parameter in paramSet.Parameters)
                {
                    w.Write(parameter.FunctionIndex);
                    if (CGameCtnMediaClip.TMUnlimiter.Parameter.IsStringParameter(parameter.FunctionIndex))
                    {
                        w.Write(parameter.StringValue ?? "", StringLengthPrefix.Byte);
                    }
                    else
                    {
                        w.Write(parameter.Value);
                    }
                }
            }

            var mappedClips = clips
                .Select((x, i) => (x.Clip, i))
                .Where(x => x.Clip.TMUnlimiterData?.Resource is not null)
                .ToList();

            w.Write(mappedClips.Count);
            foreach (var (clip, i) in mappedClips)
            {
                w.Write(i);
                switch (clip.TMUnlimiterData!.Resource)
                {
                    case CGameCtnMediaClip.TMUnlimiter.LegacyParameterSet parameterSet:
                        w.Write((byte)0); // Parameter Set
                        w.Write(parameterSets.IndexOf(parameterSet));
                        break;
                    case CGameCtnMediaClip.TMUnlimiter.LegacyScript legacyScript:
                        w.Write((byte)1); // Legacy Script
                        w.Write(legacyScripts.IndexOf(legacyScript));
                        break;
                    default:
                        throw new InvalidOperationException("Unsupported TM Unlimiter media clip resource type.");
                }
            }

            var blockGroups = n.blocks?.SelectMany(x => x.TMUnlimiterData?.BlockGroups ?? [])
                .Concat(n.TMUnlimiterData?.ExternalBlocks.SelectMany(x => x.BlockGroups) ?? [])
                .Distinct()
                .ToList() ?? [];

            w.WriteListWritable(blockGroups);

            WriteBlocks(n, n.TMUnlimiterData!, w, [], [], blockGroups);
        }

        private static void ReadV7(CGameCtnChallenge n, GbxReader r, int ver)
        {
            var flags = (ChallengeFlags)r.ReadUInt16();
            var isDecorationMoved = (flags & ChallengeFlags.IsDecorationMoved) != 0;
            var isDecorationScaled = (flags & ChallengeFlags.IsDecorationScaled) != 0;

            n.TMUnlimiterData = new()
            {
                SkyDecorationVisibility = (TMUnlimiter.DecorationVisibility)((int)flags & 3),
                IsPylonsDisabled = (flags & ChallengeFlags.IsPylonsDisabled) != 0,
                IsTrackBaseEmpty = (flags & ChallengeFlags.IsTrackBaseEmpty) != 0,
                EnableVehicleCollisions = (flags & ChallengeFlags.EnableVehicleCollisions) != 0,
                EnableRandomStartLine = (flags & ChallengeFlags.EnableRandomStartLine) != 0,
                EnableServerCommunication = (flags & ChallengeFlags.EnableServerCommunication) != 0,
                IgnoreMultiplayerTimeSyncForMotionBlocks = (flags & ChallengeFlags.IgnoreMultiplayerTimeSyncForMotionBlocks) != 0
            };

            // throw if any flags outside the enum are set
            if ((flags & ~(
                  ChallengeFlags.DecorationVisibility_SkyOnly
                | ChallengeFlags.DecorationVisibility_Background
                | ChallengeFlags.IsDecorationMoved
                | ChallengeFlags.IsDecorationScaled
                | ChallengeFlags.IsPylonsDisabled
                | ChallengeFlags.IsTrackBaseEmpty
                | ChallengeFlags.EnableVehicleCollisions
                | ChallengeFlags.EnableRandomStartLine
                | ChallengeFlags.EnableServerCommunication
                | ChallengeFlags.IgnoreMultiplayerTimeSyncForMotionBlocks)) != 0)
            {
                throw new InvalidDataException($"Unknown challenge flags set: {(ushort)flags & 0xFE00}");
            }

            if (n.TMUnlimiterData.SkyDecorationVisibility != TMUnlimiter.DecorationVisibility.Nothing)
            {
                if (isDecorationMoved)
                {
                    n.TMUnlimiterData.DecorationOffset = r.ReadVec3();
                }

                if (isDecorationScaled)
                {
                    n.TMUnlimiterData.DecorationScale = r.ReadVec3();
                }
            }

            n.TMUnlimiterData.AngelScriptModules = r.ReadListReadable<TMUnlimiter.AngelScriptModule>();
            n.TMUnlimiterData.ParameterSets = r.ReadListReadable<TMUnlimiter.ParameterSet>();
            var triggerGroups = r.ReadListReadable<TMUnlimiter.TriggerGroup>();
            var blockGroups = r.ReadListReadable<TMUnlimiter.BlockGroup>();

            var embeddedBlockCount = r.ReadInt32();
            var materialModelRefs = r.ReadListReadable<TMUnlimiter.MaterialModelRef>();

            var tempRefTable = new GbxRefTable();

            foreach (var materialModelRef in materialModelRefs)
            {
                r.NodeDict[materialModelRef.InstanceIndex] = new GbxRefTableFile(tempRefTable, flags: 0x40000000, useFile: false, materialModelRef.MaterialModelRelativePath);
            }

            var replacementTexture = r.ReadReadable<TMUnlimiter.ReplacementTextureFlags>();

            if (replacementTexture.SpecularInstanceIndex.HasValue)
            {
                r.NodeDict[replacementTexture.SpecularInstanceIndex.Value] = new GbxRefTableFile(tempRefTable, 0, useFile: true, "Specular");
            }

            if (replacementTexture.NormalInstanceIndex.HasValue)
            {
                r.NodeDict[replacementTexture.NormalInstanceIndex.Value] = new GbxRefTableFile(tempRefTable, 0, useFile: true, "Normal");
            }

            if (replacementTexture.WhiteInstanceIndex.HasValue)
            {
                r.NodeDict[replacementTexture.WhiteInstanceIndex.Value] = new GbxRefTableFile(tempRefTable, 0, useFile: true, "White");
            }

            if (replacementTexture.BlackInstanceIndex.HasValue)
            {
                r.NodeDict[replacementTexture.BlackInstanceIndex.Value] = new GbxRefTableFile(tempRefTable, 0, useFile: true, "Black");
            }

            n.TMUnlimiterData.EmbeddedImages = r.ReadListReadable<TMUnlimiter.EmbeddedImage>();

            foreach (var embeddedImage in n.TMUnlimiterData.EmbeddedImages)
            {
                var file = new GbxRefTableFile(tempRefTable, 0, useFile: true, embeddedImage.RelativePath);

                foreach (var bitmapPair in embeddedImage.BitmapPairs)
                {
                    r.NodeDict[bitmapPair.InstanceIndex] = file;
                    bitmapPair.Instance = file;
                }
            }

            var embeddedBlockData = r.ReadListReadable<TMUnlimiter.EmbeddedBlockData>(length: embeddedBlockCount, version: ver);

            n.TMUnlimiterData.VehicleIdentifiers = r.ReadListIdent();

            ReadBlocks(n, n.TMUnlimiterData, r, embeddedBlockData, triggerGroups, blockGroups);
        }

        private static void WriteV1(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(n.blocks?.Count ?? 0);

            foreach (var block in n.blocks ?? [])
            {
                WriteGameBlock(w, block, byteCoord: true);
            }
        }

        private static void WriteV7(CGameCtnChallenge n, GbxWriter w, int ver)
        {
            var flags = (ChallengeFlags)0;
            if (n.TMUnlimiterData is not null)
            {
                flags |= (ChallengeFlags)((int)n.TMUnlimiterData.SkyDecorationVisibility & 3);
                if (n.TMUnlimiterData.IsDecorationMoved) flags |= ChallengeFlags.IsDecorationMoved;
                if (n.TMUnlimiterData.IsDecorationScaled) flags |= ChallengeFlags.IsDecorationScaled;
                if (n.TMUnlimiterData.IsPylonsDisabled) flags |= ChallengeFlags.IsPylonsDisabled;
                if (n.TMUnlimiterData.IsTrackBaseEmpty) flags |= ChallengeFlags.IsTrackBaseEmpty;
                if (n.TMUnlimiterData.EnableVehicleCollisions) flags |= ChallengeFlags.EnableVehicleCollisions;
                if (n.TMUnlimiterData.EnableRandomStartLine) flags |= ChallengeFlags.EnableRandomStartLine;
                if (n.TMUnlimiterData.EnableServerCommunication) flags |= ChallengeFlags.EnableServerCommunication;
                if (n.TMUnlimiterData.IgnoreMultiplayerTimeSyncForMotionBlocks) flags |= ChallengeFlags.IgnoreMultiplayerTimeSyncForMotionBlocks;
            }

            w.Write((ushort)flags);

            if (n.TMUnlimiterData is not null)
            {
                if (n.TMUnlimiterData.IsDecorationMoved)
                {
                    w.Write(n.TMUnlimiterData.DecorationOffset);
                }

                if (n.TMUnlimiterData.IsDecorationScaled)
                {
                    w.Write(n.TMUnlimiterData.DecorationScale);
                }
            }

            w.WriteListWritable(n.TMUnlimiterData?.AngelScriptModules ?? []);
            w.WriteListWritable(n.TMUnlimiterData?.ParameterSets ?? []);

            var triggerGroups = n.TMUnlimiterData?.TriggerBlocks
                .SelectMany(tb => tb.TriggerGroups)
                .Distinct()
                .ToList() ?? [];

            w.WriteListWritable(triggerGroups);

            var blockGroups = n.blocks?.SelectMany(x => x.TMUnlimiterData?.BlockGroups ?? [])
                .Concat(n.TMUnlimiterData?.TriggerBlocks.SelectMany(x => x.BlockGroups) ?? [])
                .Concat(n.TMUnlimiterData?.ExternalBlocks.SelectMany(x => x.BlockGroups) ?? [])
                .Concat(n.TMUnlimiterData?.EmbeddedBlocks.SelectMany(x => x.BlockGroups) ?? [])
                .Distinct()
                .ToList();

            w.WriteListWritable(blockGroups);

            var embeddedBlockDatas = n.TMUnlimiterData?.EmbeddedBlocks.Select(x => x.Data).Distinct() ?? [];

            w.Write(embeddedBlockDatas.Count());

            using var ms = new MemoryStream();
            using var embeddedW = new GbxWriter(ms);
            embeddedW.LoadFrom(w);

            foreach (var embeddedBlockData in embeddedBlockDatas)
            {
                embeddedBlockData.Write(embeddedW, v: ver);
            }

            w.LoadFrom(embeddedW);

            var materialModelRefs = embeddedW.NodeDict
                .Where(x => x.Key is GbxRefTableFile { Flags: 0x40000000 })
                .Select(x => new TMUnlimiter.MaterialModelRef
                {
                    InstanceIndex = x.Value,
                    MaterialModelRelativePath = ((GbxRefTableFile)x.Key).FilePath
                })
                .ToList();

            w.WriteListWritable(materialModelRefs);

            int? GetReplacementTextureInstanceIndex(string name)
            {
                var match = embeddedW.NodeDict
                    .FirstOrDefault(x => x.Key is GbxRefTableFile { UseFile: true } file
                        && file.FilePath == name);
                return match.Key is not null ? match.Value : null;
            }

            var replacementTextureFlags = new TMUnlimiter.ReplacementTextureFlags
            {
                SpecularInstanceIndex = GetReplacementTextureInstanceIndex("Specular"),
                NormalInstanceIndex = GetReplacementTextureInstanceIndex("Normal"),
                WhiteInstanceIndex = GetReplacementTextureInstanceIndex("White"),
                BlackInstanceIndex = GetReplacementTextureInstanceIndex("Black")
            };

            w.WriteWritable(replacementTextureFlags);
            w.WriteListWritable(n.TMUnlimiterData?.EmbeddedImages ?? []);

            ms.WriteTo(w.BaseStream);

            w.WriteList(n.TMUnlimiterData?.VehicleIdentifiers ?? []);

            WriteBlocks(n, n.TMUnlimiterData!, w, embeddedBlockDatas.ToList(), triggerGroups, blockGroups ?? []);
        }

        private static void ReadBlocks(
            CGameCtnChallenge n, 
            TMUnlimiter tmUnlimiter, 
            GbxReader r, 
            List<TMUnlimiter.EmbeddedBlockData> embeddedBlockData,
            List<TMUnlimiter.TriggerGroup> allTriggerGroups,
            List<TMUnlimiter.BlockGroup> allBlockGroups)
        {
            tmUnlimiter.FakeBlocks = n.blocks;

            var blockCount = r.ReadInt32();
            n.blocks = [];

            for (var i = 0; i < blockCount; i++)
            {
                var blockType = r.ReadByte();

                var block = default(CGameCtnBlock);
                var tmUnlimiterBlock = default(TMUnlimiter.Block);

                switch (blockType)
                {
                    case 0: // game block
                        n.blocks.Add(ReadGameBlock(r, byteCoord: false));
                        break;
                    case 1: // trigger block
                        var triggerBlock = new TMUnlimiter.TriggerBlock
                        {
                            Coord = r.ReadInt3(),
                            Direction = (Direction)r.ReadByte(),
                            TriggerGroups = r.ReadArray<int>()
                                .Select(index => allTriggerGroups[index])
                                .ToList(),
                        };
                        tmUnlimiter.TriggerBlocks.Add(triggerBlock);
                        tmUnlimiterBlock = triggerBlock;
                        break;
                    case 2: // external block
                        var externalBlock = new TMUnlimiter.ExternalBlock(
                            Name: r.ReadIdAsString(), 
                            Author: r.ReadIdAsString())
                        {
                            Coord = r.ReadInt3(),
                            Direction = (Direction)r.ReadByte(),
                            Flags = r.ReadInt32()
                        };
                        tmUnlimiter.ExternalBlocks.Add(externalBlock);
                        tmUnlimiterBlock = externalBlock;
                        break;

                    case 3: // embedded block
                        var embeddedBlock = new TMUnlimiter.EmbeddedBlock(
                            Data: embeddedBlockData[r.ReadInt32()])
                        {
                            Coord = r.ReadInt3(),
                            Direction = (Direction)r.ReadByte(),
                            Flags = r.ReadInt32()
                        };
                        tmUnlimiter.EmbeddedBlocks.Add(embeddedBlock);
                        tmUnlimiterBlock = embeddedBlock;
                        break;
                    default:
                        throw new InvalidDataException($"Unknown block type {blockType}.");
                }

                switch (blockType)
                {
                    case 0:
                    case 2:
                    case 3:
                        {
                            var flags2 = r.ReadUInt16();

                            ReadBlockPositionalProperties(r, block, tmUnlimiterBlock, flags2);

                            if ((flags2 & (1 << 4)) != 0) // isOriginOffsetApplied  
                            {
                                var originOffset = r.ReadVec3();
                                block?.TMUnlimiterData?.OriginOffset = originOffset;
                                tmUnlimiterBlock?.OriginOffset = originOffset;
                            }

                            if ((flags2 & (1 << 5)) != 0) // isInBlockGroups  
                            {
                                var mappedBlockGroups = r.ReadArray<int>()
                                    .Select(index => allBlockGroups[index])
                                    .ToList();
                                block?.TMUnlimiterData?.BlockGroups = mappedBlockGroups;
                                tmUnlimiterBlock?.BlockGroups = mappedBlockGroups;
                            }

                            var isDynamic = (flags2 & (1 << 6)) != 0;
                            block?.TMUnlimiterData?.IsDynamic = isDynamic;
                            tmUnlimiterBlock?.IsDynamic = isDynamic;

                            var isInvisible = (flags2 & (1 << 7)) != 0;
                            block?.TMUnlimiterData?.IsInvisible = isInvisible;
                            tmUnlimiterBlock?.IsInvisible = isInvisible;

                            var isNonCollidable = (flags2 & (1 << 8)) != 0;
                            block?.TMUnlimiterData?.IsNonCollidable = isNonCollidable;
                            tmUnlimiterBlock?.IsNonCollidable = isNonCollidable;

                            var spawnPointAlterMethod = (TMUnlimiter.ESpawnPointAlterMethod)((flags2 >> 9) & 3);
                            block?.TMUnlimiterData?.SpawnPointAlterMethod = spawnPointAlterMethod;
                            tmUnlimiterBlock?.SpawnPointAlterMethod = spawnPointAlterMethod;

                            if (spawnPointAlterMethod == TMUnlimiter.ESpawnPointAlterMethod.Manual)
                            {
                                var spawnOffset = r.ReadVec3();
                                var spawnRotation = r.ReadVec3();

                                block?.TMUnlimiterData?.SpawnOffset = spawnOffset;
                                block?.TMUnlimiterData?.SpawnRotation = spawnRotation;
                                tmUnlimiterBlock?.SpawnOffset = spawnOffset;
                                tmUnlimiterBlock?.SpawnRotation = spawnRotation;
                            }

                            var respawnCapability = (TMUnlimiter.ERespawnCapability)((flags2 >> 11) & 3);
                            block?.TMUnlimiterData?.RespawnCapability = respawnCapability;
                            tmUnlimiterBlock?.RespawnCapability = respawnCapability;

                            var blockTriggerActivationMethod = (TMUnlimiter.EBlockTriggerActivationMethod)((flags2 >> 13) & 3);
                            block?.TMUnlimiterData?.BlockTriggerActivationMethod = blockTriggerActivationMethod;
                            tmUnlimiterBlock?.BlockTriggerActivationMethod = blockTriggerActivationMethod;

                            // throw if any flags outside the enum are set
                            if ((flags2 & ~(
                                  (1 << 0) // isOffsetApplied
                                | (1 << 1) // isRotationApplied
                                | (1 << 2) // isScaleApplied
                                | (1 << 3) // isMotionApplied
                                | (1 << 4) // isOriginOffsetApplied
                                | (1 << 5) // isInBlockGroups
                                | (1 << 6) // isDynamic
                                | (1 << 7) // isInvisible
                                | (1 << 8) // isNonCollidable
                                | (3 << 9) // spawnPointAlterMethod
                                | (3 << 11) // respawnCapability
                                | (3 << 13))) != 0)
                            {
                                throw new InvalidDataException($"Unknown block flags2 set: {flags2 & 0xC11F}");
                            }

                            break;
                        }

                    case 1: // trigger block
                        {
                            var flags2 = r.ReadByte();

                            ReadBlockPositionalProperties(r, block, tmUnlimiterBlock, flags2);

                            if ((flags2 & (1 << 4)) != 0) // isInBlockGroups
                            {
                                var mappedBlockGroups = r.ReadArray<int>()
                                    .Select(index => allBlockGroups[index])
                                    .ToList();
                                tmUnlimiterBlock?.BlockGroups = mappedBlockGroups;
                            }

                            tmUnlimiterBlock?.IsNonCollidable = (flags2 & (1 << 5)) != 0;

                            // throw if any flags outside the enum are set
                            if ((flags2 & ~(
                                  (1 << 0) // isOffsetApplied
                                | (1 << 1) // isRotationApplied
                                | (1 << 2) // isScaleApplied
                                | (1 << 3) // isMotionApplied
                                | (1 << 4) // isInBlockGroups
                                | (1 << 5))) != 0)
                            {
                                throw new InvalidDataException($"Unknown trigger block flags2 set: {flags2 & 0xE0}");
                            }

                            break;
                        }
                    default:
                        throw new InvalidDataException($"Unknown block type {blockType}.");
                }
            }
        }

        private static CGameCtnBlock ReadGameBlock(GbxReader r, bool byteCoord)
        {
            var block = new CGameCtnBlock
            {
                TMUnlimiterData = new(),
                BlockModel = (r.ReadIdAsString(), r.ReadId(), ""),
                Coord = byteCoord ? r.ReadByte3() : r.ReadInt3(),
                Direction = (Direction)r.ReadByte(),
                Flags = r.ReadInt32()
            };

            if ((block.Flags & (1 << 15)) != 0) // hasAuthorAndSkin
            {
                block.Author = r.ReadIdAsString();
                block.Skin = r.ReadNodeRef<CGameCtnBlockSkin>();
            }

            return block;
        }

        private static void ReadBlockPositionalProperties(GbxReader r, CGameCtnBlock? block, TMUnlimiter.Block? tmUnlimiterBlock, ushort flags)
        {
            if ((flags & 1) != 0) // isOffsetApplied  
            {
                var offset = r.ReadVec3();
                block?.TMUnlimiterData?.Offset = offset;
                tmUnlimiterBlock?.Offset = offset;
            }

            if ((flags & (1 << 1)) != 0) // isRotationApplied  
            {
                var rotation = r.ReadVec3();
                block?.TMUnlimiterData?.Rotation = rotation;
                tmUnlimiterBlock?.Rotation = rotation;
            }

            if ((flags & (1 << 2)) != 0) // isScaleApplied  
            {
                var scale = r.ReadVec3();
                block?.TMUnlimiterData?.Scale = scale;
                tmUnlimiterBlock?.Scale = scale;
            }

            if ((flags & (1 << 3)) != 0) // isMotionApplied  
            {
                var flags3 = r.ReadByte();

                var motion = new TMUnlimiter.Motion
                {
                    WaveType = flags3 & 7
                };

                if ((flags3 & (1 << 3)) == 0) // not isManuallyControlled  
                {
                    motion.Points = r.ReadListReadable<TMUnlimiter.MotionPoint>();
                }

                block?.TMUnlimiterData?.Motion = motion;
                tmUnlimiterBlock?.Motion = motion;
            }
        }

        private static void WriteBlocks(
            CGameCtnChallenge n,
            TMUnlimiter tmUnlimiter,
            GbxWriter w,
            List<TMUnlimiter.EmbeddedBlockData> embeddedBlockData,
            List<TMUnlimiter.TriggerGroup> allTriggerGroups,
            List<TMUnlimiter.BlockGroup> allBlockGroups)
        {
            var blocks = n.blocks ?? throw new InvalidOperationException("Blocks are null.");

            w.Write(blocks.Count + tmUnlimiter.TriggerBlocks.Count + tmUnlimiter.ExternalBlocks.Count + tmUnlimiter.EmbeddedBlocks.Count);

            foreach (var block in blocks)
            {
                w.Write((byte)0); // game block
                WriteGameBlock(w, block, byteCoord: false);

                WriteBlock20Properties(w, block, tmUnlimiterBlock: null, allBlockGroups);
            }

            foreach (var triggerBlock in tmUnlimiter.TriggerBlocks)
            {
                w.Write((byte)1); // trigger block
                w.Write(triggerBlock.Coord);
                w.Write((byte)triggerBlock.Direction);
                w.WriteArray(triggerBlock.TriggerGroups.Select(g => allTriggerGroups.IndexOf(g)).ToArray());

                WriteTriggerBlock20Properties(w, triggerBlock, allBlockGroups);
            }

            foreach (var externalBlock in tmUnlimiter.ExternalBlocks)
            {
                w.Write((byte)2); // external block
                w.Write(externalBlock.Name);
                w.Write(externalBlock.Author);
                w.Write(externalBlock.Coord);
                w.Write((byte)externalBlock.Direction);
                w.Write(externalBlock.Flags);

                WriteBlock20Properties(w, block: null, externalBlock, allBlockGroups);
            }

            foreach (var embeddedBlock in tmUnlimiter.EmbeddedBlocks)
            {
                w.Write((byte)3); // embedded block
                w.Write(embeddedBlockData.IndexOf(embeddedBlock.Data));
                w.Write(embeddedBlock.Coord);
                w.Write((byte)embeddedBlock.Direction);
                w.Write(embeddedBlock.Flags);

                WriteBlock20Properties(w, block: null, embeddedBlock, allBlockGroups);
            }
        }

        private static void WriteGameBlock(GbxWriter w, CGameCtnBlock block, bool byteCoord)
        {
            w.WriteIdAsString(block.BlockModel.Id);
            w.Write(block.BlockModel.Collection);

            if (byteCoord)
            {
                w.Write((Byte3)block.Coord);
            }
            else
            {
                w.Write(block.Coord);
            }

            w.Write((byte)block.Direction);
            w.Write(block.Flags);

            if ((block.Flags & (1 << 15)) != 0) // hasAuthorAndSkin
            {
                w.WriteIdAsString(block.Author ?? "");
                w.WriteNodeRef(block.Skin);
            }
        }

        private static void WriteBlock20Properties(GbxWriter w, CGameCtnBlock? block, TMUnlimiter.Block? tmUnlimiterBlock, List<TMUnlimiter.BlockGroup> allBlockGroups)
        {
            var offset = (block?.TMUnlimiterData?.Offset ?? tmUnlimiterBlock?.Offset).GetValueOrDefault();
            var rotation = (block?.TMUnlimiterData?.Rotation ?? tmUnlimiterBlock?.Rotation).GetValueOrDefault();
            var scale = (block?.TMUnlimiterData?.Scale ?? tmUnlimiterBlock?.Scale).GetValueOrDefault(Vec3.One);
            var motion = block?.TMUnlimiterData?.Motion ?? tmUnlimiterBlock?.Motion;
            var originOffset = block?.TMUnlimiterData?.OriginOffset ?? tmUnlimiterBlock?.OriginOffset;
            var blockGroups = block?.TMUnlimiterData?.BlockGroups ?? tmUnlimiterBlock?.BlockGroups;
            var isDynamic = block?.TMUnlimiterData?.IsDynamic ?? tmUnlimiterBlock?.IsDynamic ?? false;
            var isInvisible = block?.TMUnlimiterData?.IsInvisible ?? tmUnlimiterBlock?.IsInvisible ?? false;
            var isNonCollidable = block?.TMUnlimiterData?.IsNonCollidable ?? tmUnlimiterBlock?.IsNonCollidable ?? false;
            var spawnPointAlterMethod = block?.TMUnlimiterData?.SpawnPointAlterMethod ?? tmUnlimiterBlock?.SpawnPointAlterMethod ?? TMUnlimiter.ESpawnPointAlterMethod.None;
            var spawnOffset = block?.TMUnlimiterData?.SpawnOffset ?? tmUnlimiterBlock?.SpawnOffset;
            var spawnRotation = block?.TMUnlimiterData?.SpawnRotation ?? tmUnlimiterBlock?.SpawnRotation;
            var respawnCapability = block?.TMUnlimiterData?.RespawnCapability ?? tmUnlimiterBlock?.RespawnCapability ?? TMUnlimiter.ERespawnCapability.Default;
            var blockTriggerActivationMethod = block?.TMUnlimiterData?.BlockTriggerActivationMethod ?? tmUnlimiterBlock?.BlockTriggerActivationMethod ?? TMUnlimiter.EBlockTriggerActivationMethod.Default;

            var flags = (ushort)0;
            if (offset != Vec3.Zero) flags |= 1;
            if (rotation != Vec3.Zero) flags |= 1 << 1;
            if (scale != Vec3.One) flags |= 1 << 2;
            if (motion is not null) flags |= 1 << 3;
            if (originOffset is not null && originOffset != Vec3.Zero) flags |= 1 << 4;
            if (blockGroups?.Count > 0) flags |= 1 << 5;
            if (isDynamic) flags |= 1 << 6;
            if (isInvisible) flags |= 1 << 7;
            if (isNonCollidable) flags |= 1 << 8;
            flags |= (ushort)(((ushort)spawnPointAlterMethod & 3) << 9);
            flags |= (ushort)(((ushort)respawnCapability & 3) << 11);
            flags |= (ushort)(((ushort)blockTriggerActivationMethod & 3) << 13);

            w.Write(flags);

            if (offset != Vec3.Zero) w.Write(offset);
            if (rotation != Vec3.Zero) w.Write(rotation);
            if (scale != Vec3.One) w.Write(scale);

            if (motion is not null)
            {
                var flags3 = (byte)motion.WaveType;
                if (motion.Points is null) flags3 |= 1 << 3; // isManuallyControlled
                w.Write(flags3);
                if (motion.Points is not null)
                {
                    w.WriteListWritable(motion.Points);
                }
            }

            if (originOffset is not null && originOffset != Vec3.Zero) w.Write(originOffset.GetValueOrDefault());

            if (blockGroups?.Count > 0)
            {
                w.WriteArray(blockGroups.Select(g => allBlockGroups.IndexOf(g)).ToArray());
            }

            if (spawnPointAlterMethod == TMUnlimiter.ESpawnPointAlterMethod.Manual)
            {
                w.Write(spawnOffset.GetValueOrDefault());
                w.Write(spawnRotation.GetValueOrDefault());
            }
        }

        private static void WriteTriggerBlock20Properties(GbxWriter w, TMUnlimiter.TriggerBlock triggerBlock, List<TMUnlimiter.BlockGroup> allBlockGroups)
        {
            var offset = triggerBlock.Offset;
            var rotation = triggerBlock.Rotation;
            var scale = triggerBlock.Scale;
            var motion = triggerBlock.Motion;
            var blockGroups = triggerBlock.BlockGroups;

            var flags = (byte)0;
            if (offset != Vec3.Zero) flags |= 1;
            if (rotation != Vec3.Zero) flags |= 1 << 1;
            if (scale != Vec3.One) flags |= 1 << 2;
            if (motion is not null) flags |= 1 << 3;
            if (blockGroups?.Count > 0) flags |= 1 << 4;
            if (triggerBlock.IsNonCollidable) flags |= 1 << 5;

            w.Write(flags);

            if (offset != Vec3.Zero) w.Write(offset);
            if (rotation != Vec3.Zero) w.Write(rotation);
            if (scale != Vec3.One) w.Write(scale);

            if (motion is not null)
            {
                var flags3 = (byte)motion.WaveType;
                if (motion.Points is null) flags3 |= 1 << 3; // isManuallyControlled
                w.Write(flags3);
                if (motion.Points is not null)
                {
                    w.WriteListWritable(motion.Points);
                }
            }

            if (blockGroups?.Count > 0)
            {
                w.WriteArray(blockGroups.Select(g => allBlockGroups.IndexOf(g)).ToArray());
            }
        }
    }

    public partial class Chunk3F001002
    {
        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw) => ReadWrite(n, rw, ver: 1);
    }

    public partial class Chunk3F001003 : IVersionable
    {
        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw) => ReadWrite(n, rw, ver: 2);
    }

    public sealed class TMUnlimiter
    {
        public Vec3 DecorationOffset { get; set; }
        public Vec3 DecorationScale { get; set; } = Vec3.One;
        public DecorationVisibility SkyDecorationVisibility { get; set; }
        public bool IsDecorationMoved => DecorationOffset != Vec3.Zero;
        public bool IsDecorationScaled => DecorationScale != Vec3.One;
        public bool IsTrackBaseEmpty { get; set; }
        public bool IsVanillaMode { get; set; }
        public bool IsPylonsDisabled { get; set; }
        public bool EnableVehicleCollisions { get; set; }
        public bool EnableRandomStartLine { get; set; }
        public bool EnableServerCommunication { get; set; }
        public bool IgnoreMultiplayerTimeSyncForMotionBlocks { get; set; }

        public List<ParameterSet> ParameterSets { get; set; } = [];

        public List<AngelScriptModule> AngelScriptModules { get; set; } = [];
        public List<EmbeddedBlock> EmbeddedBlocks { get; set; } = [];
        public List<EmbeddedImage> EmbeddedImages { get; set; } = [];
        public List<Ident> VehicleIdentifiers { get; set; } = [];
        public List<TriggerBlock> TriggerBlocks { get; set; } = [];
        public List<ExternalBlock> ExternalBlocks { get; set; } = [];
        public List<CGameCtnBlock>? FakeBlocks { get; set; }

        public enum DecorationVisibility
        {
            Everything,
            SkyOnly,
            Background,
            Nothing,
            Warp = 1 << 8
        }

        public record LegacyScript : IReadableWritable
        {
            public string Name { get; set; } = "";
            public byte[] ByteCode { get; set; } = [];

            public void ReadWrite(GbxReaderWriter rw, int v = 0)
            {
                Name = rw.String(Name);
                ByteCode = rw.Data(ByteCode);
            }
        }

        public record ParameterSet : IReadable, IWritable
        {
            public string Name { get; set; } = "";
            public List<Parameter> Parameters { get; set; } = [];

            public virtual void Read(GbxReader r, int v = 0)
            {
                Name = r.ReadString();

                var count = r.ReadInt32();
                Parameters = new List<Parameter>(count);

                for (var i = 0; i < count; i++)
                {
                    var function = (ParameterName)r.ReadInt32();
#pragma warning disable IDE0066
                    Parameter parameter;
                    switch (function)
                    {
                        // float
                        case ParameterName.Vehicle_Scale:
                        case ParameterName.Vehicle_AddLinearSpeedX:
                        case ParameterName.Vehicle_AddLinearSpeedY:
                        case ParameterName.Vehicle_AddLinearSpeedZ:
                        case ParameterName.Vehicle_Mass:
                        case ParameterName.Vehicle_GravityGround:
                        case ParameterName.Vehicle_GravityAir:
                        case ParameterName.Vehicle_MaxSpeedForward:
                        case ParameterName.Vehicle_MaxSpeedBackward:
                        case ParameterName.Vehicle_SpeedClamp:
                        case ParameterName.Vehicle_YellowBoostMultiplier:
                        case ParameterName.Vehicle_RedBoostMultiplier:
                        case ParameterName.Vehicle_YellowBoostDuration:
                        case ParameterName.Vehicle_RedBoostDuration:
                        case ParameterName.Vehicle_BrakeBase:
                        case ParameterName.Vehicle_BrakeCoef:
                        case ParameterName.Vehicle_BrakeMax:
                        case ParameterName.Vehicle_BrakeMaxDynamic:
                        case ParameterName.Vehicle_GroundSlowDownBaseValue:
                        case ParameterName.Vehicle_GroundSlowDownMultiplier:
                        case ParameterName.Vehicle_LimitToMaxSpeedForce:
                        case ParameterName.Vehicle_SlopeSpeedGainLimit:
                        case ParameterName.Vehicle_SteerRadiusMin:
                        case ParameterName.Vehicle_SteerRadiusCoef:
                        case ParameterName.Vehicle_SteerLowSpeed:
                        case ParameterName.Vehicle_SteerSlowDownCoef:
                        case ParameterName.Vehicle_SteerMaxBlend:
                        case ParameterName.Vehicle_SideFriction1:
                        case ParameterName.Vehicle_SideFriction2:
                        case ParameterName.Vehicle_MaxSideFrictionSliding:
                        case ParameterName.Vehicle_MaxSideFrictionBlendCoef:
                        case ParameterName.Vehicle_RolloverAxial:
                        case ParameterName.Vehicle_SteerGroundTorque:
                        case ParameterName.Vehicle_SteerGroundTorqueSlippingCoef:
                        case ParameterName.Vehicle_LateralSlopeAdherenceMin:
                        case ParameterName.Vehicle_LateralSlopeAdherenceMax:
                        case ParameterName.Vehicle_AxialSlopeAdherenceMin:
                        case ParameterName.Vehicle_AxialSlopeAdherenceMax:
                        case ParameterName.Vehicle_AngularSpeedYImpulseBlend:
                        case ParameterName.Vehicle_AngularSpeedImpulseScale:
                        case ParameterName.Vehicle_SteerSpeed:
                        case ParameterName.Vehicle_SteerAngleMax:
                        case ParameterName.Vehicle_SlipAngleForceMax:
                        case ParameterName.Vehicle_SlipAngleForceCoef1:
                        case ParameterName.Vehicle_SlipAngleForceCoef2:
                        case ParameterName.Vehicle_Field0x18:
                        case ParameterName.Vehicle_Field0x1c:
                        case ParameterName.Vehicle_Field0x20:
                        case ParameterName.Vehicle_Field0x28:
                        case ParameterName.Vehicle_Field0x38:
                        case ParameterName.Vehicle_Field0x3c:
                        case ParameterName.Vehicle_Field0x50:
                        case ParameterName.Vehicle_Field0x54:
                        case ParameterName.Vehicle_InertiaMass:
                        case ParameterName.Vehicle_InertiaHalfDiagX:
                        case ParameterName.Vehicle_InertiaHalfDiagY:
                        case ParameterName.Vehicle_InertiaHalfDiagZ:
                        case ParameterName.Vehicle_LinearFluidFrictionMultiplier:
                        case ParameterName.Vehicle_AngularFluidFrictionFirstMultiplier:
                        case ParameterName.Vehicle_AngularFluidFrictionSecondMultiplier:
                        case ParameterName.Vehicle_BodyFrictionWithConcreteMultiplier:
                        case ParameterName.Vehicle_BodyFrictionWithMetalMultiplier:
                        case ParameterName.Vehicle_BodyRestCoefMetal:
                        case ParameterName.Vehicle_BodyRestCoefConcrete:
                        case ParameterName.Vehicle_WheelFrictionCoefConcrete:
                        case ParameterName.Vehicle_WheelRestCoefConcrete:
                        case ParameterName.Vehicle_WheelFrictionCoefMetal:
                        case ParameterName.Vehicle_WheelRestCoefMetal:
                        case ParameterName.Vehicle_AbsorbingValKi:
                        case ParameterName.Vehicle_AbsorbingValKa:
                        case ParameterName.Vehicle_AbsorbingValMin:
                        case ParameterName.Vehicle_AbsorbingValMax:
                        case ParameterName.Vehicle_AbsorbingValRest:
                        case ParameterName.Vehicle_CMAftFore:
                        case ParameterName.Vehicle_CMDownUp:
                        case ParameterName.Vehicle_AngularSpeedClamp:
                        case ParameterName.Vehicle_LinearSpeed2PositiveDeltaMax:
                        case ParameterName.Vehicle_RubberBallElasticity:
                        case ParameterName.Vehicle_JumpImpulseVal:
                        case ParameterName.Vehicle_MaxDistPerStep:
                        case ParameterName.Vehicle_AbsorbTension:
                        case ParameterName.Vehicle_TwistAngle:
                        case ParameterName.Vehicle_GlidingGravityCoef:
                        case ParameterName.Vehicle_DebugAbsorbCoef:
                        case ParameterName.Vehicle_RelSpeedMultCoef:
                        case ParameterName.Vehicle_MaxAngularSpeedYAirControl:
                        case ParameterName.Vehicle_VibrationPeriodSpeedCoef:
                        case ParameterName.Vehicle_Field0x384:
                        case ParameterName.Vehicle_Field0x33c:
                        case ParameterName.Vehicle_Field0x340:
                        case ParameterName.Vehicle_Field0x344:
                        case ParameterName.Vehicle_Field0x348:
                        case ParameterName.Vehicle_Field0x34c:
                        case ParameterName.Vehicle_Field0x358:
                        case ParameterName.Vehicle_Field0x35c:
                        case ParameterName.Vehicle_Field0x360:
                        case ParameterName.Vehicle_WaterGravity:
                        case ParameterName.Vehicle_WaterReboundMinHSpeed:
                        case ParameterName.Vehicle_WaterBumpMinSpeed:
                        case ParameterName.Vehicle_WaterAngularFriction:
                        case ParameterName.Vehicle_WaterAngularFrictionSq:
                        case ParameterName.Vehicle_EngineVolume:
                        case ParameterName.Vehicle_EnginePitch:
                        case ParameterName.Vehicle_SoundEngineVolume:
                        case ParameterName.Vehicle_SoundSkidConcreteVolume:
                        case ParameterName.Vehicle_SoundSkidSandVolume:
                        case ParameterName.Vehicle_SoundImpactVolume:
                        case ParameterName.Vehicle_Field0x398:
                        case ParameterName.Vehicle_Field0x39c:
                        case ParameterName.Vehicle_Field0x3a0:
                        case ParameterName.Vehicle_Field0x3a4:
                        case ParameterName.Vehicle_Field0x3a8:
                        case ParameterName.Vehicle_M6InertialMass:
                        case ParameterName.Vehicle_M6MaxDiffBtwnPropulsionAndSpeed:
                        case ParameterName.Vehicle_M6ForceEpsilon:
                        case ParameterName.Vehicle_M6InertialTorqueModulationX:
                        case ParameterName.Vehicle_M6InertialTorqueModulationZ:
                        case ParameterName.Vehicle_M6BrakeModulationWhenSlipping:
                        case ParameterName.Vehicle_M6FrictionModulationWhenSlipNBrake:
                        case ParameterName.Vehicle_M6BrakeMaxRear:
                        case ParameterName.Vehicle_M6BrakeMaxDynamicRear:
                        case ParameterName.Vehicle_M6MinSpeed4Burnout:
                        case ParameterName.Vehicle_M6MaxSpeed4Burnout:
                        case ParameterName.Vehicle_M6BurnoutLateralSpeedCoeff:
                        case ParameterName.Vehicle_M6BurnoutSteerCoeff:
                        case ParameterName.Vehicle_M6BurnoutSteerCoeff2:
                        case ParameterName.Vehicle_M6BurnoutSteerCoeff3:
                        case ParameterName.Vehicle_M6BurnoutSteerCoeff4:
                        case ParameterName.Vehicle_M6BurnoutCenterForceCoeff:
                        case ParameterName.Vehicle_M6BurnoutCenterForceCoeff2:
                        case ParameterName.Vehicle_M6BurnoutRadiusMax:
                        case ParameterName.Vehicle_M6BurnoutLateralSpeedMax:
                        case ParameterName.Vehicle_M6MaxDiffBtwGroundNormal:
                        case ParameterName.Vehicle_M6MaxPosAngle4Burnout:
                        case ParameterName.Vehicle_M6MaxNegAngle4Burnout:
                        case ParameterName.Vehicle_M6BurnoutAccMod:
                        case ParameterName.Vehicle_M6BurnoutFricMod:
                        case ParameterName.Vehicle_M6AfterBurnoutAccMod:
                        case ParameterName.Vehicle_M6BurnoutSmokeIntensity:
                        case ParameterName.Vehicle_M6BurnoutSmokeVelocity:
                        case ParameterName.Vehicle_M6AfterBurnoutImpulse:
                        case ParameterName.Vehicle_M6BurnoutWheelAngularRotation:
                        case ParameterName.Vehicle_M6BrakeSmokeIntensity:
                        case ParameterName.Vehicle_M6MaxRpm:
                        case ParameterName.Vehicle_M6BurnoutRpmAcc:
                        case ParameterName.Vehicle_M6AirRpmAcc:
                        case ParameterName.Vehicle_M6AirRpmDeadening:
                        case ParameterName.Vehicle_M6RpmLossCoefOnGearUp:
                        case ParameterName.Vehicle_M6RpmGainCoefOnGearDown:
                        case ParameterName.Vehicle_M6RpmGainOnTakeOff:
                        case ParameterName.Vehicle_M6RpmLossOnTakeOffFinished:
                        case ParameterName.Vehicle_M6SpeedLimitPositiveForTakeOffFront:
                        case ParameterName.Vehicle_M6SpeedLimitNegForTakeOffFront:
                        case ParameterName.Vehicle_M6SpeedLimitPositiveForTakeOffRear:
                        case ParameterName.Vehicle_M6SpeedLimitNegForTakeOffRear:
                        case ParameterName.Vehicle_M5SlippingAccelCurveCoef:
                        case ParameterName.Vehicle_M5MaxAxialRolloverTorque:
                        case ParameterName.Vehicle_M5AccelSlipCoefMax:
                        case ParameterName.Vehicle_M4SteerTorqueCoef:
                        case ParameterName.Vehicle_M4LateralFrictionTorque:
                        case ParameterName.Vehicle_M4LateralFrictionSquareTorque:
                        case ParameterName.Vehicle_M4LateralFrictionForce:
                        case ParameterName.Vehicle_M4LateralFrictionSquareForce:
                        case ParameterName.Vehicle_M4LeaveSplippingSpeed:
                        case ParameterName.Vehicle_M4SteerRadiusWhenSlippingCoef:
                        case ParameterName.Vehicle_M4MaxFrictionForceWhenSlippingCoef:
                        case ParameterName.Vehicle_M4MaxFrictionTorqueWhenSlippingCoef:
                        case ParameterName.Vehicle_M4SlipAngleSpeed:
                        case ParameterName.Vehicle_Field0x1d8:
                        case ParameterName.Vehicle_M4SteerAngleWhenSlippingMax:
                        case ParameterName.World_DisplayMediaTrackerClip:
                        case ParameterName.World_SetGravityForceX:
                        case ParameterName.World_SetGravityForceY:
                        case ParameterName.World_SetGravityForceZ:
                        case ParameterName.World_SetDayTime:
                        case ParameterName.World_SetDynamicDayTimeFlowSpeed:
                            parameter = new FloatParameter(function);
                            break;

                        // string
                        case ParameterName.Vehicle_Transform:
                        case ParameterName.Vehicle_SetVehicleTuningByName:
                        case ParameterName.World_ExecuteParameterSet:
                        case ParameterName.World_ExecuteScript:
                        case ParameterName.World_BlockGroupMakeVisible:
                        case ParameterName.World_BlockGroupMakeInvisible:
                        case ParameterName.World_BlockGroupMakeCollidable:
                        case ParameterName.World_BlockGroupMakeNonCollidable:
                            parameter = new StringParameter(function);
                            break;

                        // keys
                        case ParameterName.Vehicle_AccelerationCurve:
                        case ParameterName.Vehicle_SteerDriveTorque:
                        case ParameterName.Vehicle_SteerSlowDown:
                        case ParameterName.Vehicle_LateralContactSlowDown:
                        case ParameterName.Vehicle_MaxSideFriction:
                        case ParameterName.Vehicle_RolloverLateral:
                        case ParameterName.Vehicle_RolloverLateralFromAngle:
                        case ParameterName.Vehicle_BrakeHeatSpeedFromFBrake:
                        case ParameterName.Vehicle_VisualSteerAngleFromSpeed:
                        case ParameterName.Vehicle_AirControlZCoefFromAngularSpeed:
                        case ParameterName.Vehicle_WaterSplashFromSpeed:
                        case ParameterName.Vehicle_WaterReboundFromSpeedRatio:
                        case ParameterName.Vehicle_WaterBumpSlowDownFromSpeedRatio:
                        case ParameterName.Vehicle_WaterFrictionFromSpeed:
                        case ParameterName.Vehicle_ModulationFromWheelCompression:
                        case ParameterName.Vehicle_AccelCurveRearGear:
                        case ParameterName.Vehicle_M6RolloverLateralFromSpeedRatio:
                        case ParameterName.Vehicle_M6BurnoutRadius:
                        case ParameterName.Vehicle_M6BurnoutLateralSpeed:
                        case ParameterName.Vehicle_M6DonutRolloverFromSpeed:
                        case ParameterName.Vehicle_M6BurnoutRolloverFromSpeed:
                        case ParameterName.Vehicle_M5SlippingAccelCurve:
                        case ParameterName.Vehicle_M5SteerCoefFromSpeed:
                        case ParameterName.Vehicle_M5SmoothInputSteerDurationFromSpeed:
                        case ParameterName.Vehicle_M4SteerRadiusFromSpeed:
                        case ParameterName.Vehicle_M4MaxFrictionForceFromSpeed:
                        case ParameterName.Vehicle_M4MaxFrictionTorqueFromSpeed:
                        case ParameterName.Vehicle_M4SteerRadiusCoefFromSlipAngle:
                        case ParameterName.Vehicle_M4AccelFromSlipAngle:
                            parameter = new KeysRealParameter(function);
                            break;

                        // int
                        case ParameterName.Vehicle_SetVehicleTuningByIndex:
                        case ParameterName.Vehicle_SteerModel:
                        case ParameterName.Vehicle_ShockModel:
                        case ParameterName.Vehicle_M5KeepNoSteerSlowDownWhenSlippingDuration:
                        case ParameterName.Vehicle_SteerSlowDownFadeInDuration:
                        case ParameterName.Vehicle_SteerSlowDownFadeOutDuration:
                        case ParameterName.Vehicle_SteerDurationBeforeSteerSlowDown:
                        case ParameterName.Vehicle_TireMaterial:
                        case ParameterName.Vehicle_GearCount:
                        case ParameterName.Vehicle_MinGear:
                        case ParameterName.Vehicle_AirControlDuration:
                        case ParameterName.Vehicle_M6BurnoutDuration:
                        case ParameterName.Vehicle_M6AfterBurnoutDuration:
                        case ParameterName.Vehicle_M5LateralContactSlowDownDuration:
                        case ParameterName.Vehicle_M5KeepSlidingAccelDurarion:
                        case ParameterName.Vehicle_M5KeepSteerSlowDownDurarion:
                            parameter = new NaturalParameter(function);
                            break;

                        // float[]
                        case ParameterName.Vehicle_M6GearRatio:
                        case ParameterName.Vehicle_M6MaxRPM:
                        case ParameterName.Vehicle_M6MinRPM:
                        case ParameterName.Vehicle_M6RpmWantedOnGearUp:
                        case ParameterName.Vehicle_M6RpmDelta:
                        case ParameterName.Vehicle_M6RpmComputedOnGearDown:
                            parameter = new FastBufferRealParameter(function);
                            break;

                        // bool
                        case ParameterName.Vehicle_NoSteerSlowDownWhenSlipping:
                        case ParameterName.Vehicle_IsFakeEngine:
                            parameter = new BoolParameter(function);
                            break;

                        default:
                            parameter = new Parameter(function);
                            break;
                    }
#pragma warning restore IDE0066

                    parameter.Read(r, v);
                    Parameters.Add(parameter);
                }
            }

            public virtual void Write(GbxWriter w, int v = 0)
            {
                w.Write(Name);
                w.Write(Parameters.Count);

                foreach (var parameter in Parameters)
                {
                    w.Write((int)parameter.Function);
                    parameter.Write(w, v);
                }
            }
        }

        public record Parameter : IReadable, IWritable
        {
            public ParameterName Function { get; }
            public ParameterOperation ParameterOperation { get; set; }

            public Parameter(ParameterName function)
            {
                Function = function;
            }

            public virtual void Read(GbxReader r, int v = 0)
            {
                ParameterOperation = (ParameterOperation)r.ReadByte();
            }

            public virtual void Write(GbxWriter w, int v = 0)
            {
                w.Write((byte)ParameterOperation);
            }
        }

        public record FloatParameter : Parameter
        {
            public float Value { get; set; }

            public FloatParameter(ParameterName function) : base(function) { }

            public override void Read(GbxReader r, int v = 0)
            {
                base.Read(r, v);
                Value = r.ReadSingle();
            }

            public override void Write(GbxWriter w, int v = 0)
            {
                base.Write(w, v);
                w.Write(Value);
            }
        }

        public record StringParameter : Parameter
        {
            public string Value { get; set; } = string.Empty;

            public StringParameter(ParameterName function) : base(function) { }

            public override void Read(GbxReader r, int v = 0)
            {
                base.Read(r, v);
                Value = r.ReadString();
            }

            public override void Write(GbxWriter w, int v = 0)
            {
                base.Write(w, v);
                w.Write(Value);
            }
        }

        public record KeysRealParameter : Parameter
        {
            public float? MultiplyValue { get; set; }
            public CFuncKeysReal? Value { get; set; }

            public KeysRealParameter(ParameterName function) : base(function) { }

            public override void Read(GbxReader r, int v = 0)
            {
                base.Read(r, v);

                if (ParameterOperation == ParameterOperation.Multiply)
                {
                    MultiplyValue = r.ReadSingle();
                    return;
                }

                Value = r.ReadNode<CFuncKeysReal>();
            }

            public override void Write(GbxWriter w, int v = 0)
            {
                base.Write(w, v);

                if (ParameterOperation == ParameterOperation.Multiply)
                {
                    w.Write(MultiplyValue ?? 0f);
                }
                else
                {
                    w.WriteNode(Value);
                }
            }
        }

        public record NaturalParameter : Parameter
        {
            public uint? Value { get; set; }

            public NaturalParameter(ParameterName function) : base(function) { }

            public override void Read(GbxReader r, int v = 0)
            {
                base.Read(r, v);

                if (ParameterOperation == ParameterOperation.Multiply)
                {
                    Value = (uint)r.ReadSingle();
                }
                else
                {
                    Value = r.ReadUInt32();
                }
            }

            public override void Write(GbxWriter w, int v = 0)
            {
                base.Write(w, v);

                if (ParameterOperation == ParameterOperation.Multiply)
                {
                    w.Write((float)(Value ?? 0));
                }
                else
                {
                    w.Write(Value ?? 0u);
                }
            }
        }

        public record FastBufferRealParameter : Parameter
        {
            public float? MultiplyValue { get; set; }
            public float[]? Value { get; set; }

            public FastBufferRealParameter(ParameterName function) : base(function) { }

            public override void Read(GbxReader r, int v = 0)
            {
                base.Read(r, v);

                if (ParameterOperation == ParameterOperation.Multiply)
                {
                    Value = [r.ReadSingle()];
                }
                else
                {
                    Value = r.ReadArray<float>();
                }
            }

            public override void Write(GbxWriter w, int v = 0)
            {
                base.Write(w, v);

                if (ParameterOperation == ParameterOperation.Multiply)
                {
                    w.Write(MultiplyValue ?? 0f);
                }
                else
                {
                    w.WriteArray(Value ?? []);
                }
            }
        }

        public record BoolParameter : Parameter
        {
            public bool Value { get; set; }

            public BoolParameter(ParameterName function) : base(function) { }

            public override void Read(GbxReader r, int v = 0)
            {
                base.Read(r, v);
                Value = r.ReadBoolean(asByte: true);
            }

            public override void Write(GbxWriter w, int v = 0)
            {
                base.Write(w, v);
                w.Write(Value, asByte: true);
            }
        }

        public class EmbeddedBlockData : IReadable, IWritable
        {
            public string Id { get; set; } = string.Empty;
            public string Author { get; set; } = string.Empty;
            public uint U01 { get; set; }
            public byte Flags { get; set; }

            public byte BlockType
            {
                get => (byte)(Flags & 0b111);
                set => Flags = (byte)((Flags & ~0b111) | (value & 0b111));
            }

            public CGameItemModel.EWaypointType WaypointType
            {
                get => (CGameItemModel.EWaypointType)((Flags >> 3) & 0b111);
                set => Flags = (byte)((Flags & ~(0b111 << 3)) | (((byte)value & 0b111) << 3));
            }

            public byte? IconWidth { get; set; }
            public byte? IconHeight { get; set; }
            public byte[]? IconData { get; set; }
            public List<SubVariation> GroundSubVariations0 { get; set; } = [];
            public List<SubVariation> AirSubVariations0 { get; set; } = [];
            public List<SubVariation> GroundSubVariations1 { get; set; } = [];
            public List<SubVariation> AirSubVariations1 { get; set; } = [];
            public List<SubVariation> GroundSubVariations2 { get; set; } = [];
            public List<SubVariation> AirSubVariations2 { get; set; } = [];
            public List<SubVariation> GroundSubVariations3 { get; set; } = [];
            public List<SubVariation> AirSubVariations3 { get; set; } = [];
            public List<SubVariation> GroundSubVariations4 { get; set; } = [];
            public List<SubVariation> AirSubVariations4 { get; set; } = [];
            public List<SubVariation> GroundSubVariations5 { get; set; } = [];
            public List<SubVariation> AirSubVariations5 { get; set; } = [];
            public List<Int3>? GroundBlockUnitInfos { get; set; }
            public List<Int3>? AirBlockUnitInfos { get; set; }
            public Vec3 SpawnOffsetGround { get; set; }
            public Vec3 SpawnRotationGround { get; set; }
            public Vec3 SpawnOffsetAir { get; set; }
            public Vec3 SpawnRotationAir { get; set; }

            public override string ToString()
            {
                return $"{Id} by {Author} (Type: {WaypointType}, BlockType: {BlockType})";
            }

            public void Read(GbxReader r, int v = 0)
            {
                Id = r.ReadIdAsString();
                Author = r.ReadIdAsString();

                if (v == 0)
                {
                    U01 = r.ReadUInt32();
                }
                else
                {
                    Flags = r.ReadByte();

                    if ((Flags & 0b01000000) != 0) // has icon
                    {
                        IconWidth = r.ReadByte();
                        IconHeight = r.ReadByte();
                        IconData = r.ReadBytes(IconWidth.GetValueOrDefault() * IconHeight.GetValueOrDefault() * 4);
                    }
                }

                if (v == 0)
                {
                    WaypointType = (CGameItemModel.EWaypointType)r.ReadByte();
                    BlockType = r.ReadByte();
                }

                var packedVersion = (int)WaypointType | (v << 4);

                if (BlockType is 2 or 3) // classic or road
                {
                    GroundSubVariations0 = r.ReadListReadable<SubVariation>(byteLengthPrefix: true, packedVersion);
                    AirSubVariations0 = r.ReadListReadable<SubVariation>(byteLengthPrefix: true, packedVersion);
                }

                if (BlockType == 3) // road
                {
                    GroundSubVariations1 = r.ReadListReadable<SubVariation>(byteLengthPrefix: true, packedVersion);
                    AirSubVariations1 = r.ReadListReadable<SubVariation>(byteLengthPrefix: true, packedVersion);
                    GroundSubVariations2 = r.ReadListReadable<SubVariation>(byteLengthPrefix: true, packedVersion);
                    AirSubVariations2 = r.ReadListReadable<SubVariation>(byteLengthPrefix: true, packedVersion);
                    GroundSubVariations3 = r.ReadListReadable<SubVariation>(byteLengthPrefix: true, packedVersion);
                    AirSubVariations3 = r.ReadListReadable<SubVariation>(byteLengthPrefix: true, packedVersion);
                    GroundSubVariations4 = r.ReadListReadable<SubVariation>(byteLengthPrefix: true, packedVersion);
                    AirSubVariations4 = r.ReadListReadable<SubVariation>(byteLengthPrefix: true, packedVersion);
                    GroundSubVariations5 = r.ReadListReadable<SubVariation>(byteLengthPrefix: true, packedVersion);
                    AirSubVariations5 = r.ReadListReadable<SubVariation>(byteLengthPrefix: true, packedVersion);
                }

                SpawnOffsetGround = r.ReadVec3();
                SpawnRotationGround = r.ReadVec3();
                SpawnOffsetAir = r.ReadVec3();
                SpawnRotationAir = r.ReadVec3();

                if (v >= 1)
                {
                    var groundBlockUnitInfosCount = r.ReadInt32();
                    var airBlockUnitInfosCount = r.ReadInt32();

                    GroundBlockUnitInfos = r.ReadList<Int3>(groundBlockUnitInfosCount);
                    AirBlockUnitInfos = r.ReadList<Int3>(airBlockUnitInfosCount);
                }
            }

            public void Write(GbxWriter w, int v = 0)
            {
                w.WriteIdAsString(Id);
                w.WriteIdAsString(Author);

                if (v == 0)
                {
                    w.Write(U01);
                }
                else
                {
                    w.Write(Flags);

                    if ((Flags & 0b01000000) != 0) // has icon
                    {
                        w.Write(IconWidth.GetValueOrDefault());
                        w.Write(IconHeight.GetValueOrDefault());
                        w.Write(IconData ?? []);
                    }
                }

                if (v == 0)
                {
                    w.Write((byte)WaypointType);
                    w.Write(BlockType);
                }

                var packedVersion = (int)WaypointType | (v << 4);

                if (BlockType is 2 or 3) // classic or road
                {
                    w.WriteListWritable(GroundSubVariations0, byteLengthPrefix: true, packedVersion);
                    w.WriteListWritable(AirSubVariations0, byteLengthPrefix: true, packedVersion);
                }

                if (BlockType == 3) // road
                {
                    w.WriteListWritable(GroundSubVariations1, byteLengthPrefix: true, packedVersion);
                    w.WriteListWritable(AirSubVariations1, byteLengthPrefix: true, packedVersion);
                    w.WriteListWritable(GroundSubVariations2, byteLengthPrefix: true, packedVersion);
                    w.WriteListWritable(AirSubVariations2, byteLengthPrefix: true, packedVersion);
                    w.WriteListWritable(GroundSubVariations3, byteLengthPrefix: true, packedVersion);
                    w.WriteListWritable(AirSubVariations3, byteLengthPrefix: true, packedVersion);
                    w.WriteListWritable(GroundSubVariations4, byteLengthPrefix: true, packedVersion);
                    w.WriteListWritable(AirSubVariations4, byteLengthPrefix: true, packedVersion);
                    w.WriteListWritable(GroundSubVariations5, byteLengthPrefix: true, packedVersion);
                    w.WriteListWritable(AirSubVariations5, byteLengthPrefix: true, packedVersion);
                }

                w.Write(SpawnOffsetGround);
                w.Write(SpawnRotationGround);
                w.Write(SpawnOffsetAir);
                w.Write(SpawnRotationAir);

                if (v >= 1)
                {
                    w.Write(GroundBlockUnitInfos?.Count ?? 0);
                    w.Write(AirBlockUnitInfos?.Count ?? 0);

                    foreach (var groundBlockUnitInfo in GroundBlockUnitInfos ?? [])
                    {
                        w.Write(groundBlockUnitInfo);
                    }

                    foreach (var airBlockUnitInfo in AirBlockUnitInfos ?? [])
                    {
                        w.Write(airBlockUnitInfo);
                    }
                }
            }
        }

        public class SubVariation : IReadable, IWritable
        {
            public CPlugTree? Tree { get; set; }
            public CPlugTree? TriggerTree { get; set; }
            public byte? PreLightGenTileCountU { get; set; }

            public void Read(GbxReader r, int v = 0)
            {
                var waypointType = (CGameItemModel.EWaypointType)(v & 0xF);
                var ver = (v >> 4) & 0xF;

                Tree = r.ReadNodeRef<CPlugTree>();

                if (waypointType is CGameItemModel.EWaypointType.Finish
                                 or CGameItemModel.EWaypointType.Checkpoint
                                 or CGameItemModel.EWaypointType.StartFinish)
                {
                    TriggerTree = r.ReadNodeRef<CPlugTree>();
                }

                if (ver >= 2)
                {
                    PreLightGenTileCountU = r.ReadByte();
                }
            }

            public void Write(GbxWriter w, int v = 0)
            {
                var waypointType = (CGameItemModel.EWaypointType)(v & 0xF);
                var ver = (v >> 4) & 0xF;

                w.WriteNodeRef(Tree);

                if (waypointType is CGameItemModel.EWaypointType.Finish
                                 or CGameItemModel.EWaypointType.Checkpoint
                                 or CGameItemModel.EWaypointType.StartFinish)
                {
                    w.WriteNodeRef(TriggerTree);
                }

                if (ver >= 2)
                {
                    w.Write(PreLightGenTileCountU.GetValueOrDefault());
                }
            }
        }

        public class ReplacementTextureFlags : IReadable, IWritable
        {
            public const byte SpecularBit = 0;
            public const byte NormalBit = 1;
            public const byte WhiteBit = 2;
            public const byte BlackBit = 3;

            public int? SpecularInstanceIndex { get; set; }
            public int? NormalInstanceIndex { get; set; }
            public int? WhiteInstanceIndex { get; set; }
            public int? BlackInstanceIndex { get; set; }

            public void Read(GbxReader r, int v = 0)
            {
                var flags = r.ReadByte();

                if ((flags & (1 << SpecularBit)) != 0) SpecularInstanceIndex = r.ReadInt32();
                if ((flags & (1 << NormalBit)) != 0) NormalInstanceIndex = r.ReadInt32();
                if ((flags & (1 << WhiteBit)) != 0) WhiteInstanceIndex = r.ReadInt32();
                if ((flags & (1 << BlackBit)) != 0) BlackInstanceIndex = r.ReadInt32();
            }

            public void Write(GbxWriter w, int v = 0)
            {
                byte flags = 0;
                if (SpecularInstanceIndex.HasValue) flags |= 1 << SpecularBit;
                if (NormalInstanceIndex.HasValue) flags |= 1 << NormalBit;
                if (WhiteInstanceIndex.HasValue) flags |= 1 << WhiteBit;
                if (BlackInstanceIndex.HasValue) flags |= 1 << BlackBit;
                w.Write(flags);

                if (SpecularInstanceIndex.HasValue) w.Write(SpecularInstanceIndex.GetValueOrDefault());
                if (NormalInstanceIndex.HasValue) w.Write(NormalInstanceIndex.GetValueOrDefault());
                if (WhiteInstanceIndex.HasValue) w.Write(WhiteInstanceIndex.GetValueOrDefault());
                if (BlackInstanceIndex.HasValue) w.Write(BlackInstanceIndex.GetValueOrDefault());
            }
        }

        internal class MaterialModelRef : IReadable, IWritable
        {
            public int InstanceIndex { get; set; }
            public string MaterialModelRelativePath { get; set; } = string.Empty;

            public void Read(GbxReader r, int v = 0)
            {
                InstanceIndex = r.ReadInt32();
                MaterialModelRelativePath = r.ReadString();
            }

            public void Write(GbxWriter w, int v = 0)
            {
                w.Write(InstanceIndex);
                w.Write(MaterialModelRelativePath);
            }
        }

        public class EmbeddedImage : IReadable, IWritable
        {
            public uint ClassId { get; set; }
            public string RelativePath { get; set; } = string.Empty;
            public int ImageSize { get; set; }
            public byte[] ImageData { get; set; } = [];
            public List<BitmapPair> BitmapPairs { get; set; } = [];

            public void Read(GbxReader r, int v = 0)
            {
                ClassId = r.ReadUInt32();
                RelativePath = r.ReadString();
                ImageSize = r.ReadInt32();
                ImageData = r.ReadBytes(ImageSize);
                BitmapPairs = r.ReadListReadable<BitmapPair>();
            }

            public void Write(GbxWriter w, int v = 0)
            {
                w.Write(ClassId);
                w.Write(RelativePath);
                w.Write(ImageSize);
                w.Write(ImageData);
                w.WriteListWritable(BitmapPairs);
            }
        }

        public class BitmapPair : IReadable, IWritable
        {
            internal int InstanceIndex { get; set; }
            public GbxRefTableFile? Instance { get; set; }
            public byte TexFilter { get; set; }
            public byte TexAddress { get; set; }

            public void Read(GbxReader r, int v = 0)
            {
                InstanceIndex = r.ReadInt32();
                TexFilter = r.ReadByte();
                TexAddress = r.ReadByte();
            }

            public void Write(GbxWriter w, int v = 0)
            {
                w.Write(Instance is not null && w.NodeDict.TryGetValue(Instance, out var index) ? index : -1);
                w.Write(TexFilter);
                w.Write(TexAddress);
            }
        }

        public class TriggerGroup : IReadable, IWritable
        {
            private TriggerGroupCondition? condition;
            private TriggerGroupEvent? onEnterEvent;
            private TriggerGroupEvent? onInsideEvent;
            private TriggerGroupEvent? onLeaveEvent;

            public string Name { get; set; } = string.Empty;
            public byte Flags { get; set; }

            public TriggerGroupCondition? Condition
            {
                get => condition;
                set
                {
                    condition = value;

                    if (value is null) Flags &= 0xFE;
                    else Flags |= 1;
                }
            }

            public TriggerGroupEvent? OnEnterEvent
            {
                get => onEnterEvent;
                set
                {
                    onEnterEvent = value;

                    if (value is null) Flags &= 0xFD;
                    else Flags |= 2;
                }
            }

            public TriggerGroupEvent? OnInsideEvent
            {
                get => onInsideEvent;
                set
                {
                    onInsideEvent = value;

                    if (value is null) Flags &= 0xFB;
                    else Flags |= 4;
                }
            }

            public TriggerGroupEvent? OnLeaveEvent
            {
                get => onLeaveEvent;
                set
                {
                    onLeaveEvent = value;

                    if (value is null) Flags &= 0xF7;
                    else Flags |= 8;
                }
            }

            public void Read(GbxReader r, int v = 0)
            {
                Name = r.ReadString();
                Flags = r.ReadByte();

                if ((Flags & 1) != 0)
                {
                    condition = r.ReadReadable<TriggerGroupCondition>();
                }

                if ((Flags & 2) != 0)
                {
                    onEnterEvent = r.ReadReadable<TriggerGroupEvent>();
                }

                if ((Flags & 4) != 0)
                {
                    onInsideEvent = r.ReadReadable<TriggerGroupEvent>();
                }

                if ((Flags & 8) != 0)
                {
                    onLeaveEvent = r.ReadReadable<TriggerGroupEvent>();
                }
            }

            public void Write(GbxWriter w, int v = 0)
            {
                w.Write(Name);
                w.Write(Flags);

                if (Condition is not null)
                {
                    w.WriteWritable(Condition);
                }

                if (OnEnterEvent is not null)
                {
                    w.WriteWritable(OnEnterEvent);
                }

                if (OnInsideEvent is not null)
                {
                    w.WriteWritable(OnInsideEvent);
                }

                if (OnLeaveEvent is not null)
                {
                    w.WriteWritable(OnLeaveEvent);
                }
            }
        }

        public class TriggerGroupCondition : IReadable, IWritable
        {
            public enum EEventTarget
            {
                MediaTracker,
                AngelScript
            }

            public EEventTarget EventTarget { get; set; }
            public byte? ConditionType { get; set; }
            public float? Value { get; set; }
            public uint? ModuleIndex { get; set; }
            public uint? FunctionIndex { get; set; }

            public void Read(GbxReader r, int v = 0)
            {
                EventTarget = (EEventTarget)r.ReadByte();

                switch (EventTarget)
                {
                    case EEventTarget.MediaTracker:
                        ConditionType = r.ReadByte();
                        if (ConditionType != 0) // none
                        {
                            Value = r.ReadSingle();
                        }
                        break;
                    case EEventTarget.AngelScript:
                        ModuleIndex = r.ReadUInt32();
                        FunctionIndex = r.ReadUInt32();
                        break;
                }
            }

            public void Write(GbxWriter w, int v = 0)
            {
                w.Write((byte)EventTarget);

                switch (EventTarget)
                {
                    case EEventTarget.MediaTracker:
                        w.Write(ConditionType ?? 0);
                        if (ConditionType != 0) // none
                        {
                            w.Write(Value ?? 0);
                        }
                        break;
                    case EEventTarget.AngelScript:
                        w.Write(ModuleIndex ?? 0);
                        w.Write(FunctionIndex ?? 0);
                        break;
                }
            }
        }

        public class TriggerGroupEvent : IReadable, IWritable
        {
            public enum EEventTarget
            {
                ParameterSet,
                AngelScript = 2
            }

            public EEventTarget EventTarget { get; set; }
            public uint? ParameterSetIndex { get; set; }
            public uint? ModuleIndex { get; set; }
            public uint? FunctionIndex { get; set; }

            public void Read(GbxReader r, int v = 0)
            {
                EventTarget = (EEventTarget)r.ReadByte();

                switch (EventTarget)
                {
                    case EEventTarget.ParameterSet:
                        ParameterSetIndex = r.ReadUInt32();
                        break;
                    case EEventTarget.AngelScript:
                        ModuleIndex = r.ReadUInt32();
                        FunctionIndex = r.ReadUInt32();
                        break;
                }
            }

            public void Write(GbxWriter w, int v = 0)
            {
                w.Write((byte)EventTarget);

                switch (EventTarget)
                {
                    case EEventTarget.ParameterSet:
                        w.Write(ParameterSetIndex ?? 0);
                        break;
                    case EEventTarget.AngelScript:
                        w.Write(ModuleIndex ?? 0);
                        w.Write(FunctionIndex ?? 0);
                        break;
                }
            }
        }

        public class BlockGroup : IReadable, IWritable
        {
            public string Name { get; set; } = string.Empty;

            public void Read(GbxReader r, int v = 0)
            {
                Name = r.ReadString();
            }

            public void Write(GbxWriter w, int v = 0)
            {
                w.Write(Name);
            }
        }

        public record AngelScriptModule : IReadable, IWritable
        {
            private string? moduleName;
            private byte[]? byteCode;

            public byte Flags { get; private set; }

            public string? ModuleName
            {
                get => moduleName;
                set
                {
                    moduleName = value;

                    if (value is null)
                    {
                        Flags |= 2;
                    }
                    else
                    {
                        Flags &= 0xFD;
                    }
                }
            }

            public byte[]? ByteCode
            {
                get => byteCode;
                set
                {
                    byteCode = value;

                    if (value is null)
                    {
                        Flags |= 1;
                    }
                    else
                    {
                        Flags &= 0xFE;
                    }
                }
            }

            public bool IsEmpty => (Flags & 1) != 0;
            public bool IsCoreModule => (Flags & 2) != 0;

            public void Read(GbxReader r, int v = 0)
            {
                Flags = r.ReadByte();

                if (!IsCoreModule)
                {
                    moduleName = r.ReadString();
                }

                if (!IsEmpty)
                {
                    throw new NotSupportedException("AngelScript modules are not supported for TMUnlimiter 2.0 due to parsing problems");
                }
            }

            public void Write(GbxWriter w, int v = 0)
            {
                w.Write(Flags);

                if (!IsCoreModule)
                {
                    w.Write(ModuleName);
                }

                if (!IsEmpty)
                {
                    throw new NotSupportedException("AngelScript modules are not currently supported");
                }
            }
        }

        public record Block
        {
            public Int3 Coord { get; set; }
            public Direction Direction { get; set; }
            public Vec3 Offset { get; set; }
            public Vec3 Rotation { get; set; }
            public Vec3 Scale { get; set; } = Vec3.One;
            public Motion? Motion { get; set; }
            public Vec3? OriginOffset { get; set; }
            public List<BlockGroup> BlockGroups { get; set; } = [];
            public ESpawnPointAlterMethod SpawnPointAlterMethod { get; set; }
            public Vec3? SpawnOffset { get; set; }
            public Vec3? SpawnRotation { get; set; }
            public bool IsNonCollidable { get; set; }
            public bool IsDynamic { get; set; }
            public bool IsInvisible { get; set; }
            public ERespawnCapability RespawnCapability { get; set; }
            public EBlockTriggerActivationMethod BlockTriggerActivationMethod { get; set; }
        }

        public record TriggerBlock : Block
        {
            public List<TriggerGroup> TriggerGroups { get; set; } = [];
        }

        public record ExternalBlock(string Name, string Author) : Block
        {
            public int Flags { get; set; }
        }

        public record EmbeddedBlock(EmbeddedBlockData Data) : Block
        {
            public int Flags { get; set; }
        }

        public record Motion
        {
            public int WaveType { get; set; }
            public List<MotionPoint> Points { get; set; } = [];
        }

        public record MotionPoint : IReadable, IWritable
        {
            public TimeInt32 Time { get; set; }
            public Vec3 Offset { get; set; }
            public Vec3 Rotation { get; set; }

            public void Read(GbxReader r, int v = 0)
            {
                Time = r.ReadTimeInt32();
                Offset = r.ReadVec3();
                Rotation = r.ReadVec3();
            }

            public void Write(GbxWriter w, int v = 0)
            {
                w.Write(Time);
                w.Write(Offset);
                w.Write(Rotation);
            }
        }

        public enum ESpawnPointAlterMethod
        {
            None,
            Manual,
            Automatic
        }

        public enum ERespawnCapability
        {
            Default,
            ForceIsRespawnable,
            ForceIsNonRespawnable
        }

        public enum EBlockTriggerActivationMethod
        {
            Default,
            Disabled,
            TriggerManually
        }

        public enum MediaClipMappedResourceType
        {
            ParameterSet,
            LegacyScript
        }

        public enum ParameterOperation
        {
            Execute,
            Set,
            Multiply
        }

        public enum ParameterName
        {
            Vehicle_GravityGround,
            Vehicle_GravityAir,
            Vehicle_SpeedClamp,
            Vehicle_MaxSpeedForward,
            Vehicle_MaxSpeedBackward,
            Vehicle_YellowBoostMultiplier,
            Vehicle_RedBoostMultiplier,
            Vehicle_YellowBoostDuration,
            Vehicle_RedBoostDuration,
            Vehicle_AccelerationCurve,
            Vehicle_BrakeMax,
            Vehicle_LinearFluidFrictionMultiplier,
            Vehicle_SteerDriveTorque,
            Vehicle_SlopeSpeedGainLimit,
            Vehicle_BodyFrictionWithConcreteMultiplier,
            Vehicle_BodyFrictionWithMetalMultiplier,
            Vehicle_MaxSideFriction,
            Vehicle_GroundSlowDownBaseValue,
            Vehicle_GroundSlowDownMultiplier,
            Vehicle_BrakeMaxDynamic,
            Vehicle_WaterReboundFromSpeedRatio,
            Vehicle_AngularFluidFrictionFirstMultiplier,
            Vehicle_AngularFluidFrictionSecondMultiplier,
            Vehicle_Scale,
            Vehicle_EnginePitch,
            Vehicle_EngineVolume,
            Vehicle_AddLinearSpeedX,
            Vehicle_AddLinearSpeedY,
            Vehicle_AddLinearSpeedZ,
            Reset_Everything,
            Reset_GravityGround,
            Reset_GravityAir,
            Reset_SpeedClamp,
            Reset_MaxSpeedForward,
            Reset_MaxSpeedBackward,
            Reset_YellowBoostMultiplier,
            Reset_RedBoostMultiplier,
            Reset_YellowBoostDuration,
            Reset_RedBoostDuration,
            Reset_AccelerationCurve,
            Reset_BrakeMax,
            Reset_LinearFluidFrictionMultiplier,
            Reset_SteerDriveTorque,
            Reset_SlopeSpeedGainLimit,
            Reset_BodyFrictionWithConcreteMultiplier,
            Reset_BodyFrictionWithMetalMultiplier,
            Reset_MaxSideFriction,
            Reset_GroundSlowDownBaseValue,
            Reset_GroundSlowDownMultiplier,
            Reset_BrakeMaxDynamic,
            Reset_WaterReboundFromSpeedRatio,
            Reset_AngularFluidFrictionFirstMultiplier,
            Reset_AngularFluidFrictionSecondMultiplier,
            Reset_Scale,
            Reset_EnginePitch,
            Reset_EngineVolume,
            World_ExecuteParameterSet,
            World_ExecuteScript,
            World_DisplayMediaTrackerClip,
            World_BlockGroupMakeVisible,
            World_BlockGroupMakeInvisible,
            World_BlockGroupMakeCollidable,
            World_BlockGroupMakeNonCollidable,
            World_SetGravityForceX,
            World_SetGravityForceY,
            World_SetGravityForceZ,
            Vehicle_AbsorbTension,
            Vehicle_AbsorbingValKa,
            Vehicle_AbsorbingValKi,
            Vehicle_AbsorbingValMax,
            Vehicle_AbsorbingValMin,
            Vehicle_AbsorbingValRest,
            Vehicle_AccelCurveRearGear,
            Vehicle_AirControlDuration,
            Vehicle_AirControlZCoefFromAngularSpeed,
            Vehicle_AngularSpeedClamp,
            Vehicle_AngularSpeedImpulseScale,
            Vehicle_AngularSpeedYImpulseBlend,
            Vehicle_AxialSlopeAdherenceMax,
            Vehicle_AxialSlopeAdherenceMin,
            Vehicle_BodyRestCoefConcrete,
            Vehicle_BodyRestCoefMetal,
            Vehicle_BrakeBase,
            Vehicle_BrakeCoef,
            Vehicle_BrakeHeatSpeedFromFBrake,
            Vehicle_CMAftFore,
            Vehicle_CMDownUp,
            Vehicle_DebugAbsorbCoef,
            Vehicle_Field0x18,
            Vehicle_Field0x1c,
            Vehicle_Field0x1d8,
            Vehicle_Field0x20,
            Vehicle_Field0x28,
            Vehicle_Field0x33c,
            Vehicle_Field0x340,
            Vehicle_Field0x344,
            Vehicle_Field0x348,
            Vehicle_Field0x34c,
            Vehicle_Field0x358,
            Vehicle_Field0x35c,
            Vehicle_Field0x360,
            Vehicle_Field0x384,
            Vehicle_Field0x38,
            Vehicle_Field0x398,
            Vehicle_Field0x39c,
            Vehicle_Field0x3a0,
            Vehicle_Field0x3a4,
            Vehicle_Field0x3a8,
            Vehicle_Field0x3c,
            Vehicle_Field0x50,
            Vehicle_Field0x54,
            Vehicle_GearCount,
            Vehicle_GlidingGravityCoef,
            Vehicle_InertiaHalfDiagX,
            Vehicle_InertiaHalfDiagY,
            Vehicle_InertiaHalfDiagZ,
            Vehicle_InertiaMass,
            Vehicle_IsFakeEngine,
            Vehicle_JumpImpulseVal,
            Vehicle_LateralContactSlowDown,
            Vehicle_LateralSlopeAdherenceMax,
            Vehicle_LateralSlopeAdherenceMin,
            Vehicle_LimitToMaxSpeedForce,
            Vehicle_LinearSpeed2PositiveDeltaMax,
            Vehicle_M4AccelFromSlipAngle,
            Vehicle_M4LateralFrictionForce,
            Vehicle_M4LateralFrictionSquareForce,
            Vehicle_M4LateralFrictionSquareTorque,
            Vehicle_M4LateralFrictionTorque,
            Vehicle_M4LeaveSplippingSpeed,
            Vehicle_M4MaxFrictionForceFromSpeed,
            Vehicle_M4MaxFrictionForceWhenSlippingCoef,
            Vehicle_M4MaxFrictionTorqueFromSpeed,
            Vehicle_M4MaxFrictionTorqueWhenSlippingCoef,
            Vehicle_M4SlipAngleSpeed,
            Vehicle_M4SteerAngleWhenSlippingMax,
            Vehicle_M4SteerRadiusCoefFromSlipAngle,
            Vehicle_M4SteerRadiusFromSpeed,
            Vehicle_M4SteerRadiusWhenSlippingCoef,
            Vehicle_M4SteerTorqueCoef,
            Vehicle_M5AccelSlipCoefMax,
            Vehicle_M5KeepNoSteerSlowDownWhenSlippingDuration,
            Vehicle_M5KeepSlidingAccelDurarion,
            Vehicle_M5KeepSteerSlowDownDurarion,
            Vehicle_M5LateralContactSlowDownDuration,
            Vehicle_M5MaxAxialRolloverTorque,
            Vehicle_M5SlippingAccelCurve,
            Vehicle_M5SlippingAccelCurveCoef,
            Vehicle_M5SmoothInputSteerDurationFromSpeed,
            Vehicle_M5SteerCoefFromSpeed,
            Vehicle_M6AfterBurnoutAccMod,
            Vehicle_M6AfterBurnoutDuration,
            Vehicle_M6AfterBurnoutImpulse,
            Vehicle_M6AirRpmAcc,
            Vehicle_M6AirRpmDeadening,
            Vehicle_M6BrakeMaxDynamicRear,
            Vehicle_M6BrakeMaxRear,
            Vehicle_M6BrakeModulationWhenSlipping,
            Vehicle_M6BrakeSmokeIntensity,
            Vehicle_M6BurnoutAccMod,
            Vehicle_M6BurnoutCenterForceCoeff2,
            Vehicle_M6BurnoutCenterForceCoeff,
            Vehicle_M6BurnoutDuration,
            Vehicle_M6BurnoutFricMod,
            Vehicle_M6BurnoutLateralSpeed,
            Vehicle_M6BurnoutLateralSpeedCoeff,
            Vehicle_M6BurnoutLateralSpeedMax,
            Vehicle_M6BurnoutRadius,
            Vehicle_M6BurnoutRadiusMax,
            Vehicle_M6BurnoutRolloverFromSpeed,
            Vehicle_M6BurnoutRpmAcc,
            Vehicle_M6BurnoutSmokeIntensity,
            Vehicle_M6BurnoutSmokeVelocity,
            Vehicle_M6BurnoutSteerCoeff2,
            Vehicle_M6BurnoutSteerCoeff3,
            Vehicle_M6BurnoutSteerCoeff4,
            Vehicle_M6BurnoutSteerCoeff,
            Vehicle_M6BurnoutWheelAngularRotation,
            Vehicle_M6DonutRolloverFromSpeed,
            Vehicle_M6ForceEpsilon,
            Vehicle_M6FrictionModulationWhenSlipNBrake,
            Vehicle_M6GearRatio,
            Vehicle_M6InertialMass,
            Vehicle_M6InertialTorqueModulationX,
            Vehicle_M6InertialTorqueModulationZ,
            Vehicle_M6MaxDiffBtwGroundNormal,
            Vehicle_M6MaxDiffBtwnPropulsionAndSpeed,
            Vehicle_M6MaxNegAngle4Burnout,
            Vehicle_M6MaxPosAngle4Burnout,
            Vehicle_M6MaxRPM,
            Vehicle_M6MaxRpm,
            Vehicle_M6MaxSpeed4Burnout,
            Vehicle_M6MinRPM,
            Vehicle_M6MinSpeed4Burnout,
            Vehicle_M6RolloverLateralFromSpeedRatio,
            Vehicle_M6RpmComputedOnGearDown,
            Vehicle_M6RpmDelta,
            Vehicle_M6RpmGainCoefOnGearDown,
            Vehicle_M6RpmGainOnTakeOff,
            Vehicle_M6RpmLossCoefOnGearUp,
            Vehicle_M6RpmLossOnTakeOffFinished,
            Vehicle_M6RpmWantedOnGearUp,
            Vehicle_M6SpeedLimitNegForTakeOffFront,
            Vehicle_M6SpeedLimitNegForTakeOffRear,
            Vehicle_M6SpeedLimitPositiveForTakeOffFront,
            Vehicle_M6SpeedLimitPositiveForTakeOffRear,
            Vehicle_Mass,
            Vehicle_MaxAngularSpeedYAirControl,
            Vehicle_MaxDistPerStep,
            Vehicle_MaxSideFrictionBlendCoef,
            Vehicle_MaxSideFrictionSliding,
            Vehicle_MinGear,
            Vehicle_ModulationFromWheelCompression,
            Vehicle_NoSteerSlowDownWhenSlipping,
            Vehicle_RelSpeedMultCoef,
            Vehicle_RolloverAxial,
            Vehicle_RolloverLateral,
            Vehicle_RolloverLateralFromAngle,
            Vehicle_RubberBallElasticity,
            Vehicle_ShockModel,
            Vehicle_SideFriction1,
            Vehicle_SideFriction2,
            Vehicle_SlipAngleForceCoef1,
            Vehicle_SlipAngleForceCoef2,
            Vehicle_SlipAngleForceMax,
            Vehicle_SoundEngineVolume,
            Vehicle_SoundImpactVolume,
            Vehicle_SoundSkidConcreteVolume,
            Vehicle_SoundSkidSandVolume,
            Vehicle_SteerAngleMax,
            Vehicle_SteerDurationBeforeSteerSlowDown,
            Vehicle_SteerGroundTorque,
            Vehicle_SteerGroundTorqueSlippingCoef,
            Vehicle_SteerLowSpeed,
            Vehicle_SteerMaxBlend,
            Vehicle_SteerModel,
            Vehicle_SteerRadiusCoef,
            Vehicle_SteerRadiusMin,
            Vehicle_SteerSlowDown,
            Vehicle_SteerSlowDownCoef,
            Vehicle_SteerSlowDownFadeInDuration,
            Vehicle_SteerSlowDownFadeOutDuration,
            Vehicle_SteerSpeed,
            Vehicle_TireMaterial,
            Vehicle_TwistAngle,
            Vehicle_VibrationPeriodSpeedCoef,
            Vehicle_VisualSteerAngleFromSpeed,
            Vehicle_WaterAngularFriction,
            Vehicle_WaterAngularFrictionSq,
            Vehicle_WaterBumpMinSpeed,
            Vehicle_WaterBumpSlowDownFromSpeedRatio,
            Vehicle_WaterFrictionFromSpeed,
            Vehicle_WaterGravity,
            Vehicle_WaterReboundMinHSpeed,
            Vehicle_WaterSplashFromSpeed,
            Vehicle_WheelFrictionCoefConcrete,
            Vehicle_WheelFrictionCoefMetal,
            Vehicle_WheelRestCoefConcrete,
            Vehicle_WheelRestCoefMetal,
            Reset_GravityForce,
            Reset_AbsorbTension,
            Reset_AbsorbingValKa,
            Reset_AbsorbingValKi,
            Reset_AbsorbingValMax,
            Reset_AbsorbingValMin,
            Reset_AbsorbingValRest,
            Reset_AccelCurveRearGear,
            Reset_AirControlDuration,
            Reset_AirControlZCoefFromAngularSpeed,
            Reset_AngularSpeedClamp,
            Reset_AngularSpeedImpulseScale,
            Reset_AngularSpeedYImpulseBlend,
            Reset_AxialSlopeAdherenceMax,
            Reset_AxialSlopeAdherenceMin,
            Reset_BodyRestCoefConcrete,
            Reset_BodyRestCoefMetal,
            Reset_BrakeBase,
            Reset_BrakeCoef,
            Reset_BrakeHeatSpeedFromFBrake,
            Reset_CMAftFore,
            Reset_CMDownUp,
            Reset_DebugAbsorbCoef,
            Reset_Field0x18,
            Reset_Field0x1c,
            Reset_Field0x1d8,
            Reset_Field0x20,
            Reset_Field0x28,
            Reset_Field0x33c,
            Reset_Field0x340,
            Reset_Field0x344,
            Reset_Field0x348,
            Reset_Field0x34c,
            Reset_Field0x358,
            Reset_Field0x35c,
            Reset_Field0x360,
            Reset_Field0x384,
            Reset_Field0x38,
            Reset_Field0x398,
            Reset_Field0x39c,
            Reset_Field0x3a0,
            Reset_Field0x3a4,
            Reset_Field0x3a8,
            Reset_Field0x3c,
            Reset_Field0x50,
            Reset_Field0x54,
            Reset_GearCount,
            Reset_GlidingGravityCoef,
            Reset_InertiaHalfDiagX,
            Reset_InertiaHalfDiagY,
            Reset_InertiaHalfDiagZ,
            Reset_InertiaMass,
            Reset_IsFakeEngine,
            Reset_JumpImpulseVal,
            Reset_LateralContactSlowDown,
            Reset_LateralSlopeAdherenceMax,
            Reset_LateralSlopeAdherenceMin,
            Reset_LimitToMaxSpeedForce,
            Reset_LinearSpeed2PositiveDeltaMax,
            Reset_M4AccelFromSlipAngle,
            Reset_M4LateralFrictionForce,
            Reset_M4LateralFrictionSquareForce,
            Reset_M4LateralFrictionSquareTorque,
            Reset_M4LateralFrictionTorque,
            Reset_M4LeaveSplippingSpeed,
            Reset_M4MaxFrictionForceFromSpeed,
            Reset_M4MaxFrictionForceWhenSlippingCoef,
            Reset_M4MaxFrictionTorqueFromSpeed,
            Reset_M4MaxFrictionTorqueWhenSlippingCoef,
            Reset_M4SlipAngleSpeed,
            Reset_M4SteerAngleWhenSlippingMax,
            Reset_M4SteerRadiusCoefFromSlipAngle,
            Reset_M4SteerRadiusFromSpeed,
            Reset_M4SteerRadiusWhenSlippingCoef,
            Reset_M4SteerTorqueCoef,
            Reset_M5AccelSlipCoefMax,
            Reset_M5KeepNoSteerSlowDownWhenSlippingDuration,
            Reset_M5KeepSlidingAccelDurarion,
            Reset_M5KeepSteerSlowDownDurarion,
            Reset_M5LateralContactSlowDownDuration,
            Reset_M5MaxAxialRolloverTorque,
            Reset_M5SlippingAccelCurve,
            Reset_M5SlippingAccelCurveCoef,
            Reset_M5SmoothInputSteerDurationFromSpeed,
            Reset_M5SteerCoefFromSpeed,
            Reset_M6AfterBurnoutAccMod,
            Reset_M6AfterBurnoutDuration,
            Reset_M6AfterBurnoutImpulse,
            Reset_M6AirRpmAcc,
            Reset_M6AirRpmDeadening,
            Reset_M6BrakeMaxDynamicRear,
            Reset_M6BrakeMaxRear,
            Reset_M6BrakeModulationWhenSlipping,
            Reset_M6BrakeSmokeIntensity,
            Reset_M6BurnoutAccMod,
            Reset_M6BurnoutCenterForceCoeff2,
            Reset_M6BurnoutCenterForceCoeff,
            Reset_M6BurnoutDuration,
            Reset_M6BurnoutFricMod,
            Reset_M6BurnoutLateralSpeed,
            Reset_M6BurnoutLateralSpeedCoeff,
            Reset_M6BurnoutLateralSpeedMax,
            Reset_M6BurnoutRadius,
            Reset_M6BurnoutRadiusMax,
            Reset_M6BurnoutRolloverFromSpeed,
            Reset_M6BurnoutRpmAcc,
            Reset_M6BurnoutSmokeIntensity,
            Reset_M6BurnoutSmokeVelocity,
            Reset_M6BurnoutSteerCoeff2,
            Reset_M6BurnoutSteerCoeff3,
            Reset_M6BurnoutSteerCoeff4,
            Reset_M6BurnoutSteerCoeff,
            Reset_M6BurnoutWheelAngularRotation,
            Reset_M6DonutRolloverFromSpeed,
            Reset_M6ForceEpsilon,
            Reset_M6FrictionModulationWhenSlipNBrake,
            Reset_M6GearRatio,
            Reset_M6InertialMass,
            Reset_M6InertialTorqueModulationX,
            Reset_M6InertialTorqueModulationZ,
            Reset_M6MaxDiffBtwGroundNormal,
            Reset_M6MaxDiffBtwnPropulsionAndSpeed,
            Reset_M6MaxNegAngle4Burnout,
            Reset_M6MaxPosAngle4Burnout,
            Reset_M6MaxRPM,
            Reset_M6MaxRpm,
            Reset_M6MaxSpeed4Burnout,
            Reset_M6MinRPM,
            Reset_M6MinSpeed4Burnout,
            Reset_M6RolloverLateralFromSpeedRatio,
            Reset_M6RpmComputedOnGearDown,
            Reset_M6RpmDelta,
            Reset_M6RpmGainCoefOnGearDown,
            Reset_M6RpmGainOnTakeOff,
            Reset_M6RpmLossCoefOnGearUp,
            Reset_M6RpmLossOnTakeOffFinished,
            Reset_M6RpmWantedOnGearUp,
            Reset_M6SpeedLimitNegForTakeOffFront,
            Reset_M6SpeedLimitNegForTakeOffRear,
            Reset_M6SpeedLimitPositiveForTakeOffFront,
            Reset_M6SpeedLimitPositiveForTakeOffRear,
            Reset_Mass,
            Reset_MaxAngularSpeedYAirControl,
            Reset_MaxDistPerStep,
            Reset_MaxSideFrictionBlendCoef,
            Reset_MaxSideFrictionSliding,
            Reset_MinGear,
            Reset_ModulationFromWheelCompression,
            Reset_NoSteerSlowDownWhenSlipping,
            Reset_RelSpeedMultCoef,
            Reset_RolloverAxial,
            Reset_RolloverLateral,
            Reset_RolloverLateralFromAngle,
            Reset_RubberBallElasticity,
            Reset_ShockModel,
            Reset_SideFriction1,
            Reset_SideFriction2,
            Reset_SlipAngleForceCoef1,
            Reset_SlipAngleForceCoef2,
            Reset_SlipAngleForceMax,
            Reset_SoundEngineVolume,
            Reset_SoundImpactVolume,
            Reset_SoundSkidConcreteVolume,
            Reset_SoundSkidSandVolume,
            Reset_SteerAngleMax,
            Reset_SteerDurationBeforeSteerSlowDown,
            Reset_SteerGroundTorque,
            Reset_SteerGroundTorqueSlippingCoef,
            Reset_SteerLowSpeed,
            Reset_SteerMaxBlend,
            Reset_SteerModel,
            Reset_SteerRadiusCoef,
            Reset_SteerRadiusMin,
            Reset_SteerSlowDown,
            Reset_SteerSlowDownCoef,
            Reset_SteerSlowDownFadeInDuration,
            Reset_SteerSlowDownFadeOutDuration,
            Reset_SteerSpeed,
            Reset_TireMaterial,
            Reset_TwistAngle,
            Reset_VibrationPeriodSpeedCoef,
            Reset_VisualSteerAngleFromSpeed,
            Reset_WaterAngularFriction,
            Reset_WaterAngularFrictionSq,
            Reset_WaterBumpMinSpeed,
            Reset_WaterBumpSlowDownFromSpeedRatio,
            Reset_WaterFrictionFromSpeed,
            Reset_WaterGravity,
            Reset_WaterReboundMinHSpeed,
            Reset_WaterSplashFromSpeed,
            Reset_WheelFrictionCoefConcrete,
            Reset_WheelFrictionCoefMetal,
            Reset_WheelRestCoefConcrete,
            Reset_WheelRestCoefMetal,
            World_SetDayTime,
            World_EnableDynamicDayTime,
            World_DisableDynamicDayTime,
            World_SetDynamicDayTimeFlowSpeed,
            Vehicle_SetVehicleTuningByIndex,
            Vehicle_SetVehicleTuningByName,
            Vehicle_Transform,
            Reset_VehicleTuningParameters,
            Reset_VehicleTuning,
            Reset_Transform,
            Reset_VehicleEverything,
            Vehicle_DisableFreeWheeling,
            Vehicle_EnableFreeWheeling,
            Reset_VehicleMaterials
        }

        private record MediaClipMapping : IReadableWritable
        {
            public int MediaClipIndex { get; set; }
            public MediaClipMappedResourceType MappedResourceType { get; set; }
            public int? ParameterSetIndex { get; set; }
            public int? LegacyScriptIndex { get; set; }

            public void ReadWrite(GbxReaderWriter rw, int v = 0)
            {
                MediaClipIndex = rw.Int32(MediaClipIndex);
                MappedResourceType = rw.EnumByte(MappedResourceType);

                switch (MappedResourceType)
                {
                    case MediaClipMappedResourceType.ParameterSet:
                        ParameterSetIndex = rw.Int32(ParameterSetIndex);
                        break;
                    case MediaClipMappedResourceType.LegacyScript:
                        LegacyScriptIndex = rw.Int32(LegacyScriptIndex);
                        break;
                }
            }
        }
    }
}
