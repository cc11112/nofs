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

namespace org.apache.commons.collections.collection
{

    /**
     * Decorates another <code>Collection</code> to synchronize its behaviour
     * for a multi-threaded environment.
     * <p>
     * Iterators must be manually synchronized:
     * <pre>
     * synchronized (coll) {
     *   Iterator it = coll.iterator();
     *   // do stuff with iterator
     * }
     * </pre>
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    [Serializable]
    public class SynchronizedCollection : java.util.Collection<Object>, java.io.Serializable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 2412805092710877986L;

        /** The collection to decorate */
        protected internal readonly java.util.Collection<Object> collection;
        /** The object to lock on, needed for List/SortedSet views */
        protected readonly Object lockJ;

        /**
         * Factory method to create a synchronized collection.
         * 
         * @param coll  the collection to decorate, must not be null
         * @return a new synchronized collection
         * @throws IllegalArgumentException if collection is null
         */
        public static java.util.Collection<Object> decorate(java.util.Collection<Object> coll)
        {
            return new SynchronizedCollection(coll);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param collection  the collection to decorate, must not be null
         * @throws IllegalArgumentException if the collection is null
         */
        protected SynchronizedCollection(java.util.Collection<Object> collection)
        {
            if (collection == null)
            {
                throw new java.lang.IllegalArgumentException("Collection must not be null");
            }
            this.collection = collection;
            this.lockJ = this;
        }

        /**
         * Constructor that wraps (not copies).
         * 
         * @param collection  the collection to decorate, must not be null
         * @param lock  the lock object to use, must not be null
         * @throws IllegalArgumentException if the collection is null
         */
        protected SynchronizedCollection(java.util.Collection<Object> collection, Object lockJ)
        {
            if (collection == null)
            {
                throw new java.lang.IllegalArgumentException("Collection must not be null");
            }
            this.collection = collection;
            this.lockJ = lockJ;
        }

        //-----------------------------------------------------------------------
        public virtual bool add(Object obj)
        {
            lock (lockJ)
            {
                return collection.add(obj);
            }
        }

        public virtual bool addAll(java.util.Collection<Object> coll)
        {
            lock (lockJ)
            {
                return collection.addAll(coll);
            }
        }

        public virtual void clear()
        {
            lock (lockJ)
            {
                collection.clear();
            }
        }

        public virtual bool contains(Object obj)
        {
            lock (lockJ)
            {
                return collection.contains(obj);
            }
        }

        public virtual bool containsAll(java.util.Collection<Object> coll)
        {
            lock (lockJ)
            {
                return collection.containsAll(coll);
            }
        }

        public virtual bool isEmpty()
        {
            lock (lockJ)
            {
                return collection.isEmpty();
            }
        }

        /**
         * Iterators must be manually synchronized.
         * <pre>
         * synchronized (coll) {
         *   Iterator it = coll.iterator();
         *   // do stuff with iterator
         * }
         * 
         * @return an iterator that must be manually synchronized on the collection
         */
        public virtual java.util.Iterator<Object> iterator()
        {
            return collection.iterator();
        }

        public virtual Object[] toArray()
        {
            lock (lockJ)
            {
                return collection.toArray();
            }
        }

        public virtual Object[] toArray<Object>(Object[] obj)
        {
            lock (lockJ)
            {
                return collection.toArray(obj);
            }
        }

        public virtual bool remove(Object obj)
        {
            lock (lockJ)
            {
                return collection.remove(obj);
            }
        }

        public virtual bool removeAll(java.util.Collection<Object> coll)
        {
            lock (lockJ)
            {
                return collection.removeAll(coll);
            }
        }

        public virtual bool retainAll(java.util.Collection<Object> coll)
        {
            lock (lockJ)
            {
                return collection.retainAll(coll);
            }
        }

        public virtual int size()
        {
            lock (lockJ)
            {
                return collection.size();
            }
        }

        public override bool Equals(Object obj)
        {
            lock (lockJ)
            {
                if (obj == this)
                {
                    return true;
                }
                return collection.equals(obj);
            }
        }

        public override int GetHashCode()
        {
            lock (lockJ)
            {
                return collection.GetHashCode();
            }
        }

        public override String ToString()
        {
            lock (lockJ)
            {
                return collection.ToString();
            }
        }

    }
}