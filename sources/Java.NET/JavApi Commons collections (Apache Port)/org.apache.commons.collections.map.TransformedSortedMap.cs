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
     * Decorates another <code>SortedMap </code> to transform objects that are added.
     * <p>
     * The Map put methods and Map.Entry setValue method are affected by this class.
     * Thus objects must be removed or searched for using their transformed form.
     * For example, if the transformation converts Strings to Integers, you must
     * use the Integer form to remove objects.
     * <p>
     * <strong>Note that TransformedSortedMap is not synchronized and is not thread-safe.</strong>
     * If you wish to use this map from multiple threads concurrently, you must use
     * appropriate synchronization. The simplest approach is to wrap this map
     * using {@link java.util.Collections#synchronizedSortedMap}. This class may throw 
     * exceptions when accessed by concurrent threads without synchronization.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    [Serializable]
    public class TransformedSortedMap
            : TransformedMap
            , java.util.SortedMap<Object, Object>
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -8751771676410385778L;

        /**
         * Factory method to create a transforming sorted map.
         * <p>
         * If there are any elements already in the map being decorated, they
         * are NOT transformed.
         * Constrast this with {@link #decorateTransform}.
         * 
         * @param map  the map to decorate, must not be null
         * @param keyTransformer  the predicate to validate the keys, null means no transformation
         * @param valueTransformer  the predicate to validate to values, null means no transformation
         * @throws IllegalArgumentException if the map is null
         */
        public static java.util.SortedMap<Object, Object> decorate(java.util.SortedMap<Object, Object> map, Transformer keyTransformer, Transformer valueTransformer)
        {
            return new TransformedSortedMap(map, keyTransformer, valueTransformer);
        }

        /**
         * Factory method to create a transforming sorted map that will transform
         * existing contents of the specified map.
         * <p>
         * If there are any elements already in the map being decorated, they
         * will be transformed by this method.
         * Constrast this with {@link #decorate}.
         * 
         * @param map  the map to decorate, must not be null
         * @param keyTransformer  the transformer to use for key conversion, null means no transformation
         * @param valueTransformer  the transformer to use for value conversion, null means no transformation
         * @throws IllegalArgumentException if map is null
         * @since Commons Collections 3.2
         */
        public static java.util.SortedMap<Object, Object> decorateTransform(java.util.SortedMap<Object, Object> map, Transformer keyTransformer, Transformer valueTransformer)
        {
            TransformedSortedMap decorated = new TransformedSortedMap(map, keyTransformer, valueTransformer);
            if (map.size() > 0)
            {
                java.util.Map<Object, Object> transformed = decorated.transformMap(map);
                decorated.clear();
                decorated.getMap().putAll(transformed);  // avoids double transformation
            }
            return decorated;
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * <p>
         * If there are any elements already in the collection being decorated, they
         * are NOT transformed.</p>
         * 
         * @param map  the map to decorate, must not be null
         * @param keyTransformer  the predicate to validate the keys, null means no transformation
         * @param valueTransformer  the predicate to validate to values, null means no transformation
         * @throws IllegalArgumentException if the map is null
         */
        protected TransformedSortedMap(java.util.SortedMap<Object, Object> map, Transformer keyTransformer, Transformer valueTransformer)
            : base(map, keyTransformer, valueTransformer)
        {
        }

        //-----------------------------------------------------------------------
        /**
         * Gets the map being decorated.
         * 
         * @return the decorated map
         */
        protected java.util.SortedMap<Object, Object> getSortedMap()
        {
            return (java.util.SortedMap<Object, Object>)map;
        }

        //-----------------------------------------------------------------------
        public Object firstKey()
        {
            return getSortedMap().firstKey();
        }

        public Object lastKey()
        {
            return getSortedMap().lastKey();
        }

        public java.util.Comparator<Object> comparator()
        {
            return getSortedMap().comparator();
        }

        public java.util.SortedMap<Object, Object> subMap(Object fromKey, Object toKey)
        {
            java.util.SortedMap<Object, Object> map = getSortedMap().subMap(fromKey, toKey);
            return new TransformedSortedMap(map, keyTransformer, valueTransformer);
        }

        public java.util.SortedMap<Object, Object> headMap(Object toKey)
        {
            java.util.SortedMap<Object, Object> map = getSortedMap().headMap(toKey);
            return new TransformedSortedMap(map, keyTransformer, valueTransformer);
        }

        public java.util.SortedMap<Object, Object> tailMap(Object fromKey)
        {
            java.util.SortedMap<Object, Object> map = getSortedMap().tailMap(fromKey);
            return new TransformedSortedMap(map, keyTransformer, valueTransformer);
        }

    }
}