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
using org.apache.commons.collections.set;
using org.apache.commons.collections.map;
using org.apache.commons.collections.collection;
using org.apache.commons.collections.iterators;

namespace org.apache.commons.collections.bidimap
{
    /**
     * Decorates another <code>OrderedBidiMap</code> to ensure it can't be altered.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    public sealed class UnmodifiableOrderedBidiMap
            : AbstractOrderedBidiMapDecorator, Unmodifiable
    {

        /** The inverse unmodifiable map */
        private UnmodifiableOrderedBidiMap inverse;

        /**
         * Factory method to create an unmodifiable map.
         * <p>
         * If the map passed in is already unmodifiable, it is returned.
         * 
         * @param map  the map to decorate, must not be null
         * @return an unmodifiable OrderedBidiMap
         * @throws IllegalArgumentException if map is null
         */
        public static OrderedBidiMap decorate(OrderedBidiMap map)
        {
            if (map is Unmodifiable)
            {
                return map;
            }
            return new UnmodifiableOrderedBidiMap(map);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param map  the map to decorate, must not be null
         * @throws IllegalArgumentException if map is null
         */
        private UnmodifiableOrderedBidiMap(OrderedBidiMap map)
            : base(map)
        {
        }

        //-----------------------------------------------------------------------
        public override void clear()
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override Object put(Object key, Object value)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override void putAll(java.util.Map<Object, Object> mapToCopy)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override Object remove(Object key)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public new java.util.Set<Object> entrySet()
        {
            java.util.Set<Object> set = (java.util.Set<Object>)base.entrySet();
            return UnmodifiableEntrySet.decorate(set);
        }

        public override java.util.Set<Object> keySet()
        {
            java.util.Set<Object> set = base.keySet();
            return UnmodifiableSet.decorate(set);
        }

        public override java.util.Collection<Object> values()
        {
            java.util.Collection<Object> coll = base.values();
            return UnmodifiableCollection.decorate(coll);
        }

        //-----------------------------------------------------------------------
        public override Object removeValue(Object value)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override MapIterator mapIterator()
        {
            return orderedMapIterator();
        }

        public override BidiMap inverseBidiMap()
        {
            return inverseOrderedBidiMap();
        }

        //-----------------------------------------------------------------------
        public override OrderedMapIterator orderedMapIterator()
        {
            OrderedMapIterator it = getOrderedBidiMap().orderedMapIterator();
            return UnmodifiableOrderedMapIterator.decorate(it);
        }

        public override OrderedBidiMap inverseOrderedBidiMap()
        {
            if (inverse == null)
            {
                inverse = new UnmodifiableOrderedBidiMap(getOrderedBidiMap().inverseOrderedBidiMap());
                inverse.inverse = this;
            }
            return inverse;
        }

    }
}