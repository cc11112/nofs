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
using org.apache.commons.collections.functors;

namespace org.apache.commons.collections.map
{

    /**
     * Decorates another <code>Map</code> to create objects in the map on demand.
     * <p>
     * When the {@link #get(Object)} method is called with a key that does not
     * exist in the map, the factory is used to create the object. The created
     * object will be added to the map using the requested key.
     * <p>
     * For instance:
     * <pre>
     * Factory factory = new Factory() {
     *     public Object create() {
     *         return new Date();
     *     }
     * }
     * Map lazy = Lazy.map(new HashMap(), factory);
     * Object obj = lazy.get("NOW");
     * </pre>
     *
     * After the above code is executed, <code>obj</code> will contain
     * a new <code>Date</code> instance. Furthermore, that <code>Date</code>
     * instance is mapped to the "NOW" key in the map.
     * <p>
     * <strong>Note that LazyMap is not synchronized and is not thread-safe.</strong>
     * If you wish to use this map from multiple threads concurrently, you must use
     * appropriate synchronization. The simplest approach is to wrap this map
     * using {@link java.util.Collections#synchronizedMap(Map)}. This class may throw 
     * exceptions when accessed by concurrent threads without synchronization.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     * @author Paul Jack
     */
    [Serializable]
    public class LazyMap
            : AbstractMapDecorator,
            java.util.Map<Object, Object>, java.io.Serializable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 7990956402564206740L;

        /** The factory to use to construct elements */
        protected readonly Transformer factory;

        /**
         * Factory method to create a lazily instantiated map.
         * 
         * @param map  the map to decorate, must not be null
         * @param factory  the factory to use, must not be null
         * @throws IllegalArgumentException if map or factory is null
         */
        public static java.util.Map<Object, Object> decorate(java.util.Map<Object, Object> map, Factory factory)
        {
            return new LazyMap(map, factory);
        }

        /**
         * Factory method to create a lazily instantiated map.
         * 
         * @param map  the map to decorate, must not be null
         * @param factory  the factory to use, must not be null
         * @throws IllegalArgumentException if map or factory is null
         */
        public static java.util.Map<Object, Object> decorate(java.util.Map<Object, Object> map, Transformer factory)
        {
            return new LazyMap(map, factory);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param map  the map to decorate, must not be null
         * @param factory  the factory to use, must not be null
         * @throws IllegalArgumentException if map or factory is null
         */
        protected LazyMap(java.util.Map<Object, Object> map, Factory factory)
            : base(map)
        {
            if (factory == null)
            {
                throw new java.lang.IllegalArgumentException("Factory must not be null");
            }
            this.factory = FactoryTransformer.getInstance(factory);
        }

        /**
         * Constructor that wraps (not copies).
         * 
         * @param map  the map to decorate, must not be null
         * @param factory  the factory to use, must not be null
         * @throws IllegalArgumentException if map or factory is null
         */
        protected LazyMap(java.util.Map<Object, Object> map, Transformer factory)
            : base(map)
        {
            if (factory == null)
            {
                throw new java.lang.IllegalArgumentException("Factory must not be null");
            }
            this.factory = factory;
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
        {//throws IOException {
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
        {//throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            map = (java.util.Map<Object, Object>)inJ.readObject();
        }

        //-----------------------------------------------------------------------
        public override Object get(Object key)
        {
            // create value for key if key is not currently in the map
            if (map.containsKey(key) == false)
            {
                Object value = factory.transform(key);
                map.put(key, value);
                return value;
            }
            return map.get(key);
        }

        // no need to wrap keySet, entrySet or values as they are views of
        // existing map entries - you can't do a map-style get on them.
    }
}