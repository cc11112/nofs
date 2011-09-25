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

namespace org.apache.commons.collections.set
{

    /**
     * Decorates another <code>SortedSet</code> to ensure it can't be altered.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    [Serializable]
    public sealed class UnmodifiableSortedSet
            : AbstractSortedSetDecorator
            , Unmodifiable, java.io.Serializable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -725356885467962424L;

        /**
         * Factory method to create an unmodifiable set.
         * 
         * @param set  the set to decorate, must not be null
         * @throws IllegalArgumentException if set is null
         */
        public static java.util.SortedSet<Object> decorate(java.util.SortedSet<Object> set)
        {
            if (set is Unmodifiable)
            {
                return set;
            }
            return new UnmodifiableSortedSet(set);
        }

        //-----------------------------------------------------------------------
        /**
         * Write the collection out using a custom routine.
         * 
         * @param out  the output stream
         * @throws IOException
         */
        private void writeObject(java.io.ObjectOutputStream outJ) {//throws IOException {
        outJ.defaultWriteObject();
        outJ.writeObject(collection);
    }

        /**
         * Read the collection in using a custom routine.
         * 
         * @param in  the input stream
         * @throws IOException
         * @throws ClassNotFoundException
         */
        private void readObject(java.io.ObjectInputStream inJ)
        {// throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            collection = (java.util.Collection<Object>)inJ.readObject();
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param set  the set to decorate, must not be null
         * @throws IllegalArgumentException if set is null
         */
        private UnmodifiableSortedSet(java.util.SortedSet<Object> set)
            : base(set)
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
        public override java.util.SortedSet<Object> subSet(Object fromElement, Object toElement)
        {
            java.util.SortedSet<Object> sub = getSortedSet().subSet(fromElement, toElement);
            return new UnmodifiableSortedSet(sub);
        }

        public override java.util.SortedSet<Object> headSet(Object toElement)
        {
            java.util.SortedSet<Object> sub = getSortedSet().headSet(toElement);
            return new UnmodifiableSortedSet(sub);
        }

        public override java.util.SortedSet<Object> tailSet(Object fromElement)
        {
            java.util.SortedSet<Object> sub = getSortedSet().tailSet(fromElement);
            return new UnmodifiableSortedSet(sub);
        }

    }
}