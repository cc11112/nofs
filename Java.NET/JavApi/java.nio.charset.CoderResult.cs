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

namespace biz.ritter.javapi.nio.charset
{

    /// <summary>
    /// Used to indicate the result of encoding/decoding. There are four types of
    /// results:
    /// <para>UNDERFLOW indicates that all input has been processed but more input is
    /// required. It is represented by the unique object <code>CoderResult.UNDERFLOW</code>.</para>
    /// <para>OVERFLOW indicates an insufficient output buffer size. It is represented
    /// by the unique object <code>CoderResult.OVERFLOW</code>.
    /// </para>
    /// <para>A malformed-input error indicates that an unrecognizable sequence of
    /// input units has been encountered. Get an instance of this type of result by
    /// calling <code>CoderResult.malformedForLength(int)</code> with the length of
    /// the malformed-input.
    /// </para>
    /// <para>An unmappable-character error indicates that a sequence of input units
    /// can not be mapped to the output charset. Get an instance of this type of
    /// result by calling <code>CoderResult.unmappableForLength(int)</code> with
    /// the input sequence size indicating the identity of the unmappable character.
    /// </para>
    /// </summary>
    public class CoderResult {

        // indicating underflow error type
        private static readonly int TYPE_UNDERFLOW = 1;

        // indicating overflow error type
        private static readonly int TYPE_OVERFLOW = 2;

        // indicating malformed-input error type
        private static readonly int TYPE_MALFORMED_INPUT = 3;

        // indicating unmappable character error type
        private static readonly int TYPE_UNMAPPABLE_CHAR = 4;

        /// <summary>
        /// Result object indicating that there is insufficient data in the
        /// encoding/decoding buffer or that additional data is required.
        /// </summary>
        public static readonly CoderResult UNDERFLOW = new CoderResult(TYPE_UNDERFLOW,0);

        /// <summary>
        /// Result object used to indicate that the output buffer does not have
        /// enough space available to store the result of the encoding/decoding.
        /// </summary>
        public static readonly CoderResult OVERFLOW = new CoderResult(TYPE_OVERFLOW, 0);

        /*
         * Stores unique result objects for each malformed-input error of a certain
         * length
         * <strong>Basties note: use no WeakHashMap</strong>
         */
        private static java.util.HashMap<int, CoderResult> _malformedErrors = new java.util.LinkedHashMap<int, CoderResult>();

        /*
         * Stores unique result objects for each unmappable-character error of a
         * certain length
         * <strong>Basties note: use no WeakHashMap</strong>
         */
        private static java.util.HashMap<int, CoderResult> _unmappableErrors = new java.util.LinkedHashMap<int, CoderResult>();

        // the type of this result
        private readonly int type;

        // the length of the erroneous input
        private readonly int lengthJ;

        /**
         * Constructs a <code>CoderResult</code> object with its text description.
         * 
         * @param type
         *            the type of this result
         * @param length
         *            the length of the erroneous input
         */
        private CoderResult(int type, int length) {
            this.type = type;
            this.lengthJ = length;
        }

        private readonly static Object lockObject = new Object();

        /**
         * Gets a <code>CoderResult</code> object indicating a malformed-input
         * error.
         * 
         * @param length
         *            the length of the malformed-input.
         * @return a <code>CoderResult</code> object indicating a malformed-input
         *         error.
         * @throws IllegalArgumentException
         *             if <code>length</code> is non-positive.
         */
        public static CoderResult malformedForLength(int length) {
            lock (lockObject) {
                if (length > 0) {
                    int key = length;
                    lock (_malformedErrors) {
                        CoderResult r = _malformedErrors.get(key);
                        if (null == r) {
                            r = new CoderResult(TYPE_MALFORMED_INPUT, length);
                            _malformedErrors.put(key, r);
                        }
                        return r;
                    }
                }
                throw new java.lang.IllegalArgumentException("The length must be positive: "+length+"."); //$NON-NLS-1$
            }
        }

        /**
         * Gets a <code>CoderResult</code> object indicating an unmappable
         * character error.
         * 
         * @param length
         *            the length of the input unit sequence denoting the unmappable
         *            character.
         * @return a <code>CoderResult</code> object indicating an unmappable
         *         character error.
         * @throws IllegalArgumentException
         *             if <code>length</code> is non-positive.
         */
        public static CoderResult unmappableForLength(int length) {
            lock (lockObject) {
                if (length > 0) {
                    int key = length;
                    lock (_unmappableErrors) {
                        CoderResult r = _unmappableErrors.get(key);
                        if (null == r) {
                            r = new CoderResult(TYPE_UNMAPPABLE_CHAR, length);
                            _unmappableErrors.put(key, r);
                        }
                        return r;
                    }
                }
                throw new java.lang.IllegalArgumentException("The length must be positive: "+length+"."); //$NON-NLS-1$
            }
        }

        /**
         * Returns true if this result is an underflow condition.
         * 
         * @return true if an underflow, otherwise false.
         */
        public bool isUnderflow() {
            return this.type == TYPE_UNDERFLOW;
        }

        /**
         * Returns true if this result represents a malformed-input error or an
         * unmappable-character error.
         * 
         * @return true if this is a malformed-input error or an
         *         unmappable-character error, otherwise false.
         */
        public bool isError() {
            return this.type == TYPE_MALFORMED_INPUT
                    || this.type == TYPE_UNMAPPABLE_CHAR;
        }

        /**
         * Returns true if this result represents a malformed-input error.
         * 
         * @return true if this is a malformed-input error, otherwise false.
         */
        public bool isMalformed() {
            return this.type == TYPE_MALFORMED_INPUT;
        }

        /**
         * Returns true if this result is an overflow condition.
         * 
         * @return true if this is an overflow, otherwise false.
         */
        public bool isOverflow() {
            return this.type == TYPE_OVERFLOW;
        }

        /**
         * Returns true if this result represents an unmappable-character error.
         * 
         * @return true if this is an unmappable-character error, otherwise false.
         */
        public bool isUnmappable() {
            return this.type == TYPE_UNMAPPABLE_CHAR;
        }

        /**
         * Gets the length of the erroneous input. The length is only meaningful to
         * a malformed-input error or an unmappble character error.
         * 
         * @return the length, as an integer, of this object's erroneous input.
         * @throws UnsupportedOperationException
         *             if this result is an overflow or underflow.
         */
        public int length() {
            if (this.type == TYPE_MALFORMED_INPUT
                    || this.type == TYPE_UNMAPPABLE_CHAR) {
                return this.lengthJ;
            }
            throw new java.lang.UnsupportedOperationException("The length of the erroneous input is only meaningful to a malformed-input error or an unmappble character error");
        }

        /**
         * Throws an exception corresponding to this coder result.
         * 
         * @throws BufferUnderflowException
         *             in case this is an underflow.
         * @throws BufferOverflowException
         *             in case this is an overflow.
         * @throws UnmappableCharacterException
         *             in case this is an unmappable-character error.
         * @throws MalformedInputException
         *             in case this is a malformed-input error.
         * @throws CharacterCodingException
         *             the default exception.
         */
        public void throwException() //throws CharacterCodingException 
        {
            if (TYPE_UNDERFLOW == this.type) throw new BufferUnderflowException();
            if (TYPE_OVERFLOW == this.type) throw new BufferOverflowException();
            if (TYPE_UNMAPPABLE_CHAR == this.type) throw new UnmappableCharacterException(this.lengthJ);
            if (TYPE_MALFORMED_INPUT == this.type) throw new MalformedInputException(this.lengthJ);
            throw new CharacterCodingException();
            
        }

        /**
         * Returns a text description of this result.
         * 
         * @return a text description of this result.
         */
        public override String ToString() {
            String dsc = null;
            if      (TYPE_UNDERFLOW == this.type) dsc = "UNDERFLOW error"; //$NON-NLS-1$
            else if (TYPE_OVERFLOW == this.type) dsc = "OVERFLOW error"; //$NON-NLS-1$
            else if (TYPE_UNMAPPABLE_CHAR == this.type) dsc = "Unmappable-character error with erroneous input length " + this.lengthJ;
            else if (TYPE_MALFORMED_INPUT == this.type) dsc = "Malformed-input error with erroneous input length " + this.lengthJ;
            else dsc = ""; //$NON-NLS-1$
            return "CoderResult[" + dsc + "]"; //$NON-NLS-1$ //$NON-NLS-2$

        }

    }
}
