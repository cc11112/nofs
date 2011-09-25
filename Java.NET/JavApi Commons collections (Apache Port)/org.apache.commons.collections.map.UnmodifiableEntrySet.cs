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
using org.apache.commons.collections.keyvalue;
using org.apache.commons.collections.iterators;
using org.apache.commons.collections.set;

namespace org.apache.commons.collections.map
{

    /**
     * Decorates a map entry <code>Set</code> to ensure it can't be altered.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    public sealed class UnmodifiableEntrySet
            : AbstractSetDecorator, Unmodifiable
    {

        /**
         * Factory method to create an unmodifiable set of Map Entry objects.
         * 
         * @param set  the set to decorate, must not be null
         * @throws IllegalArgumentException if set is null
         */
        public static java.util.Set<Object> decorate(java.util.Set<Object> set)
        {
            if (set is Unmodifiable)
            {
                return set;
            }
            return new UnmodifiableEntrySet(set);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param set  the set to decorate, must not be null
         * @throws IllegalArgumentException if set is null
         */
        private UnmodifiableEntrySet(java.util.Set<Object> set)
            : base(set)
        {
        }

        //-----------------------------------------------------------------------
        public override bool add(Object obj)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override bool addAll(java.util.Collection<Object> coll)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override void clear()
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override bool remove(Object obj)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override bool removeAll(java.util.Collection<Object> coll)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override bool retainAll(java.util.Collection<Object> coll)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        //-----------------------------------------------------------------------
        public override java.util.Iterator<Object> iterator()
        {
            return new UnmodifiableEntrySetIterator(collection.iterator());
        }

        public override Object[] toArray()
        {
            Object[] array = collection.toArray();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new UnmodifiableEntry((java.util.MapNS.Entry<Object, Object>)array[i]);
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
                //result = (Object[]) Array.newInstance(array.getClass().getComponentType(), 0);
                result = new Object[array.Length];
            }
            Object [] input = collection.toArray(result);
            for (int i = 0; i < result.Length; i++)
            {
                java.util.MapNS.Entry<object, object> makeUnmodifiable = (java.util.MapNS.Entry<object, object>)input[i];
                UnmodifiableEntry ue = new UnmodifiableEntry(makeUnmodifiable);
                result[i] = ((Object)(object) ue);
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
                array[result.Length] = default (Object);
            }
            return array;
        }

        //-----------------------------------------------------------------------
    }
    /**
     * Implementation of an entry set iterator.
     */
    internal class UnmodifiableEntrySetIterator : AbstractIteratorDecorator
    {

        protected internal UnmodifiableEntrySetIterator(java.util.Iterator<Object> iterator) :
            base(iterator)
        {
        }

        public override Object next()
        {
            java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)iterator.next();
            return new UnmodifiableEntry(entry);
        }

        public override void remove()
        {
            throw new java.lang.UnsupportedOperationException();
        }
    }

    //-----------------------------------------------------------------------
    /**
     * Implementation of a map entry that is unmodifiable.
     */
    internal class UnmodifiableEntry : AbstractMapEntryDecorator
    {

        protected internal UnmodifiableEntry(java.util.MapNS.Entry<Object, Object> entry) :
            base(entry)
        {
        }

        public override Object setValue(Object obj)
        {
            throw new java.lang.UnsupportedOperationException();
        }
    }

}