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
using org.apache.commons.collections.collection;
using org.apache.commons.collections.set;

namespace org.apache.commons.collections.bag
{

    /**
     * Decorates another <code>Bag</code> to synchronize its behaviour
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
    public class SynchronizedBag
            : SynchronizedCollection, Bag
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 8084674570753837109L;

        /**
         * Factory method to create a synchronized bag.
         * 
         * @param bag  the bag to decorate, must not be null
         * @return a new synchronized Bag
         * @throws IllegalArgumentException if bag is null
         */
        public static Bag decorate(Bag bag)
        {
            return new SynchronizedBag(bag);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param bag  the bag to decorate, must not be null
         * @throws IllegalArgumentException if bag is null
         */
        protected SynchronizedBag(Bag bag)
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
        protected SynchronizedBag(Bag bag, Object lockJ)
            : base(bag, lockJ)
        {
        }

        /**
         * Gets the bag being decorated.
         * 
         * @return the decorated bag
         */
        protected Bag getBag()
        {
            return (Bag)collection;
        }

        //-----------------------------------------------------------------------
        public bool add(Object obj, int count)
        {
            lock (lockJ)
            {
                return getBag().add(obj, count);
            }
        }

        public bool remove(Object obj, int count)
        {
            lock (lockJ)
            {
                return getBag().remove(obj, count);
            }
        }

        public java.util.Set<Object> uniqueSet()
        {
            lock (lockJ)
            {
                java.util.Set<Object> set = getBag().uniqueSet();
                return new SynchronizedBagSet(set, lockJ);
            }
        }

        public int getCount(Object obj)
        {
            lock (lockJ)
            {
                return getBag().getCount(obj);
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Synchronized Set for the Bag class.
         */
        internal class SynchronizedBagSet : SynchronizedSet
        {
            /**
             * Constructor.
             * @param set  the set to decorate
             * @param lock  the lock to use, shared with the bag
             */
            internal SynchronizedBagSet(java.util.Set<Object> set, Object lockJ)
                : base(set, lockJ)
            {
            }
        }

    }
}