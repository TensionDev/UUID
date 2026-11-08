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
using System.Threading;

namespace TensionDev.UUID
{
    /// <summary>
    /// Class Library to generate Universally Unique Identifier (UUID) / Globally Unique Identifier (GUID) based on Version 7 (date-time).
    /// </summary>
    public static class UUIDv7
    {
#if NETCOREAPP2_1_OR_GREATER
        private static readonly DateTime s_epoch = DateTime.UnixEpoch;
#else
        private static readonly DateTime s_epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
#endif

        private static UInt16 s_counter = 0;
        private static Int64 s_method1TimestampMilliseconds = -1;
        private static UInt16 s_method1Counter;

#if NET9_0_OR_GREATER
        private static readonly System.Threading.Lock s_counterLock = new System.Threading.Lock();
#else
        private static readonly Object s_counterLock = new Object();
#endif

        /// <summary>
        /// Specifies the method of generating the clock sequence and Node ID for UUIDv7.
        /// </summary>
        public enum GenerationMethod
        {
            /// <summary>
            /// Cryptographically random bits for the remaining 74 bits.
            /// </summary>
            Random = 0,

            /// <summary>
            /// Fixed Bit-Length Dedicated Counter (Method 1) as specified in RFC 9562 Section 6.2.
            /// </summary>
            Method1 = 1,

            //Method2 = 2,

            /// <summary>
            /// Replace Leftmost Random Bits with Increased Clock Precision (Method 3) as specified in RFC 9562 Section 6.2.
            /// </summary>
            Method3 = 3,
        }

        /// <summary>
        /// Initialises a new GUID/UUID based on Version 7 (date-time)
        /// </summary>
        /// <param name="method">The method of generating the clock sequence and Node ID.</param>
        /// <returns>A new Uuid object</returns>
        public static Uuid NewUUIDv7(GenerationMethod method = GenerationMethod.Random)
        {
            return NewUUIDv7(DateTime.UtcNow, method);
        }

        /// <summary>
        /// Initialises a new GUID/UUID based on Version 7 (date-time), based on the given date and time.
        /// </summary>
        /// <param name="dateTime">Given Date and Time</param>
        /// <param name="method">The method of generating the clock sequence and Node ID.</param>
        /// <returns>A new Uuid object</returns>
        public static Uuid NewUUIDv7(DateTime dateTime, GenerationMethod method = GenerationMethod.Random)
        {
            switch (method)
            {
                default:
                case GenerationMethod.Random:
                    return NewUUIDv7(dateTime, GetRandomA(), GetRandomB());

                case GenerationMethod.Method1:
                    Int64 requestedTimestamp = GetUnixTimestampMilliseconds(dateTime);
                    Int64 effectiveTimestamp = GetMethod1Counter(requestedTimestamp, out Byte[] counterA);
                    return NewUUIDv7(effectiveTimestamp, counterA, GetRandomB());

                case GenerationMethod.Method3:
                    return NewUUIDv7(dateTime, GetIncreasedClockPrecisionA(dateTime), GetRandomB());
            }
        }

        /// <summary>
        /// Initialises a new GUID/UUID based on Version 7 (date-time), based on the given date and time, Clock Sequence with Variant and Node ID.
        /// </summary>
        /// <param name="dateTime">Given Date and Time</param>
        /// <param name="randomA">Given 16-bit rand_a</param>
        /// <param name="randomB">Given 64-bit rand_b</param>
        /// <returns>A new Uuid object</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static Uuid NewUUIDv7(DateTime dateTime, Byte[] randomA, Byte[] randomB)
        {
            if (randomA == null)
                throw new ArgumentNullException(nameof(randomA));

            if (randomA.Length < 2)
                throw new ArgumentException(String.Format("rand_a contains less than 16-bit: {0} bytes", randomA.Length), nameof(randomA));

            if (randomB == null)
                throw new ArgumentNullException(nameof(randomB));

            if (randomB.Length < 8)
                throw new ArgumentException(String.Format("rand_b contains less than 64-bit: {0} bytes", randomB.Length), nameof(randomB));

            Int64 timeInterval = GetUnixTimestampMilliseconds(dateTime);

            return NewUUIDv7(timeInterval, randomA, randomB);
        }

        private static Uuid NewUUIDv7(Int64 timeInterval, Byte[] randomA, Byte[] randomB)
        {

            Byte[] hex = new Byte[16];

            hex[0] = (Byte)(timeInterval >> 40);
            hex[1] = (Byte)(timeInterval >> 32);
            hex[2] = (Byte)(timeInterval >> 24);
            hex[3] = (Byte)(timeInterval >> 16);
            hex[4] = (Byte)(timeInterval >> 8);
            hex[5] = (Byte)timeInterval;

            hex[6] = (Byte)((randomA[0] & 0x0F) + 0x70);
            hex[7] = randomA[1];

            hex[8] = (Byte)((randomB[0] & 0x3F) | 0x80);
            hex[9] = randomB[1];
            hex[10] = randomB[2];
            hex[11] = randomB[3];
            hex[12] = randomB[4];
            hex[13] = randomB[5];
            hex[14] = randomB[6];
            hex[15] = randomB[7];

            Uuid Id = new Uuid(hex);

            return Id;
        }

        /// <summary>
        /// Initialises the 12-bit rand_a and returns it.<br />
        /// Returns a randomly genrated 16-bit rand_a.
        /// </summary>
        /// <returns>A byte-array representing the 16-bit rand_a</returns>
        public static Byte[] GetRandomA()
        {
#if NET6_0_OR_GREATER
            using var randomNumberGenerator = System.Security.Cryptography.RandomNumberGenerator.Create();
            Byte[] fakeNode = new Byte[2];
            randomNumberGenerator.GetBytes(fakeNode);
            return fakeNode;
#else
            using (System.Security.Cryptography.RNGCryptoServiceProvider cryptoServiceProvider = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                Byte[] fakeNode = new Byte[2];
                cryptoServiceProvider.GetBytes(fakeNode);
                return fakeNode;
            }
#endif
        }

        /// <summary>
        /// Returns the next value from a standalone 12-bit counter helper.<br />
        /// UUIDv7 Method 1 generation uses a separate timestamp-aware counter with rollover handling.
        /// </summary>
        /// <returns>A byte-array representing the 16-bit rand_a</returns>
        public static Byte[] GetFixedBitLengthDedicatedCounterA()
        {
            // Retained as a low-level wrapping 12-bit counter helper for compatibility.
            // UUIDv7 Method1 uses a timestamp-aware counter with rollover handling below.
            lock (s_counterLock)
            {
                Int16 value = Convert.ToInt16(s_counter);
                Byte[] counter = BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(value));

                ++s_counter;
                if (s_counter >= 0x1000)
                    s_counter = 0;

                return counter;
            }
        }

        /// <summary>
        /// Initialises the 12-bit rand_a based on Method 3 in Section 6.2 and returns it.<br />
        /// Returns a Increased Clock Precision 16-bit rand_a.
        /// </summary>
        /// <param name="currentDateTime">The current date time to calculate precision interval from.</param>
        /// <returns>A byte-array representing the 16-bit rand_a calculated from the sub-millisecond precision of the input datetime.</returns>
        public static Byte[] GetIncreasedClockPrecisionA(DateTime currentDateTime)
        {
            TimeSpan timeSince = currentDateTime.ToUniversalTime() - s_epoch.ToUniversalTime();
            Int64 timeInterval = ((Int64)timeSince.TotalMilliseconds);
            Double precisionInterval = timeSince.TotalMilliseconds - timeInterval;
            Int16 precisionA = (Int16)Math.Floor(precisionInterval * 0x1000);

            Byte[] bytes = BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(precisionA));

            return bytes;

        }

        /// <summary>
        /// Initialises the 62-bit rand_b and returns it.<br />
        /// Returns a randomly genrated 64-bit rand_b.
        /// </summary>
        /// <returns>A byte-array representing the 64-bit rand_b</returns>
        public static Byte[] GetRandomB()
        {
#if NET6_0_OR_GREATER
            using var randomNumberGenerator = System.Security.Cryptography.RandomNumberGenerator.Create();
            Byte[] fakeNode = new Byte[8];
            randomNumberGenerator.GetBytes(fakeNode);
            return fakeNode;
#else
            using (System.Security.Cryptography.RNGCryptoServiceProvider cryptoServiceProvider = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                Byte[] fakeNode = new Byte[8];
                cryptoServiceProvider.GetBytes(fakeNode);
                return fakeNode;
            }
#endif
        }

        /// <summary>
        /// Returns true if the Uuid specified is Version 7.
        /// </summary>
        /// <param name="uuid">The Uuid to be tested.</param>
        /// <returns>Returns true if the Uuid specified is Version 7.</returns>
        public static bool IsUUIDv7(Uuid uuid)
        {
            return (uuid.ToByteArray()[6] >> 4) == 0x07;
        }

        /// <summary>
        /// Returns the approximate DateTime used to generate the Uuid.
        /// </summary>
        /// <param name="uuid">The Uuid Version 7 object</param>
        /// <returns>DateTime of the Uuid in UTC.</returns>
        /// <exception cref="ArgumentException">Thrown when uuid is not a Version 7 UUID.</exception>
        public static DateTime ToDateTime(Uuid uuid)
        {
            if (!IsUUIDv7(uuid))
                throw new ArgumentException(String.Format("{0} is not a Version 7 UUID.", uuid), nameof(uuid));

            long timeInterval = GetTimeInterval(uuid);
            return s_epoch.AddTicks(timeInterval * TimeSpan.TicksPerMillisecond);
        }

        private static Int64 GetTimeInterval(Uuid uuid)
        {
            Byte[] hex = uuid.ToByteArray();

            return ((Int64)hex[0] << 40)
                | ((Int64)hex[1] << 32)
                | ((Int64)hex[2] << 24)
                | ((Int64)hex[3] << 16)
                | ((Int64)hex[4] << 8)
                | hex[5];
        }

        private static Int64 GetUnixTimestampMilliseconds(DateTime dateTime)
        {
            TimeSpan timeSince = dateTime.ToUniversalTime() - s_epoch;
            if (timeSince.Ticks < 0)
                throw new ArgumentOutOfRangeException(nameof(dateTime), "The date-time is outside the UUIDv7 48-bit Unix timestamp range.");

            Int64 timeInterval = timeSince.Ticks / TimeSpan.TicksPerMillisecond;

            if (timeInterval < 0 || timeInterval > 0xFFFFFFFFFFFFL)
                throw new ArgumentOutOfRangeException(nameof(dateTime), "The date-time is outside the UUIDv7 48-bit Unix timestamp range.");

            return timeInterval;
        }

        private static Int64 GetMethod1Counter(Int64 requestedTimestamp, out Byte[] counterA)
        {
            lock (s_counterLock)
            {
                if (s_method1TimestampMilliseconds < 0 || requestedTimestamp > s_method1TimestampMilliseconds)
                {
                    s_method1TimestampMilliseconds = requestedTimestamp;
                    s_method1Counter = GetRandomCounterA();
                }
                else if (s_method1Counter < 0x0FFF)
                {
                    ++s_method1Counter;
                }
                else
                {
                    // Advance the logical timestamp rather than wrapping the counter and sorting backward.
                    ++s_method1TimestampMilliseconds;
                    s_method1Counter = GetRandomCounterA();
                }

                counterA = new Byte[]
                {
                    (Byte)(s_method1Counter >> 8),
                    (Byte)s_method1Counter
                };

                return s_method1TimestampMilliseconds;
            }
        }

        private static UInt16 GetRandomCounterA()
        {
            Byte[] randomA = GetRandomA();
            return (UInt16)(((randomA[0] & 0x0F) << 8) | randomA[1]);
        }
    }
}
