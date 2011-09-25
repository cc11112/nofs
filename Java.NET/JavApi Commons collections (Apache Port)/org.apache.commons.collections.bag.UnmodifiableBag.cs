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
using org.apache.commons.collections.set;

namespace org.apache.commons.collections.bag
{

    /**
     * Decorates another <code>Bag</code> to ensure it can't be altered.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    [Serializable]
    public sealed class UnmodifiableBag
            : AbstractBagDecorator, Unmodifiable, java.io.Serializable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -1873799975157099624L;

        /**
         * Factory method to create an unmodifiable bag.
         * <p>
         * If the bag passed in is already unmodifiable, it is returned.
         * 
         * @param bag  the bag to decorate, must not be null
         * @return an unmodifiable Bag
         * @throws IllegalArgumentException if bag is null
         */
        public static Bag decorate(Bag bag)
        {
            if (bag is Unmodifiable)
            {
                return bag;
            }
            return new UnmodifiableBag(bag);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param bag  the bag to decorate, must not be null
         * @throws IllegalArgumentException if bag is null
         */
        private UnmodifiableBag(Bag bag)
            : base(bag)
        {
        }

        //-----------------------------------------------------------------------
        /**
         * Write the collection out using a custom routine.
         * 
         * @param out  the output stream
         * @throws IOException
         */
        private void writeObject(java.io.ObjectOutputStream outJ)
        {// throws IOException {
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
        {//throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            collection = (java.util.Collection<Object>)inJ.readObject();
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
        public override bool add(Object obj, int count)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override bool remove(Object obj, int count)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override java.util.Set<Object> uniqueSet()
        {
            java.util.Set<Object> set = getBag().uniqueSet();
            return UnmodifiableSet.decorate(set);
        }

    }
}