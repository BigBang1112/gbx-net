using GBX.NET.Engines.Function;

namespace GBX.NET.Engines.Plug;

public partial class CPlugSoundSurface
{
    private MaterialSound[] materialSounds = CreateMaterialSounds(80);
    private CFuncKeysReal? skidVolumeFromIntensity = CreateSkidVolumeFromIntensity(true);

    public CPlugSoundSurface()
    {
    }

    public CPlugSoundSurface(GameVersion gameVersion)
    {
        materialSounds = CreateMaterialSounds(gameVersion switch
        {
            GameVersion.TMSX or GameVersion.TMNESWC => 27,
            GameVersion.VSK5 => 30,
            GameVersion.TMF => 31,
            GameVersion.MP3 => 69,
            GameVersion.TMT => 63,
            GameVersion.MP4 => 75,
            _ => 80
        });
        skidVolumeFromIntensity = CreateSkidVolumeFromIntensity(gameVersion is GameVersion.Unspecified or GameVersion.TM2020);
    }

    private static MaterialSound[] CreateMaterialSounds(int count)
    {
        var sounds = new MaterialSound[count];
        for (var i = 0; i < sounds.Length; i++) sounds[i] = new();
        return sounds;
    }

    private static CFuncKeysReal CreateSkidVolumeFromIntensity(bool simple)
    {
        if (simple)
        {
            var curve = new CPlugCurveSimpleNod
            {
                Curve = new PlugCurve { Values = [new(0, 0), new(0.5f, 0.707f), new(1, 1), new(2, 1.4f)] }
            };
            curve.CreateChunk<CPlugCurveSimpleNod.Chunk09185000>();
            return curve;
        }

        var keys = new CFuncKeysReal
        {
            Xs = [0, 0.5f, 1, 2],
            Ys = [0, 0.707f, 1, 1.4f],
            RealInterp = CFuncKeysReal.ERealInterp.Linear
        };
        keys.CreateChunk<CFuncKeys.Chunk05002001>();
        keys.CreateChunk<CFuncKeysReal.Chunk0501A002>();
        return keys;
    }

    public partial class Chunk0905E003
    {
        public override void ReadWrite(CPlugSoundSurface n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref n.edMaxSpeedKmh);
            var count = rw.Int32(n.MaterialSounds?.Length ?? 0);
            rw.ArrayReadableWritable(ref n.materialSounds, count, version: 1);

            // The direct node body has no class ID. TM2020 uses CPlugCurveSimpleNod.
            if (rw.Reader is not null)
            {
                // Both curve types use CMwNod as the native parent. Initialize before
                // reading the first chunk, which can cross an encrypted block boundary.
                rw.Reader.TryInitializeDecryption(new CFuncKeysReal());
                var firstChunkId = rw.Reader.ReadHexUInt32();
                n.skidVolumeFromIntensity = firstChunkId == 0x09185000
                    ? new CPlugCurveSimpleNod()
                    : new CFuncKeysReal();
                n.skidVolumeFromIntensity.Read(rw, firstChunkId);
            }
            if (rw.Writer is not null)
            {
                rw.Writer.WriteNode(n.SkidVolumeFromIntensity);
            }
        }
    }
}
