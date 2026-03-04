using System;
using Xunit;

namespace TensionDev.UUID.Tests
{
    public class UUIDv3Tests
    {
        [Fact]
        public void TestNewUUIDv3_DNS()
        {
            Uuid expectedGuid = new Uuid("de87628d-5377-3ba7-b31b-cde1cc8d423f");

            String name = "www.google.com";
            Uuid guid = UUIDv3.NewUUIDv3(UUIDNamespace.DNS, name);

            Assert.Equal(expectedGuid, guid);
        }

        [Fact]
        public void TestNewUUIDv3_URL()
        {
            Uuid expectedGuid = new Uuid("d39a36cc-b262-3c67-a6ca-0168e948bdd4");

            String name = "https://www.google.com";
            Uuid guid = UUIDv3.NewUUIDv3(UUIDNamespace.URL, name);

            Assert.Equal(expectedGuid, guid);
        }

        [Fact]
        public void TestNewUUIDv3_OID()
        {
            Uuid expectedGuid = new Uuid("ef4dc0a0-9fc8-368e-9413-0bbf811aca7b");

            String name = "1.0.3166.1";
            Uuid guid = UUIDv3.NewUUIDv3(UUIDNamespace.OID, name);

            Assert.Equal(expectedGuid, guid);
        }

        [Fact]
        public void TestNewUUIDv3_X500()
        {
            Uuid expectedGuid = new Uuid("87d4875f-af5a-3491-8c26-cef5a0d16aa0");

            String name = "/c=us/o=Sun/ou=People/cn=Rosanna Lee";
            Uuid guid = UUIDv3.NewUUIDv3(UUIDNamespace.X500, name);

            Assert.Equal(expectedGuid, guid);
        }

        [Fact]
        public void TestIsUUIDv3Withv1()
        {
            Uuid uuid = UUIDv1.NewUUIDv1();

            bool actual = UUIDv3.IsUUIDv3(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv3Withv3()
        {
            String name = "www.google.com";
            Uuid uuid = UUIDv3.NewUUIDv3(UUIDNamespace.DNS, name);

            bool actual = UUIDv3.IsUUIDv3(uuid);
            Assert.True(actual);
        }

        [Fact]
        public void TestIsUUIDv3Withv4()
        {
            Uuid uuid = UUIDv4.NewUUIDv4();

            bool actual = UUIDv3.IsUUIDv3(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv3Withv5()
        {
            String name = "www.contoso.com";
            Uuid uuid = UUIDv5.NewUUIDv5(UUIDNamespace.DNS, name);

            bool actual = UUIDv3.IsUUIDv3(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv3Withv6()
        {
            Uuid uuid = UUIDv6.NewUUIDv6();

            bool actual = UUIDv3.IsUUIDv3(uuid);
            Assert.False(actual);
        }

        [Fact]
        public void TestIsUUIDv3Withv7()
        {
            Uuid uuid = UUIDv7.NewUUIDv7();

            bool actual = UUIDv3.IsUUIDv3(uuid);
            Assert.False(actual);
        }
    }
}
