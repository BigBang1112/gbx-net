namespace GBX.NET.Engines.Game;

public partial class CGameCtnChallenge
{
    public partial class Chunk3F001001 : IVersionable
    {
        public int Version { get; set; }

        public ushort Flags { get; set; }

        public EDecorationVisibility DecorationVisibility
        {
            get => (EDecorationVisibility)(Flags & 3);
            set => Flags = (ushort)((Flags & ~3) | (ushort)value);
        }

        private Vec3 decorationOffset;
        public Vec3 DecorationOffset { get => decorationOffset; set => decorationOffset = value; }

        private Vec3 decorationScale = new(1, 1, 1);
        public Vec3 DecorationScale { get => decorationScale; set => decorationScale = value; }

        public bool SkyDecorationVisibility { get; set; }

        private List<LegacyScript> legacyScripts = [];
        public List<LegacyScript> LegacyScripts { get => legacyScripts; set => legacyScripts = value; }

        private List<ParameterSet> parameterSets = [];
        public List<ParameterSet> ParameterSets { get => parameterSets; set => parameterSets = value; }

        private List<MediaClipMapping> mediaClipMappings = [];
        public List<MediaClipMapping> MediaClipMappings { get => mediaClipMappings; set => mediaClipMappings = value; }

        private List<AngelScriptModule> angelScriptModules = [];
        public List<AngelScriptModule> AngelScriptModules { get => angelScriptModules; set => angelScriptModules = value; }

        private List<TriggerGroup> triggerGroups = [];
        public List<TriggerGroup> TriggerGroups { get => triggerGroups; set => triggerGroups = value; }

        private List<BlockGroup> blockGroups = [];
        public List<BlockGroup> BlockGroups { get => blockGroups; set => blockGroups = value; }

        private List<EmbeddedBlock> embeddedBlocks = [];
        public List<EmbeddedBlock> EmbeddedBlocks { get => embeddedBlocks; set => embeddedBlocks = value; }

        private List<MaterialModelRef> materialModelRefs = [];
        public List<MaterialModelRef> MaterialModelRefs { get => materialModelRefs; set => materialModelRefs = value; }

        private ReplacementTextureFlags replacementTexture = new();
        public ReplacementTextureFlags ReplacementTexture { get => replacementTexture; set => replacementTexture = value; }

        private List<EmbeddedImage> embeddedImages = [];
        public List<EmbeddedImage> EmbeddedImages { get => embeddedImages; set => embeddedImages = value; }

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw) => ReadWrite(n, rw, ver: 0);

        protected void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw, int ver)
        {
            rw.VersionByte(this);

            if (Version == 0)
            {
                return;
            }

            ReadWriteWithoutVersion(n, rw, ver);
        }

        public void ReadWriteWithoutVersion(CGameCtnChallenge n, GbxReaderWriter rw, int ver)
        {
            if (Version == 4)
            {
                Flags = rw.Byte((byte)Flags);

                if ((Flags & 1) != 0)
                {
                    DecorationOffset = rw.Int3((Int3)DecorationOffset);
                }

                rw.ListReadableWritable(ref legacyScripts);

                if (rw.Reader is not null) ParameterSets = rw.Reader.ReadListReadable<ParameterSet>();
                rw.Writer?.WriteListWritable(ParameterSets);

                rw.ListReadableWritable(ref mediaClipMappings);

                if (rw.Reader is not null)
                {
                    var count = rw.Reader.ReadInt32();
                    for (var i = 0; i < count; i++)
                    {
                        var data = rw.Reader.ReadBytes(30);
                        var blockType = (BlockType)rw.Reader.ReadByte();
                        var name = rw.Reader.ReadId();

                        switch (blockType)
                        {
                            case BlockType.GameBlock:
                                var blockIndex = rw.Reader.ReadInt32();
                                break;
                            case BlockType.ExternalBlock:
                                var externalBlock = rw.Reader.ReadIdent();
                                break;
                        }
                    }
                }
            }

            if (Version == 7)
            {
                Flags = rw.UInt16(Flags);

                if (DecorationVisibility != EDecorationVisibility.Nothing)
                {
                    if ((Flags & 4) != 0) // decorationOffsetApplied
                    {
                        rw.Vec3(ref decorationOffset);
                    }

                    if ((Flags & 8) != 0) // decorationScaleApplied
                    {
                        rw.Vec3(ref decorationScale);
                    }
                }

                rw.ListReadableWritable<AngelScriptModule>(ref angelScriptModules);

                if (rw.Reader is not null) ParameterSets = rw.Reader.ReadListReadable<ParameterSet>();
                rw.Writer?.WriteListWritable(ParameterSets);

                rw.ListReadableWritable<TriggerGroup>(ref triggerGroups);
                rw.ListReadableWritable<BlockGroup>(ref blockGroups);
                var embeddedBlockCount = rw.Int32(embeddedBlocks.Count);
                rw.ListReadableWritable<MaterialModelRef>(ref materialModelRefs);
                rw.ReadableWritable(ref replacementTexture);
                rw.ListReadableWritable<EmbeddedImage>(ref embeddedImages);
            }
        }

        public class EmbeddedBlock
        {

        }

        public class ReplacementTextureFlags : IReadableWritable
        {
            public const byte SpecularBit = 0;
            public const byte NormalBit = 1;
            public const byte WhiteBit = 2;
            public const byte BlackBit = 3;

            public byte Flags { get; set; }

            public bool HasSpecular => (Flags & (1 << SpecularBit)) != 0;
            public bool HasNormal => (Flags & (1 << NormalBit)) != 0;
            public bool HasWhite => (Flags & (1 << WhiteBit)) != 0;
            public bool HasBlack => (Flags & (1 << BlackBit)) != 0;

            public uint? SpecularInstanceIndex { get; set; }
            public uint? NormalInstanceIndex { get; set; }
            public uint? WhiteInstanceIndex { get; set; }
            public uint? BlackInstanceIndex { get; set; }

            public void ReadWrite(GbxReaderWriter rw, int v = 0)
            {
                Flags = rw.Byte(Flags);

                if (HasSpecular)
                {
                    SpecularInstanceIndex = rw.UInt32(SpecularInstanceIndex);
                }

                if (HasNormal)
                {
                    NormalInstanceIndex = rw.UInt32(NormalInstanceIndex);
                }

                if (HasWhite)
                {
                    WhiteInstanceIndex = rw.UInt32(WhiteInstanceIndex);
                }

                if (HasBlack)
                {
                    BlackInstanceIndex = rw.UInt32(BlackInstanceIndex);
                }
            }
        }

        public class MaterialModelRef : IReadableWritable
        {
            public uint InstanceIndex { get; set; }
            public string MaterialModelRelativePath { get; set; } = string.Empty;

            public void ReadWrite(GbxReaderWriter rw, int v = 0)
            {
                InstanceIndex = rw.UInt32(InstanceIndex);
                MaterialModelRelativePath = rw.String(MaterialModelRelativePath);
            }
        }

        public class EmbeddedImage : IReadableWritable
        {
            public uint ClassId { get; set; }
            public string RelativePath { get; set; } = string.Empty;
            public uint ImageSize { get; set; }
            public byte[] ImageData { get; set; } = [];
            public List<BitmapPair> BitmapPairs { get; set; } = [];

            public void ReadWrite(GbxReaderWriter rw, int v = 0)
            {
                ClassId = rw.UInt32(ClassId);
                RelativePath = rw.String(RelativePath);
                ImageSize = rw.UInt32(ImageSize);
                ImageData = rw.Data(ImageData, (int)ImageSize);
                BitmapPairs = rw.ListReadableWritable(BitmapPairs);
            }
        }

        public class BitmapPair : IReadableWritable
        {
            public uint InstanceIndex { get; set; }
            public byte TexFilter { get; set; }
            public byte TexAddress { get; set; }

            public void ReadWrite(GbxReaderWriter rw, int v = 0)
            {
                InstanceIndex = rw.UInt32(InstanceIndex);
                TexFilter = rw.Byte(TexFilter);
                TexAddress = rw.Byte(TexAddress);
            }
        }

        public class TriggerGroup : IReadableWritable
        {
            public string Name { get; set; } = string.Empty;
            public byte Flags { get; set; }
            public TriggerGroupCondition? Condition { get; set; }
            public TriggerGroupEvent? OnEnterEvent { get; set; }
            public TriggerGroupEvent? OnInsideEvent { get; set; }
            public TriggerGroupEvent? OnLeaveEvent { get; set; }

            public void ReadWrite(GbxReaderWriter rw, int v = 0)
            {
                Name = rw.String(Name);
                Flags = rw.Byte(Flags);

                if ((Flags & 1) != 0) // hasCondition
                {
                    Condition ??= new();
                    rw.ReadableWritable<TriggerGroupCondition>(Condition);
                }

                if ((Flags & 2) != 0) // hasOnEnterEvent
                {
                    OnEnterEvent ??= new();
                    rw.ReadableWritable<TriggerGroupEvent>(OnEnterEvent);
                }

                if ((Flags & 4) != 0) // hasOnInsideEvent
                {
                    OnInsideEvent ??= new();
                    rw.ReadableWritable<TriggerGroupEvent>(OnInsideEvent);
                }

                if ((Flags & 8) != 0) // hasOnLeaveEvent
                {
                    OnLeaveEvent ??= new();
                    rw.ReadableWritable<TriggerGroupEvent>(OnLeaveEvent);
                }
            }
        }

        public class TriggerGroupCondition : IReadableWritable
        {
            public byte EventTarget { get; set; }
            public byte? ConditionType { get; set; }
            public float? Value { get; set; }
            public uint? ModuleIndex { get; set; }
            public uint? FunctionIndex { get; set; }

            public void ReadWrite(GbxReaderWriter rw, int v = 0)
            {
                EventTarget = rw.Byte(EventTarget);

                if (EventTarget == 0) // mediaTracker
                {
                    ConditionType = rw.Byte(ConditionType ?? 0);

                    if (ConditionType != 0) // none
                    {
                        Value = rw.Single(Value ?? 0);
                    }
                }
                else if (EventTarget == 1) // angelScript
                {
                    ModuleIndex = rw.UInt32(ModuleIndex ?? 0);
                    FunctionIndex = rw.UInt32(FunctionIndex ?? 0);
                }
            }
        }

        public class TriggerGroupEvent : IReadableWritable
        {
            public byte EventTarget { get; set; }
            public uint? ParameterSetIndex { get; set; }
            public uint? ModuleIndex { get; set; }
            public uint? FunctionIndex { get; set; }

            public void ReadWrite(GbxReaderWriter rw, int v = 0)
            {
                EventTarget = rw.Byte(EventTarget);

                if (EventTarget == 0) // parameterSet
                {
                    ParameterSetIndex = rw.UInt32(ParameterSetIndex ?? 0);
                }
                else if (EventTarget == 2) // angelScript
                {
                    ModuleIndex = rw.UInt32(ModuleIndex ?? 0);
                    FunctionIndex = rw.UInt32(FunctionIndex ?? 0);
                }
            }
        }

        public class BlockGroup : IReadableWritable
        {
            public string Name { get; set; } = string.Empty;

            public void ReadWrite(GbxReaderWriter rw, int v = 0)
            {
                Name = rw.String(Name);
            }
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

        public record MediaClipMapping : IReadableWritable
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

        public record AngelScriptModule : IReadableWritable
        {
            public byte Flags { get; set; }
            public string? ModuleName { get; set; }
            public byte[]? ByteCode { get; set; }
            public bool IsCoreModule => (Flags & 2) != 0; // isCoreModule

            public void ReadWrite(GbxReaderWriter rw, int v = 0)
            {
                Flags = rw.Byte(Flags);

                if ((Flags & 2) == 0) // not isCoreModule
                {
                    ModuleName = rw.String(ModuleName);
                }

                if ((Flags & 1) == 0) // not isEmpty
                {
                    throw new NotSupportedException("AngelScript modules are not currently supported");
                    //ByteCode = rw.Data(ByteCode);
                }
            }
        }

        public enum MediaClipMappedResourceType
        {
            ParameterSet,
            LegacyScript
        }

        public enum BlockType
        {
            GameBlock,
            ExternalBlock
        }

        public enum EDecorationVisibility
        {
            Everything,
            Sky,
            Background,
            Nothing
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
    }

    public partial class Chunk3F001002
    {
        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw) => ReadWrite(n, rw, ver: 1);
    }

    public partial class Chunk3F001003 : IVersionable
    {
        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw) => ReadWrite(n, rw, ver: 2);
    }
}
