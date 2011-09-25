/*
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at 
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *  
 */
using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.lang
{
    /**
     * The wrapper for the primitive type {@code byte}.
     *
     * @since 1.1
     */
    [Serializable]
    public sealed class Byte : Number, Comparable<java.lang.Byte>
    {
        /**
         * The {@link Class} object that represents the primitive type {@code byte}.
         */
        public static readonly Type TYPE = new byte[0].GetType().GetElementType();

        private static readonly long serialVersionUID = -7183698231559129828L;

        /**
         * The value which the receiver represents.
         */
        private readonly sbyte value;

        /**
         * The maximum {@code Byte} value, 2<sup>7</sup>-1.
         */
        public static readonly sbyte MAX_VALUE = SByte.MaxValue;

        /**
         * The minimum {@code Byte} value, -2<sup>7</sup>.
         */
        public static readonly sbyte MIN_VALUE = SByte.MinValue;

        /**
         * The number of bits needed to represent a {@code Byte} value in two's
         * complement form.
         * 
         * @since 1.5
         */
        public static readonly int SIZE = 8;

        /**
         * A cache of instances used by {@link #valueOf(byte)} and auto-boxing.
         */
        private static readonly java.lang.Byte[] CACHE = new java.lang.Byte[256];

        /**
         * Constructs a new {@code Byte} with the specified primitive byte value.
         * 
         * @param value
         *            the primitive byte value to store in the new instance.
         */
        public Byte(byte value)
        {
            this.value = (sbyte)value;
        }
        /// <summary>
        /// In addition, set the signed byte value.
        /// </summary>
        /// <param name="value"></param>
        public Byte(sbyte value)
        {
            this.value = value;
        }

        /**
         * Constructs a new {@code Byte} from the specified string.
         * 
         * @param string
         *            the string representation of a single byte value.
         * @throws NumberFormatException
         *             if {@code string} can not be decoded into a byte value.
         * @see #parseByte(String)
         */
        public Byte(String str) ://throws NumberFormatException {
            this(parseByte(str))
        {
        }

        /**
         * Gets the primitive value of this byte.
         * 
         * @return this object's primitive value.
         */
        public override byte byteValue()
        {
            return (byte)value;
        }
        /// <summary>
        /// In addition, returns thesigned byte value.
        /// </summary>
        /// <returns></returns>
        public sbyte sbyteValue()
        {
            return this.value;
        }

        /**
         * Compares this object to the specified byte object to determine their
         * relative order.
         * 
         * @param object
         *            the byte object to compare this object to.
         * @return a negative value if the value of this byte is less than the value
         *         of {@code object}; 0 if the value of this byte and the value of
         *         {@code object} are equal; a positive value if the value of this
         *         byte is greater than the value of {@code object}.
         * @see java.lang.Comparable
         * @since 1.2
         */
        public int compareTo(java.lang.Byte obj)
        {
            return value > obj.value ? 1 : (value < obj.value ? -1 : 0);
        }

        /**
         * Parses the specified string and returns a {@code Byte} instance if the
         * string can be decoded into a single byte value. The string may be an
         * optional minus sign "-" followed by a hexadecimal ("0x..." or "#..."),
         * octal ("0..."), or decimal ("...") representation of a byte.
         * 
         * @param string
         *            a string representation of a single byte value.
         * @return a {@code Byte} containing the value represented by {@code string}.
         * @throws NumberFormatException
         *             if {@code string} can not be parsed as a byte value.
         */
        public static Byte decode(String str)
        {// throws NumberFormatException {
            int intValue = Integer.decode(str).intValue();
            byte result = (byte)intValue;
            if (result == intValue)
            {
                return valueOf(result);
            }
            throw new NumberFormatException();
        }

        public override double doubleValue()
        {
            return value;
        }

        /**
         * Compares this object with the specified object and indicates if they are
         * equal. In order to be equal, {@code object} must be an instance of
         * {@code Byte} and have the same byte value as this object.
         * 
         * @param object
         *            the object to compare this byte with.
         * @return {@code true} if the specified object is equal to this
         *         {@code Byte}; {@code false} otherwise.
         */
        public override bool Equals(Object obj)
        {
            return (obj == this) || (obj is java.lang.Byte)
                    && (value == ((java.lang.Byte)obj).value);
        }

        public override float floatValue()
        {
            return value;
        }

        public override int GetHashCode()
        {
            return value;
        }

        public override int intValue()
        {
            return value;
        }

        public override long longValue()
        {
            return value;
        }

        /**
         * Parses the specified string as a signed decimal byte value. The ASCII
         * character \u002d ('-') is recognized as the minus sign.
         * 
         * @param string
         *            the string representation of a single byte value.
         * @return the primitive byte value represented by {@code string}.
         * @throws NumberFormatException
         *             if {@code string} is {@code null}, has a length of zero or
         *             can not be parsed as a byte value.
         */
        public static byte parseByte(String str)
        {//throws NumberFormatException {
            int intValue = Integer.parseInt(str);
            byte result = (byte)intValue;
            if (result == intValue)
            {
                return result;
            }
            throw new NumberFormatException();
        }

        /**
         * Parses the specified string as a signed byte value using the specified
         * radix. The ASCII character \u002d ('-') is recognized as the minus sign.
         * 
         * @param string
         *            the string representation of a single byte value.
         * @param radix
         *            the radix to use when parsing.
         * @return the primitive byte value represented by {@code string} using
         *         {@code radix}.
         * @throws NumberFormatException
         *             if {@code string} is {@code null} or has a length of zero,
         *             {@code radix < Character.MIN_RADIX},
         *             {@code radix > Character.MAX_RADIX}, or if {@code string}
         *             can not be parsed as a byte value.
         */
        public static byte parseByte(String str, int radix)
        {//throws NumberFormatException {
            int intValue = Integer.parseInt(str, radix);
            byte result = (byte)intValue;
            if (result == intValue)
            {
                return result;
            }
            throw new NumberFormatException();
        }

        public override short shortValue()
        {
            return value;
        }

        public override String ToString()
        {
            return Integer.toString(value);
        }

        /**
         * Returns a string containing a concise, human-readable description of the
         * specified byte value.
         * 
         * @param value
         *            the byte to convert to a string.
         * @return a printable representation of {@code value}.
         */
        public static String toString(byte value)
        {
            return Integer.toString(value);
        }

        /**
         * Parses the specified string as a signed decimal byte value.
         * 
         * @param string
         *            the string representation of a single byte value.
         * @return a {@code Byte} instance containing the byte value represented by
         *         {@code string}.
         * @throws NumberFormatException
         *             if {@code string} is {@code null}, has a length of zero or
         *             can not be parsed as a byte value.
         * @see #parseByte(String)
         */
        public static java.lang.Byte valueOf(String str)
        {// throws NumberFormatException {
            return valueOf(parseByte(str));
        }

        /**
         * Parses the specified string as a signed byte value using the specified
         * radix.
         * 
         * @param string
         *            the string representation of a single byte value.
         * @param radix
         *            the radix to use when parsing.
         * @return a {@code Byte} instance containing the byte value represented by
         *         {@code string} using {@code radix}.
         * @throws NumberFormatException
         *             if {@code string} is {@code null} or has a length of zero,
         *             {@code radix < Character.MIN_RADIX},
         *             {@code radix > Character.MAX_RADIX}, or if {@code string}
         *             can not be parsed as a byte value.
         * @see #parseByte(String, int)
         */
        public static java.lang.Byte valueOf(String str, int radix)
        {//throws NumberFormatException {
            return valueOf(parseByte(str, radix));
        }

        /**
         * Returns a {@code Byte} instance for the specified byte value.
         * <p>
         * If it is not necessary to get a new {@code Byte} instance, it is
         * recommended to use this method instead of the constructor, since it
         * maintains a cache of instances which may result in better performance.
         * 
         * @param b
         *            the byte value to store in the instance.
         * @return a {@code Byte} instance containing {@code b}.
         * @since 1.5
         */
        public static java.lang.Byte valueOf(byte b)
        {
            lock (CACHE)
            {
                int idx = b - MIN_VALUE;
                Byte result = CACHE[idx];
                return (result == null ? CACHE[idx] = new Byte(b) : result);
            }
        }

   }
}