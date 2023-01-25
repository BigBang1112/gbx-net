using GBX.NET.Inputs;

namespace GBX.NET.Engines.Game;

//
// Massive thanks to Mystixor and Shweetz for many of the findings!!!
// Without these guys, this would've stayed a mystery.
// Even though the solution was read out of the game code in the end, they gave the important hint.
//

public partial class CGameCtnGhost
{
    /// <summary>
    /// Set of inputs.
    /// </summary>
    public class PlayerInputData : IReadableWritable
    {
        public enum EVersion
        {
            _2017_07_07 = 7,
            _2017_09_12 = 8,
            _2020_04_08 = 11,
            _2020_07_20 = 12
        }

        private enum EStart
        {
            NotStarted,
            Character,
            Vehicle,
        }

        private EVersion version; // 8 in shootmania, 12 in tm2020
        private int u02;
        private TimeInt32? startOffset;
        private int ticks;
        private byte[] data = Array.Empty<byte>();

        private IList<IInputChange>? inputChanges;
        private IReadOnlyCollection<IInput>? inputs;

        public EVersion Version { get => version; set => version = value; }
        public int U02 { get => u02; set => u02 = value; }
        public TimeInt32? StartOffset { get => startOffset; set => startOffset = value; }
        public int Ticks { get => ticks; set => ticks = value; }
        public byte[] Data { get => data; set => data = value; }

        [Obsolete("Use Inputs instead. Property will be removed in 1.3.0.")]
        public IList<IInputChange> InputChanges
        {
            get
            {
                if (inputChanges is null)
                {
                    var inputEnumerable = version is <= EVersion._2017_09_12
                        ? ProcessShootmaniaInputChanges()
                        : ProcessTrackmaniaInputChanges();

#if DEBUG
                    var testList = new List<IInputChange>();

                    try
                    {
                        foreach (var input in inputEnumerable)
                        {
                            testList.Add(input);
                        }
                    }
                    catch
                    {

                    }
#endif

                    inputChanges = inputEnumerable.ToArray();
                }

                return inputChanges;
            }
        }

        public IReadOnlyCollection<IInput> Inputs
        {
            get
            {
                if (inputs is null)
                {
                    var inputEnumerable = version is <= EVersion._2017_09_12
                        ? ProcessShootmaniaInputs()
                        : ProcessTrackmaniaInputs();

#if DEBUG
                    var testList = new List<IInput>();

                    try
                    {
                        foreach (var input in inputEnumerable)
                        {
                            testList.Add(input);
                        }
                    }
                    catch
                    {

                    }
#endif

                    inputs = inputEnumerable.ToArray();
                }

                return inputs;
            }
        }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.EnumInt32<EVersion>(ref this.version); // 8 in shootmania, 12 in tm2020
            rw.Int32(ref u02);

            if (version >= 4)
            {
                rw.TimeInt32Nullable(ref startOffset);
            }

            rw.Int32(ref ticks);
            rw.Bytes(ref data!);
        }

        private static bool StateIsDifferent(int bit, out bool result, int states, int? prevStates)
        {
            var mask = 1 << bit;
            return StateIsDifferentWithMask(mask, out result, states, prevStates);
        }
        
        private static bool StateIsDifferentWithMask(int mask, out bool result, int states, int? prevStates)
        {
            var masked = states & mask;
            result = masked != 0;

            if (prevStates is null)
            {
                return result;
            }

            return masked != (prevStates & mask);
        }

        private static bool StateIsDifferent(int bit, out bool result, ulong states, ulong? prevStates)
        {
            var mask = (ulong)0 << bit;
            return StateIsDifferentWithMask(mask, out result, states, prevStates);
        }

        private static bool StateIsDifferentWithMask(ulong mask, out bool result, ulong states, ulong? prevStates)
        {
            var masked = states & mask;
            result = masked != 0;

            if (prevStates is null)
            {
                return result;
            }

            return masked != (prevStates & mask);
        }

        internal IEnumerable<IInput> ProcessShootmaniaInputs()
        {
            var r = new BitReader(data);
            
            var prevStrafe = NET.Inputs.EStrafe.None;
            var prevWalk = NET.Inputs.EWalk.None;
            var prevVertical = default(byte);
            var prevStates = default(int?);

            for (var i = 0; i < ticks; i++)
            {
                var time = new TimeInt32(i * 10);
                
                var sameMouse = r.ReadBit();

                if (!sameMouse)
                {
                    var mouseAccuX = r.ReadUInt16();
                    var mouseAccuY = r.ReadUInt16();

                    yield return new MouseAccu(time, mouseAccuX, mouseAccuY);
                }

                var sameValuesAfter = r.ReadBit();
                var sameValue = sameValuesAfter;

                if (!sameValuesAfter)
                {
                    sameValue = r.ReadBit();
                }

                if (!sameValue)
                {
                    var strafe = (Inputs.EStrafe)r.Read2Bit();
                    if (strafe == NET.Inputs.EStrafe.Corrupted) throw new Exception("Corrupted inputs. (strafe == 2)");

                    if (strafe != prevStrafe)
                    {
                        yield return new Strafe(time, strafe);
                        prevStrafe = strafe;
                    }

                    var walk = (Inputs.EWalk)r.Read2Bit();
                    if (walk == NET.Inputs.EWalk.Corrupted) throw new Exception("Corrupted inputs. (walk == 2)");

                    if (walk != prevWalk)
                    {
                        yield return new Walk(time, walk);
                        prevWalk = walk;
                    }

                    if (version == EVersion._2017_09_12)
                    {
                        var vertical = r.Read2Bit();
                        
                        if (vertical != prevVertical)
                        {
                            yield return new Vertical(time, vertical);
                            prevVertical = vertical;
                        }
                    }
                }

                if (!sameValuesAfter)
                {
                    sameValue = r.ReadBit();

                    if (!sameValue)
                    {
                        var onlyGunTriggerAndAction = r.ReadBit();

                        var states = onlyGunTriggerAndAction
                            ? r.Read2Bit()
                            : r.ReadInt32();

                        if (states == prevStates)
                        {
                            continue;
                        }

                        if (StateIsDifferent(bit: 1, out bool gunTrigger, states, prevStates))
                        {
                            yield return new GunTrigger(time, gunTrigger);
                        }

                        if (onlyGunTriggerAndAction)
                        {
                            if (StateIsDifferent(bit: 0, out bool actionFrom2bit, states, prevStates))
                            {
                                yield return new Inputs.Action(time, actionFrom2bit);
                            }

                            continue;
                        }

                        if (StateIsDifferent(bit: 2, out bool freeLook, states, prevStates))
                        {
                            yield return new FreeLook(time, freeLook);
                        }

                        if (StateIsDifferent(bit: 3, out bool fly, states, prevStates))
                        {
                            yield return new Fly(time, fly);
                        }

                        if (StateIsDifferent(bit: 5, out bool camera2, states, prevStates))
                        {
                            yield return new Camera2(time, camera2);
                        }

                        if (StateIsDifferent(bit: 7, out bool jump, states, prevStates))
                        {
                            yield return new Jump(time, jump);
                        }

                        if (StateIsDifferent(bit: 8, out bool action, states, prevStates))
                        {
                            yield return new Inputs.Action(time, action);
                        }

                        for (var j = 0; j < 4; j++) // 4 action slots
                        {
                            if (StateIsDifferent(bit: 10 + j, out bool actionSlot, states, prevStates))
                            {
                                yield return new ActionSlot(time, (byte)(1 + j), actionSlot);
                            }
                        }

                        if (StateIsDifferentWithMask(16896, out bool use1, states, prevStates))
                        {
                            yield return new ActionSlot(time, 1, use1);
                        }

                        if (StateIsDifferentWithMask(32832, out bool use2, states, prevStates))
                        {
                            yield return new ActionSlot(time, 2, use2);
                        }

                        if (StateIsDifferent(bit: 16, out bool menu, states, prevStates))
                        {
                            yield return new Menu(time, menu);
                        }

                        if (StateIsDifferent(bit: 20, out bool horn, states, prevStates))
                        {
                            yield return new Horn(time, horn);
                        }

                        if (StateIsDifferent(bit: 21, out bool respawn, states, prevStates))
                        {
                            yield return new Respawn(time, respawn);
                        }

                        if (StateIsDifferent(bit: 26, out bool giveUp, states, prevStates))
                        {
                            yield return new GiveUp(time, giveUp);
                        }

                        prevStates = states;
                    }
                }
            }
        }

        internal IEnumerable<IInput> ProcessTrackmaniaInputs()
        {
            var inputs = new List<IInput>();
            
            var r = new BitReader(data);

            var started = EStart.NotStarted;
            
            var prevStates = default(ulong?);
            var prevOnlyHorn = default(bool?);
            var prevSteer = default(sbyte);
            var prevAccel = default(bool);
            var prevBrake = default(bool);
            var prevStrafe = NET.Inputs.EStrafe.None;
            var prevWalk = NET.Inputs.EWalk.None;
            var prevVertical = default(byte);
            var prevHorizontal = default(byte);

            for (var i = 0; i < ticks; i++)
            {
                var time = new TimeInt32(i * 10);
                
                try
                {
                    var sameState = r.ReadBit();

                    if (!sameState)
                    {
                        var onlyHorn = r.ReadBit();

                        var states = onlyHorn
                            ? r.Read2Bit()
                            : r.ReadNumber(bits: version is EVersion._2020_04_08 ? 33 : 34);

                        if (states == prevStates || onlyHorn == prevOnlyHorn)
                        {
                            continue;
                        }

                        if (started is EStart.NotStarted)
                        {
                            started = (EStart)(states & 3);
                        }

                        if (onlyHorn)
                        {
                            if (StateIsDifferent(bit: 1, out bool horn2Bit, states, prevStates))
                            {
                                inputs.Add(new Horn(time, horn2Bit));
                            }
                        }
                        else
                        {
                            if (StateIsDifferent(bit: 6, out bool horn, states, prevStates))
                            {
                                inputs.Add(new Horn(time, horn));
                            }

                            if (StateIsDifferent(bit: 13, out bool freeLook, states, prevStates))
                            {
                                inputs.Add(new FreeLook(time, freeLook));
                            }

                            for (var j = 0; j < 9; j++) // 9 action slots
                            {
                                // Action slots 2 and 4 have an additional bit 7 usage, which is ignored here
                                
                                if (StateIsDifferent(bit: 14 + j, out bool actionSlot, states, prevStates))
                                {
                                    inputs.Add(new ActionSlot(time, (byte)(1 + j), actionSlot));
                                }
                            }
                            
                            // + 1 action slot 0

                            if (StateIsDifferent(bit: 23, out bool actionSlot0, states, prevStates))
                            {
                                inputs.Add(new ActionSlot(time, 0, actionSlot0));
                            }
                            
                            if (StateIsDifferent(bit: 31, out bool _, states, prevStates))
                            {
                                inputs.Add(new RespawnTM2020(time));
                            }
                            
                            if (StateIsDifferent(bit: 33, out bool _, states, prevStates))
                            {
                                inputs.Add(new SecondaryRespawn(time));
                            }
                        }

                        prevStates = states;
                        prevOnlyHorn = onlyHorn;
                    }

                    var sameMouse = r.ReadBit();

                    if (!sameMouse)
                    {
                        var mouseAccuX = r.ReadUInt16();
                        var mouseAccuY = r.ReadUInt16();

                        inputs.Add(new MouseAccu(time, mouseAccuX, mouseAccuY));
                    }
                    
                    // In code, this check is presented as '(X - 2 & 0xfffffffd) == 0'

                    switch (started)
                    {
                        case EStart.Vehicle:
                            var sameVehicleValue = r.ReadBit();

                            if (sameVehicleValue)
                            {
                                break;
                            }
                            
                            var steer = r.ReadSByte();

                            if (steer != prevSteer)
                            {
                                inputs.Add(new SteerTM2020(time, steer));
                                prevSteer = steer;
                            }

                            var accel = r.ReadBit();

                            if (accel != prevAccel)
                            {
                                inputs.Add(new Accelerate(time, accel));
                                prevAccel = accel;
                            }

                            var brake = r.ReadBit();

                            if (brake != prevBrake)
                            {
                                inputs.Add(new Brake(time, brake));
                                prevBrake = brake;
                            }

                            break;

                        case EStart.Character:
                            var sameCharacterValue = r.ReadBit();

                            if (sameCharacterValue)
                            {
                                break;
                            }
                            
                            var strafe = (Inputs.EStrafe)r.Read2Bit();
                            if (strafe == NET.Inputs.EStrafe.Corrupted) throw new Exception("Corrupted inputs. (strafe == 2)");

                            if (strafe != prevStrafe)
                            {
                                inputs.Add(new Strafe(time, strafe));
                                prevStrafe = strafe;
                            }

                            var walk = (Inputs.EWalk)r.Read2Bit();
                            if (walk == NET.Inputs.EWalk.Corrupted) throw new Exception("Corrupted inputs. (walk == 2)");

                            if (walk != prevWalk)
                            {
                                inputs.Add(new Walk(time, walk));
                                prevWalk = walk;
                            }

                            var vertical = r.Read2Bit();

                            if (vertical != prevVertical)
                            {
                                inputs.Add(new Vertical(time, vertical));
                                prevVertical = vertical;
                            }

                            var horizontal = r.Read2Bit();

                            if (horizontal != prevHorizontal)
                            {
                                inputs.Add(new Horizontal(time, horizontal));
                                prevHorizontal = horizontal;
                            }

                            break;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    // TM2020 moment
                }
            }

            var theRest = r.ReadToEnd();

            if (theRest.Any(x => x != 0))
            {
                throw new Exception("Input buffer not cleared out completely");
            }

            return inputs;
        }

        internal IEnumerable<IInputChange> ProcessShootmaniaInputChanges()
        {
            var r = new BitReader(data);

            for (var i = 0; i < ticks; i++)
            {
                var different = false;

                var mouseAccuX = default(ushort?);
                var mouseAccuY = default(ushort?);
                var strafe = default(EStrafe?);
                var walk = default(EWalk?);
                var vertical = default(byte?);
                var states = default(int?);

                var sameMouse = r.ReadBit();

                if (!sameMouse)
                {
                    mouseAccuX = r.ReadUInt16();
                    mouseAccuY = r.ReadUInt16();

                    different = true;
                }

                var sameValuesAfter = r.ReadBit();
                var sameValue = sameValuesAfter;

                if (!sameValuesAfter)
                {
                    sameValue = r.ReadBit();
                }

                if (!sameValue)
                {
                    strafe = (EStrafe)r.Read2Bit();
                    if (strafe == EStrafe.Corrupted) throw new Exception("Corrupted inputs. (strafe == 2)");

                    walk = (EWalk)r.Read2Bit();
                    if (walk == EWalk.Corrupted) throw new Exception("Corrupted inputs. (walk == 2)");

                    if (version == EVersion._2017_09_12)
                    {
                        vertical = r.Read2Bit();
                    }

                    different = true;
                }

                if (!sameValuesAfter)
                {
                    sameValue = r.ReadBit();

                    if (!sameValue)
                    {
                        var onlyTriggerAndAction = r.ReadBit();

                        states = onlyTriggerAndAction
                            ? r.Read2Bit()
                            : r.ReadInt32();

                        different = true;
                    }
                }

                if (different)
                {
                    yield return new ShootmaniaInputChange(i, mouseAccuX, mouseAccuY, strafe, walk, vertical, states);
                }
            }

            var theRest = r.ReadToEnd();

            if (theRest.Any(x => x != 0))
            {
                throw new Exception("Input buffer not cleared out completely");
            }
        }

        internal IEnumerable<IInputChange> ProcessTrackmaniaInputChanges()
        {
            var r = new BitReader(data);

            var started = EStart.NotStarted;

            for (var i = 0; i < ticks; i++)
            {
                var different = false;

                var states = default(ulong?);
                var mouseAccuX = default(ushort?);
                var mouseAccuY = default(ushort?);
                var steer = default(sbyte?);
                var gas = default(bool?);
                var brake = default(bool?);
                var horn = default(bool?);
                var characterStates = default(byte?);

                try
                {
                    var sameState = r.ReadBit();

                    if (!sameState)
                    {
                        var onlyHorn = r.ReadBit();

                        states = onlyHorn
                            ? r.Read2Bit()
                            : r.ReadNumber(bits: version is EVersion._2020_04_08 ? 33 : 34);

                        if (started is EStart.NotStarted)
                        {
                            started = (EStart)(states & 3);
                            horn = (states & 64) != 0; // a weird bit that can appear sometimes during the run too
                        }
                        else if (started is EStart.Vehicle)
                        {
                            horn = onlyHorn
                                ? (states & 2) != 0
                                : (states & 64) != 0;
                        }

                        different = true;
                    }

                    var sameMouse = r.ReadBit();

                    if (!sameMouse)
                    {
                        mouseAccuX = r.ReadUInt16();
                        mouseAccuY = r.ReadUInt16();

                        different = true;
                    }

                    // This check is a bit weird, may not work for StormMan gameplay
                    // If starting with horn on, it is included on first tick
                    // If mouse is not plugged, it is also included
                    // In code, this check is presented as '(X - 2 & 0xfffffffd) == 0'

                    switch (started)
                    {
                        case EStart.Vehicle:
                            var sameVehicleValue = r.ReadBit();

                            if (!sameVehicleValue)
                            {
                                steer = r.ReadSByte();
                                gas = r.ReadBit();
                                brake = r.ReadBit();

                                different = true;
                            }

                            break;

                        case EStart.Character:
                            var sameCharacterValue = r.ReadBit();

                            if (!sameCharacterValue)
                            {
                                // Strafe2
                                // Walk2
                                // Vertical2
                                // Horiz2
                                characterStates = r.ReadByte();

                                different = true;
                            }

                            break;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    // TM2020 moment
                }

                if (different)
                {
                    yield return new TrackmaniaInputChange(i, states, mouseAccuX, mouseAccuY, steer, gas, brake, horn, characterStates);
                }
            }

            var theRest = r.ReadToEnd();

            if (theRest.Any(x => x != 0))
            {
                throw new Exception("Input buffer not cleared out completely");
            }
        }

        public enum EStrafe : byte
        {
            None,
            Left,
            Corrupted,
            Right
        }

        public enum EWalk : byte
        {
            None,
            Forward,
            Corrupted,
            Backward
        }

        public interface IInputChange
        {
            int Tick { get; }
            ushort? MouseAccuX { get; }
            ushort? MouseAccuY { get; }
            ulong? States { get; }
            bool? Respawn { get; }
            bool? Horn { get; }

            bool? FreeLook { get; }
            bool? ActionSlot1 { get; }
            bool? ActionSlot2 { get; }
            bool? ActionSlot3 { get; }
            bool? ActionSlot4 { get; }

            EStrafe? Strafe { get; }
            EWalk? Walk { get; }
            byte? Vertical { get; }

            TimeInt32 Timestamp { get; }
        }

        public readonly record struct ShootmaniaInputChange(int Tick,
                                                            ushort? MouseAccuX,
                                                            ushort? MouseAccuY,
                                                            EStrafe? Strafe,
                                                            EWalk? Walk,
                                                            byte? Vertical,
                                                            int? States) : IInputChange
        {
            public TimeInt32 Timestamp => new(Tick * 10);

            public bool? IsGunTrigger => States is null ? null : (States & 2) != 0;
            public bool? FreeLook => States is null ? null : (States & 4) != 0;
            public bool? Fly => States is null ? null : (States & 8) != 0;
            public bool? Camera2 => States is null ? null : (States & 32) != 0;
            public bool? Jump => States is null ? null : (States & 128) != 0;
            public bool? IsAction => States is null ? null : (States & 257) != 0;
            public bool? ActionSlot1 => States is null ? null : (States & 1024) != 0;
            public bool? ActionSlot2 => States is null ? null : (States & 2048) != 0;
            public bool? ActionSlot3 => States is null ? null : (States & 4096) != 0;
            public bool? ActionSlot4 => States is null ? null : (States & 8192) != 0;
            public bool? Use1 => States is null ? null : (States & 16896) != 0;
            public bool? Use2 => States is null ? null : (States & 32832) != 0;
            public bool? Menu => States is null ? null : (States & 65536) != 0;
            public bool? Horn => States is null ? null : (States & 1048576) != 0;
            public bool? Respawn => States is null ? null : (States & 2097152) != 0;
            public bool? GiveUp => States is null ? null : (States & (1 << 26)) != 0;

            ulong? IInputChange.States => States is null ? null : (ulong)States;
        }

        public readonly record struct TrackmaniaInputChange(int Tick,
                                                            ulong? States,
                                                            ushort? MouseAccuX,
                                                            ushort? MouseAccuY,
                                                            sbyte? Steer,
                                                            bool? Gas,
                                                            bool? Brake,
                                                            bool? Horn = null,
                                                            byte? CharacterStates = null) : IInputChange
        {
            public TimeInt32 Timestamp { get; } = new(Tick * 10);

            public bool? FreeLook => States is null ? null : (States & 8192) != 0; // bit 13
            public bool? ActionSlot1 => States is null ? null : (States & (1 << 14)) != 0;
            public bool? ActionSlot2 => States is null ? null : (States & 32896) != 0; // bit 15 (and bit 7?)
            public bool? ActionSlot3 => States is null ? null : (States & (1 << 16)) != 0;
            public bool? ActionSlot4 => States is null ? null : (States & 132096) != 0; // bit 17 (and bit 7?)
            public bool? ActionSlot5 => States is null ? null : (States & (1 << 18)) != 0;
            public bool? ActionSlot6 => States is null ? null : (States & 524288) != 0; // bit 19
            public bool? ActionSlot7 => States is null ? null : (States & (1 << 20)) != 0;
            public bool? ActionSlot8 => States is null ? null : (States & 2097152) != 0; // bit 21
            public bool? ActionSlot9 => States is null ? null : (States & (1 << 22)) != 0;
            public bool? ActionSlot0 => States is null ? null : (States & 8388608) != 0; // bit 23
            public bool? Respawn => States is null ? null : (States & 2147483648) != 0;
            public bool? SecondaryRespawn => States is null ? null : (States & 8589934592) != 0;
            
            public EStrafe? Strafe => CharacterStates is null ? null : (EStrafe)(CharacterStates & 3);
            public EWalk? Walk => CharacterStates is null ? null : (EWalk)((CharacterStates >> 2) & 3);
            public byte? Vertical => CharacterStates is null ? null : (byte)((CharacterStates >> 4) & 3);
            public byte? Horizontal => CharacterStates is null ? null : (byte)((CharacterStates >> 6) & 3);

        }
    }
}
