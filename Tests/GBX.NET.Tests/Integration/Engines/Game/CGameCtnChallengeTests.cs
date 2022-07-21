namespace GBX.NET.Tests.Integration.Engines.Game;

public partial class CGameCtnChallengeTests
{
    public partial class Chunk03043003Tests
    {
        [ReadWriteEqualityTest(FirstIdOccurance = true), IgnoreReadWriteEqualityTest]
        public partial void ReadAndWrite_ManiaPlanet_2_DataShouldEqual();

        [ReadWriteEqualityTest(FirstIdOccurance = true)]
        public partial void ReadAndWrite_ManiaPlanet_Latest_DataShouldEqual();

        [ReadWriteEqualityTest(FirstIdOccurance = true), IgnoreReadWriteEqualityTest]
        public partial void ReadAndWrite_TMUF_DataShouldEqual();
    }

    public partial class Chunk0304300DTests
    {
        [ReadWriteEqualityTest(FirstIdOccurance = true)]
        public partial void ReadAndWrite_ManiaPlanet_2_DataShouldEqual();

        [ReadWriteEqualityTest(FirstIdOccurance = true)]
        public partial void ReadAndWrite_ManiaPlanet_Latest_DataShouldEqual();

        [ReadWriteEqualityTest(FirstIdOccurance = true)]
        public partial void ReadAndWrite_TMUF_DataShouldEqual();
    }

    public partial class Chunk0304301FTests
    {
        [IgnoreReadWriteEqualityTest]
        public partial void ReadAndWrite_ManiaPlanet_2_DataShouldEqual();

        [IgnoreReadWriteEqualityTest]
        public partial void ReadAndWrite_ManiaPlanet_Latest_DataShouldEqual();

        [IgnoreReadWriteEqualityTest]
        public partial void ReadAndWrite_TMUF_DataShouldEqual();
    }

    public partial class Chunk03043021Tests
    {
        [IgnoreReadWriteEqualityTest]
        public partial void ReadAndWrite_TMUF_DataShouldEqual();
    }
}
