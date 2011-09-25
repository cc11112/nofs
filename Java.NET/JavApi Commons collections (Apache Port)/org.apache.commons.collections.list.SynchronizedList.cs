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

namespace org.apache.commons.collections.list
{

    /**
     * Decorates another <code>List</code> to synchronize its behaviour
     * for a multi-threaded environment.
     * <p>
     * Methods are synchronized, then forwarded to the decorated list.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    [Serializable]
    public class SynchronizedList : SynchronizedCollection, java.util.List<Object>
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -1403835447328619437L;

        /**
         * Factory method to create a synchronized list.
         * 
         * @param list  the list to decorate, must not be null
         * @throws IllegalArgumentException if list is null
         */
        public static java.util.List<Object> decorate(java.util.List<Object> list)
        {
            return new SynchronizedList(list);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param list  the list to decorate, must not be null
         * @throws IllegalArgumentException if list is null
         */
        protected SynchronizedList(java.util.List<Object> list)
            : base(list)
        {

        }

        /**
         * Constructor that wraps (not copies).
         * 
         * @param list  the list to decorate, must not be null
         * @param lock  the lock to use, must not be null
         * @throws IllegalArgumentException if list is null
         */
        protected SynchronizedList(java.util.List<Object> list, Object lockJ)
            : base(list, lockJ)
        {
        }

        /**
         * Gets the decorated list.
         * 
         * @return the decorated list
         */
        protected java.util.List<Object> getList()
        {
            return (java.util.List<Object>)collection;
        }

        //-----------------------------------------------------------------------
        public void add(int index, Object obj)
        {
            lock (lockJ)
            {
                getList().add(index, obj);
            }
        }

        public bool addAll(int index, java.util.Collection<Object> coll)
        {
            lock (lockJ)
            {
                return getList().addAll(index, coll);
            }
        }

        public Object get(int index)
        {
            lock (lockJ)
            {
                return getList().get(index);
            }
        }

        public int indexOf(Object obj)
        {
            lock (lockJ)
            {
                return getList().indexOf(obj);
            }
        }

        public int lastIndexOf(Object obj)
        {
            lock (lockJ)
            {
                return getList().lastIndexOf(obj);
            }
        }

        /**
         * Iterators must be manually synchronized.
         * <pre>
         * synchronized (coll) {
         *   ListIterator it = coll.listIterator();
         *   // do stuff with iterator
         * }
         * 
         * @return an iterator that must be manually synchronized on the collection
         */
        public java.util.ListIterator<Object> listIterator()
        {
            return getList().listIterator();
        }

        /**
         * Iterators must be manually synchronized.
         * <pre>
         * synchronized (coll) {
         *   ListIterator it = coll.listIterator(3);
         *   // do stuff with iterator
         * }
         * 
         * @return an iterator that must be manually synchronized on the collection
         */
        public java.util.ListIterator<Object> listIterator(int index)
        {
            return getList().listIterator(index);
        }

        public Object remove(int index)
        {
            lock (lockJ)
            {
                return getList().remove(index);
            }
        }

        public Object set(int index, Object obj)
        {
            lock (lockJ)
            {
                return getList().set(index, obj);
            }
        }

        public java.util.List<Object> subList(int fromIndex, int toIndex)
        {
            lock (lockJ)
            {
                java.util.List<Object> list = getList().subList(fromIndex, toIndex);
                // the lock is passed into the constructor here to ensure that the sublist is
                // synchronized on the same lock as the parent list
                return new SynchronizedList(list, lockJ);
            }
        }

    }
}