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

namespace org.apache.commons.collections
{

    /**
     * A doubly-linked list implementation of the {@link List} interface,
     * supporting a {@link ListIterator} that allows concurrent modifications
     * to the underlying list.
     * <p>
     * Implements all of the optional {@link List} operations, the
     * stack/queue/dequeue operations available in {@link java.util.LinkedList}
     * and supports a {@link ListIterator} that allows concurrent modifications
     * to the underlying list (see {@link #cursor}).
     * <p>
     * <b>Note that this implementation is not synchronized.</b>
     *
     * @deprecated Use new version in list subpackage, which has been rewritten
     *  and now returns the cursor from the listIterator method. Will be removed in v4.0
     * @see java.util.LinkedList
     * @since Commons Collections 1.0
     * @version $Revision$ $Date$
     * 
     * @author Rodney Waldhoff
     * @author Janek Bogucki
     * @author Simon Kitching
     */
    [Serializable]
    public class CursorableLinkedList : java.util.List<Object>, java.io.Serializable
    {
        /** Ensure serialization compatibility */
        private static readonly long serialVersionUID = 8836393098519411393L;

        //--- public methods ---------------------------------------------

        /**
         * Appends the specified element to the end of this list.
         *
         * @param o element to be appended to this list.
         * @return <tt>true</tt>
         */
        public virtual bool add(Object o)
        {
            insertListable(_head.prev(), null, o);
            return true;
        }

        /**
         * Inserts the specified element at the specified position in this list.
         * Shifts the element currently at that position (if any) and any subsequent
         *  elements to the right (adds one to their indices).
         *
         * @param index index at which the specified element is to be inserted.
         * @param element element to be inserted.
         *
         * @throws ClassCastException if the class of the specified element
         *           prevents it from being added to this list.
         * @throws IllegalArgumentException if some aspect of the specified
         *             element prevents it from being added to this list.
         * @throws IndexOutOfBoundsException if the index is out of range
         *             (index &lt; 0 || index &gt; size()).
         */
        public virtual void add(int index, Object element)
        {
            if (index == _size)
            {
                add(element);
            }
            else
            {
                if (index < 0 || index > _size)
                {
                    throw new java.lang.IndexOutOfBoundsException("" + index + " < 0 or " + index + " > " + _size);
                }
                Listable succ = (isEmpty() ? null : getListableAt(index));
                Listable pred = (null == succ ? null : succ.prev());
                insertListable(pred, succ, element);
            }
        }

        /**
         * Appends all of the elements in the specified collection to the end of
         * this list, in the order that they are returned by the specified
         * {@link Collection}'s {@link Iterator}.  The behavior of this operation is
         * unspecified if the specified collection is modified while
         * the operation is in progress.  (Note that this will occur if the
         * specified collection is this list, and it's nonempty.)
         *
         * @param c collection whose elements are to be added to this list.
         * @return <tt>true</tt> if this list changed as a result of the call.
         *
         * @throws ClassCastException if the class of an element in the specified
         *          collection prevents it from being added to this list.
         * @throws IllegalArgumentException if some aspect of an element in the
         *         specified collection prevents it from being added to this
         *         list.
         */
        public virtual bool addAll(java.util.Collection<Object> c)
        {
            if (c.isEmpty())
            {
                return false;
            }
            java.util.Iterator<Object> it = c.iterator();
            while (it.hasNext())
            {
                insertListable(_head.prev(), null, it.next());
            }
            return true;
        }

        /**
         * Inserts all of the elements in the specified collection into this
         * list at the specified position.  Shifts the element currently at
         * that position (if any) and any subsequent elements to the right
         * (increases their indices).  The new elements will appear in this
         * list in the order that they are returned by the specified
         * {@link Collection}'s {@link Iterator}.  The behavior of this operation is
         * unspecified if the specified collection is modified while the
         * operation is in progress.  (Note that this will occur if the specified
         * collection is this list, and it's nonempty.)
         *
         * @param index index at which to insert first element from the specified
         *                collection.
         * @param c elements to be inserted into this list.
         * @return <tt>true</tt> if this list changed as a result of the call.
         *
         * @throws ClassCastException if the class of one of elements of the
         *            specified collection prevents it from being added to this
         *            list.
         * @throws IllegalArgumentException if some aspect of one of elements of
         *         the specified collection prevents it from being added to
         *         this list.
         * @throws IndexOutOfBoundsException if the index is out of range (index
         *          &lt; 0 || index &gt; size()).
         */
        public virtual bool addAll(int index, java.util.Collection<Object> c)
        {
            if (c.isEmpty())
            {
                return false;
            }
            else if (_size == index || _size == 0)
            {
                return addAll(c);
            }
            else
            {
                Listable succ = getListableAt(index);
                Listable pred = (null == succ) ? null : succ.prev();
                java.util.Iterator<Object> it = c.iterator();
                while (it.hasNext())
                {
                    pred = insertListable(pred, succ, it.next());
                }
                return true;
            }
        }

        /**
         * Inserts the specified element at the beginning of this list.
         * (Equivalent to {@link #add(int,java.lang.Object) <tt>add(0,o)</tt>}).
         *
         * @param o element to be prepended to this list.
         * @return <tt>true</tt>
         */
        public virtual bool addFirst(Object o)
        {
            insertListable(null, _head.next(), o);
            return true;
        }

        /**
         * Inserts the specified element at the end of this list.
         * (Equivalent to {@link #add(java.lang.Object)}).
         *
         * @param o element to be appended to this list.
         * @return <tt>true</tt>
         */
        public virtual bool addLast(Object o)
        {
            insertListable(_head.prev(), null, o);
            return true;
        }

        /**
         * Removes all of the elements from this list.  This
         * list will be empty after this call returns (unless
         * it throws an exception).
         */
        public virtual void clear()
        {
            /*
            // this is the quick way, but would force us
            // to break all the cursors
            _modCount++;
            _head.setNext(null);
            _head.setPrev(null);
            _size = 0;
            */
            java.util.Iterator<Object> it = iterator();
            while (it.hasNext())
            {
                it.next();
                it.remove();
            }
        }

        /**
         * Returns <tt>true</tt> if this list contains the specified element.
         * More formally, returns <tt>true</tt> if and only if this list contains
         * at least one element <tt>e</tt> such that
         * <tt>(o==null&nbsp;?&nbsp;e==null&nbsp;:&nbsp;o.equals(e))</tt>.
         *
         * @param o element whose presence in this list is to be tested.
         * @return <tt>true</tt> if this list contains the specified element.
         */
        public virtual bool contains(Object o)
        {
            for (Listable elt = _head.next(), past = null; null != elt && past != _head.prev(); elt = (past = elt).next())
            {
                if ((null == o && null == elt.value()) ||
                   (o != null && o.equals(elt.value())))
                {
                    return true;
                }
            }
            return false;
        }

        /**
         * Returns <tt>true</tt> if this list contains all of the elements of the
         * specified collection.
         *
         * @param c collection to be checked for containment in this list.
         * @return <tt>true</tt> if this list contains all of the elements of the
         *         specified collection.
         */
        public virtual bool containsAll(java.util.Collection<Object> c)
        {
            java.util.Iterator<Object> it = c.iterator();
            while (it.hasNext())
            {
                if (!this.contains(it.next()))
                {
                    return false;
                }
            }
            return true;
        }

        /**
         * Returns a {@link ListIterator} for iterating through the
         * elements of this list. Unlike {@link #iterator}, a cursor
         * is not bothered by concurrent modifications to the
         * underlying list.
         * <p>
         * Specifically, when elements are added to the list before or
         * after the cursor, the cursor simply picks them up automatically.
         * When the "current" (i.e., last returned by {@link ListIterator#next}
         * or {@link ListIterator#previous}) element of the list is removed,
         * the cursor automatically adjusts to the change (invalidating the
         * last returned value--i.e., it cannot be removed).
         * <p>
         * Note that the returned {@link ListIterator} does not support the
         * {@link ListIterator#nextIndex} and {@link ListIterator#previousIndex}
         * methods (they throw {@link UnsupportedOperationException} when invoked.
         * <p>
         * Historical Note: In previous versions of this class, the object 
         * returned from this method was required to be explicitly closed. This 
         * is no longer necessary.
         *
         * @see #cursor(int)
         * @see #listIterator
         * @see CursorableLinkedList.Cursor
         */
        public virtual CursorableLinkedList.Cursor cursor()
        {
            return new Cursor(0, this);
        }

        /**
         * Returns a {@link ListIterator} for iterating through the
         * elements of this list, initialized such that
         * {@link ListIterator#next} will return the element at
         * the specified index (if any) and {@link ListIterator#previous}
         * will return the element immediately preceding it (if any).
         * Unlike {@link #iterator}, a cursor
         * is not bothered by concurrent modifications to the
         * underlying list.
         *
         * @see #cursor
         * @see #listIterator(int)
         * @see CursorableLinkedList.Cursor
         * @throws IndexOutOfBoundsException if the index is out of range (index
         *            &lt; 0 || index &gt; size()).
         */
        public virtual CursorableLinkedList.Cursor cursor(int i)
        {
            return new Cursor(i, this);
        }

        /**
         * Compares the specified object with this list for equality.  Returns
         * <tt>true</tt> if and only if the specified object is also a list, both
         * lists have the same size, and all corresponding pairs of elements in
         * the two lists are <i>equal</i>.  (Two elements <tt>e1</tt> and
         * <tt>e2</tt> are <i>equal</i> if <tt>(e1==null ? e2==null :
         * e1.equals(e2))</tt>.)  In other words, two lists are defined to be
         * equal if they contain the same elements in the same order.  This
         * definition ensures that the equals method works properly across
         * different implementations of the <tt>List</tt> interface.
         *
         * @param o the object to be compared for equality with this list.
         * @return <tt>true</tt> if the specified object is equal to this list.
         */
        public override bool Equals(Object o)
        {
            if (o == this)
            {
                return true;
            }
            else if (!(o is java.util.List<Object>))
            {
                return false;
            }
            java.util.Iterator<Object> it = ((java.util.List<Object>)o).listIterator();
            for (Listable elt = _head.next(), past = null; null != elt && past != _head.prev(); elt = (past = elt).next())
            {
                if (!it.hasNext() || (null == elt.value() ? null != it.next() : !(elt.value().equals(it.next()))))
                {
                    return false;
                }
            }
            return !it.hasNext();
        }

        /**
         * Returns the element at the specified position in this list.
         *
         * @param index index of element to return.
         * @return the element at the specified position in this list.
         *
         * @throws IndexOutOfBoundsException if the index is out of range (index
         *           &lt; 0 || index &gt;= size()).
         */
        public virtual Object get(int index)
        {
            return getListableAt(index).value();
        }

        /**
         * Returns the element at the beginning of this list.
         */
        public virtual Object getFirst()
        {
            try
            {
                return _head.next().value();
            }
            catch (java.lang.NullPointerException e)
            {
                throw new java.util.NoSuchElementException(e.Message); // Basties note: giving more informations
            }
        }

        /**
         * Returns the element at the end of this list.
         */
        public virtual Object getLast()
        {
            try
            {
                return _head.prev().value();
            }
            catch (java.lang.NullPointerException e)
            {
                throw new java.util.NoSuchElementException(e.Message); // Basties note: giving more informations
            }
        }

        /**
         * Returns the hash code value for this list.  The hash code of a list
         * is defined to be the result of the following calculation:
         * <pre>
         *  hashCode = 1;
         *  Iterator i = list.iterator();
         *  while (i.hasNext()) {
         *      Object obj = i.next();
         *      hashCode = 31*hashCode + (obj==null ? 0 : obj.hashCode());
         *  }
         * </pre>
         * This ensures that <tt>list1.equals(list2)</tt> implies that
         * <tt>list1.hashCode()==list2.hashCode()</tt> for any two lists,
         * <tt>list1</tt> and <tt>list2</tt>, as required by the general
         * contract of <tt>Object.hashCode</tt>.
         *
         * @return the hash code value for this list.
         * @see Object#hashCode()
         * @see Object#equals(Object)
         * @see #equals(Object)
         */
        public override int GetHashCode()
        {
            int hash = 1;
            for (Listable elt = _head.next(), past = null; null != elt && past != _head.prev(); elt = (past = elt).next())
            {
                hash = 31 * hash + (null == elt.value() ? 0 : elt.value().GetHashCode());
            }
            return hash;
        }

        /**
         * Returns the index in this list of the first occurrence of the specified
         * element, or -1 if this list does not contain this element.
         * More formally, returns the lowest index <tt>i</tt> such that
         * <tt>(o==null ? get(i)==null : o.equals(get(i)))</tt>,
         * or -1 if there is no such index.
         *
         * @param o element to search for.
         * @return the index in this list of the first occurrence of the specified
         *         element, or -1 if this list does not contain this element.
         */
        public virtual int indexOf(Object o)
        {
            int ndx = 0;

            // perform the null check outside of the loop to save checking every
            // single time through the loop.
            if (null == o)
            {
                for (Listable elt = _head.next(), past = null; null != elt && past != _head.prev(); elt = (past = elt).next())
                {
                    if (null == elt.value())
                    {
                        return ndx;
                    }
                    ndx++;
                }
            }
            else
            {

                for (Listable elt = _head.next(), past = null; null != elt && past != _head.prev(); elt = (past = elt).next())
                {
                    if (o.equals(elt.value()))
                    {
                        return ndx;
                    }
                    ndx++;
                }
            }
            return -1;
        }

        /**
         * Returns <tt>true</tt> if this list contains no elements.
         * @return <tt>true</tt> if this list contains no elements.
         */
        public virtual bool isEmpty()
        {
            return (0 == _size);
        }

        /**
         * Returns a fail-fast iterator.
         * @see List#iterator
         */
        public virtual java.util.Iterator<Object> iterator()
        {
            return listIterator(0);
        }

        /**
         * Returns the index in this list of the last occurrence of the specified
         * element, or -1 if this list does not contain this element.
         * More formally, returns the highest index <tt>i</tt> such that
         * <tt>(o==null ? get(i)==null : o.equals(get(i)))</tt>,
         * or -1 if there is no such index.
         *
         * @param o element to search for.
         * @return the index in this list of the last occurrence of the specified
         *            element, or -1 if this list does not contain this element.
         */
        public virtual int lastIndexOf(Object o)
        {
            int ndx = _size - 1;

            // perform the null check outside of the loop to save checking every
            // single time through the loop.
            if (null == o)
            {
                for (Listable elt = _head.prev(), past = null; null != elt && past != _head.next(); elt = (past = elt).prev())
                {
                    if (null == elt.value())
                    {
                        return ndx;
                    }
                    ndx--;
                }
            }
            else
            {
                for (Listable elt = _head.prev(), past = null; null != elt && past != _head.next(); elt = (past = elt).prev())
                {
                    if (o.equals(elt.value()))
                    {
                        return ndx;
                    }
                    ndx--;
                }
            }
            return -1;
        }

        /**
         * Returns a fail-fast ListIterator.
         * @see List#listIterator
         */
        public virtual java.util.ListIterator<Object> listIterator()
        {
            return listIterator(0);
        }

        /**
         * Returns a fail-fast ListIterator.
         * @see List#listIterator(int)
         */
        public virtual java.util.ListIterator<Object> listIterator(int index)
        {
            if (index < 0 || index > _size)
            {
                throw new java.lang.IndexOutOfBoundsException(index + " < 0 or > " + _size);
            }
            return new ListIter(index, this);
        }

        /**
         * Removes the first occurrence in this list of the specified element.
         * If this list does not contain the element, it is
         * unchanged.  More formally, removes the element with the lowest index i
         * such that <tt>(o==null ? get(i)==null : o.equals(get(i)))</tt> (if
         * such an element exists).
         *
         * @param o element to be removed from this list, if present.
         * @return <tt>true</tt> if this list contained the specified element.
         */
        public virtual bool remove(Object o)
        {
            for (Listable elt = _head.next(), past = null; null != elt && past != _head.prev(); elt = (past = elt).next())
            {
                if (null == o && null == elt.value())
                {
                    removeListable(elt);
                    return true;
                }
                else if (o != null && o.equals(elt.value()))
                {
                    removeListable(elt);
                    return true;
                }
            }
            return false;
        }

        /**
         * Removes the element at the specified position in this list (optional
         * operation).  Shifts any subsequent elements to the left (subtracts one
         * from their indices).  Returns the element that was removed from the
         * list.
         *
         * @param index the index of the element to removed.
         * @return the element previously at the specified position.
         *
         * @throws IndexOutOfBoundsException if the index is out of range (index
         *            &lt; 0 || index &gt;= size()).
         */
        public virtual Object remove(int index)
        {
            Listable elt = getListableAt(index);
            Object ret = elt.value();
            removeListable(elt);
            return ret;
        }

        /**
         * Removes from this list all the elements that are contained in the
         * specified collection.
         *
         * @param c collection that defines which elements will be removed from
         *          this list.
         * @return <tt>true</tt> if this list changed as a result of the call.
         */
        public virtual bool removeAll(java.util.Collection<Object> c)
        {
            if (0 == c.size() || 0 == _size)
            {
                return false;
            }
            else
            {
                bool changed = false;
                java.util.Iterator<Object> it = iterator();
                while (it.hasNext())
                {
                    if (c.contains(it.next()))
                    {
                        it.remove();
                        changed = true;
                    }
                }
                return changed;
            }
        }

        /**
         * Removes the first element of this list, if any.
         */
        public virtual Object removeFirst()
        {
            if (_head.next() != null)
            {
                Object val = _head.next().value();
                removeListable(_head.next());
                return val;
            }
            else
            {
                throw new java.util.NoSuchElementException();
            }
        }

        /**
         * Removes the last element of this list, if any.
         */
        public virtual Object removeLast()
        {
            if (_head.prev() != null)
            {
                Object val = _head.prev().value();
                removeListable(_head.prev());
                return val;
            }
            else
            {
                throw new java.util.NoSuchElementException();
            }
        }

        /**
         * Retains only the elements in this list that are contained in the
         * specified collection.  In other words, removes
         * from this list all the elements that are not contained in the specified
         * collection.
         *
         * @param c collection that defines which elements this set will retain.
         *
         * @return <tt>true</tt> if this list changed as a result of the call.
         */
        public virtual bool retainAll(java.util.Collection<Object> c)
        {
            bool changed = false;
            java.util.Iterator<Object> it = iterator();
            while (it.hasNext())
            {
                if (!c.contains(it.next()))
                {
                    it.remove();
                    changed = true;
                }
            }
            return changed;
        }

        /**
         * Replaces the element at the specified position in this list with the
         * specified element.
         *
         * @param index index of element to replace.
         * @param element element to be stored at the specified position.
         * @return the element previously at the specified position.
         *
         * @throws ClassCastException if the class of the specified element
         *           prevents it from being added to this list.
         * @throws IllegalArgumentException if some aspect of the specified
         *            element prevents it from being added to this list.
         * @throws IndexOutOfBoundsException if the index is out of range
         *             (index &lt; 0 || index &gt;= size()).
         */
        public virtual Object set(int index, Object element)
        {
            Listable elt = getListableAt(index);
            Object val = elt.setValue(element);
            broadcastListableChanged(elt);
            return val;
        }

        /**
         * Returns the number of elements in this list.
         * @return the number of elements in this list.
         */
        public virtual int size()
        {
            return _size;
        }

        /**
         * Returns an array containing all of the elements in this list in proper
         * sequence.  Obeys the general contract of the {@link Collection#toArray} method.
         *
         * @return an array containing all of the elements in this list in proper
         *         sequence.
         */
        public virtual Object[] toArray()
        {
            Object[] array = new Object[_size];
            int i = 0;
            for (Listable elt = _head.next(), past = null; null != elt && past != _head.prev(); elt = (past = elt).next())
            {
                array[i++] = elt.value();
            }
            return array;
        }

        /**
         * Returns an array containing all of the elements in this list in proper
         * sequence; the runtime type of the returned array is that of the
         * specified array. Obeys the general contract of the
         * {@link Collection#toArray} method.
         *
         * @param a      the array into which the elements of this list are to
         *               be stored, if it is big enough; otherwise, a new array of the
         *               same runtime type is allocated for this purpose.
         * @return an array containing the elements of this list.
         * @exception ArrayStoreException
         *                   if the runtime type of the specified array
         *                   is not a supertype of the runtime type of every element in
         *                   this list.
         */
        public virtual Object[] toArray<Object>(Object[] a)
        {
            if (a.Length < _size)
            {
                a = new Object[_size]; //(Object[])Array.newInstance(a.getClass().getComponentType(), _size);
            }
            int i = 0;
            for (Listable elt = _head.next(), past = null; null != elt && past != _head.prev(); elt = (past = elt).next())
            {
                a[i++] = (Object)elt.value();
            }
            if (a.Length > _size)
            {
                a[_size] = default(Object); // should we null out the rest of the array also? java.util.LinkedList doesn't
            }
            return a;
        }

        /**
         * Returns a {@link String} representation of this list, suitable for debugging.
         * @return a {@link String} representation of this list, suitable for debugging.
         */
        public override String ToString()
        {
            java.lang.StringBuffer buf = new java.lang.StringBuffer();
            buf.append("[");
            for (Listable elt = _head.next(), past = null; null != elt && past != _head.prev(); elt = (past = elt).next())
            {
                if (_head.next() != elt)
                {
                    buf.append(", ");
                }
                buf.append(elt.value());
            }
            buf.append("]");
            return buf.toString();
        }

        /**
         * Returns a fail-fast sublist.
         * @see List#subList(int,int)
         */
        public virtual java.util.List<Object> subList(int i, int j)
        {
            if (i < 0 || j > _size || i > j)
            {
                throw new java.lang.IndexOutOfBoundsException();
            }
            else if (i == 0 && j == _size)
            {
                return this;
            }
            else
            {
                return new CursorableSubList(this, i, j);
            }
        }

        //--- protected methods ------------------------------------------

        /**
         * Inserts a new <i>value</i> into my
         * list, after the specified <i>before</i> element, and before the
         * specified <i>after</i> element
         *
         * @return the newly created 
         * {@link org.apache.commons.collections.CursorableLinkedList.Listable}
         */
        protected internal virtual Listable insertListable(Listable before, Listable after, Object value)
        {
            _modCount++;
            _size++;
            Listable elt = new Listable(before, after, value);
            if (null != before)
            {
                before.setNext(elt);
            }
            else
            {
                _head.setNext(elt);
            }

            if (null != after)
            {
                after.setPrev(elt);
            }
            else
            {
                _head.setPrev(elt);
            }
            broadcastListableInserted(elt);
            return elt;
        }

        /**
         * Removes the given 
         * {@link org.apache.commons.collections.CursorableLinkedList.Listable} 
         * from my list.
         */
        protected internal virtual void removeListable(Listable elt)
        {
            _modCount++;
            _size--;
            if (_head.next() == elt)
            {
                _head.setNext(elt.next());
            }
            if (null != elt.next())
            {
                elt.next().setPrev(elt.prev());
            }
            if (_head.prev() == elt)
            {
                _head.setPrev(elt.prev());
            }
            if (null != elt.prev())
            {
                elt.prev().setNext(elt.next());
            }
            broadcastListableRemoved(elt);
        }

        /**
         * Returns the 
         * {@link org.apache.commons.collections.CursorableLinkedList.Listable} 
         * at the specified index.
         *
         * @throws IndexOutOfBoundsException if index is less than zero or
         *         greater than or equal to the size of this list.
         */
        protected internal virtual Listable getListableAt(int index)
        {
            if (index < 0 || index >= _size)
            {
                throw new java.lang.IndexOutOfBoundsException("" + index + " < 0 or " + index + " >= " + _size);
            }
            if (index <= _size / 2)
            {
                Listable elt = _head.next();
                for (int i = 0; i < index; i++)
                {
                    elt = elt.next();
                }
                return elt;
            }
            else
            {
                Listable elt = _head.prev();
                for (int i = (_size - 1); i > index; i--)
                {
                    elt = elt.prev();
                }
                return elt;
            }
        }

        /**
         * Registers a {@link CursorableLinkedList.Cursor} to be notified
         * of changes to this list.
         */
        protected virtual void registerCursor(Cursor cur)
        {
            // We take this opportunity to clean the _cursors list
            // of WeakReference objects to garbage-collected cursors.
            for (java.util.Iterator<Object> it = _cursors.iterator(); it.hasNext(); )
            {
                java.lang.refj.WeakReference<Object> refJ = (java.lang.refj.WeakReference<Object>)it.next();
                if (refJ.get() == null)
                {
                    it.remove();
                }
            }

            _cursors.add(new WeakReference(cur));
        }

        /**
         * Removes a {@link CursorableLinkedList.Cursor} from
         * the set of cursors to be notified of changes to this list.
         */
        protected virtual void unregisterCursor(Cursor cur)
        {
            for (java.util.Iterator<Object> it = _cursors.iterator(); it.hasNext(); )
            {
                java.lang.refj.WeakReference<Object> refJ = (java.lang.refj.WeakReference<Object>)it.next();
                Cursor cursor = (Cursor)refJ.get();
                if (cursor == null)
                {
                    // some other unrelated cursor object has been 
                    // garbage-collected; let's take the opportunity to
                    // clean up the cursors list anyway..
                    it.remove();

                }
                else if (cursor == cur)
                {
                    refJ.clear();
                    it.remove();
                    break;
                }
            }
        }

        /**
         * Informs all of my registered cursors that they are now
         * invalid.
         */
        protected virtual void invalidateCursors()
        {
            java.util.Iterator<Object> it = _cursors.iterator();
            while (it.hasNext())
            {
                java.lang.refj.WeakReference<Object> refJ = (java.lang.refj.WeakReference<Object>)it.next();
                Cursor cursor = (Cursor)refJ.get();
                if (cursor != null)
                {
                    // cursor is null if object has been garbage-collected
                    cursor.invalidate();
                    refJ.clear();
                }
                it.remove();
            }
        }

        /**
         * Informs all of my registered cursors that the specified
         * element was changed.
         * @see #set(int,java.lang.Object)
         */
        protected virtual void broadcastListableChanged(Listable elt)
        {
            java.util.Iterator<Object> it = _cursors.iterator();
            while (it.hasNext())
            {
                java.lang.refj.WeakReference<Object> refJ = (java.lang.refj.WeakReference<Object>)it.next();
                Cursor cursor = (Cursor)refJ.get();
                if (cursor == null)
                {
                    it.remove(); // clean up list
                }
                else
                {
                    cursor.listableChanged(elt);
                }
            }
        }

        /**
         * Informs all of my registered cursors that the specified
         * element was just removed from my list.
         */
        protected virtual void broadcastListableRemoved(Listable elt)
        {
            java.util.Iterator<Object> it = _cursors.iterator();
            while (it.hasNext())
            {
                java.lang.refj.WeakReference<Object> refJ = (java.lang.refj.WeakReference<Object>)it.next();
                Cursor cursor = (Cursor)refJ.get();
                if (cursor == null)
                {
                    it.remove(); // clean up list
                }
                else
                {
                    cursor.listableRemoved(elt);
                }
            }
        }

        /**
         * Informs all of my registered cursors that the specified
         * element was just added to my list.
         */
        protected virtual void broadcastListableInserted(Listable elt)
        {
            java.util.Iterator<Object> it = _cursors.iterator();
            while (it.hasNext())
            {
                java.lang.refj.WeakReference<Object> refj = (java.lang.refj.WeakReference<Object>)it.next();
                Cursor cursor = (Cursor)refj.get();
                if (cursor == null)
                {
                    it.remove();  // clean up list
                }
                else
                {
                    cursor.listableInserted(elt);
                }
            }
        }

        private void writeObject(java.io.ObjectOutputStream outJ)
        {//throws IOException {
            outJ.defaultWriteObject();
            outJ.writeInt(_size);
            Listable cur = _head.next();
            while (cur != null)
            {
                outJ.writeObject(cur.value());
                cur = cur.next();
            }
        }

        private void readObject(java.io.ObjectInputStream inJ)
        {//throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            _size = 0;
            _modCount = 0;
            _cursors = new java.util.ArrayList<Object>();
            _head = new Listable(null, null, null);
            int size = inJ.readInt();
            for (int i = 0; i < size; i++)
            {
                this.add(inJ.readObject());
            }
        }

        //--- protected attributes ---------------------------------------

        /** The number of elements in me. */
        [NonSerialized]
        protected int _size = 0;

        /**
         * A sentry node.
         * <p>
         * <tt>_head.next()</tt> points to the first element in the list,
         * <tt>_head.prev()</tt> to the last. Note that it is possible for
         * <tt>_head.next().prev()</tt> and <tt>_head.prev().next()</tt> to be
         * non-null, as when I am a sublist for some larger list.
         * Use <tt>== _head.next()</tt> and <tt>== _head.prev()</tt> to determine
         * if a given 
         * {@link org.apache.commons.collections.CursorableLinkedList.Listable} 
         * is the first or last element in the list.
         */
        [NonSerialized]
        protected Listable _head = new Listable(null, null, null);

        /** Tracks the number of structural modifications to me. */
        [NonSerialized]
        protected internal int _modCount = 0;

        /**
         * A list of the currently {@link CursorableLinkedList.Cursor}s currently
         * open in this list.
         */
        [NonSerialized]
        protected java.util.List<Object> _cursors = new java.util.ArrayList<Object>();

        //--- inner classes ----------------------------------------------

        [Serializable]
        public class Listable : java.io.Serializable
        {
            private Listable _prev = null;
            private Listable _next = null;
            private Object _val = null;

            internal Listable(Listable prev, Listable next, Object val)
            {
                _prev = prev;
                _next = next;
                _val = val;
            }

            internal virtual Listable next()
            {
                return _next;
            }

            internal virtual Listable prev()
            {
                return _prev;
            }

            internal virtual Object value()
            {
                return _val;
            }

            internal virtual void setNext(Listable next)
            {
                _next = next;
            }

            internal virtual void setPrev(Listable prev)
            {
                _prev = prev;
            }

            internal virtual Object setValue(Object val)
            {
                Object temp = _val;
                _val = val;
                return temp;
            }
        }

        public class ListIter : java.util.ListIterator<Object>
        {
            private readonly CursorableLinkedList root;
            public Listable _cur = null;
            protected internal Listable _lastReturned = null;
            protected int _expectedModCount;// = _modCount;
            protected int _nextIndex = 0;

            internal ListIter(int index, CursorableLinkedList root)
            {
                this.root = root;
                this._expectedModCount = root._modCount;
                if (index == 0)
                {
                    _cur = new Listable(null, root._head.next(), null);
                    _nextIndex = 0;
                }
                else if (index == root._size)
                {
                    _cur = new Listable(root._head.prev(), null, null);
                    _nextIndex = root._size;
                }
                else
                {
                    Listable temp = root.getListableAt(index);
                    _cur = new Listable(temp.prev(), temp, null);
                    _nextIndex = index;
                }
            }

            public virtual Object previous()
            {
                checkForComod();
                if (!hasPrevious())
                {
                    throw new java.util.NoSuchElementException();
                }
                else
                {
                    Object ret = _cur.prev().value();
                    _lastReturned = _cur.prev();
                    _cur.setNext(_cur.prev());
                    _cur.setPrev(_cur.prev().prev());
                    _nextIndex--;
                    return ret;
                }
            }

            public virtual bool hasNext()
            {
                checkForComod();
                return (null != _cur.next() && _cur.prev() != root._head.prev());
            }

            public virtual Object next()
            {
                checkForComod();
                if (!hasNext())
                {
                    throw new java.util.NoSuchElementException();
                }
                else
                {
                    Object ret = _cur.next().value();
                    _lastReturned = _cur.next();
                    _cur.setPrev(_cur.next());
                    _cur.setNext(_cur.next().next());
                    _nextIndex++;
                    return ret;
                }
            }

            public virtual int previousIndex()
            {
                checkForComod();
                if (!hasPrevious())
                {
                    return -1;
                }
                return _nextIndex - 1;
            }

            public virtual bool hasPrevious()
            {
                checkForComod();
                return (null != _cur.prev() && _cur.next() != root._head.next());
            }

            public virtual void set(Object o)
            {
                checkForComod();
                try
                {
                    _lastReturned.setValue(o);
                }
                catch (java.lang.NullPointerException e)
                {
                    throw new java.lang.IllegalStateException(e); // Basties note: giving more informations
                }
            }

            public virtual int nextIndex()
            {
                checkForComod();
                if (!hasNext())
                {
                    return root.size();
                }
                return _nextIndex;
            }

            public virtual void remove()
            {
                checkForComod();
                if (null == _lastReturned)
                {
                    throw new java.lang.IllegalStateException();
                }
                else
                {
                    _cur.setNext(_lastReturned == root._head.prev() ? null : _lastReturned.next());
                    _cur.setPrev(_lastReturned == root._head.next() ? null : _lastReturned.prev());
                    root.removeListable(_lastReturned);
                    _lastReturned = null;
                    _nextIndex--;
                    _expectedModCount++;
                }
            }

            public virtual void add(Object o)
            {
                checkForComod();
                _cur.setPrev(root.insertListable(_cur.prev(), _cur.next(), o));
                _lastReturned = null;
                _nextIndex++;
                _expectedModCount++;
            }

            protected virtual void checkForComod()
            {
                if (_expectedModCount != root._modCount)
                {
                    throw new java.util.ConcurrentModificationException();
                }
            }
        }

        public class Cursor : ListIter, java.util.ListIterator<Object>
        {
            bool _valid = false;
            private CursorableLinkedList root;
            internal Cursor(int index, CursorableLinkedList root)
                : base(index, root)
            {
                this.root = root;
                _valid = true;
                root.registerCursor(this);
            }

            public override int previousIndex()
            {
                throw new java.lang.UnsupportedOperationException();
            }

            public override int nextIndex()
            {
                throw new java.lang.UnsupportedOperationException();
            }

            public override void add(Object o)
            {
                checkForComod();
                Listable elt = root.insertListable(_cur.prev(), _cur.next(), o);
                _cur.setPrev(elt);
                _cur.setNext(elt.next());
                _lastReturned = null;
                _nextIndex++;
                _expectedModCount++;
            }

            protected internal virtual void listableRemoved(Listable elt)
            {
                if (null == root._head.prev())
                {
                    _cur.setNext(null);
                }
                else if (_cur.next() == elt)
                {
                    _cur.setNext(elt.next());
                }
                if (null == root._head.next())
                {
                    _cur.setPrev(null);
                }
                else if (_cur.prev() == elt)
                {
                    _cur.setPrev(elt.prev());
                }
                if (_lastReturned == elt)
                {
                    _lastReturned = null;
                }
            }

            protected internal virtual void listableInserted(Listable elt)
            {
                if (null == _cur.next() && null == _cur.prev())
                {
                    _cur.setNext(elt);
                }
                else if (_cur.prev() == elt.prev())
                {
                    _cur.setNext(elt);
                }
                if (_cur.next() == elt.next())
                {
                    _cur.setPrev(elt);
                }
                if (_lastReturned == elt)
                {
                    _lastReturned = null;
                }
            }

            protected internal virtual void listableChanged(Listable elt)
            {
                if (_lastReturned == elt)
                {
                    _lastReturned = null;
                }
            }

            protected override void checkForComod()
            {
                if (!_valid)
                {
                    throw new java.util.ConcurrentModificationException();
                }
            }

            protected internal void invalidate()
            {
                _valid = false;
            }

            /**
             * Mark this cursor as no longer being needed. Any resources
             * associated with this cursor are immediately released.
             * In previous versions of this class, it was mandatory to close
             * all cursor objects to avoid memory leaks. It is <i>no longer</i>
             * necessary to call this close method; an instance of this class
             * can now be treated exactly like a normal iterator.
             */
            public void close()
            {
                if (_valid)
                {
                    _valid = false;
                    root.unregisterCursor(this);
                }
            }
        }

    }

    /**
     * @deprecated Use new version in list subpackage, which has been rewritten
     *  and now returns the cursor from the listIterator method. Will be removed in v4.0
     */
    class CursorableSubList : CursorableLinkedList, java.util.List<Object>
    {

        //--- constructors -----------------------------------------------

        internal CursorableSubList(CursorableLinkedList list, int from, int to)
        {
            if (0 > from || list.size() < to)
            {
                throw new java.lang.IndexOutOfBoundsException();
            }
            else if (from > to)
            {
                throw new java.lang.IllegalArgumentException();
            }
            _list = list;
            if (from < list.size())
            {
                _head.setNext(_list.getListableAt(from));
                _pre = (null == _head.next()) ? null : _head.next().prev();
            }
            else
            {
                _pre = _list.getListableAt(from - 1);
            }
            if (from == to)
            {
                _head.setNext(null);
                _head.setPrev(null);
                if (to < list.size())
                {
                    _post = _list.getListableAt(to);
                }
                else
                {
                    _post = null;
                }
            }
            else
            {
                _head.setPrev(_list.getListableAt(to - 1));
                _post = _head.prev().next();
            }
            _size = to - from;
            _modCount = _list._modCount;
        }

        //--- public methods ------------------------------------------

        public override void clear()
        {
            checkForComod();
            java.util.Iterator<Object> it = iterator();
            while (it.hasNext())
            {
                it.next();
                it.remove();
            }
        }

        public override java.util.Iterator<Object> iterator()
        {
            checkForComod();
            return base.iterator();
        }

        public override int size()
        {
            checkForComod();
            return base.size();
        }

        public override bool isEmpty()
        {
            checkForComod();
            return base.isEmpty();
        }

        public override Object[] toArray()
        {
            checkForComod();
            return base.toArray();
        }

        public override Object[] toArray<Object>(Object[] a)
        {
            checkForComod();
            return base.toArray(a);
        }

        public override bool contains(Object o)
        {
            checkForComod();
            return base.contains(o);
        }

        public override bool remove(Object o)
        {
            checkForComod();
            return base.remove(o);
        }

        public override Object removeFirst()
        {
            checkForComod();
            return base.removeFirst();
        }

        public override Object removeLast()
        {
            checkForComod();
            return base.removeLast();
        }

        public override bool addAll(java.util.Collection<Object> c)
        {
            checkForComod();
            return base.addAll(c);
        }

        public override bool add(Object o)
        {
            checkForComod();
            return base.add(o);
        }

        public override bool addFirst(Object o)
        {
            checkForComod();
            return base.addFirst(o);
        }

        public override bool addLast(Object o)
        {
            checkForComod();
            return base.addLast(o);
        }

        public override bool removeAll(java.util.Collection<Object> c)
        {
            checkForComod();
            return base.removeAll(c);
        }

        public override bool containsAll(java.util.Collection<Object> c)
        {
            checkForComod();
            return base.containsAll(c);
        }

        public override bool addAll(int index, java.util.Collection<Object> c)
        {
            checkForComod();
            return base.addAll(index, c);
        }

        public override int GetHashCode()
        {
            checkForComod();
            return base.GetHashCode();
        }

        public override bool retainAll(java.util.Collection<Object> c)
        {
            checkForComod();
            return base.retainAll(c);
        }

        public override Object set(int index, Object element)
        {
            checkForComod();
            return base.set(index, element);
        }

        public override bool Equals(Object o)
        {
            checkForComod();
            return base.Equals(o);
        }

        public override Object get(int index)
        {
            checkForComod();
            return base.get(index);
        }

        public override Object getFirst()
        {
            checkForComod();
            return base.getFirst();
        }

        public override Object getLast()
        {
            checkForComod();
            return base.getLast();
        }

        public override void add(int index, Object element)
        {
            checkForComod();
            base.add(index, element);
        }

        public override java.util.ListIterator<Object> listIterator(int index)
        {
            checkForComod();
            return base.listIterator(index);
        }

        public override Object remove(int index)
        {
            checkForComod();
            return base.remove(index);
        }

        public override int indexOf(Object o)
        {
            checkForComod();
            return base.indexOf(o);
        }

        public override int lastIndexOf(Object o)
        {
            checkForComod();
            return base.lastIndexOf(o);
        }

        public override java.util.ListIterator<Object> listIterator()
        {
            checkForComod();
            return base.listIterator();
        }

        public override java.util.List<Object> subList(int fromIndex, int toIndex)
        {
            checkForComod();
            return base.subList(fromIndex, toIndex);
        }

        //--- protected methods ------------------------------------------

        /**
         * Inserts a new <i>value</i> into my
         * list, after the specified <i>before</i> element, and before the
         * specified <i>after</i> element
         *
         * @return the newly created {@link CursorableLinkedList.Listable}
         */
        protected internal override Listable insertListable(Listable before, Listable after, Object value)
        {
            _modCount++;
            _size++;
            Listable elt = _list.insertListable((null == before ? _pre : before), (null == after ? _post : after), value);
            if (null == _head.next())
            {
                _head.setNext(elt);
                _head.setPrev(elt);
            }
            if (before == _head.prev())
            {
                _head.setPrev(elt);
            }
            if (after == _head.next())
            {
                _head.setNext(elt);
            }
            broadcastListableInserted(elt);
            return elt;
        }

        /**
         * Removes the given {@link CursorableLinkedList.Listable} from my list.
         */
        protected internal override void removeListable(Listable elt)
        {
            _modCount++;
            _size--;
            if (_head.next() == elt && _head.prev() == elt)
            {
                _head.setNext(null);
                _head.setPrev(null);
            }
            if (_head.next() == elt)
            {
                _head.setNext(elt.next());
            }
            if (_head.prev() == elt)
            {
                _head.setPrev(elt.prev());
            }
            _list.removeListable(elt);
            broadcastListableRemoved(elt);
        }

        /**
         * Test to see if my underlying list has been modified
         * by some other process.  If it has, throws a
         * {@link ConcurrentModificationException}, otherwise
         * quietly returns.
         *
         * @throws ConcurrentModificationException
         */
        protected virtual void checkForComod()
        {//throws ConcurrentModificationException {
            if (_modCount != _list._modCount)
            {
                throw new java.util.ConcurrentModificationException();
            }
        }

        //--- protected attributes ---------------------------------------

        /** My underlying list */
        protected CursorableLinkedList _list = null;

        /** The element in my underlying list preceding the first element in my list. */
        protected Listable _pre = null;

        /** The element in my underlying list following the last element in my list. */
        protected Listable _post = null;

    }
}