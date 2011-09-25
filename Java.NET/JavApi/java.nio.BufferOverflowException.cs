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

    /// <summary>
    /// A <code>BufferOverflowException</code> is thrown when elements are written
    /// to a buffer but there is not enough remaining space in the buffer.
    /// </summary>
    /// <remarks>Class is ported from Apache Harmony project.</remarks>
    [Serializable]
    public class BufferOverflowException : java.lang.RuntimeException {

        private static readonly long serialVersionUID = -5484897634319144535L;

        /// <summary>
        /// Constructs a <code>BufferOverflowException</code>.
        /// </summary>
        public BufferOverflowException() : base (){}
    }
}
