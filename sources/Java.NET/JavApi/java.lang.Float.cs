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
     * The wrapper for the primitive type {@code float}.
     * 
     * @see java.lang.Number
     * @since 1.0
     */
    public sealed class Float : Number
    {//, Comparable<Float> {

        public static java.lang.Float valueOf(String d)
        {
            System.Single sysD = Convert.ToSingle(d);
            Float returnValue = new Float(sysD);
            return returnValue;
        }
        /**
         * The {@link Class} object that represents the primitive type {@code
         * float}.
         *
         * @since 1.1
         */
        public static readonly Type TYPE = new float[0].GetType().GetElementType();
        public Float(String value) : this(Convert.ToSingle(value)) { }
        public Float () {}
        public Float(float f)
        {
            this.value = f;
        }

        private static readonly long serialVersionUID = -2671257302660747028L;

        /**
         * The value which the receiver represents.
         */
        private readonly float value;

        /**
         * Constant for the maximum {@code float} value, (2 - 2<sup>-23</sup>) * 2<sup>127</sup>.
         */
        public static readonly float MAX_VALUE = 3.40282346638528860e+38f;

        /**
         * Constant for the minimum {@code float} value, 2<sup>-149</sup>.
         */
        public static readonly float MIN_VALUE = 1.40129846432481707e-45f;

        /**
         * <p>
         * Constant for the smallest positive normal value of <code>float</code>.
         * </p>
         * @since 1.6
         */
        public static readonly float MIN_NORMAL = 1.1754943508222875E-38f;

        /**
         * Constant for the Not-a-Number (NaN) value of the {@code float} type.
         */
        public static readonly float NaN = 0.0f / 0.0f;

        /**
         * Constant for the Positive Infinity value of the {@code float} type.
         */
        public static readonly float POSITIVE_INFINITY = 1.0f / 0.0f;

        /**
         * Constant for the Negative Infinity value of the {@code float} type.
         */
        public static readonly float NEGATIVE_INFINITY = -1.0f / 0.0f;


        public override byte byteValue()
        {
            return (byte)value;
        }

        public override short shortValue()
        {
            return (short)value;
        }
        public override float floatValue()
        {
            return value;
        }
        public override long longValue()
        {
            return (long)value;
        }
        public override int intValue()
        {
            return (int)value;
        }
        public override double doubleValue()
        {
            return value;
        }
        public static String toString(float f)
        {
            return Convert.ToString(f);
        }

        private const int JAVA_FLOAT_NAN_VALUE = 2143289344;
        public static int floatToIntBits(float f)
        {
            if (Float.NaN == f) return JAVA_FLOAT_NAN_VALUE; // not 4290772992
            return BitConverter.ToInt32(BitConverter.GetBytes(f), 0);
        }
        public static float intBitsToFloat(int value)
        {
            if (JAVA_FLOAT_NAN_VALUE == value) return Float.NaN;
            return BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
        }
        /**
         * Indicates whether this object is a <em>Not-a-Number (NaN)</em> value.
         * 
         * @return {@code true} if this float is <em>Not-a-Number</em>;
         *         {@code false} if it is a (potentially infinite) float number.
         */
        public bool isNaN()
        {
            return isNaN(value);
        }

        /**
         * Indicates whether the specified float is a <em>Not-a-Number (NaN)</em>
         * value.
         * 
         * @param f
         *            the float value to check.
         * @return {@code true} if {@code f} is <em>Not-a-Number</em>;
         *         {@code false} if it is a (potentially infinite) float number.
         */
        public static bool isNaN(float f)
        {
            return f == Float.NaN;
        }

    }
}
