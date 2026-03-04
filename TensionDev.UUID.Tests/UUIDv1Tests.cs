using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Xunit;

namespace TensionDev.UUID.Tests
{
    public class UUIDv1Tests
    {
        [Fact]
        public void TestGetNodeID()
        {
            List<byte[]> expectedNodeIDs = [];
            int expectedLength = 6;
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface nic in nics)
            {
                expectedNodeIDs.Add(nic.GetPhysicalAddress().GetAddressBytes());
            }

            byte[] nodeID = UUIDv1.GetNodeID();

            if (nics.Length > 0)
                Assert.Contains(nodeID, expectedNodeIDs);
            else
                Assert.Equal(expectedLength, nodeID.Length);
        }

        [Fact]
        public void TestConsistentGetNodeID()
        {
            byte[] nodeID1 = UUIDv1.GetNodeID();
            byte[] nodeID2 = UUIDv1.GetNodeID();

            Assert.Equal(nodeID1, nodeID2);
        }

        [Fact]
        public void TestNewUUIDv1()
        {
            Uuid expectedUUID = new Uuid("164a714c-0c79-11ec-82a8-0242ac130003");

            byte[] nodeID = [0x02, 0x42, 0xac, 0x13, 0x00, 0x03];
            byte[] clockSequence = [0x82, 0xa8];
            DateTime dateTime = DateTime.Parse("2021-09-03T05:37:54.619630Z");
            Uuid uuid = UUIDv1.NewUUIDv1(dateTime, clockSequence, nodeID);

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
                    Byte[] vs = UUIDv1.GetClockSequence();
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
            IList<char> expectedVariantField = ['8', '9', 'a', 'b'];

            ConcurrentBag<String> concurrentBag = [];

            Parallel.For(0, UInt16.MaxValue,
                body =>
                {
                    concurrentBag.Add(UUIDv1.NewUUIDv1().ToString());
                });

            foreach (String value in concurrentBag)
            {
                Assert.Contains<char>(value[19], expectedVariantField);
            }
        }

        [Fact]
        public void TestNewUUIDv1NullClockSequence()
        {
            byte[] nodeID = [0x02, 0x42, 0xac, 0x13, 0x00, 0x03];
            byte[] clockSequence = null;
            Assert.Throws<ArgumentNullException>(() => UUIDv1.NewUUIDv1(DateTime.UtcNow, clockSequence, nodeID));
        }

        [Fact]
        public void TestNewUUIDv1ReducedClockSequence()
        {
            byte[] nodeID = [0x02, 0x42, 0xac, 0x13, 0x00, 0x03];
            byte[] clockSequence = [0x82];
            Assert.Throws<ArgumentException>(() => UUIDv1.NewUUIDv1(DateTime.UtcNow, clockSequence, nodeID));
        }

        [Fact]
        public void TestNewUUIDv1NullNodeID()
        {
            byte[] nodeID = null;
            byte[] clockSequence = [0x82, 0xa8];
            Assert.Throws<ArgumentNullException>(() => UUIDv1.NewUUIDv1(DateTime.UtcNow, clockSequence, nodeID));
        }

        [Fact]
        public void TestNewUUIDv1ReducedNodeID()
        {
            byte[] nodeID = [0x02, 0x42, 0xac, 0x13];
            byte[] clockSequence = [0x82, 0xa8];
            Assert.Throws<ArgumentException>(() => UUIDv1.NewUUIDv1(DateTime.UtcNow, clockSequence, nodeID));
        }

        [Fact]
        public void TestIsUUIDv1Withv1()
        {
            Uuid uuid = UUIDv1.NewUUIDv1();

            bool actual = UUIDv1.IsUUIDv1(uuid);
            Assert.True(actual);
        }

        [Fact]
        public void TestIsUUIDv1Withv3()
        {
            String name = "www.google.com";
            Uuid uuid = UUIDv3.NewUUIDv3(UUIDNamespace.DNS, name);

            bool actual = UUIDv1.IsUUIDv1(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv1Withv4()
        {
            Uuid uuid = UUIDv4.NewUUIDv4();

            bool actual = UUIDv1.IsUUIDv1(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv1Withv5()
        {
            String name = "www.contoso.com";
            Uuid uuid = UUIDv5.NewUUIDv5(UUIDNamespace.DNS, name);

            bool actual = UUIDv1.IsUUIDv1(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv1Withv6()
        {
            Uuid uuid = UUIDv6.NewUUIDv6();

            bool actual = UUIDv1.IsUUIDv1(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv1Withv7()
        {
            Uuid uuid = UUIDv7.NewUUIDv7();

            bool actual = UUIDv1.IsUUIDv1(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestToDateTime()
        {
            DateTime expected = DateTime.UtcNow;
            Uuid uuid = UUIDv1.NewUUIDv1(expected);

            DateTime actual = UUIDv1.ToDateTime(uuid);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestToUUIDv6()
        {
            DateTime expectedDateTime = DateTime.UtcNow;
            Uuid uuid1 = UUIDv1.NewUUIDv1(expectedDateTime);
            Uuid uuid6 = UUIDv1.ToUUIDv6(uuid1);

            DateTime actualDateTime = UUIDv6.ToDateTime(uuid6);
            bool actual = UUIDv6.IsUUIDv6(uuid6);
            Assert.Equal(expectedDateTime, actualDateTime);
            Assert.True(actual);
        }
    }
}
