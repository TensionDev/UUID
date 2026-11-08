using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TensionDev.UUID.Tests
{
    public class UUIDv7Tests
    {
        [Fact]
        public void TestGetRandomA()
        {
            byte[] randA = UUIDv7.GetRandomA();

            Assert.Equal(2, randA.Length);
        }

        [Fact]
        public void TestRandomGetRandomA()
        {
            byte[] randA1 = UUIDv7.GetRandomA();
            byte[] randA2 = UUIDv7.GetRandomA();

            Assert.NotEqual(randA1, randA2);
        }

        [Fact]
        public void TestGetFixedBitLengthDedicatedCounterA()
        {
            byte[] counterA = UUIDv7.GetFixedBitLengthDedicatedCounterA();

            Assert.Equal(2, counterA.Length);
        }

        [Fact]
        public void TestUniqueGetFixedBitLengthDedicatedCounterA()
        {
            ConcurrentBag<Byte[]> countersA = [];

            Parallel.For(0, 0x1000,
                counter =>
                {
                    countersA.Add(UUIDv7.GetFixedBitLengthDedicatedCounterA());
                });

            IEnumerable<Byte[]> distinctCounters = countersA.Distinct();

            Assert.Equal(distinctCounters.Count(), countersA.Count);
            Assert.Equal(0x1000, distinctCounters.Count());
            Assert.Equal(0x1000, countersA.Count);
        }

        [Fact]
        public void TestOverflowGetCounterA()
        {
            ConcurrentBag<Byte[]> countersA = [];

            Parallel.For(0, 0x4000,
                counter =>
                {
                    countersA.Add(UUIDv7.GetFixedBitLengthDedicatedCounterA());
                });

            IEnumerable<Int16> distinctCounters = countersA.Select(m => BitConverter.ToInt16(m)).Distinct();

            Assert.Equal(0x1000, distinctCounters.Count());
            Assert.Equal(0x4000, countersA.Count);
        }

        [Fact]
        public void TestGetIncreasedClockPrecisionA()
        {
            DateTime dateTime = DateTime.Now;
            byte[] counterA = UUIDv7.GetIncreasedClockPrecisionA(dateTime);

            Assert.Equal(2, counterA.Length);
        }

        [Fact]
        public void TestOverflowGetIncreasedClockPrecisionA()
        {
            ConcurrentBag<Byte[]> countersA = [];

            DateTime start = DateTime.Now;
            DateTime end = start.AddMilliseconds(1);

            Parallel.For(start.Ticks, end.Ticks,
                ticks =>
                {
                    DateTime dateTime = new DateTime(ticks, DateTimeKind.Local);
                    countersA.Add(UUIDv7.GetIncreasedClockPrecisionA(dateTime));
                });

            IEnumerable<Int16> distinctCounters = countersA.Select(m => BitConverter.ToInt16(m)).Distinct();

            Assert.Equal(0x1000, distinctCounters.Count());
            Assert.Equal(10000, countersA.Count);
        }

        [Fact]
        public void TestGetRandomB()
        {
            byte[] randB = UUIDv7.GetRandomB();

            Assert.Equal(8, randB.Length);
        }

        [Fact]
        public void TestRandomGetRandomB()
        {
            byte[] randB1 = UUIDv7.GetRandomB();
            byte[] randB2 = UUIDv7.GetRandomB();

            Assert.NotEqual(randB1, randB2);
        }

        [Fact]
        public void TestNewUUIDv7()
        {
            Uuid expectedUUID = new Uuid("017f22e2-79B0-7cc3-98c4-dc0c0c07398f");

            byte[] randA = [0x7c, 0xc3];
            byte[] randB = [0x98, 0xc4, 0xdc, 0x0c, 0x0c, 0x07, 0x39, 0x8f];
            DateTime dateTime = DateTime.Parse("2022-02-22T19:22:22.000000Z");
            Uuid uuid = UUIDv7.NewUUIDv7(dateTime, randA, randB);

            Assert.Equal(expectedUUID, uuid);
        }

        [Fact]
        public void TestMethod1CounterIsMonotonicAndHandlesRollover()
        {
            DateTime timestamp = DateTime.UtcNow.AddYears(1);
            Uuid previous = UUIDv7.NewUUIDv7(timestamp, UUIDv7.GenerationMethod.Method1);
            DateTime firstTimestamp = UUIDv7.ToDateTime(previous);
            Uuid current = UUIDv7.NewUUIDv7(timestamp, UUIDv7.GenerationMethod.Method1);
            Assert.True(previous.CompareTo(current) < 0);

            previous = current;
            current = UUIDv7.NewUUIDv7(timestamp.AddMilliseconds(1), UUIDv7.GenerationMethod.Method1);
            Assert.True(previous.CompareTo(current) < 0);
            DateTime advancedTimestamp = UUIDv7.ToDateTime(current);
            Assert.Equal(firstTimestamp.AddMilliseconds(1), advancedTimestamp);

            // A backwards requested timestamp must continue from the last emitted timestamp.
            previous = current;
            current = UUIDv7.NewUUIDv7(timestamp, UUIDv7.GenerationMethod.Method1);
            Assert.True(previous.CompareTo(current) < 0);
            Assert.Equal(advancedTimestamp, UUIDv7.ToDateTime(current));

            previous = current;
            for (int i = 0; i < 0x1001; ++i)
            {
                current = UUIDv7.NewUUIDv7(timestamp.AddMilliseconds(1), UUIDv7.GenerationMethod.Method1);
                Assert.True(previous.CompareTo(current) < 0);
                previous = current;
            }

            Assert.True(UUIDv7.ToDateTime(current) > firstTimestamp);
        }

        [Fact]
        public void TestNewUUIDv7RejectsDateBeforeUnixEpoch()
        {
            byte[] randomA = [0x00, 0x00];
            byte[] randomB = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                UUIDv7.NewUUIDv7(DateTime.UnixEpoch.AddTicks(-1), randomA, randomB));
        }

        [Fact]
        public void TestMethod3Generation()
        {
            Uuid uuid = UUIDv7.NewUUIDv7(DateTime.UtcNow, UUIDv7.GenerationMethod.Method3);

            Assert.True(UUIDv7.IsUUIDv7(uuid));
        }

        [Fact]
        public void TestUUIDVariantField()
        {
            IList<char> expectedVariantField = ['8', '9', 'a', 'b'];

            ConcurrentBag<String> concurrentBag = [];

            Parallel.For(0, UInt16.MaxValue,
                body =>
                {
                    concurrentBag.Add(UUIDv7.NewUUIDv7().ToString());
                });

            foreach (String value in concurrentBag)
            {
                Assert.Contains<char>(value[19], expectedVariantField);
            }
        }

        [Fact]
        public void TestNewUUIDv7NullRandomA()
        {
            byte[] randA = null;
            byte[] randB = [0x98, 0xc4, 0xdc, 0x0c, 0x0c, 0x07, 0x39, 0x8f];
            Assert.Throws<ArgumentNullException>(() => UUIDv7.NewUUIDv7(DateTime.UtcNow, randA, randB));
        }

        [Fact]
        public void TestNewUUIDv7ReducedRandomA()
        {
            byte[] randA = [0x7c];
            byte[] randB = [0x98, 0xc4, 0xdc, 0x0c, 0x0c, 0x07, 0x39, 0x8f];
            Assert.Throws<ArgumentException>(() => UUIDv7.NewUUIDv7(DateTime.UtcNow, randA, randB));
        }

        [Fact]
        public void TestNewUUIDv7NullRandomB()
        {
            byte[] randA = [0x7c, 0xc3];
            byte[] randB = null;
            Assert.Throws<ArgumentNullException>(() => UUIDv7.NewUUIDv7(DateTime.UtcNow, randA, randB));
        }

        [Fact]
        public void TestNewUUIDv7ReducedRandomB()
        {
            byte[] randA = [0x7c, 0xc3];
            byte[] randB = [0x98, 0xc4, 0xdc, 0x0c, 0x0c, 0x07, 0x39];
            Assert.Throws<ArgumentException>(() => UUIDv7.NewUUIDv7(DateTime.UtcNow, randA, randB));
        }

        [Fact]
        public void TestIsUUIDv7Withv1()
        {
            Uuid uuid = UUIDv1.NewUUIDv1();

            bool actual = UUIDv7.IsUUIDv7(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv7Withv3()
        {
            String name = "www.google.com";
            Uuid uuid = UUIDv3.NewUUIDv3(UUIDNamespace.DNS, name);

            bool actual = UUIDv7.IsUUIDv7(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv7Withv4()
        {
            Uuid uuid = UUIDv4.NewUUIDv4();

            bool actual = UUIDv7.IsUUIDv7(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv7Withv5()
        {
            String name = "www.contoso.com";
            Uuid uuid = UUIDv5.NewUUIDv5(UUIDNamespace.DNS, name);

            bool actual = UUIDv7.IsUUIDv7(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv7Withv6()
        {
            Uuid uuid = UUIDv6.NewUUIDv6();

            bool actual = UUIDv7.IsUUIDv7(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv7Withv7()
        {
            Uuid uuid = UUIDv7.NewUUIDv7();

            bool actual = UUIDv7.IsUUIDv7(uuid);
            Assert.True(actual);
        }

        [Fact]
        public void TestToDateTime()
        {
            DateTime expected = DateTime.UtcNow;
            Uuid uuid = UUIDv7.NewUUIDv7(expected);

            DateTime actual = UUIDv7.ToDateTime(uuid);
            Assert.Equal(expected, actual, TimeSpan.FromMilliseconds(1));
        }
    }
}
