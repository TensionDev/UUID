using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TensionDev.UUID.Tests
{
    public class UUIDv6Tests
    {
        [Fact]
        public void TestGetNodeID()
        {
            int expectedLength = 6;
            byte[] nodeID = UUIDv6.GetNodeID();

            Assert.Equal(expectedLength, nodeID.Length);
        }

        [Fact]
        public void TestRandomGetNodeID()
        {
            byte[] nodeID1 = UUIDv6.GetNodeID();
            byte[] nodeID2 = UUIDv6.GetNodeID();

            Assert.NotEqual(nodeID1, nodeID2);
        }

        [Fact]
        public void TestNewUUIDv6()
        {
            Uuid expectedUUID = new Uuid("1ec9414c-232a-6b00-82a8-0242ac130003");

            byte[] nodeID = [0x02, 0x42, 0xac, 0x13, 0x00, 0x03];
            byte[] clockSequence = [0x82, 0xa8];
            DateTime dateTime = DateTime.Parse("2022-02-22T19:22:22.000000Z");
            Uuid uuid = UUIDv6.NewUUIDv6(dateTime, clockSequence, nodeID);

            Assert.Equal(expectedUUID, uuid);
        }

        [Fact]
        public void TestGetClockSequence()
        {
            ConcurrentDictionary<UInt16, Boolean> concurrentDictionary = new ConcurrentDictionary<UInt16, Boolean>();
            Int32 expectedMaxSequence = 0x4000;

            Parallel.For(0, UInt16.MaxValue,
                clock =>
                {
                    Byte[] vs = UUIDv6.GetClockSequence();
                    Int16 networkorder = BitConverter.ToInt16(vs);
                    UInt16 key = (UInt16)System.Net.IPAddress.NetworkToHostOrder(networkorder);
                    concurrentDictionary.TryAdd(key, true);
                });

            Assert.Equal(expectedMaxSequence, concurrentDictionary.Values.Count);
            ICollection<UInt16> keys = concurrentDictionary.Keys;
            foreach (UInt16 key in keys)
            {
                Assert.InRange(key, 0x8000, 0xBFFF);
            }
        }

        [Fact]
        public void TestUUIDVariantField()
        {
            List<char> expectedVariantField = ['8', '9', 'a', 'b'];

            ConcurrentBag<String> concurrentBag = [];

            Parallel.For(0, UInt16.MaxValue,
                body =>
                {
                    concurrentBag.Add(UUIDv6.NewUUIDv6().ToString());
                });

            foreach (String value in concurrentBag)
            {
                Assert.Contains<char>(value[19], expectedVariantField);
            }
        }

        [Fact]
        public void TestNewUUIDv6NullClockSequence()
        {
            byte[] nodeID = [0x02, 0x42, 0xac, 0x13, 0x00, 0x03];
            byte[] clockSequence = null;
            Assert.Throws<ArgumentNullException>(() => UUIDv6.NewUUIDv6(DateTime.UtcNow, clockSequence, nodeID));
        }

        [Fact]
        public void TestNewUUIDv6ReducedClockSequence()
        {
            byte[] nodeID = [0x02, 0x42, 0xac, 0x13, 0x00, 0x03];
            byte[] clockSequence = [0x82];
            Assert.Throws<ArgumentException>(() => UUIDv6.NewUUIDv6(DateTime.UtcNow, clockSequence, nodeID));
        }

        [Fact]
        public void TestNewUUIDv6NullNodeID()
        {
            byte[] nodeID = null;
            byte[] clockSequence = [0x82, 0xa8];
            Assert.Throws<ArgumentNullException>(() => UUIDv6.NewUUIDv6(DateTime.UtcNow, clockSequence, nodeID));
        }

        [Fact]
        public void TestNewUUIDv6ReducedNodeID()
        {
            byte[] nodeID = [0x02, 0x42, 0xac, 0x13];
            byte[] clockSequence = [0x82, 0xa8];
            Assert.Throws<ArgumentException>(() => UUIDv6.NewUUIDv6(DateTime.UtcNow, clockSequence, nodeID));
        }

        [Fact]
        public void TestIsUUIDv6Withv1()
        {
            Uuid uuid = UUIDv1.NewUUIDv1();

            bool actual = UUIDv6.IsUUIDv6(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv6Withv3()
        {
            String name = "www.google.com";
            Uuid uuid = UUIDv3.NewUUIDv3(UUIDNamespace.DNS, name);

            bool actual = UUIDv6.IsUUIDv6(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv6Withv4()
        {
            Uuid uuid = UUIDv4.NewUUIDv4();

            bool actual = UUIDv6.IsUUIDv6(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv6Withv5()
        {
            String name = "www.contoso.com";
            Uuid uuid = UUIDv5.NewUUIDv5(UUIDNamespace.DNS, name);

            bool actual = UUIDv6.IsUUIDv6(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv6Withv6()
        {
            Uuid uuid = UUIDv6.NewUUIDv6();

            bool actual = UUIDv6.IsUUIDv6(uuid);
            Assert.True(actual);
        }

        [Fact]
        public void TestIsUUIDv6Withv7()
        {
            Uuid uuid = UUIDv7.NewUUIDv7();

            bool actual = UUIDv6.IsUUIDv6(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestToDateTime()
        {
            DateTime expected = DateTime.UtcNow;
            Uuid uuid = UUIDv6.NewUUIDv6(expected);

            DateTime actual = UUIDv6.ToDateTime(uuid);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToUUIDv1()
        {
            DateTime expectedDateTime = DateTime.UtcNow;
            Uuid uuid6 = UUIDv6.NewUUIDv6(expectedDateTime);
            Uuid uuid1 = UUIDv6.ToUUIDv1(uuid6);

            DateTime actualDateTime = UUIDv1.ToDateTime(uuid1);
            bool actual = UUIDv1.IsUUIDv1(uuid1);
            Assert.Equal(expectedDateTime, actualDateTime);
            Assert.True(actual);
        }
    }
}
