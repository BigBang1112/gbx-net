using GBX.NET.Serialization;

namespace GBX.NET.Engines.Plug;

public partial class CPlugCrystal
{
    public sealed partial class Crystal
    {
        public ElementPool VertexPool { get; set; } = new();
        public ElementPool EdgePool { get; set; } = new();
        public ElementPool FacePool { get; set; } = new();
        public EditableEdge[] EditableEdges { get; set; } = [];
        public CrystalMesh EditableMesh { get; set; } = new();
        public int[] FaceTypes { get; set; } = [];

        private void ReadWriteEditableGeometry(GbxReaderWriter rw)
        {
            if (rw.Writer is not null)
            {
                VertexPool = PreparePool(VertexPool, Positions.Length);
                EdgePool = PreparePool(EdgePool, Edges.Length);
                FacePool = PreparePool(FacePool, Faces.Length);
            }

            VertexPool.ReadWrite(rw);
            EdgePool.ReadWrite(rw);
            FacePool.ReadWrite(rw);
            var vertexHandles = GetUsedHandles(VertexPool);
            var edgeHandles = GetUsedHandles(EdgePool);
            var faceHandles = GetUsedHandles(FacePool);

            if (rw.Reader is not null) EditableEdges = new EditableEdge[edgeHandles.Length];
            if (EditableEdges.Length != edgeHandles.Length)
            {
                var edges = EditableEdges;
                Array.Resize(ref edges, edgeHandles.Length);
                EditableEdges = edges;
            }
            for (var i = 0; i < EditableEdges.Length; i++)
            {
                var edge = EditableEdges[i] ??= new EditableEdge();
                if (rw.Writer is not null)
                {
                    edge.VertexHandles = new Int2(vertexHandles[Edges[i].X], vertexHandles[Edges[i].Y]);
                }
                edge.ReadWrite(rw, Version);
            }

            if (rw.Writer is not null) UpdateEditableMesh();
            EditableMesh.ReadWrite(rw);
            if (EditableMesh.Version != 0) throw new VersionNotSupportedException(EditableMesh.Version);
            FaceTypes = Resize(FaceTypes, EditableMesh.Faces.Length);
            FaceTypes = rw.Array(FaceTypes, EditableMesh.Faces.Length)!;

            if (rw.Reader is not null)
            {
                if (vertexHandles.Length != EditableMesh.Positions.Length || faceHandles.Length != EditableMesh.Faces.Length)
                {
                    throw new InvalidDataException("Crystal element pools do not match the mesh.");
                }
                Positions = EditableMesh.Positions;
                var indices = vertexHandles.Select((handle, index) => (handle, index)).ToDictionary(x => x.handle, x => x.index);
                Edges = EditableEdges.Select(edge => new Int2(indices[edge.VertexHandles.X], indices[edge.VertexHandles.Y])).ToArray();
                Faces = new Face[faceHandles.Length];
            }
            TotalEdgeCount = Edges.Length;
            FaceProperties = Resize(FaceProperties, faceHandles.Length);
            FaceFlags = Resize(FaceFlags, faceHandles.Length);
            if (Version < 28) LegacyFaceValues = Resize(LegacyFaceValues, faceHandles.Length);
            for (var i = 0; i < faceHandles.Length; i++)
            {
                if (Version != 28) FaceProperties[i] = rw.Int32(FaceProperties[i]);
                if (Version < 28) LegacyFaceValues[i] = rw.Int32(LegacyFaceValues[i]);
                FaceFlags[i] = rw.Int32(FaceFlags[i]);
                var groupIndex = rw.Writer is null ? 0 : Array.IndexOf(Groups, Faces[i].Group);
                if (Version >= 22) groupIndex = rw.Int32(groupIndex);
                if (rw.Reader is not null)
                {
                    if (Groups.Length == 0) Groups = [new Part { Name = "part", IsInUse = true }];
                    if (groupIndex == -1) groupIndex = 0;
                    var meshFace = EditableMesh.Faces[i];
                    var coords = meshFace.TexCoordLayers.Select(layer => layer.TexCoords).ToArray();
                    var vertices = meshFace.Vertices.Select((vertex, j) => new Vertex(vertex.PositionIndex,
                        coords.Length == 0 ? default : coords[0][j], default)).ToArray();
                    var normalIndex = meshFace.Vertices.FirstOrDefault()?.NormalIndex ?? -1;
                    var normal = normalIndex >= 0 && normalIndex < EditableMesh.Normals.Length
                        ? EditableMesh.Normals[normalIndex] : default(Vec3?);
                    Faces[i] = new Face(vertices, Groups[groupIndex], null, normal) { TexCoordLayers = coords };
                }
            }
            if (Version < 29) VertexValues = rw.Array(VertexValues, vertexHandles.Length)!;
        }

        private void UpdateEditableMesh()
        {
            EditableMesh.Positions = Positions;
            var meshFaces = new CrystalMeshFace[Faces.Length];
            for (var i = 0; i < Faces.Length; i++)
            {
                var face = Faces[i];
                var meshFace = i < EditableMesh.Faces.Length ? EditableMesh.Faces[i] : new CrystalMeshFace();
                if (i >= FaceTypes.Length || meshFace.Vertices.Length != face.Vertices.Length)
                {
                    FaceTypes = Resize(FaceTypes, Faces.Length);
                    FaceTypes[i] = face.Vertices.Length == 3 ? 2 : 0;
                }
                var oldNormalIndex = meshFace.Vertices.FirstOrDefault()?.NormalIndex ?? -1;
                var oldNormal = oldNormalIndex >= 0 && oldNormalIndex < EditableMesh.Normals.Length
                    ? EditableMesh.Normals[oldNormalIndex] : default(Vec3?);
                var normalIndex = -1;
                if (face.Normal is Vec3 normal)
                {
                    normalIndex = Array.IndexOf(EditableMesh.Normals, normal);
                    if (normalIndex == -1)
                    {
                        normalIndex = EditableMesh.Normals.Length;
                        EditableMesh.Normals = [.. EditableMesh.Normals, normal];
                    }
                }
                var vertices = new CrystalMeshVertex[face.Vertices.Length];
                for (var j = 0; j < vertices.Length; j++)
                {
                    vertices[j] = j < meshFace.Vertices.Length ? meshFace.Vertices[j] : new CrystalMeshVertex { NormalIndex = -1, Color = new Vec4(1, 1, 1, 1) };
                    vertices[j].PositionIndex = face.Vertices[j].Index;
                    if (face.Normal != oldNormal) vertices[j].NormalIndex = normalIndex;
                }
                meshFace.Vertices = vertices;
                if (meshFace.TexCoordLayers.Length == 0 && face.Vertices.Any(vertex => vertex.TexCoord != default))
                {
                    meshFace.TexCoordLayers = [new CrystalMeshTexCoordLayer()];
                }
                for (var layer = 0; layer < meshFace.TexCoordLayers.Length; layer++)
                {
                    var coords = layer == 0
                        ? face.Vertices.Select(vertex => vertex.TexCoord).ToArray()
                        : layer < face.TexCoordLayers.Length ? face.TexCoordLayers[layer] : new Vec2[vertices.Length];
                    if (coords.Length != vertices.Length) Array.Resize(ref coords, vertices.Length);
                    meshFace.TexCoordLayers[layer].TexCoords = coords;
                }
                meshFaces[i] = meshFace;
            }
            EditableMesh.Faces = meshFaces;
        }

        private static ElementPool PreparePool(ElementPool pool, int count)
        {
            if (GetUsedHandles(pool).Length == count) return pool;
            return new ElementPool
            {
                FirstUsed = count == 0 ? int.MaxValue : 0,
                Handles = Enumerable.Range(0, count).Select(i => new ElementHandle
                {
                    Next = i + 1 == count ? int.MaxValue : i + 1
                }).ToArray()
            };
        }

        private static int[] GetUsedHandles(ElementPool pool)
        {
            var handles = new List<int>();
            var visited = new HashSet<int>();
            for (var index = pool.FirstUsed; index != int.MaxValue; index = pool.Handles[index].Next)
            {
                if ((uint)index >= (uint)pool.Handles.Length || pool.Handles[index].IsFree || !visited.Add(index))
                {
                    throw new InvalidDataException("Invalid crystal element handle chain.");
                }
                handles.Add((index << 10) | (pool.Handles[index].Generation & 0x3FF));
            }
            if (handles.Count != pool.Handles.Count(handle => !handle.IsFree))
            {
                throw new InvalidDataException("Crystal element handle chain omits used handles.");
            }
            return handles.ToArray();
        }
    }
}
