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

namespace biz.ritter.javapi.util.zip
{
    /**
     * {@code DataFormatException} is used to indicate an error in the format of a
     * particular data stream which is to be uncompressed.
     */
    [Serializable]
    public class DataFormatException : java.lang.Exception {

        private static readonly long serialVersionUID = 2219632870893641452L;

        /**
         * Constructs a new {@code DataFormatException} instance.
         */
        public DataFormatException() : base() {
        }

        /**
         * Constructs a new {@code DataFormatException} instance with the specified
         * message.
         *
         * @param detailMessage
         *            the detail message for the exception.
         */
        public DataFormatException(String detailMessage) : base (detailMessage) {
        }
    }
}
