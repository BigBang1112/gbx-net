namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03039000</remarks>
[Class(0x03039000)]
public partial class CGameCtnDecorationAudio : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03039000;




    private CPlugAudioEnvironment? audioEnvOutsideOpen;
    [AppliedWithChunk<Chunk03039001>]
    [AppliedWithChunk<Chunk03039004>]
    public CPlugAudioEnvironment? AudioEnvOutsideOpen { get => audioEnvOutsideOpenFile?.GetNode(ref audioEnvOutsideOpen) ?? audioEnvOutsideOpen; set => audioEnvOutsideOpen = value; }
    private Components.GbxRefTableFile? audioEnvOutsideOpenFile;
    public Components.GbxRefTableFile? AudioEnvOutsideOpenFile { get => audioEnvOutsideOpenFile; set => audioEnvOutsideOpenFile = value; }
    public CPlugAudioEnvironment? GetAudioEnvOutsideOpen(GbxReadSettings settings = default, bool exceptions = false) => audioEnvOutsideOpenFile?.GetNode(ref audioEnvOutsideOpen, settings, exceptions) ?? audioEnvOutsideOpen;

    private CPlugAudioEnvironment? audioEnvOutsideEnclosed;
    [AppliedWithChunk<Chunk03039001>]
    [AppliedWithChunk<Chunk03039004>]
    public CPlugAudioEnvironment? AudioEnvOutsideEnclosed { get => audioEnvOutsideEnclosedFile?.GetNode(ref audioEnvOutsideEnclosed) ?? audioEnvOutsideEnclosed; set => audioEnvOutsideEnclosed = value; }
    private Components.GbxRefTableFile? audioEnvOutsideEnclosedFile;
    public Components.GbxRefTableFile? AudioEnvOutsideEnclosedFile { get => audioEnvOutsideEnclosedFile; set => audioEnvOutsideEnclosedFile = value; }
    public CPlugAudioEnvironment? GetAudioEnvOutsideEnclosed(GbxReadSettings settings = default, bool exceptions = false) => audioEnvOutsideEnclosedFile?.GetNode(ref audioEnvOutsideEnclosed, settings, exceptions) ?? audioEnvOutsideEnclosed;

    private float soundAttenuationInEditor;
    [AppliedWithChunk<Chunk03039002>]
    public float SoundAttenuationInEditor { get => soundAttenuationInEditor; set => soundAttenuationInEditor = value; }

    private CPlugAudioBalance? audioBalance_PlaygroundLoud;
    [AppliedWithChunk<Chunk03039003>]
    [AppliedWithChunk<Chunk03039003>]
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_PlaygroundLoud { get => audioBalance_PlaygroundLoudFile?.GetNode(ref audioBalance_PlaygroundLoud) ?? audioBalance_PlaygroundLoud; set => audioBalance_PlaygroundLoud = value; }
    private Components.GbxRefTableFile? audioBalance_PlaygroundLoudFile;
    public Components.GbxRefTableFile? AudioBalance_PlaygroundLoudFile { get => audioBalance_PlaygroundLoudFile; set => audioBalance_PlaygroundLoudFile = value; }
    public CPlugAudioBalance? GetAudioBalance_PlaygroundLoud(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_PlaygroundLoudFile?.GetNode(ref audioBalance_PlaygroundLoud, settings, exceptions) ?? audioBalance_PlaygroundLoud;

    private CPlugAudioBalance? audioBalance_ReplayLoud;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_ReplayLoud { get => audioBalance_ReplayLoudFile?.GetNode(ref audioBalance_ReplayLoud) ?? audioBalance_ReplayLoud; set => audioBalance_ReplayLoud = value; }
    private Components.GbxRefTableFile? audioBalance_ReplayLoudFile;
    public Components.GbxRefTableFile? AudioBalance_ReplayLoudFile { get => audioBalance_ReplayLoudFile; set => audioBalance_ReplayLoudFile = value; }
    public CPlugAudioBalance? GetAudioBalance_ReplayLoud(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_ReplayLoudFile?.GetNode(ref audioBalance_ReplayLoud, settings, exceptions) ?? audioBalance_ReplayLoud;

    private CPlugAudioBalance? audioBalance_Podium;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_Podium { get => audioBalance_PodiumFile?.GetNode(ref audioBalance_Podium) ?? audioBalance_Podium; set => audioBalance_Podium = value; }
    private Components.GbxRefTableFile? audioBalance_PodiumFile;
    public Components.GbxRefTableFile? AudioBalance_PodiumFile { get => audioBalance_PodiumFile; set => audioBalance_PodiumFile = value; }
    public CPlugAudioBalance? GetAudioBalance_Podium(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_PodiumFile?.GetNode(ref audioBalance_Podium, settings, exceptions) ?? audioBalance_Podium;

    private CPlugAudioBalance? audioBalance_PlaygroundSoft;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_PlaygroundSoft { get => audioBalance_PlaygroundSoftFile?.GetNode(ref audioBalance_PlaygroundSoft) ?? audioBalance_PlaygroundSoft; set => audioBalance_PlaygroundSoft = value; }
    private Components.GbxRefTableFile? audioBalance_PlaygroundSoftFile;
    public Components.GbxRefTableFile? AudioBalance_PlaygroundSoftFile { get => audioBalance_PlaygroundSoftFile; set => audioBalance_PlaygroundSoftFile = value; }
    public CPlugAudioBalance? GetAudioBalance_PlaygroundSoft(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_PlaygroundSoftFile?.GetNode(ref audioBalance_PlaygroundSoft, settings, exceptions) ?? audioBalance_PlaygroundSoft;

    private CPlugAudioBalance? audioBalance_ReplaySoft;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_ReplaySoft { get => audioBalance_ReplaySoftFile?.GetNode(ref audioBalance_ReplaySoft) ?? audioBalance_ReplaySoft; set => audioBalance_ReplaySoft = value; }
    private Components.GbxRefTableFile? audioBalance_ReplaySoftFile;
    public Components.GbxRefTableFile? AudioBalance_ReplaySoftFile { get => audioBalance_ReplaySoftFile; set => audioBalance_ReplaySoftFile = value; }
    public CPlugAudioBalance? GetAudioBalance_ReplaySoft(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_ReplaySoftFile?.GetNode(ref audioBalance_ReplaySoft, settings, exceptions) ?? audioBalance_ReplaySoft;

    private CPlugAudioBalance? audioBalance_TM_EvtStartLine;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_TM_EvtStartLine { get => audioBalance_TM_EvtStartLineFile?.GetNode(ref audioBalance_TM_EvtStartLine) ?? audioBalance_TM_EvtStartLine; set => audioBalance_TM_EvtStartLine = value; }
    private Components.GbxRefTableFile? audioBalance_TM_EvtStartLineFile;
    public Components.GbxRefTableFile? AudioBalance_TM_EvtStartLineFile { get => audioBalance_TM_EvtStartLineFile; set => audioBalance_TM_EvtStartLineFile = value; }
    public CPlugAudioBalance? GetAudioBalance_TM_EvtStartLine(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_TM_EvtStartLineFile?.GetNode(ref audioBalance_TM_EvtStartLine, settings, exceptions) ?? audioBalance_TM_EvtStartLine;

    private CPlugAudioBalance? audioBalance_TM_EvtCheckpoint;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_TM_EvtCheckpoint { get => audioBalance_TM_EvtCheckpointFile?.GetNode(ref audioBalance_TM_EvtCheckpoint) ?? audioBalance_TM_EvtCheckpoint; set => audioBalance_TM_EvtCheckpoint = value; }
    private Components.GbxRefTableFile? audioBalance_TM_EvtCheckpointFile;
    public Components.GbxRefTableFile? AudioBalance_TM_EvtCheckpointFile { get => audioBalance_TM_EvtCheckpointFile; set => audioBalance_TM_EvtCheckpointFile = value; }
    public CPlugAudioBalance? GetAudioBalance_TM_EvtCheckpoint(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_TM_EvtCheckpointFile?.GetNode(ref audioBalance_TM_EvtCheckpoint, settings, exceptions) ?? audioBalance_TM_EvtCheckpoint;

    private CPlugAudioBalance? audioBalance_TM_EvtRespawn;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_TM_EvtRespawn { get => audioBalance_TM_EvtRespawnFile?.GetNode(ref audioBalance_TM_EvtRespawn) ?? audioBalance_TM_EvtRespawn; set => audioBalance_TM_EvtRespawn = value; }
    private Components.GbxRefTableFile? audioBalance_TM_EvtRespawnFile;
    public Components.GbxRefTableFile? AudioBalance_TM_EvtRespawnFile { get => audioBalance_TM_EvtRespawnFile; set => audioBalance_TM_EvtRespawnFile = value; }
    public CPlugAudioBalance? GetAudioBalance_TM_EvtRespawn(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_TM_EvtRespawnFile?.GetNode(ref audioBalance_TM_EvtRespawn, settings, exceptions) ?? audioBalance_TM_EvtRespawn;

    private CPlugAudioBalance? audioBalance_TM_EvtCrash;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_TM_EvtCrash { get => audioBalance_TM_EvtCrashFile?.GetNode(ref audioBalance_TM_EvtCrash) ?? audioBalance_TM_EvtCrash; set => audioBalance_TM_EvtCrash = value; }
    private Components.GbxRefTableFile? audioBalance_TM_EvtCrashFile;
    public Components.GbxRefTableFile? AudioBalance_TM_EvtCrashFile { get => audioBalance_TM_EvtCrashFile; set => audioBalance_TM_EvtCrashFile = value; }
    public CPlugAudioBalance? GetAudioBalance_TM_EvtCrash(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_TM_EvtCrashFile?.GetNode(ref audioBalance_TM_EvtCrash, settings, exceptions) ?? audioBalance_TM_EvtCrash;

    private CPlugAudioBalance? audioBalance_TM_EvtFlying;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_TM_EvtFlying { get => audioBalance_TM_EvtFlyingFile?.GetNode(ref audioBalance_TM_EvtFlying) ?? audioBalance_TM_EvtFlying; set => audioBalance_TM_EvtFlying = value; }
    private Components.GbxRefTableFile? audioBalance_TM_EvtFlyingFile;
    public Components.GbxRefTableFile? AudioBalance_TM_EvtFlyingFile { get => audioBalance_TM_EvtFlyingFile; set => audioBalance_TM_EvtFlyingFile = value; }
    public CPlugAudioBalance? GetAudioBalance_TM_EvtFlying(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_TM_EvtFlyingFile?.GetNode(ref audioBalance_TM_EvtFlying, settings, exceptions) ?? audioBalance_TM_EvtFlying;

    private CPlugAudioBalance? audioBalance_TM_EvtSwimming;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_TM_EvtSwimming { get => audioBalance_TM_EvtSwimmingFile?.GetNode(ref audioBalance_TM_EvtSwimming) ?? audioBalance_TM_EvtSwimming; set => audioBalance_TM_EvtSwimming = value; }
    private Components.GbxRefTableFile? audioBalance_TM_EvtSwimmingFile;
    public Components.GbxRefTableFile? AudioBalance_TM_EvtSwimmingFile { get => audioBalance_TM_EvtSwimmingFile; set => audioBalance_TM_EvtSwimmingFile = value; }
    public CPlugAudioBalance? GetAudioBalance_TM_EvtSwimming(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_TM_EvtSwimmingFile?.GetNode(ref audioBalance_TM_EvtSwimming, settings, exceptions) ?? audioBalance_TM_EvtSwimming;

    private CPlugAudioBalance? audioBalance_SM_EvtSpawn;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_SM_EvtSpawn { get => audioBalance_SM_EvtSpawnFile?.GetNode(ref audioBalance_SM_EvtSpawn) ?? audioBalance_SM_EvtSpawn; set => audioBalance_SM_EvtSpawn = value; }
    private Components.GbxRefTableFile? audioBalance_SM_EvtSpawnFile;
    public Components.GbxRefTableFile? AudioBalance_SM_EvtSpawnFile { get => audioBalance_SM_EvtSpawnFile; set => audioBalance_SM_EvtSpawnFile = value; }
    public CPlugAudioBalance? GetAudioBalance_SM_EvtSpawn(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_SM_EvtSpawnFile?.GetNode(ref audioBalance_SM_EvtSpawn, settings, exceptions) ?? audioBalance_SM_EvtSpawn;

    private CPlugAudioBalance? audioBalance_Overlay_Underground;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_Overlay_Underground { get => audioBalance_Overlay_UndergroundFile?.GetNode(ref audioBalance_Overlay_Underground) ?? audioBalance_Overlay_Underground; set => audioBalance_Overlay_Underground = value; }
    private Components.GbxRefTableFile? audioBalance_Overlay_UndergroundFile;
    public Components.GbxRefTableFile? AudioBalance_Overlay_UndergroundFile { get => audioBalance_Overlay_UndergroundFile; set => audioBalance_Overlay_UndergroundFile = value; }
    public CPlugAudioBalance? GetAudioBalance_Overlay_Underground(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_Overlay_UndergroundFile?.GetNode(ref audioBalance_Overlay_Underground, settings, exceptions) ?? audioBalance_Overlay_Underground;

    private CPlugAudioBalance? audioBalance_Overlay_Far;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_Overlay_Far { get => audioBalance_Overlay_FarFile?.GetNode(ref audioBalance_Overlay_Far) ?? audioBalance_Overlay_Far; set => audioBalance_Overlay_Far = value; }
    private Components.GbxRefTableFile? audioBalance_Overlay_FarFile;
    public Components.GbxRefTableFile? AudioBalance_Overlay_FarFile { get => audioBalance_Overlay_FarFile; set => audioBalance_Overlay_FarFile = value; }
    public CPlugAudioBalance? GetAudioBalance_Overlay_Far(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_Overlay_FarFile?.GetNode(ref audioBalance_Overlay_Far, settings, exceptions) ?? audioBalance_Overlay_Far;

    private CPlugAudioBalance? audioBalance_SM_EvtUnspawn;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_SM_EvtUnspawn { get => audioBalance_SM_EvtUnspawnFile?.GetNode(ref audioBalance_SM_EvtUnspawn) ?? audioBalance_SM_EvtUnspawn; set => audioBalance_SM_EvtUnspawn = value; }
    private Components.GbxRefTableFile? audioBalance_SM_EvtUnspawnFile;
    public Components.GbxRefTableFile? AudioBalance_SM_EvtUnspawnFile { get => audioBalance_SM_EvtUnspawnFile; set => audioBalance_SM_EvtUnspawnFile = value; }
    public CPlugAudioBalance? GetAudioBalance_SM_EvtUnspawn(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_SM_EvtUnspawnFile?.GetNode(ref audioBalance_SM_EvtUnspawn, settings, exceptions) ?? audioBalance_SM_EvtUnspawn;

    private CPlugAudioBalance? audioBalance_SM_EvtHit;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_SM_EvtHit { get => audioBalance_SM_EvtHitFile?.GetNode(ref audioBalance_SM_EvtHit) ?? audioBalance_SM_EvtHit; set => audioBalance_SM_EvtHit = value; }
    private Components.GbxRefTableFile? audioBalance_SM_EvtHitFile;
    public Components.GbxRefTableFile? AudioBalance_SM_EvtHitFile { get => audioBalance_SM_EvtHitFile; set => audioBalance_SM_EvtHitFile = value; }
    public CPlugAudioBalance? GetAudioBalance_SM_EvtHit(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_SM_EvtHitFile?.GetNode(ref audioBalance_SM_EvtHit, settings, exceptions) ?? audioBalance_SM_EvtHit;

    private CPlugAudioBalance? audioBalance_SM_EvtFire;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_SM_EvtFire { get => audioBalance_SM_EvtFireFile?.GetNode(ref audioBalance_SM_EvtFire) ?? audioBalance_SM_EvtFire; set => audioBalance_SM_EvtFire = value; }
    private Components.GbxRefTableFile? audioBalance_SM_EvtFireFile;
    public Components.GbxRefTableFile? AudioBalance_SM_EvtFireFile { get => audioBalance_SM_EvtFireFile; set => audioBalance_SM_EvtFireFile = value; }
    public CPlugAudioBalance? GetAudioBalance_SM_EvtFire(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_SM_EvtFireFile?.GetNode(ref audioBalance_SM_EvtFire, settings, exceptions) ?? audioBalance_SM_EvtFire;

    private CPlugAudioBalance? audioBalance_SM_EvtHitEliminated;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_SM_EvtHitEliminated { get => audioBalance_SM_EvtHitEliminatedFile?.GetNode(ref audioBalance_SM_EvtHitEliminated) ?? audioBalance_SM_EvtHitEliminated; set => audioBalance_SM_EvtHitEliminated = value; }
    private Components.GbxRefTableFile? audioBalance_SM_EvtHitEliminatedFile;
    public Components.GbxRefTableFile? AudioBalance_SM_EvtHitEliminatedFile { get => audioBalance_SM_EvtHitEliminatedFile; set => audioBalance_SM_EvtHitEliminatedFile = value; }
    public CPlugAudioBalance? GetAudioBalance_SM_EvtHitEliminated(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_SM_EvtHitEliminatedFile?.GetNode(ref audioBalance_SM_EvtHitEliminated, settings, exceptions) ?? audioBalance_SM_EvtHitEliminated;

    private CPlugAudioBalance? audioBalance_SM_EvtBulletTime;
    [AppliedWithChunk<Chunk03039003>]
    public CPlugAudioBalance? AudioBalance_SM_EvtBulletTime { get => audioBalance_SM_EvtBulletTimeFile?.GetNode(ref audioBalance_SM_EvtBulletTime) ?? audioBalance_SM_EvtBulletTime; set => audioBalance_SM_EvtBulletTime = value; }
    private Components.GbxRefTableFile? audioBalance_SM_EvtBulletTimeFile;
    public Components.GbxRefTableFile? AudioBalance_SM_EvtBulletTimeFile { get => audioBalance_SM_EvtBulletTimeFile; set => audioBalance_SM_EvtBulletTimeFile = value; }
    public CPlugAudioBalance? GetAudioBalance_SM_EvtBulletTime(GbxReadSettings settings = default, bool exceptions = false) => audioBalance_SM_EvtBulletTimeFile?.GetNode(ref audioBalance_SM_EvtBulletTime, settings, exceptions) ?? audioBalance_SM_EvtBulletTime;

    private CPlugAudioEnvironment? audioEnvUndergroundOpen;
    [AppliedWithChunk<Chunk03039004>]
    public CPlugAudioEnvironment? AudioEnvUndergroundOpen { get => audioEnvUndergroundOpenFile?.GetNode(ref audioEnvUndergroundOpen) ?? audioEnvUndergroundOpen; set => audioEnvUndergroundOpen = value; }
    private Components.GbxRefTableFile? audioEnvUndergroundOpenFile;
    public Components.GbxRefTableFile? AudioEnvUndergroundOpenFile { get => audioEnvUndergroundOpenFile; set => audioEnvUndergroundOpenFile = value; }
    public CPlugAudioEnvironment? GetAudioEnvUndergroundOpen(GbxReadSettings settings = default, bool exceptions = false) => audioEnvUndergroundOpenFile?.GetNode(ref audioEnvUndergroundOpen, settings, exceptions) ?? audioEnvUndergroundOpen;

    private CPlugAudioEnvironment? audioEnvUndergroundEnclosed;
    [AppliedWithChunk<Chunk03039004>]
    public CPlugAudioEnvironment? AudioEnvUndergroundEnclosed { get => audioEnvUndergroundEnclosedFile?.GetNode(ref audioEnvUndergroundEnclosed) ?? audioEnvUndergroundEnclosed; set => audioEnvUndergroundEnclosed = value; }
    private Components.GbxRefTableFile? audioEnvUndergroundEnclosedFile;
    public Components.GbxRefTableFile? AudioEnvUndergroundEnclosedFile { get => audioEnvUndergroundEnclosedFile; set => audioEnvUndergroundEnclosedFile = value; }
    public CPlugAudioEnvironment? GetAudioEnvUndergroundEnclosed(GbxReadSettings settings = default, bool exceptions = false) => audioEnvUndergroundEnclosedFile?.GetNode(ref audioEnvUndergroundEnclosed, settings, exceptions) ?? audioEnvUndergroundEnclosed;

    private float cameraWooshMinSpeedKmh;
    [AppliedWithChunk<Chunk03039005>]
    public float CameraWooshMinSpeedKmh { get => cameraWooshMinSpeedKmh; set => cameraWooshMinSpeedKmh = value; }

    private float reverbMinBlockDist;
    [AppliedWithChunk<Chunk03039005>]
    public float ReverbMinBlockDist { get => reverbMinBlockDist; set => reverbMinBlockDist = value; }

    private float reverbMaxBlockDist;
    [AppliedWithChunk<Chunk03039005>]
    public float ReverbMaxBlockDist { get => reverbMaxBlockDist; set => reverbMaxBlockDist = value; }

    private float[]? reverbMaterialGains;
    [AppliedWithChunk<Chunk03039005>]
    public float[]? ReverbMaterialGains { get => reverbMaterialGains; set => reverbMaterialGains = value; }

    private CPlugFileText? modifierXmlFile;
    [AppliedWithChunk<Chunk03039006>]
    public CPlugFileText? ModifierXmlFile { get => modifierXmlFileFile?.GetNode(ref modifierXmlFile) ?? modifierXmlFile; set => modifierXmlFile = value; }
    private Components.GbxRefTableFile? modifierXmlFileFile;
    public Components.GbxRefTableFile? ModifierXmlFileFile { get => modifierXmlFileFile; set => modifierXmlFileFile = value; }
    public CPlugFileText? GetModifierXmlFile(GbxReadSettings settings = default, bool exceptions = false) => modifierXmlFileFile?.GetNode(ref modifierXmlFile, settings, exceptions) ?? modifierXmlFile;


    /// <summary>
    /// CGameCtnDecorationAudio 0x000 chunk
    /// </summary>
    [Chunk(0x03039000)]
    public partial class Chunk03039000 : Chunk<CGameCtnDecorationAudio>
    {
        /// <inheritdoc />
        public override uint Id => 0x03039000;

    }

    /// <summary>
    /// CGameCtnDecorationAudio 0x001 chunk
    /// </summary>
    [Chunk(0x03039001)]
    public partial class Chunk03039001 : Chunk<CGameCtnDecorationAudio>
    {
        /// <inheritdoc />
        public override uint Id => 0x03039001;


        public override void ReadWrite(CGameCtnDecorationAudio n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugAudioEnvironment>(ref n.audioEnvOutsideOpen, ref n.audioEnvOutsideOpenFile);
            rw.NodeRef<CPlugAudioEnvironment>(ref n.audioEnvOutsideEnclosed, ref n.audioEnvOutsideEnclosedFile);
        }
    }

    /// <summary>
    /// CGameCtnDecorationAudio 0x002 chunk
    /// </summary>
    [Chunk(0x03039002)]
    public partial class Chunk03039002 : Chunk<CGameCtnDecorationAudio>
    {
        /// <inheritdoc />
        public override uint Id => 0x03039002;


        public override void ReadWrite(CGameCtnDecorationAudio n, GbxReaderWriter rw)
        {
            rw.Single(ref n.soundAttenuationInEditor);
        }
    }

    /// <summary>
    /// CGameCtnDecorationAudio 0x003 chunk
    /// </summary>
    [Chunk(0x03039003)]
    public partial class Chunk03039003 : Chunk<CGameCtnDecorationAudio>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03039003;

        public int Version { get; set; }

        public CPlugAudioBalance? U01;
        public CPlugAudioBalance? U02;
        public CPlugAudioBalance? U03;
        public CPlugAudioBalance? U04;
        public CPlugAudioBalance? U05;
        public CPlugAudioBalance? U06;
        public CPlugAudioBalance? U07;

        public override void ReadWrite(CGameCtnDecorationAudio n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 1)
            {
                if (Version <= 3)
                {
                    rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_PlaygroundLoud, ref n.audioBalance_PlaygroundLoudFile);
                    rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_PlaygroundLoud, ref n.audioBalance_PlaygroundLoudFile);
                }
            }
            rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_PlaygroundLoud, ref n.audioBalance_PlaygroundLoudFile);
            rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_ReplayLoud, ref n.audioBalance_ReplayLoudFile);
            rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_Podium, ref n.audioBalance_PodiumFile);
            if (Version >= 6)
            {
                rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_PlaygroundSoft, ref n.audioBalance_PlaygroundSoftFile);
                rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_ReplaySoft, ref n.audioBalance_ReplaySoftFile);
            }
            rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_TM_EvtStartLine, ref n.audioBalance_TM_EvtStartLineFile);
            rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_TM_EvtCheckpoint, ref n.audioBalance_TM_EvtCheckpointFile);
            rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_TM_EvtRespawn, ref n.audioBalance_TM_EvtRespawnFile);
            rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_TM_EvtCrash, ref n.audioBalance_TM_EvtCrashFile);
            rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_TM_EvtFlying, ref n.audioBalance_TM_EvtFlyingFile);
            if (Version >= 7)
            {
                rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_TM_EvtSwimming, ref n.audioBalance_TM_EvtSwimmingFile);
            }
            rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_SM_EvtSpawn, ref n.audioBalance_SM_EvtSpawnFile);
            if (Version >= 2)
            {
                rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_Overlay_Underground, ref n.audioBalance_Overlay_UndergroundFile);
                rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_Overlay_Far, ref n.audioBalance_Overlay_FarFile);
                rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_SM_EvtUnspawn, ref n.audioBalance_SM_EvtUnspawnFile);
                rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_SM_EvtHit, ref n.audioBalance_SM_EvtHitFile);
                if (Version >= 3)
                {
                    rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_SM_EvtFire, ref n.audioBalance_SM_EvtFireFile);
                    if (Version >= 4)
                    {
                        rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_SM_EvtHitEliminated, ref n.audioBalance_SM_EvtHitEliminatedFile);
                        if (Version >= 5)
                        {
                            if (Version == 5)
                            {
                                rw.NodeRef<CPlugAudioBalance>(ref U01);
                                rw.NodeRef<CPlugAudioBalance>(ref U02);
                                rw.NodeRef<CPlugAudioBalance>(ref U03);
                                rw.NodeRef<CPlugAudioBalance>(ref U04);
                                rw.NodeRef<CPlugAudioBalance>(ref U05);
                                rw.NodeRef<CPlugAudioBalance>(ref U06);
                            }
                            if (Version >= 6)
                            {
                                rw.NodeRef<CPlugAudioBalance>(ref U07);
                                if (Version >= 8)
                                {
                                    rw.NodeRef<CPlugAudioBalance>(ref n.audioBalance_SM_EvtBulletTime, ref n.audioBalance_SM_EvtBulletTimeFile);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnDecorationAudio 0x004 chunk
    /// </summary>
    [Chunk(0x03039004)]
    public partial class Chunk03039004 : Chunk<CGameCtnDecorationAudio>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03039004;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnDecorationAudio n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugAudioEnvironment>(ref n.audioEnvOutsideOpen, ref n.audioEnvOutsideOpenFile);
            rw.NodeRef<CPlugAudioEnvironment>(ref n.audioEnvOutsideEnclosed, ref n.audioEnvOutsideEnclosedFile);
            rw.NodeRef<CPlugAudioEnvironment>(ref n.audioEnvUndergroundOpen, ref n.audioEnvUndergroundOpenFile);
            rw.NodeRef<CPlugAudioEnvironment>(ref n.audioEnvUndergroundEnclosed, ref n.audioEnvUndergroundEnclosedFile);
        }
    }

    /// <summary>
    /// CGameCtnDecorationAudio 0x005 chunk
    /// </summary>
    [Chunk(0x03039005)]
    public partial class Chunk03039005 : Chunk<CGameCtnDecorationAudio>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03039005;

        public int Version { get; set; }

        public float U01;

        public override void ReadWrite(CGameCtnDecorationAudio n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.Single(ref n.cameraWooshMinSpeedKmh);
            rw.Single(ref n.reverbMinBlockDist);
            rw.Single(ref n.reverbMaxBlockDist);
            rw.Array<float>(ref n.reverbMaterialGains!);
        }
    }

    /// <summary>
    /// CGameCtnDecorationAudio 0x006 chunk
    /// </summary>
    [Chunk(0x03039006)]
    public partial class Chunk03039006 : Chunk<CGameCtnDecorationAudio>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03039006;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnDecorationAudio n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugFileText>(ref n.modifierXmlFile, ref n.modifierXmlFileFile);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03039000 => new Chunk03039000(),
        0x03039001 => new Chunk03039001(),
        0x03039002 => new Chunk03039002(),
        0x03039003 => new Chunk03039003(),
        0x03039004 => new Chunk03039004(),
        0x03039005 => new Chunk03039005(),
        0x03039006 => new Chunk03039006(),
        _ => base.NewChunk(chunkId),
    };
}
