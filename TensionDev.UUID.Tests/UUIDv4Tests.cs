using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TensionDev.UUID.Tests
{
    public class UUIDv4Tests
    {
        [Fact]
        public void TestNewUUIDv4()
        {
            char expectedVersionField = '4';

            ConcurrentBag<String> concurrentBag = [];

            Parallel.For(0, UInt16.MaxValue,
                body =>
                {
                    concurrentBag.Add(UUIDv4.NewUUIDv4().ToString());
                });

            foreach (String value in concurrentBag)
            {
                Assert.Equal(value[14], expectedVersionField);
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
                    concurrentBag.Add(UUIDv4.NewUUIDv4().ToString());
                });

            foreach (String value in concurrentBag)
            {
                Assert.Contains<char>(value[19], expectedVariantField);
            }
        }

        [Fact]
        public void TestIsUUIDv4Withv1()
        {
            Uuid uuid = UUIDv1.NewUUIDv1();

            bool actual = UUIDv4.IsUUIDv4(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv4Withv3()
        {
            String name = "www.google.com";
            Uuid uuid = UUIDv3.NewUUIDv3(UUIDNamespace.DNS, name);

            bool actual = UUIDv4.IsUUIDv4(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv4Withv4()
        {
            Uuid uuid = UUIDv4.NewUUIDv4();

            bool actual = UUIDv4.IsUUIDv4(uuid);
            Assert.True(actual);
        }

        [Fact]
        public void TestIsUUIDv4Withv5()
        {
            String name = "www.contoso.com";
            Uuid uuid = UUIDv5.NewUUIDv5(UUIDNamespace.DNS, name);

            bool actual = UUIDv4.IsUUIDv4(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv4Withv6()
        {
            Uuid uuid = UUIDv6.NewUUIDv6();

            bool actual = UUIDv4.IsUUIDv4(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv4Withv7()
        {
            Uuid uuid = UUIDv7.NewUUIDv7();

            bool actual = UUIDv4.IsUUIDv4(uuid);
            Assert.False(actual);
        }
    }
}
