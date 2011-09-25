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

namespace org.apache.commons.collections.collection
{

    /**
     * <code>UnmodifiableBoundedCollection</code> decorates another 
     * <code>BoundedCollection</code> to ensure it can't be altered.
     * <p>
     * If a BoundedCollection is first wrapped in some other collection decorator,
     * such as synchronized or predicated, the BoundedCollection methods are no 
     * longer accessible.
     * The factory on this class will attempt to retrieve the bounded nature by
     * examining the package scope variables.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    public sealed class UnmodifiableBoundedCollection
            : AbstractSerializableCollectionDecorator
            , BoundedCollection
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -7112672385450340330L;

        /**
         * Factory method to create an unmodifiable bounded collection.
         * 
         * @param coll  the <code>BoundedCollection</code> to decorate, must not be null
         * @return a new unmodifiable bounded collection
         * @throws IllegalArgumentException if bag is null
         */
        public static BoundedCollection decorate(BoundedCollection coll)
        {
            return new UnmodifiableBoundedCollection(coll);
        }

        /**
         * Factory method to create an unmodifiable bounded collection.
         * <p>
         * This method is capable of drilling down through up to 1000 other decorators 
         * to find a suitable BoundedCollection.
         * 
         * @param coll  the <code>BoundedCollection</code> to decorate, must not be null
         * @return a new unmodifiable bounded collection
         * @throws IllegalArgumentException if bag is null
         */
        public static BoundedCollection decorateUsing(java.util.Collection<Object> coll)
        {
            if (coll == null)
            {
                throw new java.lang.IllegalArgumentException("The collection must not be null");
            }

            // handle decorators
            for (int i = 0; i < 1000; i++)
            {  // counter to prevent infinite looping
                if (coll is BoundedCollection)
                {
                    break;  // normal loop exit
                }
                else if (coll is AbstractCollectionDecorator)
                {
                    coll = ((AbstractCollectionDecorator)coll).collection;
                }
                else if (coll is SynchronizedCollection)
                {
                    coll = ((SynchronizedCollection)coll).collection;
                }
                else
                {
                    break;  // normal loop exit
                }
            }

            if (coll is BoundedCollection == false)
            {
                throw new java.lang.IllegalArgumentException("The collection is not a bounded collection");
            }
            return new UnmodifiableBoundedCollection((BoundedCollection)coll);
        }

        /**
         * Constructor that wraps (not copies).
         * 
         * @param coll  the collection to decorate, must not be null
         * @throws IllegalArgumentException if coll is null
         */
        private UnmodifiableBoundedCollection(BoundedCollection coll)
            : base(coll)
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
        public bool isFull()
        {
            return ((BoundedCollection)collection).isFull();
        }

        public int maxSize()
        {
            return ((BoundedCollection)collection).maxSize();
        }

    }
}