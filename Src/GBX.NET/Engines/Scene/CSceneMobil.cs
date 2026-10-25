using GBX.NET.Components;
using GBX.NET.Engines.Control;
using GBX.NET.Exceptions;

namespace GBX.NET.Engines.Scene;

public partial class CSceneMobil
{
    public partial class Chunk0A011005
    {
        public override void ReadWrite(CSceneMobil n, GbxReaderWriter rw)
        {
            if (n is CControlBase control)
            {
                control.HasHmsItem = rw.Boolean(control.HasHmsItem);
                if (!control.HasHmsItem)
                {
                    return;
                }
            }

            rw.Node(ref n.item);
        }
    }

    public partial class MobilPtr
    {
        // TM10 and TMPU containers append their own instance data after the mobil instance,
        // and menu frames append theirs after that. The model class decides which data follows.
        internal bool IsContainerModel => ModelType is Type type
            ? typeof(CControlContainer).IsAssignableFrom(type)
            : ModelFileTypes().All(static type => typeof(CControlContainer).IsAssignableFrom(type)) && ModelFileTypes().Any();

        internal bool IsMenuFrameModel => ModelType is Type type && typeof(Game.CGameMenuFrame).IsAssignableFrom(type);

        private Type? ModelType => modelRef?.Kind switch
        {
            NodeRefKind.Embedded => modelRef.CachedNode?.GetType(),
            NodeRefKind.ExternalFile => modelRef.Descriptor is GbxRefTableFile file ? GetExternalModelType(file) : null,
            _ => null,
        };

        // Only the class ID of an external model is read, because its body is not needed here.
        private static Type? GetExternalModelType(GbxRefTableFile file)
        {
            if (file.RefTable.FileSystemPath is null)
            {
                return null;
            }

            var path = file.RefTable.GetFullFilePath(file);

            if (!File.Exists(path))
            {
                return null;
            }

            try
            {
                return Managers.ClassManager.GetType(Gbx.ParseClassId(path));
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or NotAGbxException)
            {
                return null;
            }
        }

        // Without the external file, the extension is used when every class sharing it agrees.
        private IEnumerable<Type> ModelFileTypes()
        {
            if (modelRef?.Kind != NodeRefKind.ExternalFile || modelRef.Descriptor is not GbxRefTableFile file)
            {
                yield break;
            }

            foreach (var pair in Managers.ClassManager.ClassIds)
            {
                foreach (var extension in Managers.ClassManager.GetFileExtensions(pair.Value))
                {
                    if (file.FilePath.EndsWith('.' + extension, StringComparison.OrdinalIgnoreCase))
                    {
                        yield return pair.Key;
                        break;
                    }
                }
            }
        }
    }
}
