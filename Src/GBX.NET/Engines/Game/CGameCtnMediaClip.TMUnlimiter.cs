namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaClip
{
    public class TMUnlimiter
    {
        public LegacyResource? Resource { get; set; }

        public abstract record LegacyResource;

        public sealed record LegacyParameterSet : LegacyResource
        {
            public string? Name { get; set; }
            public Parameter[] Parameters { get; set; } = [];
        }

        public sealed record Parameter
        {
            public byte CatalogIndex { get; set; }
            public byte FunctionIndex { get; set; }
            public float Value { get; set; }
            public string? StringValue { get; set; }

            internal static bool IsStringParameter(uint functionIndex) => (CGameCtnChallenge.TMUnlimiter.ParameterName)functionIndex switch
            {
                CGameCtnChallenge.TMUnlimiter.ParameterName.World_ExecuteParameterSet
                or CGameCtnChallenge.TMUnlimiter.ParameterName.World_ExecuteScript
                or CGameCtnChallenge.TMUnlimiter.ParameterName.World_BlockGroupMakeVisible
                or CGameCtnChallenge.TMUnlimiter.ParameterName.World_BlockGroupMakeInvisible
                or CGameCtnChallenge.TMUnlimiter.ParameterName.World_BlockGroupMakeCollidable
                or CGameCtnChallenge.TMUnlimiter.ParameterName.World_BlockGroupMakeNonCollidable
                or CGameCtnChallenge.TMUnlimiter.ParameterName.Vehicle_Transform
                or CGameCtnChallenge.TMUnlimiter.ParameterName.Vehicle_SetVehicleTuningByName => true,
                _ => false
            };
        }

        public sealed record LegacyScript : LegacyResource
        {
            public string? Name { get; set; }
            public byte[] ByteCode { get; set; } = [];
        }
    }
}
