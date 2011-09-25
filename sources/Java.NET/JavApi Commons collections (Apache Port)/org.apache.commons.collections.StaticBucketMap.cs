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
     * A StaticBucketMap is an efficient, thread-safe implementation of
     * <code>java.util.Map</code> that performs well in in a highly
     * thread-contentious environment.  The map supports very efficient
     * {@link #get(Object) get}, {@link #put(Object,Object) put}, 
     * {@link #remove(Object) remove} and {@link #containsKey(Object) containsKey}
     * operations, assuming (approximate) uniform hashing and
     * that the number of entries does not exceed the number of buckets.  If the
     * number of entries exceeds the number of buckets or if the hash codes of the
     * objects are not uniformly distributed, these operations have a worst case
     * scenario that is proportional to the number of elements in the map
     * (<i>O(n)</i>).<p>
     *
     * Each bucket in the hash table has its own monitor, so two threads can 
     * safely operate on the map at the same time, often without incurring any 
     * monitor contention.  This means that you don't have to wrap instances
     * of this class with {@link java.util.Collections#synchronizedMap(Map)};
     * instances are already thread-safe.  Unfortunately, however, this means 
     * that this map implementation behaves in ways you may find disconcerting.  
     * Bulk operations, such as {@link #putAll(Map) putAll} or the
     * {@link Collection#retainAll(Collection) retainAll} operation in collection 
     * views, are <i>not</i> atomic.  If two threads are simultaneously 
     * executing 
     *
     * <pre>
     *   staticBucketMapInstance.putAll(map);
     * </pre>
     *
     * and
     *
     * <pre>
     *   staticBucketMapInstance.entrySet().removeAll(map.entrySet());
     * </pre>
     *
     * then the results are generally random.  Those two statement could cancel
     * each other out, leaving <code>staticBucketMapInstance</code> essentially 
     * unchanged, or they could leave some random subset of <code>map</code> in 
     * <code>staticBucketMapInstance</code>.<p>
     *
     * Also, much like an encyclopedia, the results of {@link #size()} and 
     * {@link #isEmpty()} are out-of-date as soon as they are produced.<p>
     *
     * The iterators returned by the collection views of this class are <i>not</i>
     * fail-fast.  They will <i>never</i> raise a 
     * {@link java.util.ConcurrentModificationException}.  Keys and values 
     * added to the map after the iterator is created do not necessarily appear
     * during iteration.  Similarly, the iterator does not necessarily fail to 
     * return keys and values that were removed after the iterator was created.<p>
     *
     * Finally, unlike {@link java.util.HashMap}-style implementations, this
     * class <i>never</i> rehashes the map.  The number of buckets is fixed 
     * at construction time and never altered.  Performance may degrade if 
     * you do not allocate enough buckets upfront.<p>
     *
     * The {@link #atomic(Runnable)} method is provided to allow atomic iterations
     * and bulk operations; however, overuse of {@link #atomic(Runnable) atomic}
     * will basically result in a map that's slower than an ordinary synchronized
     * {@link java.util.HashMap}.
     *
     * Use this class if you do not require reliable bulk operations and 
     * iterations, or if you can make your own guarantees about how bulk 
     * operations will affect the map.<p>
     *
     * @deprecated Moved to map subpackage. Due to be removed in v4.0.
     * @since Commons Collections 2.1
     * @version $Revision$ $Date$
     * 
     * @author <a href="mailto:bloritsch@apache.org">Berin Loritsch</a>
     * @author <a href="mailto:g-froehlich@gmx.de">Gerhard Froehlich</a>
     * @author <a href="mailto:mas@apache.org">Michael A. Smith</a>
     * @author Paul Jack
     * @author Leo Sutic
     * @author Janek Bogucki
     * @author Kazuya Ujihara
     */
[Obsolete]
    public sealed class StaticBucketMap : java.util.Map<Object, Object>
    {

        private static readonly int DEFAULT_BUCKETS = 255;
        private Node[] m_buckets;
        private LockJ[] m_locks;

        /**
         * Initializes the map with the default number of buckets (255).
         */
        public StaticBucketMap()
            : this(DEFAULT_BUCKETS)
        {
        }

        /**
         * Initializes the map with a specified number of buckets.  The number
         * of buckets is never below 17, and is always an odd number (StaticBucketMap
         * ensures this). The number of buckets is inversely proportional to the
         * chances for thread contention.  The fewer buckets, the more chances for
         * thread contention.  The more buckets the fewer chances for thread
         * contention.
         *
         * @param numBuckets  the number of buckets for this map
         */
        public StaticBucketMap(int numBuckets)
        {
            int size = java.lang.Math.max(17, numBuckets);

            // Ensure that bucketSize is never a power of 2 (to ensure maximal distribution)
            if (size % 2 == 0)
            {
                size--;
            }

            m_buckets = new Node[size];
            m_locks = new LockJ[size];

            for (int i = 0; i < size; i++)
            {
                m_locks[i] = new LockJ();
            }
        }

        /**
         * Determine the exact hash entry for the key.  The hash algorithm
         * is rather simplistic, but it does the job:
         *
         * <pre>
         *   He = |Hk mod n|
         * </pre>
         *
         * <p>
         *   He is the entry's hashCode, Hk is the key's hashCode, and n is
         *   the number of buckets.
         * </p>
         */
        private int getHash(Object key)
        {
            if (key == null) return 0;
            int hash = key.GetHashCode();
            hash += ~(hash << 15);
            hash ^= java.dotnet.lang.Operator.shiftRightUnsignet(hash, 10);
            hash += (hash << 3);
            hash ^= java.dotnet.lang.Operator.shiftRightUnsignet(hash, 6);
            hash += ~(hash << 11);
            hash ^= java.dotnet.lang.Operator.shiftRightUnsignet(hash, 16);
            hash %= m_buckets.Length;
            return (hash < 0) ? hash * -1 : hash;
        }

        /**
         *  Implements {@link Map#keySet()}.
         */
        public java.util.Set<Object> keySet()
        {
            return new KeySet(this);
        }

        /**
         *  Implements {@link Map#size()}.
         */
        public int size()
        {
            int cnt = 0;

            for (int i = 0; i < m_buckets.Length; i++)
            {
                cnt += m_locks[i].size;
            }

            return cnt;
        }

        /**
         *  Implements {@link Map#put(Object, Object)}.
         */
        public Object put(Object key, Object value)
        {
            int hash = getHash(key);

            lock (m_locks[hash])
            {
                Node n = m_buckets[hash];

                if (n == null)
                {
                    n = new Node();
                    n.key = key;
                    n.value = value;
                    m_buckets[hash] = n;
                    m_locks[hash].size++;
                    return null;
                }

                // Set n to the last node in the linked list.  Check each key along the way
                //  If the key is found, then change the value of that node and return
                //  the old value.
                for (Node next = n; next != null; next = next.next)
                {
                    n = next;

                    if (n.key == key || (n.key != null && n.key.equals(key)))
                    {
                        Object returnVal = n.value;
                        n.value = value;
                        return returnVal;
                    }
                }

                // The key was not found in the current list of nodes, add it to the end
                //  in a new node.
                Node newNode = new Node();
                newNode.key = key;
                newNode.value = value;
                n.next = newNode;
                m_locks[hash].size++;
            }

            return null;
        }

        /**
         *  Implements {@link Map#get(Object)}.
         */
        public Object get(Object key)
        {
            int hash = getHash(key);

            lock (m_locks[hash])
            {
                Node n = m_buckets[hash];

                while (n != null)
                {
                    if (n.key == key || (n.key != null && n.key.equals(key)))
                    {
                        return n.value;
                    }

                    n = n.next;
                }
            }

            return null;
        }

        /**
         * Implements {@link Map#containsKey(Object)}.
         */
        public bool containsKey(Object key)
        {
            int hash = getHash(key);

            lock (m_locks[hash])
            {
                Node n = m_buckets[hash];

                while (n != null)
                {
                    if (n.key == key || (n.key != null && n.key.equals(key)))
                    {
                        return true;
                    }

                    n = n.next;
                }
            }

            return false;
        }

        /**
         * Implements {@link Map#containsValue(Object)}.
         */
        public bool containsValue(Object value)
        {
            for (int i = 0; i < m_buckets.Length; i++)
            {
                lock (m_locks[i])
                {
                    Node n = m_buckets[i];

                    while (n != null)
                    {
                        if (n.value == value ||
                            (n.value != null && n.value.equals(value)))
                        {
                            return true;
                        }

                        n = n.next;
                    }
                }
            }

            return false;
        }

        /**
         *  Implements {@link Map#values()}.
         */
        public java.util.Collection<Object> values()
        {
            return new Values(this);
        }

        /**
         *  Implements {@link Map#entrySet()}.
         */
        public java.util.Set<java.util.MapNS.Entry<Object,Object>> entrySet()
        {
            return new EntrySet(this);
        }

        /**
         *  Implements {@link Map#putAll(Map)}.
         */
        public void putAll(java.util.Map<Object, Object> other)
        {
            java.util.Iterator<Object> i = other.keySet().iterator();

            while (i.hasNext())
            {
                Object key = i.next();
                put(key, other.get(key));
            }
        }

        /**
         *  Implements {@link Map#remove(Object)}.
         */
        public Object remove(Object key)
        {
            int hash = getHash(key);

            lock (m_locks[hash])
            {
                Node n = m_buckets[hash];
                Node prev = null;

                while (n != null)
                {
                    if (n.key == key || (n.key != null && n.key.equals(key)))
                    {
                        // Remove this node from the linked list of nodes.
                        if (null == prev)
                        {
                            // This node was the head, set the next node to be the new head.
                            m_buckets[hash] = n.next;
                        }
                        else
                        {
                            // Set the next node of the previous node to be the node after this one.
                            prev.next = n.next;
                        }
                        m_locks[hash].size--;
                        return n.value;
                    }

                    prev = n;
                    n = n.next;
                }
            }

            return null;
        }

        /**
         *  Implements {@link Map#isEmpty()}.
         */
        public bool isEmpty()
        {
            return size() == 0;
        }

        /**
         *  Implements {@link Map#clear()}.
         */
        public void clear()
        {
            for (int i = 0; i < m_buckets.Length; i++)
            {
                LockJ lockj = m_locks[i];
                lock (lockj)
                {
                    m_buckets[i] = null;
                    lockj.size = 0;
                }
            }
        }

        /**
         *  Implements {@link Map#equals(Object)}.
         */
        public override bool Equals(Object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;

            if (!(obj is java.util.Map<Object, Object>)) return false;

            java.util.Map<Object, Object> other = (java.util.Map<Object, Object>)obj;

            return entrySet().equals(other.entrySet());
        }

        /**
         *  Implements {@link Map#hashCode()}.
         */
        public override int GetHashCode()
        {
            int hashCode = 0;

            for (int i = 0; i < m_buckets.Length; i++)
            {
                lock (m_locks[i])
                {
                    Node n = m_buckets[i];

                    while (n != null)
                    {
                        hashCode += n.hashCode();
                        n = n.next;
                    }
                }
            }
            return hashCode;
        }

        /**
         * The Map.Entry for the StaticBucketMap.
         */
        private sealed class Node : java.util.MapNS.Entry<Object, Object>, KeyValue
        {
             internal Object key;
             internal Object value;
             internal Node next;

            public Object getKey()
            {
                return key;
            }

            public Object getValue()
            {
                return value;
            }

            public int hashCode()
            {
                return ((key == null ? 0 : key.GetHashCode()) ^
                         (value == null ? 0 : value.GetHashCode()));
            }

            public bool equals(Object o)
            {
                if (o == null) return false;
                if (o == this) return true;

                if (!(o is java.util.MapNS.Entry<Object, Object>))
                    return false;

                java.util.MapNS.Entry<Object, Object> e2 = (java.util.MapNS.Entry<Object, Object>)o;

                return ((key == null ?
                         e2.getKey() == null : key.equals(e2.getKey())) &&
                        (value == null ?
                         e2.getValue() == null : value.equals(e2.getValue())));
            }

            public Object setValue(Object val)
            {
                Object retVal = value;
                value = val;
                return retVal;
            }
        }

        private sealed class LockJ
        {

            public int size;

        }


        private class EntryIterator : java.util.Iterator<java.util.MapNS.Entry<Object, Object>>
        {

            private readonly StaticBucketMap root;

            public EntryIterator(StaticBucketMap sbm)
            {
                this.root = sbm;
            }

            private java.util.ArrayList<Object> current = new java.util.ArrayList<Object>();
            private int bucket;
            private java.util.MapNS.Entry<Object, Object> last;


            public virtual bool hasNext()
            {
                if (current.size() > 0) return true;
                while (bucket < root.m_buckets.Length)
                {
                    lock (root.m_locks[bucket])
                    {
                        Node n = root.m_buckets[bucket];
                        while (n != null)
                        {
                            current.add(n);
                            n = n.next;
                        }
                        bucket++;
                        if (current.size() > 0) return true;
                    }
                }
                return false;
            }

            protected virtual java.util.MapNS.Entry<Object, Object> nextEntry()
            {
                if (!hasNext()) throw new java.util.NoSuchElementException();
                last = (java.util.MapNS.Entry<Object, Object>)current.remove(current.size() - 1);
                return last;
            }

            public virtual java.util.MapNS.Entry<Object, Object> next()
            {
                return nextEntry();
            }

            public virtual void remove()
            {
                if (last == null) throw new java.lang.IllegalStateException();
                root.remove(last.getKey());
                last = null;
            }

        }

        private class ValueIterator : EntryIterator
        {

            public ValueIterator(StaticBucketMap sbm) : base(sbm) { }
            public override java.util.MapNS.Entry<Object, Object> next()
            {
                return (java.util.MapNS.Entry<Object,Object>) nextEntry().getValue();
            }

        }

        private class KeyIterator : EntryIterator
        {

            public KeyIterator(StaticBucketMap sbm) : base(sbm) { }

            public override java.util.MapNS.Entry<Object, Object> next()
            {
                return (java.util.MapNS.Entry<Object,Object>) nextEntry().getKey();
            }

        }

        private class EntrySet : java.util.AbstractSet<java.util.MapNS.Entry<Object,Object>>
        {

            private readonly StaticBucketMap root;

            public EntrySet(StaticBucketMap sbm)
            {
                this.root = sbm;
            }

            public override int size()
            {
                return root.size();
            }

            public override void clear()
            {
                root.clear();
            }

            public override java.util.Iterator<java.util.MapNS.Entry<Object, Object>> iterator()
            {
                return new EntryIterator(root);
            }

            public override bool contains(Object o)
            {
                java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)o;
                int hash = root.getHash(entry.getKey());
                lock (root.m_locks[hash])
                {
                    for (Node n = root.m_buckets[hash]; n != null; n = n.next)
                    {
                        if (n.equals(entry)) return true;
                    }
                }
                return false;
            }

            public override bool remove(Object obj)
            {
                if (obj is java.util.MapNS.Entry<Object, Object> == false)
                {
                    return false;
                }
                java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)obj;
                int hash = root.getHash(entry.getKey());
                lock (root.m_locks[hash])
                {
                    for (Node n = root.m_buckets[hash]; n != null; n = n.next)
                    {
                        if (n.equals(entry))
                        {
                            root.remove(n.getKey());
                            return true;
                        }
                    }
                }
                return false;
            }

        }


        private class KeySet : java.util.AbstractSet<Object>
        {

            private readonly StaticBucketMap root;

            public KeySet(StaticBucketMap sbm)
            {
                this.root = sbm;
            }

            public override int size()
            {
                return root.size();
            }

            public override void clear()
            {
                root.clear();
            }

            public override java.util.Iterator<Object> iterator()
            {
                return (java.util.Iterator<Object>)new KeyIterator(root);
            }

            public override bool contains(Object o)
            {
                return root.containsKey(o);
            }

            public override bool remove(Object o)
            {
                int hash = root.getHash(o);
                lock (root.m_locks[hash])
                {
                    for (Node n = root.m_buckets[hash]; n != null; n = n.next)
                    {
                        Object k = n.getKey();
                        if ((k == o) || ((k != null) && k.equals(o)))
                        {
                            root.remove(k);
                            return true;
                        }
                    }
                }
                return false;
            }
        }


        private class Values : java.util.AbstractCollection<Object>
        {

            private readonly StaticBucketMap root;

            public Values(StaticBucketMap sbm)
            {
                this.root = sbm;
            }

            public override int size()
            {
                return root.size();
            }

            public override void clear()
            {
                root.clear();
            }

            public override java.util.Iterator<Object> iterator()
            {
                return (java.util.Iterator<Object>)new ValueIterator(root);
            }

        }


        /**
         *  Prevents any operations from occurring on this map while the
         *  given {@link Runnable} executes.  This method can be used, for
         *  instance, to execute a bulk operation atomically: 
         *
         *  <pre>
         *    staticBucketMapInstance.atomic(new Runnable() {
         *        public void run() {
         *            staticBucketMapInstance.putAll(map);
         *        }
         *    });
         *  </pre>
         *
         *  It can also be used if you need a reliable iterator:
         *
         *  <pre>
         *    staticBucketMapInstance.atomic(new Runnable() {
         *        public void run() {
         *            Iterator iterator = staticBucketMapInstance.iterator();
         *            while (iterator.hasNext()) {
         *                foo(iterator.next();
         *            }
         *        }
         *    });
         *  </pre>
         *
         *  <B>Implementation note:</B> This method requires a lot of time
         *  and a ton of stack space.  Essentially a recursive algorithm is used
         *  to enter each bucket's monitor.  If you have twenty thousand buckets
         *  in your map, then the recursive method will be invoked twenty thousand
         *  times.  You have been warned.
         *
         *  @param r  the code to execute atomically
         */
        public void atomic(java.lang.Runnable r)
        {
            if (r == null) throw new java.lang.NullPointerException();
            atomic(r, 0);
        }

        private void atomic(java.lang.Runnable r, int bucket)
        {
            if (bucket >= m_buckets.Length)
            {
                r.run();
                return;
            }
            lock (m_locks[bucket])
            {
                atomic(r, bucket + 1);
            }
        }
    }
}