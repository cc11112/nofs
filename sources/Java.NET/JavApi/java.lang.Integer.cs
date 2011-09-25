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
    public sealed class Integer : Number, Comparable<Integer>
    {
        /**
         * The {@link Class} object that represents the primitive type {@code int}.
         */
        public static readonly Type TYPE = new int[0].GetType().GetElementType();
        /*
         * Progressively smaller decimal order of magnitude that can be represented
         * by an instance of Integer. Used to help compute the String
         * representation.
         */
        private static readonly int[] decimalScale = new int[] { 1000000000, 100000000,
                10000000, 1000000, 100000, 10000, 1000, 100, 10, 1 };
        private int value;

        public const int MAX_VALUE = int.MaxValue;
        public const int MIN_VALUE = int.MinValue;

        public Integer(int newValue)
        {
            this.value = newValue;
        }
        public Integer (String value) : this (parseInt(value)){}

        public override int intValue()
        {
            return value;
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
        public static java.lang.StringJ toString(int value, int radix)
        {
            return new StringJ(Integer.ToString(value, radix));
        }
        public int compareTo(Integer other)
        {
            return (this.value == other.value) ? 0 : (this.value > other.value) ? -1 : +1;
        }

        internal static System.String ToString(int value, int radix)
        {
            return Convert.ToString(value, radix);
        }
        public static int parseInt(String s)
        {
            return int.Parse(s);
        }
        /**
         * Parses the specified string as a signed integer value using the specified
         * radix.
         * 
         * @param string
         *            the string representation of an integer value.
         * @param radix
         *            the radix to use when parsing.
         * @return an {@code Integer} instance containing the integer value
         *         represented by {@code string} using {@code radix}.
         * @throws NumberFormatException
         *             if {@code string} is {@code null} or has a length of zero,
         *             {@code radix < Character.MIN_RADIX},
         *             {@code radix > Character.MAX_RADIX}, or if {@code string}
         *             can not be parsed as an integer value.
         * @see #parseInt(String, int)
         */
        public static int parseInt(String s, int radix)
        {
            try
            {
                Int64 i = Convert.ToInt64(s, radix);
                if (i < int.MinValue || i > int.MaxValue) throw new java.lang.NumberFormatException();
                return (int)i;
            }
            catch (FormatException fe)
            {
                throw new java.lang.NumberFormatException(fe.Message);
            }
            catch (OverflowException oe)
            {
                throw new java.lang.NumberFormatException();
            }
        }
        /**
         * Determines the number of trailing zeros in the specified integer after
         * the {@link #lowestOneBit(int) lowest one bit}.
         *
         * @param i
         *            the integer to examine.
         * @return the number of trailing zeros in {@code i}.
         * @since 1.5
         */
        public static int numberOfTrailingZeros(int i)
        {
            return bitCount((i & -i) - 1);
        }

        /**
         * Counts the number of 1 bits in the specified integer; this is also
         * referred to as population count.
         *
         * @param i
         *            the integer to examine.
         * @return the number of 1 bits in {@code i}.
         * @since 1.5
         */
        public static int bitCount(int i)
        {
            i -= ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            i = (((i >> 4) + i) & 0x0F0F0F0F);
            i += (i >> 8);
            i += (i >> 16);
            return (i & 0x0000003F);
        }

        /**
         * Converts the specified integer into its decimal string representation.
         * The returned string is a concatenation of a minus sign if the number is
         * negative and characters from '0' to '9'.
         * 
         * @param value
         *            the integer to convert.
         * @return the decimal string representation of {@code value}.
         */
        public static String toString(int value)
        {
            if (value == 0)
            {
                return "0"; //$NON-NLS-1$
            }

            // Faster algorithm for smaller Integers
            if (value < 1000 && value > -1000)
            {
                char[] buffer = new char[4];
                int positive_value = value < 0 ? -value : value;
                int first_digit = 0;
                if (value < 0)
                {
                    buffer[0] = '-';
                    first_digit++;
                }
                int last_digit = first_digit;
                int quot = positive_value;
                do
                {
                    int res = quot / 10;
                    int digit_value = quot - ((res << 3) + (res << 1));
                    digit_value += '0';
                    buffer[last_digit++] = (char)digit_value;
                    quot = res;
                } while (quot != 0);

                int count = last_digit--;
                do
                {
                    char tmp = buffer[last_digit];
                    buffer[last_digit--] = buffer[first_digit];
                    buffer[first_digit++] = tmp;
                } while (first_digit < last_digit);
                return new java.lang.StringJ(0, count, buffer).ToString();
            }
            if (value == MIN_VALUE)
            {
                return "-2147483648";//$NON-NLS-1$
            }

            char[] bufferJ = new char[11];
            int positive_valueJ = value < 0 ? -value : value;
            byte first_digitJ = 0;
            if (value < 0)
            {
                bufferJ[0] = '-';
                first_digitJ++;
            }
            byte last_digitJ = first_digitJ;
            byte countJ;
            int number;
            bool start = false;
            for (int i = 0; i < 9; i++)
            {
                countJ = 0;
                if (positive_valueJ < (number = decimalScale[i]))
                {
                    if (start)
                    {
                        bufferJ[last_digitJ++] = '0';
                    }
                    continue;
                }

                if (i > 0)
                {
                    number = (decimalScale[i] << 3);
                    if (positive_valueJ >= number)
                    {
                        positive_valueJ -= number;
                        countJ += 8;
                    }
                    number = (decimalScale[i] << 2);
                    if (positive_valueJ >= number)
                    {
                        positive_valueJ -= number;
                        countJ += 4;
                    }
                }
                number = (decimalScale[i] << 1);
                if (positive_valueJ >= number)
                {
                    positive_valueJ -= number;
                    countJ += 2;
                }
                if (positive_valueJ >= decimalScale[i])
                {
                    positive_valueJ -= decimalScale[i];
                    countJ++;
                }
                if (countJ > 0 && !start)
                {
                    start = true;
                }
                if (start)
                {
                    bufferJ[last_digitJ++] = (char)(countJ + '0');
                }
            }

            bufferJ[last_digitJ++] = (char)(positive_valueJ + '0');
            countJ = last_digitJ--;
            return new java.lang.StringJ(0, countJ, bufferJ).ToString();
        }
        /**
         * Determines the number of leading zeros in the specified integer prior to
         * the {@link #highestOneBit(int) highest one bit}.
         *
         * @param i
         *            the integer to examine.
         * @return the number of leading zeros in {@code i}.
         * @since 1.5
         */
        public static int numberOfLeadingZeros(int i)
        {
            i |= i >> 1;
            i |= i >> 2;
            i |= i >> 4;
            i |= i >> 8;
            i |= i >> 16;
            return bitCount(~i);
        }
        /**
         * Determines the highest (leftmost) bit of the specified integer that is 1
         * and returns the bit mask value for that bit. This is also referred to as
         * the Most Significant 1 Bit. Returns zero if the specified integer is
         * zero.
         * 
         * @param i
         *            the integer to examine.
         * @return the bit mask indicating the highest 1 bit in {@code i}.
         * @since 1.5
         */
        public static int highestOneBit(int i) {
            i |= (i >> 1);
            i |= (i >> 2);
            i |= (i >> 4);
            i |= (i >> 8);
            i |= (i >> 16);
            return (i & ~ java.dotnet.lang.Operator.shiftRightUnsignet (i, 1));
        }

        /**
         * Returns the value of the {@code signum} function for the specified
         * integer.
         * 
         * @param i
         *            the integer value to check.
         * @return -1 if {@code i} is negative, 1 if {@code i} is positive, 0 if
         *         {@code i} is zero.
         * @since 1.5
         */
        public static int signum(int i)
        {
            return (i == 0 ? 0 : (i < 0 ? -1 : 1));
        }


        /**
         * Parses the specified string and returns a {@code Integer} instance if the
         * string can be decoded into an integer value. The string may be an
         * optional minus sign "-" followed by a hexadecimal ("0x..." or "#..."),
         * octal ("0..."), or decimal ("...") representation of an integer.
         * 
         * @param string
         *            a string representation of an integer value.
         * @return an {@code Integer} containing the value represented by
         *         {@code string}.
         * @throws NumberFormatException
         *             if {@code string} can not be parsed as an integer value.
         */
        public static Integer decode(String str) {//throws NumberFormatException {
            int length = str.length(), i = 0;
            if (length == 0) {
                throw new NumberFormatException();
            }
            char firstDigit = str.charAt(i);
            bool negative = firstDigit == '-';
            if (negative) {
                if (length == 1) {
                    throw new NumberFormatException(str);
                }
                firstDigit = str.charAt(++i);
            }

            int baseJ = 10;
            if (firstDigit == '0') {
                if (++i == length) {
                    return valueOf(0);
                }
                if ((firstDigit = str.charAt(i)) == 'x' || firstDigit == 'X') {
                    if (++i == length) {
                        throw new NumberFormatException(str);
                    }
                    baseJ = 16;
                } else {
                    baseJ = 8;
                }
            } else if (firstDigit == '#') {
                if (++i == length) {
                    throw new NumberFormatException(str);
                }
                baseJ = 16;
            }

            int result = parse(str, i, baseJ, negative);
            return valueOf(result);
        }
        /**
         * Parses the specified string as a signed decimal integer value.
         * 
         * @param string
         *            the string representation of an integer value.
         * @return an {@code Integer} instance containing the integer value
         *         represented by {@code string}.
         * @throws NumberFormatException
         *             if {@code string} is {@code null}, has a length of zero or
         *             can not be parsed as an integer value.
         * @see #parseInt(String)
         */
        public static Integer valueOf(String str){// throws NumberFormatException {
            return valueOf(parseInt(str));
        }


        private static int parse(String str, int offset, int radix,
                bool negative) {//throws NumberFormatException {
            int max = Integer.MIN_VALUE / radix;
            int result = 0, length = str.length();
            while (offset < length) {
                int digit = Character.digit(str.charAt(offset++), radix);
                if (digit == -1) {
                    throw new NumberFormatException(str);
                }
                if (max > result) {
                    throw new NumberFormatException(str);
                }
                int next = result * radix - digit;
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
     * Returns a {@code Integer} instance for the specified integer value.
     * <p>
     * If it is not necessary to get a new {@code Integer} instance, it is
     * recommended to use this method instead of the constructor, since it
     * maintains a cache of instances which may result in better performance.
     *
     * @param i
     *            the integer value to store in the instance.
     * @return a {@code Integer} instance containing {@code i}.
     * @since 1.5
     */
    public static Integer valueOf(int i) {
        if (i < -128 || i > 127) {
            return new Integer(i);
        }
        return IAC_INTERGER_valueOfCache.CACHE[i + 128];

    }

    }
   internal class IAC_INTERGER_valueOfCache {
        /**
         * <p>
         * A cache of instances used by {@link Integer#valueOf(int)} and auto-boxing.
         */
        internal static readonly Integer[] CACHE = new Integer[256];

        static IAC_INTERGER_valueOfCache(){
            for(int i=-128; i<=127; i++) {
                CACHE[i+128] = new Integer(i);
            }
        }
    }
}
