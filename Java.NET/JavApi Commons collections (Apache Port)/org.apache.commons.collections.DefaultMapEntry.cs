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
 */

using System;
using java = biz.ritter.javapi;
using org.apache.commons.collections;

namespace org.apache.commons.collections
{

    /**
     * A default implementation of {@link java.util.Map.Entry}
     *
     * @deprecated Use the version in the keyvalue subpackage. Will be removed in v4.0
     * @since Commons Collections 1.0
     * @version $Revision$ $Date$
     * 
     * @author James Strachan
     * @author Michael A. Smith
     * @author Neil O'Toole
     * @author Stephen Colebourne
     */
    [Obsolete]
    public class DefaultMapEntry : java.util.MapNS.Entry<Object, Object>, KeyValue
    {

        /** The key */
        private Object key;
        /** The value */
        private Object value;

        /**
         * Constructs a new <code>DefaultMapEntry</code> with a null key
         * and null value.
         */
        public DefaultMapEntry()
            : base()
        {
        }

        /**
         * Constructs a new <code>DefaultMapEntry</code> with the given
         * key and given value.
         *
         * @param entry  the entry to copy, must not be null
         * @throws NullPointerException if the entry is null
         */
        public DefaultMapEntry(java.util.MapNS.Entry<Object, Object> entry)
            : base()
        {
            this.key = entry.getKey();
            this.value = entry.getValue();
        }

        /**
         * Constructs a new <code>DefaultMapEntry</code> with the given
         * key and given value.
         *
         * @param key  the key for the entry, may be null
         * @param value  the value for the entry, may be null
         */
        public DefaultMapEntry(Object key, Object value)
            : base()
        {
            this.key = key;
            this.value = value;
        }

        // Map.Entry interface
        //-------------------------------------------------------------------------
        /**
         * Gets the key from the Map Entry.
         *
         * @return the key 
         */
        public Object getKey()
        {
            return key;
        }

        /**
         * Sets the key stored in this Map Entry.
         * <p>
         * This Map Entry is not connected to a Map, so only the local data is changed.
         *
         * @param key  the new key
         */
        public void setKey(Object key)
        {
            this.key = key;
        }

        /**
         * Gets the value from the Map Entry.
         *
         * @return the value
         */
        public Object getValue()
        {
            return value;
        }

        /** 
         * Sets the value stored in this Map Entry.
         * <p>
         * This Map Entry is not connected to a Map, so only the local data is changed.
         *
         * @param value  the new value
         * @return the previous value
         */
        public Object setValue(Object value)
        {
            Object answer = this.value;
            this.value = value;
            return answer;
        }

        // Basics
        //-----------------------------------------------------------------------
        /**
         * Compares this Map Entry with another Map Entry.
         * <p>
         * Implemented per API documentation of {@link java.util.Map.Entry#equals(Object)}
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
            if (obj is java.util.MapNS.Entry<Object, Object> == false)
            {
                return false;
            }
            java.util.MapNS.Entry<Object, Object> other = (java.util.MapNS.Entry<Object, Object>)obj;
            return
                (getKey() == null ? other.getKey() == null : getKey().equals(other.getKey())) &&
                (getValue() == null ? other.getValue() == null : getValue().equals(other.getValue()));
        }

        /**
         * Gets a hashCode compatible with the equals method.
         * <p>
         * Implemented per API documentation of {@link java.util.Map.Entry#hashCode()}
         * 
         * @return a suitable hash code
         */
        public override int GetHashCode()
        {
            return (getKey() == null ? 0 : getKey().GetHashCode()) ^
                   (getValue() == null ? 0 : getValue().GetHashCode());
        }

        /**
         * Written to match the output of the Map.Entry's used in 
         * a {@link java.util.HashMap}. 
         * @since 3.0
         */
        public override String ToString()
        {
            return "" + getKey() + "=" + getValue();
        }
    }
}