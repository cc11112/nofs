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
     * This implementation does not perform any special processing with
     * {@link #entrySet()}, {@link #keySet()} or {@link #values()}. Instead
     * it simply returns the set/collection from the wrapped map. This may be
     * undesirable, for example if you are trying to write a validating
     * implementation it would provide a loophole around the validation.
     * But, you might want that loophole, so this class is kept simple.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Daniel Rall
     * @author Stephen Colebourne
     */
    public abstract class AbstractMapDecorator : java.util.Map<Object, Object>
    {

        /** The map to decorate */
        [NonSerialized]
        protected java.util.Map<Object, Object> map;

        /**
         * Constructor only used in deserialization, do not use otherwise.
         * @since Commons Collections 3.1
         */
        protected AbstractMapDecorator()
            : base()
        {
        }

        /**
         * Constructor that wraps (not copies).
         *
         * @param map  the map to decorate, must not be null
         * @throws IllegalArgumentException if the collection is null
         */
        public AbstractMapDecorator(java.util.Map<Object, Object> map)
        {
            if (map == null)
            {
                throw new java.lang.IllegalArgumentException("Map must not be null");
            }
            this.map = map;
        }

        /**
         * Gets the map being decorated.
         * 
         * @return the decorated map
         */
        protected virtual java.util.Map<Object, Object> getMap()
        {
            return map;
        }

        //-----------------------------------------------------------------------
        public virtual void clear()
        {
            map.clear();
        }

        public virtual bool containsKey(Object key)
        {
            return map.containsKey(key);
        }

        public virtual bool containsValue(Object value)
        {
            return map.containsValue(value);
        }

        public virtual java.util.Set<java.util.MapNS.Entry<Object, Object>> entrySet()
        {
            return map.entrySet();
        }

        public virtual Object get(Object key)
        {
            return map.get(key);
        }

        public virtual bool isEmpty()
        {
            return map.isEmpty();
        }

        public virtual java.util.Set<Object> keySet()
        {
            return map.keySet();
        }

        public virtual Object put(Object key, Object value)
        {
            return map.put(key, value);
        }

        public virtual void putAll(java.util.Map<Object, Object> mapToCopy)
        {
            map.putAll(mapToCopy);
        }

        public virtual Object remove(Object key)
        {
            return map.remove(key);
        }

        public virtual int size()
        {
            return map.size();
        }

        public virtual java.util.Collection<Object> values()
        {
            return map.values();
        }

        public override bool Equals(Object obj)
        {
            if (obj == this)
            {
                return true;
            }
            return map.Equals(obj);
        }

        public override int GetHashCode()
        {
            return map.GetHashCode();
        }

        public override String ToString()
        {
            return map.toString();
        }

    }
}