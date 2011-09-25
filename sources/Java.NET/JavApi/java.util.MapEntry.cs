/*
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */
using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util
{

    /**
     * MapEntry is an internal class which provides an implementation of Map.Entry.
     */
    internal class MapEntry<K, V> : MapNS.Entry<K, V>, java.lang.Cloneable
    {

        protected internal K key;
        protected internal V value;

        interface Type<RT, KT, VT>
        {
            RT get(MapEntry<KT, VT> entry);
        }

        protected internal MapEntry(K theKey)
        {
            key = theKey;
        }

        protected internal MapEntry(K theKey, V theValue)
        {
            key = theKey;
            value = theValue;
        }

        public Object clone()
        {
            try
            {
                return base.MemberwiseClone();
            }
            catch (java.lang.CloneNotSupportedException e)
            {
                return null;
            }
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj is MapNS.Entry<Object, Object>)
            {
                MapNS.Entry<Object, Object> entry = (MapNS.Entry<Object, Object>)obj;
                return (key == null ? entry.getKey() == null : key.equals(entry
                        .getKey()))
                        && (value == null ? entry.getValue() == null : value
                                .equals(entry.getValue()));
            }
            return false;
        }

        public virtual K getKey()
        {
            return key;
        }

        public virtual V getValue()
        {
            return value;
        }


        public override int GetHashCode()
        {
            return (key == null ? 0 : key.GetHashCode())
                    ^ (value == null ? 0 : value.GetHashCode());
        }

        public virtual V setValue(V obj)
        {
            V result = value;
            value = obj;
            return result;
        }


        public override String ToString()
        {
            return key + "=" + value;
        }
    }
}