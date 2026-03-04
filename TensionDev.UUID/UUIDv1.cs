// SPDX-License-Identifier: Apache-2.0
//
//   Copyright 2021 - 2026 TensionDev <TensionDev@outlook.com>
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Linq;

namespace TensionDev.UUID
{
    /// <summary>
    /// Class Library to generate Universally Unique Identifier (UUID) / Globally Unique Identifier (GUID) based on Version 1 (date-time and MAC address).
    /// </summary>
    public static class UUIDv1
    {
        private static System.Net.NetworkInformation.PhysicalAddress s_physicalAddress = System.Net.NetworkInformation.PhysicalAddress.None;
        private static Int32 s_clock = Int32.MinValue;
        private static readonly DateTime s_epoch = new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);

        private static readonly Object s_initLock = new Object();
        private static readonly Object s_clockLock = new Object();

        /// <summary>
        /// Initialises a new GUID/UUID based on Version 1 (date-time and MAC address)
        /// </summary>
        /// <returns>A new Uuid object</returns>
        public static Uuid NewUUIDv1()
        {
            return NewUUIDv1(DateTime.UtcNow);
        }

        /// <summary>
        /// Initialises a new GUID/UUID based on Version 1 (date-time and MAC address), based on the given date and time.
        /// </summary>
        /// <param name="dateTime">Given Date and Time</param>
        /// <returns>A new Uuid object</returns>
        public static Uuid NewUUIDv1(DateTime dateTime)
        {
            return NewUUIDv1(dateTime, GetClockSequence(), GetNodeID());
        }

        /// <summary>
        /// Initialises a new GUID/UUID based on Version 1 (date-time and MAC address), based on the given Node ID.
        /// </summary>
        /// <param name="nodeID">Given 48-bit Node ID</param>
        /// <returns>A new Uuid object</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static Uuid NewUUIDv1(Byte[] nodeID)
        {
            return NewUUIDv1(DateTime.UtcNow, GetClockSequence(), nodeID);
        }

        /// <summary>
        /// Initialises a new GUID/UUID based on Version 1 (date-time and MAC address), based on the given date and time, Clock Sequence with Variant and Node ID.
        /// </summary>
        /// <param name="dateTime">Given Date and Time</param>
        /// <param name="clockSequence">Given 16-bit Clock Sequence with Variant</param>
        /// <param name="nodeID">Given 48-bit Node ID</param>
        /// <returns>A new Uuid object</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static Uuid NewUUIDv1(DateTime dateTime, Byte[] clockSequence, Byte[] nodeID)
        {
            if (clockSequence == null)
                throw new ArgumentNullException(nameof(clockSequence));

            if (clockSequence.Length < 2)
                throw new ArgumentException(String.Format("Clock Sequence contains less than 16-bit: {0} bytes", clockSequence.Length), nameof(clockSequence));

            if (nodeID == null)
                throw new ArgumentNullException(nameof(nodeID));

            if (nodeID.Length < 6)
                throw new ArgumentException(String.Format("Node ID contains less than 48-bit: {0} bytes", nodeID.Length), nameof(nodeID));

            TimeSpan timeSince = dateTime.ToUniversalTime() - s_epoch.ToUniversalTime();
            Int64 timeInterval = timeSince.Ticks;

            Byte[] time = BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(timeInterval));

            Byte[] hex = new Byte[16];

            hex[0] = time[4];
            hex[1] = time[5];
            hex[2] = time[6];
            hex[3] = time[7];

            hex[4] = time[2];
            hex[5] = time[3];

            hex[6] = (Byte)((time[0] & 0x0F) + 0x10);
            hex[7] = time[1];

            hex[8] = clockSequence[0];
            hex[9] = clockSequence[1];

            hex[10] = nodeID[0];
            hex[11] = nodeID[1];
            hex[12] = nodeID[2];
            hex[13] = nodeID[3];
            hex[14] = nodeID[4];
            hex[15] = nodeID[5];

            Uuid Id = new Uuid(hex);

            return Id;
        }

        /// <summary>
        /// Initialises the 48-bit Node ID and returns it.<br />
        /// Returns the MAC Address of a Network Interface Card, if available.
        /// Otherwise, returns a randomly genrated 48-bit Node ID.
        /// </summary>
        /// <returns>A byte-array representing the 48-bit Node ID</returns>
        public static Byte[] GetNodeID()
        {
            if (System.Net.NetworkInformation.PhysicalAddress.None.Equals(s_physicalAddress))
            {
                var networkInterfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                foreach (var networkInterface in networkInterfaces)
                {
                    var address = networkInterface.GetPhysicalAddress();
                    if (!address.Equals(System.Net.NetworkInformation.PhysicalAddress.None))
                    {
                        s_physicalAddress = address;
                        break;
                    }
                }
                if (s_physicalAddress.Equals(System.Net.NetworkInformation.PhysicalAddress.None))
                {
                    // Fallback to random
                    using (var cryptoServiceProvider = new System.Security.Cryptography.RNGCryptoServiceProvider())
                    {
                        Byte[] fakeNode = new Byte[6];
                        cryptoServiceProvider.GetBytes(fakeNode);
                        fakeNode[0] = (Byte)(fakeNode[0] | 0x01);
                        s_physicalAddress = new System.Net.NetworkInformation.PhysicalAddress(fakeNode);
                    }
                }
            }

            return s_physicalAddress.GetAddressBytes();
        }

        /// <summary>
        /// Intialises the 14-bit Clock Sequence and returns the current value with the Variant.<br />
        /// Will return an incremented Clock Sequence on each call, modulo 14-bit.
        /// </summary>
        /// <returns>A byte-array representing the 14-bit Clock Sequence, together with the Variant</returns>
        public static Byte[] GetClockSequence()
        {
            lock (s_initLock)
            {
                if (s_clock < 0)
                {
                    using (System.Security.Cryptography.RNGCryptoServiceProvider cryptoServiceProvider = new System.Security.Cryptography.RNGCryptoServiceProvider())
                    {
                        Byte[] clockInit = new Byte[4];
                        cryptoServiceProvider.GetBytes(clockInit);
                        s_clock = BitConverter.ToInt32(clockInit, 0) & 0x3FFF;
                        s_clock |= 0x8000;
                    }
                }
            }

            Int32 result;
            lock (s_clockLock)
            {
                result = s_clock++;
                if (s_clock >= 0xC000)
                    s_clock = 0x8000;
            }

            return BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((Int16)result));
        }

        /// <summary>
        /// Returns true if the Uuid specified is Version 1.
        /// </summary>
        /// <param name="uuid">The Uuid to be tested.</param>
        /// <returns>Returns true if the Uuid specified is Version 1.</returns>
        public static bool IsUUIDv1(Uuid uuid)
        {
            return (uuid.ToByteArray()[6] >> 4) == 0x01;
        }

        /// <summary>
        /// Returns the approximate DateTime used to generate the Uuid.
        /// </summary>
        /// <param name="uuid">The Uuid Version 1 object.</param>
        /// <returns>DateTime of the Uuid in UTC.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static DateTime ToDateTime(Uuid uuid)
        {
            if (!IsUUIDv1(uuid))
                throw new ArgumentException(String.Format("{0} is not a Version 1 UUID.", uuid), nameof(uuid));

            long timeInterval = GetTimeInterval(uuid);
            TimeSpan timeSpan = TimeSpan.FromTicks(timeInterval);

            return s_epoch.ToUniversalTime() + timeSpan;
        }

        /// <summary>
        /// Returns the Version 6 representation of the provided Version 1 Uuid.
        /// </summary>
        /// <param name="uuid">The Uuid Version 1 object.</param>
        /// <returns>The converted Uuid Version 6 object.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static Uuid ToUUIDv6(Uuid uuid)
        {
            if (!IsUUIDv1(uuid))
                throw new ArgumentException(String.Format("{0} is not a Version 1 UUID.", uuid), nameof(uuid));

            Int64 timeInterval = GetTimeInterval(uuid);

            Byte[] hex = uuid.ToByteArray();
            timeInterval <<= 4;
            Byte[] time = BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(timeInterval));

            hex[0] = time[0];
            hex[1] = time[1];
            hex[2] = time[2];
            hex[3] = time[3];

            hex[4] = time[4];
            hex[5] = time[5];

            hex[6] = (Byte)(((time[6] >> 4) & 0x0F) + 0x60);
            hex[7] = (Byte)((time[6] << 4) + (time[7] >> 4));

            return new Uuid(hex);
        }

        private static Int64 GetTimeInterval(Uuid uuid)
        {
            Byte[] hex = uuid.ToByteArray();
            Byte[] time = new Byte[8];

            time[0] = (Byte)(hex[6] & 0x0F);
            time[1] = hex[7];
            time[2] = hex[4];
            time[3] = hex[5];
            time[4] = hex[0];
            time[5] = hex[1];
            time[6] = hex[2];
            time[7] = hex[3];

            Int64 timeInterval = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt64(time, 0));
            return timeInterval;
        }
    }
}
