namespace GBX.NET.Engines.Control;

public partial class CControlContainer
{
    public partial class Anchor
    {
        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.IdAsString(ref name);
            rw.Int32(ref version);
            var context = new CControlFrame();
            rw.ArrayReadableWritable<Child, CControlContainer>(ref childs, context);
        }
    }

    public partial class Child
    {
        public void Read(GbxReader r, CControlContainer n, int v = 0)
        {
            using var rw = new GbxReaderWriter(r);
            ReadWrite(rw, n, v);
        }

        public void Write(GbxWriter w, CControlContainer n, int v = 0)
        {
            using var rw = new GbxReaderWriter(w);
            ReadWrite(rw, n, v);
        }

        public void ReadWrite(GbxReaderWriter rw, CControlContainer n, int v = 0)
        {
            if (n.HasMenuFrameChildVersion) rw.Int32(ref menuFrameVersion);
            if (n.HasFrameChildLocations) rw.Int32(ref frameVersion);
            if (n.HasGridChildLocations) rw.Int32(ref gridVersion);
            Version = rw.Int32(Version);

            if (Version == 1)
            {
                rw.NodeRef(ref controlRef);
            }
            else if (Version == 2)
            {
                ReadWriteModel(rw);
            }
            else if (Version == 3)
            {
                ReadWritePointer(rw);
            }
            else
            {
                throw new NotSupportedException($"Unsupported control child version {Version}.");
            }

            if (n.HasFrameChildLocations) rw.Iso4(ref relativeLocation);
            if (n.HasGridChildLocations)
            {
                rw.Vec2(ref square);
                if (GridVersion >= 1) rw.Single(ref squareZ);
            }
        }

        internal void ReadWritePointer(GbxReaderWriter rw)
        {
            if (rw.Reader is not null)
            {
                index = rw.Reader.ReadInt32();
                if (index >= 0)
                {
                    if (rw.Reader.NodeDict.TryGetValue(index, out var existing))
                    {
                        instance = existing as ModelInstance
                            ?? throw new InvalidDataException($"Control instance reference {index} has an incompatible type.");
                        return;
                    }

                    instance = new ModelInstance();
                    rw.Reader.NodeDict.Add(index, instance);
                    ReadWriteModel(rw);
                    return;
                }
            }

            if (rw.Writer is not null)
            {
                if (instance is not null)
                {
                    if (rw.Writer.NodeDict.TryGetValue(instance, out var existing))
                    {
                        rw.Writer.Write(existing);
                        return;
                    }

                    rw.Writer.Write(rw.Writer.RegisterNodeReference(instance));
                    ReadWriteModel(rw);
                    return;
                }

                rw.Writer.Write(index == -1 ? -1 : -2);
            }

            if (index != -1) rw.NodeRef(ref controlRef);
        }

        internal void ReadWriteModel(GbxReaderWriter rw)
        {
            instance ??= new ModelInstance();
            rw.NodeRef(instance.References.Model);
            if (instance.References.Model.Kind == NodeRefKind.None)
            {
                rw.NodeRef(ref controlRef);
                return;
            }

            instance.Data ??= new InstanceData();
            var isContainer = instance.References.Model.CachedNode is CControlContainer
                || instance.References.Model.File?.FilePath.EndsWith(".Frame.Gbx", StringComparison.OrdinalIgnoreCase) == true;
            var instanceVersion = instance.References.Model.CachedNode is CGameMenuFrame ? 2 : isContainer ? 1 : 0;
            instance.Data.ReadWrite(rw, instanceVersion);
        }
    }

    public partial class InstanceData
    {
        public void Read(GbxReader r, int v = 0)
        {
            using var rw = new GbxReaderWriter(r);
            ReadWrite(rw, v);
        }

        public void Write(GbxWriter w, int v = 0)
        {
            using var rw = new GbxReaderWriter(w);
            ReadWrite(rw, v);
        }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            Version = rw.Int32(Version);
            if (Version >= 2)
            {
                if (rw.Reader is not null)
                {
                    var records = new List<OwnDataRecord>();
                    for (var classId = rw.UInt32(); classId != uint.MaxValue; classId = rw.UInt32())
                    {
                        var record = new OwnDataRecord { ClassId = classId, Version = rw.Int32() };
                        if (Managers.ClassManager.Wrap(classId) != 0x030BD000)
                        {
                            throw new NotSupportedException($"Unsupported control instance class 0x{classId:X8}.");
                        }
                        if (record.Version != 2)
                        {
                            var localVersion = record.Version < 4 ? 0 : record.Version - 3;
                            if (localVersion >= 1) localVersion = rw.Byte();
                            record.Data = rw.ReadableWritable<Engines.Game.CGameControlCardGeneric.LocalData>(null, version: localVersion);
                        }
                        records.Add(record);
                    }
                    ownDataRecords = records.ToArray();
                }

                if (rw.Writer is not null)
                {
                    foreach (var record in OwnDataRecords ?? [])
                    {
                        rw.Writer.Write(record.ClassId);
                        rw.Writer.Write(record.Version);
                        if (record.Version != 2)
                        {
                            var localVersion = record.Version < 4 ? 0 : record.Version - 3;
                            if (localVersion >= 1) rw.Writer.Write((byte)localVersion);
                            rw.ReadableWritable(record.Data, version: localVersion);
                        }
                    }
                    rw.Writer.Write(uint.MaxValue);
                }
            }

            rw.IdAsString(ref name);
            if (Version >= 1) rw.ListNodeRef(ref links);
            if (v == 0) return;

            rw.Int32(ref containerVersion);
            var context = new CControlFrame();
            if (rw.Reader is not null)
            {
                var count = rw.Reader.ReadInt32();
                if (count < 0) throw new InvalidDataException("Negative control anchor count.");
                anchors = new Anchor[count];
                for (var i = 0; i < count; i++)
                {
                    var anchor = new Anchor { Name = rw.IdAsString(), Version = rw.Int32() };
                    anchor.Childs = rw.ArrayReadableWritable<Child, CControlContainer>(null, context);
                    anchors[i] = anchor;
                }
            }
            if (rw.Writer is not null)
            {
                rw.Writer.Write(Anchors?.Length ?? 0);
                foreach (var anchor in Anchors ?? [])
                {
                    rw.IdAsString(anchor.Name);
                    rw.Int32(anchor.Version);
                    rw.ArrayReadableWritable<Child, CControlContainer>(anchor.Childs, context);
                }
            }

            if (ContainerVersion >= 3 && rw.Reader is not null)
            {
                var records = new List<TerminalDataRecord>();
                for (var id = rw.IdAsString(); !string.IsNullOrEmpty(id); id = rw.IdAsString())
                {
                    records.Add(new TerminalDataRecord { Name = id, Version = rw.Byte(), Data = rw.Data() });
                }
                terminalData = records.ToArray();
            }
            if (ContainerVersion >= 3 && rw.Writer is not null)
            {
                foreach (var record in TerminalData ?? [])
                {
                    rw.IdAsString(record.Name);
                    rw.Byte(record.Version);
                    rw.Data(record.Data);
                }
                rw.IdAsString("");
            }

            if (v >= 2) rw.ReadableWritable(ref menuFrameData);
        }
    }
}
