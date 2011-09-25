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
     * This error is thrown when an unrecoverable ZIP error has occurred.
     */
    [Serializable]
    public class ZipError : java.lang.InternalError {

        private static readonly long serialVersionUID = 853973422266861979L;

        /**
         * the Constructor
         * 
         * @param s
         *            the message
         */
        public ZipError(String s) : base (s) {
        }
    }
}
