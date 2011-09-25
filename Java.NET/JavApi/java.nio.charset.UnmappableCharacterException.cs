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

namespace biz.ritter.javapi.nio.charset
{
    /**
     * An {@code UnmappableCharacterException} is thrown when an unmappable
     * character for the given charset is encountered.
     */
    public class UnmappableCharacterException : CharacterCodingException {

        /*
         * This constant is used during deserialization to check the version
         * which created the serialized object.
         */
        private static readonly long serialVersionUID = -7026962371537706123L;

        // The length of the unmappable character
        private int inputLength;

        /**
         * Constructs a new {@code UnmappableCharacterException}.
         * 
         * @param length
         *            the length of the unmappable character.
         */
        public UnmappableCharacterException(int length) {
            this.inputLength = length;
        }

        /**
         * Gets the length of the unmappable character.
         * 
         * @return the length of the unmappable character.
         */
        public int getInputLength() {
            return this.inputLength;
        }

        /**
         * Gets a message describing this exception.
         * 
         * @return a message describing this exception.
         */
        
        public override String getMessage() {
            // niochar.0A=The unmappable character length is {0}.
            return "The unmappable character length is "+ this.inputLength+"."; //$NON-NLS-1$
        }
    }
}
