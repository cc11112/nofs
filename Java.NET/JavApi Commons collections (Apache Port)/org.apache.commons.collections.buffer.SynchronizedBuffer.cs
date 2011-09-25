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
using org.apache.commons.collections;
using org.apache.commons.collections.collection;

namespace org.apache.commons.collections.buffer
{

    /**
     * Decorates another <code>Buffer</code> to synchronize its behaviour
     * for a multi-threaded environment.
     * <p>
     * Methods are synchronized, then forwarded to the decorated buffer.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    [Serializable]
    public class SynchronizedBuffer : SynchronizedCollection, Buffer
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -6859936183953626253L;

        /**
         * Factory method to create a synchronized buffer.
         * 
         * @param buffer  the buffer to decorate, must not be null
         * @return a new synchronized Buffer
         * @throws IllegalArgumentException if buffer is null
         */
        public static Buffer decorate(Buffer buffer)
        {
            return new SynchronizedBuffer(buffer);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param buffer  the buffer to decorate, must not be null
         * @throws IllegalArgumentException if the buffer is null
         */
        protected SynchronizedBuffer(Buffer buffer)
            : base(buffer)
        {
        }

        /**
         * Constructor that wraps (not copies).
         * 
         * @param buffer  the buffer to decorate, must not be null
         * @param lock  the lock object to use, must not be null
         * @throws IllegalArgumentException if the buffer is null
         */
        protected SynchronizedBuffer(Buffer buffer, Object lockJ)
            : base(buffer, lockJ)
        {
        }

        /**
         * Gets the buffer being decorated.
         * 
         * @return the decorated buffer
         */
        protected virtual Buffer getBuffer()
        {
            return (Buffer)collection;
        }

        //-----------------------------------------------------------------------
        public virtual Object get()
        {
            lock (lockJ)
            {
                return getBuffer().get();
            }
        }

        public virtual Object remove()
        {
            lock (lockJ)
            {
                return getBuffer().remove();
            }
        }

    }
}
