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

namespace biz.ritter.javapi.nio.charset
{

    /**
     * A {@code CoderMalfunctionError} is thrown when the encoder/decoder is
     * malfunctioning.
     */
    public class CoderMalfunctionError : java.lang.Error {

        /*
         * This constant is used during deserialization to check the version
         * which created the serialized object.
         */
        private static readonly long serialVersionUID = -1151412348057794301L;

        /**
         * Constructs a new {@code CoderMalfunctionError}.
         * 
         * @param ex
         *            the original exception thrown by the encoder/decoder.
         */
        public CoderMalfunctionError(java.lang.Exception ex) : base (ex){
        }
    }
}
