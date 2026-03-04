using System;
using Xunit;

namespace TensionDev.UUID.Tests
{
    public class UUIDv5Tests
    {
        [Fact]
        public void TestNewUUIDv5_DNS()
        {
            Uuid expectedGuid = new Uuid("016ab729-5c3f-55fe-9d10-e30cb958f0e0");

            String name = "www.contoso.com";
            Uuid guid = UUIDv5.NewUUIDv5(UUIDNamespace.DNS, name);

            Assert.Equal(expectedGuid, guid);
        }

        [Fact]
        public void TestNewUUIDv5_URL()
        {
            Uuid expectedGuid = new Uuid("1bf6935b-49e6-54cf-a9c8-51fb21c41b46");

            String name = "https://www.contoso.com";
            Uuid guid = UUIDv5.NewUUIDv5(UUIDNamespace.URL, name);

            Assert.Equal(expectedGuid, guid);
        }

        [Fact]
        public void TestNewUUIDv5_OID()
        {
            Uuid expectedGuid = new Uuid("8d3d0edc-5b53-56a7-953b-f11fbc48388a");

            String name = "1.0.3166.1";
            Uuid guid = UUIDv5.NewUUIDv5(UUIDNamespace.OID, name);

            Assert.Equal(expectedGuid, guid);
        }

        [Fact]
        public void TestNewUUIDv5_X500()
        {
            Uuid expectedGuid = new Uuid("61c75a15-eedd-5349-ab65-b35f0ef81d7c");

            String name = "/c=us/o=Sun/ou=People/cn=Rosanna Lee";
            Uuid guid = UUIDv5.NewUUIDv5(UUIDNamespace.X500, name);

            Assert.Equal(expectedGuid, guid);
        }

        [Fact]
        public void TestIsUUIDv5Withv1()
        {
            Uuid uuid = UUIDv1.NewUUIDv1();

            bool actual = UUIDv5.IsUUIDv5(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv5Withv3()
        {
            String name = "www.google.com";
            Uuid uuid = UUIDv3.NewUUIDv3(UUIDNamespace.DNS, name);

            bool actual = UUIDv5.IsUUIDv5(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv5Withv4()
        {
            Uuid uuid = UUIDv4.NewUUIDv4();

            bool actual = UUIDv5.IsUUIDv5(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv5Withv5()
        {
            String name = "www.contoso.com";
            Uuid uuid = UUIDv5.NewUUIDv5(UUIDNamespace.DNS, name);

            bool actual = UUIDv5.IsUUIDv5(uuid);
            Assert.True(actual);
        }

        [Fact]
        public void TestIsUUIDv5Withv6()
        {
            Uuid uuid = UUIDv6.NewUUIDv6();

            bool actual = UUIDv5.IsUUIDv5(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv5Withv7()
        {
            Uuid uuid = UUIDv7.NewUUIDv7();

            bool actual = UUIDv5.IsUUIDv5(uuid);
            Assert.False(actual);
        }
    }
}
