namespace GBX.NET.Inputs;

public readonly partial record struct Accelerate
{
    IInput IInput.WithTime(TimeInt32 time) => new Accelerate(time, Pressed);
}

public readonly partial record struct AccelerateReal
{
    IInput IInput.WithTime(TimeInt32 time) => new AccelerateReal(time, Value);
}

public readonly partial record struct Action
{
    IInput IInput.WithTime(TimeInt32 time) => new Action(time, Pressed);
}

public readonly partial record struct ActionSlot
{
    IInput IInput.WithTime(TimeInt32 time) => new ActionSlot(time, Slot, Pressed);
}

public readonly partial record struct Brake
{
    IInput IInput.WithTime(TimeInt32 time) => new Brake(time, Pressed);
}

public readonly partial record struct BrakeReal
{
    IInput IInput.WithTime(TimeInt32 time) => new BrakeReal(time, Value);
}

public readonly partial record struct Camera2
{
    IInput IInput.WithTime(TimeInt32 time) => new Camera2(time, Pressed);
}

public readonly partial record struct FakeDontInverseAxis
{
    IInput IInput.WithTime(TimeInt32 time) => new FakeDontInverseAxis(time, Pressed);
}

public readonly partial record struct FakeFinishLine
{
    IInput IInput.WithTime(TimeInt32 time) => new FakeFinishLine(time, Data);
}

public readonly partial record struct FakeIsRaceRunning
{
    IInput IInput.WithTime(TimeInt32 time) => new FakeIsRaceRunning(time, Data);
}

public readonly partial record struct Fly
{
    IInput IInput.WithTime(TimeInt32 time) => new Fly(time, Pressed);
}

public readonly partial record struct FreeLook
{
    IInput IInput.WithTime(TimeInt32 time) => new FreeLook(time, Pressed);
}

public readonly partial record struct Gas
{
    IInput IInput.WithTime(TimeInt32 time) => new Gas(time, Value);
}

public readonly partial record struct GiveUp
{
    IInput IInput.WithTime(TimeInt32 time) => new GiveUp(time, Pressed);
}

public readonly partial record struct GunTrigger
{
    IInput IInput.WithTime(TimeInt32 time) => new GunTrigger(time, Pressed);
}

public readonly partial record struct Horizontal
{
    IInput IInput.WithTime(TimeInt32 time) => new Horizontal(time, Pressed);
}

public readonly partial record struct Horn
{
    IInput IInput.WithTime(TimeInt32 time) => new Horn(time, Pressed);
}

public readonly partial record struct Jump
{
    IInput IInput.WithTime(TimeInt32 time) => new Jump(time, Pressed);
}

public readonly partial record struct Menu
{
    IInput IInput.WithTime(TimeInt32 time) => new Menu(time, Pressed);
}

public readonly partial record struct MouseAccu
{
    IInput IInput.WithTime(TimeInt32 time) => new MouseAccu(time, X, Y);
}

public readonly partial record struct Respawn
{
    IInput IInput.WithTime(TimeInt32 time) => new Respawn(time, Pressed);
}

public readonly partial record struct RespawnTM2020
{
    IInput IInput.WithTime(TimeInt32 time) => new RespawnTM2020(time);
}

public readonly partial record struct SecondaryRespawn
{
    IInput IInput.WithTime(TimeInt32 time) => new SecondaryRespawn(time);
}

public readonly partial record struct Steer
{
    IInput IInput.WithTime(TimeInt32 time) => new Steer(time, Value);
}

public readonly partial record struct SteerLeft
{
    IInput IInput.WithTime(TimeInt32 time) => new SteerLeft(time, Pressed);
}

public readonly partial record struct SteerOld
{
    IInput IInput.WithTime(TimeInt32 time) => new SteerOld(time, Value);
}

public readonly partial record struct SteerRight
{
    IInput IInput.WithTime(TimeInt32 time) => new SteerRight(time, Pressed);
}

public readonly partial record struct SteerTM2020
{
    IInput IInput.WithTime(TimeInt32 time) => new SteerTM2020(time, Value);
}

public readonly partial record struct Strafe
{
    IInput IInput.WithTime(TimeInt32 time) => new Strafe(time, Pressed);
}

public readonly partial record struct UnknownInput
{
    IInput IInput.WithTime(TimeInt32 time) => new UnknownInput(time, Name, Data);
}

public readonly partial record struct Use
{
    IInput IInput.WithTime(TimeInt32 time) => new Use(time, Num, Pressed);
}

public readonly partial record struct Vertical
{
    IInput IInput.WithTime(TimeInt32 time) => new Vertical(time, Pressed);
}

public readonly partial record struct Walk
{
    IInput IInput.WithTime(TimeInt32 time) => new Walk(time, Pressed);
}
