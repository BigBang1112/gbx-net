namespace GBX.NET.Engines.TrackMania;

/// <remarks>ID: 0x24086000</remarks>
[Class(0x24086000)]
public partial class CGameControlCameraTrackManiaRace2 : CGameControlCameraTarget, IClass
{
    [Hexadecimal] public static new uint Id => 0x24086000;




    private float inputGasDistDelta;
    [AppliedWithChunk<Chunk24086000>]
    public float InputGasDistDelta { get => inputGasDistDelta; set => inputGasDistDelta = value; }

    private int inputGasDistTimeUp;
    [AppliedWithChunk<Chunk24086000>]
    public int InputGasDistTimeUp { get => inputGasDistTimeUp; set => inputGasDistTimeUp = value; }

    private int inputGasDistTimeDown;
    [AppliedWithChunk<Chunk24086000>]
    public int InputGasDistTimeDown { get => inputGasDistTimeDown; set => inputGasDistTimeDown = value; }

    private float inputBrakeDistDelta;
    [AppliedWithChunk<Chunk24086000>]
    public float InputBrakeDistDelta { get => inputBrakeDistDelta; set => inputBrakeDistDelta = value; }

    private int inputBrakeDistTimeUp;
    [AppliedWithChunk<Chunk24086000>]
    public int InputBrakeDistTimeUp { get => inputBrakeDistTimeUp; set => inputBrakeDistTimeUp = value; }

    private int inputBrakeDistTimeDown;
    [AppliedWithChunk<Chunk24086000>]
    public int InputBrakeDistTimeDown { get => inputBrakeDistTimeDown; set => inputBrakeDistTimeDown = value; }

    private float inputSteerDistDelta;
    [AppliedWithChunk<Chunk24086000>]
    public float InputSteerDistDelta { get => inputSteerDistDelta; set => inputSteerDistDelta = value; }

    private int inputSteerDistTimeUp;
    [AppliedWithChunk<Chunk24086000>]
    public int InputSteerDistTimeUp { get => inputSteerDistTimeUp; set => inputSteerDistTimeUp = value; }

    private int inputSteerDistTimeDown;
    [AppliedWithChunk<Chunk24086000>]
    public int InputSteerDistTimeDown { get => inputSteerDistTimeDown; set => inputSteerDistTimeDown = value; }

    private float eventTurboFovDelta;
    [AppliedWithChunk<Chunk24086000>]
    public float EventTurboFovDelta { get => eventTurboFovDelta; set => eventTurboFovDelta = value; }

    private int eventTurboFovTimeUp;
    [AppliedWithChunk<Chunk24086000>]
    public int EventTurboFovTimeUp { get => eventTurboFovTimeUp; set => eventTurboFovTimeUp = value; }

    private int eventTurboFovTimeDown;
    [AppliedWithChunk<Chunk24086000>]
    public int EventTurboFovTimeDown { get => eventTurboFovTimeDown; set => eventTurboFovTimeDown = value; }

    private float eventTurboDistDelta;
    [AppliedWithChunk<Chunk24086000>]
    public float EventTurboDistDelta { get => eventTurboDistDelta; set => eventTurboDistDelta = value; }

    private int eventTurboDistTimeUp;
    [AppliedWithChunk<Chunk24086000>]
    public int EventTurboDistTimeUp { get => eventTurboDistTimeUp; set => eventTurboDistTimeUp = value; }

    private int eventTurboDistTimeDown;
    [AppliedWithChunk<Chunk24086000>]
    public int EventTurboDistTimeDown { get => eventTurboDistTimeDown; set => eventTurboDistTimeDown = value; }

    private float eventChangeGearDistDelta;
    [AppliedWithChunk<Chunk24086000>]
    public float EventChangeGearDistDelta { get => eventChangeGearDistDelta; set => eventChangeGearDistDelta = value; }

    private int eventChangeGearDistTimeUp;
    [AppliedWithChunk<Chunk24086000>]
    public int EventChangeGearDistTimeUp { get => eventChangeGearDistTimeUp; set => eventChangeGearDistTimeUp = value; }

    private int eventChangeGearDistTimeDown;
    [AppliedWithChunk<Chunk24086000>]
    public int EventChangeGearDistTimeDown { get => eventChangeGearDistTimeDown; set => eventChangeGearDistTimeDown = value; }

    private float eventBurningLookAtFactorDelta;
    [AppliedWithChunk<Chunk24086000>]
    public float EventBurningLookAtFactorDelta { get => eventBurningLookAtFactorDelta; set => eventBurningLookAtFactorDelta = value; }

    private int eventBurningLookAtFactorTimeUp;
    [AppliedWithChunk<Chunk24086000>]
    public int EventBurningLookAtFactorTimeUp { get => eventBurningLookAtFactorTimeUp; set => eventBurningLookAtFactorTimeUp = value; }

    private int eventBurningLookAtFactorTimeDown;
    [AppliedWithChunk<Chunk24086000>]
    public int EventBurningLookAtFactorTimeDown { get => eventBurningLookAtFactorTimeDown; set => eventBurningLookAtFactorTimeDown = value; }

    private float eventBurningDistDelta;
    [AppliedWithChunk<Chunk24086000>]
    public float EventBurningDistDelta { get => eventBurningDistDelta; set => eventBurningDistDelta = value; }

    private int eventBurningDistTimeUp;
    [AppliedWithChunk<Chunk24086000>]
    public int EventBurningDistTimeUp { get => eventBurningDistTimeUp; set => eventBurningDistTimeUp = value; }

    private int eventBurningDistTimeDown;
    [AppliedWithChunk<Chunk24086000>]
    public int EventBurningDistTimeDown { get => eventBurningDistTimeDown; set => eventBurningDistTimeDown = value; }

    private float stateFlyingDistDelta;
    [AppliedWithChunk<Chunk24086000>]
    public float StateFlyingDistDelta { get => stateFlyingDistDelta; set => stateFlyingDistDelta = value; }

    private int stateFlyingDistTimeUp;
    [AppliedWithChunk<Chunk24086000>]
    public int StateFlyingDistTimeUp { get => stateFlyingDistTimeUp; set => stateFlyingDistTimeUp = value; }

    private int stateFlyingDistTimeDown;
    [AppliedWithChunk<Chunk24086000>]
    public int StateFlyingDistTimeDown { get => stateFlyingDistTimeDown; set => stateFlyingDistTimeDown = value; }

    private float stateFlyingPlaneDistDelta;
    [AppliedWithChunk<Chunk24086000>]
    public float StateFlyingPlaneDistDelta { get => stateFlyingPlaneDistDelta; set => stateFlyingPlaneDistDelta = value; }

    private int stateFlyingPlaneDistTimeUp;
    [AppliedWithChunk<Chunk24086000>]
    public int StateFlyingPlaneDistTimeUp { get => stateFlyingPlaneDistTimeUp; set => stateFlyingPlaneDistTimeUp = value; }

    private int stateFlyingPlaneDistTimeDown;
    [AppliedWithChunk<Chunk24086000>]
    public int StateFlyingPlaneDistTimeDown { get => stateFlyingPlaneDistTimeDown; set => stateFlyingPlaneDistTimeDown = value; }

    private float stateFlyingLookAtFactorDelta;
    [AppliedWithChunk<Chunk24086000>]
    public float StateFlyingLookAtFactorDelta { get => stateFlyingLookAtFactorDelta; set => stateFlyingLookAtFactorDelta = value; }

    private int stateFlyingLookAtFactorTimeUp;
    [AppliedWithChunk<Chunk24086000>]
    public int StateFlyingLookAtFactorTimeUp { get => stateFlyingLookAtFactorTimeUp; set => stateFlyingLookAtFactorTimeUp = value; }

    private int stateFlyingLookAtFactorTimeDown;
    [AppliedWithChunk<Chunk24086000>]
    public int StateFlyingLookAtFactorTimeDown { get => stateFlyingLookAtFactorTimeDown; set => stateFlyingLookAtFactorTimeDown = value; }

    private float inputLeftSteerRollDelta;
    [AppliedWithChunk<Chunk24086000>]
    public float InputLeftSteerRollDelta { get => inputLeftSteerRollDelta; set => inputLeftSteerRollDelta = value; }

    private int inputLeftSteerRollTimeUp;
    [AppliedWithChunk<Chunk24086000>]
    public int InputLeftSteerRollTimeUp { get => inputLeftSteerRollTimeUp; set => inputLeftSteerRollTimeUp = value; }

    private int inputLeftSteerRollTimeDown;
    [AppliedWithChunk<Chunk24086000>]
    public int InputLeftSteerRollTimeDown { get => inputLeftSteerRollTimeDown; set => inputLeftSteerRollTimeDown = value; }

    private float inputRightSteerRollDelta;
    [AppliedWithChunk<Chunk24086000>]
    public float InputRightSteerRollDelta { get => inputRightSteerRollDelta; set => inputRightSteerRollDelta = value; }

    private int inputRightSteerRollTimeUp;
    [AppliedWithChunk<Chunk24086000>]
    public int InputRightSteerRollTimeUp { get => inputRightSteerRollTimeUp; set => inputRightSteerRollTimeUp = value; }

    private int inputRightSteerRollTimeDown;
    [AppliedWithChunk<Chunk24086000>]
    public int InputRightSteerRollTimeDown { get => inputRightSteerRollTimeDown; set => inputRightSteerRollTimeDown = value; }

    private float inputLeftSteerYawDelta;
    [AppliedWithChunk<Chunk24086000>]
    public float InputLeftSteerYawDelta { get => inputLeftSteerYawDelta; set => inputLeftSteerYawDelta = value; }

    private int inputLeftSteerYawTimeUp;
    [AppliedWithChunk<Chunk24086000>]
    public int InputLeftSteerYawTimeUp { get => inputLeftSteerYawTimeUp; set => inputLeftSteerYawTimeUp = value; }

    private int inputLeftSteerYawTimeDown;
    [AppliedWithChunk<Chunk24086000>]
    public int InputLeftSteerYawTimeDown { get => inputLeftSteerYawTimeDown; set => inputLeftSteerYawTimeDown = value; }

    private float inputRightSteerYawDelta;
    [AppliedWithChunk<Chunk24086000>]
    public float InputRightSteerYawDelta { get => inputRightSteerYawDelta; set => inputRightSteerYawDelta = value; }

    private int inputRightSteerYawTimeUp;
    [AppliedWithChunk<Chunk24086000>]
    public int InputRightSteerYawTimeUp { get => inputRightSteerYawTimeUp; set => inputRightSteerYawTimeUp = value; }

    private int inputRightSteerYawTimeDown;
    [AppliedWithChunk<Chunk24086000>]
    public int InputRightSteerYawTimeDown { get => inputRightSteerYawTimeDown; set => inputRightSteerYawTimeDown = value; }

    private float stateFlyingLookAtStep;
    [AppliedWithChunk<Chunk24086000>]
    public float StateFlyingLookAtStep { get => stateFlyingLookAtStep; set => stateFlyingLookAtStep = value; }

    private float minSpeed;
    [AppliedWithChunk<Chunk24086000>]
    public float MinSpeed { get => minSpeed; set => minSpeed = value; }

    private int stateFlyingDurationBeforeFlyingMode;
    [AppliedWithChunk<Chunk24086000>]
    public int StateFlyingDurationBeforeFlyingMode { get => stateFlyingDurationBeforeFlyingMode; set => stateFlyingDurationBeforeFlyingMode = value; }

    private int stateFlyingDurationBeforeCameraMove;
    [AppliedWithChunk<Chunk24086000>]
    public int StateFlyingDurationBeforeCameraMove { get => stateFlyingDurationBeforeCameraMove; set => stateFlyingDurationBeforeCameraMove = value; }

    private int inputSteerDurationBeforeBurnoutShowView;
    [AppliedWithChunk<Chunk24086000>]
    public int InputSteerDurationBeforeBurnoutShowView { get => inputSteerDurationBeforeBurnoutShowView; set => inputSteerDurationBeforeBurnoutShowView = value; }

    private int inputSteerDurationBeforeAnticipatingTurnTriggered;
    [AppliedWithChunk<Chunk24086000>]
    public int InputSteerDurationBeforeAnticipatingTurnTriggered { get => inputSteerDurationBeforeAnticipatingTurnTriggered; set => inputSteerDurationBeforeAnticipatingTurnTriggered = value; }

    private int inputSteerDurationBeforeRollTriggered;
    [AppliedWithChunk<Chunk24086000>]
    public int InputSteerDurationBeforeRollTriggered { get => inputSteerDurationBeforeRollTriggered; set => inputSteerDurationBeforeRollTriggered = value; }

    private int inputNoSteerDurationBeforeReset;
    [AppliedWithChunk<Chunk24086000>]
    public int InputNoSteerDurationBeforeReset { get => inputNoSteerDurationBeforeReset; set => inputNoSteerDurationBeforeReset = value; }

    private float maxDeltaPlaneDistStep;
    [AppliedWithChunk<Chunk24086000>]
    public float MaxDeltaPlaneDistStep { get => maxDeltaPlaneDistStep; set => maxDeltaPlaneDistStep = value; }

    private float maxDeltaFovStep;
    [AppliedWithChunk<Chunk24086000>]
    public float MaxDeltaFovStep { get => maxDeltaFovStep; set => maxDeltaFovStep = value; }

    private CFuncKeysReal? lookAtFactorFromUpSpeedRatio;
    [AppliedWithChunk<Chunk24086000>]
    public CFuncKeysReal? LookAtFactorFromUpSpeedRatio { get => lookAtFactorFromUpSpeedRatio; set => lookAtFactorFromUpSpeedRatio = value; }

    private CFuncKeysReal? yawFromSpeed;
    [AppliedWithChunk<Chunk24086000>]
    public CFuncKeysReal? YawFromSpeed { get => yawFromSpeed; set => yawFromSpeed = value; }

    private CFuncKeysReal? rollFromSpeed;
    [AppliedWithChunk<Chunk24086000>]
    public CFuncKeysReal? RollFromSpeed { get => rollFromSpeed; set => rollFromSpeed = value; }

    private float deltaDistDamperKi;
    [AppliedWithChunk<Chunk24086000>]
    public float DeltaDistDamperKi { get => deltaDistDamperKi; set => deltaDistDamperKi = value; }

    private float deltaDistDamperKa;
    [AppliedWithChunk<Chunk24086000>]
    public float DeltaDistDamperKa { get => deltaDistDamperKa; set => deltaDistDamperKa = value; }

    private float deltaLookAtFactorDamperKi;
    [AppliedWithChunk<Chunk24086000>]
    public float DeltaLookAtFactorDamperKi { get => deltaLookAtFactorDamperKi; set => deltaLookAtFactorDamperKi = value; }

    private float deltaLookAtFactorDamperKa;
    [AppliedWithChunk<Chunk24086000>]
    public float DeltaLookAtFactorDamperKa { get => deltaLookAtFactorDamperKa; set => deltaLookAtFactorDamperKa = value; }

    private bool isRollFromInput;
    [AppliedWithChunk<Chunk24086001>]
    public bool IsRollFromInput { get => isRollFromInput; set => isRollFromInput = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameControlCameraTrackManiaRace2"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameControlCameraTrackManiaRace2() { }


    /// <summary>
    /// CGameControlCameraTrackManiaRace2 0x000 chunk
    /// </summary>
    [Chunk(0x24086000)]
    public partial class Chunk24086000 : Chunk<CGameControlCameraTrackManiaRace2>
    {
        /// <inheritdoc />
        public override uint Id => 0x24086000;


        public override void ReadWrite(CGameControlCameraTrackManiaRace2 n, GbxReaderWriter rw)
        {
            rw.Single(ref n.inputGasDistDelta);
            rw.Int32(ref n.inputGasDistTimeUp);
            rw.Int32(ref n.inputGasDistTimeDown);
            rw.Single(ref n.inputBrakeDistDelta);
            rw.Int32(ref n.inputBrakeDistTimeUp);
            rw.Int32(ref n.inputBrakeDistTimeDown);
            rw.Single(ref n.inputSteerDistDelta);
            rw.Int32(ref n.inputSteerDistTimeUp);
            rw.Int32(ref n.inputSteerDistTimeDown);
            rw.Single(ref n.eventTurboFovDelta);
            rw.Int32(ref n.eventTurboFovTimeUp);
            rw.Int32(ref n.eventTurboFovTimeDown);
            rw.Single(ref n.eventTurboDistDelta);
            rw.Int32(ref n.eventTurboDistTimeUp);
            rw.Int32(ref n.eventTurboDistTimeDown);
            rw.Single(ref n.eventChangeGearDistDelta);
            rw.Int32(ref n.eventChangeGearDistTimeUp);
            rw.Int32(ref n.eventChangeGearDistTimeDown);
            rw.Single(ref n.eventBurningLookAtFactorDelta);
            rw.Int32(ref n.eventBurningLookAtFactorTimeUp);
            rw.Int32(ref n.eventBurningLookAtFactorTimeDown);
            rw.Single(ref n.eventBurningDistDelta);
            rw.Int32(ref n.eventBurningDistTimeUp);
            rw.Int32(ref n.eventBurningDistTimeDown);
            rw.Single(ref n.stateFlyingDistDelta);
            rw.Int32(ref n.stateFlyingDistTimeUp);
            rw.Int32(ref n.stateFlyingDistTimeDown);
            rw.Single(ref n.stateFlyingPlaneDistDelta);
            rw.Int32(ref n.stateFlyingPlaneDistTimeUp);
            rw.Int32(ref n.stateFlyingPlaneDistTimeDown);
            rw.Single(ref n.stateFlyingLookAtFactorDelta);
            rw.Int32(ref n.stateFlyingLookAtFactorTimeUp);
            rw.Int32(ref n.stateFlyingLookAtFactorTimeDown);
            rw.Single(ref n.inputLeftSteerRollDelta);
            rw.Int32(ref n.inputLeftSteerRollTimeUp);
            rw.Int32(ref n.inputLeftSteerRollTimeDown);
            rw.Single(ref n.inputRightSteerRollDelta);
            rw.Int32(ref n.inputRightSteerRollTimeUp);
            rw.Int32(ref n.inputRightSteerRollTimeDown);
            rw.Single(ref n.inputLeftSteerYawDelta);
            rw.Int32(ref n.inputLeftSteerYawTimeUp);
            rw.Int32(ref n.inputLeftSteerYawTimeDown);
            rw.Single(ref n.inputRightSteerYawDelta);
            rw.Int32(ref n.inputRightSteerYawTimeUp);
            rw.Int32(ref n.inputRightSteerYawTimeDown);
            rw.Single(ref n.stateFlyingLookAtStep);
            rw.Single(ref n.minSpeed);
            rw.Int32(ref n.stateFlyingDurationBeforeFlyingMode);
            rw.Int32(ref n.stateFlyingDurationBeforeCameraMove);
            rw.Int32(ref n.inputSteerDurationBeforeBurnoutShowView);
            rw.Int32(ref n.inputSteerDurationBeforeAnticipatingTurnTriggered);
            rw.Int32(ref n.inputSteerDurationBeforeRollTriggered);
            rw.Int32(ref n.inputNoSteerDurationBeforeReset);
            rw.Single(ref n.maxDeltaPlaneDistStep);
            rw.Single(ref n.maxDeltaFovStep);
            rw.NodeRef<CFuncKeysReal>(ref n.lookAtFactorFromUpSpeedRatio);
            rw.NodeRef<CFuncKeysReal>(ref n.yawFromSpeed);
            rw.NodeRef<CFuncKeysReal>(ref n.rollFromSpeed);
            rw.Single(ref n.deltaDistDamperKi);
            rw.Single(ref n.deltaDistDamperKa);
            rw.Single(ref n.deltaLookAtFactorDamperKi);
            rw.Single(ref n.deltaLookAtFactorDamperKa);
        }
    }

    /// <summary>
    /// CGameControlCameraTrackManiaRace2 0x001 chunk
    /// </summary>
    [Chunk(0x24086001)]
    public partial class Chunk24086001 : Chunk<CGameControlCameraTrackManiaRace2>
    {
        /// <inheritdoc />
        public override uint Id => 0x24086001;


        public override void ReadWrite(CGameControlCameraTrackManiaRace2 n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isRollFromInput);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x24086000 => new Chunk24086000(),
        0x24086001 => new Chunk24086001(),
        _ => base.NewChunk(chunkId),
    };
}
