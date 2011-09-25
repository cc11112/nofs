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
    public sealed class Double : Number
    {
        /**
         * The {@link Class} object that represents the primitive type {@code
         * double}.
         *
         * @since 1.1
         */
        public static readonly Type TYPE = new double[0].GetType().GetElementType();
        public Double() { }
        public Double(double d)
        {
            this.value = d;
        }
        public Double (String value) : this (Convert.ToDouble(value)){}
        private double value;

        /**
         * Constant for the maximum {@code double} value, (2 - 2<sup>-52</sup>) *
         * 2<sup>1023</sup>.
         */
        public static readonly double MAX_VALUE = 1.79769313486231570e+308;

        /**
         * Constant for the minimum {@code double} value, 2<sup>-1074</sup>.
         */
        public static readonly double MIN_VALUE = 5e-324;

        /**
         * Constant for the Not-a-Number (NaN) value of the {@code double} type.
         */
        public static readonly double NaN = 0.0 / 0.0;

        /**
         * Constant for the Positive Infinity value of the {@code double} type.
         */
        public static readonly double POSITIVE_INFINITY = 1.0 / 0.0;
    
        /**
         * <p>
         * Maximum exponent that a finite double variable may have.
         * </p>
         * @since 1.6
         */
        public static readonly int MAX_EXPONENT = 1023;
    
            /**
         * <p>
         * Minimum exponent that a finite double variable may have.
         * </p>
         * @since 1.6
         */
        public static readonly int MIN_EXPONENT = -1022;

        /**
         * Constant for the Negative Infinity value of the {@code double} type.
         */
        public static readonly double NEGATIVE_INFINITY = -1.0 / 0.0;

        /**
         * Indicates whether this object is a <em>Not-a-Number (NaN)</em> value.
         * 
         * @return {@code true} if this double is <em>Not-a-Number</em>;
         *         {@code false} if it is a (potentially infinite) double number.
         */
        public bool isNaN()
        {
            return isNaN(value);
        }

        /**
         * Converts the specified IEEE 754 floating-point double precision bit
         * pattern to a Java double value.
         * 
         * @param bits
         *            the IEEE 754 floating-point double precision representation of
         *            a double value.
         * @return the double value converted from {@code bits}.
         * @see #doubleToLongBits(double)
         * @see #doubleToRawLongBits(double)
         */
        public static double longBitsToDouble(long bits)
        {
            return BitConverter.Int64BitsToDouble(bits);
        }

        /**
         * Converts the specified double value to a binary representation conforming
         * to the IEEE 754 floating-point double precision bit layout. All
         * <em>Not-a-Number (NaN)</em> values are converted to a single NaN
         * representation ({@code 0x7ff8000000000000L}).
         * 
         * @param value
         *            the double value to convert.
         * @return the IEEE 754 floating-point double precision representation of
         *         {@code value}.
         * @see #doubleToRawLongBits(double)
         * @see #longBitsToDouble(long)
         */
        public static long doubleToLongBits(double value)
        {
            return BitConverter.DoubleToInt64Bits(value);
        }


        /**
         * Indicates whether the specified double is a <em>Not-a-Number (NaN)</em>
         * value.
         * 
         * @param d
         *            the double value to check.
         * @return {@code true} if {@code d} is <em>Not-a-Number</em>;
         *         {@code false} if it is a (potentially infinite) double number.
         */
        public static bool isNaN(double d)
        {
            return d != d;
        }
        /**
         * Indicates whether this object represents an infinite value.
         * 
         * @return {@code true} if the value of this double is positive or negative
         *         infinity; {@code false} otherwise.
         */
        public bool isInfinite()
        {
            return isInfinite(value);
        }

        /**
         * Indicates whether the specified double represents an infinite value.
         * 
         * @param d
         *            the double to check.
         * @return {@code true} if the value of {@code d} is positive or negative
         *         infinity; {@code false} otherwise.
         */
        public static bool isInfinite(double d)
        {
            return (d == POSITIVE_INFINITY) || (d == NEGATIVE_INFINITY);
        }
        public override String ToString() {
            return Double.toString(value);
        }

        /**
         * Returns a string containing a concise, human-readable description of the
         * specified double value.
         * 
         * @param d
         *             the double to convert to a string.
         * @return a printable representation of {@code d}.
         */
        public static String toString(double d) {
            return d.ToString();
        }

        public static java.lang.Double valueOf(String d)
        {
            Double returnValue = new Double();
            System.Double sysD = Convert.ToDouble(d);
            returnValue.value = sysD;
            return returnValue;
        }

        #region Number implementations
        public override byte byteValue() {
            return (byte) value;
        }

        public override float floatValue() {
            return (float) value;
        }

        public override int intValue() {
            return (int) value;
        }

        public override long longValue()
        {
            return (long)value;
        }

        public override double doubleValue()
        {
            return value;
        }
        
        public override short shortValue() {
            return (short) value;
        }
        #endregion
    }
}
