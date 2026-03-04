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
using System.Text.RegularExpressions;

namespace TensionDev.UUID
{
    /// <summary>
    /// Represents a Universally unique identifier (UUID).
    /// </summary>
    public sealed class Uuid : IComparable<Uuid>, IEquatable<Uuid>
    {
        private const string INVALID_FORMAT_STRING = "The format of s is invalid";
        private const string ONE_BYTE_HEX_FORMAT_STRING = "{0:x2}";
        private const string TWO_BYTE_HEX_FORMAT_STRING = "{0:x4}";
        private const string FOUR_BYTE_HEX_FORMAT_STRING = "{0:x8}";
        private readonly uint _time_low;
        private readonly ushort _time_mid;
        private readonly ushort _time_hi_and_version;
        private readonly byte _clock_seq_hi_and_reserved;
        private readonly byte _clock_seq_low;
        private readonly byte[] _node;

        /// <summary>
        /// A read-only instance of the Uuid object whose value is all zeros.
        /// </summary>
        public static readonly Uuid Empty = new Uuid();

        /// <summary>
        /// A read-only instance of the Uuid object whose value is all ones.
        /// </summary>
        public static readonly Uuid Max = new Uuid(uint.MaxValue, ushort.MaxValue, ushort.MaxValue, byte.MaxValue, byte.MaxValue, new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff });

        /// <summary>
        /// Initializes a new instance of the Uuid object whose value is all zeros.
        /// </summary>
        public Uuid()
        {
            _time_low = 0;
            _time_mid = 0;
            _time_hi_and_version = 0;
            _clock_seq_hi_and_reserved = 0;
            _clock_seq_low = 0;
            _node = new byte[6];
        }

        /// <summary>
        /// Initializes a new instance of the Uuid object by using the specified array of bytes.
        /// </summary>
        /// <param name="b">A 16-element byte array containing values with which to initialize the Uuid.</param>
        /// <exception cref="System.ArgumentNullException">b is null.</exception>
        /// <exception cref="System.ArgumentException">b is not 16 bytes long.</exception>
        public Uuid(byte[] b) : this()
        {
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            if (b.Length != 16)
                throw new ArgumentException("b is not 16 bytes long.", nameof(b));

            _time_low = (uint)(b[0] << 24 | b[1] << 16 | b[2] << 8 | b[3]);
            _time_mid = (ushort)(b[4] << 8 | b[5]);
            _time_hi_and_version = (ushort)(b[6] << 8 | b[7]);
            _clock_seq_hi_and_reserved = b[8];
            _clock_seq_low = b[9];
            Array.Copy(b, 10, _node, 0, _node.Length);
        }

        /// <summary>
        /// Initializes a new instance of the Uuid object by using the value represented by the specified string.
        /// </summary>
        /// <param name="s">A string that contains a Uuid in one of the following formats ("d" represents a hexadecimal digit whose case is ignored)
        /// <para>32 contiguous hexadecimal digits:<br />dddddddddddddddddddddddddddddddd</para>
        /// <para>Groups of 8, 4, 4, 4, and 12 digits with hyphens between the groups.<br />The entire Uuid can optionally be enclosed in matching braces or parentheses:</para>
        /// <para>dddddddd-dddd-dddd-dddd-dddddddddddd<br />{dddddddd-dddd-dddd-dddd-dddddddddddd}<br />(dddddddd-dddd-dddd-dddd-dddddddddddd)</para>
        /// </param>
        /// <exception cref="System.ArgumentNullException">s is null</exception>
        /// <exception cref="System.FormatException">The format of s is invalid</exception>
        /// <exception cref="System.OverflowException">The format of s is invalid</exception>
        public Uuid(string s) : this()
        {
            if (String.IsNullOrEmpty(s))
                throw new ArgumentNullException(nameof(s), INVALID_FORMAT_STRING);

            if (s.Length != 32 && s.Length != 36 && s.Length != 38)
                throw new FormatException(INVALID_FORMAT_STRING);

            if (s.Length == 38 &&
                !(s.StartsWith("{") && s.EndsWith("}")) &&
                !(s.StartsWith("(") && s.EndsWith(")")))
            {
                throw new FormatException(INVALID_FORMAT_STRING);
            }

            string vs = s.Replace("{", "");
            vs = vs.Replace("}", "");
            vs = vs.Replace("(", "");
            vs = vs.Replace(")", "");

            if (vs.Length == 36)
            {
                Regex regex = new Regex(@"\b[0-9a-f]{8}\b-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-\b[0-9a-f]{12}\b", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(100));
                MatchCollection matches = regex.Matches(vs);
                if (matches.Count == 0)
                    throw new FormatException(INVALID_FORMAT_STRING);
            }
            vs = vs.Replace("-", "");

            if (vs.Length != 32)
                throw new FormatException(INVALID_FORMAT_STRING);

            Byte[] b = new Byte[16];
            for (Int32 i = 0; i < vs.Length; i += 2)
            {
                try
                {
                    b[i / 2] = Byte.Parse(vs.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                }
                catch (FormatException)
                {
                    throw new FormatException(INVALID_FORMAT_STRING);
                }
                catch (OverflowException)
                {
                    throw new OverflowException(INVALID_FORMAT_STRING);
                }
            }

            _time_low = (uint)(b[0] << 24 | b[1] << 16 | b[2] << 8 | b[3]);
            _time_mid = (ushort)(b[4] << 8 | b[5]);
            _time_hi_and_version = (ushort)(b[6] << 8 | b[7]);
            _clock_seq_hi_and_reserved = b[8];
            _clock_seq_low = b[9];
            Array.Copy(b, 10, _node, 0, _node.Length);
        }

        /// <summary>
        /// Initializes a new instance of the Uuid object by using the specified integers and byte array.
        /// </summary>
        /// <param name="a">The first 4 bytes of the Uuid.</param>
        /// <param name="b">The next 2 bytes of the Uuid.</param>
        /// <param name="c">The next 2 bytes of the Uuid.</param>
        /// <param name="d">The next byte of the Uuid.</param>
        /// <param name="e">The next byte of the Uuid.</param>
        /// <param name="f">The next 6 bytes of the Uuid.</param>
        /// <exception cref="System.ArgumentNullException">f is null</exception>
        /// <exception cref="System.ArgumentException">f is not 6 bytes long</exception>
        public Uuid(uint a, ushort b, ushort c, byte d, byte e, byte[] f) : this()
        {
            if (f == null)
                throw new ArgumentNullException(nameof(f));

            if (f.Length != 6)
                throw new ArgumentException("f is not 6 bytes long");

            _time_low = a;
            _time_mid = b;
            _time_hi_and_version = c;
            _clock_seq_hi_and_reserved = d;
            _clock_seq_low = e;
            f.CopyTo(_node, 0);
        }

        /// <summary>
        /// Initializes a new instance of the Uuid object by using the specified integers and bytes.
        /// </summary>
        /// <param name="a">The first 4 bytes of the Uuid.</param>
        /// <param name="b">The next 2 bytes of the Uuid.</param>
        /// <param name="c">The next 2 bytes of the Uuid.</param>
        /// <param name="d">The next byte of the Uuid.</param>
        /// <param name="e">The next byte of the Uuid.</param>
        /// <param name="f">The next byte of the Uuid.</param>
        /// <param name="g">The next byte of the Uuid.</param>
        /// <param name="h">The next byte of the Uuid.</param>
        /// <param name="i">The next byte of the Uuid.</param>
        /// <param name="j">The next byte of the Uuid.</param>
        /// <param name="k">The next byte of the Uuid.</param>
        public Uuid(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k) : this()
        {
            _time_low = a;
            _time_mid = b;
            _time_hi_and_version = c;
            _clock_seq_hi_and_reserved = d;
            _clock_seq_low = e;
            _node[0] = f;
            _node[1] = g;
            _node[2] = h;
            _node[3] = i;
            _node[4] = j;
            _node[5] = k;
        }

        /// <summary>
        /// Initializes a new instance of the Uuid object.
        /// </summary>
        /// <param name="input">A universally unique identifier in the proper format.</param>
        /// <returns>A new Uuid object.</returns>
        public static Uuid Parse(string input)
        {
            return new Uuid(input);
        }

        /// <summary>
        /// Converts the string representation of a Uuid to the equivalent Uuid object.
        /// </summary>
        /// <param name="input">The Uuid to convert.</param>
        /// <param name="result">The object that will contain the parsed value. If the method returns true, result contains a valid Uuid.
        /// If the method returns false, result equals Uuid.Empty.</param>
        /// <returns>true if the parse operation was successful; otherwise, false.</returns>
        public static bool TryParse(string input, out Uuid result)
        {
            bool vs = false;
            result = new Uuid();

            try
            {
                result = Parse(input);
                vs = true;
            }
            catch (Exception)
            {
                // Quietly suppress exception on Parse.
            }

            return vs;
        }

        /// <summary>
        /// Compares the current instance with another object and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(object other)
        {
            if (other is Uuid u)
            {
                return CompareTo(u);
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An UUID to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(Uuid other)
        {
            if (other is null)
                return 1;

            if (_time_low.CompareTo(other._time_low) != 0)
                return _time_low.CompareTo(other._time_low);

            if (_time_mid.CompareTo(other._time_mid) != 0)
                return _time_mid.CompareTo(other._time_mid);

            if (_time_hi_and_version.CompareTo(other._time_hi_and_version) != 0)
                return _time_hi_and_version.CompareTo(other._time_hi_and_version);

            if (_clock_seq_hi_and_reserved.CompareTo(other._clock_seq_hi_and_reserved) != 0)
                return _clock_seq_hi_and_reserved.CompareTo(other._clock_seq_hi_and_reserved);

            if (_clock_seq_low.CompareTo(other._clock_seq_low) != 0)
                return _clock_seq_low.CompareTo(other._clock_seq_low);

            for (Int32 i = 0; i < _node.Length; i++)
            {
                if (_node[i].CompareTo(other._node[i]) != 0)
                    return _node[i].CompareTo(other._node[i]);
            }

            return 0;
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified Uuid object represent the same value.
        /// </summary>
        /// <param name="other">An object to compare to this instance.</param>
        /// <returns>true if other is equal to this instance; otherwise, false.</returns>
        public bool Equals(Uuid other)
        {
            if (other is null)
                return false;

            int result = CompareTo(other);
            return result == 0;

        }

        /// <summary>
        /// Returns a value that indicates whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns>true if o is a Uuid that has the same value as this instance; otherwise,false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Uuid other)
            {
                return Equals(other);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return (_time_low, _time_mid, _time_hi_and_version, _clock_seq_hi_and_reserved, _clock_seq_low, _node).GetHashCode();
        }

        /// <summary>
        /// Returns a 16-element byte array that contains the value of this instance.
        /// </summary>
        /// <returns>A 16-element byte array.</returns>
        public byte[] ToByteArray()
        {
            Byte[] vs = new Byte[16];

            vs[0] = (Byte)(_time_low >> 24);
            vs[1] = (Byte)(_time_low >> 16);
            vs[2] = (Byte)(_time_low >> 8);
            vs[3] = (Byte)(_time_low);
            vs[4] = (Byte)(_time_mid >> 8);
            vs[5] = (Byte)(_time_mid);
            vs[6] = (Byte)(_time_hi_and_version >> 8);
            vs[7] = (Byte)(_time_hi_and_version);
            vs[8] = _clock_seq_hi_and_reserved;
            vs[9] = _clock_seq_low;
            Array.Copy(_node, 0, vs, 10, 6);

            return vs;
        }

        /// <summary>
        /// Returns the System.Guid equivalent of this instance.
        /// </summary>
        /// <returns>A System.Guid object.</returns>
        public Guid ToGuid()
        {
            return new Guid(ToString());
        }

        /// <summary>
        /// Return the Microsoft Variant version in System.Guid.
        /// </summary>
        /// <returns>A System.Guid object.</returns>
        public Guid ToMicrosoftVariant()
        {
            byte newClockSeq = (byte)(_clock_seq_hi_and_reserved & 0x1F);
            newClockSeq = (byte)(newClockSeq | 0xC0);
            byte[] array = ToByteArray();
            array[8] = newClockSeq;
            Uuid variant2 = new Uuid(array);

            return variant2.ToGuid();
        }

        /// <summary>
        /// Returns the RFC 4122 Variant version in TensionDev.UUID.Uuid.
        /// </summary>
        /// <param name="guid">The System.Guid object to convert.</param>
        /// <returns>A TensionDev.UUID.Uuid object.</returns>
        public static Uuid ToRfc4122Variant(Guid guid)
        {
            Uuid variant2 = new Uuid(guid.ToString());
            byte newClockSeq = (byte)(variant2._clock_seq_hi_and_reserved & 0x3F);
            newClockSeq = (byte)(newClockSeq | 0x80);
            byte[] array = variant2.ToByteArray();
            array[8] = newClockSeq;
            Uuid variant1 = new Uuid(array);

            return variant1;
        }

        /// <summary>
        /// Returns a string representation of the value of this instance as per RFC 4122 Section 3.
        /// </summary>
        /// <returns>The value of this Uuid, formatted by using the "D" format specifier as follows:
        /// xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx where the value of the Uuid is represented as a
        /// series of lowercase hexadecimal digits in groups of 8, 4, 4, 4, and 12 digits and separated by hyphens.
        /// An example of a return value is "7d444840-9dc0-11d1-b245-5ffdce74fad2".</returns>
        public override string ToString()
        {
            return ToString("D");
        }

        /// <summary>
        /// Returns a string representation of the value of this Uuid instance, according to the provided format specifier.
        /// </summary>
        /// <param name="format">A single format specifier that indicates how to format the value of this Uuid. The format parameter can be "N", "D", "B" or "P". If format is null or an empty string (""), "D" is used.</param>
        /// <returns>The value of this Uuid, represented as a series of lowercase hexadecimal digits in the specified format.</returns>
        /// <exception cref="System.FormatException">The value of format is not null, an empty string (""), "N", "D", "B" or "P"</exception>
        public string ToString(string format)
        {
            if (String.IsNullOrEmpty(format))
            {
                return ToStringCannonical();
            }

            switch (format)
            {
                case "N":
                    return ToHexString();

                case "D":
                    return ToStringCannonical();

                case "B":
                    return ToStringBraces();

                case "P":
                    return ToStringParenthesis();

                default:
                    throw new FormatException("The value of format is not null, an empty string(\"\"), \"N\", \"D\", \"B\" or \"P\"");
            }
        }

        private string ToStringCannonical()
        {
            StringBuilder sb = new StringBuilder();

            // RFC 4122 big-endian layout
            sb.AppendFormat(FOUR_BYTE_HEX_FORMAT_STRING, _time_low);               // 8 hex digits
            sb.Append("-");
            sb.AppendFormat(TWO_BYTE_HEX_FORMAT_STRING, _time_mid);               // 4 hex digits
            sb.Append("-");
            sb.AppendFormat(TWO_BYTE_HEX_FORMAT_STRING, _time_hi_and_version);    // 4 hex digits
            sb.Append("-");
            sb.AppendFormat(ONE_BYTE_HEX_FORMAT_STRING, _clock_seq_hi_and_reserved);
            sb.AppendFormat(ONE_BYTE_HEX_FORMAT_STRING, _clock_seq_low);
            sb.Append("-");

            foreach (byte b in _node)
                sb.AppendFormat(ONE_BYTE_HEX_FORMAT_STRING, b);

            return sb.ToString().ToLower();
        }

        private string ToHexString()
        {
            StringBuilder sb = new StringBuilder();

            // RFC 4122 big-endian layout
            sb.AppendFormat(FOUR_BYTE_HEX_FORMAT_STRING, _time_low);               // 8 hex digits
            sb.AppendFormat(TWO_BYTE_HEX_FORMAT_STRING, _time_mid);               // 4 hex digits
            sb.AppendFormat(TWO_BYTE_HEX_FORMAT_STRING, _time_hi_and_version);    // 4 hex digits
            sb.AppendFormat(ONE_BYTE_HEX_FORMAT_STRING, _clock_seq_hi_and_reserved);
            sb.AppendFormat(ONE_BYTE_HEX_FORMAT_STRING, _clock_seq_low);

            foreach (byte b in _node)
                sb.AppendFormat(ONE_BYTE_HEX_FORMAT_STRING, b);

            return sb.ToString().ToLower();
        }

        private string ToStringParenthesis()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("(");
            sb.Append(ToStringCannonical());
            sb.Append(")");

            return sb.ToString();
        }

        private string ToStringBraces()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            sb.Append(ToStringCannonical());
            sb.Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// Indicates whether the values of two specified Uuid objects are equal.
        /// </summary>
        /// <param name="a">The first object to compare.</param>
        /// <param name="b">The second object to compare.</param>
        /// <returns>true if a and b are equal; otherwise, false.</returns>
        public static bool operator ==(Uuid a, Uuid b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        /// <summary>
        /// Indicates whether the values of two specified Uuid objects are not equal.
        /// </summary>
        /// <param name="a">The first object to compare.</param>
        /// <param name="b">The second object to compare.</param>
        /// <returns>true if a and b are not equal; otherwise, false.</returns>
        public static bool operator !=(Uuid a, Uuid b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Indicates whether the values of the first Uuid object is strictly less than the other.
        /// </summary>
        /// <param name="a">The first object to compare.</param>
        /// <param name="b">The second object to compare.</param>
        /// <returns>true if a is strictly less than b; otherwise, false.</returns>
        public static bool operator <(Uuid a, Uuid b)
        {
            return a.CompareTo(b) < 0;
        }

        /// <summary>
        /// Indicates whether the values of the first Uuid object is strictly greater than the other.
        /// </summary>
        /// <param name="a">The first object to compare.</param>
        /// <param name="b">The second object to compare.</param>
        /// <returns>true if a is strictly greater than b; otherwise, false.</returns>
        public static bool operator >(Uuid a, Uuid b)
        {
            return a.CompareTo(b) > 0;
        }

        /// <summary>
        /// Indicates whether the values of the first Uuid object is less than or equal to the other.
        /// </summary>
        /// <param name="a">The first object to compare.</param>
        /// <param name="b">The second object to compare.</param>
        /// <returns>true if a is less than or equal to b; otherwise, false.</returns>
        public static bool operator <=(Uuid a, Uuid b)
        {
            return a.CompareTo(b) <= 0;
        }

        /// <summary>
        /// Indicates whether the values of the first Uuid object is greater than or equal to the other.
        /// </summary>
        /// <param name="a">The first object to compare.</param>
        /// <param name="b">The second object to compare.</param>
        /// <returns>true if a is greater than or equal to b; otherwise, false.</returns>
        public static bool operator >=(Uuid a, Uuid b)
        {
            return a.CompareTo(b) >= 0;
        }
    }
}
