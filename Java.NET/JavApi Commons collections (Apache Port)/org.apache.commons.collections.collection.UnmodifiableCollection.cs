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
     * Decorates another <code>Collection</code> to ensure it can't be altered.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    public sealed class UnmodifiableCollection
            : AbstractSerializableCollectionDecorator
            , Unmodifiable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -239892006883819945L;

        /**
         * Factory method to create an unmodifiable collection.
         * <p>
         * If the collection passed in is already unmodifiable, it is returned.
         * 
         * @param coll  the collection to decorate, must not be null
         * @return an unmodifiable collection
         * @throws IllegalArgumentException if collection is null
         */
        public static java.util.Collection<Object> decorate(java.util.Collection<Object> coll)
        {
            if (coll is Unmodifiable)
            {
                return coll;
            }
            return new UnmodifiableCollection(coll);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param coll  the collection to decorate, must not be null
         * @throws IllegalArgumentException if collection is null
         */
        private UnmodifiableCollection(java.util.Collection<Object> coll)
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

    }
}