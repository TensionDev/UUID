using System;
using Xunit;

namespace TensionDev.UUID.Tests
{
    public class UUIDv8Tests
    {
        [Fact]
        public void TestIsUUIDv8()
        {
            Uuid uuid = new Uuid("2489E9AD-2EE2-8E00-8EC9-32D5F69181C0");

            bool actual = UUIDv8.IsUUIDv8(uuid);
            Assert.True(actual);
        }

        [Fact]
        public void TestIsUUIDv8Withv1()
        {
            Uuid uuid = UUIDv1.NewUUIDv1();

            bool actual = UUIDv8.IsUUIDv8(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv8Withv3()
        {
            String name = "www.google.com";
            Uuid uuid = UUIDv3.NewUUIDv3(UUIDNamespace.DNS, name);

            bool actual = UUIDv8.IsUUIDv8(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv8Withv4()
        {
            Uuid uuid = UUIDv4.NewUUIDv4();

            bool actual = UUIDv8.IsUUIDv8(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv8Withv5()
        {
            String name = "www.contoso.com";
            Uuid uuid = UUIDv5.NewUUIDv5(UUIDNamespace.DNS, name);

            bool actual = UUIDv8.IsUUIDv8(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv8Withv6()
        {
            Uuid uuid = UUIDv6.NewUUIDv6();

            bool actual = UUIDv8.IsUUIDv8(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv8Withv7()
        {
            Uuid uuid = UUIDv7.NewUUIDv7();

            bool actual = UUIDv8.IsUUIDv8(uuid);
            Assert.False(actual);
        }
    }
}
