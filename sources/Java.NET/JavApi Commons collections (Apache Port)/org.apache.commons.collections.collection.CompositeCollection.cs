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
using org.apache.commons.collections.iterators;
using org.apache.commons.collections.list;

namespace org.apache.commons.collections.collection
{

    /**
     * Decorates a collection of other collections to provide a single unified view.
     * <p>
     * Changes made to this collection will actually be made on the decorated collection.
     * Add and remove operations require the use of a pluggable strategy. If no 
     * strategy is provided then add and remove are unsupported.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Brian McCallister
     * @author Stephen Colebourne
     * @author Phil Steitz
     */
    public class CompositeCollection : java.util.Collection<Object>
    {

        /** CollectionMutator to handle changes to the collection */
        protected CollectionMutator mutator;

        /** Collections in the composite */
        protected java.util.Collection<Object>[] all;

        /**
         * Create an empty CompositeCollection.
         */
        public CompositeCollection()
            : base()
        {
            this.all = new java.util.Collection<Object>[0];
        }

        /**
         * Create a Composite Collection with only coll composited.
         * 
         * @param coll  a collection to decorate
         */
        public CompositeCollection(java.util.Collection<Object> coll)
            : this()
        {
            this.addComposited(coll);
        }

        /**
         * Create a CompositeCollection with colls as the initial list of
         * composited collections.
         * 
         * @param colls  an array of collections to decorate
         */
        public CompositeCollection(java.util.Collection<Object>[] colls)
            : this()
        {
            this.addComposited(colls);
        }

        //-----------------------------------------------------------------------
        /**
         * Gets the size of this composite collection.
         * <p>
         * This implementation calls <code>size()</code> on each collection.
         *
         * @return total number of elements in all contained containers
         */
        public virtual int size()
        {
            int size = 0;
            for (int i = this.all.Length - 1; i >= 0; i--)
            {
                size += this.all[i].size();
            }
            return size;
        }

        /**
         * Checks whether this composite collection is empty.
         * <p>
         * This implementation calls <code>isEmpty()</code> on each collection.
         *
         * @return true if all of the contained collections are empty
         */
        public virtual bool isEmpty()
        {
            for (int i = this.all.Length - 1; i >= 0; i--)
            {
                if (this.all[i].isEmpty() == false)
                {
                    return false;
                }
            }
            return true;
        }

        /**
         * Checks whether this composite collection contains the object.
         * <p>
         * This implementation calls <code>contains()</code> on each collection.
         *
         * @param obj  the object to search for
         * @return true if obj is contained in any of the contained collections
         */
        public virtual bool contains(Object obj)
        {
            for (int i = this.all.Length - 1; i >= 0; i--)
            {
                if (this.all[i].contains(obj))
                {
                    return true;
                }
            }
            return false;
        }

        /**
         * Gets an iterator over all the collections in this composite.
         * <p>
         * This implementation uses an <code>IteratorChain</code>.
         *
         * @return an <code>IteratorChain</code> instance which supports
         *  <code>remove()</code>. Iteration occurs over contained collections in
         *  the order they were added, but this behavior should not be relied upon.
         * @see IteratorChain
         */
        public virtual java.util.Iterator<Object> iterator()
        {
            if (this.all.Length == 0)
            {
                return EmptyIterator.INSTANCE;
            }
            IteratorChain chain = new IteratorChain();
            for (int i = 0; i < this.all.Length; ++i)
            {
                chain.addIterator(this.all[i].iterator());
            }
            return chain;
        }

        /**
         * Returns an array containing all of the elements in this composite.
         *
         * @return an object array of all the elements in the collection
         */
        public virtual Object[] toArray()
        {
            Object[] result = new Object[this.size()];
            int i = 0;
            for (java.util.Iterator<Object> it = this.iterator(); it.hasNext(); i++)
            {
                result[i] = it.next();
            }
            return result;
        }

        /**
         * Returns an object array, populating the supplied array if possible.
         * See <code>Collection</code> interface for full details.
         *
         * @param array  the array to use, populating if possible
         * @return an array of all the elements in the collection
         */
        public virtual Object[] toArray<Object>(Object[] array)
        {
            int size = this.size();
            Object[] result = null;
            if (array.Length >= size)
            {
                result = array;
            }
            else
            {
                result = new Object[size];// (Object[]) Array.newInstance(array.getClass().getComponentType(), size);
            }

            int offset = 0;
            for (int i = 0; i < this.all.Length; ++i)
            {
                for (java.util.Iterator<Object> it = ((java.util.Iterator<Object>)this.all[i].iterator()); it.hasNext(); )
                {
                    result[offset++] = it.next();
                }
            }
            if (result.Length > size)
            {
                result[size] = default(Object);
            }
            return result;
        }

        /**
         * Adds an object to the collection, throwing UnsupportedOperationException
         * unless a CollectionMutator strategy is specified.
         *
         * @param obj  the object to add
         * @return true if the collection was modified
         * @throws UnsupportedOperationException if CollectionMutator hasn't been set
         * @throws UnsupportedOperationException if add is unsupported
         * @throws ClassCastException if the object cannot be added due to its type
         * @throws NullPointerException if the object cannot be added because its null
         * @throws IllegalArgumentException if the object cannot be added
         */
        public virtual bool add(Object obj)
        {
            if (this.mutator == null)
            {
                throw new java.lang.UnsupportedOperationException(
                "add() is not supported on CompositeCollection without a CollectionMutator strategy");
            }
            return this.mutator.add(this, this.all, obj);
        }

        /**
         * Removes an object from the collection, throwing UnsupportedOperationException
         * unless a CollectionMutator strategy is specified.
         *
         * @param obj  the object being removed
         * @return true if the collection is changed
         * @throws UnsupportedOperationException if removed is unsupported
         * @throws ClassCastException if the object cannot be removed due to its type
         * @throws NullPointerException if the object cannot be removed because its null
         * @throws IllegalArgumentException if the object cannot be removed
         */
        public virtual bool remove(Object obj)
        {
            if (this.mutator == null)
            {
                throw new java.lang.UnsupportedOperationException(
                "remove() is not supported on CompositeCollection without a CollectionMutator strategy");
            }
            return this.mutator.remove(this, this.all, obj);
        }

        /**
         * Checks whether this composite contains all the elements in the specified collection.
         * <p>
         * This implementation calls <code>contains()</code> for each element in the
         * specified collection.
         *
         * @param coll  the collection to check for
         * @return true if all elements contained
         */
        public virtual bool containsAll(java.util.Collection<Object> coll)
        {
            for (java.util.Iterator<Object> it = coll.iterator(); it.hasNext(); )
            {
                if (this.contains(it.next()) == false)
                {
                    return false;
                }
            }
            return true;
        }

        /**
         * Adds a collection of elements to this collection, throwing
         * UnsupportedOperationException unless a CollectionMutator strategy is specified.
         *
         * @param coll  the collection to add
         * @return true if the collection was modified
         * @throws UnsupportedOperationException if CollectionMutator hasn't been set
         * @throws UnsupportedOperationException if add is unsupported
         * @throws ClassCastException if the object cannot be added due to its type
         * @throws NullPointerException if the object cannot be added because its null
         * @throws IllegalArgumentException if the object cannot be added
         */
        public virtual bool addAll(java.util.Collection<Object> coll)
        {
            if (this.mutator == null)
            {
                throw new java.lang.UnsupportedOperationException(
                "addAll() is not supported on CompositeCollection without a CollectionMutator strategy");
            }
            return this.mutator.addAll(this, this.all, coll);
        }

        /**
         * Removes the elements in the specified collection from this composite collection.
         * <p>
         * This implementation calls <code>removeAll</code> on each collection.
         *
         * @param coll  the collection to remove
         * @return true if the collection was modified
         * @throws UnsupportedOperationException if removeAll is unsupported
         */
        public virtual bool removeAll(java.util.Collection<Object> coll)
        {
            if (coll.size() == 0)
            {
                return false;
            }
            bool changed = false;
            for (int i = this.all.Length - 1; i >= 0; i--)
            {
                changed = (this.all[i].removeAll(coll) || changed);
            }
            return changed;
        }

        /**
         * Retains all the elements in the specified collection in this composite collection,
         * removing all others.
         * <p>
         * This implementation calls <code>retainAll()</code> on each collection.
         *
         * @param coll  the collection to remove
         * @return true if the collection was modified
         * @throws UnsupportedOperationException if retainAll is unsupported
         */
        public virtual bool retainAll(java.util.Collection<Object> coll)
        {
            bool changed = false;
            for (int i = this.all.Length - 1; i >= 0; i--)
            {
                changed = (this.all[i].retainAll(coll) || changed);
            }
            return changed;
        }

        /**
         * Removes all of the elements from this collection .
         * <p>
         * This implementation calls <code>clear()</code> on each collection.
         *
         * @throws UnsupportedOperationException if clear is unsupported
         */
        public virtual void clear()
        {
            for (int i = 0; i < this.all.Length; ++i)
            {
                this.all[i].clear();
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Specify a CollectionMutator strategy instance to handle changes.
         *
         * @param mutator  the mutator to use
         */
        public virtual void setMutator(CollectionMutator mutator)
        {
            this.mutator = mutator;
        }

        /**
         * Add these Collections to the list of collections in this composite
         *
         * @param comps Collections to be appended to the composite
         */
        public virtual void addComposited(java.util.Collection<Object>[] comps)
        {
            java.util.ArrayList<Object> list = new java.util.ArrayList<Object>(java.util.Arrays<Object>.asList<Object>(this.all));
            list.addAll(java.util.Arrays<Object>.asList<Object>(comps));
            all = (java.util.Collection<Object>[])list.toArray(new java.util.Collection<Object>[list.size()]);
        }

        /**
         * Add an additional collection to this composite.
         * 
         * @param c  the collection to add
         */
        public virtual void addComposited(java.util.Collection<Object> c)
        {
            this.addComposited(new java.util.Collection<Object>[] { c });
        }

        /**
         * Add two additional collections to this composite.
         * 
         * @param c  the first collection to add
         * @param d  the second collection to add
         */
        public virtual void addComposited(java.util.Collection<Object> c, java.util.Collection<Object> d)
        {
            this.addComposited(new java.util.Collection<Object>[] { c, d });
        }

        /**
         * Removes a collection from the those being decorated in this composite.
         *
         * @param coll  collection to be removed
         */
        public virtual void removeComposited(java.util.Collection<Object> coll)
        {
            java.util.ArrayList<Object> list = new java.util.ArrayList<Object>(this.all.Length);
            list.addAll(java.util.Arrays<Object>.asList<Object>(this.all));
            list.remove(coll);
            this.all = (java.util.Collection<Object>[])list.toArray(new java.util.Collection<Object>[list.size()]);
        }

        /**
         * Returns a new collection containing all of the elements
         *
         * @return A new ArrayList containing all of the elements in this composite.
         *         The new collection is <i>not</i> backed by this composite.
         */
        public virtual java.util.Collection<Object> toCollection()
        {
            return new java.util.ArrayList<Object>(this);
        }

        /**
         * Gets the collections being decorated.
         *
         * @return Unmodifiable collection of all collections in this composite.
         */
        public virtual java.util.Collection<Object> getCollections()
        {
            return UnmodifiableList.decorate(java.util.Arrays<Object>.asList<Object>(this.all));
        }

        //-----------------------------------------------------------------------
        /**
         * Pluggable strategy to handle changes to the composite.
         */
        public interface CollectionMutator
        {

            /**
             * Called when an object is to be added to the composite.
             *
             * @param composite  the CompositeCollection being changed
             * @param collections  all of the Collection instances in this CompositeCollection
             * @param obj  the object being added
             * @return true if the collection is changed
             * @throws UnsupportedOperationException if add is unsupported
             * @throws ClassCastException if the object cannot be added due to its type
             * @throws NullPointerException if the object cannot be added because its null
             * @throws IllegalArgumentException if the object cannot be added
             */
            bool add(CompositeCollection composite, java.util.Collection<Object>[] collections, Object obj);

            /**
             * Called when a collection is to be added to the composite.
             *
             * @param composite  the CompositeCollection being changed
             * @param collections  all of the Collection instances in this CompositeCollection
             * @param coll  the collection being added
             * @return true if the collection is changed
             * @throws UnsupportedOperationException if add is unsupported
             * @throws ClassCastException if the object cannot be added due to its type
             * @throws NullPointerException if the object cannot be added because its null
             * @throws IllegalArgumentException if the object cannot be added
             */
            bool addAll(CompositeCollection composite, java.util.Collection<Object>[] collections, java.util.Collection<Object> coll);

            /**
             * Called when an object is to be removed to the composite.
             *
             * @param composite  the CompositeCollection being changed
             * @param collections  all of the Collection instances in this CompositeCollection
             * @param obj  the object being removed
             * @return true if the collection is changed
             * @throws UnsupportedOperationException if removed is unsupported
             * @throws ClassCastException if the object cannot be removed due to its type
             * @throws NullPointerException if the object cannot be removed because its null
             * @throws IllegalArgumentException if the object cannot be removed
             */
            bool remove(CompositeCollection composite, java.util.Collection<Object>[] collections, Object obj);

        }

    }

}