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

namespace org.apache.commons.collections.list
{

    /**
     * Decorates another <code>List</code> to ensure it can't be altered.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    [Serializable]
    public sealed class UnmodifiableList
            : AbstractSerializableListDecorator
            , Unmodifiable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 6595182819922443652L;

        /**
         * Factory method to create an unmodifiable list.
         * 
         * @param list  the list to decorate, must not be null
         * @throws IllegalArgumentException if list is null
         */
        public static java.util.List<Object> decorate(java.util.List<Object> list)
        {
            if (list is Unmodifiable)
            {
                return list;
            }
            return new UnmodifiableList(list);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param list  the list to decorate, must not be null
         * @throws IllegalArgumentException if list is null
         */
        private UnmodifiableList(java.util.List<Object> list)
            : base(list)
        {
        }

        //-----------------------------------------------------------------------
        public override java.util.Iterator<Object> iterator()
        {
            return UnmodifiableIterator.decorate(getCollection().iterator());
        }

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
        public override java.util.ListIterator<Object> listIterator()
        {
            return UnmodifiableListIterator.decorate(getList().listIterator());
        }

        public override java.util.ListIterator<Object> listIterator(int index)
        {
            return UnmodifiableListIterator.decorate(getList().listIterator(index));
        }

        public override void add(int index, Object obj)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override bool addAll(int index, java.util.Collection<Object> coll)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override Object remove(int index)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override Object set(int index, Object obj)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override java.util.List<Object> subList(int fromIndex, int toIndex)
        {
            java.util.List<Object> sub = getList().subList(fromIndex, toIndex);
            return new UnmodifiableList(sub);
        }

    }
}