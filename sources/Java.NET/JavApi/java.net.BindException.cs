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

namespace biz.ritter.javapi.net
{

    /**
     * A {@code BindException} is thrown when a process cannot bind a local
     * address/port, either because it is already bound or reserved by the OS.
     */
    [Serializable]
    public class BindException : SocketException {

        private static readonly long serialVersionUID = -5945005768251722951L;

        /**
         * Constructs a new instance with its walkback filled in.
         */
        public BindException() : base (){
        }

        /**
         * Constructs a new instance with its walkback and message filled in.
         * 
         * @param detailMessage
         *            detail message of the exception.
         */
        public BindException(String detailMessage) : base (detailMessage) {
        }
    }
}
