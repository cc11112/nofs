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
    /// <summary>
    /// <c>Collections</c> contains static methods which operate on <c>Collection</c> classes.
    /// </summary>
    public class Collections
    {
    /**
     * Returns a wrapper on the specified sorted map which synchronizes all
     * access to the sorted map.
     * 
     * @param map
     *            the sorted map to wrap in a synchronized sorted map.
     * @return a synchronized sorted map.
     */
        public static SortedMap<K, V> synchronizedSortedMap<K, V>(
            SortedMap<K, V> map) {
        if (map == null) {
            throw new java.lang.NullPointerException();
        }
        return new SynchronizedSortedMap<K, V>(map);
    }
        [Serializable]
     class SynchronizedSortedMap<K, V> : SynchronizedMap<K, V>
            , SortedMap<K, V> {
        private static readonly long serialVersionUID = -8798146769416483793L;

        private readonly SortedMap<K, V> sm;

        internal SynchronizedSortedMap(SortedMap<K, V> map) :base(map){
            sm = map;
        }

        internal SynchronizedSortedMap(SortedMap<K, V> map, Object mutex) :base(map,mutex){
            sm = map;
        }

        public Comparator<K> comparator() {
            lock (mutex) {
                return sm.comparator();
            }
        }

        public K firstKey() {
            lock (mutex) {
                return sm.firstKey();
            }
        }

        public SortedMap<K, V> headMap(K endKey) {
            lock (mutex) {
                return new SynchronizedSortedMap<K, V>(sm.headMap(endKey),
                        mutex);
            }
        }

        public K lastKey() {
            lock (mutex) {
                return sm.lastKey();
            }
        }

        public SortedMap<K, V> subMap(K startKey, K endKey) {
            lock (mutex) {
                return new SynchronizedSortedMap<K, V>(sm.subMap(startKey,
                        endKey), mutex);
            }
        }

        public SortedMap<K, V> tailMap(K startKey) {
            lock (mutex) {
                return new SynchronizedSortedMap<K, V>(sm.tailMap(startKey),
                        mutex);
            }
        }

        private void writeObject(java.io.ObjectOutputStream stream) {//throws IOException {
            lock (mutex) {
                stream.defaultWriteObject();
            }
        }
    }

        /**
         * Returns a wrapper on the specified map which synchronizes all access to
         * the map.
         * 
         * @param map
         *            the map to wrap in a lock map.
         * @return a lock Map.
         */
        public static Map<K, V> synchronizedMap<K, V>(Map<K, V> map)
        {
            if (map == null)
            {
                throw new java.lang.NullPointerException();
            }
            return new SynchronizedMap<K, V>(map);
        }
        class SynchronizedMap<K, V> : Map<K, V>, java.io.Serializable
        {
            private static readonly long serialVersionUID = 1978198479659022715L;

            private readonly Map<K, V> m;

            internal readonly Object mutex;

            internal SynchronizedMap(Map<K, V> map)
            {
                m = map;
                mutex = this;
            }

            internal SynchronizedMap(Map<K, V> map, Object mutex)
            {
                m = map;
                this.mutex = mutex;
            }

            public void clear()
            {
                lock (mutex)
                {
                    m.clear();
                }
            }

            public bool containsKey(Object key)
            {
                lock (mutex)
                {
                    return m.containsKey(key);
                }
            }

            public bool containsValue(Object value)
            {
                lock (mutex)
                {
                    return m.containsValue(value);
                }
            }

            public Set<MapNS.Entry<K, V>> entrySet()
            {
                lock (mutex)
                {
                    return new SynchronizedSet<MapNS.Entry<K, V>>(m.entrySet(), mutex);
                }
            }

            public override bool Equals(Object obj)
            {
                lock (mutex)
                {
                    return m.equals(obj);
                }
            }

            public V get(Object key)
            {
                lock (mutex)
                {
                    return m.get(key);
                }
            }

            public override int GetHashCode()
            {
                lock (mutex)
                {
                    return m.GetHashCode();
                }
            }

            public bool isEmpty()
            {
                lock (mutex)
                {
                    return m.isEmpty();
                }
            }

            public Set<K> keySet()
            {
                lock (mutex)
                {
                    return new SynchronizedSet<K>(m.keySet(), mutex);
                }
            }

            public V put(K key, V value)
            {
                lock (mutex)
                {
                    return m.put(key, value);
                }
            }

            public void putAll(Map<K, V> map)
            {
                lock (mutex)
                {
                    m.putAll(map);
                }
            }

            public V remove(Object key)
            {
                lock (mutex)
                {
                    return m.remove(key);
                }
            }

            public int size()
            {
                lock (mutex)
                {
                    return m.size();
                }
            }

            public Collection<V> values()
            {
                lock (mutex)
                {
                    return new SynchronizedCollection<V>(m.values(), mutex);
                }
            }

            public override String ToString()
            {
                lock (mutex)
                {
                    return m.toString();
                }
            }

            private void writeObject(java.io.ObjectOutputStream stream)
            {//throws IOException {
                lock (mutex)
                {
                    stream.defaultWriteObject();
                }
            }
        }
        [Serializable]
        class SynchronizedSet<E> : SynchronizedCollection<E>,
                    Set<E>
        {
            private static readonly long serialVersionUID = 487447009682186044L;

            internal SynchronizedSet(Set<E> set)
                : base(set)
            {
            }

            internal SynchronizedSet(Set<E> set, Object mutex)
                : base(set, mutex)
            {
            }

            public override bool Equals(Object obj)
            {
                lock (mutex)
                {
                    return c.equals(obj);
                }
            }

            public override int GetHashCode()
            {
                lock (mutex)
                {
                    return c.GetHashCode();
                }
            }

            private void writeObject(java.io.ObjectOutputStream stream)
            {//throws IOException {
                lock (mutex)
                {
                    stream.defaultWriteObject();
                }
            }
        }
        /**
         * An empty immutable instance of {@link Set}.
         */
        public static readonly Set<Object> EMPTY_SET = new EmptySet();
        [Serializable]
        sealed class EmptySet : AbstractSet<Object>, java.io.Serializable
        {
            private static readonly long serialVersionUID = 1582296315990362920L;

            public override bool contains(Object obj)
            {
                return false;
            }

            public override int size()
            {
                return 0;
            }

            public override Iterator<Object> iterator()
            {
                return new EmptySetIterator();
            }

            private Object readResolve()
            {
                return Collections.EMPTY_SET;
            }
            //+ TODO this class is an template for en EmptyIterator
            sealed class EmptySetIterator : Iterator<Object>
            {
                public bool hasNext()
                {
                    return false;
                }

                public Object next()
                {
                    throw new NoSuchElementException();
                }

                public void remove()
                {
                    throw new java.lang.UnsupportedOperationException();
                }
            }
        }
        /**
         * An empty immutable instance of {@link List}.
         */
        public static readonly List<Object> EMPTY_LIST = new EmptyList();

        /**
         * Returns a list containing the specified number of the specified element.
         * The list cannot be modified. The list is serializable.
         * 
         * @param length
         *            the size of the returned list.
         * @param object
         *            the element to be added {@code length} times to a list.
         * @return a list containing {@code length} copies of the element.
         * @throws IllegalArgumentException
         *             when {@code length < 0}.
         */
        public static List<T> nCopies<T>(int length, T obj)
        {
            return new CopiesList<T>(length, obj);
        }

        /**
         * Returns a wrapper on the specified list which throws an
         * {@code UnsupportedOperationException} whenever an attempt is made to
         * modify the list.
         * 
         * @param list
         *            the list to wrap in an unmodifiable list.
         * @return an unmodifiable List.
         */
        public static List<E> unmodifiableList<E>(List<E> list)
        {
            if (list == null)
            {
                throw new java.lang.NullPointerException();
            }
            if (list is RandomAccess)
            {
                return new UnmodifiableRandomAccessList<E>((List<E>)list);
            }
            return new UnmodifiableList<E>((List<E>)list);
        }

        /**
         * Returns a wrapper on the specified collection which throws an
         * {@code UnsupportedOperationException} whenever an attempt is made to
         * modify the collection.
         * 
         * @param collection
         *            the collection to wrap in an unmodifiable collection.
         * @return an unmodifiable collection.
         */
        public static Collection<E> unmodifiableCollection<E>(Collection<E> collection)
        {
            if (collection == null)
            {
                throw new java.lang.NullPointerException();
            }
            return new UnmodifiableCollection<E>(collection);
        }

        /**
         * Returns a wrapper on the specified sorted map which throws an
         * {@code UnsupportedOperationException} whenever an attempt is made to
         * modify the sorted map.
         * 
         * @param map
         *            the sorted map to wrap in an unmodifiable sorted map.
         * @return a unmodifiable sorted map
         */
        public static SortedMap<K, V> unmodifiableSortedMap<K, V>(SortedMap<K, V> map)
        {
            if (map == null)
            {
                throw new java.lang.NullPointerException();
            }
            return new UnmodifiableSortedMap<K, V>((SortedMap<K, V>)map);
        }

        /**
         * Returns a wrapper on the specified sorted set which throws an
         * {@code UnsupportedOperationException} whenever an attempt is made to
         * modify the sorted set.
         * 
         * @param set
         *            the sorted set to wrap in an unmodifiable sorted set.
         * @return a unmodifiable sorted set.
         */
        public static SortedSet<E> unmodifiableSortedSet<E>(SortedSet<E> set)
        {
            if (set == null)
            {
                throw new java.lang.NullPointerException();
            }
            return new UnmodifiableSortedSet<E>(set);
        }

        /**
         * Returns a wrapper on the specified map which throws an
         * {@code UnsupportedOperationException} whenever an attempt is made to
         * modify the map.
         * 
         * @param map
         *            the map to wrap in an unmodifiable map.
         * @return a unmodifiable map.
         */
        public static Map<K, V> unmodifiableMap<K, V>(Map<K, V> map)
        {
            if (map == null)
            {
                throw new java.lang.NullPointerException();
            }
            return new UnmodifiableMap<K, V>((Map<K, V>)map);
        }

        /**
         * Returns a wrapper on the specified set which throws an
         * {@code UnsupportedOperationException} whenever an attempt is made to
         * modify the set.
         * 
         * @param set
         *            the set to wrap in an unmodifiable set.
         * @return a unmodifiable set
         */
        public static Set<E> unmodifiableSet<E>(Set<E> set)
        {
            if (set == null)
            {
                throw new java.lang.NullPointerException();
            }
            return new UnmodifiableSet<E>((Set<E>)set);
        }

        /**
         * Returns a set containing the specified element. The set cannot be
         * modified. The set is serializable.
         * 
         * @param object
         *            the element.
         * @return a set containing the element.
         */
        public static Set<E> singleton<E>(E obj)
        {
            return new SingletonSet<E>(obj);
        }

        /**
         * Returns a {@link Comparator} that reverses the order of the
         * {@code Comparator} passed. If the {@code Comparator} passed is
         * {@code null}, then this method is equivalent to {@link #reverseOrder()}.
         * <p>
         * The {@code Comparator} that's returned is {@link Serializable} if the
         * {@code Comparator} passed is serializable or {@code null}.
         *
         * @param c
         *            the {@code Comparator} to reverse or {@code null}.
         * @return a {@code Comparator} instance.
         * @see Comparator
         * @since 1.5
         */
        public static Comparator<T> reverseOrder<T>(Comparator<T> c)
        {
            if (c == null)
            {
                return reverseOrder<T>();
            }
            if (c is ReverseComparatorWithComparator<T>)
            {
                return ((ReverseComparatorWithComparator<T>)c).comparator;
            }
            return new ReverseComparatorWithComparator<T>(c);
        }
        /**
         * A comparator which reverses the natural order of the elements. The
         * {@code Comparator} that's returned is {@link Serializable}.
         *
         * @return a {@code Comparator} instance.
         * @see Comparator
         * @see Comparable
         * @see Serializable
         */
        public static Comparator<T> reverseOrder<T>()
        {
            return (Comparator<T>)ReverseComparator<T>.INSTANCE;
        }

        /**
         * This class is a singleton so that equals() and hashCode() work properly.
         */
        [Serializable]
        private sealed class ReverseComparator<T> : Comparator<T>, java.io.Serializable
        {

            internal static readonly ReverseComparator<T> INSTANCE
                    = new ReverseComparator<T>();

            private static readonly long serialVersionUID = 7207038068494060240L;

            public int compare(T o1, T o2)
            {
                java.lang.Comparable<T> c2 = (java.lang.Comparable<T>)o2;
                return c2.compareTo(o1);
            }

            private Object readResolve()
            {//throws ObjectStreamException {
                return INSTANCE;
            }

            public bool equals(Object o)
            {
                return Equals(o);
            }
        }

        [Serializable]
        private sealed class ReverseComparatorWithComparator<T> :
                Comparator<T>, java.io.Serializable
        {
            private static readonly long serialVersionUID = 4374092139857L;

            internal readonly Comparator<T> comparator;

            internal ReverseComparatorWithComparator(Comparator<T> comparator)
                : base()
            {
                this.comparator = comparator;
            }

            public int compare(T o1, T o2)
            {
                return comparator.compare(o2, o1);
            }


            public override bool Equals(Object o)
            {
                return o is ReverseComparatorWithComparator<T>
                        && ((ReverseComparatorWithComparator<T>)o).comparator
                                .equals(comparator);
            }


            public override int GetHashCode()
            {
                return ~comparator.GetHashCode();
            }
            public bool equals(Object o)
            {
                return Equals(o);
            }
        }

        [Serializable]
        sealed class EmptyList : AbstractList<Object>,
                RandomAccess, java.io.Serializable
        {
            private static readonly long serialVersionUID = 8842843931221139166L;


            public override bool contains(Object obj)
            {
                return false;
            }


            public override int size()
            {
                return 0;
            }


            public override Object get(int location)
            {
                throw new java.lang.IndexOutOfBoundsException();
            }

            private Object readResolve()
            {
                return Collections.EMPTY_LIST;
            }
        }

    }



    [Serializable]
    internal class UnmodifiableCollection<E> : Collection<E>, java.io.Serializable
    {
        private static readonly long serialVersionUID = 1820017752578914078L;

        readonly protected internal Collection<E> c;

        protected internal UnmodifiableCollection(Collection<E> collection)
        {
            c = collection;
        }

        public virtual bool add(E obj)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public virtual bool addAll(Collection<E> collection)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public virtual void clear()
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public virtual bool contains(Object obj)
        {
            return c.contains(obj);
        }

        public virtual bool containsAll(Collection<E> collection)
        {
            return c.containsAll(collection);
        }

        public virtual bool isEmpty()
        {
            return c.isEmpty();
        }

        public virtual Iterator<E> iterator()
        {
            UnmodifiableCollectionWrapperIterator<Collection<E>> w;
            w = new UnmodifiableCollectionWrapperIterator<Collection<E>>(c);
            return (Iterator<E>)w;
        }

        public virtual bool remove(Object obj)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public virtual bool removeAll(Collection<E> collection)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public virtual bool retainAll(Collection<E> collection)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public virtual int size()
        {
            return c.size();
        }

        public virtual Object[] toArray()
        {
            return c.toArray();
        }

        public virtual T[] toArray<T>(T[] array)
        {
            return c.toArray(array);
        }

        public override String ToString()
        {
            return c.ToString();
        }
    }

    [Serializable]
    internal class UnmodifiableMap<K, V> : Map<K, V>, java.io.Serializable
    {
        private static readonly long serialVersionUID = -1034234728574286014L;

        private readonly Map<K, V> m;

        protected internal UnmodifiableMap(Map<K, V> map)
        {
            m = map;
        }

        public void clear()
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public bool containsKey(Object key)
        {
            return m.containsKey(key);
        }

        public bool containsValue(Object value)
        {
            return m.containsValue(value);
        }

        public Set<MapNS.Entry<K, V>> entrySet()
        {
            return new UnmodifiableEntrySet<K, V>(m.entrySet());
        }

        public override bool Equals(Object obj)
        {
            return m.equals(obj);
        }

        public V get(Object key)
        {
            return m.get(key);
        }

        public override int GetHashCode()
        {
            return m.GetHashCode();
        }

        public bool isEmpty()
        {
            return m.isEmpty();
        }

        public Set<K> keySet()
        {
            return new UnmodifiableSet<K>(m.keySet());
        }

        public V put(K key, V value)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public void putAll(Map<K, V> map)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public V remove(Object key)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public int size()
        {
            return m.size();
        }

        public Collection<V> values()
        {
            return new UnmodifiableCollection<V>(m.values());
        }


        public override String ToString()
        {
            return m.toString();
        }
    }

    internal class UnmodifiableSet<E> : UnmodifiableCollection<E>, Set<E>
    {
        private static readonly long serialVersionUID = -9215047833775013803L;

        protected internal UnmodifiableSet(Set<E> set)
            : base(set)
        {
        }


        public override bool Equals(Object obj)
        {
            return c.Equals(obj);
        }


        public override int GetHashCode()
        {
            return c.GetHashCode();
        }
    }


    [Serializable]
    internal class SynchronizedRandomAccessList<E> : SynchronizedList<E>, RandomAccess
    {
        private static long serialVersionUID = 1530674583602358482L;

        protected internal SynchronizedRandomAccessList(java.util.List<E> l)
            : base(l)
        {
        }

        protected internal SynchronizedRandomAccessList(List<E> l, Object mutex)
            : base(l, mutex)
        {
        }

        public override List<E> subList(int start, int end)
        {
            lock (mutex)
            {
                return new SynchronizedRandomAccessList<E>(list.subList(start,
                        end), mutex);
            }
        }

        /**
         * Replaces this SynchronizedRandomAccessList with a SynchronizedList so
         * that JREs before 1.4 can deserialize this obj without any
         * problems. This is necessary since RandomAccess API was introduced
         * only in 1.4.
         * <p>
         * 
         * @return SynchronizedList
         * 
         * @see SynchronizedList#readResolve()
         */
        private Object writeReplace()
        {
            return new SynchronizedList<E>(list);
        }
    }

    [Serializable]
    internal class SynchronizedList<E> : SynchronizedCollection<E>, List<E>
    {
        private static long serialVersionUID = -7754090372962971524L;

        internal readonly List<E> list;

        internal SynchronizedList(List<E> l)
            : base(l)
        {
            list = l;
        }

        internal SynchronizedList(List<E> l, Object mutex)
            : base(l, mutex)
        {
            list = l;
        }

        public virtual void add(int location, E obj)
        {
            lock (mutex)
            {
                list.add(location, obj);
            }
        }

        public bool addAll(int location, Collection<E> collection)
        {
            lock (mutex)
            {
                return list.addAll(location, collection);
            }
        }

        public override bool Equals(Object obj)
        {
            lock (mutex)
            {
                return list.equals(obj);
            }
        }

        public E get(int location)
        {
            lock (mutex)
            {
                return list.get(location);
            }
        }


        public override int GetHashCode()
        {
            lock (mutex)
            {
                return list.GetHashCode();
            }
        }

        public int indexOf(Object obj)
        {
            int size;
            Object[] array;
            lock (mutex)
            {
                size = list.size();
                array = new Object[size];
                list.toArray(array);
            }
            if (null != obj)
                for (int i = 0; i < size; i++)
                {
                    if (obj.equals(array[i]))
                    {
                        return i;
                    }
                }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    if (null == array[i])
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public int lastIndexOf(Object obj)
        {
            int size;
            Object[] array;
            lock (mutex)
            {
                size = list.size();
                array = new Object[size];
                list.toArray(array);
            }
            if (null != obj)
                for (int i = size - 1; i >= 0; i--)
                {
                    if (obj.equals(array[i]))
                    {
                        return i;
                    }
                }
            else
            {
                for (int i = size - 1; i >= 0; i--)
                {
                    if (null == array[i])
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public ListIterator<E> listIterator()
        {
            lock (mutex)
            {
                return list.listIterator();
            }
        }

        public ListIterator<E> listIterator(int location)
        {
            lock (mutex)
            {
                return list.listIterator(location);
            }
        }

        public E remove(int location)
        {
            lock (mutex)
            {
                return list.remove(location);
            }
        }

        public E set(int location, E obj)
        {
            lock (mutex)
            {
                return list.set(location, obj);
            }
        }

        public virtual List<E> subList(int start, int end)
        {
            lock (mutex)
            {
                return new SynchronizedList<E>(list.subList(start, end), mutex);
            }
        }
        /*
                    private void writeObject(ObjectOutputStream stream) throws IOException {
                        lock (mutex) {
                            stream.defaultWriteObject();
                        }
                    }*/

        /**
         * Resolves SynchronizedList instances to SynchronizedRandomAccessList
         * instances if the underlying list is a Random Access list.
         * <p>
         * This is necessary since SynchronizedRandomAccessList instances are
         * replaced with SynchronizedList instances during serialization for
         * compliance with JREs before 1.4.
         * <p>
         * 
         * @return a SynchronizedList instance if the underlying list implements
         *         RandomAccess interface, or this same obj if not.
         * 
         * @see SynchronizedRandomAccessList#writeReplace()
         */
        private Object readResolve()
        {
            if (list is RandomAccess)
            {
                return new SynchronizedRandomAccessList<E>(list, mutex);
            }
            return this;
        }
    }

    [Serializable]
    internal class SynchronizedCollection<E> : Collection<E>, java.io.Serializable
    {
        private static readonly long serialVersionUID = 3053995032091335093L;

        protected internal readonly Collection<E> c;

        protected internal readonly Object mutex;

        protected internal SynchronizedCollection(Collection<E> collection)
        {
            c = collection;
            mutex = this;
        }

        protected internal SynchronizedCollection(Collection<E> collection, Object mutex)
        {
            c = collection;
            this.mutex = mutex;
        }

        public bool add(E obj)
        {
            lock (mutex)
            {
                return c.add(obj);
            }
        }

        public bool addAll(Collection<E> collection)
        {
            lock (mutex)
            {
                return c.addAll(collection);
            }
        }

        public void clear()
        {
            lock (mutex)
            {
                c.clear();
            }
        }

        public bool contains(Object obj)
        {
            lock (mutex)
            {
                return c.contains(obj);
            }
        }

        public bool containsAll(Collection<E> collection)
        {
            lock (mutex)
            {
                return c.containsAll(collection);
            }
        }

        public bool isEmpty()
        {
            lock (mutex)
            {
                return c.isEmpty();
            }
        }

        public Iterator<E> iterator()
        {
            lock (mutex)
            {
                return c.iterator();
            }
        }

        public bool remove(Object obj)
        {
            lock (mutex)
            {
                return c.remove(obj);
            }
        }

        public bool removeAll(Collection<E> collection)
        {
            lock (mutex)
            {
                return c.removeAll(collection);
            }
        }

        public bool retainAll(Collection<E> collection)
        {
            lock (mutex)
            {
                return c.retainAll(collection);
            }
        }

        public int size()
        {
            lock (mutex)
            {
                return c.size();
            }
        }

        public Object[] toArray()
        {
            lock (mutex)
            {
                return c.toArray();
            }
        }

        public override String ToString()
        {
            lock (mutex)
            {
                return c.toString();
            }
        }

        public T[] toArray<T>(T[] array)
        {
            lock (mutex)
            {
                return c.toArray(array);
            }
        }

        private void writeObject(java.io.ObjectOutputStream stream)
        {//throws IOException {
            lock (mutex)
            {
                stream.defaultWriteObject();
            }
        }
    }

    [Serializable]
    internal class UnmodifiableSortedMap<K, V> : UnmodifiableMap<K, V>, SortedMap<K, V>
    {
        private static readonly long serialVersionUID = -8806743815996713206L;

        private readonly SortedMap<K, V> sm;

        protected internal UnmodifiableSortedMap(SortedMap<K, V> map)
            : base(map)
        {
            sm = map;
        }

        public Comparator<K> comparator()
        {
            return sm.comparator();
        }

        public K firstKey()
        {
            return sm.firstKey();
        }

        public SortedMap<K, V> headMap(K before)
        {
            return new UnmodifiableSortedMap<K, V>(sm.headMap(before));
        }

        public K lastKey()
        {
            return sm.lastKey();
        }

        public SortedMap<K, V> subMap(K start, K end)
        {
            return new UnmodifiableSortedMap<K, V>(sm.subMap(start, end));
        }

        public SortedMap<K, V> tailMap(K after)
        {
            return new UnmodifiableSortedMap<K, V>(sm.tailMap(after));
        }
    }

    [Serializable]
    internal class UnmodifiableSortedSet<E> : UnmodifiableSet<E>, SortedSet<E>
    {
        private static readonly long serialVersionUID = -4929149591599911165L;

        private readonly SortedSet<E> ss;

        protected internal UnmodifiableSortedSet(SortedSet<E> set)
            : base(set)
        {
            ss = set;
        }

        public Comparator<E> comparator()
        {
            return ss.comparator();
        }

        public E first()
        {
            return ss.first();
        }

        public SortedSet<E> headSet(E before)
        {
            return new UnmodifiableSortedSet<E>(ss.headSet(before));
        }

        public E last()
        {
            return ss.last();
        }

        public SortedSet<E> subSet(E start, E end)
        {
            return new UnmodifiableSortedSet<E>(ss.subSet(start, end));
        }

        public SortedSet<E> tailSet(E after)
        {
            return new UnmodifiableSortedSet<E>(ss.tailSet(after));
        }
    }


    internal class UnmodifiableEntrySet<K, V> : UnmodifiableSet<MapNS.Entry<K, V>>
    {
        private static readonly long serialVersionUID = 7854390611657943733L;

        protected internal UnmodifiableEntrySet(Set<MapNS.Entry<K, V>> set)
            : base(set)
        {
        }

        public override Iterator<MapNS.Entry<K, V>> iterator()
        {
            UnmodifiableMapWrapperIterator<Object, K, V> w;
            w = new UnmodifiableMapWrapperIterator<Object, K, V>(this.c);
            return (Iterator<MapNS.Entry<K, V>>)w;
        }

        public override Object[] toArray()
        {
            int length = c.size();
            Object[] result = new Object[length];
            Iterator<MapNS.Entry<K, V>> it = iterator();
            for (int i = length; --i >= 0; )
            {
                result[i] = it.next();
            }
            return result;
        }

        public override T[] toArray<T>(T[] contents)
        {
            int size = c.size(), index = 0;
            Iterator<MapNS.Entry<K, V>> it = iterator();
            if (size > contents.Length)
            {
                contents = new T[size];
            }
            while (index < size)
            {
                contents[index++] = (T)it.next();
            }
            if (index < contents.Length)
            {
                contents[index] = default(T);
            }
            return contents;
        }
    }
    #region UnmodifiableIterator implementations
    internal class UnmodifiableCollectionWrapperIterator<E> : SimpleListIterator<E>
    {
        protected internal UnmodifiableCollectionWrapperIterator(E c)
            : base(new ArrayList<E>((Collection<E>)c))
        {
        }
        public override void remove()
        {
            throw new java.lang.UnsupportedOperationException();
        }
    }
    internal class UnmodifiableMapWrapperIterator<E, K, V> : SimpleListIterator<E>
    {
        public UnmodifiableMapWrapperIterator(E c)
            : base(new ArrayList<E>((Collection<E>)(Set<MapNS.Entry<K, V>>)c))
        {
        }

        public override E next()
        {
            Object o = (Object)new UnmodifiableMapEntry<K, V>((MapNS.Entry<K, V>)base.next());
            return (E)o;
        }

        public override void remove()
        {
            throw new java.lang.UnsupportedOperationException();
        }
    }
    internal class UnmodifiableMapEntry<K, V> : MapNS.Entry<K, V>
    {
        private readonly MapNS.Entry<K, V> mapEntry;

        protected internal UnmodifiableMapEntry(MapNS.Entry<K, V> entry)
        {
            mapEntry = entry;
        }

        public override bool Equals(Object obj)
        {
            return mapEntry.equals(obj);
        }

        public K getKey()
        {
            return mapEntry.getKey();
        }

        public V getValue()
        {
            return mapEntry.getValue();
        }


        public override int GetHashCode()
        {
            return mapEntry.GetHashCode();
        }

        public V setValue(V obj)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override String ToString()
        {
            return mapEntry.toString();
        }
    }
    #endregion
    #region UnmodifiableList implementations
    [Serializable]
    internal class UnmodifiableRandomAccessList<E> : UnmodifiableList<E>, RandomAccess
    {
        private static readonly long serialVersionUID = -2542308836966382001L;

        internal protected UnmodifiableRandomAccessList(List<E> l)
            : base(l)
        {
        }

        public override List<E> subList(int start, int end)
        {
            return new UnmodifiableRandomAccessList<E>(list.subList(start, end));
        }

        /**
         * Replaces this UnmodifiableRandomAccessList with an UnmodifiableList
         * so that JREs before 1.4 can deserialize this object without any
         * problems. This is necessary since RandomAccess API was introduced
         * only in 1.4.
         * <p>
         * 
         * @return UnmodifiableList
         * 
         * @see UnmodifiableList#readResolve()
         */
        private Object writeReplace()
        {
            return new UnmodifiableList<E>(list);
        }
    }

    [Serializable]
    internal class UnmodifiableList<E> : UnmodifiableCollection<E>, List<E>
    {
        private static readonly long serialVersionUID = -283967356065247728L;

        protected internal List<E> list;

        internal UnmodifiableList(List<E> l)
            : base(l)
        {
            list = l;
        }

        public void add(int location, E o)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public bool addAll(int location, Collection<E> collection)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public override bool Equals(Object o)
        {
            return list.Equals(o);
        }

        public E get(int location)
        {
            return list.get(location);
        }

        public override int GetHashCode()
        {
            return list.GetHashCode();
        }

        public int indexOf(Object o)
        {
            return list.indexOf(o);
        }

        public int lastIndexOf(Object o)
        {
            return list.lastIndexOf(o);
        }

        public ListIterator<E> listIterator()
        {
            return listIterator(0);
        }

        public ListIterator<E> listIterator(int location)
        {
            return new IAC_ListIterator<E>(this, location);
        }

        public E remove(int location)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public E set(int location, E o)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public virtual java.util.List<E> subList(int start, int end)
        {
            return new UnmodifiableList<E>(list.subList(start, end));
        }

        /**
         * Resolves UnmodifiableList instances to UnmodifiableRandomAccessList
         * instances if the underlying list is a Random Access list.
         * <p>
         * This is necessary since UnmodifiableRandomAccessList instances are
         * replaced with UnmodifiableList instances during serialization for
         * compliance with JREs before 1.4.
         * <p>
         * 
         * @return an UnmodifiableList instance if the underlying list
         *         implements RandomAccess interface, or this same object if
         *         not.
         * 
         * @see UnmodifiableRandomAccessList#writeReplace()
         */
        private Object readResolve()
        {
            if (list is RandomAccess)
            {
                return new UnmodifiableRandomAccessList<E>(list);
            }
            return this;
        }
    }

    internal class IAC_ListIterator<E> : ListIterator<E>
    {
        readonly ListIterator<E> iterator;

        public IAC_ListIterator(List<E> l, int location)
        {
            iterator = l.listIterator(location);
        }

        public void add(E o)
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public bool hasNext()
        {
            return iterator.hasNext();
        }

        public bool hasPrevious()
        {
            return iterator.hasPrevious();
        }

        public E next()
        {
            return iterator.next();
        }

        public int nextIndex()
        {
            return iterator.nextIndex();
        }

        public E previous()
        {
            return iterator.previous();
        }

        public int previousIndex()
        {
            return iterator.previousIndex();
        }

        public void remove()
        {
            throw new java.lang.UnsupportedOperationException();
        }

        public void set(E o)
        {
            throw new java.lang.UnsupportedOperationException();
        }
    }

    [Serializable]
    internal sealed class SingletonSet<E> : AbstractSet<E>, java.io.Serializable
    {
        private static readonly long serialVersionUID = 3193687207550431679L;

        internal readonly E element;

        internal SingletonSet(E obj)
        {
            element = obj;
        }

        public override bool contains(Object obj)
        {
            return element == null ? obj == null : element.equals(obj);
        }

        public override int size()
        {
            return 1;
        }

        public override Iterator<E> iterator()
        {
            return new IAC_SingletonSetIterator(this);
        }

        internal sealed class IAC_SingletonSetIterator : Iterator<E>
        {
            private SingletonSet<E> parent;
            public IAC_SingletonSetIterator(SingletonSet<E> parent)
            {
                this.parent = parent;
            }
            bool hasNextJ = true;

            public bool hasNext()
            {
                return hasNextJ;
            }

            public E next()
            {
                if (hasNextJ)
                {
                    hasNextJ = false;
                    return parent.element;
                }
                throw new NoSuchElementException();
            }

            public void remove()
            {
                throw new java.lang.UnsupportedOperationException();
            }

        }
    }
    [Serializable]
    internal sealed class CopiesList<E> : AbstractList<E>, java.io.Serializable
    {
        private static readonly long serialVersionUID = 2739099268398711800L;

        private readonly int n;

        private readonly E element;

        internal CopiesList(int length, E obj)
        {
            if (length < 0)
            {
                throw new java.lang.IllegalArgumentException();
            }
            n = length;
            element = obj;
        }


        public override bool contains(Object obj)
        {
            return element == null ? obj == null : element.equals(obj);
        }


        public override int size()
        {
            return n;
        }


        public override E get(int location)
        {
            if (0 <= location && location < n)
            {
                return element;
            }
            throw new java.lang.IndexOutOfBoundsException();
        }
    }

    #endregion
}