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

namespace biz.ritter.javapi.util
{

    /**
     * TreeSet is an implementation of SortedSet. All optional operations (adding
     * and removing) are supported. The elements can be any objects which are
     * comparable to each other either using their natural order or a specified
     * Comparator.
     * 
     * @since 1.2
     */
    [Serializable]
    public class TreeSet<E> : AbstractSet<E>, NavigableSet<E>,
            java.lang.Cloneable, java.io.Serializable
    {

        private static readonly long serialVersionUID = -2479143000061671589L;

        /** Keys are this set's elements. Values are always Boolean.TRUE */
        [NonSerialized]
        private NavigableMap<E, Object> backingMap;

        [NonSerialized]
        private NavigableSet<E> descendingSetJ;

        TreeSet(NavigableMap<E, Object> map)
        {
            backingMap = map;
        }

        /**
         * Constructs a new empty instance of {@code TreeSet} which uses natural
         * ordering.
         */
        public TreeSet()
        {
            backingMap = new TreeMap<E, Object>();
        }

        /**
         * Constructs a new instance of {@code TreeSet} which uses natural ordering
         * and containing the unique elements in the specified collection.
         * 
         * @param collection
         *            the collection of elements to add.
         * @throws ClassCastException
         *                when an element in the collection does not implement the
         *                Comparable interface, or the elements in the collection
         *                cannot be compared.
         */
        public TreeSet(Collection<E> collection)
            : this()
        {
            addAll(collection);
        }

        /**
         * Constructs a new empty instance of {@code TreeSet} which uses the
         * specified comparator.
         * 
         * @param comparator
         *            the comparator to use.
         */
        public TreeSet(Comparator<E> comparator)
        {
            backingMap = new TreeMap<E, Object>(comparator);
        }

        /**
         * Constructs a new instance of {@code TreeSet} containing the elements of
         * the specified SortedSet and using the same Comparator.
         * 
         * @param set
         *            the SortedSet of elements to add.
         */
        public TreeSet(SortedSet<E> set)
            : this(set.comparator())
        {
            Iterator<E> it = set.iterator();
            while (it.hasNext())
            {
                add(it.next());
            }
        }

        /**
         * Adds the specified object to this {@code TreeSet}.
         * 
         * @param object
         *            the object to add.
         * @return {@code true} when this {@code TreeSet} did not already contain
         *         the object, {@code false} otherwise.
         * @throws ClassCastException
         *             when the object cannot be compared with the elements in this
         *             {@code TreeSet}.
         * @throws NullPointerException
         *             when the object is null and the comparator cannot handle
         *             null.
         */

        public override bool add(E obj)
        {
            return backingMap.put(obj, java.lang.Boolean.TRUE) == null;
        }

        /**
         * Adds the objects in the specified collection to this {@code TreeSet}.
         * 
         * @param collection
         *            the collection of objects to add.
         * @return {@code true} if this {@code TreeSet} was modified, {@code false}
         *         otherwise.
         * @throws ClassCastException
         *             when an object in the collection cannot be compared with the
         *             elements in this {@code TreeSet}.
         * @throws NullPointerException
         *             when an object in the collection is null and the comparator
         *             cannot handle null.
         */
        public override bool addAll(Collection<E> collection)
        {
            return base.addAll(collection);
        }

        /**
         * Removes all elements from this {@code TreeSet}, leaving it empty.
         * 
         * @see #isEmpty
         * @see #size
         */

        public override void clear()
        {
            backingMap.clear();
        }

        /**
         * Returns a new {@code TreeSet} with the same elements, size and comparator
         * as this {@code TreeSet}.
         * 
         * @return a shallow copy of this {@code TreeSet}.
         * @see java.lang.Cloneable
         */
        public Object clone()
        {
            try
            {
                TreeSet<E> clone = (TreeSet<E>)base.MemberwiseClone();
                if (backingMap is TreeMap<E, Object>)
                {
                    clone.backingMap = (NavigableMap<E, Object>)((TreeMap<E, Object>)backingMap)
                            .clone();
                }
                else
                {
                    clone.backingMap = new TreeMap<E, Object>(backingMap);
                }
                return clone;
            }
            catch (java.lang.CloneNotSupportedException e)
            {
                return null;
            }
        }

        /**
         * Returns the comparator used to compare elements in this {@code TreeSet}.
         * 
         * @return a Comparator or null if the natural ordering is used
         */
        public Comparator<E> comparator()
        {
            return backingMap.comparator();
        }

        /**
         * Searches this {@code TreeSet} for the specified object.
         * 
         * @param object
         *            the object to search for.
         * @return {@code true} if {@code object} is an element of this
         *         {@code TreeSet}, {@code false} otherwise.
         * @throws ClassCastException
         *             when the object cannot be compared with the elements in this
         *             {@code TreeSet}.
         * @throws NullPointerException
         *             when the object is null and the comparator cannot handle
         *             null.
         */

        public override bool contains(Object obj)
        {
            return backingMap.containsKey(obj);
        }

        /**
         * Returns true if this {@code TreeSet} has no element, otherwise false.
         * 
         * @return true if this {@code TreeSet} has no element.
         * @see #size
         */

        public override bool isEmpty()
        {
            return backingMap.isEmpty();
        }

        /**
         * Returns an Iterator on the elements of this {@code TreeSet}.
         * 
         * @return an Iterator on the elements of this {@code TreeSet}.
         * @see Iterator
         */

        public override Iterator<E> iterator()
        {
            return backingMap.keySet().iterator();
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableSet#descendingIterator()
         * @since 1.6
         */
        public Iterator<E> descendingIterator()
        {
            return descendingSet().iterator();
        }

        /**
         * Removes an occurrence of the specified object from this {@code TreeSet}.
         * 
         * @param object
         *            the object to remove.
         * @return {@code true} if this {@code TreeSet} was modified, {@code false}
         *         otherwise.
         * @throws ClassCastException
         *             when the object cannot be compared with the elements in this
         *             {@code TreeSet}.
         * @throws NullPointerException
         *             when the object is null and the comparator cannot handle
         *             null.
         */

        public override bool remove(Object obj)
        {
            return backingMap.remove(obj) != null;
        }

        /**
         * Returns the number of elements in this {@code TreeSet}.
         * 
         * @return the number of elements in this {@code TreeSet}.
         */

        public override int size()
        {
            return backingMap.size();
        }

        /**
         * Answers the first element in this TreeSet.
         * 
         * @return the first element
         * 
         * @exception NoSuchElementException
         *                when this TreeSet is empty
         */
        public E first()
        {
            return backingMap.firstKey();
        }

        /**
         * Answers the last element in this TreeSet.
         * 
         * @return the last element
         * 
         * @exception NoSuchElementException
         *                when this TreeSet is empty
         */
        public E last()
        {
            return backingMap.lastKey();
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableSet#pollFirst()
         * @since 1.6
         */
        public E pollFirst()
        {
            MapNS.Entry<E, Object> entry = backingMap.pollFirstEntry();
            return (null == entry) ? default(E) : entry.getKey();
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableSet#pollLast()
         * @since 1.6
         */
        public E pollLast()
        {
            MapNS.Entry<E, Object> entry = backingMap.pollLastEntry();
            return (null == entry) ? default(E) : entry.getKey();
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableSet#higher(java.lang.Object)
         * @since 1.6
         */
        public E higher(E e)
        {
            return backingMap.higherKey(e);
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableSet#lower(java.lang.Object)
         * @since 1.6
         */
        public E lower(E e)
        {
            return backingMap.lowerKey(e);
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableSet#ceiling(java.lang.Object)
         * @since 1.6
         */
        public E ceiling(E e)
        {
            return backingMap.ceilingKey(e);
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableSet#floor(java.lang.Object)
         * @since 1.6
         */
        public E floor(E e)
        {
            return backingMap.floorKey(e);
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableSet#descendingSet()
         * @since 1.6
         */
        public NavigableSet<E> descendingSet()
        {

            return (null != descendingSetJ) ? descendingSetJ
                    : (descendingSetJ = new TreeSet<E>(backingMap.descendingMap()));
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableSet#subSet(Object, boolean, Object, boolean)
         * @since 1.6
         */
        public NavigableSet<E> subSet(E start, bool startInclusive, E end,
                bool endInclusive)
        {
            Comparator<E> c = backingMap.comparator();
            int compare = (c == null) ? ((java.lang.Comparable<E>)start).compareTo(end) : c
                    .compare(start, end);
            if (compare <= 0)
            {
                return new TreeSet<E>(backingMap.subMap(start, startInclusive, end,
                        endInclusive));
            }
            throw new java.lang.IllegalArgumentException();
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableSet#headSet(Object, boolean)
         * @since 1.6
         */
        public NavigableSet<E> headSet(E end, bool endInclusive)
        {
            // Check for errors
            Comparator<E> c = backingMap.comparator();
            if (c == null)
            {
                ((java.lang.Comparable<E>)end).compareTo(end);
            }
            else
            {
                c.compare(end, end);
            }
            return new TreeSet<E>(backingMap.headMap(end, endInclusive));
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableSet#tailSet(Object, boolean)
         * @since 1.6
         */
        public NavigableSet<E> tailSet(E start, bool startInclusive)
        {
            // Check for errors
            Comparator<E> c = backingMap.comparator();
            if (c == null)
            {
                ((java.lang.Comparable<E>)start).compareTo(start);
            }
            else
            {
                c.compare(start, start);
            }
            return new TreeSet<E>(backingMap.tailMap(start, startInclusive));
        }

        /**
         * Answers a SortedSet of the specified portion of this TreeSet which
         * contains elements greater or equal to the start element but less than the
         * end element. The returned SortedSet is backed by this TreeSet so changes
         * to one are reflected by the other.
         * 
         * @param start
         *            the start element
         * @param end
         *            the end element
         * @return a subset where the elements are greater or equal to
         *         <code>start</code> and less than <code>end</code>
         * 
         * @exception ClassCastException
         *                when the start or end object cannot be compared with the
         *                elements in this TreeSet
         * @exception NullPointerException
         *                when the start or end object is null and the comparator
         *                cannot handle null
         */
        public SortedSet<E> subSet(E start, E end)
        {
            return subSet(start, true, end, false);
        }

        /**
         * Answers a SortedSet of the specified portion of this TreeSet which
         * contains elements less than the end element. The returned SortedSet is
         * backed by this TreeSet so changes to one are reflected by the other.
         * 
         * @param end
         *            the end element
         * @return a subset where the elements are less than <code>end</code>
         * 
         * @exception ClassCastException
         *                when the end object cannot be compared with the elements
         *                in this TreeSet
         * @exception NullPointerException
         *                when the end object is null and the comparator cannot
         *                handle null
         */
        public SortedSet<E> headSet(E end)
        {
            return headSet(end, false);
        }

        /**
         * Answers a SortedSet of the specified portion of this TreeSet which
         * contains elements greater or equal to the start element. The returned
         * SortedSet is backed by this TreeSet so changes to one are reflected by
         * the other.
         * 
         * @param start
         *            the start element
         * @return a subset where the elements are greater or equal to
         *         <code>start</code>
         * 
         * @exception ClassCastException
         *                when the start object cannot be compared with the elements
         *                in this TreeSet
         * @exception NullPointerException
         *                when the start object is null and the comparator cannot
         *                handle null
         */
        public SortedSet<E> tailSet(E start)
        {
            return tailSet(start, true);
        }

        private void writeObject(java.io.ObjectOutputStream stream)
        {//throws IOException {
            stream.defaultWriteObject();
            stream.writeObject(backingMap.comparator());
            int size = backingMap.size();
            stream.writeInt(size);
            if (size > 0)
            {
                Iterator<E> it = backingMap.keySet().iterator();
                while (it.hasNext())
                {
                    stream.writeObject(it.next());
                }
            }
        }

        private void readObject(java.io.ObjectInputStream stream)
        {//throws IOException,ClassNotFoundException 
            {
                stream.defaultReadObject();
                TreeMap<E, Object> map = new TreeMap<E, Object>(
                        (Comparator<E>)stream.readObject());
                int size = stream.readInt();
                if (size > 0)
                {
                    TreeMap<E,Object>.Node<E, Object> lastNode = null;
                    for (int i = 0; i < size; i++)
                    {
                        E elem = (E)stream.readObject();
                        lastNode = map.addToLast(lastNode, elem, java.lang.Boolean.TRUE);
                    }
                }
                backingMap = map;
            }
        }
    }
}