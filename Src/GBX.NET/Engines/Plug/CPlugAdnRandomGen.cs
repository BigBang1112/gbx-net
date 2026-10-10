using GBX.NET.Exceptions;

namespace GBX.NET.Engines.Plug;

public partial class CPlugAdnRandomGen
{
    private string? randomGenId;
    private int randSeed;
    private int modelCount = 1;
    private SPlugAdnRandomGenSet[] sets = [];

    /// <summary>
    /// The native Id member identifying the random generator.
    /// </summary>
    public string? RandomGenId { get => randomGenId; set => randomGenId = value; }
    public int RandSeed { get => randSeed; set => randSeed = value; }
    public int cModel { get => modelCount; set => modelCount = value; }
    public SPlugAdnRandomGenSet[] Sets { get => sets; set => sets = value; }

    public partial class Chunk09140000
    {
        public override void ReadWrite(CPlugAdnRandomGen n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version is < 0 or > 6)
            {
                throw new VersionNotSupportedException(Version);
            }

            if (Version >= 1)
            {
                rw.Int32(ref n.randSeed);
            }

            var count = Version >= 3 ? rw.Int32(n.sets.Length) : 1;
            if (count is < 0 or > 32)
            {
                throw new InvalidDataException("An ADN random generator supports up to 32 tag sets.");
            }
            if (rw.Reader is not null)
            {
                n.sets = new SPlugAdnRandomGenSet[count];
                for (var i = 0; i < count; i++)
                {
                    n.sets[i] = new SPlugAdnRandomGenSet();
                }
            }
            else if (Version < 3 && n.sets.Length != 1)
            {
                throw new InvalidDataException("ADN random generator versions below 3 require one tag set.");
            }

            foreach (var set in n.sets)
            {
                set.RequiredTags = rw.ArrayReadableWritable(set.RequiredTags, version: Version);
                set.ExcludedTags = rw.ArrayReadableWritable(set.ExcludedTags, version: Version);
            }

            if (Version >= 2)
            {
                rw.Int32(ref n.modelCount);
            }

            // Animation tags follow the model count in a separate pass over the sets.
            if (Version >= 4)
            {
                foreach (var set in n.sets)
                {
                    set.AnimRequiredTags = rw.ArrayReadableWritable(set.AnimRequiredTags, version: Version);
                }
            }

            if (Version >= 5)
            {
                rw.Id(ref n.randomGenId);
            }
        }
    }
}
