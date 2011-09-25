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
     * Decorates another <code>Map</code> to validate that additions
     * match a specified predicate.
     * <p>
     * This map exists to provide validation for the decorated map.
     * It is normally created to decorate an empty map.
     * If an object cannot be added to the map, an IllegalArgumentException is thrown.
     * <p>
     * One usage would be to ensure that no null keys are added to the map.
     * <pre>Map map = PredicatedSet.decorate(new HashMap(), NotNullPredicate.INSTANCE, null);</pre>
     * <p>
     * <strong>Note that PredicatedMap is not synchronized and is not thread-safe.</strong>
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
    public class PredicatedMap
            : AbstractInputCheckedMapDecorator
            , java.io.Serializable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 7412622456128415156L;

        /** The key predicate to use */
        protected readonly Predicate keyPredicate;
        /** The value predicate to use */
        protected readonly Predicate valuePredicate;

        /**
         * Factory method to create a predicated (validating) map.
         * <p>
         * If there are any elements already in the list being decorated, they
         * are validated.
         * 
         * @param map  the map to decorate, must not be null
         * @param keyPredicate  the predicate to validate the keys, null means no check
         * @param valuePredicate  the predicate to validate to values, null means no check
         * @throws IllegalArgumentException if the map is null
         */
        public static java.util.Map<Object, Object> decorate(java.util.Map<Object, Object> map, Predicate keyPredicate, Predicate valuePredicate)
        {
            return new PredicatedMap(map, keyPredicate, valuePredicate);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param map  the map to decorate, must not be null
         * @param keyPredicate  the predicate to validate the keys, null means no check
         * @param valuePredicate  the predicate to validate to values, null means no check
         * @throws IllegalArgumentException if the map is null
         */
        protected internal PredicatedMap(java.util.Map<Object, Object> map, Predicate keyPredicate, Predicate valuePredicate)
            : base(map)
        {
            this.keyPredicate = keyPredicate;
            this.valuePredicate = valuePredicate;

            java.util.Iterator<java.util.MapNS.Entry<Object, Object>> it = map.entrySet().iterator();
            while (it.hasNext())
            {
                java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)it.next();
                Object key = entry.getKey();
                Object value = entry.getValue();
                validate(key, value);
            }
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
        {//throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            map = (java.util.Map<Object, Object>)inJ.readObject();
        }

        //-----------------------------------------------------------------------
        /**
         * Validates a key value pair.
         * 
         * @param key  the key to validate
         * @param value  the value to validate
         * @throws IllegalArgumentException if invalid
         */
        protected virtual void validate(Object key, Object value)
        {
            if (keyPredicate != null && keyPredicate.evaluate(key) == false)
            {
                throw new java.lang.IllegalArgumentException("Cannot add key - Predicate rejected it");
            }
            if (valuePredicate != null && valuePredicate.evaluate(value) == false)
            {
                throw new java.lang.IllegalArgumentException("Cannot add value - Predicate rejected it");
            }
        }

        /**
         * Override to validate an object set into the map via <code>setValue</code>.
         * 
         * @param value  the value to validate
         * @throws IllegalArgumentException if invalid
         * @since Commons Collections 3.1
         */
        protected internal override Object checkSetValue(Object value)
        {
            if (valuePredicate.evaluate(value) == false)
            {
                throw new java.lang.IllegalArgumentException("Cannot set value - Predicate rejected it");
            }
            return value;
        }

        /**
         * Override to only return true when there is a value transformer.
         * 
         * @return true if a value predicate is in use
         * @since Commons Collections 3.1
         */
        protected override bool isSetValueChecking()
        {
            return (valuePredicate != null);
        }

        //-----------------------------------------------------------------------
        public override Object put(Object key, Object value)
        {
            validate(key, value);
            return map.put(key, value);
        }

        public override void putAll(java.util.Map<Object, Object> mapToCopy)
        {
            java.util.Iterator<java.util.MapNS.Entry<Object, Object>> it = mapToCopy.entrySet().iterator();
            while (it.hasNext())
            {
                java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)it.next();
                Object key = entry.getKey();
                Object value = entry.getValue();
                validate(key, value);
            }
            map.putAll(mapToCopy);
        }
    }
}