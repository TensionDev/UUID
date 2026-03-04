using System;
using Xunit;

namespace TensionDev.UUID.Tests
{
    public class UuidTests
    {
        [Fact]
        public void TestEmptyUUID()
        {
            string expectedUUID = "00000000-0000-0000-0000-000000000000";

            Uuid uuid = Uuid.Empty;
            Assert.Equal(expectedUUID, uuid.ToString());
        }

        [Fact]
        public void TestMaxUUID()
        {
            string expectedUUID = "ffffffff-ffff-ffff-ffff-ffffffffffff";

            Uuid uuid = Uuid.Max;
            Assert.Equal(expectedUUID, uuid.ToString());
        }

        [Fact]
        public void TestConstructorByteArray1()
        {
            byte[] vs = null;
            Assert.Throws<ArgumentNullException>(() => { new Uuid(vs); });
        }

        [Fact]
        public void TestConstructorByteArray2()
        {
            byte[] vs = new byte[17];
            Assert.Throws<ArgumentException>(() => { new Uuid(vs); });
        }

        [Fact]
        public void TestConstructorByteArray3()
        {
            string expectedUUID = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            byte[] vs = [0x7d, 0x44, 0x48, 0x40, 0x9d, 0xc0, 0x11, 0xd1, 0xb2, 0x45, 0x5f, 0xfd, 0xce, 0x74, 0xfa, 0xd2];

            Uuid uuid = new Uuid(vs);
            Assert.Equal(expectedUUID, uuid.ToString());
        }

        [Fact]
        public void TestConstructorString1()
        {
            string vs = null;
            Assert.Throws<ArgumentNullException>(() => { new Uuid(vs); });
        }

        [Fact]
        public void TestConstructorString2()
        {
            string vs = "(7d444840-9dc0-11d1-b245-5ffdce74fad2}";
            Assert.Throws<FormatException>(() => { new Uuid(vs); });
        }

        [Fact]
        public void TestConstructorString3()
        {
            string expectedUUID = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            string vs = "7d444840-9dc0-11d1-b245-5ffdce74fad2";

            Uuid uuid = new Uuid(vs);
            Assert.Equal(expectedUUID, uuid.ToString());
        }

        [Fact]
        public void TestConstructorComponents1()
        {
            uint a = uint.MaxValue;
            ushort b = ushort.MaxValue;
            ushort c = ushort.MaxValue;
            byte d = byte.MaxValue;
            byte e = byte.MaxValue;
            byte[] f = null;
            Assert.Throws<ArgumentNullException>(() => { new Uuid(a, b, c, d, e, f); });
        }

        [Fact]
        public void TestConstructorComponents2()
        {
            uint a = uint.MinValue;
            ushort b = ushort.MinValue;
            ushort c = ushort.MinValue;
            byte d = byte.MinValue;
            byte e = byte.MinValue;
            byte[] f = new byte[5];
            Assert.Throws<ArgumentException>(() => { new Uuid(a, b, c, d, e, f); });
        }

        [Fact]
        public void TestConstructorComponents3()
        {
            string expectedUUID = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            uint a = 2101626944;
            ushort b = 40384;
            ushort c = 4561;
            byte d = 178;
            byte e = 69;
            byte[] f = [0x5f, 0xfd, 0xce, 0x74, 0xfa, 0xd2];

            Uuid uuid = new Uuid(a, b, c, d, e, f);
            Assert.Equal(expectedUUID, uuid.ToString());
        }

        [Fact]
        public void TestConstructorNodeBytes1()
        {
            string expectedUUID = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            uint a = 2101626944;
            ushort b = 40384;
            ushort c = 4561;
            byte d = 178;
            byte e = 69;
            byte f = 0x5f;
            byte g = 0xfd;
            byte h = 0xce;
            byte i = 0x74;
            byte j = 0xfa;
            byte k = 0xd2;

            Uuid uuid = new Uuid(a, b, c, d, e, f, g, h, i, j, k);
            Assert.Equal(expectedUUID, uuid.ToString());
        }

        [Fact]
        public void TestParse1()
        {
            string expectedUUID = "7d444840-9dc0-11d1-b245-5ffdce74fad2";

            Uuid uuid = Uuid.Parse(expectedUUID);
            Assert.Equal(expectedUUID, uuid.ToString());
        }

        [Fact]
        public void TestTryParse1()
        {
            string expectedUUID = "7d444840-9dc0-11d1-b245-5ffdce74fad2";

            bool result = Uuid.TryParse(expectedUUID, out Uuid uuid);
            Assert.Equal(expectedUUID, uuid.ToString());
            Assert.True(result);
        }

        [Fact]
        public void TestTryParse2()
        {
            string expectedUUID = "00000000-0000-0000-0000-000000000000";
            string vs = "(7d444840-9dc0-11d1-b245-5ffdce74fad2}";

            bool result = Uuid.TryParse(vs, out Uuid uuid);
            Assert.Equal(expectedUUID, uuid.ToString());
            Assert.False(result);
        }

        [Fact]
        public void TestCompareToObject()
        {
            object other = new object();
            string vs = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            Uuid uuid = Uuid.Parse(vs);

            int actualResult = uuid.CompareTo(other);
            Assert.True(actualResult > 0);
        }

        [Fact]
        public void TestCompareToUUIDObject()
        {
            int expectedResult = 0;
            string vs = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            object other = Uuid.Parse(vs);
            Uuid uuid = Uuid.Parse(vs);

            int actualResult = uuid.CompareTo(other);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void TestCompareToUUID()
        {
            int expectedResult = 0;
            string vs = "7d4448409dc011d1b2455ffdce74fad2";
            Uuid uuid1 = Uuid.Parse(vs);
            Uuid uuid2 = Uuid.Parse(vs);

            int actualResult = uuid1.CompareTo(uuid2);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void TestCompareToTimeLow()
        {
            Uuid uuid1 = Uuid.Parse("7d4448309dc011d1b2455ffdce74fad2");
            Uuid uuid2 = Uuid.Parse("7d4448409dc011d1b2455ffdce74fad2");

            int actualResult = uuid1.CompareTo(uuid2);
            Assert.True(actualResult < 0);
        }

        [Fact]
        public void TestCompareToTimeMid()
        {
            Uuid uuid1 = Uuid.Parse("7d4448409dd011d1b2455ffdce74fad2");
            Uuid uuid2 = Uuid.Parse("7d4448409dc011d1b2455ffdce74fad2");

            int actualResult = uuid1.CompareTo(uuid2);
            Assert.True(actualResult > 0);
        }

        [Fact]
        public void TestCompareToTimeHigh()
        {
            Uuid uuid1 = Uuid.Parse("7d4448409dc011c1b2455ffdce74fad2");
            Uuid uuid2 = Uuid.Parse("7d4448409dc011d1b2455ffdce74fad2");

            int actualResult = uuid1.CompareTo(uuid2);
            Assert.True(actualResult < 0);
        }

        [Fact]
        public void TestCompareToClockHigh()
        {
            Uuid uuid1 = Uuid.Parse("7d4448409dc011d1a2455ffdce74fad2");
            Uuid uuid2 = Uuid.Parse("7d4448409dc011d1b2455ffdce74fad2");

            int actualResult = uuid1.CompareTo(uuid2);
            Assert.True(actualResult < 0);
        }

        [Fact]
        public void TestCompareToClockLow()
        {
            Uuid uuid1 = Uuid.Parse("7d4448409dc011d1b2555ffdce74fad2");
            Uuid uuid2 = Uuid.Parse("7d4448409dc011d1b2455ffdce74fad2");

            int actualResult = uuid1.CompareTo(uuid2);
            Assert.True(actualResult > 0);
        }

        [Fact]
        public void TestCompareToNode1()
        {
            Uuid uuid1 = Uuid.Parse("7d4448409dc011d1b2455efdce74fad2");
            Uuid uuid2 = Uuid.Parse("7d4448409dc011d1b2455ffdce74fad2");

            int actualResult = uuid1.CompareTo(uuid2);
            Assert.True(actualResult < 0);
        }

        [Fact]
        public void TestCompareToNode2()
        {
            Uuid uuid1 = Uuid.Parse("7d4448409dc011d1b2455ffcce74fad2");
            Uuid uuid2 = Uuid.Parse("7d4448409dc011d1b2455ffdce74fad2");

            int actualResult = uuid1.CompareTo(uuid2);
            Assert.True(actualResult < 0);
        }

        [Fact]
        public void TestCompareToNode3()
        {
            Uuid uuid1 = Uuid.Parse("7d4448409dc011d1b2455ffdcd74fad2");
            Uuid uuid2 = Uuid.Parse("7d4448409dc011d1b2455ffdce74fad2");

            int actualResult = uuid1.CompareTo(uuid2);
            Assert.True(actualResult < 0);
        }

        [Fact]
        public void TestCompareToNode4()
        {
            Uuid uuid1 = Uuid.Parse("7d4448409dc011d1b2455ffdce73fad2");
            Uuid uuid2 = Uuid.Parse("7d4448409dc011d1b2455ffdce74fad2");

            int actualResult = uuid1.CompareTo(uuid2);
            Assert.True(actualResult < 0);
        }

        [Fact]
        public void TestCompareToNode5()
        {
            Uuid uuid1 = Uuid.Parse("7d4448409dc011d1b2455ffdce74fbd2");
            Uuid uuid2 = Uuid.Parse("7d4448409dc011d1b2455ffdce74fad2");

            int actualResult = uuid1.CompareTo(uuid2);
            Assert.True(actualResult > 0);
        }

        [Fact]
        public void TestCompareToNode6()
        {
            Uuid uuid1 = Uuid.Parse("7d4448409dc011d1b2455ffdce74faf2");
            Uuid uuid2 = Uuid.Parse("7d4448409dc011d1b2455ffdce74fad2");

            int actualResult = uuid1.CompareTo(uuid2);
            Assert.True(actualResult > 0);
        }

        [Fact]
        public void TestEquals1()
        {
            object other = new object();
            string vs = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            Uuid uuid = Uuid.Parse(vs);

            bool actualResult = uuid.Equals(other);
            Assert.False(actualResult);
        }

        [Fact]
        public void TestEquals2()
        {
            string vs = "{7d444840-9dc0-11d1-b245-5ffdce74fad2}";
            Uuid uuid1 = Uuid.Parse(vs);
            Uuid uuid2 = Uuid.Parse(vs);

            bool actualResult = uuid1.Equals(uuid2);
            Assert.True(actualResult);
        }

        [Fact]
        public void TestToByteArray3()
        {
            byte[] expected = [0x7d, 0x44, 0x48, 0x40, 0x9d, 0xc0, 0x11, 0xd1, 0xb2, 0x45, 0x5f, 0xfd, 0xce, 0x74, 0xfa, 0xd2];
            string vs = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            Uuid uuid = new Uuid(vs);

            byte[] actual = uuid.ToByteArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToGuid()
        {
            Guid expected = new Guid("7d444840-9dc0-11d1-b245-5ffdce74fad2");
            Uuid uuid = new Uuid("7d444840-9dc0-11d1-b245-5ffdce74fad2");

            Guid actual = uuid.ToGuid();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToVariant2()
        {
            Guid expected = new Guid("7d444840-9dc0-11d1-d245-5ffdce74fad2");
            Uuid uuid = new Uuid("7d444840-9dc0-11d1-b245-5ffdce74fad2");

            Guid actual = uuid.ToMicrosoftVariant();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToVariant1()
        {
            Uuid expected = new Uuid("7d444840-9dc0-11d1-9245-5ffdce74fad2");
            Guid guid = new Guid("7d444840-9dc0-11d1-d245-5ffdce74fad2");

            Uuid actual = Uuid.ToRfc4122Variant(guid);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToString1()
        {
            string expected = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            byte[] vs = [0x7d, 0x44, 0x48, 0x40, 0x9d, 0xc0, 0x11, 0xd1, 0xb2, 0x45, 0x5f, 0xfd, 0xce, 0x74, 0xfa, 0xd2];
            Uuid uuid = new Uuid(vs);

            string actual = uuid.ToString();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToString2()
        {
            string expected = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            byte[] vs = [0x7d, 0x44, 0x48, 0x40, 0x9d, 0xc0, 0x11, 0xd1, 0xb2, 0x45, 0x5f, 0xfd, 0xce, 0x74, 0xfa, 0xd2];
            Uuid uuid = new Uuid(vs);

            string actual = uuid.ToString(null);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToString3()
        {
            string expected = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            byte[] vs = [0x7d, 0x44, 0x48, 0x40, 0x9d, 0xc0, 0x11, 0xd1, 0xb2, 0x45, 0x5f, 0xfd, 0xce, 0x74, 0xfa, 0xd2];
            Uuid uuid = new Uuid(vs);

            string actual = uuid.ToString(String.Empty);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToHexString4()
        {
            string expected = "7d4448409dc011d1b2455ffdce74fad2";
            byte[] vs = [0x7d, 0x44, 0x48, 0x40, 0x9d, 0xc0, 0x11, 0xd1, 0xb2, 0x45, 0x5f, 0xfd, 0xce, 0x74, 0xfa, 0xd2];
            Uuid uuid = new Uuid(vs);

            string actual = uuid.ToString("N");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToString5()
        {
            string expected = "{7d444840-9dc0-11d1-b245-5ffdce74fad2}";
            byte[] vs = [0x7d, 0x44, 0x48, 0x40, 0x9d, 0xc0, 0x11, 0xd1, 0xb2, 0x45, 0x5f, 0xfd, 0xce, 0x74, 0xfa, 0xd2];
            Uuid uuid = new Uuid(vs);

            string actual = uuid.ToString("B");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToString6()
        {
            string expected = "(7d444840-9dc0-11d1-b245-5ffdce74fad2)";
            byte[] vs = [0x7d, 0x44, 0x48, 0x40, 0x9d, 0xc0, 0x11, 0xd1, 0xb2, 0x45, 0x5f, 0xfd, 0xce, 0x74, 0xfa, 0xd2];
            Uuid uuid = new Uuid(vs);

            string actual = uuid.ToString("P");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToString7()
        {
            byte[] vs = [0x7d, 0x44, 0x48, 0x40, 0x9d, 0xc0, 0x11, 0xd1, 0xb2, 0x45, 0x5f, 0xfd, 0xce, 0x74, 0xfa, 0xd2];
            Uuid uuid = new Uuid(vs);

            Assert.Throws<FormatException>(() => { uuid.ToString("C"); });
        }

        [Fact]
        public void TestOperatorEquals1()
        {
            object other = new object();
            string vs = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            Uuid uuid = Uuid.Parse(vs);

            bool actualResult = (uuid == other);
            Assert.False(actualResult);
        }

        [Fact]
        public void TestOperatorEquals2()
        {
            string vs = "{7d444840-9dc0-11d1-b245-5ffdce74fad2}";
            Uuid uuid1 = Uuid.Parse(vs);
            Uuid uuid2 = Uuid.Parse(vs);

            bool actualResult = uuid1 == uuid2;
            Assert.True(actualResult);
        }

        [Fact]
        public void TestOperatorEquals3()
        {
            Uuid other = null;
            Uuid uuid = null;

            bool actualResult = uuid == other;
            Assert.True(actualResult);
        }

        [Fact]
        public void TestOperatorEquals4()
        {
            string vs = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            Uuid other = null;
            Uuid uuid = Uuid.Parse(vs);

            bool actualResult = uuid == other;
            Assert.False(actualResult);
        }

        [Fact]
        public void TestOperatorEquals5()
        {
            string vs = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            Uuid other = Uuid.Parse(vs);
            Uuid uuid = null;

            bool actualResult = uuid == other;
            Assert.False(actualResult);
        }

        [Fact]
        public void TestOperatorNotEquals1()
        {
            object other = new object();
            string vs = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            Uuid uuid = Uuid.Parse(vs);

            bool actualResult = (uuid != other);
            Assert.True(actualResult);
        }

        [Fact]
        public void TestOperatorNotEquals2()
        {
            string vs = "{7d444840-9dc0-11d1-b245-5ffdce74fad2}";
            Uuid uuid1 = Uuid.Parse(vs);
            Uuid uuid2 = Uuid.Parse(vs);

            bool actualResult = uuid1 != uuid2;
            Assert.False(actualResult);
        }

        [Fact]
        public void TestOperatorNotEquals3()
        {
            Uuid other = null;
            string vs = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            Uuid uuid = Uuid.Parse(vs);

            bool actualResult = uuid != other;
            Assert.True(actualResult);
        }

        [Fact]
        public void TestOperatorLessThan1()
        {
            string vs1 = "7d444830-9dc0-11d1-b245-5ffdce74fad2";
            string vs2 = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            Uuid uuid1 = Uuid.Parse(vs1);
            Uuid uuid2 = Uuid.Parse(vs2);

            bool actualResult = uuid1 < uuid2;
            Assert.True(actualResult);
        }

        [Fact]
        public void TestOperatorLessThan2()
        {
            string vs = "{7d444840-9dc0-11d1-b245-5ffdce74fad2}";
            Uuid uuid1 = Uuid.Parse(vs);
            Uuid uuid2 = Uuid.Parse(vs);

            bool actualResult = uuid1 < uuid2;
            Assert.False(actualResult);
        }

        [Fact]
        public void TestOperatorLessThan3()
        {
            Uuid other = null;
            string vs = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            Uuid uuid = Uuid.Parse(vs);

            bool actualResult = uuid < other;
            Assert.False(actualResult);
        }

        [Fact]
        public void TestOperatorGreaterThan1()
        {
            string vs1 = "7d444830-9dc0-11d1-b245-5ffdce74fad2";
            string vs2 = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            Uuid uuid1 = Uuid.Parse(vs1);
            Uuid uuid2 = Uuid.Parse(vs2);

            bool actualResult = uuid1 > uuid2;
            Assert.False(actualResult);
        }

        [Fact]
        public void TestOperatorGreaterThan2()
        {
            string vs = "{7d444840-9dc0-11d1-b245-5ffdce74fad2}";
            Uuid uuid1 = Uuid.Parse(vs);
            Uuid uuid2 = Uuid.Parse(vs);

            bool actualResult = uuid1 > uuid2;
            Assert.False(actualResult);
        }

        [Fact]
        public void TestOperatorGreaterThan3()
        {
            Uuid other = null;
            string vs = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            Uuid uuid = Uuid.Parse(vs);

            bool actualResult = uuid > other;
            Assert.True(actualResult);
        }

        [Fact]
        public void TestOperatorLessThanOrEqual1()
        {
            string vs1 = "7d444830-9dc0-11d1-b245-5ffdce74fad2";
            string vs2 = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            Uuid uuid1 = Uuid.Parse(vs1);
            Uuid uuid2 = Uuid.Parse(vs2);

            bool actualResult = uuid1 <= uuid2;
            Assert.True(actualResult);
        }

        [Fact]
        public void TestOperatorLessThanOrEqual2()
        {
            string vs = "{7d444840-9dc0-11d1-b245-5ffdce74fad2}";
            Uuid uuid1 = Uuid.Parse(vs);
            Uuid uuid2 = Uuid.Parse(vs);

            bool actualResult = uuid1 <= uuid2;
            Assert.True(actualResult);
        }

        [Fact]
        public void TestOperatorLessThanOrEqual3()
        {
            Uuid other = null;
            string vs = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            Uuid uuid = Uuid.Parse(vs);

            bool actualResult = uuid <= other;
            Assert.False(actualResult);
        }

        [Fact]
        public void TestOperatorGreaterThanOrEqual1()
        {
            string vs1 = "7d444830-9dc0-11d1-b245-5ffdce74fad2";
            string vs2 = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            Uuid uuid1 = Uuid.Parse(vs1);
            Uuid uuid2 = Uuid.Parse(vs2);

            bool actualResult = uuid1 >= uuid2;
            Assert.False(actualResult);
        }

        [Fact]
        public void TestOperatorGreaterThanOrEqual2()
        {
            string vs = "{7d444840-9dc0-11d1-b245-5ffdce74fad2}";
            Uuid uuid1 = Uuid.Parse(vs);
            Uuid uuid2 = Uuid.Parse(vs);

            bool actualResult = uuid1 >= uuid2;
            Assert.True(actualResult);
        }

        [Fact]
        public void TestOperatorGreaterThanOrEqual3()
        {
            Uuid other = null;
            string vs = "7d444840-9dc0-11d1-b245-5ffdce74fad2";
            Uuid uuid = Uuid.Parse(vs);

            bool actualResult = uuid >= other;
            Assert.True(actualResult);
        }
    }
}
