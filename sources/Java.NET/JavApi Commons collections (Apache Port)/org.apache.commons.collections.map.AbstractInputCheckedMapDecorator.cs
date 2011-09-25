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
using org.apache.commons.collections.iterators;
using org.apache.commons.collections.keyvalue;
using org.apache.commons.collections.set;

namespace org.apache.commons.collections.map
{

    /**
     * An abstract base class that simplifies the task of creating map decorators.
     * <p>
     * The Map API is very difficult to decorate correctly, and involves implementing
     * lots of different classes. This class exists to provide a simpler API.
     * <p>
     * Special hook methods are provided that are called when objects are added to
     * the map. By overriding these methods, the input can be validated or manipulated.
     * In addition to the main map methods, the entrySet is also affected, which is
     * the hardest part of writing map implementations.
     * <p>
     * This class is package-scoped, and may be withdrawn or replaced in future
     * versions of Commons Collections.
     *
     * @since Commons Collections 3.1
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    public abstract class AbstractInputCheckedMapDecorator
            : AbstractMapDecorator
    {

        /**
         * Constructor only used in deserialization, do not use otherwise.
         */
        protected AbstractInputCheckedMapDecorator()
            : base()
        {
        }

        /**
         * Constructor that wraps (not copies).
         * 
         * @param map  the map to decorate, must not be null
         * @throws IllegalArgumentException if map is null
         */
        protected AbstractInputCheckedMapDecorator(java.util.Map<Object, Object> map)
            : base(map)
        {
        }

        //-----------------------------------------------------------------------
        /**
         * Hook method called when a value is being set using <code>setValue</code>.
         * <p>
         * An implementation may validate the value and throw an exception
         * or it may transform the value into another object.
         * <p>
         * This implementation returns the input value.
         * 
         * @param value  the value to check
         * @throws UnsupportedOperationException if the map may not be changed by setValue
         * @throws IllegalArgumentException if the specified value is invalid
         * @throws ClassCastException if the class of the specified value is invalid
         * @throws NullPointerException if the specified value is null and nulls are invalid
         */
        protected internal abstract Object checkSetValue(Object value);

        /**
         * Hook method called to determine if <code>checkSetValue</code> has any effect.
         * <p>
         * An implementation should return false if the <code>checkSetValue</code> method
         * has no effect as this optimises the implementation.
         * <p>
         * This implementation returns <code>true</code>.
         * 
         * @return true always
         */
        protected virtual bool isSetValueChecking()
        {
            return true;
        }

        //-----------------------------------------------------------------------
        public virtual new java.util.Set<Object> entrySet()
        {
            if (isSetValueChecking())
            {
                return new EntrySet((java.util.Set<Object>)map.entrySet(),this);
            }
            else
            {
                return(java.util.Set<Object>) map.entrySet();
            }
        }

        //-----------------------------------------------------------------------

        #region iac
        ///<summary>
        /// Implementation of an entry set that checks additions via setValue.
        ///</summary>
        internal class EntrySet : AbstractSetDecorator
        {

            protected internal readonly AbstractInputCheckedMapDecorator parent;

            protected internal EntrySet(java.util.Set<Object> set, AbstractInputCheckedMapDecorator parent)
                : base(set)
            {
                this.parent = parent;
            }

            public override java.util.Iterator<Object> iterator()
            {
                return new EntrySetIterator(collection.iterator(), parent);
            }

            public override Object[] toArray()
            {
                Object[] array = collection.toArray();
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = new MapEntry((java.util.MapNS.Entry<Object, Object>)array[i], parent);
                }
                return array;
            }

            public override Object[] toArray<Object>(Object[] array)
            {
                Object[] result = array;
                if (array.Length > 0)
                {
                    // we must create a new array to handle multi-threaded situations
                    // where another thread could access data before we decorate it
                    result = new Object[array.Length]; //result = (Object[]) java.lang.reflect.Array.newInstance(array.getClass().getComponentType(), 0);
                }
                result = collection.toArray(result);
                for (int i = 0; i < result.Length; i++)
                {
                    MapEntry me = new MapEntry((java.util.MapNS.Entry<object, object>)result[i], parent);
                    result[i] = (Object)(object)me;
                }

                // check to see if result should be returned straight
                if (result.Length > array.Length)
                {
                    return result;
                }

                // copy back into input array to fulfil the method contract
                java.lang.SystemJ.arraycopy(result, 0, array, 0, result.Length);
                if (array.Length > result.Length)
                {
                    array[result.Length] = default(Object);
                }
                return array;
            }
        }

        /**
         * Implementation of an entry set iterator that checks additions via setValue.
         */
        public class EntrySetIterator : AbstractIteratorDecorator
        {

            /** The parent map */
            private readonly AbstractInputCheckedMapDecorator parent;

            protected internal EntrySetIterator(java.util.Iterator<Object> iterator, AbstractInputCheckedMapDecorator parent)
                : base(iterator)
            {
                this.parent = parent;
            }

            public override Object next()
            {
                java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)iterator.next();
                return new MapEntry(entry, parent);
            }
        }

        /**
         * Implementation of a map entry that checks additions via setValue.
         */
        public class MapEntry : AbstractMapEntryDecorator
        {

            /** The parent map */
            protected internal readonly AbstractInputCheckedMapDecorator parent;

            protected internal MapEntry(java.util.MapNS.Entry<Object, Object> entry, AbstractInputCheckedMapDecorator parent)
                : base(entry)
            {
                this.parent = parent;
            }

            public override Object setValue(Object value)
            {
                value = parent.checkSetValue(value);
                return entry.setValue(value);
            }
        }

        #endregion
    }

}