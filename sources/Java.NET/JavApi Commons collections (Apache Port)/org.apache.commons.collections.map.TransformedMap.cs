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
     * Decorates another <code>Map</code> to transform objects that are added.
     * <p>
     * The Map put methods and Map.Entry setValue method are affected by this class.
     * Thus objects must be removed or searched for using their transformed form.
     * For example, if the transformation converts Strings to Integers, you must
     * use the Integer form to remove objects.
     * <p>
     * <strong>Note that TransformedMap is not synchronized and is not thread-safe.</strong>
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
     */
    [Serializable]
    public class TransformedMap
            : AbstractInputCheckedMapDecorator
            , java.io.Serializable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 7023152376788900464L;

        /** The transformer to use for the key */
        protected readonly Transformer keyTransformer;
        /** The transformer to use for the value */
        protected readonly Transformer valueTransformer;

        /**
         * Factory method to create a transforming map.
         * <p>
         * If there are any elements already in the map being decorated, they
         * are NOT transformed.
         * Constrast this with {@link #decorateTransform}.
         * 
         * @param map  the map to decorate, must not be null
         * @param keyTransformer  the transformer to use for key conversion, null means no transformation
         * @param valueTransformer  the transformer to use for value conversion, null means no transformation
         * @throws IllegalArgumentException if map is null
         */
        public static java.util.Map<Object, Object> decorate(java.util.Map<Object, Object> map, Transformer keyTransformer, Transformer valueTransformer)
        {
            return new TransformedMap(map, keyTransformer, valueTransformer);
        }

        /**
         * Factory method to create a transforming map that will transform
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
        public static java.util.Map<Object, Object> decorateTransform(java.util.Map<Object, Object> map, Transformer keyTransformer, Transformer valueTransformer)
        {
            TransformedMap decorated = new TransformedMap(map, keyTransformer, valueTransformer);
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
         * are NOT transformed.
         * 
         * @param map  the map to decorate, must not be null
         * @param keyTransformer  the transformer to use for key conversion, null means no conversion
         * @param valueTransformer  the transformer to use for value conversion, null means no conversion
         * @throws IllegalArgumentException if map is null
         */
        protected TransformedMap(java.util.Map<Object, Object> map, Transformer keyTransformer, Transformer valueTransformer)
            : base(map)
        {
            this.keyTransformer = keyTransformer;
            this.valueTransformer = valueTransformer;
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
        /**
         * Transforms a key.
         * <p>
         * The transformer itself may throw an exception if necessary.
         * 
         * @param object  the object to transform
         * @throws the transformed object
         */
        protected virtual Object transformKey(Object obj)
        {
            if (keyTransformer == null)
            {
                return obj;
            }
            return keyTransformer.transform(obj);
        }

        /**
         * Transforms a value.
         * <p>
         * The transformer itself may throw an exception if necessary.
         * 
         * @param object  the object to transform
         * @throws the transformed object
         */
        protected virtual Object transformValue(Object obj)
        {
            if (valueTransformer == null)
            {
                return obj;
            }
            return valueTransformer.transform(obj);
        }

        /**
         * Transforms a map.
         * <p>
         * The transformer itself may throw an exception if necessary.
         * 
         * @param map  the map to transform
         * @throws the transformed object
         */
        protected virtual java.util.Map<Object, Object> transformMap(java.util.Map<Object, Object> map)
        {
            if (map.isEmpty())
            {
                return map;
            }
            java.util.Map<Object, Object> result = new LinkedMap(map.size());
            for (java.util.Iterator<java.util.MapNS.Entry<Object, Object>> it = map.entrySet().iterator(); it.hasNext(); )
            {
                java.util.MapNS.Entry<Object, Object> entry = it.next();
                result.put(transformKey(entry.getKey()), transformValue(entry.getValue()));
            }
            return result;
        }

        /**
         * Override to transform the value when using <code>setValue</code>.
         * 
         * @param value  the value to transform
         * @return the transformed value
         * @since Commons Collections 3.1
         */
        protected internal override Object checkSetValue(Object value)
        {
            return valueTransformer.transform(value);
        }

        /**
         * Override to only return true when there is a value transformer.
         * 
         * @return true if a value transformer is in use
         * @since Commons Collections 3.1
         */
        protected override bool isSetValueChecking()
        {
            return (valueTransformer != null);
        }

        //-----------------------------------------------------------------------
        public override Object put(Object key, Object value)
        {
            key = transformKey(key);
            value = transformValue(value);
            return getMap().put(key, value);
        }

        public override void putAll(java.util.Map<Object, Object> mapToCopy)
        {
            mapToCopy = transformMap(mapToCopy);
            getMap().putAll(mapToCopy);
        }

    }
}