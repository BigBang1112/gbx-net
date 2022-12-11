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

    [IgnoreReadWriteEqualityTest]
    public partial class Chunk0304301FTests
    {
        
    }

    public partial class Chunk03043021Tests
    {
        [IgnoreReadWriteEqualityTest]
        public partial void ReadAndWrite_TMUF_DataShouldEqual();
    }

    public partial class Chunk0304303ETests
    {
        [IgnoreReadWriteEqualityTest]
        public partial void ReadAndWrite_ManiaPlanet_2019_10_10_22_15_DataShouldEqual();
    }

#if NET462_OR_GREATER || NETCOREAPP3_1
    [IgnoreReadWriteEqualityTest]
    public partial class Chunk0304303DTests
    {

    }
#endif

    [IgnoreReadWriteEqualityTest]
    public partial class Chunk03043048Tests
    {

    }

    public partial class Chunk03043049Tests
    {
        [IgnoreReadWriteEqualityTest]
        public partial void ReadAndWrite_ManiaPlanet_2019_10_10_22_15_DataShouldEqual();
        
        [IgnoreReadWriteEqualityTest]
        public partial void ReadAndWrite_ManiaPlanet_3_Frozen_DataShouldEqual();
    }

    [IgnoreReadWriteEqualityTest]
    public partial class Chunk03043051Tests
    {
        
    }
}
