namespace GBX.NET.Engines.GameData;

public partial class CGameActionModel
{
    [Obsolete("Use ActionName instead.")]
    public string? Name { get => ActionName; set => ActionName = value; }

    public partial class SpriteBlock
    {
        [Obsolete("Use Image instead.")]
        public Plug.CPlugFileImg? U02 { get => Image; set => Image = value; }
    }

    public partial class ScriptParams
    {
        [Obsolete("Use Cooldown instead.")]
        public int U01 { get => Cooldown; set => Cooldown = value; }

        [Obsolete("Use EnergyCost instead.")]
        public int U02 { get => EnergyCost; set => EnergyCost = value; }

        [Obsolete("Use EnergyMax instead.")]
        public int U03 { get => EnergyMax; set => EnergyMax = value; }

        [Obsolete("Use Cooldown instead.")]
        public int U24 { get => Cooldown; set => Cooldown = value; }

        [Obsolete("Use EnergyCost instead.")]
        public int U25 { get => EnergyCost; set => EnergyCost = value; }

        [Obsolete("Use EnergyMax instead.")]
        public int U26 { get => EnergyMax; set => EnergyMax = value; }

        [Obsolete("Use U07 instead.")]
        public bool U27 { get => U07; set => U07 = value; }

        [Obsolete("Use EnergyReload instead.")]
        public bool U28 { get => EnergyReload; set => EnergyReload = value; }
    }
}
