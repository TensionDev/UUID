using System;
using Xunit;

namespace TensionDev.UUID.Tests
{
    public class UUIDNamespaceTests
    {
        [Fact]
        public void TestUUIDNamespace_DNS()
        {
            Byte[] guidBytes = UUIDNamespace.DNS.ToByteArray();

            Assert.Equal(0x6b, guidBytes[0]);
            Assert.Equal(0xa7, guidBytes[1]);
            Assert.Equal(0xb8, guidBytes[2]);
            Assert.Equal(0x10, guidBytes[3]);
            Assert.Equal(0x9d, guidBytes[4]);
            Assert.Equal(0xad, guidBytes[5]);
            Assert.Equal(0x11, guidBytes[6]);
            Assert.Equal(0xd1, guidBytes[7]);
            Assert.Equal(0x80, guidBytes[8]);
            Assert.Equal(0xb4, guidBytes[9]);
            Assert.Equal(0x00, guidBytes[10]);
            Assert.Equal(0xc0, guidBytes[11]);
            Assert.Equal(0x4f, guidBytes[12]);
            Assert.Equal(0xd4, guidBytes[13]);
            Assert.Equal(0x30, guidBytes[14]);
            Assert.Equal(0xc8, guidBytes[15]);
        }
        [Fact]
        public void TestUUIDNamespace_URL()
        {
            Byte[] guidBytes = UUIDNamespace.URL.ToByteArray();

            Assert.Equal(0x6b, guidBytes[0]);
            Assert.Equal(0xa7, guidBytes[1]);
            Assert.Equal(0xb8, guidBytes[2]);
            Assert.Equal(0x11, guidBytes[3]);
            Assert.Equal(0x9d, guidBytes[4]);
            Assert.Equal(0xad, guidBytes[5]);
            Assert.Equal(0x11, guidBytes[6]);
            Assert.Equal(0xd1, guidBytes[7]);
            Assert.Equal(0x80, guidBytes[8]);
            Assert.Equal(0xb4, guidBytes[9]);
            Assert.Equal(0x00, guidBytes[10]);
            Assert.Equal(0xc0, guidBytes[11]);
            Assert.Equal(0x4f, guidBytes[12]);
            Assert.Equal(0xd4, guidBytes[13]);
            Assert.Equal(0x30, guidBytes[14]);
            Assert.Equal(0xc8, guidBytes[15]);
        }
        [Fact]
        public void TestUUIDNamespace_ISOOID()
        {
            Byte[] guidBytes = UUIDNamespace.OID.ToByteArray();

            Assert.Equal(0x6b, guidBytes[0]);
            Assert.Equal(0xa7, guidBytes[1]);
            Assert.Equal(0xb8, guidBytes[2]);
            Assert.Equal(0x12, guidBytes[3]);
            Assert.Equal(0x9d, guidBytes[4]);
            Assert.Equal(0xad, guidBytes[5]);
            Assert.Equal(0x11, guidBytes[6]);
            Assert.Equal(0xd1, guidBytes[7]);
            Assert.Equal(0x80, guidBytes[8]);
            Assert.Equal(0xb4, guidBytes[9]);
            Assert.Equal(0x00, guidBytes[10]);
            Assert.Equal(0xc0, guidBytes[11]);
            Assert.Equal(0x4f, guidBytes[12]);
            Assert.Equal(0xd4, guidBytes[13]);
            Assert.Equal(0x30, guidBytes[14]);
            Assert.Equal(0xc8, guidBytes[15]);
        }
        [Fact]
        public void TestUUIDNamespace_X500()
        {
            Byte[] guidBytes = UUIDNamespace.X500.ToByteArray();

            Assert.Equal(0x6b, guidBytes[0]);
            Assert.Equal(0xa7, guidBytes[1]);
            Assert.Equal(0xb8, guidBytes[2]);
            Assert.Equal(0x14, guidBytes[3]);
            Assert.Equal(0x9d, guidBytes[4]);
            Assert.Equal(0xad, guidBytes[5]);
            Assert.Equal(0x11, guidBytes[6]);
            Assert.Equal(0xd1, guidBytes[7]);
            Assert.Equal(0x80, guidBytes[8]);
            Assert.Equal(0xb4, guidBytes[9]);
            Assert.Equal(0x00, guidBytes[10]);
            Assert.Equal(0xc0, guidBytes[11]);
            Assert.Equal(0x4f, guidBytes[12]);
            Assert.Equal(0xd4, guidBytes[13]);
            Assert.Equal(0x30, guidBytes[14]);
            Assert.Equal(0xc8, guidBytes[15]);
        }
    }
}
