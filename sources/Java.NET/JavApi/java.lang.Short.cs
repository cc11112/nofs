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
 */

using System;
using System.Text;

using java = biz.ritter.javapi;

namespace biz.ritter.javapi.lang
{

    /**
     * The wrapper for the primitive type {@code short}.
     * 
     * @see java.lang.Number
     * @since 1.1
     */
    [Serializable]
    public sealed class Short : Number, Comparable<Short>
    {
        /**
         * The {@link Class} object that represents the primitive type {@code
         * short}.
         */
        public static readonly Type TYPE = new short[0].GetType().GetElementType();

        private static readonly long serialVersionUID = 7515723908773894738L;

        /**
         * The value which the receiver represents.
         */
        private readonly short value;

        /**
         * Constant for the maximum {@code short} value, 2<sup>15</sup>-1.
         */
        public static readonly short MAX_VALUE = short.MaxValue;

        /**
         * Constant for the minimum {@code short} value, -2<sup>15</sup>.
         */
        public static readonly short MIN_VALUE = short.MinValue;

        /**
         * Constant for the number of bits needed to represent a {@code short} in
         * two's complement form.
         *
         * @since 1.5
         */
        public static readonly int SIZE = 16;

        /**
         * Constructs a new {@code Short} from the specified string.
         *
         * @param string
         *            the string representation of a short value.
         * @throws NumberFormatException
         *             if {@code string} can not be decoded into a short value.
         * @see #parseShort(String)
         */
        public Short(String str) ://throws NumberFormatException {
            this(parseShort(str))
        {
        }

        /**
         * Constructs a new {@code Short} with the specified primitive short value.
         *
         * @param value
         *            the primitive short value to store in the new instance.
         */
        public Short(short value)
        {
            this.value = value;
        }


        public override byte byteValue()
        {
            return (byte)value;
        }

        /**
         * Compares this object to the specified short object to determine their
         * relative order.
         * 
         * @param object
         *            the short object to compare this object to.
         * @return a negative value if the value of this short is less than the
         *         value of {@code object}; 0 if the value of this short and the
         *         value of {@code object} are equal; a positive value if the value
         *         of this short is greater than the value of {@code object}.
         * @throws NullPointerException
         *             if {@code object} is null.
         * @see java.lang.Comparable
         * @since 1.2
         */
        public int compareTo(Short obj)
        {
            return value > obj.value ? 1 : (value < obj.value ? -1 : 0);
        }

        /**
         * Parses the specified string and returns a {@code Short} instance if the
         * string can be decoded into a short value. The string may be an optional
         * minus sign "-" followed by a hexadecimal ("0x..." or "#..."), octal
         * ("0..."), or decimal ("...") representation of a short.
         *
         * @param string
         *            a string representation of a short value.
         * @return a {@code Short} containing the value represented by
         *         {@code string}.
         * @throws NumberFormatException
         *             if {@code string} can not be parsed as a short value.
         */
        public static Short decode(String str)
        {//throws NumberFormatException {
            int intValue = Integer.decode(str).intValue();
            short result = (short)intValue;
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
         * Compares this instance with the specified object and indicates if they
         * are equal. In order to be equal, {@code object} must be an instance of
         * {@code Short} and have the same short value as this object.
         *
         * @param object
         *            the object to compare this short with.
         * @return {@code true} if the specified object is equal to this
         *         {@code Short}; {@code false} otherwise.
         */

        public override bool Equals(Object obj)
        {
            return (obj is Short)
                    && (value == ((Short)obj).value);
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
         * Parses the specified string as a signed decimal short value. The ASCII
         * character \u002d ('-') is recognized as the minus sign.
         *
         * @param string
         *            the string representation of a short value.
         * @return the primitive short value represented by {@code string}.
         * @throws NumberFormatException
         *             if {@code string} is {@code null}, has a length of zero or
         *             can not be parsed as a short value.
         */
        public static short parseShort(String str)
        {//throws NumberFormatException {
            return parseShort(str, 10);
        }

        /**
         * Parses the specified string as a signed short value using the specified
         * radix. The ASCII character \u002d ('-') is recognized as the minus sign.
         *
         * @param string
         *            the string representation of a short value.
         * @param radix
         *            the radix to use when parsing.
         * @return the primitive short value represented by {@code string} using
         *         {@code radix}.
         * @throws NumberFormatException
         *             if {@code string} is {@code null} or has a length of zero,
         *             {@code radix < Character.MIN_RADIX},
         *             {@code radix > Character.MAX_RADIX}, or if {@code string}
         *             can not be parsed as a short value.
         */
        public static short parseShort(String str, int radix)
        {//throws NumberFormatException {
            int intValue = Integer.parseInt(str, radix);
            short result = (short)intValue;
            if (result == intValue)
            {
                return result;
            }
            throw new NumberFormatException();
        }

        /**
         * Gets the primitive value of this short.
         *
         * @return this object's primitive value.
         */

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
         * specified short value with radix 10.
         *
         * @param value
         *             the short to convert to a string.
         * @return a printable representation of {@code value}.
         */
        public static String toString(short value)
        {
            return Integer.toString(value);
        }

        /**
         * Parses the specified string as a signed decimal short value.
         *
         * @param string
         *            the string representation of a short value.
         * @return a {@code Short} instance containing the short value represented
         *         by {@code string}.
         * @throws NumberFormatException
         *             if {@code string} is {@code null}, has a length of zero or
         *             can not be parsed as a short value.
         * @see #parseShort(String)
         */
        public static Short valueOf(String str)
        {//throws NumberFormatException {
            return valueOf(parseShort(str));
        }

        /**
         * Parses the specified string as a signed short value using the specified
         * radix.
         *
         * @param string
         *            the string representation of a short value.
         * @param radix
         *            the radix to use when parsing.
         * @return a {@code Short} instance containing the short value represented
         *         by {@code string} using {@code radix}.
         * @throws NumberFormatException
         *             if {@code string} is {@code null} or has a length of zero,
         *             {@code radix < Character.MIN_RADIX},
         *             {@code radix > Character.MAX_RADIX}, or if {@code string}
         *             can not be parsed as a short value.
         * @see #parseShort(String, int)
         */
        public static Short valueOf(String str, int radix)
        {//throws NumberFormatException {
            return valueOf(parseShort(str, radix));
        }

        /**
         * Reverses the bytes of the specified short.
         * 
         * @param s
         *            the short value for which to reverse bytes.
         * @return the reversed value.
         * @since 1.5
         */
        public static short reverseBytes(short s)
        {
            int high = (s >> 8) & 0xFF;
            int low = (s & 0xFF) << 8;
            return (short)(low | high);
        }

        /**
         * Returns a {@code Short} instance for the specified short value.
         * <p>
         * If it is not necessary to get a new {@code Short} instance, it is
         * recommended to use this method instead of the constructor, since it
         * maintains a cache of instances which may result in better performance.
         *
         * @param s
         *            the short value to store in the instance.
         * @return a {@code Short} instance containing {@code s}.
         * @since 1.5
         */
        public static Short valueOf(short s)
        {
            if (s < -128 || s > 127)
            {
                return new Short(s);
            }
            return IAC_SHORT_valueOfCache.CACHE[s + 128];
        }

    }

    internal class IAC_SHORT_valueOfCache
    {
        /**
         * A cache of instances used by {@link Short#valueOf(short)} and auto-boxing.
         */
        internal static readonly Short[] CACHE = new Short[256];

        static IAC_SHORT_valueOfCache()
        {
            for (int i = -128; i <= 127; i++)
            {
                CACHE[i + 128] = new Short((short)i);
            }
        }
    }
}