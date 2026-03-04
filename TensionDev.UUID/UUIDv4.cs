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

namespace TensionDev.UUID
{
    /// <summary>
    /// Class Library to generate Universally Unique Identifier (UUID) / Globally Unique Identifier (GUID) based on Version 4 (random).
    /// </summary>
    public static class UUIDv4
    {
        /// <summary>
        /// Initialises a new GUID/UUID based on Version 4 (random)
        /// </summary>
        /// <returns>A new Uuid object</returns>
        public static Uuid NewUUIDv4()
        {
            Byte[] time = new Byte[8];
            Byte[] clockSequence = new Byte[2];
            Byte[] nodeID = new Byte[6];

            using (System.Security.Cryptography.RNGCryptoServiceProvider cryptoServiceProvider = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                cryptoServiceProvider.GetBytes(time);
                cryptoServiceProvider.GetBytes(clockSequence);
                cryptoServiceProvider.GetBytes(nodeID);
            }

            Byte[] hex = new Byte[16];

            hex[0] = time[4];
            hex[1] = time[5];
            hex[2] = time[6];
            hex[3] = time[7];

            hex[4] = time[2];
            hex[5] = time[3];

            hex[6] = (Byte)((time[0] & 0x0F) + 0x40);
            hex[7] = time[1];

            hex[8] = (Byte)((clockSequence[0] & 0x3F) + 0x80);
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
        /// Returns true if the Uuid specified is Version 4.
        /// </summary>
        /// <param name="uuid">The Uuid to be tested.</param>
        /// <returns>Returns true if the Uuid specified is Version 4.</returns>
        public static bool IsUUIDv4(Uuid uuid)
        {
            return (uuid.ToByteArray()[6] >> 4) == 0x04;
        }
    }
}
