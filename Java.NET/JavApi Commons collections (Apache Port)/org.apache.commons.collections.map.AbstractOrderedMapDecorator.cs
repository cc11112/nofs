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

namespace org.apache.commons.collections.map
{

    /** 
     * Provides a base decorator that enables additional functionality to be added
     * to an OrderedMap via decoration.
     * <p>
     * Methods are forwarded directly to the decorated map.
     * <p>
     * This implementation does not perform any special processing with the map views.
     * Instead it simply returns the set/collection from the wrapped map. This may be
     * undesirable, for example if you are trying to write a validating implementation
     * it would provide a loophole around the validation.
     * But, you might want that loophole, so this class is kept simple.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    public abstract class AbstractOrderedMapDecorator
            : AbstractMapDecorator, OrderedMap
    {

        /**
         * Constructor only used in deserialization, do not use otherwise.
         * @since Commons Collections 3.1
         */
        protected AbstractOrderedMapDecorator()
            : base()
        {

        }

        /**
         * Constructor that wraps (not copies).
         *
         * @param map  the map to decorate, must not be null
         * @throws IllegalArgumentException if the collection is null
         */
        public AbstractOrderedMapDecorator(OrderedMap map)
            : base(map)
        {
        }

        /**
         * Gets the map being decorated.
         * 
         * @return the decorated map
         */
        protected virtual OrderedMap getOrderedMap()
        {
            return (OrderedMap)map;
        }

        //-----------------------------------------------------------------------
        public virtual Object firstKey()
        {
            return getOrderedMap().firstKey();
        }

        public virtual Object lastKey()
        {
            return getOrderedMap().lastKey();
        }

        public virtual Object nextKey(Object key)
        {
            return getOrderedMap().nextKey(key);
        }

        public virtual Object previousKey(Object key)
        {
            return getOrderedMap().previousKey(key);
        }

        public virtual MapIterator mapIterator()
        {
            return getOrderedMap().mapIterator();
        }

        public virtual OrderedMapIterator orderedMapIterator()
        {
            return getOrderedMap().orderedMapIterator();
        }

    }
}