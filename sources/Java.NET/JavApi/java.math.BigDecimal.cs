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
using System.Text;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.math
{
    /**
     * This class represents immutable arbitrary precision decimal numbers. Each
     * {@code BigDecimal} instance is represented with a unscaled arbitrary
     * precision mantissa (the unscaled value) and a scale. The value of the {@code
     * BigDecimal} is {@code unscaledValue} 10^(-{@code scale}).
     */
    [Serializable]
    public class BigDecimal : java.lang.Number, java.lang.Comparable<BigDecimal>, java.io.Serializable {

        /**
         * The constant zero as a {@code BigDecimal}.
         */
        public static readonly BigDecimal ZERO = new BigDecimal(0, 0);

        /**
         * The constant one as a {@code BigDecimal}.
         */
        public static readonly BigDecimal ONE = new BigDecimal(1, 0);

        /**
         * The constant ten as a {@code BigDecimal}.
         */
        public static readonly BigDecimal TEN = new BigDecimal(10, 0);

        /**
         * Rounding mode where positive values are rounded towards positive infinity
         * and negative values towards negative infinity.
         *
         * @see RoundingMode#UP
         */
        public const int ROUND_UP = 0;

        /**
         * Rounding mode where the values are rounded towards zero.
         *
         * @see RoundingMode#DOWN
         */
        public const int ROUND_DOWN = 1;

        /**
         * Rounding mode to round towards positive infinity. For positive values
         * this rounding mode behaves as {@link #ROUND_UP}, for negative values as
         * {@link #ROUND_DOWN}.
         *
         * @see RoundingMode#CEILING
         */
        public const int ROUND_CEILING = 2;

        /**
         * Rounding mode to round towards negative infinity. For positive values
         * this rounding mode behaves as {@link #ROUND_DOWN}, for negative values as
         * {@link #ROUND_UP}.
         *
         * @see RoundingMode#FLOOR
         */
        public const int ROUND_FLOOR = 3;

        /**
         * Rounding mode where values are rounded towards the nearest neighbor.
         * Ties are broken by rounding up.
         *
         * @see RoundingMode#HALF_UP
         */
        public const int ROUND_HALF_UP = 4;

        /**
         * Rounding mode where values are rounded towards the nearest neighbor.
         * Ties are broken by rounding down.
         *
         * @see RoundingMode#HALF_DOWN
         */
        public const int ROUND_HALF_DOWN = 5;

        /**
         * Rounding mode where values are rounded towards the nearest neighbor.
         * Ties are broken by rounding to the even neighbor.
         *
         * @see RoundingMode#HALF_EVEN
         */
        public const int ROUND_HALF_EVEN = 6;

        /**
         * Rounding mode where the rounding operations throws an {@code
         * ArithmeticException} for the case that rounding is necessary, i.e. for
         * the case that the value cannot be represented exactly.
         *
         * @see RoundingMode#UNNECESSARY
         */
        public const int ROUND_UNNECESSARY = 7;

        /** This is the serialVersionUID used by the sun implementation. */
        private static readonly long serialVersionUID = 6108874887143696463L;

        /** The double closer to <code>Log10(2)</code>. */
        private static readonly double LOG10_2 = 0.3010299956639812;

        /** The <code>String</code> representation is cached. */
        [NonSerialized]
        private String toStringImage = null;

        /** Cache for the hash code. */
        [NonSerialized]
        private int hashCode = 0;

        /**
         * An array with powers of five that fit in the type <code>long</code>
         * (<code>5^0,5^1,...,5^27</code>).
         */
        private static readonly BigInteger[] FIVE_POW;

        /**
         * An array with powers of ten that fit in the type <code>long</code>
         * (<code>10^0,10^1,...,10^18</code>).
         */
        private static readonly BigInteger []TEN_POW;

        /**
         * An array with powers of ten that fit in the type <code>long</code>
         * (<code>10^0,10^1,...,10^18</code>).
         */
        private static readonly long[] LONG_TEN_POW = new long[]
        {   1L,
            10L,
            100L,
            1000L,
            10000L,
            100000L,
            1000000L,
            10000000L,
            100000000L,
            1000000000L,
            10000000000L,
            100000000000L,
            1000000000000L,
            10000000000000L,
            100000000000000L,
            1000000000000000L,
            10000000000000000L,
            100000000000000000L,
            1000000000000000000L, };
    
    
        private static readonly long[] LONG_FIVE_POW = new long[]
        {   1L,
            5L,
            25L,
            125L,
            625L,
            3125L,
            15625L,
            78125L,
            390625L,
            1953125L,
            9765625L,
            48828125L,
            244140625L,
            1220703125L,
            6103515625L,
            30517578125L,
            152587890625L,
            762939453125L,
            3814697265625L,
            19073486328125L,
            95367431640625L,
            476837158203125L,
            2384185791015625L,
            11920928955078125L,
            59604644775390625L,
            298023223876953125L,
            1490116119384765625L,
            7450580596923828125L, };
    
        private static readonly int[] LONG_FIVE_POW_BIT_LENGTH = new int[LONG_FIVE_POW.Length];
        private static readonly int[] LONG_TEN_POW_BIT_LENGTH = new int[LONG_TEN_POW.Length];
    
        private static readonly int BI_SCALED_BY_ZERO_LENGTH = 11;

        /**
         * An array with the first <code>BigInteger</code> scaled by zero.
         * (<code>[0,0],[1,0],...,[10,0]</code>).
         */
        private static readonly BigDecimal[] BI_SCALED_BY_ZERO = new BigDecimal[BI_SCALED_BY_ZERO_LENGTH];

        /**
         * An array with the zero number scaled by the first positive scales.
         * (<code>0*10^0, 0*10^1, ..., 0*10^10</code>).
         */
        private static readonly BigDecimal []ZERO_SCALED_BY = new BigDecimal[11];

        /** An array filled with characters <code>'0'</code>. */
        private static readonly char[] CH_ZEROS = new char[100];

        static BigDecimal() {
            // To fill all static arrays.
            int i = 0;

            for (; i < ZERO_SCALED_BY.Length; i++) {
                BI_SCALED_BY_ZERO[i] = new BigDecimal(i, 0);
                ZERO_SCALED_BY[i] = new BigDecimal(0, i);
                CH_ZEROS[i] = '0';
            }
        
            for (; i < CH_ZEROS.Length; i++) {
                CH_ZEROS[i] = '0';
            }
            for(int j=0; j<LONG_FIVE_POW_BIT_LENGTH.Length; j++) {
                LONG_FIVE_POW_BIT_LENGTH[j] = bitLength(LONG_FIVE_POW[j]);
            }
            for(int j=0; j<LONG_TEN_POW_BIT_LENGTH.Length; j++) {
                LONG_TEN_POW_BIT_LENGTH[j] = bitLength(LONG_TEN_POW[j]);
            }
        
            // Taking the references of useful powers.
            TEN_POW = Multiplication.bigTenPows;
            FIVE_POW = Multiplication.bigFivePows;
        }

        /**
         * The arbitrary precision integer (unscaled value) in the internal
         * representation of {@code BigDecimal}.
         */
        private BigInteger intVal;
        [NonSerialized]
        private int bitLengthJ;
        [NonSerialized]
        private long smallValue;

        /** 
         * The 32-bit integer scale in the internal representation of {@code BigDecimal}.
         */
        private int scaleJ;

        /**
         * Represent the number of decimal digits in the unscaled value. This
         * precision is calculated the first time, and used in the following calls
         * of method <code>precision()</code>. Note that some call to the private
         * method <code>inplaceRound()</code> could update this field.
         *
         * @see #precision()
         * @see #inplaceRound(MathContext)
         */
        [NonSerialized]
        private int precisionJ = 0;

        private BigDecimal(long smallValue, int scale){
            this.smallValue = smallValue;
            this.scaleJ = scale;
            this.bitLengthJ = bitLength(smallValue);
        }
    
        private BigDecimal(int smallValue, int scale){
            this.smallValue = smallValue;
            this.scaleJ = scale;
            this.bitLengthJ = bitLength(smallValue);
        }

        /**
         * Constructs a new {@code BigDecimal} instance from a string representation
         * given as a character array.
         *
         * @param in
         *            array of characters containing the string representation of
         *            this {@code BigDecimal}.
         * @param offset
         *            first index to be copied.
         * @param len
         *            number of characters to be used.
         * @throws NullPointerException
         *             if {@code in == null}.
         * @throws NumberFormatException
         *             if {@code offset < 0} or {@code len <= 0} or {@code
         *             offset+len-1 < 0} or {@code offset+len-1 >= in.length}.
         * @throws NumberFormatException
         *             if in does not contain a valid string representation of a big
         *             decimal.
         */
        public BigDecimal(char[] inJ, int offset, int len) {
            int begin = offset; // first index to be copied
            int last = offset + (len - 1); // last index to be copied
            String scaleString = null; // buffer for scale
            StringBuilder unscaledBuffer; // buffer for unscaled value
            long newScale; // the new scale

            if (inJ == null) {
                throw new java.lang.NullPointerException();
            }
            if ((last >= inJ.Length) || (offset < 0) || (len <= 0) || (last < 0)) {
                throw new java.lang.NumberFormatException();
            }
            unscaledBuffer = new StringBuilder(len);
            int bufLength = 0;
            // To skip a possible '+' symbol
            if ((offset <= last) && (inJ[offset] == '+')) {
                offset++;
                begin++;
            }
            int counter = 0;
            bool wasNonZero = false;
            // Accumulating all digits until a possible decimal point
            for (; (offset <= last) && (inJ[offset] != '.')
            && (inJ[offset] != 'e') && (inJ[offset] != 'E'); offset++) {
                if (!wasNonZero) {
                    if (inJ[offset] == '0') {
                        counter++;
                    } else {
                        wasNonZero = true;
                    }
                }

            }
            unscaledBuffer.Append(inJ, begin, offset - begin);
            bufLength += offset - begin;
            // A decimal point was found
            if ((offset <= last) && (inJ[offset] == '.')) {
                offset++;
                // Accumulating all digits until a possible exponent
                begin = offset;
                for (; (offset <= last) && (inJ[offset] != 'e')
                && (inJ[offset] != 'E'); offset++) {
                    if (!wasNonZero) {
                        if (inJ[offset] == '0') {
                            counter++;
                        } else {
                            wasNonZero = true;
                        }
                    }
                }
                scaleJ = offset - begin;
                bufLength +=scaleJ;
                unscaledBuffer.Append(inJ, begin, scaleJ);
            } else {
                scaleJ = 0;
            }
            // An exponent was found
            if ((offset <= last) && ((inJ[offset] == 'e') || (inJ[offset] == 'E'))) {
                offset++;
                // Checking for a possible sign of scale
                begin = offset;
                if ((offset <= last) && (inJ[offset] == '+')) {
                    offset++;
                    if ((offset <= last) && (inJ[offset] != '-')) {
                        begin++;
                    }
                }
                // Accumulating all remaining digits
                scaleString = java.lang.StringJ.valueOf(inJ, begin, last + 1 - begin).ToString();
                // Checking if the scale is defined            
                newScale = (long)scaleJ - java.lang.Integer.parseInt(scaleString);
                scaleJ = (int)newScale;
                if (newScale != scaleJ) {
                    // math.02=Scale out of range.
                    throw new java.lang.NumberFormatException("Scale out of range."); //$NON-NLS-1$
                }
            }
            // Parsing the unscaled value
            if (bufLength < 19) {
                smallValue = java.lang.Long.parseLong(unscaledBuffer.toString());
                bitLengthJ = bitLength(smallValue);
            } else {
                setUnscaledValue(new BigInteger(unscaledBuffer.toString()));
            }        
            precisionJ = unscaledBuffer.Length - counter;
            if (unscaledBuffer[0] == '-') {
                precisionJ --;
            }    
        }

        /**
         * Constructs a new {@code BigDecimal} instance from a string representation
         * given as a character array.
         *
         * @param in
         *            array of characters containing the string representation of
         *            this {@code BigDecimal}.
         * @param offset
         *            first index to be copied.
         * @param len
         *            number of characters to be used.
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @throws NullPointerException
         *             if {@code in == null}.
         * @throws NumberFormatException
         *             if {@code offset < 0} or {@code len <= 0} or {@code
         *             offset+len-1 < 0} or {@code offset+len-1 >= in.length}.
         * @throws NumberFormatException
         *             if {@code in} does not contain a valid string representation
         *             of a big decimal.
         * @throws ArithmeticException
         *             if {@code mc.precision > 0} and {@code mc.roundingMode ==
         *             UNNECESSARY} and the new big decimal cannot be represented
         *             within the given precision without rounding.
         */
        public BigDecimal(char[] inJ, int offset, int len, MathContext mc) :this(inJ, offset, len){
            inplaceRound(mc);
        }

        /**
         * Constructs a new {@code BigDecimal} instance from a string representation
         * given as a character array.
         *
         * @param in
         *            array of characters containing the string representation of
         *            this {@code BigDecimal}.
         * @throws NullPointerException
         *             if {@code in == null}.
         * @throws NumberFormatException
         *             if {@code in} does not contain a valid string representation
         *             of a big decimal.
         */
        public BigDecimal(char[] inJ) :this(inJ, 0, inJ.Length){
        }

        /**
         * Constructs a new {@code BigDecimal} instance from a string representation
         * given as a character array. The result is rounded according to the
         * specified math context.
         *
         * @param in
         *            array of characters containing the string representation of
         *            this {@code BigDecimal}.
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @throws NullPointerException
         *             if {@code in == null}.
         * @throws NumberFormatException
         *             if {@code in} does not contain a valid string representation
         *             of a big decimal.
         * @throws ArithmeticException
         *             if {@code mc.precision > 0} and {@code mc.roundingMode ==
         *             UNNECESSARY} and the new big decimal cannot be represented
         *             within the given precision without rounding.
         */
        public BigDecimal(char[] inJ, MathContext mc) :this(inJ, 0, inJ.Length){
            inplaceRound(mc);
        }

        /**
         * Constructs a new {@code BigDecimal} instance from a string
         * representation.
         *
         * @param val
         *            string containing the string representation of this {@code
         *            BigDecimal}.
         * @throws NumberFormatException
         *             if {@code val} does not contain a valid string representation
         *             of a big decimal.
         */
        public BigDecimal(String val) :this(val.toCharArray(), 0, val.length()){
        }

        /**
         * Constructs a new {@code BigDecimal} instance from a string
         * representation. The result is rounded according to the specified math
         * context.
         *
         * @param val
         *            string containing the string representation of this {@code
         *            BigDecimal}.
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @throws NumberFormatException
         *             if {@code val} does not contain a valid string representation
         *             of a big decimal.
         * @throws ArithmeticException
         *             if {@code mc.precision > 0} and {@code mc.roundingMode ==
         *             UNNECESSARY} and the new big decimal cannot be represented
         *             within the given precision without rounding.
         */
        public BigDecimal(String val, MathContext mc) :this(val.toCharArray(), 0, val.length()){
            inplaceRound(mc);
        }

        /**
         * Constructs a new {@code BigDecimal} instance from the 64bit double
         * {@code val}. The constructed big decimal is equivalent to the given
         * double. For example, {@code new BigDecimal(0.1)} is equal to {@code
         * 0.1000000000000000055511151231257827021181583404541015625}. This happens
         * as {@code 0.1} cannot be represented exactly in binary.
         * <p>
         * To generate a big decimal instance which is equivalent to {@code 0.1} use
         * the {@code BigDecimal(String)} constructor.
         *
         * @param val
         *            double value to be converted to a {@code BigDecimal} instance.
         * @throws NumberFormatException
         *             if {@code val} is infinity or not a number.
         */
        public BigDecimal(double val) {
            if (java.lang.Double.isInfinite(val) || java.lang.Double.isNaN(val)) {
                // math.03=Infinity or NaN
                throw new java.lang.NumberFormatException("Infinity or NaN"); //$NON-NLS-1$
            }
            long bits = java.lang.Double.doubleToLongBits(val); // IEEE-754
            long mantisa;
            int trailingZeros;
            // Extracting the exponent, note that the bias is 1023
            scaleJ = 1075 - (int)((bits >> 52) & 0x7FFL);
            // Extracting the 52 bits of the mantisa.
            mantisa = (scaleJ == 1075) ? (bits & 0xFFFFFFFFFFFFFL) << 1
                    : (bits & 0xFFFFFFFFFFFFFL) | 0x10000000000000L;
            if (mantisa == 0) {
                scaleJ = 0;
                precisionJ = 1;
            }
            // To simplify all factors '2' in the mantisa 
            if (scaleJ > 0) {
                trailingZeros = java.lang.Math.min(scaleJ, java.lang.Long.numberOfTrailingZeros(mantisa));
                mantisa= java.dotnet.lang.Operator.shiftRightUnsignet(mantisa, trailingZeros);
                scaleJ -= trailingZeros;
            }
            // Calculating the new unscaled value and the new scale
            if((bits >> 63) != 0) {
                mantisa = -mantisa;
            }
            int mantisaBits = bitLength(mantisa);
            if (scaleJ < 0) {
                bitLengthJ = mantisaBits == 0 ? 0 : mantisaBits - scaleJ;
                if(bitLengthJ < 64) {
                    smallValue = mantisa << (-scaleJ);
                } else {
                    intVal = BigInteger.valueOf(mantisa).shiftLeft(-scaleJ);
                }
                scaleJ = 0;
            } else if (scaleJ > 0) {
                // m * 2^e =  (m * 5^(-e)) * 10^e
                if(scaleJ < LONG_FIVE_POW.Length
                        && mantisaBits+LONG_FIVE_POW_BIT_LENGTH[scaleJ] < 64) {
                    smallValue = mantisa * LONG_FIVE_POW[scaleJ];
                    bitLengthJ = bitLength(smallValue);
                } else {
                    setUnscaledValue(Multiplication.multiplyByFivePow(BigInteger.valueOf(mantisa), scaleJ));
                }
            } else { // scale == 0
                smallValue = mantisa;
                bitLengthJ = mantisaBits;
            }
        }

        /**
         * Constructs a new {@code BigDecimal} instance from the 64bit double
         * {@code val}. The constructed big decimal is equivalent to the given
         * double. For example, {@code new BigDecimal(0.1)} is equal to {@code
         * 0.1000000000000000055511151231257827021181583404541015625}. This happens
         * as {@code 0.1} cannot be represented exactly in binary.
         * <p>
         * To generate a big decimal instance which is equivalent to {@code 0.1} use
         * the {@code BigDecimal(String)} constructor.
         *
         * @param val
         *            double value to be converted to a {@code BigDecimal} instance.
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @throws NumberFormatException
         *             if {@code val} is infinity or not a number.
         * @throws ArithmeticException
         *             if {@code mc.precision > 0} and {@code mc.roundingMode ==
         *             UNNECESSARY} and the new big decimal cannot be represented
         *             within the given precision without rounding.
         */
        public BigDecimal(double val, MathContext mc) :this(val){
            inplaceRound(mc);
        }

        /**
         * Constructs a new {@code BigDecimal} instance from the given big integer
         * {@code val}. The scale of the result is {@code 0}.
         *
         * @param val
         *            {@code BigInteger} value to be converted to a {@code
         *            BigDecimal} instance.
         */
        public BigDecimal(BigInteger val):this(val, 0){
        }

        /**
         * Constructs a new {@code BigDecimal} instance from the given big integer
         * {@code val}. The scale of the result is {@code 0}.
         *
         * @param val
         *            {@code BigInteger} value to be converted to a {@code
         *            BigDecimal} instance.
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @throws ArithmeticException
         *             if {@code mc.precision > 0} and {@code mc.roundingMode ==
         *             UNNECESSARY} and the new big decimal cannot be represented
         *             within the given precision without rounding.
         */
        public BigDecimal(BigInteger val, MathContext mc) :this(val){
            inplaceRound(mc);
        }

        /**
         * Constructs a new {@code BigDecimal} instance from a given unscaled value
         * {@code unscaledVal} and a given scale. The value of this instance is
         * {@code unscaledVal} 10^(-{@code scale}).
         *
         * @param unscaledVal
         *            {@code BigInteger} representing the unscaled value of this
         *            {@code BigDecimal} instance.
         * @param scale
         *            scale of this {@code BigDecimal} instance.
         * @throws NullPointerException
         *             if {@code unscaledVal == null}.
         */
        public BigDecimal(BigInteger unscaledVal, int scale) {
            if (unscaledVal == null) {
                throw new java.lang.NullPointerException();
            }
            this.scaleJ = scale;
            setUnscaledValue(unscaledVal);
        }

        /**
         * Constructs a new {@code BigDecimal} instance from a given unscaled value
         * {@code unscaledVal} and a given scale. The value of this instance is
         * {@code unscaledVal} 10^(-{@code scale}). The result is rounded according
         * to the specified math context.
         *
         * @param unscaledVal
         *            {@code BigInteger} representing the unscaled value of this
         *            {@code BigDecimal} instance.
         * @param scale
         *            scale of this {@code BigDecimal} instance.
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @throws ArithmeticException
         *             if {@code mc.precision > 0} and {@code mc.roundingMode ==
         *             UNNECESSARY} and the new big decimal cannot be represented
         *             within the given precision without rounding.
         * @throws NullPointerException
         *             if {@code unscaledVal == null}.
         */
        public BigDecimal(BigInteger unscaledVal, int scale, MathContext mc) :this(unscaledVal, scale){
            inplaceRound(mc);
        }

        /**
         * Constructs a new {@code BigDecimal} instance from the given int
         * {@code val}. The scale of the result is 0.
         *
         * @param val
         *            int value to be converted to a {@code BigDecimal} instance.
         */
        public BigDecimal(int val) :this(val,0){
        }

        /**
         * Constructs a new {@code BigDecimal} instance from the given int {@code
         * val}. The scale of the result is {@code 0}. The result is rounded
         * according to the specified math context.
         *
         * @param val
         *            int value to be converted to a {@code BigDecimal} instance.
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @throws ArithmeticException
         *             if {@code mc.precision > 0} and {@code c.roundingMode ==
         *             UNNECESSARY} and the new big decimal cannot be represented
         *             within the given precision without rounding.
         */
        public BigDecimal(int val, MathContext mc) :this(val,0){
            inplaceRound(mc);
        }

        /**
         * Constructs a new {@code BigDecimal} instance from the given long {@code
         * val}. The scale of the result is {@code 0}.
         *
         * @param val
         *            long value to be converted to a {@code BigDecimal} instance.
         */
        public BigDecimal(long val) :this(val,0){
        }

        /**
         * Constructs a new {@code BigDecimal} instance from the given long {@code
         * val}. The scale of the result is {@code 0}. The result is rounded
         * according to the specified math context.
         *
         * @param val
         *            long value to be converted to a {@code BigDecimal} instance.
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @throws ArithmeticException
         *             if {@code mc.precision > 0} and {@code mc.roundingMode ==
         *             UNNECESSARY} and the new big decimal cannot be represented
         *             within the given precision without rounding.
         */
        public BigDecimal(long val, MathContext mc) :this(val){
            inplaceRound(mc);
        }

        /* Public Methods */

        /**
         * Returns a new {@code BigDecimal} instance whose value is equal to {@code
         * unscaledVal} 10^(-{@code scale}). The scale of the result is {@code
         * scale}, and its unscaled value is {@code unscaledVal}.
         *
         * @param unscaledVal
         *            unscaled value to be used to construct the new {@code
         *            BigDecimal}.
         * @param scale
         *            scale to be used to construct the new {@code BigDecimal}.
         * @return {@code BigDecimal} instance with the value {@code unscaledVal}*
         *         10^(-{@code unscaledVal}).
         */
        public static BigDecimal valueOf(long unscaledVal, int scale) {
            if (scale == 0) {
                return valueOf(unscaledVal);
            }
            if ((unscaledVal == 0) && (scale >= 0)
                    && (scale < ZERO_SCALED_BY.Length)) {
                return ZERO_SCALED_BY[scale];
            }
            return new BigDecimal(unscaledVal, scale);
        }

        /**
         * Returns a new {@code BigDecimal} instance whose value is equal to {@code
         * unscaledVal}. The scale of the result is {@code 0}, and its unscaled
         * value is {@code unscaledVal}.
         *
         * @param unscaledVal
         *            value to be converted to a {@code BigDecimal}.
         * @return {@code BigDecimal} instance with the value {@code unscaledVal}.
         */
        public static BigDecimal valueOf(long unscaledVal) {
            if ((unscaledVal >= 0) && (unscaledVal < BI_SCALED_BY_ZERO_LENGTH)) {
                return BI_SCALED_BY_ZERO[(int)unscaledVal];
            }
            return new BigDecimal(unscaledVal,0);
        }

        /**
         * Returns a new {@code BigDecimal} instance whose value is equal to {@code
         * val}. The new decimal is constructed as if the {@code BigDecimal(String)}
         * constructor is called with an argument which is equal to {@code
         * Double.toString(val)}. For example, {@code valueOf("0.1")} is converted to
         * (unscaled=1, scale=1), although the double {@code 0.1} cannot be
         * represented exactly as a double value. In contrast to that, a new {@code
         * BigDecimal(0.1)} instance has the value {@code
         * 0.1000000000000000055511151231257827021181583404541015625} with an
         * unscaled value {@code 1000000000000000055511151231257827021181583404541015625}
         * and the scale {@code 55}.
         *
         * @param val
         *            double value to be converted to a {@code BigDecimal}.
         * @return {@code BigDecimal} instance with the value {@code val}.
         * @throws NumberFormatException
         *             if {@code val} is infinite or {@code val} is not a number
         */
        public static BigDecimal valueOf(double val) {
            if (java.lang.Double.isInfinite(val) || java.lang.Double.isNaN(val)) {
                // math.03=Infinity or NaN
                throw new java.lang.NumberFormatException("Infinity or NaN"); //$NON-NLS-1$
            }
            return new BigDecimal(java.lang.Double.toString(val));
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this + augend}.
         * The scale of the result is the maximum of the scales of the two
         * arguments.
         *
         * @param augend
         *            value to be added to {@code this}.
         * @return {@code this + augend}.
         * @throws NullPointerException
         *             if {@code augend == null}.
         */
        public BigDecimal add(BigDecimal augend) {
            int diffScale = this.scaleJ - augend.scaleJ;
            // Fast return when some operand is zero
            if (this.isZero()) {
                if (diffScale <= 0) {
                    return augend;
                }
                if (augend.isZero()) {
                    return this;
                }
            } else if (augend.isZero()) {
                if (diffScale >= 0) {
                    return this;
                }
            }
            // Let be:  this = [u1,s1]  and  augend = [u2,s2]
            if (diffScale == 0) {
                // case s1 == s2: [u1 + u2 , s1]
                if (java.lang.Math.max(this.bitLengthJ, augend.bitLengthJ) + 1 < 64) {
                    return valueOf(this.smallValue + augend.smallValue, this.scaleJ);
                }
                return new BigDecimal(this.getUnscaledValue().add(augend.getUnscaledValue()), this.scaleJ);
            } else if (diffScale > 0) {
                // case s1 > s2 : [(u1 + u2) * 10 ^ (s1 - s2) , s1]
                return addAndMult10(this, augend, diffScale);
            } else {// case s2 > s1 : [(u2 + u1) * 10 ^ (s2 - s1) , s2]
                return addAndMult10(augend, this, -diffScale);
            }
        }

        private static BigDecimal addAndMult10(BigDecimal thisValue,BigDecimal augend, int diffScale) {
            if(diffScale < LONG_TEN_POW.Length &&
                    java.lang.Math.max(thisValue.bitLengthJ,augend.bitLengthJ+LONG_TEN_POW_BIT_LENGTH[diffScale])+1<64) {
                return valueOf(thisValue.smallValue+augend.smallValue*LONG_TEN_POW[diffScale],thisValue.scaleJ);
            }
            return new BigDecimal(thisValue.getUnscaledValue().add(
                    Multiplication.multiplyByTenPow(augend.getUnscaledValue(),diffScale)), thisValue.scaleJ);
        }
    
        /**
         * Returns a new {@code BigDecimal} whose value is {@code this + augend}.
         * The result is rounded according to the passed context {@code mc}.
         *
         * @param augend
         *            value to be added to {@code this}.
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @return {@code this + augend}.
         * @throws NullPointerException
         *             if {@code augend == null} or {@code mc == null}.
         */
        public BigDecimal add(BigDecimal augend, MathContext mc) {
            BigDecimal larger; // operand with the largest unscaled value
            BigDecimal smaller; // operand with the smallest unscaled value
            BigInteger tempBI;
            long diffScale = (long)this.scaleJ - augend.scaleJ;
            int largerSignum;
            // Some operand is zero or the precision is infinity  
            if ((augend.isZero()) || (this.isZero())
                    || (mc.getPrecision() == 0)) {
                return add(augend).round(mc);
            }
            // Cases where there is room for optimizations
            if (this.aproxPrecision() < diffScale - 1) {
                larger = augend;
                smaller = this;
            } else if (augend.aproxPrecision() < -diffScale - 1) {
                larger = this;
                smaller = augend;
            } else {// No optimization is done 
                return add(augend).round(mc);
            }
            if (mc.getPrecision() >= larger.aproxPrecision()) {
                // No optimization is done
                return add(augend).round(mc);
            }
            // Cases where it's unnecessary to add two numbers with very different scales 
            largerSignum = larger.signum();
            if (largerSignum == smaller.signum()) {
                tempBI = Multiplication.multiplyByPositiveInt(larger.getUnscaledValue(),10)
                .add(BigInteger.valueOf(largerSignum));
            } else {
                tempBI = larger.getUnscaledValue().subtract(
                        BigInteger.valueOf(largerSignum));
                tempBI = Multiplication.multiplyByPositiveInt(tempBI,10)
                .add(BigInteger.valueOf(largerSignum * 9));
            }
            // Rounding the improved adding 
            larger = new BigDecimal(tempBI, larger.scaleJ + 1);
            return larger.round(mc);
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this - subtrahend}.
         * The scale of the result is the maximum of the scales of the two arguments.
         *
         * @param subtrahend
         *            value to be subtracted from {@code this}.
         * @return {@code this - subtrahend}.
         * @throws NullPointerException
         *             if {@code subtrahend == null}.
         */
        public BigDecimal subtract(BigDecimal subtrahend) {
            int diffScale = this.scaleJ - subtrahend.scaleJ;
            // Fast return when some operand is zero
            if (this.isZero()) {
                if (diffScale <= 0) {
                    return subtrahend.negate();
                }
                if (subtrahend.isZero()) {
                    return this;
                }
            } else if (subtrahend.isZero()) {
                if (diffScale >= 0) {
                    return this;
                }
            }
            // Let be: this = [u1,s1] and subtrahend = [u2,s2] so:
            if (diffScale == 0) {
                // case s1 = s2 : [u1 - u2 , s1]
                if (java.lang.Math.max(this.bitLengthJ, subtrahend.bitLengthJ) + 1 < 64) {
                    return valueOf(this.smallValue - subtrahend.smallValue,this.scaleJ);
                }
                return new BigDecimal(this.getUnscaledValue().subtract(subtrahend.getUnscaledValue()), this.scaleJ);
            } else if (diffScale > 0) {
                // case s1 > s2 : [ u1 - u2 * 10 ^ (s1 - s2) , s1 ]
                if(diffScale < LONG_TEN_POW.Length &&
                        java.lang.Math.max(this.bitLengthJ,subtrahend.bitLengthJ+LONG_TEN_POW_BIT_LENGTH[diffScale])+1<64) {
                    return valueOf(this.smallValue-subtrahend.smallValue*LONG_TEN_POW[diffScale],this.scaleJ);
                }
                return new BigDecimal(this.getUnscaledValue().subtract(
                        Multiplication.multiplyByTenPow(subtrahend.getUnscaledValue(),diffScale)), this.scaleJ);
            } else {// case s2 > s1 : [ u1 * 10 ^ (s2 - s1) - u2 , s2 ]
                diffScale = -diffScale;
                if(diffScale < LONG_TEN_POW.Length &&
                        java.lang.Math.max(this.bitLengthJ+LONG_TEN_POW_BIT_LENGTH[diffScale],subtrahend.bitLengthJ)+1<64) {
                    return valueOf(this.smallValue*LONG_TEN_POW[diffScale]-subtrahend.smallValue,subtrahend.scaleJ);
                }
                return new BigDecimal(Multiplication.multiplyByTenPow(this.getUnscaledValue(),diffScale)
                .subtract(subtrahend.getUnscaledValue()), subtrahend.scaleJ);
            }
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this - subtrahend}.
         * The result is rounded according to the passed context {@code mc}.
         *
         * @param subtrahend
         *            value to be subtracted from {@code this}.
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @return {@code this - subtrahend}.
         * @throws NullPointerException
         *             if {@code subtrahend == null} or {@code mc == null}.
         */
        public BigDecimal subtract(BigDecimal subtrahend, MathContext mc) {
            long diffScale = subtrahend.scaleJ - (long)this.scaleJ;
            int thisSignum;
            BigDecimal leftOperand; // it will be only the left operand (this) 
            BigInteger tempBI;
            // Some operand is zero or the precision is infinity  
            if ((subtrahend.isZero()) || (this.isZero())
                    || (mc.getPrecision() == 0)) {
                return subtract(subtrahend).round(mc);
            }
            // Now:   this != 0   and   subtrahend != 0
            if (subtrahend.aproxPrecision() < diffScale - 1) {
                // Cases where it is unnecessary to subtract two numbers with very different scales
                if (mc.getPrecision() < this.aproxPrecision()) {
                    thisSignum = this.signum();
                    if (thisSignum != subtrahend.signum()) {
                        tempBI = Multiplication.multiplyByPositiveInt(this.getUnscaledValue(), 10)
                        .add(BigInteger.valueOf(thisSignum));
                    } else {
                        tempBI = this.getUnscaledValue().subtract(BigInteger.valueOf(thisSignum));
                        tempBI = Multiplication.multiplyByPositiveInt(tempBI, 10)
                        .add(BigInteger.valueOf(thisSignum * 9));
                    }
                    // Rounding the improved subtracting
                    leftOperand = new BigDecimal(tempBI, this.scaleJ + 1);
                    return leftOperand.round(mc);
                }
            }
            // No optimization is done
            return subtract(subtrahend).round(mc);
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this *
         * multiplicand}. The scale of the result is the sum of the scales of the
         * two arguments.
         *
         * @param multiplicand
         *            value to be multiplied with {@code this}.
         * @return {@code this * multiplicand}.
         * @throws NullPointerException
         *             if {@code multiplicand == null}.
         */
        public BigDecimal multiply(BigDecimal multiplicand) {
            long newScale = (long)this.scaleJ + multiplicand.scaleJ;

            if ((this.isZero()) || (multiplicand.isZero())) {
                return zeroScaledBy(newScale);
            }
            /* Let be: this = [u1,s1] and multiplicand = [u2,s2] so:
             * this x multiplicand = [ s1 * s2 , s1 + s2 ] */
            if(this.bitLengthJ + multiplicand.bitLengthJ < 64) {
                return valueOf(this.smallValue*multiplicand.smallValue,toIntScale(newScale));
            }
            return new BigDecimal(this.getUnscaledValue().multiply(
                    multiplicand.getUnscaledValue()), toIntScale(newScale));
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this *
         * multiplicand}. The result is rounded according to the passed context
         * {@code mc}.
         *
         * @param multiplicand
         *            value to be multiplied with {@code this}.
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @return {@code this * multiplicand}.
         * @throws NullPointerException
         *             if {@code multiplicand == null} or {@code mc == null}.
         */
        public BigDecimal multiply(BigDecimal multiplicand, MathContext mc) {
            BigDecimal result = multiply(multiplicand);

            result.inplaceRound(mc);
            return result;
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this / divisor}.
         * As scale of the result the parameter {@code scale} is used. If rounding
         * is required to meet the specified scale, then the specified rounding mode
         * {@code roundingMode} is applied.
         *
         * @param divisor
         *            value by which {@code this} is divided.
         * @param scale
         *            the scale of the result returned.
         * @param roundingMode
         *            rounding mode to be used to round the result.
         * @return {@code this / divisor} rounded according to the given rounding
         *         mode.
         * @throws NullPointerException
         *             if {@code divisor == null}.
         * @throws IllegalArgumentException
         *             if {@code roundingMode} is not a valid rounding mode.
         * @throws ArithmeticException
         *             if {@code divisor == 0}.
         * @throws ArithmeticException
         *             if {@code roundingMode == ROUND_UNNECESSARY} and rounding is
         *             necessary according to the given scale.
         */
        public BigDecimal divide(BigDecimal divisor, int scale, int roundingMode) {
            return divide(divisor, scale, RoundingMode.valueOf(roundingMode));
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this / divisor}.
         * As scale of the result the parameter {@code scale} is used. If rounding
         * is required to meet the specified scale, then the specified rounding mode
         * {@code roundingMode} is applied.
         *
         * @param divisor
         *            value by which {@code this} is divided.
         * @param scale
         *            the scale of the result returned.
         * @param roundingMode
         *            rounding mode to be used to round the result.
         * @return {@code this / divisor} rounded according to the given rounding
         *         mode.
         * @throws NullPointerException
         *             if {@code divisor == null} or {@code roundingMode == null}.
         * @throws ArithmeticException
         *             if {@code divisor == 0}.
         * @throws ArithmeticException
         *             if {@code roundingMode == RoundingMode.UNNECESSAR}Y and
         *             rounding is necessary according to the given scale and given
         *             precision.
         */
        public BigDecimal divide(BigDecimal divisor, int scale, RoundingMode roundingMode) {
            // Let be: this = [u1,s1]  and  divisor = [u2,s2]
            if (roundingMode == null) {
                throw new java.lang.NullPointerException();
            }
            if (divisor.isZero()) {
                // math.04=Division by zero
                throw new java.lang.ArithmeticException("Division by zero"); //$NON-NLS-1$
            }
        
            long diffScale = ((long)this.scaleJ - divisor.scaleJ) - scale;
            if(this.bitLengthJ < 64 && divisor.bitLengthJ < 64 ) {
                if(diffScale == 0) {
                    return dividePrimitiveLongs(this.smallValue,
                            divisor.smallValue,
                            scale,
                            roundingMode );
                } else if(diffScale > 0) {
                    if(diffScale < LONG_TEN_POW.Length &&
                            divisor.bitLengthJ + LONG_TEN_POW_BIT_LENGTH[(int)diffScale] < 64) {
                        return dividePrimitiveLongs(this.smallValue,
                                divisor.smallValue*LONG_TEN_POW[(int)diffScale],
                                scale,
                                roundingMode);
                    }
                } else { // diffScale < 0
                    if(-diffScale < LONG_TEN_POW.Length &&
                            this.bitLengthJ + LONG_TEN_POW_BIT_LENGTH[(int)-diffScale] < 64) {
                        return dividePrimitiveLongs(this.smallValue*LONG_TEN_POW[(int)-diffScale],
                                divisor.smallValue,
                                scale,
                                roundingMode);
                    }
                
                }
            }
            BigInteger scaledDividend = this.getUnscaledValue();
            BigInteger scaledDivisor = divisor.getUnscaledValue(); // for scaling of 'u2'
        
            if (diffScale > 0) {
                // Multiply 'u2'  by:  10^((s1 - s2) - scale)
                scaledDivisor = Multiplication.multiplyByTenPow(scaledDivisor, (int)diffScale);
            } else if (diffScale < 0) {
                // Multiply 'u1'  by:  10^(scale - (s1 - s2))
                scaledDividend  = Multiplication.multiplyByTenPow(scaledDividend, (int)-diffScale);
            }
            return divideBigIntegers(scaledDividend, scaledDivisor, scale, roundingMode);
            }
    
        private static BigDecimal divideBigIntegers(BigInteger scaledDividend, BigInteger scaledDivisor, int scale, RoundingMode roundingMode) {
        
            BigInteger[] quotAndRem = scaledDividend.divideAndRemainder(scaledDivisor);  // quotient and remainder
            // If after division there is a remainder...
            BigInteger quotient = quotAndRem[0];
            BigInteger remainder = quotAndRem[1];
            if (remainder.signum() == 0) {
                return new BigDecimal(quotient, scale);
            }
            int sign = scaledDividend.signum() * scaledDivisor.signum();
            int compRem;                                      // 'compare to remainder'
            if(scaledDivisor.bitLength() < 63) { // 63 in order to avoid out of long after <<1
                long rem = remainder.longValue();
                long divisor = scaledDivisor.longValue();
                compRem = longCompareTo(java.lang.Math.abs(rem) << 1,java.lang.Math.abs(divisor));
                // To look if there is a carry
                compRem = roundingBehavior(quotient.testBit(0) ? 1 : 0,
                        sign * (5 + compRem), roundingMode);
            
            } else {
                // Checking if:  remainder * 2 >= scaledDivisor 
                compRem = remainder.abs().shiftLeftOneBit().compareTo(scaledDivisor.abs());
                compRem = roundingBehavior(quotient.testBit(0) ? 1 : 0,
                        sign * (5 + compRem), roundingMode);
            }
                if (compRem != 0) {
                if(quotient.bitLength() < 63) {
                    return valueOf(quotient.longValue() + compRem,scale);
                }
                quotient = quotient.add(BigInteger.valueOf(compRem));
                return new BigDecimal(quotient, scale);
            }
            // Constructing the result with the appropriate unscaled value
            return new BigDecimal(quotient, scale);
        }
    
        private static BigDecimal dividePrimitiveLongs(long scaledDividend, long scaledDivisor, int scale, RoundingMode roundingMode) {
            long quotient = scaledDividend / scaledDivisor;
            long remainder = scaledDividend % scaledDivisor;
            int sign = java.lang.Long.signum( scaledDividend ) * java.lang.Long.signum( scaledDivisor );
            if (remainder != 0) {
                // Checking if:  remainder * 2 >= scaledDivisor
                int compRem;                                      // 'compare to remainder'
                compRem = longCompareTo(java.lang.Math.abs(remainder) << 1,java.lang.Math.abs(scaledDivisor));
                // To look if there is a carry
                quotient += roundingBehavior(((int)quotient) & 1,
                        sign * (5 + compRem),
                        roundingMode);
            }
            // Constructing the result with the appropriate unscaled value
            return valueOf(quotient, scale);
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this / divisor}.
         * The scale of the result is the scale of {@code this}. If rounding is
         * required to meet the specified scale, then the specified rounding mode
         * {@code roundingMode} is applied.
         *
         * @param divisor
         *            value by which {@code this} is divided.
         * @param roundingMode
         *            rounding mode to be used to round the result.
         * @return {@code this / divisor} rounded according to the given rounding
         *         mode.
         * @throws NullPointerException
         *             if {@code divisor == null}.
         * @throws IllegalArgumentException
         *             if {@code roundingMode} is not a valid rounding mode.
         * @throws ArithmeticException
         *             if {@code divisor == 0}.
         * @throws ArithmeticException
         *             if {@code roundingMode == ROUND_UNNECESSARY} and rounding is
         *             necessary according to the scale of this.
         */
        public BigDecimal divide(BigDecimal divisor, int roundingMode) {
            return divide(divisor, scaleJ, RoundingMode.valueOf(roundingMode));
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this / divisor}.
         * The scale of the result is the scale of {@code this}. If rounding is
         * required to meet the specified scale, then the specified rounding mode
         * {@code roundingMode} is applied.
         *
         * @param divisor
         *            value by which {@code this} is divided.
         * @param roundingMode
         *            rounding mode to be used to round the result.
         * @return {@code this / divisor} rounded according to the given rounding
         *         mode.
         * @throws NullPointerException
         *             if {@code divisor == null} or {@code roundingMode == null}.
         * @throws ArithmeticException
         *             if {@code divisor == 0}.
         * @throws ArithmeticException
         *             if {@code roundingMode == RoundingMode.UNNECESSARY} and
         *             rounding is necessary according to the scale of this.
         */
        public BigDecimal divide(BigDecimal divisor, RoundingMode roundingMode) {
            return divide(divisor, scaleJ, roundingMode);
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this / divisor}.
         * The scale of the result is the difference of the scales of {@code this}
         * and {@code divisor}. If the exact result requires more digits, then the
         * scale is adjusted accordingly. For example, {@code 1/128 = 0.0078125}
         * which has a scale of {@code 7} and precision {@code 5}.
         *
         * @param divisor
         *            value by which {@code this} is divided.
         * @return {@code this / divisor}.
         * @throws NullPointerException
         *             if {@code divisor == null}.
         * @throws ArithmeticException
         *             if {@code divisor == 0}.
         * @throws ArithmeticException
         *             if the result cannot be represented exactly.
         */
        public BigDecimal divide(BigDecimal divisor) {
            BigInteger p = this.getUnscaledValue();
            BigInteger q = divisor.getUnscaledValue();
            BigInteger gcd; // greatest common divisor between 'p' and 'q'
            BigInteger[] quotAndRem;
            long diffScale = (long)scaleJ - divisor.scaleJ;
            int newScale; // the new scale for final quotient
            int k; // number of factors "2" in 'q'
            int l = 0; // number of factors "5" in 'q'
            int i = 1;
            int lastPow = FIVE_POW.Length - 1;

            if (divisor.isZero()) {
                // math.04=Division by zero
                throw new ArithmeticException("Division by zero"); //$NON-NLS-1$
            }
            if (p.signum() == 0) {
                return zeroScaledBy(diffScale);
            }
            // To divide both by the GCD
            gcd = p.gcd(q);
            p = p.divide(gcd);
            q = q.divide(gcd);
            // To simplify all "2" factors of q, dividing by 2^k
            k = q.getLowestSetBit();
            q = q.shiftRight(k);
            // To simplify all "5" factors of q, dividing by 5^l
            do {
                quotAndRem = q.divideAndRemainder(FIVE_POW[i]);
                if (quotAndRem[1].signum() == 0) {
                    l += i;
                    if (i < lastPow) {
                        i++;
                    }
                    q = quotAndRem[0];
                } else {
                    if (i == 1) {
                        break;
                    }
                    i = 1;
                }
            } while (true);
            // If  abs(q) != 1  then the quotient is periodic
            if (!q.abs().equals(BigInteger.ONE)) {
                // math.05=Non-terminating decimal expansion; no exact representable decimal result.
                throw new ArithmeticException("Non-terminating decimal expansion; no exact representable decimal result."); //$NON-NLS-1$
            }
            // The sign of the is fixed and the quotient will be saved in 'p'
            if (q.signum() < 0) {
                p = p.negate();
            }
            // Checking if the new scale is out of range
            newScale = toIntScale(diffScale + java.lang.Math.max(k, l));
            // k >= 0  and  l >= 0  implies that  k - l  is in the 32-bit range
            i = k - l;
        
            p = (i > 0) ? Multiplication.multiplyByFivePow(p, i)
            : p.shiftLeft(-i);
            return new BigDecimal(p, newScale);
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this / divisor}.
         * The result is rounded according to the passed context {@code mc}. If the
         * passed math context specifies precision {@code 0}, then this call is
         * equivalent to {@code this.divide(divisor)}.
         *
         * @param divisor
         *            value by which {@code this} is divided.
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @return {@code this / divisor}.
         * @throws NullPointerException
         *             if {@code divisor == null} or {@code mc == null}.
         * @throws ArithmeticException
         *             if {@code divisor == 0}.
         * @throws ArithmeticException
         *             if {@code mc.getRoundingMode() == UNNECESSARY} and rounding
         *             is necessary according {@code mc.getPrecision()}.
         */
        public BigDecimal divide(BigDecimal divisor, MathContext mc) {
            /* Calculating how many zeros must be append to 'dividend'
             * to obtain a  quotient with at least 'mc.precision()' digits */
            long traillingZeros = mc.getPrecision() + 2L
                    + divisor.aproxPrecision() - aproxPrecision();
            long diffScale = (long)scaleJ - divisor.scaleJ;
            long newScale = diffScale; // scale of the final quotient
            int compRem; // to compare the remainder
            int i = 1; // index   
            int lastPow = TEN_POW.Length - 1; // last power of ten
            BigInteger integerQuot; // for temporal results
            BigInteger []quotAndRem = {getUnscaledValue()};
            // In special cases it reduces the problem to call the dual method
            if ((mc.getPrecision() == 0) || (this.isZero())
            || (divisor.isZero())) {
                return this.divide(divisor);
            }
            if (traillingZeros > 0) {
                // To append trailing zeros at end of dividend
                quotAndRem[0] = getUnscaledValue().multiply( Multiplication.powerOf10(traillingZeros) );
                newScale += traillingZeros;
            }
            quotAndRem = quotAndRem[0].divideAndRemainder( divisor.getUnscaledValue() );
            integerQuot = quotAndRem[0];
            // Calculating the exact quotient with at least 'mc.precision()' digits
            if (quotAndRem[1].signum() != 0) {
                // Checking if:   2 * remainder >= divisor ?
                compRem = quotAndRem[1].shiftLeftOneBit().compareTo( divisor.getUnscaledValue() );
                // quot := quot * 10 + r;     with 'r' in {-6,-5,-4, 0,+4,+5,+6}
                integerQuot = integerQuot.multiply(BigInteger.TEN)
                .add(BigInteger.valueOf(quotAndRem[0].signum() * (5 + compRem)));
                newScale++;
            } else {
                // To strip trailing zeros until the preferred scale is reached
                while (!integerQuot.testBit(0)) {
                    quotAndRem = integerQuot.divideAndRemainder(TEN_POW[i]);
                    if ((quotAndRem[1].signum() == 0)
                            && (newScale - i >= diffScale)) {
                        newScale -= i;
                        if (i < lastPow) {
                            i++;
                        }
                        integerQuot = quotAndRem[0];
                    } else {
                        if (i == 1) {
                            break;
                        }
                        i = 1;
                    }
                }
            }
            // To perform rounding
            return new BigDecimal(integerQuot, toIntScale(newScale), mc);
        }

        /**
         * Returns a new {@code BigDecimal} whose value is the integral part of
         * {@code this / divisor}. The quotient is rounded down towards zero to the
         * next integer. For example, {@code 0.5/0.2 = 2}.
         *
         * @param divisor
         *            value by which {@code this} is divided.
         * @return integral part of {@code this / divisor}.
         * @throws NullPointerException
         *             if {@code divisor == null}.
         * @throws ArithmeticException
         *             if {@code divisor == 0}.
         */
        public BigDecimal divideToIntegralValue(BigDecimal divisor) {
            BigInteger integralValue; // the integer of result
            BigInteger powerOfTen; // some power of ten
            BigInteger[] quotAndRem = {getUnscaledValue()};
            long newScale = (long)this.scaleJ - divisor.scaleJ;
            long tempScale = 0;
            int i = 1;
            int lastPow = TEN_POW.Length - 1;

            if (divisor.isZero()) {
                // math.04=Division by zero
                throw new ArithmeticException("Division by zero"); //$NON-NLS-1$
            }
            if ((divisor.aproxPrecision() + newScale > this.aproxPrecision() + 1L)
            || (this.isZero())) {
                /* If the divisor's integer part is greater than this's integer part,
                 * the result must be zero with the appropriate scale */
                integralValue = BigInteger.ZERO;
            } else if (newScale == 0) {
                integralValue = getUnscaledValue().divide( divisor.getUnscaledValue() );
            } else if (newScale > 0) {
                powerOfTen = Multiplication.powerOf10(newScale);
                integralValue = getUnscaledValue().divide( divisor.getUnscaledValue().multiply(powerOfTen) );
                integralValue = integralValue.multiply(powerOfTen);
            } else {// (newScale < 0)
                powerOfTen = Multiplication.powerOf10(-newScale);
                integralValue = getUnscaledValue().multiply(powerOfTen).divide( divisor.getUnscaledValue() );
                // To strip trailing zeros approximating to the preferred scale
                while (!integralValue.testBit(0)) {
                    quotAndRem = integralValue.divideAndRemainder(TEN_POW[i]);
                    if ((quotAndRem[1].signum() == 0)
                            && (tempScale - i >= newScale)) {
                        tempScale -= i;
                        if (i < lastPow) {
                            i++;
                        }
                        integralValue = quotAndRem[0];
                    } else {
                        if (i == 1) {
                            break;
                        }
                        i = 1;
                    }
                }
                newScale = tempScale;
            }
            return ((integralValue.signum() == 0)
            ? zeroScaledBy(newScale)
                    : new BigDecimal(integralValue, toIntScale(newScale)));
        }

        /**
         * Returns a new {@code BigDecimal} whose value is the integral part of
         * {@code this / divisor}. The quotient is rounded down towards zero to the
         * next integer. The rounding mode passed with the parameter {@code mc} is
         * not considered. But if the precision of {@code mc > 0} and the integral
         * part requires more digits, then an {@code ArithmeticException} is thrown.
         *
         * @param divisor
         *            value by which {@code this} is divided.
         * @param mc
         *            math context which determines the maximal precision of the
         *            result.
         * @return integral part of {@code this / divisor}.
         * @throws NullPointerException
         *             if {@code divisor == null} or {@code mc == null}.
         * @throws ArithmeticException
         *             if {@code divisor == 0}.
         * @throws ArithmeticException
         *             if {@code mc.getPrecision() > 0} and the result requires more
         *             digits to be represented.
         */
        public BigDecimal divideToIntegralValue(BigDecimal divisor, MathContext mc) {
            int mcPrecision = mc.getPrecision();
            int diffPrecision = this.precision() - divisor.precision();
            int lastPow = TEN_POW.Length - 1;
            long diffScale = (long)this.scaleJ - divisor.scaleJ;
            long newScale = diffScale;
            long quotPrecision = diffPrecision - diffScale + 1;
            BigInteger []quotAndRem = new BigInteger[2];
            // In special cases it call the dual method
            if ((mcPrecision == 0) || (this.isZero()) || (divisor.isZero())) {
                return this.divideToIntegralValue(divisor);
            }
            // Let be:   this = [u1,s1]   and   divisor = [u2,s2]
            if (quotPrecision <= 0) {
                quotAndRem[0] = BigInteger.ZERO;
            } else if (diffScale == 0) {
                // CASE s1 == s2:  to calculate   u1 / u2 
                quotAndRem[0] = this.getUnscaledValue().divide( divisor.getUnscaledValue() );
            } else if (diffScale > 0) {
                // CASE s1 >= s2:  to calculate   u1 / (u2 * 10^(s1-s2)  
                quotAndRem[0] = this.getUnscaledValue().divide(
                        divisor.getUnscaledValue().multiply(Multiplication.powerOf10(diffScale)) );
                // To chose  10^newScale  to get a quotient with at least 'mc.precision()' digits
                newScale = java.lang.Math.min(diffScale, java.lang.Math.max(mcPrecision - quotPrecision + 1, 0));
                // To calculate: (u1 / (u2 * 10^(s1-s2)) * 10^newScale
                quotAndRem[0] = quotAndRem[0].multiply(Multiplication.powerOf10(newScale));
            } else {// CASE s2 > s1:   
                /* To calculate the minimum power of ten, such that the quotient 
                 *   (u1 * 10^exp) / u2   has at least 'mc.precision()' digits. */
                long exp = java.lang.Math.min(-diffScale, java.lang.Math.max((long)mcPrecision - diffPrecision, 0));
                long compRemDiv;
                // Let be:   (u1 * 10^exp) / u2 = [q,r]  
                quotAndRem = this.getUnscaledValue().multiply(Multiplication.powerOf10(exp)).
                        divideAndRemainder(divisor.getUnscaledValue());
                newScale += exp; // To fix the scale
                exp = -newScale; // The remaining power of ten
                // If after division there is a remainder...
                if ((quotAndRem[1].signum() != 0) && (exp > 0)) {
                    // Log10(r) + ((s2 - s1) - exp) > mc.precision ?
                    compRemDiv = (new BigDecimal(quotAndRem[1])).precision()
                    + exp - divisor.precision();
                    if (compRemDiv == 0) {
                        // To calculate:  (r * 10^exp2) / u2
                        quotAndRem[1] = quotAndRem[1].multiply(Multiplication.powerOf10(exp)).
                                divide(divisor.getUnscaledValue());
                        compRemDiv = java.lang.Math.abs(quotAndRem[1].signum());
                    }
                    if (compRemDiv > 0) {
                        // The quotient won't fit in 'mc.precision()' digits
                        // math.06=Division impossible
                        throw new java.lang.ArithmeticException("Division impossible"); //$NON-NLS-1$
                    }
                }
            }
            // Fast return if the quotient is zero
            if (quotAndRem[0].signum() == 0) {
                return zeroScaledBy(diffScale);
            }
            BigInteger strippedBI = quotAndRem[0];
            BigDecimal integralValue = new BigDecimal(quotAndRem[0]);
            long resultPrecision = integralValue.precision();
            int i = 1;
            // To strip trailing zeros until the specified precision is reached
            while (!strippedBI.testBit(0)) {
                quotAndRem = strippedBI.divideAndRemainder(TEN_POW[i]);
                if ((quotAndRem[1].signum() == 0) &&
                        ((resultPrecision - i >= mcPrecision)
                        || (newScale - i >= diffScale)) ) {
                    resultPrecision -= i;
                    newScale -= i;
                    if (i < lastPow) {
                        i++;
                    }
                    strippedBI = quotAndRem[0];
                } else {
                    if (i == 1) {
                        break;
                    }
                    i = 1;
                }
            }
            // To check if the result fit in 'mc.precision()' digits
            if (resultPrecision > mcPrecision) {
                // math.06=Division impossible
                throw new java.lang.ArithmeticException("Division impossible"); //$NON-NLS-1$
            }
            integralValue.scaleJ = toIntScale(newScale);
            integralValue.setUnscaledValue(strippedBI);
            return integralValue;
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this % divisor}.
         * <p>
         * The remainder is defined as {@code this -
         * this.divideToIntegralValue(divisor) * divisor}.
         *
         * @param divisor
         *            value by which {@code this} is divided.
         * @return {@code this % divisor}.
         * @throws NullPointerException
         *             if {@code divisor == null}.
         * @throws ArithmeticException
         *             if {@code divisor == 0}.
         */
        public BigDecimal remainder(BigDecimal divisor) {
            return divideAndRemainder(divisor)[1];
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this % divisor}.
         * <p>
         * The remainder is defined as {@code this -
         * this.divideToIntegralValue(divisor) * divisor}.
         * <p>
         * The specified rounding mode {@code mc} is used for the division only.
         *
         * @param divisor
         *            value by which {@code this} is divided.
         * @param mc
         *            rounding mode and precision to be used.
         * @return {@code this % divisor}.
         * @throws NullPointerException
         *             if {@code divisor == null}.
         * @throws ArithmeticException
         *             if {@code divisor == 0}.
         * @throws ArithmeticException
         *             if {@code mc.getPrecision() > 0} and the result of {@code
         *             this.divideToIntegralValue(divisor, mc)} requires more digits
         *             to be represented.
         */
        public BigDecimal remainder(BigDecimal divisor, MathContext mc) {
            return divideAndRemainder(divisor, mc)[1];
        }

        /**
         * Returns a {@code BigDecimal} array which contains the integral part of
         * {@code this / divisor} at index 0 and the remainder {@code this %
         * divisor} at index 1. The quotient is rounded down towards zero to the
         * next integer.
         *
         * @param divisor
         *            value by which {@code this} is divided.
         * @return {@code [this.divideToIntegralValue(divisor),
         *         this.remainder(divisor)]}.
         * @throws NullPointerException
         *             if {@code divisor == null}.
         * @throws ArithmeticException
         *             if {@code divisor == 0}.
         * @see #divideToIntegralValue
         * @see #remainder
         */
        public BigDecimal[] divideAndRemainder(BigDecimal divisor) {
            BigDecimal[] quotAndRem = new BigDecimal[2];

            quotAndRem[0] = this.divideToIntegralValue(divisor);
            quotAndRem[1] = this.subtract( quotAndRem[0].multiply(divisor) );
            return quotAndRem;
        }

        /**
         * Returns a {@code BigDecimal} array which contains the integral part of
         * {@code this / divisor} at index 0 and the remainder {@code this %
         * divisor} at index 1. The quotient is rounded down towards zero to the
         * next integer. The rounding mode passed with the parameter {@code mc} is
         * not considered. But if the precision of {@code mc > 0} and the integral
         * part requires more digits, then an {@code ArithmeticException} is thrown.
         *
         * @param divisor
         *            value by which {@code this} is divided.
         * @param mc
         *            math context which determines the maximal precision of the
         *            result.
         * @return {@code [this.divideToIntegralValue(divisor),
         *         this.remainder(divisor)]}.
         * @throws NullPointerException
         *             if {@code divisor == null}.
         * @throws ArithmeticException
         *             if {@code divisor == 0}.
         * @see #divideToIntegralValue
         * @see #remainder
         */
        public BigDecimal[] divideAndRemainder(BigDecimal divisor, MathContext mc) {
            BigDecimal[] quotAndRem = new BigDecimal[2];

            quotAndRem[0] = this.divideToIntegralValue(divisor, mc);
            quotAndRem[1] = this.subtract( quotAndRem[0].multiply(divisor) );
            return quotAndRem;
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this ^ n}. The
         * scale of the result is {@code n} times the scales of {@code this}.
         * <p>
         * {@code x.pow(0)} returns {@code 1}, even if {@code x == 0}.
         * <p>
         * Implementation Note: The implementation is based on the ANSI standard
         * X3.274-1996 algorithm.
         *
         * @param n
         *            exponent to which {@code this} is raised.
         * @return {@code this ^ n}.
         * @throws ArithmeticException
         *             if {@code n < 0} or {@code n > 999999999}.
         */
        public BigDecimal pow(int n) {
            if (n == 0) {
                return ONE;
            }
            if ((n < 0) || (n > 999999999)) {
                // math.07=Invalid Operation
                throw new ArithmeticException("Invalid Operation"); //$NON-NLS-1$
            }
            long newScale = this.scaleJ * (long)n;
            // Let be: this = [u,s]   so:  this^n = [u^n, s*n]
            return ((isZero())
            ? zeroScaledBy(newScale)
            : new BigDecimal(getUnscaledValue().pow(n), toIntScale(newScale)));
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this ^ n}. The
         * result is rounded according to the passed context {@code mc}.
         * <p>
         * Implementation Note: The implementation is based on the ANSI standard
         * X3.274-1996 algorithm.
         *
         * @param n
         *            exponent to which {@code this} is raised.
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @return {@code this ^ n}.
         * @throws ArithmeticException
         *             if {@code n < 0} or {@code n > 999999999}.
         */
        public BigDecimal pow(int n, MathContext mc) {
            // The ANSI standard X3.274-1996 algorithm
            int m = java.lang.Math.abs(n);
            int mcPrecision = mc.getPrecision();
            int elength = (int)java.lang.Math.log10(m) + 1;   // decimal digits in 'n'
            int oneBitMask; // mask of bits
            BigDecimal accum; // the single accumulator
            MathContext newPrecision = mc; // MathContext by default

            // In particular cases, it reduces the problem to call the other 'pow()'
            if ((n == 0) || ((isZero()) && (n > 0))) {
                return pow(n);
            }
            if ((m > 999999999) || ((mcPrecision == 0) && (n < 0))
                    || ((mcPrecision > 0) && (elength > mcPrecision))) {
                // math.07=Invalid Operation
                throw new ArithmeticException("Invalid Operation"); //$NON-NLS-1$
            }
            if (mcPrecision > 0) {
                newPrecision = new MathContext( mcPrecision + elength + 1,
                        mc.getRoundingMode());
            }
            // The result is calculated as if 'n' were positive        
            accum = round(newPrecision);
            oneBitMask = java.lang.Integer.highestOneBit(m) >> 1;

            while (oneBitMask > 0) {
                accum = accum.multiply(accum, newPrecision);
                if ((m & oneBitMask) == oneBitMask) {
                    accum = accum.multiply(this, newPrecision);
                }
                oneBitMask >>= 1;
            }
            // If 'n' is negative, the value is divided into 'ONE'
            if (n < 0) {
                accum = ONE.divide(accum, newPrecision);
            }
            // The final value is rounded to the destination precision
            accum.inplaceRound(mc);
            return accum;
        }

        /**
         * Returns a new {@code BigDecimal} whose value is the absolute value of
         * {@code this}. The scale of the result is the same as the scale of this.
         *
         * @return {@code abs(this)}
         */
        public BigDecimal abs() {
            return ((signum() < 0) ? negate() : this);
        }

        /**
         * Returns a new {@code BigDecimal} whose value is the absolute value of
         * {@code this}. The result is rounded according to the passed context
         * {@code mc}.
         *
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @return {@code abs(this)}
         */
        public BigDecimal abs(MathContext mc) {
            return round(mc).abs();
        }

        /**
         * Returns a new {@code BigDecimal} whose value is the {@code -this}. The
         * scale of the result is the same as the scale of this.
         *
         * @return {@code -this}
         */
        public BigDecimal negate() {
            if(bitLengthJ < 63 || (bitLengthJ == 63 && smallValue!=java.lang.Long.MIN_VALUE)) {
                return valueOf(-smallValue,scaleJ);
            }
            return new BigDecimal(getUnscaledValue().negate(), scaleJ);
        }

        /**
         * Returns a new {@code BigDecimal} whose value is the {@code -this}. The
         * result is rounded according to the passed context {@code mc}.
         *
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @return {@code -this}
         */
        public BigDecimal negate(MathContext mc) {
            return round(mc).negate();
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code +this}. The scale
         * of the result is the same as the scale of this.
         *
         * @return {@code this}
         */
        public BigDecimal plus() {
            return this;
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code +this}. The result
         * is rounded according to the passed context {@code mc}.
         *
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @return {@code this}, rounded
         */
        public BigDecimal plus(MathContext mc) {
            return round(mc);
        }

        /**
         * Returns the sign of this {@code BigDecimal}.
         *
         * @return {@code -1} if {@code this < 0},
         *         {@code 0} if {@code this == 0},
         *         {@code 1} if {@code this > 0}.     */
        public virtual int signum() {
            if( bitLengthJ < 64) {
                return java.lang.Long.signum( this.smallValue );
            }
            return getUnscaledValue().signum();
        }
    
        private bool isZero() {
            //Watch out: -1 has a bitLengthJ=0
            return bitLengthJ == 0 && this.smallValue != -1;
        }

        /**
         * Returns the scale of this {@code BigDecimal}. The scale is the number of
         * digits behind the decimal point. The value of this {@code BigDecimal} is
         * the unsignedValue * 10^(-scale). If the scale is negative, then this
         * {@code BigDecimal} represents a big integer.
         *
         * @return the scale of this {@code BigDecimal}.
         */
        public virtual int scale() {
            return scaleJ;
        }

        /**
         * Returns the precision of this {@code BigDecimal}. The precision is the
         * number of decimal digits used to represent this decimal. It is equivalent
         * to the number of digits of the unscaled value. The precision of {@code 0}
         * is {@code 1} (independent of the scale).
         *
         * @return the precision of this {@code BigDecimal}.
         */
        public int precision() {
            // Checking if the precision already was calculated
            if (precisionJ > 0) {
                return precisionJ;
            }
            int bitLengthJ = this.bitLengthJ;
            int decimalDigits = 1; // the precision to be calculated
            double doubleUnsc = 1;  // intVal in 'double'

            if (bitLengthJ < 1024) {
                // To calculate the precision for small numbers
                if (bitLengthJ >= 64) {
                    doubleUnsc = getUnscaledValue().doubleValue();
                } else if (bitLengthJ >= 1) {
                    doubleUnsc = smallValue;
                }
                decimalDigits = (int) (decimalDigits+ java.lang.Math.log10(java.lang.Math.abs(doubleUnsc)));
            } else {// (bitLengthJ >= 1024)
                /* To calculate the precision for large numbers
                 * Note that: 2 ^(bitLengthJ() - 1) <= intVal < 10 ^(precision()) */
                decimalDigits = (int)(decimalDigits+ ((bitLengthJ - 1) * LOG10_2));
                // If after division the number isn't zero, exists an aditional digit
                if (getUnscaledValue().divide(Multiplication.powerOf10(decimalDigits)).signum() != 0) {
                    decimalDigits++;
                }
            }
            precisionJ = decimalDigits;
            return precisionJ;
        }

        /**
         * Returns the unscaled value (mantissa) of this {@code BigDecimal} instance
         * as a {@code BigInteger}. The unscaled value can be computed as {@code
         * this} 10^(scale).
         *
         * @return unscaled value (this * 10^(scale)).
         */
        public BigInteger unscaledValue() {
            return getUnscaledValue();
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this}, rounded
         * according to the passed context {@code mc}.
         * <p>
         * If {@code mc.precision = 0}, then no rounding is performed.
         * <p>
         * If {@code mc.precision > 0} and {@code mc.roundingMode == UNNECESSARY},
         * then an {@code ArithmeticException} is thrown if the result cannot be
         * represented exactly within the given precision.
         *
         * @param mc
         *            rounding mode and precision for the result of this operation.
         * @return {@code this} rounded according to the passed context.
         * @throws ArithmeticException
         *             if {@code mc.precision > 0} and {@code mc.roundingMode ==
         *             UNNECESSARY} and this cannot be represented within the given
         *             precision.
         */
        public BigDecimal round(MathContext mc) {
            BigDecimal thisBD = new BigDecimal(getUnscaledValue(), scaleJ);

            thisBD.inplaceRound(mc);
            return thisBD;
        }

        /**
         * Returns a new {@code BigDecimal} instance with the specified scale.
         * <p>
         * If the new scale is greater than the old scale, then additional zeros are
         * added to the unscaled value. In this case no rounding is necessary.
         * <p>
         * If the new scale is smaller than the old scale, then trailing digits are
         * removed. If these trailing digits are not zero, then the remaining
         * unscaled value has to be rounded. For this rounding operation the
         * specified rounding mode is used.
         *
         * @param newScale
         *            scale of the result returned.
         * @param roundingMode
         *            rounding mode to be used to round the result.
         * @return a new {@code BigDecimal} instance with the specified scale.
         * @throws NullPointerException
         *             if {@code roundingMode == null}.
         * @throws ArithmeticException
         *             if {@code roundingMode == ROUND_UNNECESSARY} and rounding is
         *             necessary according to the given scale.
         */
        public BigDecimal setScale(int newScale, RoundingMode roundingMode) {
            if (roundingMode == null) {
                throw new java.lang.NullPointerException();
            }
            long diffScale = newScale - (long)scaleJ;
            // Let be:  'this' = [u,s]        
            if(diffScale == 0) {
                return this;
            }
            if(diffScale > 0) {
            // return  [u * 10^(s2 - s), newScale]
                if(diffScale < LONG_TEN_POW.Length &&
                        (this.bitLengthJ + LONG_TEN_POW_BIT_LENGTH[(int)diffScale]) < 64 ) {
                    return valueOf(this.smallValue*LONG_TEN_POW[(int)diffScale],newScale);
                }
                return new BigDecimal(Multiplication.multiplyByTenPow(getUnscaledValue(),(int)diffScale), newScale);
            }
            // diffScale < 0
            // return  [u,s] / [1,newScale]  with the appropriate scale and rounding
            if(this.bitLengthJ < 64 && -diffScale < LONG_TEN_POW.Length) {
                return dividePrimitiveLongs(this.smallValue, LONG_TEN_POW[(int)-diffScale], newScale,roundingMode);
            }
            return divideBigIntegers(this.getUnscaledValue(),Multiplication.powerOf10(-diffScale),newScale,roundingMode);
        }

        /**
         * Returns a new {@code BigDecimal} instance with the specified scale.
         * <p>
         * If the new scale is greater than the old scale, then additional zeros are
         * added to the unscaled value. In this case no rounding is necessary.
         * <p>
         * If the new scale is smaller than the old scale, then trailing digits are
         * removed. If these trailing digits are not zero, then the remaining
         * unscaled value has to be rounded. For this rounding operation the
         * specified rounding mode is used.
         *
         * @param newScale
         *            scale of the result returned.
         * @param roundingMode
         *            rounding mode to be used to round the result.
         * @return a new {@code BigDecimal} instance with the specified scale.
         * @throws IllegalArgumentException
         *             if {@code roundingMode} is not a valid rounding mode.
         * @throws ArithmeticException
         *             if {@code roundingMode == ROUND_UNNECESSARY} and rounding is
         *             necessary according to the given scale.
         */
        public BigDecimal setScale(int newScale, int roundingMode) {
            return setScale(newScale, RoundingMode.valueOf(roundingMode));
        }

        /**
         * Returns a new {@code BigDecimal} instance with the specified scale. If
         * the new scale is greater than the old scale, then additional zeros are
         * added to the unscaled value. If the new scale is smaller than the old
         * scale, then trailing zeros are removed. If the trailing digits are not
         * zeros then an ArithmeticException is thrown.
         * <p>
         * If no exception is thrown, then the following equation holds: {@code
         * x.setScale(s).compareTo(x) == 0}.
         *
         * @param newScale
         *            scale of the result returned.
         * @return a new {@code BigDecimal} instance with the specified scale.
         * @throws ArithmeticException
         *             if rounding would be necessary.
         */
        public BigDecimal setScale(int newScale) {
            return setScale(newScale, RoundingMode.UNNECESSARY);
        }

        /**
         * Returns a new {@code BigDecimal} instance where the decimal point has
         * been moved {@code n} places to the left. If {@code n < 0} then the
         * decimal point is moved {@code -n} places to the right.
         * <p>
         * The result is obtained by changing its scale. If the scale of the result
         * becomes negative, then its precision is increased such that the scale is
         * zero.
         * <p>
         * Note, that {@code movePointLeft(0)} returns a result which is
         * mathematically equivalent, but which has {@code scale >= 0}.
         *
         * @param n
         *            number of placed the decimal point has to be moved.
         * @return {@code this * 10^(-n}).
         */
        public virtual BigDecimal movePointLeft(int n) {
            return movePoint(scaleJ + (long)n);
        }

        private BigDecimal movePoint(long newScale) {
            if (isZero()) {
                return zeroScaledBy(java.lang.Math.max(newScale, 0));
            }
            /* When:  'n'== Integer.MIN_VALUE  isn't possible to call to movePointRight(-n)  
             * since  -Integer.MIN_VALUE == Integer.MIN_VALUE */
            if(newScale >= 0) {
                if(bitLengthJ < 64) {
                    return valueOf(smallValue,toIntScale(newScale));
                }
                return new BigDecimal(getUnscaledValue(), toIntScale(newScale));
            }
            if(-newScale < LONG_TEN_POW.Length &&
                    bitLengthJ + LONG_TEN_POW_BIT_LENGTH[(int)-newScale] < 64 ) {
                return valueOf(smallValue*LONG_TEN_POW[(int)-newScale],0);
            }
            return new BigDecimal(Multiplication.multiplyByTenPow(getUnscaledValue(),(int)-newScale), 0);
        }

        /**
         * Returns a new {@code BigDecimal} instance where the decimal point has
         * been moved {@code n} places to the right. If {@code n < 0} then the
         * decimal point is moved {@code -n} places to the left.
         * <p>
         * The result is obtained by changing its scale. If the scale of the result
         * becomes negative, then its precision is increased such that the scale is
         * zero.
         * <p>
         * Note, that {@code movePointRight(0)} returns a result which is
         * mathematically equivalent, but which has scale >= 0.
         *
         * @param n
         *            number of placed the decimal point has to be moved.
         * @return {@code this * 10^n}.
         */
        public BigDecimal movePointRight(int n) {
            return movePoint(scaleJ - (long)n);
        }

        /**
         * Returns a new {@code BigDecimal} whose value is {@code this} 10^{@code n}.
         * The scale of the result is {@code this.scale()} - {@code n}.
         * The precision of the result is the precision of {@code this}.
         * <p>
         * This method has the same effect as {@link #movePointRight}, except that
         * the precision is not changed.
         *
         * @param n
         *            number of places the decimal point has to be moved.
         * @return {@code this * 10^n}
         */
        public BigDecimal scaleByPowerOfTen(int n) {
            long newScale = scaleJ - (long)n;
            if(bitLengthJ < 64) {
                //Taking care when a 0 is to be scaled
                if( smallValue==0  ){
                    return zeroScaledBy( newScale );
                }
                return valueOf(smallValue,toIntScale(newScale));
            }
            return new BigDecimal(getUnscaledValue(), toIntScale(newScale));
        }

        /**
         * Returns a new {@code BigDecimal} instance with the same value as {@code
         * this} but with a unscaled value where the trailing zeros have been
         * removed. If the unscaled value of {@code this} has n trailing zeros, then
         * the scale and the precision of the result has been reduced by n.
         *
         * @return a new {@code BigDecimal} instance equivalent to this where the
         *         trailing zeros of the unscaled value have been removed.
         */
        public BigDecimal stripTrailingZeros() {
            int i = 1; // 1 <= i <= 18
            int lastPow = TEN_POW.Length - 1;
            long newScale = scaleJ;

            if (isZero()) {
                return new BigDecimal("0");
            }
            BigInteger strippedBI = getUnscaledValue();
            BigInteger[] quotAndRem;
        
            // while the number is even...
            while (!strippedBI.testBit(0)) {
                // To divide by 10^i
                quotAndRem = strippedBI.divideAndRemainder(TEN_POW[i]);
                // To look the remainder
                if (quotAndRem[1].signum() == 0) {
                    // To adjust the scale
                    newScale -= i;
                    if (i < lastPow) {
                        // To set to the next power
                        i++;
                    }
                    strippedBI = quotAndRem[0];
                } else {
                    if (i == 1) {
                        // 'this' has no more trailing zeros
                        break;
                    }
                    // To set to the smallest power of ten
                    i = 1;
                }
            }
            return new BigDecimal(strippedBI, toIntScale(newScale));
        }

        /**
         * Compares this {@code BigDecimal} with {@code val}. Returns one of the
         * three values {@code 1}, {@code 0}, or {@code -1}. The method behaves as
         * if {@code this.subtract(val)} is computed. If this difference is > 0 then
         * 1 is returned, if the difference is < 0 then -1 is returned, and if the
         * difference is 0 then 0 is returned. This means, that if two decimal
         * instances are compared which are equal in value but differ in scale, then
         * these two instances are considered as equal.
         *
         * @param val
         *            value to be compared with {@code this}.
         * @return {@code 1} if {@code this > val}, {@code -1} if {@code this < val},
         *         {@code 0} if {@code this == val}.
         * @throws NullPointerException
         *             if {@code val == null}.
         */
        public int compareTo(BigDecimal val) {
            int thisSign = signum();
            int valueSign = val.signum();

            if( thisSign == valueSign) {
                if(this.scaleJ == val.scaleJ && this.bitLengthJ<64 && val.bitLengthJ<64 ) {
                    return (smallValue < val.smallValue) ? -1 : (smallValue > val.smallValue) ? 1 : 0;
                }
                long diffScale = (long)this.scaleJ - val.scaleJ;
                int diffPrecision = this.aproxPrecision() - val.aproxPrecision();
                if (diffPrecision > diffScale + 1) {
                    return thisSign;
                } else if (diffPrecision < diffScale - 1) {
                    return -thisSign;
                } else {// thisSign == val.signum()  and  diffPrecision is aprox. diffScale
                    BigInteger thisUnscaled = this.getUnscaledValue();
                    BigInteger valUnscaled = val.getUnscaledValue();
                    // If any of both precision is bigger, append zeros to the shorter one
                    if (diffScale < 0) {
                        thisUnscaled = thisUnscaled.multiply(Multiplication.powerOf10(-diffScale));
                    } else if (diffScale > 0) {
                        valUnscaled = valUnscaled.multiply(Multiplication.powerOf10(diffScale));
                    }
                    return thisUnscaled.compareTo(valUnscaled);
                }
            } else if (thisSign < valueSign) {
                return -1;
            } else  {
                return 1;
            }
        }

        /**
         * Returns {@code true} if {@code x} is a {@code BigDecimal} instance and if
         * this instance is equal to this big decimal. Two big decimals are equal if
         * their unscaled value and their scale is equal. For example, 1.0
         * (10*10^(-1)) is not equal to 1.00 (100*10^(-2)). Similarly, zero
         * instances are not equal if their scale differs.
         *
         * @param x
         *            object to be compared with {@code this}.
         * @return true if {@code x} is a {@code BigDecimal} and {@code this == x}.
         */
        public override bool Equals(Object x) {
            if (this == x) {
                return true;
            }
            if (x is BigDecimal) {
                BigDecimal x1 = (BigDecimal) x;
                return x1.scaleJ == scaleJ
                       && (bitLengthJ < 64 ? (x1.smallValue == smallValue)
                        : intVal.equals(x1.intVal));


            }
            return false;
        }   

        /**
         * Returns the minimum of this {@code BigDecimal} and {@code val}.
         *
         * @param val
         *            value to be used to compute the minimum with this.
         * @return {@code min(this, val}.
         * @throws NullPointerException
         *             if {@code val == null}.
         */
        public BigDecimal min(BigDecimal val) {
            return ((compareTo(val) <= 0) ? this : val);
        }

        /**
         * Returns the maximum of this {@code BigDecimal} and {@code val}.
         *
         * @param val
         *            value to be used to compute the maximum with this.
         * @return {@code max(this, val}.
         * @throws NullPointerException
         *             if {@code val == null}.
         */
        public BigDecimal max(BigDecimal val) {
            return ((compareTo(val) >= 0) ? this : val);
        }

        /**
         * Returns a hash code for this {@code BigDecimal}.
         *
         * @return hash code for {@code this}.
         */
        public override int GetHashCode() {        
            if (hashCode != 0) {
                return hashCode;
            }
            if (bitLengthJ < 64) {
                hashCode = (int)(smallValue & 0xffffffff);
                hashCode = 33 * hashCode +  (int)((smallValue >> 32) & 0xffffffff);
                hashCode = 17 * hashCode + scaleJ;
                return hashCode;
            }
            hashCode = 17 * intVal.GetHashCode() + scaleJ;
            return hashCode;
        }

        /**
         * Returns a canonical string representation of this {@code BigDecimal}. If
         * necessary, scientific notation is used. This representation always prints
         * all significant digits of this value.
         * <p>
         * If the scale is negative or if {@code scale - precision >= 6} then
         * scientific notation is used.
         *
         * @return a string representation of {@code this} in scientific notation if
         *         necessary.
         */
        public override String ToString() {
            if (toStringImage != null) {
                return toStringImage;
            }
            if(bitLengthJ < 32) {
                toStringImage = Conversion.toDecimalScaledString(smallValue,scaleJ);
                return toStringImage;
            }
            String intString = getUnscaledValue().toString();
            if (scaleJ == 0) {
                return intString;
            }
            int begin = (getUnscaledValue().signum() < 0) ? 2 : 1;
            int end = intString.length();
            long exponent = -(long)scaleJ + end - begin;
            StringBuilder result = new StringBuilder();

            result.Append(intString);
            if ((scaleJ > 0) && (exponent >= -6)) {
                if (exponent >= 0) {
                    result.Insert(end - scaleJ, '.');
                } else {
                    result.Insert(begin - 1, "0."); //$NON-NLS-1$
                    result.Insert(begin + 1, CH_ZEROS, 0, -(int)exponent - 1);
                }
            } else {
                if (end - begin >= 1) {
                    result.Insert(begin, '.');
                    end++;
                }
                result.Insert(end, 'E');
                if (exponent > 0) {
                    result.Insert(++end, '+');
                }
                result.Insert(++end, java.lang.Long.toString(exponent));
            }
            toStringImage = result.ToString();
            return toStringImage;
        }

        /**
         * Returns a string representation of this {@code BigDecimal}. This
         * representation always prints all significant digits of this value.
         * <p>
         * If the scale is negative or if {@code scale - precision >= 6} then
         * engineering notation is used. Engineering notation is similar to the
         * scientific notation except that the exponent is made to be a multiple of
         * 3 such that the integer part is >= 1 and < 1000.
         *
         * @return a string representation of {@code this} in engineering notation
         *         if necessary.
         */
        public String toEngineeringString() {
            String intString = getUnscaledValue().toString();
            if (scaleJ == 0) {
                return intString;
            }
            int begin = (getUnscaledValue().signum() < 0) ? 2 : 1;
            int end = intString.length();
            long exponent = -(long)scaleJ + end - begin;
            StringBuilder result = new StringBuilder(intString);

            if ((scaleJ > 0) && (exponent >= -6)) {
                if (exponent >= 0) {
                    result.Insert(end - scaleJ, '.');
                } else {
                    result.Insert(begin - 1, "0."); //$NON-NLS-1$
                    result.Insert(begin + 1, CH_ZEROS, 0, -(int)exponent - 1);
                }
            } else {
                int delta = end - begin;
                int rem = (int)(exponent % 3);

                if (rem != 0) {
                    // adjust exponent so it is a multiple of three
                    if (getUnscaledValue().signum() == 0) {
                        // zero value
                        rem = (rem < 0) ? -rem : 3 - rem;
                        exponent += rem;
                    } else {
                        // nonzero value
                        rem = (rem < 0) ? rem + 3 : rem;
                        exponent -= rem;
                        begin += rem;
                    }
                    if (delta < 3) {
                        for (int i = rem - delta; i > 0; i--) {
                            result.Insert(end++, '0');
                        }
                    }
                }
                if (end - begin >= 1) {
                    result.Insert(begin, '.');
                    end++;
                }
                if (exponent != 0) {
                    result.Insert(end, 'E');
                    if (exponent > 0) {
                        result.Insert(++end, '+');
                    }
                    result.Insert(++end, java.lang.Long.toString(exponent));
                }
            }
            return result.toString();
        }

        /**
         * Returns a string representation of this {@code BigDecimal}. No scientific
         * notation is used. This methods adds zeros where necessary.
         * <p>
         * If this string representation is used to create a new instance, this
         * instance is generally not identical to {@code this} as the precision
         * changes.
         * <p>
         * {@code x.equals(new BigDecimal(x.toPlainString())} usually returns
         * {@code false}.
         * <p>
         * {@code x.compareTo(new BigDecimal(x.toPlainString())} returns {@code 0}.
         *
         * @return a string representation of {@code this} without exponent part.
         */
        public String toPlainString() {
            String intStr = getUnscaledValue().toString();
            if ((scaleJ == 0) || ((isZero()) && (scaleJ < 0))) {
                return intStr;
            }
            int begin = (signum() < 0) ? 1 : 0;
            int delta = scaleJ;
            // We take space for all digits, plus a possible decimal point, plus 'scale'
            StringBuilder result = new StringBuilder(intStr.length() + 1 + java.lang.Math.abs(scaleJ));

            if (begin == 1) {
                // If the number is negative, we insert a '-' character at front 
                result.Append('-');
            }
            if (scaleJ > 0) {
                delta -= (intStr.length() - begin);
                if (delta >= 0) {
                    result.Append("0."); //$NON-NLS-1$
                    // To append zeros after the decimal point
                    for (; delta > CH_ZEROS.Length; delta -= CH_ZEROS.Length) {
                        result.Append(CH_ZEROS);
                    }
                    result.Append(CH_ZEROS, 0, delta);
                    result.Append(intStr.substring(begin));
                } else {
                    delta = begin - delta;
                    result.Append(intStr.substring(begin, delta));
                    result.Append('.');
                    result.Append(intStr.substring(delta));
                }
            } else {// (scale <= 0)
                result.Append(intStr.substring(begin));
                // To append trailing zeros
                for (; delta < -CH_ZEROS.Length; delta += CH_ZEROS.Length) {
                    result.Append(CH_ZEROS);
                }
                result.Append(CH_ZEROS, 0, -delta);
            }
            return result.toString();
        }

        /**
         * Returns this {@code BigDecimal} as a big integer instance. A fractional
         * part is discarded.
         *
         * @return this {@code BigDecimal} as a big integer instance.
         */
        public BigInteger toBigInteger() {
            if ((scaleJ == 0) || (isZero())) {
                return getUnscaledValue();
            } else if (scaleJ < 0) {
                return getUnscaledValue().multiply(Multiplication.powerOf10(-(long)scaleJ));
            } else {// (scale > 0)
                return getUnscaledValue().divide(Multiplication.powerOf10(scaleJ));
            }
        }

        /**
         * Returns this {@code BigDecimal} as a big integer instance if it has no
         * fractional part. If this {@code BigDecimal} has a fractional part, i.e.
         * if rounding would be necessary, an {@code ArithmeticException} is thrown.
         *
         * @return this {@code BigDecimal} as a big integer value.
         * @throws ArithmeticException
         *             if rounding is necessary.
         */
        public BigInteger toBigIntegerExact() {
            if ((scaleJ == 0) || (isZero())) {
                return getUnscaledValue();
            } else if (scaleJ < 0) {
                return getUnscaledValue().multiply(Multiplication.powerOf10(-(long)scaleJ));
            } else {// (scale > 0)
                BigInteger[] integerAndFraction;
                // An optimization before do a heavy division
                if ((scaleJ > aproxPrecision()) || (scaleJ > getUnscaledValue().getLowestSetBit())) {
                    // math.08=Rounding necessary
                    throw new ArithmeticException("Rounding necessary"); //$NON-NLS-1$
                }
                integerAndFraction = getUnscaledValue().divideAndRemainder(Multiplication.powerOf10(scaleJ));
                if (integerAndFraction[1].signum() != 0) {
                    // It exists a non-zero fractional part 
                    // math.08=Rounding necessary
                    throw new ArithmeticException("Rounding necessary"); //$NON-NLS-1$
                }
                return integerAndFraction[0];
            }
        }

        /**
         * Returns this {@code BigDecimal} as an long value. Any fractional part is
         * discarded. If the integral part of {@code this} is too big to be
         * represented as an long, then {@code this} % 2^64 is returned.
         *
         * @return this {@code BigDecimal} as a long value.
         */
        public override long longValue() {
            /* If scale <= -64 there are at least 64 trailing bits zero in 10^(-scale).
             * If the scale is positive and very large the long value could be zero. */
            return ((scaleJ <= -64) || (scaleJ > aproxPrecision())
            ? 0L
                    : toBigInteger().longValue());
        }

        /**
         * Returns this {@code BigDecimal} as a long value if it has no fractional
         * part and if its value fits to the int range ([-2^{63}..2^{63}-1]). If
         * these conditions are not met, an {@code ArithmeticException} is thrown.
         *
         * @return this {@code BigDecimal} as a long value.
         * @throws ArithmeticException
         *             if rounding is necessary or the number doesn't fit in a long.
         */
        public long longValueExact() {
            return valueExact(64);
        }

        /**
         * Returns this {@code BigDecimal} as an int value. Any fractional part is
         * discarded. If the integral part of {@code this} is too big to be
         * represented as an int, then {@code this} % 2^32 is returned.
         *
         * @return this {@code BigDecimal} as a int value.
         */
        public override int intValue() {
            /* If scale <= -32 there are at least 32 trailing bits zero in 10^(-scale).
             * If the scale is positive and very large the long value could be zero. */
            return ((scaleJ <= -32) || (scaleJ > aproxPrecision())
            ? 0
                    : toBigInteger().intValue());
        }

        /**
         * Returns this {@code BigDecimal} as a int value if it has no fractional
         * part and if its value fits to the int range ([-2^{31}..2^{31}-1]). If
         * these conditions are not met, an {@code ArithmeticException} is thrown.
         *
         * @return this {@code BigDecimal} as a int value.
         * @throws ArithmeticException
         *             if rounding is necessary or the number doesn't fit in a int.
         */
        public int intValueExact() {
            return (int)valueExact(32);
        }

        /**
         * Returns this {@code BigDecimal} as a short value if it has no fractional
         * part and if its value fits to the short range ([-2^{15}..2^{15}-1]). If
         * these conditions are not met, an {@code ArithmeticException} is thrown.
         *
         * @return this {@code BigDecimal} as a short value.
         * @throws ArithmeticException
         *             if rounding is necessary of the number doesn't fit in a
         *             short.
         */
        public short shortValueExact() {
            return (short)valueExact(16);
        }

        /**
         * Returns this {@code BigDecimal} as a byte value if it has no fractional
         * part and if its value fits to the byte range ([-128..127]). If these
         * conditions are not met, an {@code ArithmeticException} is thrown.
         *
         * @return this {@code BigDecimal} as a byte value.
         * @throws ArithmeticException
         *             if rounding is necessary or the number doesn't fit in a byte.
         */
        public byte byteValueExact() {
            return (byte)valueExact(8);
        }

        /**
         * Returns this {@code BigDecimal} as a float value. If {@code this} is too
         * big to be represented as an float, then {@code Float.POSITIVE_INFINITY}
         * or {@code Float.NEGATIVE_INFINITY} is returned.
         * <p>
         * Note, that if the unscaled value has more than 24 significant digits,
         * then this decimal cannot be represented exactly in a float variable. In
         * this case the result is rounded.
         * <p>
         * For example, if the instance {@code x1 = new BigDecimal("0.1")} cannot be
         * represented exactly as a float, and thus {@code x1.equals(new
         * BigDecimal(x1.folatValue())} returns {@code false} for this case.
         * <p>
         * Similarly, if the instance {@code new BigDecimal(16777217)} is converted
         * to a float, the result is {@code 1.6777216E}7.
         *
         * @return this {@code BigDecimal} as a float value.
         */
        public override float floatValue() {
            /* A similar code like in doubleValue() could be repeated here,
             * but this simple implementation is quite efficient. */
            float floatResult = signum();
            long powerOfTwo = this.bitLengthJ - (long)(scaleJ / LOG10_2);
            if ((powerOfTwo < -149) || (floatResult == 0.0f)) {
                // Cases which 'this' is very small
                floatResult *= 0.0f;
            } else if (powerOfTwo > 129) {
                // Cases which 'this' is very large
                floatResult *= java.lang.Float.POSITIVE_INFINITY;
            } else {
                floatResult = (float)doubleValue();
            }
            return floatResult;
        }

        /**
         * Returns this {@code BigDecimal} as a double value. If {@code this} is too
         * big to be represented as an float, then {@code Double.POSITIVE_INFINITY}
         * or {@code Double.NEGATIVE_INFINITY} is returned.
         * <p>
         * Note, that if the unscaled value has more than 53 significant digits,
         * then this decimal cannot be represented exactly in a double variable. In
         * this case the result is rounded.
         * <p>
         * For example, if the instance {@code x1 = new BigDecimal("0.1")} cannot be
         * represented exactly as a double, and thus {@code x1.equals(new
         * BigDecimal(x1.doubleValue())} returns {@code false} for this case.
         * <p>
         * Similarly, if the instance {@code new BigDecimal(9007199254740993L)} is
         * converted to a double, the result is {@code 9.007199254740992E15}.
         * <p>
         *
         * @return this {@code BigDecimal} as a double value.
         */
        public override double doubleValue() {
            int sign = signum();
            int exponent = 1076; // bias + 53
            int lowestSetBit;
            int discardedSize;
            long powerOfTwo = this.bitLengthJ - (long)(scaleJ / LOG10_2);
            long bits; // IEEE-754 Standard
            long tempBits; // for temporal calculations     
            BigInteger mantisa;

            if ((powerOfTwo < -1074) || (sign == 0)) {
                // Cases which 'this' is very small            
                return (sign * 0.0d);
            } else if (powerOfTwo > 1025) {
                // Cases which 'this' is very large            
                return (sign * java.lang.Double.POSITIVE_INFINITY);
            }
            mantisa = getUnscaledValue().abs();
            // Let be:  this = [u,s], with s > 0
            if (scaleJ <= 0) {
                // mantisa = abs(u) * 10^s
                mantisa = mantisa.multiply(Multiplication.powerOf10(-scaleJ));
            } else {// (scale > 0)
                BigInteger[] quotAndRem;
                BigInteger powerOfTen = Multiplication.powerOf10(scaleJ);
                int k = 100 - (int)powerOfTwo;
                int compRem;

                if (k > 0) {
                    /* Computing (mantisa * 2^k) , where 'k' is a enough big
                     * power of '2' to can divide by 10^s */
                    mantisa = mantisa.shiftLeft(k);
                    exponent -= k;
                }
                // Computing (mantisa * 2^k) / 10^s
                quotAndRem = mantisa.divideAndRemainder(powerOfTen);
                // To check if the fractional part >= 0.5
                compRem = quotAndRem[1].shiftLeftOneBit().compareTo(powerOfTen);
                // To add two rounded bits at end of mantisa
                mantisa = quotAndRem[0].shiftLeft(2).add(
                        BigInteger.valueOf((compRem * (compRem + 3)) / 2 + 1));
                exponent -= 2;
            }
            lowestSetBit = mantisa.getLowestSetBit();
            discardedSize = mantisa.bitLength() - 54;
            if (discardedSize > 0) {// (n > 54)
                // mantisa = (abs(u) * 10^s) >> (n - 54)
                bits = mantisa.shiftRight(discardedSize).longValue();
                tempBits = bits;
                // #bits = 54, to check if the discarded fraction produces a carry             
                if ((((bits & 1) == 1) && (lowestSetBit < discardedSize))
                        || ((bits & 3) == 3)) {
                    bits += 2;
                }
            } else {// (n <= 54)
                // mantisa = (abs(u) * 10^s) << (54 - n)                
                bits = mantisa.longValue() << -discardedSize;
                tempBits = bits;
                // #bits = 54, to check if the discarded fraction produces a carry:
                if ((bits & 3) == 3) {
                    bits += 2;
                }
            }
            // Testing bit 54 to check if the carry creates a new binary digit
            if ((bits & 0x40000000000000L) == 0) {
                // To drop the last bit of mantisa (first discarded)
                bits >>= 1;
                // exponent = 2^(s-n+53+bias)
                exponent += discardedSize;
            } else {// #bits = 54
                bits >>= 2;
                exponent += discardedSize + 1;
            }
            // To test if the 53-bits number fits in 'double'            
            if (exponent > 2046) {// (exponent - bias > 1023)
                return (sign * java.lang.Double.POSITIVE_INFINITY);
            } else if (exponent <= 0) {// (exponent - bias <= -1023)
                // Denormalized numbers (having exponent == 0)
                if (exponent < -53) {// exponent - bias < -1076
                    return (sign * 0.0d);
                }
                // -1076 <= exponent - bias <= -1023 
                // To discard '- exponent + 1' bits
                bits = tempBits >> 1;
                tempBits = bits & (java.dotnet.lang.Operator.shiftRightUnsignet( -1L , (63 + exponent)));
                bits >>= (-exponent );
                // To test if after discard bits, a new carry is generated
                if (((bits & 3) == 3) || (((bits & 1) == 1) && (tempBits != 0)
                && (lowestSetBit < discardedSize))) {
                    bits += 1;
                }
                exponent = 0;
                bits >>= 1;
            }
            // Construct the 64 double bits: [sign(1), exponent(11), mantisa(52)]
            bits = (sign == -1 ? java.lang.Long.MIN_VALUE : 0)//(sign & 0x8000000000000000L) // bei -1 Long.MIN_VALUE sonst 0
                    | ((long)exponent << 52)
                    | (bits & 0xFFFFFFFFFFFFFL);
            return java.lang.Double.longBitsToDouble(bits);
        }

        /**
         * Returns the unit in the last place (ULP) of this {@code BigDecimal}
         * instance. An ULP is the distance to the nearest big decimal with the same
         * precision.
         * <p>
         * The amount of a rounding error in the evaluation of a floating-point
         * operation is often expressed in ULPs. An error of 1 ULP is often seen as
         * a tolerable error.
         * <p>
         * For class {@code BigDecimal}, the ULP of a number is simply 10^(-scale).
         * <p>
         * For example, {@code new BigDecimal(0.1).ulp()} returns {@code 1E-55}.
         *
         * @return unit in the last place (ULP) of this {@code BigDecimal} instance.
         */
        public BigDecimal ulp() {
            return valueOf(1, scaleJ);
        }

        /* Private Methods */

        /**
         * It does all rounding work of the public method
         * {@code round(MathContext)}, performing an inplace rounding
         * without creating a new object.
         *
         * @param mc
         *            the {@code MathContext} for perform the rounding.
         * @see #round(MathContext)
         */
        private void inplaceRound(MathContext mc) {
            int mcPrecision = mc.getPrecision();
            if (aproxPrecision() - mcPrecision <= 0 || mcPrecision == 0) {
                return;
            }
            int discardedPrecision = precision() - mcPrecision;
            // If no rounding is necessary it returns immediately
            if ((discardedPrecision <= 0)) {
                return;
            }
            // When the number is small perform an efficient rounding
            if (this.bitLengthJ < 64) {
                smallRound(mc, discardedPrecision);
                return;
            }
            // Getting the integer part and the discarded fraction
            BigInteger sizeOfFraction = Multiplication.powerOf10(discardedPrecision);
            BigInteger[] integerAndFraction = getUnscaledValue().divideAndRemainder(sizeOfFraction);
            long newScale = (long)scaleJ - discardedPrecision;
            int compRem;
            BigDecimal tempBD;
            // If the discarded fraction is non-zero, perform rounding
            if (integerAndFraction[1].signum() != 0) {
                // To check if the discarded fraction >= 0.5
                compRem = (integerAndFraction[1].abs().shiftLeftOneBit().compareTo(sizeOfFraction));
                // To look if there is a carry
                compRem =  roundingBehavior( integerAndFraction[0].testBit(0) ? 1 : 0,
                        integerAndFraction[1].signum() * (5 + compRem),
                        mc.getRoundingMode());
                if (compRem != 0) {
                    integerAndFraction[0] = integerAndFraction[0].add(BigInteger.valueOf(compRem));
                }
                tempBD = new BigDecimal(integerAndFraction[0]);
                // If after to add the increment the precision changed, we normalize the size
                if (tempBD.precision() > mcPrecision) {
                    integerAndFraction[0] = integerAndFraction[0].divide(BigInteger.TEN);
                    newScale--;
                }
            }
            // To update all internal fields
            scaleJ = toIntScale(newScale);
            precisionJ = mcPrecision;
            setUnscaledValue(integerAndFraction[0]);
        }

        private static int longCompareTo(long value1, long value2) {
            return value1 > value2 ? 1 : (value1 < value2 ? -1 : 0);
        }
        /**
         * This method implements an efficient rounding for numbers which unscaled
         * value fits in the type {@code long}.
         *
         * @param mc
         *            the context to use
         * @param discardedPrecision
         *            the number of decimal digits that are discarded
         * @see #round(MathContext)
         */
        private void smallRound(MathContext mc, int discardedPrecision) {
            long sizeOfFraction = LONG_TEN_POW[discardedPrecision];
            long newScale = (long)scaleJ - discardedPrecision;
            long unscaledVal = smallValue;
            // Getting the integer part and the discarded fraction
            long integer = unscaledVal / sizeOfFraction;
            long fraction = unscaledVal % sizeOfFraction;
            int compRem;
            // If the discarded fraction is non-zero perform rounding
            if (fraction != 0) {
                // To check if the discarded fraction >= 0.5
                compRem = longCompareTo(java.lang.Math.abs(fraction) << 1,sizeOfFraction);
                // To look if there is a carry
                integer += roundingBehavior( ((int)integer) & 1,
                        java.lang.Long.signum(fraction) * (5 + compRem),
                        mc.getRoundingMode());
                // If after to add the increment the precision changed, we normalize the size
                if (java.lang.Math.log10(java.lang.Math.abs(integer)) >= mc.getPrecision()) {
                    integer /= 10;
                    newScale--;
                }
            }
            // To update all internal fields
            scaleJ = toIntScale(newScale);
            precisionJ = mc.getPrecision();
            smallValue = integer;
            bitLengthJ = bitLength(integer);
            intVal = null;
        }

        /**
         * Return an increment that can be -1,0 or 1, depending of
         * {@code roundingMode}.
         *
         * @param parityBit
         *            can be 0 or 1, it's only used in the case
         *            {@code HALF_EVEN}
         * @param fraction
         *            the mantisa to be analyzed
         * @param roundingMode
         *            the type of rounding
         * @return the carry propagated after rounding
         */
        private static int roundingBehavior(int parityBit, int fraction, RoundingMode roundingMode) {
            int increment = 0; // the carry after rounding

            if (roundingMode.ordinal()==RoundingMode.UNNECESSARY.ordinal()) {
                if (fraction != 0) {
                    // math.08=Rounding necessary
                    throw new ArithmeticException("Rounding necessary"); //$NON-NLS-1$
                }
            }
            else if (roundingMode.ordinal()==RoundingMode.UP.ordinal()) {
                    increment = java.lang.Integer.signum(fraction);
            }
            else if (roundingMode.ordinal()==RoundingMode.DOWN.ordinal()) {
            }
            else if (roundingMode.ordinal()==RoundingMode.CEILING.ordinal()) {
                    increment = java.lang.Math.max(java.lang.Integer.signum(fraction), 0);
            }
            else if (roundingMode.ordinal()==RoundingMode.FLOOR.ordinal()) {
                    increment = java.lang.Math.min(java.lang.Integer.signum(fraction), 0);
            }
            else if (roundingMode.ordinal()==RoundingMode.HALF_UP.ordinal()) {
                    if (java.lang.Math.abs(fraction) >= 5) {
                        increment = java.lang.Integer.signum(fraction);
                    }
            }
            else if (roundingMode.ordinal()==RoundingMode.HALF_DOWN.ordinal()) {
                    if (java.lang.Math.abs(fraction) > 5) {
                        increment = java.lang.Integer.signum(fraction);
                    }
            }
            else if (roundingMode.ordinal()==RoundingMode.HALF_EVEN.ordinal()) {
                    if (java.lang.Math.abs(fraction) + parityBit > 5) {
                        increment = java.lang.Integer.signum(fraction);
                    }
            }
            return increment;
        }

        /**
         * If {@code intVal} has a fractional part throws an exception,
         * otherwise it counts the number of bits of value and checks if it's out of
         * the range of the primitive type. If the number fits in the primitive type
         * returns this number as {@code long}, otherwise throws an
         * exception.
         *
         * @param bitLengthJOfType
         *            number of bits of the type whose value will be calculated
         *            exactly
         * @return the exact value of the integer part of {@code BigDecimal}
         *         when is possible
         * @throws ArithmeticException when rounding is necessary or the
         *             number don't fit in the primitive type
         */
        private long valueExact(int bitLengthJOfType) {
            BigInteger bigInteger = toBigIntegerExact();

            if (bigInteger.bitLength() < bitLengthJOfType) {
                // It fits in the primitive type
                return bigInteger.longValue();
            }
            // math.08=Rounding necessary
            throw new ArithmeticException("Rounding necessary"); //$NON-NLS-1$
        }

        /**
         * If the precision already was calculated it returns that value, otherwise
         * it calculates a very good approximation efficiently . Note that this
         * value will be {@code precision()} or {@code precision()-1}
         * in the worst case.
         *
         * @return an approximation of {@code precision()} value
         */
        private int aproxPrecision() {
            return ((precisionJ > 0)
            ? precisionJ
                    : (int)((this.bitLengthJ - 1) * LOG10_2)) + 1;
        }

        /**
         * It tests if a scale of type {@code long} fits in 32 bits. It
         * returns the same scale being casted to {@code int} type when is
         * possible, otherwise throws an exception.
         *
         * @param longScale
         *            a 64 bit scale
         * @return a 32 bit scale when is possible
         * @throws ArithmeticException when {@code scale} doesn't
         *             fit in {@code int} type
         * @see #scale
         */
        private static int toIntScale(long longScale) {
            if (longScale < java.lang.Integer.MIN_VALUE) {
                // math.09=Overflow
                throw new java.lang.ArithmeticException("Overflow"); //$NON-NLS-1$
            } else if (longScale > java.lang.Integer.MAX_VALUE) {
                // math.0A=Underflow
                throw new java.lang.ArithmeticException("Underflow"); //$NON-NLS-1$
            } else {
                return (int)longScale;
            }
        }

        /**
         * It returns the value 0 with the most approximated scale of type
         * {@code int}. if {@code longScale > Integer.MAX_VALUE} the
         * scale will be {@code Integer.MAX_VALUE}; if
         * {@code longScale < Integer.MIN_VALUE} the scale will be
         * {@code Integer.MIN_VALUE}; otherwise {@code longScale} is
         * casted to the type {@code int}.
         *
         * @param longScale
         *            the scale to which the value 0 will be scaled.
         * @return the value 0 scaled by the closer scale of type {@code int}.
         * @see #scale
         */
        private static BigDecimal zeroScaledBy(long longScale) {
            if (longScale == (int) longScale) {
                return valueOf(0,(int)longScale);
                }
            if (longScale >= 0) {
                return new BigDecimal( 0, java.lang.Integer.MAX_VALUE);
            }
            return new BigDecimal( 0, java.lang.Integer.MIN_VALUE);
        }

        /**
         * Assignes all transient fields upon deserialization of a
         * {@code BigDecimal} instance (bitLengthJ and smallValue). The transient
         * field precision is assigned lazily.
         *
        private void readObject(ObjectInputStream in) throws IOException,
                ClassNotFoundException {
            in.defaultReadObject();

            this.bitLengthJ = intVal.bitLengthJ();
            if (this.bitLengthJ < 64) {
                this.smallValue = intVal.longValue();
            }
        }

        /**
         * Prepares this {@code BigDecimal} for serialization, i.e. the
         * non-transient field {@code intVal} is assigned.
         *
        private void writeObject(ObjectOutputStream out) throws IOException {
            getUnscaledValue();
            out.defaultWriteObject();
        }*/

        private BigInteger getUnscaledValue() {
            if(intVal == null) {
                intVal = BigInteger.valueOf(smallValue);
            }
            return intVal;
        }
    
        private void setUnscaledValue(BigInteger unscaledValue) {
            this.intVal = unscaledValue;
            this.bitLengthJ = unscaledValue.bitLength();
            if(this.bitLengthJ < 64) {
                this.smallValue = unscaledValue.longValue();
            }
        }
    
        private static int bitLength(long smallValue) {
            if(smallValue < 0) {
                smallValue = ~smallValue;
            }
            return 64 - java.lang.Long.numberOfLeadingZeros(smallValue);
        }
    
        private static int bitLength(int smallValue) {
            if(smallValue < 0) {
                smallValue = ~smallValue;
            }
            return 32 - java.lang.Integer.numberOfLeadingZeros(smallValue);
        }

        public override short shortValue()
        {
            return (short)this.intValue();
        }
        public override byte byteValue()
        {
            return (byte)this.intValue();
        }

    }

}
