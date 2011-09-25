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

namespace biz.ritter.javapi.nio
{
    /**
     * A <code>BufferUnderflowException</code> is thrown when elements are read
     * from a buffer but there are not enough remaining elements in the buffer.
     */
    [Serializable]
    public class BufferUnderflowException : java.lang.RuntimeException {

        private static readonly long serialVersionUID = -1713313658691622206L;

        /**
         * Constructs a <code>BufferUnderflowException</code>.
         */
        public BufferUnderflowException() : base (){
        }
    }
}
