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
using org.apache.commons.collections.set;

namespace org.apache.commons.collections.map
{

    /**
     * Decorates another <code>SortedMap</code> to ensure it can't be altered.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    public sealed class UnmodifiableSortedMap
            : AbstractSortedMapDecorator
            , Unmodifiable, java.io.Serializable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 5805344239827376360L;

        /**
         * Factory method to create an unmodifiable sorted map.
         * 
         * @param map  the map to decorate, must not be null
         * @throws IllegalArgumentException if map is null
         */
        public static java.util.SortedMap<Object, Object> decorate(java.util.SortedMap<Object, Object> map)
        {
            if (map is Unmodifiable)
            {
                return map;
            }
            return new UnmodifiableSortedMap(map);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param map  the map to decorate, must not be null
         * @throws IllegalArgumentException if map is null
         */
        private UnmodifiableSortedMap(java.util.SortedMap<Object, Object> map)
            : base(map)
        {
        }

        //-----------------------------------------------------------------------
        /**
         * Write the map out using a custom routine.
         * 
         * @param out  the output stream
         * @throws IOException
         * @since Commons Collections 3.1
         */
        private void writeObject(java.io.ObjectOutputStream outJ)
        {// throws IOException {
            outJ.defaultWriteObject();
            outJ.writeObject(map);
        }

        /**
         * Read the map in using a custom routine.
         * 
         * @param in  the input stream
         * @throws IOException
         * @throws ClassNotFoundException
         * @since Commons Collections 3.1
         */
        private void readObject(java.io.ObjectInputStream inJ)
        {// throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            map = (java.util.Map<Object, Object>)inJ.readObject();
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

        public override java.util.Set<java.util.MapNS.Entry<Object, Object>> entrySet()
        {
            java.util.Set<java.util.MapNS.Entry<Object, Object>> set = base.entrySet();
            return (java.util.Set<java.util.MapNS.Entry<Object, Object>>)UnmodifiableEntrySet.decorate((java.util.Set<Object>)set);
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
        public override Object firstKey()
        {
            return getSortedMap().firstKey();
        }

        public override Object lastKey()
        {
            return getSortedMap().lastKey();
        }

        public override java.util.Comparator<Object> comparator()
        {
            return getSortedMap().comparator();
        }

        public override java.util.SortedMap<Object, Object> subMap(Object fromKey, Object toKey)
        {
            java.util.SortedMap<Object, Object> map = getSortedMap().subMap(fromKey, toKey);
            return new UnmodifiableSortedMap(map);
        }

        public override java.util.SortedMap<Object, Object> headMap(Object toKey)
        {
            java.util.SortedMap<Object, Object> map = getSortedMap().headMap(toKey);
            return new UnmodifiableSortedMap(map);
        }

        public override java.util.SortedMap<Object, Object> tailMap(Object fromKey)
        {
            java.util.SortedMap<Object, Object> map = getSortedMap().tailMap(fromKey);
            return new UnmodifiableSortedMap(map);
        }
    }
}