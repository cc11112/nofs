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

namespace org.apache.commons.collections.map
{

    /** 
     * Provides a base decorator that enables additional functionality to be added
     * to a Map via decoration.
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
    public abstract class AbstractSortedMapDecorator
            : AbstractMapDecorator, java.util.SortedMap<Object, Object>
    {

        /**
         * Constructor only used in deserialization, do not use otherwise.
         * @since Commons Collections 3.1
         */
        protected AbstractSortedMapDecorator()
            : base()
        {
        }

        /**
         * Constructor that wraps (not copies).
         *
         * @param map  the map to decorate, must not be null
         * @throws IllegalArgumentException if the collection is null
         */
        public AbstractSortedMapDecorator(java.util.SortedMap<Object, Object> map)
            : base(map)
        {
        }

        /**
         * Gets the map being decorated.
         * 
         * @return the decorated map
         */
        protected virtual java.util.SortedMap<Object, Object> getSortedMap()
        {
            return (java.util.SortedMap<Object, Object>)map;
        }

        //-----------------------------------------------------------------------
        public virtual java.util.Comparator<Object> comparator()
        {
            return getSortedMap().comparator();
        }

        public virtual Object firstKey()
        {
            return getSortedMap().firstKey();
        }

        public virtual java.util.SortedMap<Object, Object> headMap(Object toKey)
        {
            return getSortedMap().headMap(toKey);
        }

        public virtual Object lastKey()
        {
            return getSortedMap().lastKey();
        }

        public virtual java.util.SortedMap<Object, Object> subMap(Object fromKey, Object toKey)
        {
            return getSortedMap().subMap(fromKey, toKey);
        }

        public virtual java.util.SortedMap<Object, Object> tailMap(Object fromKey)
        {
            return getSortedMap().tailMap(fromKey);
        }

    }
}