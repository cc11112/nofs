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
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.lang
{

    public sealed class Long : Number, Comparable<Long>
    {
        public static java.lang.Long valueOf(String d)
        {
            System.Int64 sysD = Convert.ToInt64(d);
            Long returnValue = new Long(sysD);
            return returnValue;
        }
        /**
         * The {@link Class} object that represents the primitive type {@code long}.
         */
        public static readonly Type TYPE = new long[0].GetType().GetElementType();
        public static readonly long MAX_VALUE = long.MaxValue;
        public static readonly long MIN_VALUE = long.MinValue;
        private long value;

        public Long(long newValue)
        {
            this.value = newValue;
        }
        public Long (String value) : this (parseLong(value)){}

        public static String toOctalString (long i) {
            return Convert.ToString(i, 8);
        }
        public static String toHexString(long i)
        {
            return Convert.ToString(i, 16);
        }

        public override int intValue()
        {
            return (int)value;
        }
        public override long longValue()
        {
            return value;
        }
        public override float floatValue()
        {
            return (float)value;
        }
        public override double doubleValue()
        {
            return (double)value;
        }
        public override byte byteValue()
        {
            return (byte)value;
        }
        public override short shortValue()
        {
            return (short)value;
        }
        public int compareTo(Long other)
        {
            return (this.value == other.value) ? 0 : (this.value > other.value) ? -1 : +1;
        }
        public static long parseLong(String s)
        {
            return long.Parse(s);
        }
        /**
         * Parses the specified string as a signed long value using the specified
         * radix. The ASCII character \u002d ('-') is recognized as the minus sign.
         * 
         * @param string
         *            the string representation of a long value.
         * @param radix
         *            the radix to use when parsing.
         * @return the primitive long value represented by {@code string} using
         *         {@code radix}.
         * @throws NumberFormatException
         *             if {@code string} is {@code null} or has a length of zero,
         *             {@code radix < Character.MIN_RADIX},
         *             {@code radix > Character.MAX_RADIX}, or if {@code string}
         *             can not be parsed as a long value.
         */
        public static long parseLong(String str, int radix) {//            throws NumberFormatException {
            if (str == null || radix < Character.MIN_RADIX
                    || radix > Character.MAX_RADIX) {
                throw new NumberFormatException();
            }
            int length = str.Length, i = 0;
            if (length == 0) {
                throw new NumberFormatException(str);
            }
            bool negative = str[i] == '-';
            if (negative && ++i == length) {
                throw new NumberFormatException(str);
            }

            return parse(str, i, radix, negative);
        }

        private static long parse(String str, int offset, int radix, bool negative) {
            long max = Long.MIN_VALUE / radix;
            long result = 0, length = str.Length;
            while (offset < length) {
                int digit = Character.digit(str[offset++], radix);
                if (digit == -1) {
                    throw new NumberFormatException(str);
                }
                if (max > result) {
                    throw new NumberFormatException(str);
                }
                long next = result * radix - digit;
                if (next > result) {
                    throw new NumberFormatException(str);
                }
                result = next;
            }
            if (!negative) {
                result = -result;
                if (result < 0) {
                    throw new NumberFormatException(str);
                }
            }
            return result;
        }
        /**
         * Converts the specified long value into its decimal string representation.
         * The returned string is a concatenation of a minus sign if the number is
         * negative and characters from '0' to '9'.
         * 
         * @param l
         *            the long to convert.
         * @return the decimal string representation of {@code l}.
         */
        public static String toString(long l)
        {
            return toString(l, 10);
        }

        /**
         * Converts the specified long value into a string representation based on
         * the specified radix. The returned string is a concatenation of a minus
         * sign if the number is negative and characters from '0' to '9' and 'a' to
         * 'z', depending on the radix. If {@code radix} is not in the interval
         * defined by {@code Character.MIN_RADIX} and {@code Character.MAX_RADIX}
         * then 10 is used as the base for the conversion.
         * 
         * @param l
         *            the long to convert.
         * @param radix
         *            the base to use for the conversion.
         * @return the string representation of {@code l}.
         */
        public static String toString(long l, int radix)
        {
            if (radix < Character.MIN_RADIX || radix > Character.MAX_RADIX)
            {
                radix = 10;
            }
            if (l == 0)
            {
                return "0"; //$NON-NLS-1$
            }

            int count = 2;
            long j = l;
            bool negative = l < 0;
            if (!negative)
            {
                count = 1;
                j = -l;
            }
            while ((l /= radix) != 0)
            {
                count++;
            }

            char[] buffer = new char[count];
            do
            {
                int ch = 0 - (int)(j % radix);
                if (ch > 9)
                {
                    ch = ch - 10 + 'a';
                }
                else
                {
                    ch += '0';
                }
                buffer[--count] = (char)ch;
            } while ((j /= radix) != 0);
            if (negative)
            {
                buffer[0] = '-';
            }
            return new java.lang.StringJ(0, buffer.Length, buffer).ToString();
        }
        /**
         * Determines the number of trailing zeros in the specified long value after
         * the {@link #lowestOneBit(long) lowest one bit}.
         *
         * @param lng
         *            the long to examine.
         * @return the number of trailing zeros in {@code lng}.
         * @since 1.5
         */
        public static int numberOfTrailingZeros(long lng)
        {
            return bitCount((lng & -lng) - 1);
        }

        /**
         * Counts the number of 1 bits in the specified long value; this is also
         * referred to as population count.
         *
         * @param lng
         *            the long to examine.
         * @return the number of 1 bits in {@code lng}.
         * @since 1.5
         */
        public static int bitCount(long lng) {
            lng = (lng & 0x5555555555555555L) + ((lng >> 1) & 0x5555555555555555L);
            lng = (lng & 0x3333333333333333L) + ((lng >> 2) & 0x3333333333333333L);
            // adjust for 64-bit integer
            int i = (int) (java.dotnet.lang.Operator.shiftRightUnsignet (lng, 32) + lng);
            i = (i & 0x0F0F0F0F) + ((i >> 4) & 0x0F0F0F0F);
            i = (i & 0x00FF00FF) + ((i >> 8) & 0x00FF00FF);
            i = (i & 0x0000FFFF) + ((i >> 16) & 0x0000FFFF);
            return i;
        }
        /**
         * Reverses the order of the bits of the specified long value.
         * 
         * @param lng
         *            the long value for which to reverse the bit order.
         * @return the reversed value.
         * @since 1.5
         */
        public static long reverse(long lng)
        {
            // From Hacker's Delight, 7-1, Figure 7-1
            lng = (lng & 0x5555555555555555L) << 1 | (lng >> 1)
                    & 0x5555555555555555L;
            lng = (lng & 0x3333333333333333L) << 2 | (lng >> 2)
                    & 0x3333333333333333L;
            lng = (lng & 0x0F0F0F0F0F0F0F0FL) << 4 | (lng >> 4)
                    & 0x0F0F0F0F0F0F0F0FL;
            return reverseBytes(lng);
        }

        /**
         * Reverses the order of the bytes of the specified long value.
         * 
         * @param lng
         *            the long value for which to reverse the byte order.
         * @return the reversed value.
         * @since 1.5
         */
        public static long reverseBytes(long lng) {
            long b7 = java.dotnet.lang.Operator.shiftRightUnsignet(lng , 56);
            long b6 = java.dotnet.lang.Operator.shiftRightUnsignet(lng, 40) & 0xFF00L;
            long b5 = java.dotnet.lang.Operator.shiftRightUnsignet(lng, 24) & 0xFF0000L;
            long b4 = java.dotnet.lang.Operator.shiftRightUnsignet(lng, 8) & 0xFF000000L;
            long b3 = (lng & 0xFF000000L) << 8;
            long b2 = (lng & 0xFF0000L) << 24;
            long b1 = (lng & 0xFF00L) << 40;
            long b0 = lng << 56;
            return (b0 | b1 | b2 | b3 | b4 | b5 | b6 | b7);
        }

        /**
         * Returns the value of the {@code signum} function for the specified long
         * value.
         * 
         * @param lng
         *            the long value to check.
         * @return -1 if {@code lng} is negative, 1 if {@code lng} is positive, 0 if
         *         {@code lng} is zero.
         * @since 1.5
         */
        public static int signum(long lng)
        {
            return (lng == 0 ? 0 : (lng < 0 ? -1 : 1));
        }
        /**
         * Determines the number of leading zeros in the specified long value prior
         * to the {@link #highestOneBit(long) highest one bit}.
         *
         * @param lng
         *            the long to examine.
         * @return the number of leading zeros in {@code lng}.
         * @since 1.5
         */
        public static int numberOfLeadingZeros(long lng)
        {
            lng |= lng >> 1;
            lng |= lng >> 2;
            lng |= lng >> 4;
            lng |= lng >> 8;
            lng |= lng >> 16;
            lng |= lng >> 32;
            return bitCount(~lng);
        }

    }
}
