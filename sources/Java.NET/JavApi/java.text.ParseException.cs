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

namespace biz.ritter.javapi.text
{

    /**
     * Thrown when the string being parsed is not in the correct form.
     */
    [Serializable]
    public class ParseException : java.lang.Exception {

        private static readonly long serialVersionUID = 2703218443322787634L;

        private int errorOffset;

        /**
         * Constructs a new instance of this class with its stack trace, detail
         * message and the location of the error filled in.
         * 
         * @param detailMessage
         *            the detail message for this exception.
         * @param location
         *            the index at which the parse exception occurred.
         */
        public ParseException(String detailMessage, int location) : base (detailMessage){
            errorOffset = location;
        }

        /**
         * Returns the index at which this parse exception occurred.
         * 
         * @return the location of this exception in the parsed string.
         */
        public int getErrorOffset() {
            return errorOffset;
        }
    }
}
