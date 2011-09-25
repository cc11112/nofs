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

namespace org.apache.commons.collections.keyvalue
{

    /**
     * Provides a base decorator that allows additional functionality to be
     * added to a {@link java.util.Map.Entry Map.Entry}.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    public abstract class AbstractMapEntryDecorator : java.util.MapNS.Entry<Object, Object>, KeyValue
    {

        /** The <code>Map.Entry</code> to decorate */
        protected readonly java.util.MapNS.Entry<Object, Object> entry;

        /**
         * Constructor that wraps (not copies).
         *
         * @param entry  the <code>Map.Entry</code> to decorate, must not be null
         * @throws IllegalArgumentException if the collection is null
         */
        public AbstractMapEntryDecorator(java.util.MapNS.Entry<Object, Object> entry)
        {
            if (entry == null)
            {
                throw new java.lang.IllegalArgumentException("Map Entry must not be null");
            }
            this.entry = entry;
        }

        /**
         * Gets the map being decorated.
         * 
         * @return the decorated map
         */
        protected virtual java.util.MapNS.Entry<Object, Object> getMapEntry()
        {
            return entry;
        }

        //-----------------------------------------------------------------------
        public virtual Object getKey()
        {
            return entry.getKey();
        }

        public virtual Object getValue()
        {
            return entry.getValue();
        }

        public virtual Object setValue(Object obj)
        {
            return entry.setValue(obj);
        }

        public override bool Equals(Object obj)
        {
            if (obj == this)
            {
                return true;
            }
            return entry.equals(obj);
        }

        public override int GetHashCode()
        {
            return entry.GetHashCode();
        }

        public override String ToString()
        {
            return entry.toString();
        }

    }
}