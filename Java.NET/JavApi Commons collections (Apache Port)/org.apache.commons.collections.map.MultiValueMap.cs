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
using org.apache.commons.collections.iterators;

namespace org.apache.commons.collections.map
{

    /**
     * A MultiValueMap decorates another map, allowing it to have
     * more than one value for a key.
     * <p>
     * A <code>MultiMap</code> is a Map with slightly different semantics.
     * Putting a value into the map will add the value to a Collection at that key.
     * Getting a value will return a Collection, holding all the values put to that key.
     * <p>
     * This implementation is a decorator, allowing any Map implementation
     * to be used as the base.
     * <p>
     * In addition, this implementation allows the type of collection used
     * for the values to be controlled. By default, an <code>ArrayList</code>
     * is used, however a <code>Class</code> to instantiate may be specified,
     * or a factory that returns a <code>Collection</code> instance.
     * <p>
     * <strong>Note that MultiValueMap is not synchronized and is not thread-safe.</strong>
     * If you wish to use this map from multiple threads concurrently, you must use
     * appropriate synchronization. This class may throw exceptions when accessed
     * by concurrent threads without synchronization.
     *
     * @author James Carman
     * @author Christopher Berry
     * @author James Strachan
     * @author Steve Downey
     * @author Stephen Colebourne
     * @author Julien Buret
     * @author Serhiy Yevtushenko
     * @version $Revision$ $Date$
     * @since Commons Collections 3.2
     */
    public class MultiValueMap : AbstractMapDecorator, MultiMap
    {

        /** The factory for creating value collections. */
        private readonly Factory collectionFactory;
        /** The cached values. */
        [NonSerialized]
        private java.util.Collection<Object> valuesJ;

        /**
         * Creates a map which wraps the given map and
         * maps keys to ArrayLists.
         *
         * @param map  the map to wrap
         */
        public static MultiValueMap decorate(java.util.Map<Object, Object> map)
        {
            return new MultiValueMap(map, new ReflectionFactory(new java.lang.Class(typeof(java.util.ArrayList<Object>))));
        }

        /**
         * Creates a map which decorates the given <code>map</code> and
         * maps keys to collections of type <code>collectionClass</code>.
         *
         * @param map  the map to wrap
         * @param collectionClass  the type of the collection class
         */
        public static MultiValueMap decorate(java.util.Map<Object, Object> map, java.lang.Class collectionClass)
        {
            return new MultiValueMap(map, new ReflectionFactory(collectionClass));
        }

        /**
         * Creates a map which decorates the given <code>map</code> and
         * creates the value collections using the supplied <code>collectionFactory</code>.
         *
         * @param map  the map to decorate
         * @param collectionFactory  the collection factory (must return a Collection object).
         */
        public static MultiValueMap decorate(java.util.Map<Object, Object> map, Factory collectionFactory)
        {
            return new MultiValueMap(map, collectionFactory);
        }

        //-----------------------------------------------------------------------
        /**
         * Creates a MultiValueMap based on a <code>HashMap</code> and
         * storing the multiple values in an <code>ArrayList</code>.
         */
        public MultiValueMap() :
            this(new java.util.HashMap<Object, Object>(), new ReflectionFactory(new java.lang.Class(typeof(java.util.ArrayList<Object>))))
        {
        }

        /**
         * Creates a MultiValueMap which decorates the given <code>map</code> and
         * creates the value collections using the supplied <code>collectionFactory</code>.
         *
         * @param map  the map to decorate
         * @param collectionFactory  the collection factory which must return a Collection instance
         */
        protected MultiValueMap(java.util.Map<Object, Object> map, Factory collectionFactory) :
            base(map)
        {
            if (collectionFactory == null)
            {
                throw new java.lang.IllegalArgumentException("The factory must not be null");
            }
            this.collectionFactory = collectionFactory;
        }

        //-----------------------------------------------------------------------
        /**
         * Clear the map.
         */
        public override void clear()
        {
            // If you believe that you have GC issues here, try uncommenting this code
            //        Set pairs = getMap().entrySet();
            //        Iterator pairsIterator = pairs.iterator();
            //        while (pairsIterator.hasNext()) {
            //            Map.Entry keyValuePair = (Map.Entry) pairsIterator.next();
            //            Collection coll = (Collection) keyValuePair.getValue();
            //            coll.clear();
            //        }
            getMap().clear();
        }

        /**
         * Removes a specific value from map.
         * <p>
         * The item is removed from the collection mapped to the specified key.
         * Other values attached to that key are unaffected.
         * <p>
         * If the last value for a key is removed, <code>null</code> will be returned
         * from a subsequant <code>get(key)</code>.
         *
         * @param key  the key to remove from
         * @param value the value to remove
         * @return the value removed (which was passed in), null if nothing removed
         */
        public virtual Object remove(Object key, Object value)
        {
            java.util.Collection<Object> valuesForKey = getCollection(key);
            if (valuesForKey == null)
            {
                return null;
            }
            bool removed = valuesForKey.remove(value);
            if (removed == false)
            {
                return null;
            }
            if (valuesForKey.isEmpty())
            {
                remove(key);
            }
            return value;
        }

        /**
         * Checks whether the map contains the value specified.
         * <p>
         * This checks all collections against all keys for the value, and thus could be slow.
         *
         * @param value  the value to search for
         * @return true if the map contains the value
         */
        public override bool containsValue(Object value)
        {
            java.util.Set<java.util.MapNS.Entry<Object, Object>> pairs = getMap().entrySet();
            if (pairs == null)
            {
                return false;
            }
            java.util.Iterator<java.util.MapNS.Entry<Object, Object>> pairsIterator = pairs.iterator();
            while (pairsIterator.hasNext())
            {
                java.util.MapNS.Entry<Object, Object> keyValuePair = pairsIterator.next();
                java.util.Collection<Object> coll = (java.util.Collection<Object>)keyValuePair.getValue();
                if (coll.contains(value))
                {
                    return true;
                }
            }
            return false;
        }

        /**
         * Adds the value to the collection associated with the specified key.
         * <p>
         * Unlike a normal <code>Map</code> the previous value is not replaced.
         * Instead the new value is added to the collection stored against the key.
         *
         * @param key  the key to store against
         * @param value  the value to add to the collection at the key
         * @return the value added if the map changed and null if the map did not change
         */
        public override Object put(Object key, Object value)
        {
            bool result = false;
            java.util.Collection<Object> coll = getCollection(key);
            if (coll == null)
            {
                coll = createCollection(1);
                result = coll.add(value);
                if (coll.size() > 0)
                {
                    // only add if non-zero size to maintain class state
                    getMap().put(key, coll);
                    result = false;
                }
            }
            else
            {
                result = coll.add(value);
            }
            return (result ? value : null);
        }

        /**
         * Override superclass to ensure that MultiMap instances are
         * correctly handled.
         * <p>
         * If you call this method with a normal map, each entry is
         * added using <code>put(Object,Object)</code>.
         * If you call this method with a multi map, each entry is
         * added using <code>putAll(Object,Collection)</code>.
         *
         * @param map  the map to copy (either a normal or multi map)
         */
        public override void putAll(java.util.Map<Object, Object> map)
        {
            if (map is MultiMap)
            {
                for (java.util.Iterator<java.util.MapNS.Entry<Object, Object>> it = map.entrySet().iterator(); it.hasNext(); )
                {
                    java.util.MapNS.Entry<Object, Object> entry = it.next();
                    java.util.Collection<Object> coll = (java.util.Collection<Object>)entry.getValue();
                    putAll(entry.getKey(), coll);
                }
            }
            else
            {
                for (java.util.Iterator<java.util.MapNS.Entry<Object, Object>> it = map.entrySet().iterator(); it.hasNext(); )
                {
                    java.util.MapNS.Entry<Object, Object> entry = it.next();
                    put(entry.getKey(), entry.getValue());
                }
            }
        }

        /**
         * Gets a collection containing all the values in the map.
         * <p>
         * This returns a collection containing the combination of values from all keys.
         *
         * @return a collection view of the values contained in this map
         */
        public override java.util.Collection<Object> values()
        {
            java.util.Collection<Object> vs = valuesJ;
            return (vs != null ? vs : (valuesJ = new Values(this)));
        }

        /**
         * Checks whether the collection at the specified key contains the value.
         *
         * @param value  the value to search for
         * @return true if the map contains the value
         */
        public virtual bool containsValue(Object key, Object value)
        {
            java.util.Collection<Object> coll = getCollection(key);
            if (coll == null)
            {
                return false;
            }
            return coll.contains(value);
        }

        /**
         * Gets the collection mapped to the specified key.
         * This method is a convenience method to typecast the result of <code>get(key)</code>.
         *
         * @param key  the key to retrieve
         * @return the collection mapped to the key, null if no mapping
         */
        public java.util.Collection<Object> getCollection(Object key)
        {
            return (java.util.Collection<Object>)getMap().get(key);
        }

        /**
         * Gets the size of the collection mapped to the specified key.
         *
         * @param key  the key to get size for
         * @return the size of the collection at the key, zero if key not in map
         */
        public int size(Object key)
        {
            java.util.Collection<Object> coll = getCollection(key);
            if (coll == null)
            {
                return 0;
            }
            return coll.size();
        }

        /**
         * Adds a collection of values to the collection associated with
         * the specified key.
         *
         * @param key  the key to store against
         * @param values  the values to add to the collection at the key, null ignored
         * @return true if this map changed
         */
        public virtual bool putAll(Object key, java.util.Collection<Object> values)
        {
            if (values == null || values.size() == 0)
            {
                return false;
            }
            java.util.Collection<Object> coll = getCollection(key);
            if (coll == null)
            {
                coll = createCollection(values.size());
                bool result = coll.addAll(values);
                if (coll.size() > 0)
                {
                    // only add if non-zero size to maintain class state
                    getMap().put(key, coll);
                    result = false;
                }
                return result;
            }
            else
            {
                return coll.addAll(values);
            }
        }

        /**
         * Gets an iterator for the collection mapped to the specified key.
         *
         * @param key  the key to get an iterator for
         * @return the iterator of the collection at the key, empty iterator if key not in map
         */
        public virtual java.util.Iterator<Object> iterator(Object key)
        {
            if (!containsKey(key))
            {
                return EmptyIterator.INSTANCE;
            }
            else
            {
                return new ValuesIterator(key, this);
            }
        }

        /**
         * Gets the total size of the map by counting all the values.
         *
         * @return the total size of the map counting all values
         */
        public virtual int totalSize()
        {
            int total = 0;
            java.util.Collection<Object> values = getMap().values();
            for (java.util.Iterator<Object> it = values.iterator(); it.hasNext(); )
            {
                java.util.Collection<Object> coll = (java.util.Collection<Object>)it.next();
                total += coll.size();
            }
            return total;
        }

        /**
         * Creates a new instance of the map value Collection container
         * using the factory.
         * <p>
         * This method can be overridden to perform your own processing
         * instead of using the factory.
         *
         * @param size  the collection size that is about to be added
         * @return the new collection
         */
        protected virtual java.util.Collection<Object> createCollection(int size)
        {
            return (java.util.Collection<Object>)collectionFactory.create();
        }

        //-----------------------------------------------------------------------
        /**
         * Inner class that provides the values view.
         */
        private class Values : java.util.AbstractCollection<Object>
        {
            private readonly MultiValueMap root;

            public Values(MultiValueMap root)
            {
                this.root = root;
            }

            public override java.util.Iterator<Object> iterator()
            {
                IteratorChain chain = new IteratorChain();
                for (java.util.Iterator<Object> it = root.keySet().iterator(); it.hasNext(); )
                {
                    chain.addIterator(new ValuesIterator(it.next(), root));
                }
                return chain;
            }

            public override int size()
            {
                return root.totalSize();
            }

            public override void clear()
            {
                root.clear();
            }
        }

        /**
         * Inner class that provides the values iterator.
         */
        private class ValuesIterator : java.util.Iterator<Object>
        {
            private readonly MultiValueMap root;
            private readonly Object key;
            private readonly java.util.Collection<Object> values;
            private readonly java.util.Iterator<Object> iterator;

            public ValuesIterator(Object key, MultiValueMap root)
            {
                this.root = root;
                this.key = key;
                this.values = root.getCollection(key);
                this.iterator = values.iterator();
            }

            public void remove()
            {
                iterator.remove();
                if (values.isEmpty())
                {
                    root.remove(key);
                }
            }

            public bool hasNext()
            {
                return iterator.hasNext();
            }

            public Object next()
            {
                return iterator.next();
            }
        }

        /**
         * Inner class that provides a simple reflection factory.
         */
        private class ReflectionFactory : Factory
        {
            private readonly java.lang.Class clazz;

            public ReflectionFactory(java.lang.Class clazz)
            {
                this.clazz = clazz;
            }

            public Object create()
            {
                try
                {
                    return clazz.newInstance();
                }
                catch (java.lang.Exception ex)
                {
                    throw new FunctorException("Cannot instantiate class: " + clazz, ex);
                }
            }
        }

    }
}