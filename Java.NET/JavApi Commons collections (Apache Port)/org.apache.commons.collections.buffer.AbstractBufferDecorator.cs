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
     * Decorates another <code>Buffer</code> to provide additional behaviour.
     * <p>
     * Methods are forwarded directly to the decorated buffer.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    public abstract class AbstractBufferDecorator : AbstractCollectionDecorator, Buffer
    {

        /**
         * Constructor only used in deserialization, do not use otherwise.
         * @since Commons Collections 3.1
         */
        protected AbstractBufferDecorator()
            : base()
        {
        }

        /**
         * Constructor that wraps (not copies).
         * 
         * @param buffer  the buffer to decorate, must not be null
         * @throws IllegalArgumentException if list is null
         */
        protected AbstractBufferDecorator(Buffer buffer)
            : base(buffer)
        {
        }

        /**
         * Gets the buffer being decorated.
         * 
         * @return the decorated buffer
         */
        protected virtual Buffer getBuffer()
        {
            return (Buffer)getCollection();
        }

        //-----------------------------------------------------------------------
        public virtual Object get()
        {
            return getBuffer().get();
        }

        public virtual Object remove()
        {
            return getBuffer().remove();
        }

    }
}