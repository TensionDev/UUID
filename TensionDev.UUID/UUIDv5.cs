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
using System.Security.Cryptography;
using System.Text;

namespace TensionDev.UUID
{
    /// <summary>
    /// Class Library to generate Universally Unique Identifier (UUID) / Globally Unique Identifier (GUID) based on Version 5 (SHA-1 namespace name-based).
    /// </summary>
    public static class UUIDv5
    {
        /// <summary>
        /// Initialises a new GUID/UUID based on Version 5 (SHA-1 namespace name-based)
        /// </summary>
        /// <returns>A new Uuid object</returns>
        public static Uuid NewUUIDv5(Uuid nameSpace, String name)
        {
            Byte[] nsArray = nameSpace.ToByteArray();
            Byte[] nArray = Encoding.UTF8.GetBytes(name);

            Byte[] buffer = new Byte[nsArray.Length + nArray.Length];
            Buffer.BlockCopy(nsArray, 0, buffer, 0, nsArray.Length);
            Buffer.BlockCopy(nArray, 0, buffer, nsArray.Length, nArray.Length);

            Byte[] hash;
            using (SHA1 sha1 = SHA1.Create())
            {
                hash = sha1.ComputeHash(buffer);
            }

            Byte[] hex = new Byte[16];

            hex[0] = hash[0];
            hex[1] = hash[1];
            hex[2] = hash[2];
            hex[3] = hash[3];

            hex[4] = hash[4];
            hex[5] = hash[5];

            hex[6] = (Byte)((hash[6] & 0x0F) + 0x50);
            hex[7] = hash[7];

            hex[8] = (Byte)((hash[8] & 0x3F) + 0x80);
            hex[9] = hash[9];

            hex[10] = hash[10];
            hex[11] = hash[11];
            hex[12] = hash[12];
            hex[13] = hash[13];
            hex[14] = hash[14];
            hex[15] = hash[15];

            Uuid Id = new Uuid(hex);

            return Id;
        }

        /// <summary>
        /// Returns true if the Uuid specified is Version 5.
        /// </summary>
        /// <param name="uuid">The Uuid to be tested.</param>
        /// <returns>Returns true if the Uuid specified is Version 5.</returns>
        public static bool IsUUIDv5(Uuid uuid)
        {
            return (uuid.ToByteArray()[6] >> 4) == 0x05;
        }
    }
}
