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

namespace org.apache.commons.collections.bag
{

    /**
     * Decorates another <code>SortedBag</code> to synchronize its behaviour
     * for a multi-threaded environment.
     * <p>
     * Methods are synchronized, then forwarded to the decorated bag.
     * Iterators must be separately synchronized around the loop.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    [Serializable]
    public class SynchronizedSortedBag
            : SynchronizedBag, SortedBag
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 722374056718497858L;

        /**
         * Factory method to create a synchronized sorted bag.
         * 
         * @param bag  the bag to decorate, must not be null
         * @return a new synchronized SortedBag
         * @throws IllegalArgumentException if bag is null
         */
        public static SortedBag decorate(SortedBag bag)
        {
            return new SynchronizedSortedBag(bag);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param bag  the bag to decorate, must not be null
         * @throws IllegalArgumentException if bag is null
         */
        protected SynchronizedSortedBag(SortedBag bag)
            : base(bag)
        {
        }

        /**
         * Constructor that wraps (not copies).
         * 
         * @param bag  the bag to decorate, must not be null
         * @param lock  the lock to use, must not be null
         * @throws IllegalArgumentException if bag is null
         */
        protected SynchronizedSortedBag(Bag bag, Object lockJ)
            : base(bag, lockJ)
        {
        }

        /**
         * Gets the bag being decorated.
         * 
         * @return the decorated bag
         */
        protected SortedBag getSortedBag()
        {
            return (SortedBag)collection;
        }

        //-----------------------------------------------------------------------
        public Object first()
        {
            lock (lockJ)
            {
                return getSortedBag().first();
            }
        }

        public Object last()
        {
            lock (lockJ)
            {
                return getSortedBag().last();
            }
        }

        public java.util.Comparator<Object> comparator()
        {
            lock (lockJ)
            {
                return getSortedBag().comparator();
            }
        }

    }
}