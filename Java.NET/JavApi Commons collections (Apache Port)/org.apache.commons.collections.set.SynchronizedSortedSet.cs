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
using org.apache.commons.collections.collection;

namespace org.apache.commons.collections.set
{

    /**
     * Decorates another <code>SortedSet</code> to synchronize its behaviour
     * for a multi-threaded environment.
     * <p>
     * Methods are synchronized, then forwarded to the decorated set.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    [Serializable]
    public class SynchronizedSortedSet : SynchronizedCollection, java.util.SortedSet<Object>
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 2775582861954500111L;

        /**
         * Factory method to create a synchronized set.
         * 
         * @param set  the set to decorate, must not be null
         * @throws IllegalArgumentException if set is null
         */
        public static java.util.SortedSet<Object> decorate(java.util.SortedSet<Object> set)
        {
            return new SynchronizedSortedSet(set);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param set  the set to decorate, must not be null
         * @throws IllegalArgumentException if set is null
         */
        protected SynchronizedSortedSet(java.util.SortedSet<Object> set)
            : base(set)
        {
        }

        /**
         * Constructor that wraps (not copies).
         * 
         * @param set  the set to decorate, must not be null
         * @param lock  the lock object to use, must not be null
         * @throws IllegalArgumentException if set is null
         */
        protected SynchronizedSortedSet(java.util.SortedSet<Object> set, Object lockJ)
            : base(set, lockJ)
        {
        }

        /**
         * Gets the decorated set.
         * 
         * @return the decorated set
         */
        protected java.util.SortedSet<Object> getSortedSet()
        {
            return (java.util.SortedSet<Object>)collection;
        }

        //-----------------------------------------------------------------------
        public java.util.SortedSet<Object> subSet(Object fromElement, Object toElement)
        {
            lock (lockJ)
            {
                java.util.SortedSet<Object> set = getSortedSet().subSet(fromElement, toElement);
                // the lock is passed into the constructor here to ensure that the
                // subset is synchronized on the same lock as the parent
                return new SynchronizedSortedSet(set, lockJ);
            }
        }

        public java.util.SortedSet<Object> headSet(Object toElement)
        {
            lock (lockJ)
            {
                java.util.SortedSet<Object> set = getSortedSet().headSet(toElement);
                // the lock is passed into the constructor here to ensure that the
                // headset is synchronized on the same lock as the parent
                return new SynchronizedSortedSet(set, lockJ);
            }
        }

        public java.util.SortedSet<Object> tailSet(Object fromElement)
        {
            lock (lockJ)
            {
                java.util.SortedSet<Object> set = getSortedSet().tailSet(fromElement);
                // the lock is passed into the constructor here to ensure that the
                // tailset is synchronized on the same lock as the parent
                return new SynchronizedSortedSet(set, lockJ);
            }
        }

        public Object first()
        {
            lock (lockJ)
            {
                return getSortedSet().first();
            }
        }

        public Object last()
        {
            lock (lockJ)
            {
                return getSortedSet().last();
            }
        }

        public java.util.Comparator<Object> comparator()
        {
            lock (lockJ)
            {
                return getSortedSet().comparator();
            }
        }

    }
}