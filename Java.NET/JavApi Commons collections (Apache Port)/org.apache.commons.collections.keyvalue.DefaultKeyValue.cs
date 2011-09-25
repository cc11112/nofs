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

namespace org.apache.commons.collections.keyvalue
{

    /**
     * A mutable <code>KeyValue</code> pair that does not implement
     * {@link java.util.Map.Entry Map.Entry}.
     * <p>
     * Note that a <code>DefaultKeyValue</code> instance may not contain
     * itself as a key or value.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author James Strachan
     * @author Michael A. Smith
     * @author Neil O'Toole
     * @author Stephen Colebourne
     */
    public class DefaultKeyValue : AbstractKeyValue
    {

        /**
         * Constructs a new pair with a null key and null value.
         */
        public DefaultKeyValue()
            : base(null, null)
        {
        }

        /**
         * Constructs a new pair with the specified key and given value.
         *
         * @param key  the key for the entry, may be null
         * @param value  the value for the entry, may be null
         */
        public DefaultKeyValue(Object key, Object value)
            : base(key, value)
        {
        }

        /**
         * Constructs a new pair from the specified <code>KeyValue</code>.
         *
         * @param pair  the pair to copy, must not be null
         * @throws NullPointerException if the entry is null
         */
        public DefaultKeyValue(KeyValue pair)
            : base(pair.getKey(), pair.getValue())
        {
        }

        /**
         * Constructs a new pair from the specified <code>Map.Entry</code>.
         *
         * @param entry  the entry to copy, must not be null
         * @throws NullPointerException if the entry is null
         */
        public DefaultKeyValue(java.util.MapNS.Entry<Object, Object> entry)
            : base(entry.getKey(), entry.getValue())
        {
        }

        //-----------------------------------------------------------------------
        /**
         * Sets the key.
         *
         * @param key  the new key
         * @return the old key
         * @throws IllegalArgumentException if key is this object
         */
        public Object setKey(Object key)
        {
            if (key == this)
            {
                throw new java.lang.IllegalArgumentException("DefaultKeyValue may not contain itself as a key.");
            }

            Object old = this.key;
            this.key = key;
            return old;
        }

        /** 
         * Sets the value.
         *
         * @return the old value of the value
         * @param value the new value
         * @throws IllegalArgumentException if value is this object
         */
        public Object setValue(Object value)
        {
            if (value == this)
            {
                throw new java.lang.IllegalArgumentException("DefaultKeyValue may not contain itself as a value.");
            }

            Object old = this.value;
            this.value = value;
            return old;
        }

        //-----------------------------------------------------------------------
        /**
         * Returns a new <code>Map.Entry</code> object with key and value from this pair.
         * 
         * @return a MapEntry instance
         */
        public java.util.MapNS.Entry<Object, Object> toMapEntry()
        {
            return new DefaultMapEntry(this);
        }

        //-----------------------------------------------------------------------
        /**
         * Compares this <code>Map.Entry</code> with another <code>Map.Entry</code>.
         * <p>
         * Returns true if the compared object is also a <code>DefaultKeyValue</code>,
         * and its key and value are equal to this object's key and value.
         * 
         * @param obj  the object to compare to
         * @return true if equal key and value
         */
        public override bool Equals(Object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (obj is DefaultKeyValue == false)
            {
                return false;
            }

            DefaultKeyValue other = (DefaultKeyValue)obj;
            return
                (getKey() == null ? other.getKey() == null : getKey().equals(other.getKey())) &&
                (getValue() == null ? other.getValue() == null : getValue().equals(other.getValue()));
        }

        /**
         * Gets a hashCode compatible with the equals method.
         * <p>
         * Implemented per API documentation of {@link java.util.Map.Entry#hashCode()},
         * however subclasses may override this.
         * 
         * @return a suitable hash code
         */
        public override int GetHashCode()
        {
            return (getKey() == null ? 0 : getKey().GetHashCode()) ^
                   (getValue() == null ? 0 : getValue().GetHashCode());
        }

    }
}