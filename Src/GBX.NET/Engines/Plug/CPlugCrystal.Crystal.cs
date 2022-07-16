namespace GBX.NET.Engines.Plug;

public partial class CPlugCrystal
{
    public class Crystal : IVersionable
    {
        public int Version { get; set; }

        public Vec3[] Positions { get; set; } = Array.Empty<Vec3>();
        public Face[] Faces { get; set; } = Array.Empty<Face>();
        public Group[] Groups { get; set; } = Array.Empty<Group>();

        public static Crystal Read(GameBoxReader r, CPlugMaterialUserInst?[]? materials)
        {
            var version = r.ReadInt32(); // up to 32 supported
            var u06 = r.ReadInt32(); // 4
            var u07 = r.ReadInt32(); // 3
            var u08 = r.ReadInt32(); // 4
            var u09 = r.ReadSingle(); // 64
            var u10 = r.ReadInt32(); // 2
            var u11 = r.ReadSingle(); // 128
            var u12 = r.ReadInt32(); // 1
            var u13 = r.ReadSingle(); // 192
            var u14 = r.ReadInt32(); // 0 - SAnchorInfo array?

            // SCrystalPart array
            var groups = r.ReadArray(r => new Group()
            {
                U01 = version >= 31 ? r.ReadInt32() : 0,
                U02 = version >= 36 ? r.ReadByte() : r.ReadInt32(), // maybe bool
                U03 = r.ReadInt32(),
                Name = r.ReadString(),
                U04 = r.ReadInt32(),
                U05 = r.ReadArray<int>()
            });

            if (version < 21)
            {
                // some other values
                throw new Exception("crystalVersion < 21 not supported");
            }

            var isEmbeddedCrystal = false;

            if (version >= 25)
            {
                if (version < 29)
                {
                    isEmbeddedCrystal = r.ReadBoolean();
                    isEmbeddedCrystal = r.ReadBoolean();
                }

                isEmbeddedCrystal = r.ReadBoolean(asByte: version >= 34);

                if (version >= 33)
                {
                    var u30 = r.ReadInt32(); // local_378
                    var u31 = r.ReadInt32(); // local_374
                }
            }

            var positions = default(Vec3[]);
            var edges = default(Int2[]);
            var faces = default(Face[]);

            if (isEmbeddedCrystal)
            {
                positions = r.ReadArray<Vec3>();

                var edgesCount = r.ReadInt32();

                if (version >= 35)
                {
                    var unfacedEdgesCount = r.ReadInt32();
                    var unfacedEdges = r.ReadOptimizedIntArray(unfacedEdgesCount * 2); // unfaced edges
                }

                edges = r.ReadArray<Int2>(version >= 35 ? 0 : edgesCount);

                var facesCount = r.ReadInt32();

                var uvs = default(Vec2[]);
                var faceIndicies = default(int[]);

                if (version >= 37)
                {
                    uvs = r.ReadArray<Vec2>(); // unique uv values
                    faceIndicies = r.ReadOptimizedIntArray();
                }

                var indiciesCounter = 0;

                faces = r.ReadArray(facesCount, r =>
                {
                    var vertCount = version >= 35 ? (r.ReadByte() + 3) : r.ReadInt32();
                    var inds = version >= 34 ? r.ReadOptimizedIntArray(vertCount, positions.Length) : r.ReadArray<int>(vertCount);

                    var verts = new Vertex[vertCount];

                    if (version < 27)
                    {
                        var uvCount = Math.Min(r.ReadInt32(), vertCount);

                        for (var i = 0; i < uvCount; i++)
                        {
                            verts[i] = new(positions[inds[i]], UV: r.ReadVec2());
                        }

                        var niceVec = r.ReadVec3();
                    }
                    else if (version < 37)
                    {
                        for (var i = 0; i < vertCount; i++)
                        {
                            verts[i] = new(positions[inds[i]], UV: r.ReadVec2());
                        }
                    }
                    else if (uvs is not null && faceIndicies is not null)
                    {
                        for (var i = 0; i < vertCount; i++)
                        {
                            verts[i] = new(positions[inds[i]], UV: uvs[faceIndicies[indiciesCounter]]);
                            indiciesCounter++;
                        }
                    }

                    var materialIndex = -1;

                    if (version >= 25)
                    {
                        if (version >= 33)
                        {
                            materialIndex = materials is null
                                ? r.ReadInt32()
                                : r.ReadOptimizedInt(materials.Length);
                        }
                        else
                        {
                            materialIndex = r.ReadInt32();
                        }
                    }

                    var groupIndex = version >= 33 ? r.ReadOptimizedInt(groups.Length) : r.ReadInt32();

                    var material = materialIndex != -1 ? materials?[materialIndex] : null;

                    return new Face(verts, groups[groupIndex], material);
                });
            }
            else
            {
                var handleVerts = r.ReadArray<(bool, int, int)>(r => new(
                    r.ReadBoolean(),
                    r.ReadInt32(),
                    r.ReadInt32()
                ));
                var handleVertsU01 = r.ReadInt32();
                var handleVertsU02 = r.ReadInt32();
                var handleEdges = r.ReadArray<(bool, int, int)>(r => new(
                    r.ReadBoolean(),
                    r.ReadInt32(),
                    r.ReadInt32()
                ));
                var handleEdgesU01 = r.ReadInt32();
                var handleEdgesU02 = r.ReadInt32();
                var handleFaces = r.ReadArray<(bool, int, int)>(r => new(
                    r.ReadBoolean(),
                    r.ReadInt32(),
                    r.ReadInt32()
                ));
                var handleFacesU01 = r.ReadInt32();
                var handleFacesU02 = r.ReadInt32();

                throw new NotSupportedException("Unsupported crystal.");

                var wtf = r.ReadArray<int>(15007);

                var verts = r.ReadArray<Vec3>();
            }

            foreach (var face in faces)
            {
                if (!isEmbeddedCrystal)
                {
                    var u18 = r.ReadInt32();
                }

                if (version < 30 || !isEmbeddedCrystal)
                {
                    var u19 = r.ReadInt32();
                }

                if (version >= 22 && !isEmbeddedCrystal)
                {
                    var u20 = r.ReadInt32();
                }
            }

            foreach (var pos in positions)
            {
                if (version < 29)
                {
                    var u21 = r.ReadSingle();
                }
            }

            var u22 = r.ReadInt32();

            if (version >= 7 && version < 32)
            {
                var u23 = r.ReadInt32(); // crystal link array

                if (version >= 10)
                {
                    var u24 = r.ReadInt32();
                    var u25 = r.ReadString();

                    if (version < 30)
                    {
                        var u26 = r.ReadArray<float>(); // SCrystalSmoothingGroup array
                    }
                }
            }

            if (version < 36)
            {
                var numFaces = r.ReadInt32();
                var numEdges = r.ReadInt32();
                var numVerts = r.ReadInt32();

                var u27 = r.ReadArray<int>(numFaces);
                var u28 = r.ReadArray<int>(numEdges);
                var u29 = r.ReadArray<int>(numVerts);

                var u17 = r.ReadInt32();
            }

            return new Crystal()
            {
                Version = version,
                Positions = positions,
                Faces = faces,
                Groups = groups
            };
        }
    }
}
