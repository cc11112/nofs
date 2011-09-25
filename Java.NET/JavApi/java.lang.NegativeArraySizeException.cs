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

    /**
     * Thrown when an attempt is made to create an array with a size of less than
     * zero.
     */
    [Serializable]
    public class NegativeArraySizeException : java.lang.RuntimeException {

        private static readonly long serialVersionUID = -8960118058596991861L;

        /**
         * Constructs a new {@code NegativeArraySizeException} that includes the
         * current stack trace.
         */
        public NegativeArraySizeException() : base(){
        }

        /**
         * Constructs a new {@code NegativeArraySizeException} with the current
         * stack trace and the specified detail message.
         * 
         * @param detailMessage
         *            the detail message for this exception.
         */
        public NegativeArraySizeException(String detailMessage) :base(detailMessage) {
        }
    }
}
