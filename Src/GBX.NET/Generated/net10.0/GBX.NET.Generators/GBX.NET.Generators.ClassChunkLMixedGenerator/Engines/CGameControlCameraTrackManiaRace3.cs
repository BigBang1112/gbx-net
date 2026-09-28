namespace GBX.NET.Engines.TrackMania;

/// <remarks>ID: 0x24087000</remarks>
[Class(0x24087000)]
public partial class CGameControlCameraTrackManiaRace3 : CGameControlCameraTarget, IClass
{
    [Hexadecimal] public static new uint Id => 0x24087000;




    private float slerpSpeed;
    [AppliedWithChunk<Chunk24087000>]
    public float SlerpSpeed { get => slerpSpeed; set => slerpSpeed = value; }

    private float up;
    [AppliedWithChunk<Chunk24087000>]
    public float Up { get => up; set => up = value; }

    private float far;
    [AppliedWithChunk<Chunk24087000>]
    public float Far { get => far; set => far = value; }

    private int flyDurationBeforeFlyingBehavior;
    [AppliedWithChunk<Chunk24087000>]
    public int FlyDurationBeforeFlyingBehavior { get => flyDurationBeforeFlyingBehavior; set => flyDurationBeforeFlyingBehavior = value; }

    private int inputNoSteerDurationBeforeReset;
    [AppliedWithChunk<Chunk24087000>]
    public int InputNoSteerDurationBeforeReset { get => inputNoSteerDurationBeforeReset; set => inputNoSteerDurationBeforeReset = value; }

    private int inputSteerDurationBeforeBurnoutShowView;
    [AppliedWithChunk<Chunk24087000>]
    public int InputSteerDurationBeforeBurnoutShowView { get => inputSteerDurationBeforeBurnoutShowView; set => inputSteerDurationBeforeBurnoutShowView = value; }

    private float minSpeed;
    [AppliedWithChunk<Chunk24087000>]
    public float MinSpeed { get => minSpeed; set => minSpeed = value; }

    private float minSpeed2;
    [AppliedWithChunk<Chunk24087000>]
    public float MinSpeed2 { get => minSpeed2; set => minSpeed2 = value; }

    private float slerpTargetPosNormalBehaviorDelta;
    [AppliedWithChunk<Chunk24087000>]
    public float SlerpTargetPosNormalBehaviorDelta { get => slerpTargetPosNormalBehaviorDelta; set => slerpTargetPosNormalBehaviorDelta = value; }

    private int slerpTargetPosNormalBehaviorTimeUp;
    [AppliedWithChunk<Chunk24087000>]
    public int SlerpTargetPosNormalBehaviorTimeUp { get => slerpTargetPosNormalBehaviorTimeUp; set => slerpTargetPosNormalBehaviorTimeUp = value; }

    private int slerpTargetPosNormalBehaviorTimeDown;
    [AppliedWithChunk<Chunk24087000>]
    public int SlerpTargetPosNormalBehaviorTimeDown { get => slerpTargetPosNormalBehaviorTimeDown; set => slerpTargetPosNormalBehaviorTimeDown = value; }

    private float slerpTargetPosFlyingDelta;
    [AppliedWithChunk<Chunk24087000>]
    public float SlerpTargetPosFlyingDelta { get => slerpTargetPosFlyingDelta; set => slerpTargetPosFlyingDelta = value; }

    private int slerpTargetPosFlyingTimeUp;
    [AppliedWithChunk<Chunk24087000>]
    public int SlerpTargetPosFlyingTimeUp { get => slerpTargetPosFlyingTimeUp; set => slerpTargetPosFlyingTimeUp = value; }

    private int slerpTargetPosFlyingTimeDown;
    [AppliedWithChunk<Chunk24087000>]
    public int SlerpTargetPosFlyingTimeDown { get => slerpTargetPosFlyingTimeDown; set => slerpTargetPosFlyingTimeDown = value; }

    private float slerpTargetPosDelta;
    [AppliedWithChunk<Chunk24087000>]
    public float SlerpTargetPosDelta { get => slerpTargetPosDelta; set => slerpTargetPosDelta = value; }

    private int slerpTargetPosTimeUp;
    [AppliedWithChunk<Chunk24087000>]
    public int SlerpTargetPosTimeUp { get => slerpTargetPosTimeUp; set => slerpTargetPosTimeUp = value; }

    private int slerpTargetPosTimeDown;
    [AppliedWithChunk<Chunk24087000>]
    public int SlerpTargetPosTimeDown { get => slerpTargetPosTimeDown; set => slerpTargetPosTimeDown = value; }

    private float slerpTargetCamUpDelta;
    [AppliedWithChunk<Chunk24087000>]
    public float SlerpTargetCamUpDelta { get => slerpTargetCamUpDelta; set => slerpTargetCamUpDelta = value; }

    private int slerpTargetCamUpTimeUp;
    [AppliedWithChunk<Chunk24087000>]
    public int SlerpTargetCamUpTimeUp { get => slerpTargetCamUpTimeUp; set => slerpTargetCamUpTimeUp = value; }

    private int slerpTargetCamUpTimeDown;
    [AppliedWithChunk<Chunk24087000>]
    public int SlerpTargetCamUpTimeDown { get => slerpTargetCamUpTimeDown; set => slerpTargetCamUpTimeDown = value; }

    private float slerpSpeedFlyingBehavior;
    [AppliedWithChunk<Chunk24087000>]
    public float SlerpSpeedFlyingBehavior { get => slerpSpeedFlyingBehavior; set => slerpSpeedFlyingBehavior = value; }

    private float slerpSpeedDelta;
    [AppliedWithChunk<Chunk24087000>]
    public float SlerpSpeedDelta { get => slerpSpeedDelta; set => slerpSpeedDelta = value; }

    private int slerpSpeedTimeUp;
    [AppliedWithChunk<Chunk24087000>]
    public int SlerpSpeedTimeUp { get => slerpSpeedTimeUp; set => slerpSpeedTimeUp = value; }

    private int slerpSpeedTimeDown;
    [AppliedWithChunk<Chunk24087000>]
    public int SlerpSpeedTimeDown { get => slerpSpeedTimeDown; set => slerpSpeedTimeDown = value; }

    private float slerpSpeedCamUp;
    [AppliedWithChunk<Chunk24087000>]
    public float SlerpSpeedCamUp { get => slerpSpeedCamUp; set => slerpSpeedCamUp = value; }

    private float slerpSpeedCamUpFlyingBehavior;
    [AppliedWithChunk<Chunk24087000>]
    public float SlerpSpeedCamUpFlyingBehavior { get => slerpSpeedCamUpFlyingBehavior; set => slerpSpeedCamUpFlyingBehavior = value; }

    private float slerpSpeedCamUpDelta;
    [AppliedWithChunk<Chunk24087000>]
    public float SlerpSpeedCamUpDelta { get => slerpSpeedCamUpDelta; set => slerpSpeedCamUpDelta = value; }

    private int slerpSpeedCamUpTimeUp;
    [AppliedWithChunk<Chunk24087000>]
    public int SlerpSpeedCamUpTimeUp { get => slerpSpeedCamUpTimeUp; set => slerpSpeedCamUpTimeUp = value; }

    private int slerpSpeedCamUpTimeDown;
    [AppliedWithChunk<Chunk24087000>]
    public int SlerpSpeedCamUpTimeDown { get => slerpSpeedCamUpTimeDown; set => slerpSpeedCamUpTimeDown = value; }

    private float stateFlyingLookAtFactorDelta;
    [AppliedWithChunk<Chunk24087000>]
    public float StateFlyingLookAtFactorDelta { get => stateFlyingLookAtFactorDelta; set => stateFlyingLookAtFactorDelta = value; }

    private int stateFlyingLookAtFactorTimeUp;
    [AppliedWithChunk<Chunk24087000>]
    public int StateFlyingLookAtFactorTimeUp { get => stateFlyingLookAtFactorTimeUp; set => stateFlyingLookAtFactorTimeUp = value; }

    private int stateFlyingLookAtFactorTimeDown;
    [AppliedWithChunk<Chunk24087000>]
    public int StateFlyingLookAtFactorTimeDown { get => stateFlyingLookAtFactorTimeDown; set => stateFlyingLookAtFactorTimeDown = value; }

    private float stateFlyingLookAtStep;
    [AppliedWithChunk<Chunk24087000>]
    public float StateFlyingLookAtStep { get => stateFlyingLookAtStep; set => stateFlyingLookAtStep = value; }

    private float stateFlyingRadiusDelta;
    [AppliedWithChunk<Chunk24087000>]
    public float StateFlyingRadiusDelta { get => stateFlyingRadiusDelta; set => stateFlyingRadiusDelta = value; }

    private int stateFlyingRadiusTimeUp;
    [AppliedWithChunk<Chunk24087000>]
    public int StateFlyingRadiusTimeUp { get => stateFlyingRadiusTimeUp; set => stateFlyingRadiusTimeUp = value; }

    private int stateFlyingRadiusTimeDown;
    [AppliedWithChunk<Chunk24087000>]
    public int StateFlyingRadiusTimeDown { get => stateFlyingRadiusTimeDown; set => stateFlyingRadiusTimeDown = value; }

    private float constantFlyingLookDownFactor;
    [AppliedWithChunk<Chunk24087000>]
    public float ConstantFlyingLookDownFactor { get => constantFlyingLookDownFactor; set => constantFlyingLookDownFactor = value; }

    private float flyingLookDownFactorKi;
    [AppliedWithChunk<Chunk24087000>]
    public float FlyingLookDownFactorKi { get => flyingLookDownFactorKi; set => flyingLookDownFactorKi = value; }

    private float flyingLookDownFactorKa;
    [AppliedWithChunk<Chunk24087000>]
    public float FlyingLookDownFactorKa { get => flyingLookDownFactorKa; set => flyingLookDownFactorKa = value; }

    private float radiusDamperKi;
    [AppliedWithChunk<Chunk24087000>]
    public float RadiusDamperKi { get => radiusDamperKi; set => radiusDamperKi = value; }

    private float radiusDamperKa;
    [AppliedWithChunk<Chunk24087000>]
    public float RadiusDamperKa { get => radiusDamperKa; set => radiusDamperKa = value; }

    private float inputGasFarDelta;
    [AppliedWithChunk<Chunk24087000>]
    public float InputGasFarDelta { get => inputGasFarDelta; set => inputGasFarDelta = value; }

    private int inputGasFarTimeUp;
    [AppliedWithChunk<Chunk24087000>]
    public int InputGasFarTimeUp { get => inputGasFarTimeUp; set => inputGasFarTimeUp = value; }

    private int inputGasFarTimeDown;
    [AppliedWithChunk<Chunk24087000>]
    public int InputGasFarTimeDown { get => inputGasFarTimeDown; set => inputGasFarTimeDown = value; }

    private float inputBrakeFarDelta;
    [AppliedWithChunk<Chunk24087000>]
    public float InputBrakeFarDelta { get => inputBrakeFarDelta; set => inputBrakeFarDelta = value; }

    private int inputBrakeFarTimeUp;
    [AppliedWithChunk<Chunk24087000>]
    public int InputBrakeFarTimeUp { get => inputBrakeFarTimeUp; set => inputBrakeFarTimeUp = value; }

    private int inputBrakeFarTimeDown;
    [AppliedWithChunk<Chunk24087000>]
    public int InputBrakeFarTimeDown { get => inputBrakeFarTimeDown; set => inputBrakeFarTimeDown = value; }

    private float inputSteerFarDelta;
    [AppliedWithChunk<Chunk24087000>]
    public float InputSteerFarDelta { get => inputSteerFarDelta; set => inputSteerFarDelta = value; }

    private int inputSteerFarTimeUp;
    [AppliedWithChunk<Chunk24087000>]
    public int InputSteerFarTimeUp { get => inputSteerFarTimeUp; set => inputSteerFarTimeUp = value; }

    private int inputSteerFarTimeDown;
    [AppliedWithChunk<Chunk24087000>]
    public int InputSteerFarTimeDown { get => inputSteerFarTimeDown; set => inputSteerFarTimeDown = value; }

    private float eventTurboFovDelta;
    [AppliedWithChunk<Chunk24087000>]
    public float EventTurboFovDelta { get => eventTurboFovDelta; set => eventTurboFovDelta = value; }

    private int eventTurboFovTimeUp;
    [AppliedWithChunk<Chunk24087000>]
    public int EventTurboFovTimeUp { get => eventTurboFovTimeUp; set => eventTurboFovTimeUp = value; }

    private int eventTurboFovTimeDown;
    [AppliedWithChunk<Chunk24087000>]
    public int EventTurboFovTimeDown { get => eventTurboFovTimeDown; set => eventTurboFovTimeDown = value; }

    private float eventTurboFarDelta;
    [AppliedWithChunk<Chunk24087000>]
    public float EventTurboFarDelta { get => eventTurboFarDelta; set => eventTurboFarDelta = value; }

    private int eventTurboFarTimeUp;
    [AppliedWithChunk<Chunk24087000>]
    public int EventTurboFarTimeUp { get => eventTurboFarTimeUp; set => eventTurboFarTimeUp = value; }

    private int eventTurboFarTimeDown;
    [AppliedWithChunk<Chunk24087000>]
    public int EventTurboFarTimeDown { get => eventTurboFarTimeDown; set => eventTurboFarTimeDown = value; }

    private float eventChangeGearFarDelta;
    [AppliedWithChunk<Chunk24087000>]
    public float EventChangeGearFarDelta { get => eventChangeGearFarDelta; set => eventChangeGearFarDelta = value; }

    private int eventChangeGearFarTimeUp;
    [AppliedWithChunk<Chunk24087000>]
    public int EventChangeGearFarTimeUp { get => eventChangeGearFarTimeUp; set => eventChangeGearFarTimeUp = value; }

    private int eventChangeGearFarTimeDown;
    [AppliedWithChunk<Chunk24087000>]
    public int EventChangeGearFarTimeDown { get => eventChangeGearFarTimeDown; set => eventChangeGearFarTimeDown = value; }

    private float eventBurningLookAtFactorDelta;
    [AppliedWithChunk<Chunk24087000>]
    public float EventBurningLookAtFactorDelta { get => eventBurningLookAtFactorDelta; set => eventBurningLookAtFactorDelta = value; }

    private int eventBurningLookAtFactorTimeDown;
    [AppliedWithChunk<Chunk24087000>]
    public int EventBurningLookAtFactorTimeDown { get => eventBurningLookAtFactorTimeDown; set => eventBurningLookAtFactorTimeDown = value; }

    private float eventBurningRadiusDelta;
    [AppliedWithChunk<Chunk24087000>]
    public float EventBurningRadiusDelta { get => eventBurningRadiusDelta; set => eventBurningRadiusDelta = value; }

    private int eventBurningRadiusTimeUp;
    [AppliedWithChunk<Chunk24087000>]
    public int EventBurningRadiusTimeUp { get => eventBurningRadiusTimeUp; set => eventBurningRadiusTimeUp = value; }

    private int eventBurningRadiusTimeDown;
    [AppliedWithChunk<Chunk24087000>]
    public int EventBurningRadiusTimeDown { get => eventBurningRadiusTimeDown; set => eventBurningRadiusTimeDown = value; }

    private CFuncKeysReal? slerpSpeedModulationFromSpeed;
    [AppliedWithChunk<Chunk24087000>]
    public CFuncKeysReal? SlerpSpeedModulationFromSpeed { get => slerpSpeedModulationFromSpeed; set => slerpSpeedModulationFromSpeed = value; }

    private CFuncKeysReal? lookAtFactorFromUpSpeedRatio;
    [AppliedWithChunk<Chunk24087000>]
    public CFuncKeysReal? LookAtFactorFromUpSpeedRatio { get => lookAtFactorFromUpSpeedRatio; set => lookAtFactorFromUpSpeedRatio = value; }

    private CFuncKeysReal? flyingLookDownFactorFromSpeedRatio;
    [AppliedWithChunk<Chunk24087000>]
    public CFuncKeysReal? FlyingLookDownFactorFromSpeedRatio { get => flyingLookDownFactorFromSpeedRatio; set => flyingLookDownFactorFromSpeedRatio = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameControlCameraTrackManiaRace3"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameControlCameraTrackManiaRace3() { }


    /// <summary>
    /// CGameControlCameraTrackManiaRace3 0x000 chunk
    /// </summary>
    [Chunk(0x24087000)]
    public partial class Chunk24087000 : Chunk<CGameControlCameraTrackManiaRace3>
    {
        /// <inheritdoc />
        public override uint Id => 0x24087000;

        public int U01;

        public override void ReadWrite(CGameControlCameraTrackManiaRace3 n, GbxReaderWriter rw)
        {
            rw.Single(ref n.slerpSpeed);
            rw.Single(ref n.up);
            rw.Single(ref n.far);
            rw.Int32(ref n.flyDurationBeforeFlyingBehavior);
            rw.Int32(ref n.inputNoSteerDurationBeforeReset);
            rw.Int32(ref n.inputSteerDurationBeforeBurnoutShowView);
            rw.Single(ref n.minSpeed);
            rw.Single(ref n.minSpeed2);
            rw.Single(ref n.slerpTargetPosNormalBehaviorDelta);
            rw.Int32(ref n.slerpTargetPosNormalBehaviorTimeUp);
            rw.Int32(ref n.slerpTargetPosNormalBehaviorTimeDown);
            rw.Single(ref n.slerpTargetPosFlyingDelta);
            rw.Int32(ref n.slerpTargetPosFlyingTimeUp);
            rw.Int32(ref n.slerpTargetPosFlyingTimeDown);
            rw.Single(ref n.slerpTargetPosDelta);
            rw.Int32(ref n.slerpTargetPosTimeUp);
            rw.Int32(ref n.slerpTargetPosTimeDown);
            rw.Single(ref n.slerpTargetCamUpDelta);
            rw.Int32(ref n.slerpTargetCamUpTimeUp);
            rw.Int32(ref n.slerpTargetCamUpTimeDown);
            rw.Single(ref n.slerpSpeedFlyingBehavior);
            rw.Single(ref n.slerpSpeedDelta);
            rw.Int32(ref n.slerpSpeedTimeUp);
            rw.Int32(ref n.slerpSpeedTimeDown);
            rw.Single(ref n.slerpSpeedCamUp);
            rw.Single(ref n.slerpSpeedCamUpFlyingBehavior);
            rw.Single(ref n.slerpSpeedCamUpDelta);
            rw.Int32(ref n.slerpSpeedCamUpTimeUp);
            rw.Int32(ref n.slerpSpeedCamUpTimeDown);
            rw.Single(ref n.stateFlyingLookAtFactorDelta);
            rw.Int32(ref n.stateFlyingLookAtFactorTimeUp);
            rw.Int32(ref n.stateFlyingLookAtFactorTimeDown);
            rw.Single(ref n.stateFlyingLookAtStep);
            rw.Single(ref n.stateFlyingRadiusDelta);
            rw.Int32(ref n.stateFlyingRadiusTimeUp);
            rw.Int32(ref n.stateFlyingRadiusTimeDown);
            rw.Single(ref n.constantFlyingLookDownFactor);
            rw.Single(ref n.flyingLookDownFactorKi);
            rw.Single(ref n.flyingLookDownFactorKa);
            rw.Single(ref n.radiusDamperKi);
            rw.Single(ref n.radiusDamperKa);
            rw.Single(ref n.inputGasFarDelta);
            rw.Int32(ref n.inputGasFarTimeUp);
            rw.Int32(ref n.inputGasFarTimeDown);
            rw.Single(ref n.inputBrakeFarDelta);
            rw.Int32(ref n.inputBrakeFarTimeUp);
            rw.Int32(ref n.inputBrakeFarTimeDown);
            rw.Single(ref n.inputSteerFarDelta);
            rw.Int32(ref n.inputSteerFarTimeUp);
            rw.Int32(ref n.inputSteerFarTimeDown);
            rw.Single(ref n.eventTurboFovDelta);
            rw.Int32(ref n.eventTurboFovTimeUp);
            rw.Int32(ref n.eventTurboFovTimeDown);
            rw.Single(ref n.eventTurboFarDelta);
            rw.Int32(ref n.eventTurboFarTimeUp);
            rw.Int32(ref n.eventTurboFarTimeDown);
            rw.Single(ref n.eventChangeGearFarDelta);
            rw.Int32(ref n.eventChangeGearFarTimeUp);
            rw.Int32(ref n.eventChangeGearFarTimeDown);
            rw.Single(ref n.eventBurningLookAtFactorDelta);
            rw.Int32(ref U01);
            rw.Int32(ref n.eventBurningLookAtFactorTimeDown);
            rw.Single(ref n.eventBurningRadiusDelta);
            rw.Int32(ref n.eventBurningRadiusTimeUp);
            rw.Int32(ref n.eventBurningRadiusTimeDown);
            rw.NodeRef<CFuncKeysReal>(ref n.slerpSpeedModulationFromSpeed);
            rw.NodeRef<CFuncKeysReal>(ref n.lookAtFactorFromUpSpeedRatio);
            rw.NodeRef<CFuncKeysReal>(ref n.flyingLookDownFactorFromSpeedRatio);
        }
    }

    /// <summary>
    /// CGameControlCameraTrackManiaRace3 0x001 chunk
    /// </summary>
    [Chunk(0x24087001)]
    public partial class Chunk24087001 : Chunk<CGameControlCameraTrackManiaRace3>
    {
        /// <inheritdoc />
        public override uint Id => 0x24087001;

        public float U01;

        public override void ReadWrite(CGameControlCameraTrackManiaRace3 n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x24087000 => new Chunk24087000(),
        0x24087001 => new Chunk24087001(),
        _ => base.NewChunk(chunkId),
    };
}
