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
using System.Collections.Generic;
using System.Collections;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util
{
    /**
     * {@code AbstractList} is an abstract implementation of the {@code List} interface, optimized
     * for a backing store which supports random access. This implementation does
     * not support adding or replacing. A subclass must implement the abstract
     * methods {@code get()} and {@code size()}, and to create a
     * modifiable {@code List} it's necessary to override the {@code add()} method that
     * currently throws an {@code UnsupportedOperationException}.
     *
     * @since 1.2
     */
    [Serializable]
    public abstract class AbstractList<E> : AbstractCollection<E>, List<E>, IEnumerable<E> {

        /**
         * A counter for changes to the list.
         */
        [NonSerialized]
        protected internal int modCount;

        /**
         * Constructs a new instance of this AbstractList.
         */
        protected AbstractList() : base (){
        }

        /**
         * Inserts the specified object into this List at the specified location.
         * The object is inserted before any previous element at the specified
         * location. If the location is equal to the size of this List, the object
         * is added at the end.
         * <p>
         * Concrete implementations that would like to support the add functionality
         * must override this method.
         *
         * @param location
         *            the index at which to insert.
         * @param object
         *            the object to add.
         * 
         * @throws UnsupportedOperationException
         *                if adding to this List is not supported.
         * @throws ClassCastException
         *                if the class of the object is inappropriate for this
         *                List
         * @throws IllegalArgumentException
         *                if the object cannot be added to this List
         * @throws IndexOutOfBoundsException
         *                if {@code location < 0 || >= size()}
         */
        public virtual void add(int location, E obj) {
            throw new java.lang.UnsupportedOperationException();
        }

        /**
         * Adds the specified object at the end of this List.
         * 
         * 
         * @param object
         *            the object to add
         * @return true
         * 
         * @throws UnsupportedOperationException
         *                if adding to this List is not supported
         * @throws ClassCastException
         *                if the class of the object is inappropriate for this
         *                List
         * @throws IllegalArgumentException
         *                if the object cannot be added to this List
         */
        public override bool add(E obj) {
            add(size(), obj);
            return true;
        }

        /**
         * Inserts the objects in the specified Collection at the specified location
         * in this List. The objects are added in the order they are returned from
         * the collection's iterator.
         * 
         * @param location
         *            the index at which to insert.
         * @param collection
         *            the Collection of objects
         * @return {@code true} if this List is modified, {@code false} otherwise.
         * @throws UnsupportedOperationException
         *             if adding to this list is not supported.
         * @throws ClassCastException
         *             if the class of an object is inappropriate for this list.
         * @throws IllegalArgumentException
         *             if an object cannot be added to this list.
         * @throws IndexOutOfBoundsException
         *             if {@code location < 0 || > size()}
         */
        public virtual bool addAll(int location, Collection<E> collection) {
            Iterator<E> it = collection.iterator();
            while (it.hasNext()) {
                add(location++, it.next());
            }
            return !collection.isEmpty();
        }

        /**
         * Removes all elements from this list, leaving it empty.
         * 
         * @throws UnsupportedOperationException
         *             if removing from this list is not supported.
         * @see List#isEmpty
         * @see List#size
         */
        
        public override void clear() {
            removeRange(0, size());
        }

        /**
         * Compares the specified object to this list and return true if they are
         * equal. Two lists are equal when they both contain the same objects in the
         * same order.
         * 
         * @param object
         *            the object to compare to this object.
         * @return {@code true} if the specified object is equal to this list,
         *         {@code false} otherwise.
         * @see #hashCode
         */
        
        public override bool Equals(Object obj) {
            if (this == obj) {
                return true;
            }
            if (obj is List<E>) {
                List<E> list = (List<E>) obj;
                if (list.size() != size()) {
                    return false;
                }

                Iterator<E> it1 = iterator(), it2 = list.iterator();
                while (it1.hasNext()) {
                    Object e1 = it1.next(), e2 = it2.next();
                    if (!(e1 == null ? e2 == null : e1.equals(e2))) {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        /**
         * Returns the element at the specified location in this list.
         * 
         * @param location
         *            the index of the element to return.
         * @return the element at the specified index.
         * @throws IndexOutOfBoundsException
         *             if {@code location < 0 || >= size()}
         */
        public abstract E get(int location);

        /**
         * Returns the hash code of this list. The hash code is calculated by taking
         * each element's hashcode into account.
         * 
         * @return the hash code.
         * @see #equals
         * @see List#hashCode()
         */
        
        public override int GetHashCode() {
            int result = 1;
            Iterator<E> it = iterator();
            while (it.hasNext()) {
                Object obj = it.next();
                result = (31 * result) + (obj == null ? 0 : obj.GetHashCode());
            }
            return result;
        }

        /**
         * Searches this list for the specified object and returns the index of the
         * first occurrence.
         * 
         * @param object
         *            the object to search for.
         * @return the index of the first occurrence of the object, or -1 if it was
         *         not found.
         */
        public virtual int indexOf(Object obj) {
            ListIterator<E> it = listIterator();
            if (obj != null) {
                while (it.hasNext()) {
                    if (obj.equals(it.next())) {
                        return it.previousIndex();
                    }
                }
            } else {
                while (it.hasNext()) {
                    if (it.next() == null) {
                        return it.previousIndex();
                    }
                }
            }
            return -1;
        }

        /**
         * Returns an iterator on the elements of this list. The elements are
         * iterated in the same order as they occur in the list.
         * 
         * @return an iterator on the elements of this list.
         * @see Iterator
         */
        
        public override Iterator<E> iterator() {
            return new SimpleListIterator<E>(this);
        }

        /**
         * Searches this list for the specified object and returns the index of the
         * last occurrence.
         * 
         * @param object
         *            the object to search for.
         * @return the index of the last occurrence of the object, or -1 if the
         *         object was not found.
         */
        public virtual int lastIndexOf(Object obj) {
            ListIterator<E> it = listIterator(size());
            if (obj != null) {
                while (it.hasPrevious()) {
                    if (obj.equals(it.previous())) {
                        return it.nextIndex();
                    }
                }
            } else {
                while (it.hasPrevious()) {
                    if (it.previous() == null) {
                        return it.nextIndex();
                    }
                }
            }
            return -1;
        }

        /**
         * Returns a ListIterator on the elements of this list. The elements are
         * iterated in the same order that they occur in the list.
         * 
         * @return a ListIterator on the elements of this list
         * @see ListIterator
         */
        public virtual ListIterator<E> listIterator() {
            return listIterator(0);
        }

        /**
         * Returns a list iterator on the elements of this list. The elements are
         * iterated in the same order as they occur in the list. The iteration
         * starts at the specified location.
         * 
         * @param location
         *            the index at which to start the iteration.
         * @return a ListIterator on the elements of this list.
         * @throws IndexOutOfBoundsException
         *             if {@code location < 0 || location > size()}
         * @see ListIterator
         */
        public virtual ListIterator<E> listIterator(int location) {
            return new FullListIterator<E>(this,location);
        }

        /**
         * Removes the object at the specified location from this list.
         * 
         * @param location
         *            the index of the object to remove.
         * @return the removed object.
         * @throws UnsupportedOperationException
         *             if removing from this list is not supported.
         * @throws IndexOutOfBoundsException
         *             if {@code location < 0 || >= size()}
         */
        public virtual E remove(int location) {
            throw new java.lang.UnsupportedOperationException();
        }

        /**
         * Removes the objects in the specified range from the start to the end
         * index minus one.
         * 
         * @param start
         *            the index at which to start removing.
         * @param end
         *            the index after the last element to remove.
         * @throws UnsupportedOperationException
         *             if removing from this list is not supported.
         * @throws IndexOutOfBoundsException
         *             if {@code start < 0} or {@code start >= size()}.
         */
        protected internal virtual void removeRange(int start, int end) {
            Iterator<E> it = listIterator(start);
            for (int i = start; i < end; i++) {
                it.next();
                it.remove();
            }
        }

        /**
         * Replaces the element at the specified location in this list with the
         * specified object.
         * 
         * @param location
         *            the index at which to put the specified object.
         * @param object
         *            the object to add.
         * @return the previous element at the index.
         * @throws UnsupportedOperationException
         *             if replacing elements in this list is not supported.
         * @throws ClassCastException
         *             if the class of an object is inappropriate for this list.
         * @throws IllegalArgumentException
         *             if an object cannot be added to this list.
         * @throws IndexOutOfBoundsException
         *             if {@code location < 0 || >= size()}
         */
        public virtual E set(int location, E obj) {
            throw new java.lang.UnsupportedOperationException();
        }

        /**
         * Returns a part of consecutive elements of this list as a view. The
         * returned view will be of zero length if start equals end. Any change that
         * occurs in the returned subList will be reflected to the original list,
         * and vice-versa. All the supported optional operations by the original
         * list will also be supported by this subList.
         * <p>
         * This method can be used as a handy method to do some operations on a sub
         * range of the original list, for example
         * {@code list.subList(from, to).clear();}
         * <p>
         * If the original list is modified in other ways than through the returned
         * subList, the behavior of the returned subList becomes undefined.
         * <p>
         * The returned subList is a subclass of AbstractList. The subclass stores
         * offset, size of itself, and modCount of the original list. If the
         * original list implements RandomAccess interface, the returned subList
         * also implements RandomAccess interface.
         * <p>
         * The subList's set(int, Object), get(int), add(int, Object), remove(int),
         * addAll(int, Collection) and removeRange(int, int) methods first check the
         * bounds, adjust offsets and then call the corresponding methods of the
         * original AbstractList. addAll(Collection c) method of the returned
         * subList calls the original addAll(offset + size, c).
         * <p>
         * The listIterator(int) method of the subList wraps the original list
         * iterator. The iterator() method of the subList invokes the original
         * listIterator() method, and the size() method merely returns the size of
         * the subList.
         * <p>
         * All methods will throw a ConcurrentModificationException if the modCount
         * of the original list is not equal to the expected value.
         * 
         * @param start
         *            start index of the subList (inclusive).
         * @param end
         *            end index of the subList, (exclusive).
         * @return a subList view of this list starting from {@code start}
         *         (inclusive), and ending with {@code end} (exclusive)
         * @throws IndexOutOfBoundsException
         *             if (start < 0 || end > size())
         * @throws IllegalArgumentException
         *             if (start > end)
         */
        public virtual List<E> subList(int start, int end) {
            if (0 <= start && end <= size()) {
                if (start <= end) {
                    if (this is RandomAccess) {
                        return new SubAbstractListRandomAccess<E>(this, start, end);
                    }
                    return new SubAbstractList<E>(this, start, end);
                }
                throw new java.lang.IllegalArgumentException();
            }
            throw new java.lang.IndexOutOfBoundsException();
        }

        #region Enumerator implementation
        /// <summary>
        /// Get the IEnumerator instance.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        /// <summary>
        /// Get the IEnemurator instance for this wrapped java.util.Iterator.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<E> GetEnumerator()
        {
            java.util.Iterator<E> it = this.iterator();
            while (it.hasNext())
            {
                yield return it.next();
            }
        }
        #endregion

    }
    #region SubAbstractList<E>
    
    internal class SubAbstractList<E> : AbstractList<E>
    {
        protected readonly AbstractList<E> fullList;

        private int offset;

        protected int sizeJ;


        internal SubAbstractList(AbstractList<E> list, int start, int end)
            : base()
        {
            fullList = list;
            modCount = fullList.modCount;
            offset = start;
            sizeJ = end - start;
        }


        public override void add(int location, E obj)
        {
            if (modCount == fullList.modCount)
            {
                if (0 <= location && location <= sizeJ)
                {
                    fullList.add(location + offset, obj);
                    sizeJ++;
                    modCount = fullList.modCount;
                }
                else
                {
                    throw new java.lang.IndexOutOfBoundsException();
                }
            }
            else
            {
                throw new ConcurrentModificationException();
            }
        }

        public override bool addAll(int location, Collection<E> collection)
        {
            if (modCount == fullList.modCount)
            {
                if (0 <= location && location <= sizeJ)
                {
                    bool result = fullList.addAll(location + offset,
                            collection);
                    if (result)
                    {
                        sizeJ += collection.size();
                        modCount = fullList.modCount;
                    }
                    return result;
                }
                throw new java.lang.IndexOutOfBoundsException();
            }
            throw new ConcurrentModificationException();
        }

        public override bool addAll(Collection<E> collection)
        {
            if (modCount == fullList.modCount)
            {
                bool result = fullList.addAll(offset + sizeJ, collection);
                if (result)
                {
                    sizeJ += collection.size();
                    modCount = fullList.modCount;
                }
                return result;
            }
            throw new ConcurrentModificationException();
        }


        public override E get(int location)
        {
            if (modCount == fullList.modCount)
            {
                if (0 <= location && location < sizeJ)
                {
                    return fullList.get(location + offset);
                }
                throw new java.lang.IndexOutOfBoundsException();
            }
            throw new ConcurrentModificationException();
        }


        public override Iterator<E> iterator()
        {
            return listIterator(0);
        }


        public override ListIterator<E> listIterator(int location)
        {
            if (modCount == fullList.modCount)
            {
                if (0 <= location && location <= sizeJ)
                {
                    return new SubAbstractListIterator<E>(fullList.listIterator(location + offset), this, offset,sizeJ);
                }
                throw new java.lang.IndexOutOfBoundsException();
            }
            throw new ConcurrentModificationException();
        }


        public override E remove(int location)
        {
            if (modCount == fullList.modCount)
            {
                if (0 <= location && location < sizeJ)
                {
                    E result = fullList.remove(location + offset);
                    sizeJ--;
                    modCount = fullList.modCount;
                    return result;
                }
                throw new java.lang.IndexOutOfBoundsException();
            }
            throw new ConcurrentModificationException();
        }


        protected internal override void removeRange(int start, int end)
        {
            if (start != end)
            {
                if (modCount == fullList.modCount)
                {
                    fullList.removeRange(start + offset, end + offset);
                    sizeJ -= end - start;
                    modCount = fullList.modCount;
                }
                else
                {
                    throw new ConcurrentModificationException();
                }
            }
        }


        public override E set(int location, E obj)
        {
            if (modCount == fullList.modCount)
            {
                if (0 <= location && location < sizeJ)
                {
                    return fullList.set(location + offset, obj);
                }
                throw new java.lang.IndexOutOfBoundsException();
            }
            throw new ConcurrentModificationException();
        }


        public override int size()
        {
            if (modCount == fullList.modCount)
            {
                return sizeJ;
            }
            throw new ConcurrentModificationException();
        }

        protected internal void sizeChanged(bool increment)
        {
            if (increment)
            {
                sizeJ++;
            }
            else
            {
                sizeJ--;
            }
            modCount = fullList.modCount;
        }
    }
    #endregion
    internal class SubAbstractListIterator<E> : ListIterator<E>
    {
        private readonly SubAbstractList<E> subList;

        private readonly ListIterator<E> iterator;

        private int start;

        private int end;

        internal SubAbstractListIterator(ListIterator<E> it, SubAbstractList<E> list, int offset, int length)
            : base()
        {
            iterator = it;
            subList = list;
            start = offset;
            end = start + length;
        }

        public void add(E obj)
        {
            iterator.add(obj);
            subList.sizeChanged(true);
            end++;
        }

        public bool hasNext()
        {
            return iterator.nextIndex() < end;
        }

        public bool hasPrevious()
        {
            return iterator.previousIndex() >= start;
        }

        public E next()
        {
            if (iterator.nextIndex() < end)
            {
                return iterator.next();
            }
            throw new NoSuchElementException();
        }

        public int nextIndex()
        {
            return iterator.nextIndex() - start;
        }

        public E previous()
        {
            if (iterator.previousIndex() >= start)
            {
                return iterator.previous();
            }
            throw new NoSuchElementException();
        }

        public int previousIndex()
        {
            int previous = iterator.previousIndex();
            if (previous >= start)
            {
                return previous - start;
            }
            return -1;
        }

        public void remove()
        {
            iterator.remove();
            subList.sizeChanged(false);
            end--;
        }

        public void set(E obj)
        {
            iterator.set(obj);
        }
    }

    #region SubAbstractListRandomAccess
    internal class SubAbstractListRandomAccess<E> : SubAbstractList<E>, RandomAccess
    {
        internal SubAbstractListRandomAccess(AbstractList<E> list, int start, int end)
            : base(list, start, end)
        {
        }
    }
#endregion
    #region SimpleListIterator
    
    internal class SimpleListIterator<E> : Iterator<E>
    {
        private AbstractList<E> outerInstance;
        public SimpleListIterator(AbstractList<E> outerInstance)
        {
            this.outerInstance = outerInstance;
            numLeft = outerInstance.size();
            expectedModCount = outerInstance.modCount;
        }
        protected int numLeft;
        protected int expectedModCount;
        protected int lastPosition = -1;

        public virtual bool hasNext()
        {
            return numLeft > 0;
        }

        public virtual E next()
        {
            if (expectedModCount != outerInstance.modCount)
            {
                throw new ConcurrentModificationException();
            }

            try
            {
                int index = outerInstance.size() - numLeft;
                E result = outerInstance.get(index);
                lastPosition = index;
                numLeft--;
                return result;
            }
            catch (java.lang.IndexOutOfBoundsException e)
            {
                throw new NoSuchElementException();
            }
        }

        public virtual void remove() {
                if (lastPosition == -1) {
                    throw new java.lang.IllegalStateException();
                }
                if (expectedModCount != outerInstance.modCount) {
                    throw new ConcurrentModificationException();
                }

                try {
                    if (lastPosition == outerInstance.size() - numLeft) {
                        numLeft--; // we're removing after a call to previous()
                    }
                    outerInstance.remove(lastPosition);
                } catch (java.lang.IndexOutOfBoundsException e) {
                    throw new ConcurrentModificationException();
                }
            
                expectedModCount = outerInstance.modCount;
                lastPosition = -1;
            }
    }
    #endregion
    #region
    
    internal class FullListIterator<E> : SimpleListIterator<E>, ListIterator<E>
    {
        private AbstractList<E> outInstance;
        internal FullListIterator(AbstractList<E> outerInstance, int start) : base (outerInstance)
        {
            if (start < 0 || start > numLeft)
            {
                throw new java.lang.IndexOutOfBoundsException();
            }
            numLeft -= start;
            this.outInstance = outerInstance;
        }

        public void add(E obj)
        {
            if (expectedModCount != outInstance.modCount)
            {
                throw new ConcurrentModificationException();
            }

            try
            {
                outInstance.add(outInstance.size() - numLeft, obj);
                expectedModCount = outInstance.modCount;
                lastPosition = -1;
            }
            catch (java.lang.IndexOutOfBoundsException e)
            {
                throw new NoSuchElementException();
            }
        }

        public bool hasPrevious()
        {
            return numLeft < outInstance.size();
        }

        public int nextIndex()
        {
            return outInstance.size() - numLeft;
        }

        public E previous()
        {
            if (expectedModCount != outInstance.modCount)
            {
                throw new ConcurrentModificationException();
            }

            try
            {
                int index = outInstance.size() - numLeft - 1;
                E result = outInstance.get(index);
                numLeft++;
                lastPosition = index;
                return result;
            }
            catch (java.lang.IndexOutOfBoundsException e)
            {
                throw new NoSuchElementException();
            }
        }

        public int previousIndex()
        {
            return outInstance.size() - numLeft - 1;
        }

        public void set(E obj)
        {
            if (expectedModCount != outInstance.modCount)
            {
                throw new ConcurrentModificationException();
            }

            try
            {
                outInstance.set(lastPosition, obj);
            }
            catch (java.lang.IndexOutOfBoundsException e)
            {
                throw new java.lang.IllegalStateException();
            }
        }
    }
#endregion

}
