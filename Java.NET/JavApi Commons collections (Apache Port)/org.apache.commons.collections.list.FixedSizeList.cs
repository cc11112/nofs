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
     * Decorates another <code>List</code> to fix the size preventing add/remove.
     * <p>
     * The add, remove, clear and retain operations are unsupported.
     * The set method is allowed (as it doesn't change the list size).
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
    public class FixedSizeList
            : AbstractSerializableListDecorator
            , BoundedCollection
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -2218010673611160319L;

        /**
         * Factory method to create a fixed size list.
         * 
         * @param list  the list to decorate, must not be null
         * @throws IllegalArgumentException if list is null
         */
        public static java.util.List<Object> decorate(java.util.List<Object> list)
        {
            return new FixedSizeList(list);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param list  the list to decorate, must not be null
         * @throws IllegalArgumentException if list is null
         */
        protected FixedSizeList(java.util.List<Object> list)
            : base(list)
        {
        }

        //-----------------------------------------------------------------------
        public override bool add(Object obj)
        {
            throw new java.lang.UnsupportedOperationException("List is fixed size");
        }

        public override void add(int index, Object obj)
        {
            throw new java.lang.UnsupportedOperationException("List is fixed size");
        }

        public override bool addAll(java.util.Collection<Object> coll)
        {
            throw new java.lang.UnsupportedOperationException("List is fixed size");
        }

        public override bool addAll(int index, java.util.Collection<Object> coll)
        {
            throw new java.lang.UnsupportedOperationException("List is fixed size");
        }

        public override void clear()
        {
            throw new java.lang.UnsupportedOperationException("List is fixed size");
        }

        public override Object get(int index)
        {
            return getList().get(index);
        }

        public override int indexOf(Object obj)
        {
            return getList().indexOf(obj);
        }

        public override java.util.Iterator<Object> iterator()
        {
            return UnmodifiableIterator.decorate(getCollection().iterator());
        }

        public override int lastIndexOf(Object obj)
        {
            return getList().lastIndexOf(obj);
        }

        public override java.util.ListIterator<Object> listIterator()
        {
            return new FixedSizeListIterator(getList().listIterator(0));
        }

        public override java.util.ListIterator<Object> listIterator(int index)
        {
            return new FixedSizeListIterator(getList().listIterator(index));
        }

        public override Object remove(int index)
        {
            throw new java.lang.UnsupportedOperationException("List is fixed size");
        }

        public override bool remove(Object obj)
        {
            throw new java.lang.UnsupportedOperationException("List is fixed size");
        }

        public override bool removeAll(java.util.Collection<Object> coll)
        {
            throw new java.lang.UnsupportedOperationException("List is fixed size");
        }

        public override bool retainAll(java.util.Collection<Object> coll)
        {
            throw new java.lang.UnsupportedOperationException("List is fixed size");
        }

        public override Object set(int index, Object obj)
        {
            return getList().set(index, obj);
        }

        public override java.util.List<Object> subList(int fromIndex, int toIndex)
        {
            java.util.List<Object> sub = getList().subList(fromIndex, toIndex);
            return new FixedSizeList(sub);
        }

        /**
         * List iterator that only permits changes via set()
         */
        class FixedSizeListIterator : AbstractListIteratorDecorator
        {
            protected internal FixedSizeListIterator(java.util.ListIterator<Object> iterator)
                : base(iterator)
            {
            }
            public override void remove()
            {
                throw new java.lang.UnsupportedOperationException("List is fixed size");
            }
            public override void add(Object obj)
            {
                throw new java.lang.UnsupportedOperationException("List is fixed size");
            }
        }

        public virtual bool isFull()
        {
            return true;
        }

        public virtual int maxSize()
        {
            return size();
        }

    }
}