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
using System.Text;

namespace TensionDev.UUID
{
    /// <summary>
    /// Class Library to generate Universally Unique Identifier (UUID) / Globally Unique Identifier (GUID) based on Version 8.
    /// </summary>
    public static class UUIDv8
    {
        /// <summary>
        /// Returns true if the Uuid specified is Version 8.
        /// </summary>
        /// <param name="uuid">The Uuid to be tested.</param>
        /// <returns>Returns true if the Uuid specified is Version 8.</returns>
        public static bool IsUUIDv8(Uuid uuid)
        {
            return (uuid.ToByteArray()[6] >> 4) == 0x08;
        }
    }
}
