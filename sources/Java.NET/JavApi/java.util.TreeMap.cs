/*
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */
using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util
{

    /**
     * TreeMap is an implementation of SortedMap. All optional operations (adding
     * and removing) are supported. The values can be any objects. The keys can be
     * any objects which are comparable to each other either using their natural
     * 
     * @param <K>
     *            type of key
     * @param <V>
     *            type of value
     * 
     * @since 1.2
     */
    [Serializable]
    public class TreeMap<K, V> : AbstractMap<K, V>,
            NavigableMap<K, V>, java.lang.Cloneable, java.io.Serializable
    {
        private static readonly long serialVersionUID = 919286545866124006L;

        [NonSerialized]
        int sizeJ;

        [NonSerialized]
        TreeMap<K, V>.Node<K, V> root;

        [NonSerialized]
        Comparator<K> comparatorJ;

        [NonSerialized]
        int modCount;

        [NonSerialized]
        Set<MapNS.Entry<K, V>> entrySetJ;

        [NonSerialized]
        NavigableMap<K, V> descendingMapJ;

        [NonSerialized]
        NavigableSet<K> navigableKeySetJ;

        internal class Node<K, V> : java.lang.Cloneable
        {
            internal static readonly int NODE_SIZE = 64;

            internal TreeMap<K,V>.Node<K, V> prev, next;

            internal TreeMap<K,V>.Node<K, V> parent, left, right;

            internal V[] values;

            internal K[] keys;

            internal int left_idx = 0;

            internal int right_idx = -1;

            internal int size = 0;

            internal bool color;


            public Node()
            {
                keys = new K[NODE_SIZE];
                values = new V[NODE_SIZE];
            }


            internal virtual TreeMap<K,V>.Node<K, V> clone(TreeMap<K,V>.Node<K, V> parent)
            {// throws CloneNotSupportedException {
                TreeMap<K, V>.Node<K, V> clone = (TreeMap<K, V>.Node<K, V>)base.MemberwiseClone();
                clone.keys = new K[NODE_SIZE];
                clone.values = new V[NODE_SIZE];
                java.lang.SystemJ.arraycopy(keys, 0, clone.keys, 0, keys.Length);
                java.lang.SystemJ.arraycopy(values, 0, clone.values, 0, values.Length);
                clone.left_idx = left_idx;
                clone.right_idx = right_idx;
                clone.parent = parent;
                if (left != null)
                {
                    clone.left = left.clone(clone);
                }
                if (right != null)
                {
                    clone.right = right.clone(clone);
                }
                clone.prev = null;
                clone.next = null;
                return clone;
            }
            public Object clone()
            {
                return this.clone(this.parent);
            }

        }

        /**
         * Entry is an internal class which is used to hold the entries of a
         * TreeMap.
         * 
         * also used to record key, value, and position
         */
        class Entry<K,V> : MapEntry<K, V>
        {
            internal TreeMap<K, V>.Entry<K, V> parent, left, right;

            internal TreeMap<K, V>.Node<K, V> node;

            internal int index;

            public void setLocation(TreeMap<K, V>.Node<K, V> node, int index, V value, K key)
            {
                this.node = node;
                this.index = index;
                this.value = value;
                this.key = key;
            }

            bool color;

            Entry(K theKey)
                : base(theKey)
            {
            }

            internal Entry(K theKey, V theValue)
                : base(theKey, theValue)
            {
            }


            TreeMap<K, V>.Entry<K, V> clone(TreeMap<K, V>.Entry<K, V> theParent)
            {
                TreeMap<K, V>.Entry<K, V> clone = (TreeMap<K, V>.Entry<K, V>)base.MemberwiseClone();
                clone.parent = theParent;
                if (left != null)
                {
                    clone.left = left.clone(clone);
                }
                if (right != null)
                {
                    clone.right = right.clone(clone);
                }
                return clone;
            }

            public override V setValue(V obj)
            {
                V result = value;
                value = obj;
                this.node.values[index] = value;
                return result;
            }
        }

        abstract class AbstractSubMapIterator<K, V>
        {
            internal readonly NavigableSubMap<K, V> subMap;

            internal int expectedModCount;

            internal TreeMap<K, V>.Node<K, V> node;

            internal TreeMap<K, V>.Node<K, V> lastNode;

            internal TreeMap<K, V>.Entry<K, V> boundaryPair;

            internal int offset;

            internal int lastOffset;

            bool getToEnd = false;

            internal AbstractSubMapIterator(NavigableSubMap<K, V> map)
            {
                subMap = map;
                expectedModCount = subMap.m.modCount;

                TreeMap<K, V>.Entry<K, V> entry = map.findStartNode();
                if (entry != null)
                {
                    if (map.toEnd && !map.checkUpperBound(entry.key))
                    {
                    }
                    else
                    {
                        node = entry.node;
                        offset = entry.index;
                        boundaryPair = getBoundaryNode();
                    }
                }
            }

            public virtual void remove()
            {
                if (expectedModCount == subMap.m.modCount)
                {
                    if (expectedModCount == subMap.m.modCount)
                    {
                        K key = (node != null) ? node.keys[offset] : default(K);
                        if (lastNode != null)
                        {
                            int idx = lastOffset;
                            if (idx == lastNode.left_idx)
                            {
                                subMap.m.removeLeftmost(lastNode);
                            }
                            else if (idx == lastNode.right_idx)
                            {
                                subMap.m.removeRightmost(lastNode);
                            }
                            else
                            {
                                int lastRight = lastNode.right_idx;
                                key = subMap.m.removeMiddleElement(lastNode, idx);
                                if (key == null && lastRight > lastNode.right_idx)
                                {
                                    // removed from right
                                    offset--;
                                }
                            }
                            if (null != key)
                            {
                                // the node has been cleared
                                TreeMap<K,V>.Entry<K, V> entry = subMap.m.find(key);
                                if (this.subMap.isInRange(key))
                                {
                                    node = entry.node;
                                    offset = entry.index;
                                    boundaryPair = getBoundaryNode();
                                }
                                else
                                {
                                    node = null;
                                }
                            }
                            if (node != null && !this.subMap.isInRange(node.keys[offset]))
                            {
                                node = null;
                            }
                            lastNode = null;
                            expectedModCount++;
                        }
                        else
                        {
                            throw new java.lang.IllegalStateException();
                        }
                    }
                }
                else
                {
                    throw new ConcurrentModificationException();
                }
            }

            void makeNext()
            {
                if (expectedModCount != subMap.m.modCount)
                {
                    throw new ConcurrentModificationException();
                }
                else if (node == null)
                {
                    throw new NoSuchElementException();
                }
                lastNode = node;
                lastOffset = offset;
                if (offset != lastNode.right_idx)
                {
                    offset++;
                }
                else
                {
                    node = node.next;
                    if (node != null)
                    {
                        offset = node.left_idx;
                    }
                }
                if (boundaryPair.node == lastNode
                        && boundaryPair.index == lastOffset)
                {
                    node = null;
                }
            }


            internal TreeMap<K,V>.Entry<K, V> createEntry(TreeMap<K,V>.Node<K, V> node, int index)
            {
                TreeMap<K, V>.Entry<K, V> entry = new TreeMap<K, V>.Entry<K, V>(node.keys[index], node.values[index]);
                entry.node = node;
                entry.index = index;
                return entry;
            }

            internal abstract TreeMap<K, V>.Entry<K, V> getStartNode();

            public abstract bool hasNext();

            internal abstract TreeMap<K, V>.Entry<K, V> getBoundaryNode();
        }

        abstract class AscendingSubMapIterator<K, V> :
                AbstractSubMapIterator<K, V>
        {

            internal AscendingSubMapIterator(NavigableSubMap<K, V> map)
                : base(map)
            {
            }

            internal override TreeMap<K, V>.Entry<K, V> getBoundaryNode()
            {
                TreeMap<K, V>.Entry<K, V> entry = null;
                if (subMap.toEnd)
                {
                    entry = subMap.hiInclusive ? subMap
                            .smallerOrEqualEntry(subMap.hi) : subMap
                            .smallerEntry(subMap.hi);
                }
                else
                {
                    entry = subMap.theBiggestEntry();
                }
                if (entry == null)
                {
                    entry = subMap.findStartNode();
                }
                return entry;
            }


            internal override TreeMap<K, V>.Entry<K, V> getStartNode()
            {
                if (subMap.fromStart)
                {
                    return subMap.loInclusive ? subMap
                            .biggerOrEqualEntry(subMap.lo) : subMap
                            .biggerEntry(subMap.lo);
                }
                return subMap.theSmallestEntry();
            }

            internal TreeMap<K, V>.Entry<K, V> getNext()
            {
                if (expectedModCount != subMap.m.modCount)
                {
                    throw new java.util.ConcurrentModificationException();
                }
                else if (node == null)
                {
                    throw new NoSuchElementException();
                }
                lastNode = node;
                lastOffset = offset;
                if (offset != node.right_idx)
                {
                    offset++;
                }
                else
                {
                    node = node.next;
                    if (node != null)
                    {
                        offset = node.left_idx;
                    }
                }
                if (lastNode != null)
                {
                    boundaryPair = getBoundaryNode();
                    if (boundaryPair != null && boundaryPair.node == lastNode && boundaryPair.index == lastOffset)
                    {
                        node = null;
                    }
                    return createEntry(lastNode, lastOffset);
                }
                return null;
            }


            public override bool hasNext()
            {
                return null != node;
            }

        }

        class AscendingSubMapEntryIterator<K, V> :
                AscendingSubMapIterator<K, V>, Iterator<MapNS.Entry<K, V>>
        {

            internal AscendingSubMapEntryIterator(NavigableSubMap<K, V> map)
                : base(map)
            {
            }

            public MapNS.Entry<K, V> next()
            {
                return getNext();
            }
        }

        class AscendingSubMapKeyIterator<K, V> :
                AscendingSubMapIterator<K, V>, Iterator<K>
        {

            internal AscendingSubMapKeyIterator(NavigableSubMap<K, V> map)
                : base(map)
            {
            }

            public K next()
            {
                return getNext().key;
            }
        }

        private abstract class DescendingSubMapIterator<K, V> :
                AbstractSubMapIterator<K, V>
        {

            internal DescendingSubMapIterator(NavigableSubMap<K, V> map)
                : base(map)
            {
                TreeMap<K, V>.Entry<K, V> entry;
                if (map.fromStart)
                {
                    entry = map.loInclusive ? map.m.findFloorEntry(map.lo) : map.m.findLowerEntry(map.lo);
                }
                else
                {
                    entry = map.m.findBiggestEntry();
                }
                if (entry != null)
                {
                    if (!map.isInRange(entry.key))
                    {
                        node = null;
                        return;
                    }
                    node = entry.node;
                    offset = entry.index;
                }
                else
                {
                    node = null;
                    return;
                }
                boundaryPair = getBoundaryNode();
                if (boundaryPair != null)
                {
                    if (map.m.keyCompare(boundaryPair.key, entry.key) > 0)
                    {
                        node = null;
                    }
                }
                if (map.toEnd && !map.hiInclusive)
                {
                    // the last element may be the same with first one but it is not included
                    if (map.m.keyCompare(map.hi, entry.key) == 0)
                    {
                        node = null;
                    }
                }
            }

            internal override TreeMap<K, V>.Entry<K, V> getStartNode()
            {
                if (subMap.toEnd)
                {
                    return subMap.hiInclusive ? subMap
                            .smallerOrEqualEntry(subMap.hi) : subMap
                            .smallerEntry(subMap.hi);
                }
                return subMap.theBiggestEntry();
            }

            internal override TreeMap<K, V>.Entry<K, V> getBoundaryNode()
            {
                if (subMap.toEnd)
                {
                    return subMap.hiInclusive ? subMap.m.findCeilingEntry(subMap.hi) : subMap.m.findHigherEntry(subMap.hi);
                }
                return subMap.m.findSmallestEntry();
            }

            internal TreeMap<K, V>.Entry<K, V> getNext()
            {
                if (node == null)
                {
                    throw new NoSuchElementException();
                }
                if (expectedModCount != subMap.m.modCount)
                {
                    throw new ConcurrentModificationException();
                }

                lastNode = node;
                lastOffset = offset;
                if (offset != node.left_idx)
                {
                    offset--;
                }
                else
                {
                    node = node.prev;
                    if (node != null)
                    {
                        offset = node.right_idx;
                    }
                }
                boundaryPair = getBoundaryNode();
                if (boundaryPair != null && boundaryPair.node == lastNode && boundaryPair.index == lastOffset)
                {
                    node = null;
                }
                return createEntry(lastNode, lastOffset);
            }


            public override bool hasNext()
            {
                return node != null;
            }

            public override void remove()
            {
                if (expectedModCount == subMap.m.modCount)
                {
                    if (expectedModCount == subMap.m.modCount)
                    {
                        K key = (node != null) ? node.keys[offset] : default(K);
                        if (lastNode != null)
                        {
                            int idx = lastOffset;
                            if (idx == lastNode.left_idx)
                            {
                                subMap.m.removeLeftmost(lastNode);
                            }
                            else if (idx == lastNode.right_idx)
                            {
                                subMap.m.removeRightmost(lastNode);
                            }
                            else
                            {
                                subMap.m.removeMiddleElement(lastNode, idx);
                            }
                            if (null != key)
                            {
                                // the node has been cleared
                                TreeMap<K, V>.Entry<K, V> entry = subMap.m.find(key);
                                node = entry.node;
                                offset = entry.index;
                                boundaryPair = getBoundaryNode();
                            }
                            else
                            {
                                node = null;
                            }
                            lastNode = null;
                            expectedModCount++;
                        }
                        else
                        {
                            throw new java.lang.IllegalStateException();
                        }
                    }
                }
                else
                {
                    throw new ConcurrentModificationException();
                }
            }
        }

        class DescendingSubMapEntryIterator<K, V> :
                DescendingSubMapIterator<K, V>, Iterator<MapNS.Entry<K, V>>
        {

            internal DescendingSubMapEntryIterator(NavigableSubMap<K, V> map)
                : base(map)
            {
            }

            public MapNS.Entry<K, V> next()
            {
                return getNext();
            }
        }

        class DescendingSubMapKeyIterator<K, V> :
                DescendingSubMapIterator<K, V>, Iterator<K>
        {

            internal DescendingSubMapKeyIterator(NavigableSubMap<K, V> map)
                : base(map)
            {
            }

            public K next()
            {
                return getNext().key;
            }
        }
        [Serializable]
        sealed class SubMap<K, V> : AbstractMap<K, V>,
                SortedMap<K, V>, java.io.Serializable
        {
            private static readonly long serialVersionUID = -6520786458950516097L;

            internal TreeMap<K, V> backingMap;

            internal bool hasStart, hasEnd;

            internal K startKey, endKey;

            [NonSerialized]
            Set<MapNS.Entry<K, V>> entrySetJ = null;

            [NonSerialized]
            int firstKeyModCount = -1;

            [NonSerialized]
            int lastKeyModCount = -1;

            [NonSerialized]
            internal TreeMap<K, V>.Node<K, V> firstKeyNode;

            [NonSerialized]
            internal int firstKeyIndex;

            [NonSerialized]
            internal TreeMap<K, V>.Node<K, V> lastKeyNode;

            [NonSerialized]
            internal int lastKeyIndex;

            internal SubMap(K start, TreeMap<K, V> map)
            {
                backingMap = map;
                hasStart = true;
                startKey = start;
            }

            internal SubMap(K start, TreeMap<K, V> map, K end)
            {
                backingMap = map;
                hasStart = hasEnd = true;
                startKey = start;
                endKey = end;
            }

            internal SubMap(K start, bool hasStart, TreeMap<K, V> map, K end, bool hasEnd)
            {
                backingMap = map;
                this.hasStart = hasStart;
                this.hasEnd = hasEnd;
                startKey = start;
                endKey = end;
            }

            internal SubMap(TreeMap<K, V> map, K end)
            {
                backingMap = map;
                hasEnd = true;
                endKey = end;
            }

            private void checkRange(K key)
            {
                Comparator<K> cmp = backingMap.comparatorJ;
                if (cmp == null)
                {
                    java.lang.Comparable<K> obj = toComparable(key);
                    if (hasStart && obj.compareTo(startKey) < 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                    if (hasEnd && obj.compareTo(endKey) >= 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                }
                else
                {
                    if (hasStart
                            && backingMap.comparator().compare(key, startKey) < 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                    if (hasEnd && backingMap.comparator().compare(key, endKey) >= 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                }
            }

            internal bool isInRange(K key)
            {
                Comparator<K> cmp = backingMap.comparatorJ;
                if (cmp == null)
                {
                    java.lang.Comparable<K> obj = toComparable(key);
                    if (hasStart && obj.compareTo(startKey) < 0)
                    {
                        return false;
                    }
                    if (hasEnd && obj.compareTo(endKey) >= 0)
                    {
                        return false;
                    }
                }
                else
                {
                    if (hasStart && cmp.compare(key, startKey) < 0)
                    {
                        return false;
                    }
                    if (hasEnd && cmp.compare(key, endKey) >= 0)
                    {
                        return false;
                    }
                }
                return true;
            }

            private bool checkUpperBound(K key)
            {
                if (hasEnd)
                {
                    Comparator<K> cmp = backingMap.comparatorJ;
                    if (cmp == null)
                    {
                        return (toComparable(key).compareTo(endKey) < 0);
                    }
                    return (cmp.compare(key, endKey) < 0);
                }
                return true;
            }

            private bool checkLowerBound(K key)
            {
                if (hasStart && startKey != null)
                {
                    Comparator<K> cmp = backingMap.comparatorJ;
                    if (cmp == null)
                    {
                        return (toComparable(key).compareTo(startKey) >= 0);
                    }
                    return (cmp.compare(key, startKey) >= 0);
                }
                return true;
            }

            public Comparator<K> comparator()
            {
                return backingMap.comparator();
            }



            public override bool containsKey(Object key)
            {
                if (isInRange((K)key))
                {
                    return backingMap.containsKey(key);
                }
                return false;
            }


            public override void clear()
            {
                keySet().clear();
            }


            public override bool containsValue(Object value)
            {
                Iterator<V> it = values().iterator();
                if (value != null)
                {
                    while (it.hasNext())
                    {
                        if (value.equals(it.next()))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    while (it.hasNext())
                    {
                        if (it.next() == null)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }


            public override Set<MapNS.Entry<K, V>> entrySet()
            {
                if (entrySetJ == null)
                {
                    entrySetJ = new SubMapEntrySet<K, V>(this);
                }
                return entrySetJ;
            }

            internal void setFirstKey()
            {
                if (firstKeyModCount == backingMap.modCount)
                {
                    return;
                }
                java.lang.Comparable<K> obj = backingMap.comparatorJ == null ? toComparable((K)startKey)
                        : null;
                K key = (K)startKey;
                TreeMap<K, V>.Node<K, V> node = backingMap.root;
                TreeMap<K, V>.Node<K, V> foundNode = null;
                int foundIndex = -1;
                bool topLoop = false;
                while (node != null)
                {
                    topLoop = false;
                    K[] keys = node.keys;
                    int left_idx = node.left_idx;
                    int result = backingMap.cmp(obj, key, keys[left_idx]);
                    if (result < 0)
                    {
                        foundNode = node;
                        foundIndex = node.left_idx;
                        node = node.left;
                    }
                    else if (result == 0)
                    {
                        foundNode = node;
                        foundIndex = node.left_idx;
                        break;
                    }
                    else
                    {
                        int right_idx = node.right_idx;
                        if (left_idx != right_idx)
                        {
                            result = backingMap.cmp(obj, key, keys[right_idx]);
                        }
                        if (result > 0)
                        {
                            node = node.right;
                        }
                        else if (result == 0)
                        {
                            foundNode = node;
                            foundIndex = node.right_idx;
                            break;
                        }
                        else
                        { /* search in node */
                            foundNode = node;
                            foundIndex = node.right_idx;
                            int low = left_idx + 1, mid = 0, high = right_idx - 1;
                            while (low <= high && !topLoop)
                            {
                                mid = java.dotnet.lang.Operator.shiftRightUnsignet((low + high), 1);
                                result = backingMap.cmp(obj, key, keys[mid]);
                                if (result > 0)
                                {
                                    low = mid + 1;
                                }
                                else if (result == 0)
                                {
                                    foundNode = node;
                                    foundIndex = mid;
                                    topLoop = true;
                                }
                                else
                                {
                                    foundNode = node;
                                    foundIndex = mid;
                                    high = mid - 1;
                                }
                            }
                            topLoop = true;
                        }
                    }
                }
                // note, the original subMap is strange as the endKey is always
                // excluded, to improve the performance here, we retain the original
                // subMap, and keep the bound when firstkey = lastkey
                bool isBounded = true;
                if (hasEnd && foundNode != null)
                {
                    Comparator<K> cmp = backingMap.comparatorJ;
                    if (cmp == null)
                    {
                        isBounded = (toComparable(foundNode.keys[foundIndex]).compareTo(endKey) <= 0);
                    }
                    else
                    {
                        isBounded = (cmp.compare(foundNode.keys[foundIndex], endKey) <= 0);
                    }
                }
                if (foundNode != null
                        && !isBounded)
                {
                    foundNode = null;
                }
                firstKeyNode = foundNode;
                firstKeyIndex = foundIndex;
                firstKeyModCount = backingMap.modCount;
            }

            public K firstKey()
            {
                if (backingMap.sizeJ > 0 && !(startKey.equals(endKey)))
                {
                    if (!hasStart)
                    {
                        TreeMap<K,V>.Node<K, V> node = minimum(backingMap.root);
                        if (node != null
                                && checkUpperBound(node.keys[node.left_idx]))
                        {
                            return node.keys[node.left_idx];
                        }
                    }
                    else
                    {
                        setFirstKey();
                        if (firstKeyNode != null)
                        {
                            return firstKeyNode.keys[firstKeyIndex];
                        }
                    }
                }
                throw new NoSuchElementException();
            }



            public override V get(Object key)
            {
                if (isInRange((K)key))
                {
                    return backingMap.get(key);
                }
                return default(V);
            }

            public SortedMap<K, V> headMap(K endKey)
            {
                Comparator<K> cmp = backingMap.comparatorJ;
                if (cmp == null)
                {
                    java.lang.Comparable<K> obj = toComparable(endKey);
                    if (hasStart && obj.compareTo(startKey) < 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                    if (hasEnd && obj.compareTo(this.endKey) > 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                }
                else
                {
                    if (hasStart
                            && backingMap.comparator().compare(endKey, startKey) < 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                    if (hasEnd && backingMap.comparator().compare(endKey, this.endKey) >= 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                }
                if (hasStart)
                {
                    return new SubMap<K, V>(startKey, backingMap, endKey);
                }
                return new SubMap<K, V>(backingMap, endKey);
            }

            public override bool isEmpty()
            {
                Iterator<K> it = this.keySet().iterator();
                if (it.hasNext())
                {
                    return false;
                }
                return true;
            }

            public override Set<K> keySet()
            {
                if (keySetJ == null)
                {
                    keySetJ = new SubMapKeySet<K, V>(this);
                }
                return keySetJ;
            }

            internal void setLastKey()
            {
                if (lastKeyModCount == backingMap.modCount)
                {
                    return;
                }
                java.lang.Comparable<K> obj = backingMap.comparatorJ == null ? toComparable((K)endKey)
                        : null;
                K key = (K)endKey;
                TreeMap<K, V>.Node<K, V> node = backingMap.root;
                TreeMap<K, V>.Node<K, V> foundNode = null;
                int foundIndex = -1;
                bool topLoop = false;
                while (node != null)
                {
                    topLoop = false;
                    K[] keys = node.keys;
                    int left_idx = node.left_idx;
                    // to be compatible with RI on null-key comparator
                    int result = obj != null ? obj.compareTo(keys[left_idx]) : -backingMap.comparatorJ.compare(
                            keys[left_idx], key);
                    //int result =  - backingMap.cmp(object, keys[left_idx] , key);
                    if (result < 0)
                    {
                        node = node.left;
                    }
                    else
                    {
                        int right_idx = node.right_idx;
                        if (left_idx != right_idx)
                        {
                            result = backingMap.cmp(obj, key, keys[right_idx]);
                        }
                        if (result > 0)
                        {
                            foundNode = node;
                            foundIndex = right_idx;
                            node = node.right;
                        }
                        else if (result == 0)
                        {
                            if (node.left_idx == node.right_idx)
                            {
                                foundNode = node.prev;
                                if (foundNode != null)
                                {
                                    foundIndex = foundNode.right_idx - 1;
                                }
                            }
                            else
                            {
                                foundNode = node;
                                foundIndex = right_idx;
                            }
                            break;
                        }
                        else
                        { /* search in node */
                            foundNode = node;
                            foundIndex = left_idx;
                            int low = left_idx + 1, mid = 0, high = right_idx - 1;
                            while (low <= high && !topLoop)
                            {
                                mid = java.dotnet.lang.Operator.shiftRightUnsignet((low + high), 1);
                                result = backingMap.cmp(obj, key, keys[mid]);
                                if (result > 0)
                                {
                                    foundNode = node;
                                    foundIndex = mid;
                                    low = mid + 1;
                                }
                                else if (result == 0)
                                {
                                    foundNode = node;
                                    foundIndex = mid;
                                    topLoop = true;
                                }
                                else
                                {
                                    high = mid - 1;
                                }
                            }
                            topLoop = true;
                        }
                    }
                }
                if (foundNode != null
                        && !checkLowerBound(foundNode.keys[foundIndex]))
                {
                    foundNode = null;
                }
                lastKeyNode = foundNode;
                lastKeyIndex = foundIndex;
                lastKeyModCount = backingMap.modCount;
            }

            public K lastKey()
            {
                if (backingMap.sizeJ > 0 && !(startKey.equals(endKey)))
                {
                    if (!hasEnd)
                    {
                        TreeMap<K, V>.Node<K, V> node = maximum(backingMap.root);
                        if (node != null
                                && checkLowerBound(node.keys[node.right_idx]))
                        {
                            return node.keys[node.right_idx];
                        }
                    }
                    else
                    {
                        setLastKey();
                        if (lastKeyNode != null)
                        {
                            java.lang.Comparable<K> obj = backingMap.comparatorJ == null ? toComparable((K)endKey)
                                    : null;
                            if (backingMap.cmp(obj, endKey, lastKeyNode.keys[lastKeyIndex]) != 0)
                            {
                                return lastKeyNode.keys[lastKeyIndex];
                            }
                            else
                            {
                                // according to subMap, it excludes the last element 
                                if (lastKeyIndex != lastKeyNode.left_idx)
                                {
                                    obj = backingMap.comparatorJ == null ? toComparable((K)startKey)
                                            : null;
                                    // check if the element is smaller than the startkey, there's no lastkey
                                    if (backingMap.cmp(obj, startKey, lastKeyNode.keys[lastKeyIndex - 1]) <= 0)
                                    {
                                        return lastKeyNode.keys[lastKeyIndex - 1];
                                    }
                                }
                                else
                                {
                                    TreeMap<K,V>.Node<K, V> last = lastKeyNode.prev;
                                    if (last != null)
                                    {
                                        return last.keys[last.right_idx];
                                    }
                                }
                            }
                        }
                    }
                }
                throw new NoSuchElementException();
            }


            public override V put(K key, V value)
            {
                if (isInRange(key))
                {
                    return backingMap.put(key, value);
                }
                throw new java.lang.IllegalArgumentException();
            }



            public override V remove(Object key)
            {
                if (isInRange((K)key))
                {
                    return backingMap.remove(key);
                }
                return default(V);
            }

            public SortedMap<K, V> subMap(K startKey, K endKey)
            {
                checkRange(startKey);
                Comparator<K> cmp = backingMap.comparatorJ;
                if (cmp == null)
                {
                    java.lang.Comparable<K> obj = toComparable(endKey);
                    if (hasStart && obj.compareTo(startKey) < 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                    if (hasEnd && obj.compareTo(endKey) > 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                }
                else
                {
                    if (hasStart
                            && backingMap.comparator().compare(endKey, this.startKey) < 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                    if (hasEnd && backingMap.comparator().compare(endKey, this.endKey) > 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                }
                Comparator<K> c = backingMap.comparator();
                if (c == null)
                {
                    if (toComparable(startKey).compareTo(endKey) <= 0)
                    {
                        return new SubMap<K, V>(startKey, backingMap, endKey);
                    }
                }
                else
                {
                    if (c.compare(startKey, endKey) <= 0)
                    {
                        return new SubMap<K, V>(startKey, backingMap, endKey);
                    }
                }
                throw new java.lang.IllegalArgumentException();
            }

            public SortedMap<K, V> tailMap(K startKey)
            {
                checkRange(startKey);
                if (hasEnd)
                {
                    return new SubMap<K, V>(startKey, backingMap, endKey);
                }
                return new SubMap<K, V>(startKey, backingMap);
            }


            public override Collection<V> values()
            {
                if (valuesCollection == null)
                {
                    valuesCollection = new SubMapValuesCollection<K, V>(this);
                }
                return valuesCollection;
            }

            public override int size()
            {
                TreeMap<K, V>.Node<K, V> from, to;
                int fromIndex, toIndex;
                if (hasStart)
                {
                    setFirstKey();
                    from = firstKeyNode;
                    fromIndex = firstKeyIndex;
                }
                else
                {
                    from = minimum(backingMap.root);
                    fromIndex = from == null ? 0 : from.left_idx;
                }
                if (from == null)
                {
                    return 0;
                }
                if (hasEnd)
                {
                    setLastKey();
                    to = lastKeyNode;
                    toIndex = lastKeyIndex;
                    java.lang.Comparable<K> obj = backingMap.comparatorJ == null ? toComparable((K)endKey)
                            : null;
                    if (to == null)
                    {
                        return 0;
                    }
                    else if (backingMap.cmp(obj, endKey, to.keys[toIndex]) != 0)
                    {
                        if (toIndex != to.keys.Length)
                        {
                            toIndex++;
                        }
                        else
                        {
                            to = to.next;
                            toIndex = to == null ? 0 : to.left_idx;
                        }
                    }
                }
                else
                {
                    to = maximum(backingMap.root);
                    toIndex = to == null ? 0 : to.right_idx;
                }
                if (to == null)
                {
                    return 0;
                }
                // the last element of submap if exist should be ignored
                if (from == to)
                {
                    return toIndex - fromIndex + (hasEnd ? 0 : 1);
                }
                int sum = 0;
                while (from != to)
                {
                    sum += (from.right_idx - fromIndex + 1);
                    from = from.next;
                    fromIndex = from.left_idx;
                }
                return sum + toIndex - fromIndex + (hasEnd ? 0 : 1);
            }

            private void readObject(java.io.ObjectInputStream stream)
            {//throws IOException,
                //ClassNotFoundException {
                stream.defaultReadObject();
                firstKeyModCount = -1;
                lastKeyModCount = -1;
            }
        }

        class SubMapValuesCollection<K, V> : AbstractCollection<V>
        {
            SubMap<K, V> subMap;

            public SubMapValuesCollection(SubMap<K, V> subMap)
            {
                this.subMap = subMap;
            }


            public override bool isEmpty()
            {
                return subMap.isEmpty();
            }


            public override Iterator<V> iterator()
            {
                TreeMap<K,V>.Node<K, V> from;
                int fromIndex;
                if (subMap.hasStart)
                {
                    subMap.setFirstKey();
                    from = subMap.firstKeyNode;
                    fromIndex = subMap.firstKeyIndex;
                }
                else
                {
                    from = minimum(subMap.backingMap.root);
                    fromIndex = from != null ? from.left_idx : 0;
                }
                if (!subMap.hasEnd)
                {
                    return new UnboundedValueIterator<K, V>(subMap.backingMap,
                            from, from == null ? 0 : fromIndex);
                }
                subMap.setLastKey();
                TreeMap<K,V>.Node<K, V> to = subMap.lastKeyNode;
                int toIndex = subMap.lastKeyIndex
                + (subMap.lastKeyNode != null
                        && (!subMap.lastKeyNode.keys[subMap.lastKeyIndex].equals(subMap.endKey)) ? 1
                        : 0);
                if (to != null
                        && toIndex > to.right_idx)
                {
                    to = to.next;
                    toIndex = to != null ? to.left_idx : 0;
                    if (to == null)
                    {
                        // has endkey but it does not exist, thus return UnboundedValueIterator
                        return new UnboundedValueIterator<K, V>(subMap.backingMap,
                                from, from == null ? 0 : fromIndex);
                    }
                }
                return new BoundedValueIterator<K, V>(from, from == null ? 0
                        : fromIndex, subMap.backingMap, to,
                        to == null ? 0 : toIndex);
            }


            public override int size()
            {
                return subMap.size();
            }
        }

        class BoundedMapIterator<K, V> : AbstractMapIterator<K, V>
        {

            internal TreeMap<K,V>.Node<K, V> finalNode;

            internal int finalOffset;

            internal BoundedMapIterator(TreeMap<K, V>.Node<K, V> startNode, int startOffset,
                    TreeMap<K, V> map, TreeMap<K,V>.Node<K, V> finalNode, int finalOffset) :
                base(map, startNode, startOffset)
            {
                if (startNode == null && finalNode == null)
                {
                    // no elements
                    node = null;
                    return;
                }
                if (finalNode != null)
                {
                    this.finalNode = finalNode;
                    this.finalOffset = finalOffset;
                }
                else
                {
                    TreeMap<K,V>.Entry<K,V> entry = map.findBiggestEntry();
                    if (entry != null)
                    {
                        this.finalNode = entry.node;
                        this.finalOffset = entry.index;
                    }
                    else
                    {
                        node = null;
                        return;
                    }
                }
                if (startNode != null)
                {
                    if (node == this.finalNode && offset >= this.finalOffset)
                    {
                        node = null;
                    }
                    else if (this.finalOffset < this.finalNode.right_idx)
                    {
                        java.lang.Comparable<K> obj = backingMap.comparatorJ == null ? toComparable((K)startNode.keys[startOffset])
                                : null;
                        if (this.backingMap.cmp(obj, node.keys[offset],
                                this.finalNode.keys[this.finalOffset]) > 0)
                        {
                            node = null;
                        }
                    }
                }
            }

            internal BoundedMapIterator(TreeMap<K, V>.Node<K, V> startNode, TreeMap<K, V> map,
                    TreeMap<K,V>.Node<K, V> finalNode, int finalOffset) :
                this(startNode, startNode.left_idx, map, finalNode, finalOffset)
            {
            }

            internal BoundedMapIterator(TreeMap<K, V>.Node<K, V> startNode, int startOffset,
                    TreeMap<K, V> map, TreeMap<K,V>.Node<K, V> finalNode) :
                this(startNode, startOffset, map, finalNode, finalNode.right_idx)
            {
            }

            internal void makeBoundedNext()
            {
                if (node != null)
                {
                    bool endOfIterator = lastNode == finalNode && lastOffset == finalOffset;
                    if (endOfIterator)
                    {
                        node = null;
                    }
                    else
                    {
                        makeNext();
                    }
                }
            }

            public bool hasNext()
            {
                if (finalNode == node && finalOffset == offset)
                {
                    node = null;
                }
                return node != null;
            }
        }

        class BoundedEntryIterator<K, V> : BoundedMapIterator<K, V>
                , Iterator<MapNS.Entry<K, V>>
        {

            public BoundedEntryIterator(TreeMap<K,V>.Node<K,V> startNode, int startOffset,
                    TreeMap<K, V> map, TreeMap<K,V>.Node<K, V> finalNode, int finalOffset) :
                base(startNode, startOffset, map, finalNode, finalOffset)
            {
            }

            public MapNS.Entry<K, V> next()
            {
                if (!hasNext())
                {
                    throw new NoSuchElementException();
                }
                makeBoundedNext();
                int idx = lastOffset;
                return new MapEntry<K, V>(lastNode.keys[idx], lastNode.values[idx]);
            }
        }

        class BoundedKeyIterator<K, V> : BoundedMapIterator<K, V>
                , Iterator<K>
        {

            public BoundedKeyIterator(TreeMap<K,V>.Node<K,V> startNode, int startOffset,
                    TreeMap<K, V> map, TreeMap<K,V>.Node<K, V> finalNode, int finalOffset)
                : base(startNode, startOffset, map, finalNode, finalOffset)
            {
            }

            public K next()
            {
                if (!hasNext())
                {
                    throw new NoSuchElementException();
                }
                makeBoundedNext();
                return lastNode.keys[lastOffset];
            }
        }

        class BoundedValueIterator<K, V> : BoundedMapIterator<K, V>
                , Iterator<V>
        {

            public BoundedValueIterator(TreeMap<K,V>.Node<K,V> startNode, int startOffset,
                    TreeMap<K, V> map, TreeMap<K,V>.Node<K, V> finalNode, int finalOffset) :
                base(startNode, startOffset, map, finalNode, finalOffset) { }

            public V next()
            {
                if (!hasNext())
                {
                    throw new NoSuchElementException();
                }
                makeBoundedNext();
                return lastNode.values[lastOffset];
            }
        }

        class SubMapKeySet<K, V> : AbstractSet<K>, Set<K>
        {
            SubMap<K, V> subMap;

            internal SubMapKeySet(SubMap<K, V> map)
            {
                subMap = map;
            }


            public override bool contains(Object obj)
            {
                return subMap.containsKey(obj);
            }


            public override bool isEmpty()
            {
                return subMap.isEmpty();
            }


            public override int size()
            {
                return subMap.size();
            }


            public override bool remove(Object obj)
            {
                return subMap.remove(obj) != null;
            }

            public override Iterator<K> iterator()
            {
                TreeMap<K, V>.Node<K, V> from;
                int fromIndex;
                if (subMap.hasStart)
                {
                    subMap.setFirstKey();
                    from = subMap.firstKeyNode;
                    fromIndex = subMap.firstKeyIndex;
                }
                else
                {
                    from = minimum(subMap.backingMap.root);
                    fromIndex = from != null ? from.left_idx : 0;
                }
                if (from == null)
                {
                    return new BoundedKeyIterator<K, V>(null, 0, subMap.backingMap, null, 0);
                }
                if (!subMap.hasEnd)
                {
                    return new UnboundedKeyIterator<K, V>(subMap.backingMap,
                            from, from == null ? 0 : from.right_idx - fromIndex);
                }
                subMap.setLastKey();
                TreeMap<K, V>.Node<K, V> to = subMap.lastKeyNode;
                java.lang.Comparable<K> obj = subMap.backingMap.comparatorJ == null ? toComparable((K)subMap.endKey)
                        : null;
                int toIndex = subMap.lastKeyIndex
                        + (subMap.lastKeyNode != null
                                && (!subMap.lastKeyNode.keys[subMap.lastKeyIndex].equals(subMap.endKey)) ? 1
                                : 0);
                if (subMap.lastKeyNode != null && toIndex > subMap.lastKeyNode.right_idx)
                {
                    to = to.next;
                    toIndex = to != null ? to.left_idx : 0;
                }
                // no intial nor end key, return a unbounded iterator
                if (to == null)
                {
                    return new UnboundedKeyIterator<K, V>(subMap.backingMap, from, fromIndex);
                }
                else
                    return new BoundedKeyIterator<K, V>(from, from == null ? 0
                            : fromIndex, subMap.backingMap, to,
                            to == null ? 0 : toIndex);
            }
        }


        /*
         * Entry set of sub-maps, must override methods which check the range. add
         * or addAll operations are disabled by default.
         */
        class SubMapEntrySet<K, V> : AbstractSet<MapNS.Entry<K, V>>
               , Set<MapNS.Entry<K, V>>
        {
            SubMap<K, V> subMap;

            internal SubMapEntrySet(SubMap<K, V> map)
            {
                subMap = map;
            }


            public override bool isEmpty()
            {
                return subMap.isEmpty();
            }

            public override Iterator<MapNS.Entry<K, V>> iterator()
            {
                TreeMap<K,V>.Node<K, V> from;
                int fromIndex;
                if (subMap.hasStart)
                {
                    subMap.setFirstKey();
                    from = subMap.firstKeyNode;
                    fromIndex = subMap.firstKeyIndex;
                }
                else
                {
                    from = minimum(subMap.backingMap.root);
                    fromIndex = from != null ? from.left_idx : 0;
                }
                if (from == null)
                {
                    return new BoundedEntryIterator<K, V>(null, 0, subMap.backingMap, null, 0);
                }
                if (!subMap.hasEnd)
                {
                    return new UnboundedEntryIterator<K, V>(subMap.backingMap,
                            from, from == null ? 0 : from.right_idx - fromIndex);
                }
                subMap.setLastKey();
                TreeMap<K,V>.Node<K, V> to = subMap.lastKeyNode;
                java.lang.Comparable<K> obj = subMap.backingMap.comparatorJ == null ? toComparable((K)subMap.endKey)
                        : null;
                int toIndex = subMap.lastKeyIndex
                        + (subMap.lastKeyNode != null
                                && (!subMap.lastKeyNode.keys[subMap.lastKeyIndex].equals(subMap.endKey)) ? 1
                                : 0);
                if (subMap.lastKeyNode != null && toIndex > subMap.lastKeyNode.right_idx)
                {
                    to = to.next;
                    toIndex = to != null ? to.left_idx : 0;
                }
                if (to == null)
                {
                    return new UnboundedEntryIterator<K, V>(subMap.backingMap, from, fromIndex);
                }
                else
                    return new BoundedEntryIterator<K, V>(from, from == null ? 0
                            : fromIndex, subMap.backingMap, to,
                            to == null ? 0 : toIndex);
            }


            public override int size()
            {
                return subMap.size();
            }



            public override bool contains(Object obj)
            {
                if (obj is MapNS.Entry<K, V>)
                {
                    MapNS.Entry<K, V> entry = (MapNS.Entry<K, V>)obj;
                    K key = entry.getKey();
                    if (subMap.isInRange(key))
                    {
                        V v1 = subMap.get(key), v2 = entry.getValue();
                        return v1 == null ? (v2 == null && subMap.containsKey(key)) : v1.equals(v2);
                    }
                }
                return false;
            }



            public override bool remove(Object obj)
            {
                if (contains(obj))
                {
                    MapNS.Entry<K, V> entry = (MapNS.Entry<K, V>)obj;
                    K key = entry.getKey();
                    subMap.remove(key);
                    return true;
                }
                return false;
            }
        }

        class AscendingSubMapEntrySet<K, V> :
                AbstractSet<MapNS.Entry<K, V>>, NavigableSet<MapNS.Entry<K, V>>
        {

            bool hasStart, hasEnd, startInclusive, endInclusive;

            java.util.MapNS.Entry<K, V> startEntry, lastentry;

            NavigableSubMap<K, V> map;

            internal AscendingSubMapEntrySet(NavigableSubMap<K, V> map)
            {
                this.map = map;
            }

            internal AscendingSubMapEntrySet(NavigableSubMap<K, V> map,
                    java.util.MapNS.Entry<K, V> startEntry, bool startInclusive,
                    java.util.MapNS.Entry<K, V> endEntry, bool endInclusive)
            {
                if (startEntry != null)
                {
                    hasStart = true;
                    this.startEntry = startEntry;
                    this.startInclusive = startInclusive;
                }
                if (endEntry != null)
                {
                    hasEnd = true;
                    this.lastentry = endEntry;
                    this.endInclusive = endInclusive;
                }
                if (startEntry != null && endEntry != null)
                {
                    this.map = (NavigableSubMap<K, V>)map.subMap(startEntry
                            .getKey(), startInclusive, endEntry.getKey(),
                            endInclusive);
                    return;
                }
                if (startEntry != null)
                {
                    this.map = (NavigableSubMap<K, V>)map.tailMap(startEntry
                            .getKey(), startInclusive);
                    return;
                }
                if (endEntry != null)
                {
                    this.map = (NavigableSubMap<K, V>)map.headMap(endEntry
                            .getKey(), endInclusive);
                    return;
                }
                this.map = map;
            }


            public override Iterator<MapNS.Entry<K, V>> iterator()
            {
                return new AscendingSubMapEntryIterator<K, V>(map);
            }


            public override int size()
            {
                int size = 0;
                AscendingSubMapEntryIterator<K, V> it = new AscendingSubMapEntryIterator<K, V>(map);
                while (it.hasNext())
                {
                    it.next();
                    size++;
                }
                return size;
            }

            public java.util.MapNS.Entry<K, V> ceiling(java.util.MapNS.Entry<K, V> e)
            {
                MapNS.Entry<K, V> entry = map.ceilingEntry(e.getKey());
                if (entry != null && map.isInRange(entry.getKey()))
                {
                    return entry;
                }
                else
                {
                    return null;
                }
            }

            public Iterator<java.util.MapNS.Entry<K, V>> descendingIterator()
            {
                return new DescendingSubMapEntrySet<K, V>(map.descendingSubMap()).iterator();
            }

            public NavigableSet<java.util.MapNS.Entry<K, V>> descendingSet()
            {
                return new DescendingSubMapEntrySet<K, V>(map.descendingSubMap());
            }

            public java.util.MapNS.Entry<K, V> floor(java.util.MapNS.Entry<K, V> e)
            {
                MapNS.Entry<K, V> entry = map.floorEntry(e.getKey());
                if (entry != null && map.isInRange(entry.getKey()))
                {
                    return entry;
                }
                else
                {
                    return null;
                }
            }

            public NavigableSet<java.util.MapNS.Entry<K, V>> headSet(
                    java.util.MapNS.Entry<K, V> end, bool endInclusive)
            {
                bool isInRange = true;
                int result = 0;
                K endKey = end.getKey();
                if (map.toEnd)
                {
                    result = (null != comparator()) ? keyComparator().compare(endKey,
                            map.hi) : toComparable(endKey).compareTo(map.hi);
                    isInRange = (!map.hiInclusive && endInclusive) ? result < 0
                            : result <= 0;
                }
                if (map.fromStart)
                {
                    result = (null != comparator()) ? keyComparator().compare(endKey,
                            map.lo) : toComparable(endKey).compareTo(map.lo);
                    isInRange = isInRange
                            && ((!map.loInclusive && endInclusive) ? result > 0
                                    : result >= 0);
                }
                if (isInRange)
                {
                    return new AscendingSubMapEntrySet<K, V>(map, null, false,
                            end, endInclusive);
                }
                throw new java.lang.IllegalArgumentException();
            }

            public java.util.MapNS.Entry<K, V> higher(java.util.MapNS.Entry<K, V> e)
            {
                Comparator<K> cmp = map.m.comparatorJ;
                if (cmp == null)
                {
                    java.lang.Comparable<K> obj = toComparable(e.getKey());
                    if (hasStart && obj.compareTo(startEntry.getKey()) < 0)
                    {
                        return map.higherEntry(startEntry.getKey());
                    }
                    if (hasEnd && obj.compareTo(lastentry.getKey()) >= 0)
                    {
                        return null;
                    }
                }
                else
                {
                    if (hasStart && cmp.compare(e.getKey(), startEntry.getKey()) < 0)
                    {
                        return map.higherEntry(startEntry.getKey());
                    }
                    if (hasEnd && cmp.compare(e.getKey(), lastentry.getKey()) >= 0)
                    {
                        return null;
                    }
                }
                return map.higherEntry(e.getKey());
            }

            public java.util.MapNS.Entry<K, V> lower(java.util.MapNS.Entry<K, V> e)
            {
                Comparator<K> cmp = map.m.comparatorJ;
                if (cmp == null)
                {
                    java.lang.Comparable<K> obj = toComparable(e.getKey());
                    if (hasStart && obj.compareTo(startEntry.getKey()) < 0)
                    {
                        return null;
                    }
                    if (hasEnd && obj.compareTo(lastentry.getKey()) >= 0)
                    {
                        return map.lowerEntry(lastentry.getKey());
                    }
                }
                else
                {
                    if (hasStart && cmp.compare(e.getKey(), startEntry.getKey()) < 0)
                    {
                        return null;
                    }
                    if (hasEnd && cmp.compare(e.getKey(), lastentry.getKey()) >= 0)
                    {
                        return map.lowerEntry(lastentry.getKey());
                    }
                }
                return map.lowerEntry(e.getKey());
            }

            public java.util.MapNS.Entry<K, V> pollFirst()
            {
                MapNS.Entry<K, V> ret = map.firstEntry();
                if (ret == null)
                {
                    return null;
                }
                map.m.remove(ret.getKey());
                return ret;
            }

            public java.util.MapNS.Entry<K, V> pollLast()
            {
                MapNS.Entry<K, V> ret = map.lastEntry();
                if (ret == null)
                {
                    return null;
                }
                map.m.remove(ret.getKey());
                return ret;
            }

            public NavigableSet<java.util.MapNS.Entry<K, V>> subSet(java.util.MapNS.Entry<K, V> start, bool startInclusive, java.util.MapNS.Entry<K, V> end, bool endInclusive)
            {
                if (map.m.keyCompare(start.getKey(), end.getKey()) > 0)
                {
                    throw new java.lang.IllegalArgumentException();
                }
                if (map.fromStart
                        && ((!map.loInclusive && endInclusive) ? map.m.keyCompare(end.getKey(), map.lo) <= 0
                                : map.m.keyCompare(end.getKey(), map.lo) < 0))
                {
                    throw new java.lang.IllegalArgumentException();
                }
                if (map.toEnd
                        && ((!map.hiInclusive && startInclusive) ? map.m.keyCompare(start.getKey(), map.hi) >= 0
                                : map.m.keyCompare(start.getKey(), map.hi) > 0))
                {
                    throw new java.lang.IllegalArgumentException();
                }
                return new AscendingSubMapEntrySet<K, V>(map, start, startInclusive, end, endInclusive);
            }

            public NavigableSet<java.util.MapNS.Entry<K, V>> tailSet(java.util.MapNS.Entry<K, V> start, bool startInclusive)
            {
                bool isInRange = true;
                int result = 0;
                if (map.toEnd)
                {
                    result = (null != comparator()) ? keyComparator().compare(start.getKey(),
                            map.hi) : toComparable(start.getKey()).compareTo(map.hi);
                    isInRange = (map.hiInclusive || !startInclusive) ? result <= 0
                            : result < 0;
                }
                if (map.fromStart)
                {
                    result = (null != comparator()) ? keyComparator().compare(start.getKey(),
                            map.lo) : toComparable(start.getKey()).compareTo(map.lo);
                    isInRange = isInRange
                            && ((map.loInclusive || !startInclusive) ? result >= 0
                                    : result > 0);
                }

                if (isInRange)
                {
                    return new AscendingSubMapEntrySet<K, V>(map, start, startInclusive, null, false);
                }
                throw new java.lang.IllegalArgumentException();

            }

            public virtual java.util.Comparator<MapNS.Entry<K,V>> comparator()
            {
                return (java.util.Comparator<MapNS.Entry<K, V>>)map.m.comparatorJ;
            }
            public virtual java.util.Comparator<K> keyComparator()
            {
                return map.m.comparatorJ;
            }


            public virtual java.util.MapNS.Entry<K, V> first()
            {
                if (hasStart)
                {
                    if (startInclusive)
                    {
                        return startEntry;
                    }
                    else
                    {
                        return map.floorEntry(startEntry.getKey());
                    }
                }
                java.util.MapNS.Entry<K, V> ret = map.firstEntry();
                if (ret == null)
                {
                    throw new NoSuchElementException();
                }
                return ret;
            }

            public virtual SortedSet<java.util.MapNS.Entry<K, V>> headSet(java.util.MapNS.Entry<K, V> end)
            {
                return headSet(end, false);
            }

            public virtual java.util.MapNS.Entry<K, V> last()
            {
                if (hasEnd)
                {
                    if (endInclusive)
                    {
                        return lastentry;
                    }
                    else
                    {
                        return map.ceilingEntry(lastentry.getKey());
                    }
                }
                java.util.MapNS.Entry<K, V> ret = map.lastEntry();
                if (ret == null)
                {
                    throw new NoSuchElementException();
                }
                return ret;
            }

            public virtual SortedSet<java.util.MapNS.Entry<K, V>> subSet(java.util.MapNS.Entry<K, V> start, java.util.MapNS.Entry<K, V> end)
            {
                if ((null != comparator()) ? keyComparator().compare(start.getKey(), end.getKey()) > 0
                        : toComparable(start.getKey()).compareTo(end.getKey()) > 0)
                {
                    throw new java.lang.IllegalArgumentException();
                }
                if (!map.isInRange(start.getKey()))
                {
                    throw new java.lang.IllegalArgumentException();
                }
                if (!map.isInRange(end.getKey()))
                {
                    throw new java.lang.IllegalArgumentException();
                }
                return new AscendingSubMapEntrySet<K, V>(map, start, false, end, false);
            }

            public virtual SortedSet<java.util.MapNS.Entry<K, V>> tailSet(java.util.MapNS.Entry<K, V> start)
            {
                return tailSet(start, false);
            }
        }

        class DescendingSubMapEntrySet<K, V> :
                 AbstractSet<MapNS.Entry<K, V>>, NavigableSet<MapNS.Entry<K, V>>
        {
            NavigableSubMap<K, V> map;

            internal DescendingSubMapEntrySet(NavigableSubMap<K, V> map)
            {
                this.map = map;
            }


            public override Iterator<MapNS.Entry<K, V>> iterator()
            {
                return new DescendingSubMapEntryIterator<K, V>(map);
            }


            public override int size()
            {
                int size = 0;
                DescendingSubMapEntryIterator<K, V> it = new DescendingSubMapEntryIterator<K, V>(map);
                while (it.hasNext())
                {
                    it.next();
                    size++;
                }
                return size;
            }

            public java.util.MapNS.Entry<K, V> ceiling(java.util.MapNS.Entry<K, V> e)
            {
                TreeMap<K,V>.Entry<K,V> node = map.m.findFloorEntry(e.getKey());
                if (!map.checkUpperBound(node.key))
                {
                    node = map.findEndNode();
                }

                if (!map.checkLowerBound(node.key))
                {
                    java.lang.Comparable<K> obj = map.comparator() == null ? toComparable((K)e.getKey())
                            : null;
                    TreeMap<K, V>.Entry<K, V> first = map.loInclusive ? map.m.findFloorEntry(map.lo) : map.m.findLowerEntry(map.lo);
                    if (first != null && map.cmp(obj, e.getKey(), first.getKey()) <= 0)
                    {
                        node = first;
                    }
                    else
                    {
                        node = null;
                    }
                }
                return node;
            }

            public Iterator<java.util.MapNS.Entry<K, V>> descendingIterator()
            {
                return descendingSet().iterator();
            }

            public NavigableSet<java.util.MapNS.Entry<K, V>> descendingSet()
            {
                if (map.fromStart && map.toEnd)
                {
                    return new AscendingSubMapEntrySet<K, V>(
                            new AscendingSubMap<K, V>(map.hi, map.hiInclusive,
                                    map.m, map.lo, map.loInclusive));
                }
                if (map.fromStart)
                {
                    return new AscendingSubMapEntrySet<K, V>(
                            new AscendingSubMap<K, V>(map.m, map.lo,
                                    map.loInclusive));
                }
                if (map.toEnd)
                {
                    return new AscendingSubMapEntrySet<K, V>(
                            new AscendingSubMap<K, V>(map.hi, map.hiInclusive,
                                    map.m));
                }
                return new AscendingSubMapEntrySet<K, V>(new AscendingSubMap<K, V>(
                        map.m));
            }

            public java.util.MapNS.Entry<K, V> floor(java.util.MapNS.Entry<K, V> e)
            {
                TreeMap<K,V>.Entry<K,V> node = map.m.findCeilingEntry(e.getKey());
                if (!map.checkUpperBound(node.key))
                {
                    node = map.findEndNode();
                }

                if (!map.checkLowerBound(node.key))
                {
                    java.lang.Comparable<K> obj = map.m.comparatorJ == null ? toComparable((K)e.getKey())
                            : null;
                    TreeMap<K, V>.Entry<K, V> first = map.hiInclusive ? map.m.findCeilingEntry(map.hi) : map.m.findHigherEntry(map.hi);
                    if (first != null && map.cmp(obj, e.getKey(), first.getKey()) < 0)
                    {
                        node = first;
                    }
                    else
                    {
                        node = null;
                    }
                }
                return node;
            }

            void checkInRange(K key, bool keyInclusive)
            {
                bool isInRange = true;
                int result = 0;
                if (map.toEnd)
                {
                    result = (null != map.comparator()) ? keyComparator().compare(
                            key, map.hi) : toComparable(key).compareTo(map.hi);
                    isInRange = ((!map.hiInclusive) && keyInclusive) ? result < 0
                            : result <= 0;
                }
                if (map.fromStart)
                {
                    result = (null != comparator()) ? keyComparator().compare(key,
                            map.lo) : toComparable(key).compareTo(map.lo);
                    isInRange = isInRange
                            && (((!map.loInclusive) && keyInclusive) ? result > 0
                                    : result >= 0);
                }
                if (!isInRange)
                {
                    throw new java.lang.IllegalArgumentException();
                }
            }

            public NavigableSet<java.util.MapNS.Entry<K, V>> headSet(
                    java.util.MapNS.Entry<K, V> end, bool endInclusive)
            {
                bool outRange = true;
                int result = 0;
                if (map.toEnd)
                {
                    result = (null != map.comparator()) ? keyComparator().compare(
                            end.getKey(), map.hi) : toComparable(end.getKey()).compareTo(map.hi);
                    outRange = ((!map.hiInclusive) && endInclusive) ? result >= 0
                            : result > 0;
                    if (outRange)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                }
                if (map.fromStart)
                {
                    result = (null != comparator()) ? keyComparator().compare(end.getKey(),
                            map.lo) : toComparable(end.getKey()).compareTo(map.lo);
                    outRange = (((!map.loInclusive) && endInclusive) ? result <= 0
                                    : result < 0);
                    if (outRange)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                }

                if (map.fromStart)
                {
                    return new DescendingSubMapEntrySet<K, V>(new DescendingSubMap<K, V>(
                            map.lo, map.loInclusive, map.m, end.getKey(), endInclusive));
                }
                else
                {
                    return new DescendingSubMapEntrySet<K, V>(new DescendingSubMap<K, V>(
                            map.m, end.getKey(), endInclusive));
                }
            }

            public java.util.MapNS.Entry<K, V> higher(java.util.MapNS.Entry<K, V> e)
            {
                TreeMap<K,V>.Entry<K,V> node = map.m.findLowerEntry(e.getKey());
                if (node != null && !map.checkUpperBound(node.key))
                {
                    node = map.hiInclusive ? map.findFloorEntryImpl(map.hi) : map.findLowerEntryImpl(map.hi);
                }
                java.lang.Comparable<K> obj = map.comparator() == null ? toComparable((K)e.getKey())
                        : null;
                if (node != null && (map.cmp(obj, e.getKey(), node.key)) > 0)
                {
                    return null;
                }
                if (node != null && !map.checkLowerBound(node.key))
                {
                    TreeMap<K, V>.Entry<K, V> first = map.loInclusive ? map.m.findFloorEntry(map.lo) : map.m.findLowerEntry(map.lo);
                    if (first != null && map.cmp(obj, e.getKey(), first.getKey()) < 0)
                    {
                        node = first;
                    }
                    else
                    {
                        node = null;
                    }
                }
                return node;
            }

            public java.util.MapNS.Entry<K, V> lower(java.util.MapNS.Entry<K, V> e)
            {
                TreeMap<K,V>.Entry<K,V> node = map.m.findHigherEntry(e.getKey());
                if (node != null && !map.checkUpperBound(node.key))
                {
                    node = map.loInclusive ? map.findCeilingEntryImpl(map.hi) : map.findHigherEntryImpl(map.hi);
                }
                java.lang.Comparable<K> obj = map.m.comparatorJ == null ? toComparable((K)e.getKey())
                        : null;
                if (node != null && (map.cmp(obj, e.getKey(), node.key)) >= 0)
                {
                    return null;
                }
                if (node != null && !map.checkLowerBound(node.key))
                {
                    MapNS.Entry<K, V> first = map.firstEntry();
                    if (first != null && map.cmp(obj, e.getKey(), first.getKey()) < 0)
                    {
                        node = map.findStartNode();
                    }
                    else
                    {
                        node = null;
                    }
                }
                return node;
            }

            public java.util.MapNS.Entry<K, V> pollFirst()
            {
                MapNS.Entry<K, V> ret = map.lastEntry();
                if (ret == null)
                {
                    return null;
                }
                map.m.remove(ret.getKey());
                return ret;
            }

            public java.util.MapNS.Entry<K, V> pollLast()
            {
                MapNS.Entry<K, V> ret = map.firstEntry();
                if (ret == null)
                {
                    return null;
                }
                map.m.remove(ret.getKey());
                return ret;
            }

            public NavigableSet<java.util.MapNS.Entry<K, V>> subSet(
                    java.util.MapNS.Entry<K, V> start, bool startInclusive,
                    java.util.MapNS.Entry<K, V> end, bool endInclusive)
            {
                java.lang.Comparable<K> startobject = map.comparator() == null ? toComparable((K)start
                        .getKey())
                        : null;
                java.lang.Comparable<K> endobject = map.comparator() == null ? toComparable((K)end
                        .getKey())
                        : null;
                if (map.fromStart
                        && ((!map.loInclusive && startInclusive) ? map.cmp(
                                startobject, start.getKey(), map.lo) <= 0 : map
                                .cmp(startobject, start.getKey(), map.lo) < 0)
                        || (map.toEnd && ((!map.hiInclusive && endInclusive) ? map
                                .cmp(endobject, end.getKey(), map.hi) >= 0 : map
                                .cmp(endobject, end.getKey(), map.hi) > 0)))
                {
                    throw new java.lang.IllegalArgumentException();
                }
                if (map.cmp(startobject, start.getKey(), end.getKey()) > 0)
                {
                    throw new java.lang.IllegalArgumentException();
                }

                return new DescendingSubMapEntrySet<K, V>(
                        new DescendingSubMap<K, V>(start.getKey(), startInclusive,
                                map.m, end.getKey(), endInclusive));
            }

            public NavigableSet<java.util.MapNS.Entry<K, V>> tailSet(
                    java.util.MapNS.Entry<K, V> start, bool startInclusive)
            {
                if (map.toEnd)
                {
                    return new DescendingSubMapEntrySet<K, V>(
                            new DescendingSubMap<K, V>(start.getKey(),
                                    startInclusive, map.m, map.hi, map.hiInclusive));
                }
                else
                {
                    return new DescendingSubMapEntrySet<K, V>(
                            new DescendingSubMap<K, V>(start.getKey(),
                                    startInclusive, map.m));
                }
            }

            public virtual Comparator<MapNS.Entry<K,V>> comparator()
            {
                return (Comparator<MapNS.Entry<K,V>>) map.comparator();
            }
            public virtual Comparator<K> keyComparator()
            {
                return map.comparator();
            }


            public java.util.MapNS.Entry<K, V> first()
            {
                java.util.MapNS.Entry<K, V> ret = map.lastEntry();
                if (ret == null)
                {
                    throw new NoSuchElementException();
                }
                return ret;
            }

            public SortedSet<java.util.MapNS.Entry<K, V>> headSet(java.util.MapNS.Entry<K, V> end)
            {
                return headSet(end, false);
            }

            public java.util.MapNS.Entry<K, V> last()
            {
                java.util.MapNS.Entry<K, V> ret = map.firstEntry();
                if (ret == null)
                {
                    throw new NoSuchElementException();
                }
                return ret;
            }

            public SortedSet<java.util.MapNS.Entry<K, V>> subSet(
                    java.util.MapNS.Entry<K, V> start, java.util.MapNS.Entry<K, V> end)
            {
                return subSet(start, true, end, false);
            }

            int keyCompare(K left, K right)
            {
                return (null != map.comparator()) ? map.comparator().compare(left,
                        right) : toComparable(left).compareTo(right);
            }

            public SortedSet<java.util.MapNS.Entry<K, V>> tailSet(java.util.MapNS.Entry<K, V> start)
            {
                return tailSet(start, true);
            }
        }

        class AscendingSubMapKeySet<K, V> : AbstractSet<K>, NavigableSet<K>
        {
            NavigableSubMap<K, V> map;

            internal AscendingSubMapKeySet(NavigableSubMap<K, V> map)
            {
                this.map = map;
            }


            public override Iterator<K> iterator()
            {
                return new AscendingSubMapKeyIterator<K, V>(map);
            }

            public Iterator<K> descendingIterator()
            {
                return new DescendingSubMapKeyIterator<K, V>(map.descendingSubMap());
            }


            public override int size()
            {
                int size = 0;
                AscendingSubMapEntryIterator<K, V> it = new AscendingSubMapEntryIterator<K, V>(map);
                while (it.hasNext())
                {
                    it.next();
                    size++;
                }
                return size;
            }

            public K ceiling(K e)
            {
                TreeMap<K,V>.Entry<K,V> ret = map.findFloorEntry(e);
                if (ret != null && map.isInRange(ret.key))
                {
                    return ret.key;
                }
                else
                {
                    return default(K);
                }
            }

            public NavigableSet<K> descendingSet()
            {
                return new DescendingSubMapKeySet<K, V>(map.descendingSubMap());
            }

            public K floor(K e)
            {
                TreeMap<K,V>.Entry<K,V> ret = map.findFloorEntry(e);
                if (ret != null && map.isInRange(ret.key))
                {
                    return ret.key;
                }
                else
                {
                    return default(K);
                }
            }

            public virtual NavigableSet<K> headSet(K end, bool endInclusive)
            {
                bool isInRange = true;
                int result;
                if (map.toEnd)
                {
                    result = (null != comparator()) ? comparator().compare(end,
                            map.hi) : toComparable(end).compareTo(map.hi);
                    isInRange = (map.hiInclusive || !endInclusive) ? result <= 0
                            : result < 0;
                }
                if (map.fromStart)
                {
                    result = (null != comparator()) ? comparator().compare(end,
                            map.lo) : toComparable(end).compareTo(map.lo);
                    isInRange = isInRange
                            && ((map.loInclusive || !endInclusive) ? result >= 0
                                    : result > 0);
                }
                if (isInRange)
                {
                    if (map.fromStart)
                    {
                        return new AscendingSubMapKeySet<K, V>(
                                new AscendingSubMap<K, V>(map.lo, map.loInclusive,
                                        map.m, end, endInclusive));
                    }
                    else
                    {
                        return new AscendingSubMapKeySet<K, V>(
                                new AscendingSubMap<K, V>(map.m, end, endInclusive));
                    }
                }
                throw new java.lang.IllegalArgumentException();
            }

            public K higher(K e)
            {
                K ret = map.m.higherKey(e);
                if (ret != null && map.isInRange(ret))
                {
                    return ret;
                }
                else
                {
                    return default(K);
                }
            }

            public K lower(K e)
            {
                K ret = map.m.lowerKey(e);
                if (ret != null && map.isInRange(ret))
                {
                    return ret;
                }
                else
                {
                    return default(K);
                }
            }

            public K pollFirst()
            {
                MapNS.Entry<K, V> ret = map.firstEntry();
                if (ret == null)
                {
                    return default(K);
                }
                map.m.remove(ret.getKey());
                return ret.getKey();
            }

            public K pollLast()
            {
                MapNS.Entry<K, V> ret = map.lastEntry();
                if (ret == null)
                {
                    return default(K);
                }
                map.m.remove(ret.getKey());
                return ret.getKey();
            }

            public NavigableSet<K> subSet(K start, bool startInclusive, K end, bool endInclusive)
            {
                if (map.fromStart
                        && ((!map.loInclusive && startInclusive) ? map.m.keyCompare(start,
                                map.lo) <= 0 : map.m.keyCompare(start, map.lo) < 0)
                        || (map.toEnd && ((!map.hiInclusive && (endInclusive || (startInclusive && start.equals(end)))) ? map.m.keyCompare(
                                end, map.hi) >= 0
                                : map.m.keyCompare(end, map.hi) > 0)))
                {
                    throw new java.lang.IllegalArgumentException();
                }
                if (map.m.keyCompare(start, end) > 0)
                {
                    throw new java.lang.IllegalArgumentException();
                }
                return new AscendingSubMapKeySet<K, V>(new AscendingSubMap<K, V>(start,
                        startInclusive, map.m, end, endInclusive));
            }

            public virtual NavigableSet<K> tailSet(K start, bool startInclusive)
            {
                bool isInRange = true;
                int result;
                if (map.toEnd)
                {
                    result = (null != comparator()) ? comparator().compare(start,
                            map.hi) : toComparable(start).compareTo(map.hi);
                    isInRange = (map.hiInclusive || !startInclusive) ? result <= 0
                            : result < 0;
                }
                if (map.fromStart)
                {
                    result = (null != comparator()) ? comparator().compare(start,
                            map.lo) : toComparable(start).compareTo(map.lo);
                    isInRange = isInRange
                            && ((map.loInclusive || !startInclusive) ? result >= 0
                                    : result > 0);
                }

                if (isInRange)
                {
                    if (map.toEnd)
                    {
                        return new AscendingSubMapKeySet<K, V>(
                                new AscendingSubMap<K, V>(start, startInclusive,
                                        map.m, map.hi, map.hiInclusive));
                    }
                    else
                    {
                        return new AscendingSubMapKeySet<K, V>(
                                new AscendingSubMap<K, V>(start, startInclusive,
                                        map.m));
                    }
                }
                throw new java.lang.IllegalArgumentException();
            }

            public virtual Comparator<K> comparator()
            {
                return map.m.comparatorJ;
            }

            public virtual K first()
            {
                return map.firstKey();
            }

            public virtual SortedSet<K> headSet(K end)
            {
                return headSet(end, false);
            }

            public virtual K last()
            {
                return map.lastKey();
            }

            public virtual SortedSet<K> subSet(K start, K end)
            {
                return subSet(start, true, end, false);
            }

            public virtual SortedSet<K> tailSet(K start)
            {
                return tailSet(start, true);
            }

            public override bool contains(Object obj)
            {
                return map.containsKey(obj);
            }

            public override bool remove(Object obj)
            {
                return this.map.remove(obj) != null;
            }
        }

        class DescendingSubMapKeySet<K, V> : AbstractSet<K>, NavigableSet<K>
        {
            NavigableSubMap<K, V> map;

            internal DescendingSubMapKeySet(NavigableSubMap<K, V> map)
            {
                this.map = map;
            }

            public override Iterator<K> iterator()
            {
                return new DescendingSubMapKeyIterator<K, V>(map);
            }

            public Iterator<K> descendingIterator()
            {
                if (map.fromStart && map.toEnd)
                {
                    return new AscendingSubMapKeyIterator<K, V>(
                            new AscendingSubMap<K, V>(map.hi, map.hiInclusive,
                                    map.m, map.lo, map.loInclusive));
                }
                if (map.toEnd)
                {
                    return new AscendingSubMapKeyIterator<K, V>(
                            new AscendingSubMap<K, V>(map.hi, map.hiInclusive,
                                    map.m));
                }
                if (map.fromStart)
                {
                    return new AscendingSubMapKeyIterator<K, V>(
                            new AscendingSubMap<K, V>(map.m, map.lo,
                                    map.loInclusive));
                }
                return new AscendingSubMapKeyIterator<K, V>(
                        new AscendingSubMap<K, V>(map.m));
            }


            public override int size()
            {
                int size = 0;
                DescendingSubMapEntryIterator<K, V> it = new DescendingSubMapEntryIterator<K, V>(map);
                while (it.hasNext())
                {
                    it.next();
                    size++;
                }
                return size;
            }

            public NavigableSet<K> descendingSet()
            {
                if (map.fromStart && map.toEnd)
                {
                    return new AscendingSubMapKeySet<K, V>(
                            new AscendingSubMap<K, V>(map.hi, map.hiInclusive,
                                    map.m, map.lo, map.loInclusive));
                }
                if (map.toEnd)
                {
                    return new AscendingSubMapKeySet<K, V>(
                            new AscendingSubMap<K, V>(map.hi, map.hiInclusive,
                                    map.m));
                }
                if (map.fromStart)
                {
                    return new AscendingSubMapKeySet<K, V>(
                            new AscendingSubMap<K, V>(map.m, map.lo,
                                    map.loInclusive));
                }
                return new AscendingSubMapKeySet<K, V>(
                        new AscendingSubMap<K, V>(map.m));
            }

            public K ceiling(K e)
            {
                java.lang.Comparable<K> obj = map.comparator() == null ? toComparable((K)e)
                        : null;
                TreeMap<K,V>.Entry<K,V> node = map.m.findFloorEntry(e);
                if (node != null && !map.checkUpperBound(node.key))
                {
                    return default(K);
                }

                if (node != null && !map.checkLowerBound(node.key))
                {
                    TreeMap<K,V>.Entry<K,V> first = map.loInclusive ? map.m.findFloorEntry(map.lo) : map.m.findLowerEntry(map.lo);
                    if (first != null && map.cmp(obj, e, first.key) <= 0 && map.checkUpperBound(first.key))
                    {
                        node = first;
                    }
                    else
                    {
                        node = null;
                    }
                }
                return node == null ? default(K) : node.key;
            }

            public K floor(K e)
            {
                TreeMap<K,V>.Entry<K,V> node = map.m.findCeilingEntry(e);
                if (node != null && !map.checkUpperBound(node.key))
                {
                    node = map.hiInclusive ? map.m.findCeilingEntry(map.hi) : map.m.findHigherEntry(map.hi);
                }

                if (node != null && !map.checkLowerBound(node.key))
                {
                    java.lang.Comparable<K> obj = map.comparator() == null ? toComparable((K)e)
                            : null;
                    TreeMap<K,V>.Entry<K,V> first = map.loInclusive ? map.m.findFloorEntry(map.lo) : map.m.findLowerEntry(map.lo);
                    if (first != null && map.cmp(obj, e, first.key) > 0 && map.checkUpperBound(first.key))
                    {
                        node = first;
                    }
                    else
                    {
                        node = null;
                    }
                }
                return node == null ? default(K) : node.key;
            }

            public NavigableSet<K> headSet(K end, bool endInclusive)
            {
                checkInRange(end, endInclusive);
                if (map.fromStart)
                {
                    return new DescendingSubMapKeySet<K, V>(
                            new DescendingSubMap<K, V>(map.lo, map.loInclusive,
                                    map.m, end, endInclusive));
                }
                else
                {
                    return new DescendingSubMapKeySet<K, V>(
                            new DescendingSubMap<K, V>(map.m, end, endInclusive));
                }
            }

            public K higher(K e)
            {
                java.lang.Comparable<K> obj = map.comparator() == null ? toComparable((K)e)
                        : null;
                TreeMap<K,V>.Entry<K,V> node = map.m.findLowerEntry(e);
                if (node != null && !map.checkUpperBound(node.key))
                {
                    return default(K);
                }

                if (node != null && !map.checkLowerBound(node.key))
                {
                    TreeMap<K,V>.Entry<K,V> first = map.loInclusive ? map.m.findFloorEntry(map.lo) : map.m.findLowerEntry(map.lo);
                    if (first != null && map.cmp(obj, e, first.key) < 0 && map.checkUpperBound(first.key))
                    {
                        node = first;
                    }
                    else
                    {
                        node = null;
                    }
                }
                return node == null ? default(K) : node.key;
            }

            public K lower(K e)
            {
                TreeMap<K,V>.Entry<K,V> node = map.m.findHigherEntry(e);
                if (node != null && !map.checkUpperBound(node.key))
                {
                    node = map.hiInclusive ? map.m.findCeilingEntry(map.hi) : map.m.findHigherEntry(map.hi);
                }

                if (node != null && !map.checkLowerBound(node.key))
                {
                    java.lang.Comparable<K> obj = map.comparator() == null ? toComparable((K)e)
                            : null;
                    TreeMap<K,V>.Entry<K,V> first = map.loInclusive ? map.m.findFloorEntry(map.lo) : map.m.findLowerEntry(map.lo);
                    if (first != null && map.cmp(obj, e, first.key) > 0 && map.checkUpperBound(first.key))
                    {
                        node = first;
                    }
                    else
                    {
                        node = null;
                    }
                }
                return node == null ? default(K) : node.key;
            }

            public K pollFirst()
            {
                MapNS.Entry<K, V> ret = map.firstEntry();
                if (ret == null)
                {
                    return default(K);
                }
                map.m.remove(ret.getKey());
                return ret.getKey();
            }

            public K pollLast()
            {
                MapNS.Entry<K, V> ret = map.lastEntry();
                if (ret == null)
                {
                    return default(K);
                }
                map.m.remove(ret.getKey());
                return ret.getKey();
            }

            public NavigableSet<K> subSet(K start, bool startInclusive, K end, bool endInclusive)
            {
                checkInRange(start, startInclusive);
                checkInRange(end, endInclusive);
                if ((null != map.comparator()) ? map.comparator().compare(
                        start, end) > 0 : toComparable(start).compareTo(end) > 0)
                {
                    throw new java.lang.IllegalArgumentException();
                }
                return new DescendingSubMapKeySet<K, V>(new DescendingSubMap<K, V>(
                        start, startInclusive, map.m, end, endInclusive));
            }

            public NavigableSet<K> tailSet(K start, bool startInclusive)
            {
                checkInRange(start, startInclusive);
                if (map.toEnd)
                {
                    return new DescendingSubMapKeySet<K, V>(
                            new DescendingSubMap<K, V>(start, startInclusive,
                                    map.m, map.hi, map.hiInclusive));
                }
                else
                {
                    return new DescendingSubMapKeySet<K, V>(
                            new DescendingSubMap<K, V>(start, startInclusive, map.m));
                }
            }

            void checkInRange(K key, bool keyInclusive)
            {
                bool isInRange = true;
                int result = 0;
                if (map.toEnd)
                {
                    result = (null != map.comparator()) ? map.comparator().compare(
                            key, map.hi) : toComparable(key).compareTo(map.hi);
                    isInRange = ((!map.hiInclusive) && keyInclusive) ? result < 0
                            : result <= 0;
                }
                if (map.fromStart)
                {
                    result = (null != comparator()) ? comparator().compare(key,
                            map.lo) : toComparable(key).compareTo(map.lo);
                    isInRange = isInRange
                            && (((!map.loInclusive) && keyInclusive) ? result > 0
                                    : result >= 0);
                }
                if (!isInRange)
                {
                    throw new java.lang.IllegalArgumentException();
                }
            }

            public Comparator<K> comparator()
            {
                return map.comparator();
            }

            public K first()
            {
                return map.firstKey();
            }

            public SortedSet<K> headSet(K end)
            {
                return headSet(end, false);
            }

            public K last()
            {
                return map.lastKey();
            }

            public SortedSet<K> subSet(K start, K end)
            {
                return subSet(start, true, end, false);
            }

            public SortedSet<K> tailSet(K start)
            {
                return tailSet(start, true);
            }
        }
        [Serializable]
        abstract class NavigableSubMap<K, V> : AbstractMap<K, V>
                , NavigableMap<K, V>, java.io.Serializable
        {
            public abstract NavigableMap<K, V> descendingMap();

            private static readonly long serialVersionUID = -7141723745034997872L;

            internal readonly TreeMap<K, V> m;

            internal readonly K lo, hi;

            internal readonly bool fromStart, toEnd;

            internal readonly bool loInclusive, hiInclusive;

            internal NavigableSubMap(K start, bool startKeyInclusive,
                    TreeMap<K, V> map, K end,
                    bool endKeyInclusive)
            {
                m = map;
                fromStart = toEnd = true;
                lo = start;
                hi = end;
                loInclusive = startKeyInclusive;
                hiInclusive = endKeyInclusive;
            }

            internal NavigableSubMap(K start, bool startKeyInclusive,
                    TreeMap<K, V> map)
            {
                m = map;
                fromStart = true;
                toEnd = false;
                lo = start;
                hi = default(K);
                loInclusive = startKeyInclusive;
                hiInclusive = false;
            }

            internal NavigableSubMap(TreeMap<K, V> map, K end,
                    bool endKeyInclusive)
            {
                m = map;
                fromStart = false;
                toEnd = true;
                lo = default(K);
                hi = end;
                loInclusive = false;
                hiInclusive = endKeyInclusive;
            }

            // the whole TreeMap
            internal NavigableSubMap(TreeMap<K, V> map)
            {
                m = map;
                fromStart = toEnd = false;
                lo = hi = default(K);
                loInclusive = hiInclusive = false;
            }

            internal TreeMap<K,V>.Node<K, V> findNode(K key)
            {
                return m.findNode(key);
            }

            /*
             * The basic public methods.
             */

            public virtual Comparator<K> comparator()
            {
                return m.comparator();
            }



            public override bool containsKey(Object key)
            {
                checkNull(key);
                if (isInRange((K)key))
                {
                    return m.containsKey(key);
                }
                return false;
            }

            private void checkNull(Object key)
            {
                if (null == key && null == comparator())
                {
                    throw new java.lang.NullPointerException();
                }
            }


            public override bool isEmpty()
            {
                Iterator<K> it = this.keySet().iterator();
                if (it.hasNext())
                {
                    return false;
                }
                return true;
            }

            public override int size()
            {
                return entrySet().size();
            }

            public override V put(K key, V value)
            {
                checkNull(key);
                if (isInRange(key))
                {
                    return m.put(key, value);
                }
                throw new java.lang.IllegalArgumentException();
            }


            public override V get(Object key)
            {
                checkNull(key);
                if (isInRange((K)key))
                {
                    return m.get(key);
                }
                return default(V);
            }



            public override V remove(Object key)
            {
                checkNull(key);
                if (isInRange((K)key))
                {
                    return m.remove(key);
                }
                return default(V);
            }

            /*
             * The navigable methods.
             */

            public abstract MapNS.Entry<K, V> firstEntry();

            public abstract MapNS.Entry<K, V> lastEntry();

            public abstract MapNS.Entry<K, V> pollFirstEntry();

            public abstract MapNS.Entry<K, V> pollLastEntry();

            public abstract MapNS.Entry<K, V> higherEntry(K key);

            public abstract MapNS.Entry<K, V> lowerEntry(K key);

            public abstract MapNS.Entry<K, V> ceilingEntry(K key);

            public abstract MapNS.Entry<K, V> floorEntry(K key);

            protected internal abstract NavigableSubMap<K, V> descendingSubMap();

            public K firstKey()
            {
                MapNS.Entry<K, V> node = firstEntry();
                if (node != null)
                {
                    return node.getKey();
                }
                throw new NoSuchElementException();
            }

            public K lastKey()
            {
                MapNS.Entry<K, V> node = lastEntry();
                if (node != null)
                {
                    return node.getKey();
                }
                throw new NoSuchElementException();
            }

            public K higherKey(K key)
            {
                MapNS.Entry<K, V> entry = higherEntry(key);
                return (null == entry) ? default(K) : entry.getKey();
            }

            public K lowerKey(K key)
            {
                MapNS.Entry<K, V> entry = lowerEntry(key);
                return (null == entry) ? default(K) : entry.getKey();
            }

            public K ceilingKey(K key)
            {
                MapNS.Entry<K, V> entry = ceilingEntry(key);
                return (null == entry) ? default(K) : entry.getKey();
            }

            public K floorKey(K key)
            {
                MapNS.Entry<K, V> entry = floorEntry(key);
                return (null == entry) ? default(K) : entry.getKey();
            }

            /*
             * The sub-collection methods.
             */

            public abstract NavigableSet<K> navigableKeySet();


            public override abstract Set<MapNS.Entry<K, V>> entrySet();


            public override Set<K> keySet()
            {
                return navigableKeySet();
            }

            public NavigableSet<K> descendingKeySet()
            {
                return m.descendingMap().navigableKeySet();
            }

            public virtual SortedMap<K, V> subMap(K start, K end)
            {
                // the exception check is special here, the end should not if equal
                // to endkey unless start is equal to end
                if (!checkLowerBound(start) || !checkUpperBound(start))
                {
                    throw new java.lang.IllegalArgumentException();
                }
                int result = -1;
                if (toEnd)
                {
                    result = (null != comparator()) ? comparator().compare(end, hi)
                            : toComparable(end).compareTo(hi);
                }
                if (((!hiInclusive && start.equals(end)) ? result < 0 : result <= 0))
                {
                    if ((null != comparator()) ? comparator().compare(start, end) > 0
                            : toComparable(start).compareTo(end) > 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                    return new AscendingSubMap<K, V>(start, true, m, end,
                            false);
                }
                throw new java.lang.IllegalArgumentException();
            }

            public SortedMap<K, V> headMap(K end)
            {
                if (toEnd)
                {
                    int result = (null != comparator()) ? comparator().compare(end, hi)
                            : toComparable(end).compareTo(hi);
                    if (result > 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                }
                if (fromStart)
                {
                    int result = -((null != comparator()) ? comparator().compare(
                            lo, end) : toComparable(lo).compareTo(end));
                    if (result < 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                }
                return headMap(end, false);
            }

            public SortedMap<K, V> tailMap(K start)
            {
                if (fromStart)
                {
                    int result = -((null != comparator()) ? comparator().compare(
                            lo, start) : toComparable(lo).compareTo(start));
                    if (loInclusive ? result < 0 : result <= 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                }
                if (toEnd)
                {
                    int result = (null != comparator()) ? comparator().compare(start, hi)
                            : toComparable(start).compareTo(hi);
                    if (hiInclusive ? result > 0 : result >= 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                }
                return tailMap(start, true);
            }

            public abstract NavigableMap<K, V> subMap(K start,
                    bool startKeyInclusive, K end, bool endKeyInclusive);

            public abstract NavigableMap<K, V> headMap(K end, bool inclusive);

            public abstract NavigableMap<K, V> tailMap(K start, bool inclusive);

            /**
             * 
             * @param key
             * @return false if the key bigger than the end key (if any)
             */
            internal bool checkUpperBound(K key)
            {
                if (toEnd)
                {
                    int result = (null != comparator()) ? comparator().compare(key, hi)
                            : toComparable(key).compareTo(hi);
                    return hiInclusive ? result <= 0 : result < 0;
                }
                return true;
            }

            /**
             * 
             * @param key
             * @return false if the key smaller than the start key (if any)
             */
            internal bool checkLowerBound(K key)
            {
                if (fromStart)
                {
                    int result = -((null != comparator()) ? comparator().compare(lo, key)
                            : toComparable(lo).compareTo(key));
                    return loInclusive ? result >= 0 : result > 0;
                }
                return true;
            }

            internal bool isInRange(K key)
            {
                return checkUpperBound(key) && checkLowerBound(key);
            }

            internal TreeMap<K, V>.Entry<K, V> theSmallestEntry()
            {
                TreeMap<K, V>.Entry<K, V> result = null;
                if (!fromStart)
                {
                    result = m.findSmallestEntry();
                }
                else
                {
                    result = loInclusive ? m.findCeilingEntry(lo)
                            : m.findHigherEntry(lo);
                }
                return (null != result && (!toEnd || checkUpperBound(result.getKey()))) ? result
                        : null;
            }

            internal TreeMap<K, V>.Entry<K, V> theBiggestEntry()
            {
                TreeMap<K, V>.Entry<K, V> result = null;
                if (!toEnd)
                {
                    result = m.findBiggestEntry();
                }
                else
                {
                    result = hiInclusive ? m.findFloorEntry(hi)
                            : m.findLowerEntry(hi);
                }
                return (null != result && (!fromStart || checkLowerBound(result.getKey()))) ? result
                        : null;
            }

            internal TreeMap<K, V>.Entry<K, V> smallerOrEqualEntry(K key)
            {
                TreeMap<K, V>.Entry<K, V> result = findFloorEntry(key);
                return (null != result && (!fromStart || checkLowerBound(result.getKey()))) ? result
                        : null;
            }

            internal TreeMap<K, V>.Entry<K, V> findFloorEntry(K key)
            {
                TreeMap<K, V>.Entry<K, V> node = findFloorEntryImpl(key);

                if (node == null)
                {
                    return null;
                }

                if (!checkUpperBound(node.key))
                {
                    node = findEndNode();
                }

                if (node != null && fromStart && !checkLowerBound(node.key))
                {
                    java.lang.Comparable<K> obj = m.comparatorJ == null ? toComparable((K)key)
                            : null;
                    if (cmp(obj, key, this.lo) > 0)
                    {
                        node = findStartNode();
                        if (node == null || cmp(obj, key, node.key) < 0)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        node = null;
                    }
                }
                return node;
            }

            internal int cmp(java.lang.Comparable<K> obj, K key1, K key2)
            {
                return obj != null ? obj.compareTo(key2) : comparator().compare(
                        key1, key2);
            }

            internal TreeMap<K, V>.Entry<K, V> findFloorEntryImpl(K key)
            {
                java.lang.Comparable<K> obj = comparator() == null ? toComparable((K)key)
                        : null;
                K keyK = (K)key;
                TreeMap<K,V>.Node<K, V> node = this.m.root;
                TreeMap<K,V>.Node<K, V> foundNode = null;
                int foundIndex = 0;
                while (node != null)
                {
                    K[] keys = node.keys;
                    int left_idx = node.left_idx;
                    int result = obj != null ? obj.compareTo(keys[left_idx]) : -comparator().compare(
                            keys[left_idx], keyK);
                    if (result < 0)
                    {
                        node = node.left;
                    }
                    else
                    {
                        foundNode = node;
                        foundIndex = left_idx;
                        if (result == 0)
                        {
                            break;
                        }
                        int right_idx = node.right_idx;
                        if (left_idx != right_idx)
                        {
                            result = cmp(obj, key, keys[right_idx]);
                        }
                        if (result >= 0)
                        {
                            foundNode = node;
                            foundIndex = right_idx;
                            if (result == 0)
                            {
                                break;
                            }
                            node = node.right;
                        }
                        else
                        { /* search in node */
                            int low = left_idx + 1, mid = 0, high = right_idx - 1;
                            while (low <= high && result != 0)
                            {
                                mid = (low + high) >> 1;
                                result = cmp(obj, key, keys[mid]);
                                if (result >= 0)
                                {
                                    foundNode = node;
                                    foundIndex = mid;
                                    low = mid + 1;
                                }
                                else
                                {
                                    high = mid;
                                }
                                if (low == high && high == mid)
                                {
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                if (foundNode != null
                        && cmp(obj, keyK, foundNode.keys[foundIndex]) < 0)
                {
                    foundNode = null;
                }
                if (foundNode != null)
                {
                    return createEntry(foundNode, foundIndex);
                }
                return null;
            }

            TreeMap<K, V>.Entry<K, V> createEntry(TreeMap<K,V>.Node<K,V> node, int index)
            {
                TreeMap<K, V>.Entry<K, V> entry = new TreeMap<K, V>.Entry<K, V>(node.keys[index], node.values[index]);
                entry.node = node;
                entry.index = index;
                return entry;
            }

            internal TreeMap<K, V>.Entry<K, V> biggerOrEqualEntry(K key)
            {
                TreeMap<K, V>.Entry<K, V> result = findCeilingEntry(key);
                return (null != result && (!toEnd || checkUpperBound(result.getKey()))) ? result
                        : null;
            }

            // find the node whose key equals startKey if any, or the next bigger
            // one than startKey if start exclusive
            internal TreeMap<K, V>.Entry<K, V> findStartNode()
            {
                if (fromStart)
                {
                    if (loInclusive)
                    {
                        return m.findCeilingEntry(lo);
                    }
                    else
                    {
                        return m.findHigherEntry(lo);
                    }
                }
                else
                {
                    return theSmallestEntry();
                }
            }

            // find the node whose key equals endKey if any, or the next smaller
            // one than endKey if end exclusive
            internal TreeMap<K, V>.Entry<K, V> findEndNode()
            {
                if (hiInclusive)
                {
                    return findFloorEntryImpl(hi);
                }
                else
                {
                    return findLowerEntryImpl(hi);
                }
            }

            internal TreeMap<K, V>.Entry<K, V> findCeilingEntry(K key)
            {
                TreeMap<K, V>.Entry<K, V> node = findCeilingEntryImpl(key);

                if (null == node)
                {
                    return null;
                }

                if (toEnd && !checkUpperBound(node.key))
                {
                    java.lang.Comparable<K> obj = m.comparatorJ == null ? toComparable((K)key)
                            : null;
                    if (cmp(obj, key, this.hi) < 0)
                    {
                        node = findEndNode();
                        if (node != null && cmp(obj, key, node.key) > 0)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }

                if (node != null && !checkLowerBound(node.key))
                {
                    node = findStartNode();
                }

                return node;
            }

            internal TreeMap<K, V>.Entry<K, V> findLowerEntryImpl(K key)
            {
                java.lang.Comparable<K> obj = comparator() == null ? toComparable((K)key)
                        : null;
                K keyK = (K)key;
                TreeMap<K,V>.Node<K, V> node = m.root;
                TreeMap<K,V>.Node<K, V> foundNode = null;
                int foundIndex = 0;
                while (node != null)
                {
                    K[] keys = node.keys;
                    int left_idx = node.left_idx;
                    int result = obj != null ? obj.compareTo(keys[left_idx]) : -comparator().compare(
                            keys[left_idx], keyK);
                    if (result <= 0)
                    {
                        node = node.left;
                    }
                    else
                    {
                        foundNode = node;
                        foundIndex = left_idx;
                        int right_idx = node.right_idx;
                        if (left_idx != right_idx)
                        {
                            result = cmp(obj, key, keys[right_idx]);
                        }
                        if (result > 0)
                        {
                            foundNode = node;
                            foundIndex = right_idx;
                            node = node.right;
                        }
                        else
                        { /* search in node */
                            int low = left_idx + 1, mid = 0, high = right_idx - 1;
                            while (low <= high)
                            {
                                mid = (low + high) >> 1;
                                result = cmp(obj, key, keys[mid]);
                                if (result > 0)
                                {
                                    foundNode = node;
                                    foundIndex = mid;
                                    low = mid + 1;
                                }
                                else
                                {
                                    high = mid;
                                }
                                if (low == high && high == mid)
                                {
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                if (foundNode != null
                        && cmp(obj, keyK, foundNode.keys[foundIndex]) <= 0)
                {
                    foundNode = null;
                }
                if (foundNode != null)
                {
                    return createEntry(foundNode, foundIndex);
                }
                return null;
            }

            internal TreeMap<K, V>.Entry<K, V> findCeilingEntryImpl(K key)
            {
                java.lang.Comparable<K> obj = comparator() == null ? toComparable((K)key)
                        : null;
                K keyK = (K)key;
                TreeMap<K,V>.Node<K, V> node = m.root;
                TreeMap<K,V>.Node<K, V> foundNode = null;
                int foundIndex = 0;
                while (node != null)
                {
                    K[] keys = node.keys;
                    int left_idx = node.left_idx;
                    int right_idx = node.right_idx;
                    int result = cmp(obj, keyK, keys[left_idx]);
                    if (result < 0)
                    {
                        foundNode = node;
                        foundIndex = left_idx;
                        node = node.left;
                    }
                    else if (result == 0)
                    {
                        foundNode = node;
                        foundIndex = left_idx;
                        break;
                    }
                    else
                    {
                        if (left_idx != right_idx)
                        {
                            result = cmp(obj, key, keys[right_idx]);
                        }
                        if (result > 0)
                        {
                            node = node.right;
                        }
                        else
                        { /* search in node */
                            foundNode = node;
                            foundIndex = right_idx;
                            if (result == 0)
                            {
                                break;
                            }
                            int low = left_idx + 1, mid = 0, high = right_idx - 1;
                            while (low <= high && result != 0)
                            {
                                mid = (low + high) >> 1;
                                result = cmp(obj, key, keys[mid]);
                                if (result <= 0)
                                {
                                    foundNode = node;
                                    foundIndex = mid;
                                    high = mid - 1;
                                }
                                else
                                {
                                    low = mid + 1;
                                }
                                if (result == 0 || (low == high && high == mid))
                                {
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                if (foundNode != null
                        && cmp(obj, keyK, foundNode.keys[foundIndex]) > 0)
                {
                    foundNode = null;
                }
                if (foundNode != null)
                {
                    return createEntry(foundNode, foundIndex);
                }
                return null;
            }

            internal TreeMap<K, V>.Entry<K, V> smallerEntry(K key)
            {
                TreeMap<K, V>.Entry<K, V> result = findLowerEntry(key);
                return (null != result && (!fromStart || checkLowerBound(result.getKey()))) ? result
                        : null;
            }

            internal TreeMap<K, V>.Entry<K, V> findLowerEntry(K key)
            {
                TreeMap<K, V>.Entry<K, V> node = findLowerEntryImpl(key);

                if (null == node)
                {
                    return null;
                }

                if (!checkUpperBound(node.key))
                {
                    node = findEndNode();
                }

                if (fromStart && !checkLowerBound(node.key))
                {
                    java.lang.Comparable<K> obj = m.comparatorJ == null ? toComparable((K)key)
                            : null;
                    if (cmp(obj, key, this.lo) > 0)
                    {
                        node = findStartNode();
                        if (node == null || cmp(obj, key, node.key) <= 0)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        node = null;
                    }
                }

                return node;
            }

            internal TreeMap<K, V>.Entry<K, V> biggerEntry(K key)
            {
                TreeMap<K, V>.Entry<K, V> result = findHigherEntry(key);
                return (null != result && (!toEnd || checkUpperBound(result.getKey()))) ? result
                        : null;
            }

            internal TreeMap<K, V>.Entry<K, V> findHigherEntry(K key)
            {
                TreeMap<K, V>.Entry<K, V> node = findHigherEntryImpl(key);

                if (node == null)
                {
                    return null;
                }

                if (toEnd && !checkUpperBound(node.key))
                {
                    java.lang.Comparable<K> obj = m.comparatorJ == null ? toComparable((K)key)
                            : null;
                    if (cmp(obj, key, this.hi) < 0)
                    {
                        node = findEndNode();
                        if (node != null && cmp(obj, key, node.key) >= 0)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }

                if (node != null && !checkLowerBound(node.key))
                {
                    node = findStartNode();
                }

                return node;
            }

            internal TreeMap<K, V>.Entry<K, V> findHigherEntryImpl(K key)
            {
                java.lang.Comparable<K> obj = m.comparatorJ == null ? toComparable((K)key)
                        : null;
                K keyK = (K)key;
                TreeMap<K,V>.Node<K, V> node = m.root;
                TreeMap<K,V>.Node<K, V> foundNode = null;
                int foundIndex = 0;
                while (node != null)
                {
                    K[] keys = node.keys;
                    int right_idx = node.right_idx;
                    int result = cmp(obj, keyK, keys[right_idx]);
                    if (result >= 0)
                    {
                        node = node.right;
                    }
                    else
                    {
                        foundNode = node;
                        foundIndex = right_idx;
                        int left_idx = node.left_idx;
                        if (left_idx != right_idx)
                        {
                            result = cmp(obj, key, keys[left_idx]);
                        }
                        if (result < 0)
                        {
                            foundNode = node;
                            foundIndex = left_idx;
                            node = node.left;
                        }
                        else
                        { /* search in node */
                            foundNode = node;
                            foundIndex = right_idx;
                            int low = left_idx + 1, mid = 0, high = right_idx - 1;
                            while (low <= high)
                            {
                                mid = (low + high) >> 1;
                                result = cmp(obj, key, keys[mid]);
                                if (result < 0)
                                {
                                    foundNode = node;
                                    foundIndex = mid;
                                    high = mid - 1;
                                }
                                else
                                {
                                    low = mid + 1;
                                }
                                if (low == high && high == mid)
                                {
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                if (foundNode != null
                        && cmp(obj, keyK, foundNode.keys[foundIndex]) >= 0)
                {
                    foundNode = null;
                }
                if (foundNode != null)
                {
                    return createEntry(foundNode, foundIndex);
                }
                return null;
            }

            public override Collection<V> values()
            {
                if (valuesCollection == null)
                {
                    if (!this.toEnd && !this.fromStart)
                    {
                        valuesCollection = base.values();
                    }
                    else
                    {
                        MapNS.Entry<K, V> startEntry;
                        if (loInclusive)
                        {
                            startEntry = fromStart ? m.ceilingEntry(this.lo) : theSmallestEntry();
                        }
                        else
                        {
                            startEntry = fromStart ? m.findHigherEntry(this.lo) : theSmallestEntry();
                        }
                        if (startEntry == null)
                        {
                            K key = m.isEmpty() ? this.lo : m.firstKey();
                            valuesCollection = new SubMapValuesCollection<K, V>(
                                    new SubMap<K, V>(key, true, this.m, key, true));
                            return valuesCollection;
                        }
                        // for submap, the lastKey is always exclusive, so should take care
                        MapNS.Entry<K, V> lastEntry;
                        lastEntry = toEnd ? m.ceilingEntry(this.hi) : null;
                        if (lastEntry != null)
                        {
                            if (hiInclusive && lastEntry.getKey().equals(this.hi))
                            {
                                lastEntry = m.higherEntry(this.hi);
                            }
                        }

                        K startK = startEntry == null ? default(K) : startEntry.getKey();
                        K lastK = lastEntry == null ? default(K) : lastEntry.getKey();
                        // submap always exclude the highest entry
                        valuesCollection = new SubMapValuesCollection<K, V>(
                                    new SubMap<K, V>(startK, true, this.m, lastK, lastK == null ? false : toEnd));
                    }
                }
                return valuesCollection;
            }
        }

        class NullSubMapValuesCollection<K, V> : SubMapValuesCollection<K, V>
        {
            SubMap<K, V> subMap;

            public NullSubMapValuesCollection(SubMap<K, V> subMap)
                : base(subMap)
            {
            }


            public override bool isEmpty()
            {
                return true;
            }


            public override Iterator<V> iterator()
            {
                return new NullSubMapValuesCollectionIterator<V>();
            }


            public override int size()
            {
                return 0;
            }
        }

        private class NullSubMapValuesCollectionIterator<V> : java.util.Iterator<V>
        {
            public NullSubMapValuesCollectionIterator()
            {
            }

            public bool hasNext()
            {
                return false;
            }

            public V next()
            {
                throw new NoSuchElementException();
            }

            public void remove()
            {
                throw new java.lang.IllegalStateException();
            }
        }

        [Serializable]
        class AscendingSubMap<K, V> : NavigableSubMap<K, V>,
                 java.io.Serializable
        {

            private static readonly long serialVersionUID = 912986545866124060L;

            internal AscendingSubMap(K start, bool startKeyInclusive, TreeMap<K, V> map,
                    K end, bool endKeyInclusive)
                : base(start, startKeyInclusive, map, end, endKeyInclusive)
            {
            }

            internal AscendingSubMap(TreeMap<K, V> map, K end, bool endKeyInclusive)
                : base(map, end, endKeyInclusive)
            {
            }

            internal AscendingSubMap(K start, bool startKeyInclusive, TreeMap<K, V> map)
                : base(start, startKeyInclusive, map)
            {
            }

            internal AscendingSubMap(TreeMap<K, V> map)
                : base(map)
            {
            }


            public override MapNS.Entry<K, V> firstEntry()
            {
                TreeMap<K, V>.Entry<K, V> ret = theSmallestEntry();
                if (ret != null)
                {
                    return m.newImmutableEntry(ret);
                }
                else
                {
                    return null;
                }
            }


            public override MapNS.Entry<K, V> lastEntry()
            {
                TreeMap<K, V>.Entry<K, V> ret = theBiggestEntry();
                if (ret != null)
                {
                    return m.newImmutableEntry(ret);
                }
                else
                {
                    return null;
                }
            }


            public override MapNS.Entry<K, V> pollFirstEntry()
            {
                TreeMap<K, V>.Entry<K, V> node = theSmallestEntry();
                SimpleImmutableEntry<K, V> result = m
                        .newImmutableEntry(node);
                if (null != node)
                {
                    m.remove(node.key);
                }
                return result;
            }


            public override MapNS.Entry<K, V> pollLastEntry()
            {
                TreeMap<K, V>.Entry<K, V> node = theBiggestEntry();
                SimpleImmutableEntry<K, V> result = m
                        .newImmutableEntry(node);
                if (null != node)
                {
                    m.remove(node.key);
                }
                return result;
            }


            public override MapNS.Entry<K, V> higherEntry(K key)
            {
                TreeMap<K, V>.Entry<K, V> entry = base.findHigherEntry(key);
                if (null != entry && isInRange(entry.key))
                {
                    return m.newImmutableEntry(entry);
                }
                else
                {
                    return null;
                }
            }


            public override MapNS.Entry<K, V> lowerEntry(K key)
            {
                TreeMap<K, V>.Entry<K, V> entry = base.findLowerEntry(key);
                if (null != entry && isInRange(entry.key))
                {
                    return m.newImmutableEntry(entry);
                }
                else
                {
                    return null;
                }
            }

            public override MapNS.Entry<K, V> ceilingEntry(K key)
            {
                TreeMap<K, V>.Entry<K, V> entry = base.findCeilingEntry(key);
                if (null != entry && isInRange(entry.key))
                {
                    return m.newImmutableEntry(entry);
                }
                else
                {
                    return null;
                }
            }

            public override MapNS.Entry<K, V> floorEntry(K key)
            {
                TreeMap<K, V>.Entry<K, V> entry = base.findFloorEntry(key);
                if (null != entry && isInRange(entry.key))
                {
                    return m.newImmutableEntry(entry);
                }
                else
                {
                    return null;
                }
            }

            public override Set<MapNS.Entry<K, V>> entrySet()
            {
                return new AscendingSubMapEntrySet<K, V>(this);
            }



            public override NavigableSet<K> navigableKeySet()
            {
                return (NavigableSet<K>)new AscendingSubMapKeySet<K, V>(this);
            }

            public override NavigableMap<K, V> descendingMap()
            {
                if (fromStart && toEnd)
                {
                    return new DescendingSubMap<K, V>(hi, hiInclusive,
                            m, lo, loInclusive);
                }
                if (fromStart)
                {
                    return new DescendingSubMap<K, V>(m, lo, loInclusive);
                }
                if (toEnd)
                {
                    return new DescendingSubMap<K, V>(hi, hiInclusive, m);
                }
                return new DescendingSubMap<K, V>(m);
            }

            protected internal override NavigableSubMap<K, V> descendingSubMap()
            {
                if (fromStart && toEnd)
                {
                    return new DescendingSubMap<K, V>(hi, hiInclusive,
                            m, lo, loInclusive);
                }
                if (fromStart)
                {
                    return new DescendingSubMap<K, V>(m, lo, loInclusive);
                }
                if (toEnd)
                {
                    return new DescendingSubMap<K, V>(hi, hiInclusive, m);
                }
                return new DescendingSubMap<K, V>(m);
            }


            public override NavigableMap<K, V> subMap(K start, bool startKeyInclusive,
                    K end, bool endKeyInclusive)
            {
                if (fromStart
                        && ((!loInclusive && startKeyInclusive) ? m.keyCompare(start,
                                lo) <= 0 : m.keyCompare(start, lo) < 0)
                        || (toEnd && ((!hiInclusive && (endKeyInclusive || (startKeyInclusive && start.equals(end)))) ? m.keyCompare(
                                end, hi) >= 0
                                : m.keyCompare(end, hi) > 0)))
                {
                    throw new java.lang.IllegalArgumentException();
                }
                if (m.keyCompare(start, end) > 0)
                {
                    throw new java.lang.IllegalArgumentException();
                }
                return new AscendingSubMap<K, V>(start, startKeyInclusive, m, end,
                        endKeyInclusive);
            }


            public override NavigableMap<K, V> headMap(K end, bool inclusive)
            {
                if (fromStart && ((!loInclusive && inclusive) ? m.keyCompare(end, lo) <= 0 : m.keyCompare(end, lo) < 0))
                {
                    throw new java.lang.IllegalArgumentException();
                }
                if (toEnd && ((!hiInclusive && inclusive) ? m.keyCompare(end, hi) >= 0 : m.keyCompare(end, hi) > 0))
                {
                    throw new java.lang.IllegalArgumentException();
                }
                if (checkUpperBound(end))
                {
                    if (this.fromStart)
                    {
                        return new AscendingSubMap<K, V>(this.lo,
                                this.loInclusive, m, end, inclusive);
                    }
                    return new AscendingSubMap<K, V>(m, end, inclusive);
                }
                else
                {
                    return this;
                }
            }


            public override NavigableMap<K, V> tailMap(K start, bool inclusive)
            {
                if (fromStart && ((!loInclusive && inclusive) ? m.keyCompare(start, lo) <= 0 : m.keyCompare(start, lo) < 0))
                {
                    throw new java.lang.IllegalArgumentException();
                }
                if (toEnd && ((!hiInclusive && inclusive) ? m.keyCompare(start, hi) >= 0 : m.keyCompare(start, hi) > 0))
                {
                    throw new java.lang.IllegalArgumentException();
                }
                if (checkLowerBound(start))
                {
                    if (this.toEnd)
                    {
                        return new AscendingSubMap<K, V>(start, inclusive,
                                m, this.hi, this.hiInclusive);
                    }
                    return new AscendingSubMap<K, V>(start, inclusive, m);
                }
                else
                {
                    return this;
                }
            }
        }

        [Serializable]
        class DescendingSubMap<K, V> : NavigableSubMap<K, V>,
                 java.io.Serializable
        {
            private static readonly long serialVersionUID = 912986545866120460L;

            private readonly Comparator<K> reverseComparator;

            internal DescendingSubMap(K start, bool startKeyInclusive, TreeMap<K, V> map,
                    K end, bool endKeyInclusive)
                : base(start, startKeyInclusive, map, end, endKeyInclusive)
            {
                reverseComparator = Collections.reverseOrder(m.comparatorJ);
            }

            internal DescendingSubMap(K start, bool startKeyInclusive, TreeMap<K, V> map)
                : base(start, startKeyInclusive, map)
            {
                reverseComparator = Collections.reverseOrder(m.comparatorJ);
            }

            internal DescendingSubMap(TreeMap<K, V> map, K end, bool endKeyInclusive)
                : base(map, end, endKeyInclusive)
            {
                reverseComparator = Collections.reverseOrder(m.comparatorJ);
            }

            internal DescendingSubMap(TreeMap<K, V> map)
                : base(map)
            {
                reverseComparator = Collections.reverseOrder(m.comparatorJ);
            }


            public override Comparator<K> comparator()
            {
                return reverseComparator;
            }

            public override SortedMap<K, V> subMap(K start, K end)
            {
                // the exception check is special here, the end should not if equal
                // to endkey unless start is equal to end
                if (!checkLowerBound(start) || !checkUpperBound(start))
                {
                    throw new java.lang.IllegalArgumentException();
                }
                int result = -1;
                if (toEnd)
                {
                    result = (null != comparator()) ? comparator().compare(end, hi)
                            : toComparable(end).compareTo(hi);
                }
                if (((!hiInclusive && start.equals(end)) ? result < 0 : result <= 0))
                {
                    if ((null != comparator()) ? comparator().compare(start, end) > 0
                            : toComparable(start).compareTo(end) > 0)
                    {
                        throw new java.lang.IllegalArgumentException();
                    }
                    return new DescendingSubMap<K, V>(start, true, m, end,
                            false);
                }
                throw new java.lang.IllegalArgumentException();
            }


            public override MapNS.Entry<K, V> firstEntry()
            {
                TreeMap<K, V>.Entry<K, V> result;
                if (!fromStart)
                {
                    result = m.findBiggestEntry();
                }
                else
                {
                    result = loInclusive ? m.findFloorEntry(lo)
                            : m.findLowerEntry(lo);
                }
                if (result == null || !isInRange(result.key))
                {
                    return null;
                }
                return m.newImmutableEntry(result);
            }


            public override MapNS.Entry<K, V> lastEntry()
            {
                TreeMap<K, V>.Entry<K, V> result;
                if (!toEnd)
                {
                    result = m.findSmallestEntry();
                }
                else
                {
                    result = hiInclusive ? m.findCeilingEntry(hi) : m
                            .findHigherEntry(hi);
                }
                if (result != null && !isInRange(result.key))
                {
                    return null;
                }
                return m.newImmutableEntry(result);
            }


            public override MapNS.Entry<K, V> pollFirstEntry()
            {
                TreeMap<K, V>.Entry<K, V> node = null;
                if (fromStart)
                {
                    node = loInclusive ? this.m.findFloorEntry(lo) : this.m
                            .findLowerEntry(lo);
                }
                else
                {
                    node = this.m.findBiggestEntry();
                }
                if (node != null && fromStart && (loInclusive ? this.m.keyCompare(lo, node.key) < 0 : this.m.keyCompare(lo, node.key) <= 0))
                {
                    node = null;
                }
                if (node != null && toEnd && (hiInclusive ? this.m.keyCompare(hi, node.key) > 0 : this.m.keyCompare(hi, node.key) >= 0))
                {
                    node = null;
                }
                SimpleImmutableEntry<K, V> result = m
                        .newImmutableEntry(node);
                if (null != node)
                {
                    m.remove(node.key);
                }
                return result;
            }


            public override MapNS.Entry<K, V> pollLastEntry()
            {
                TreeMap<K, V>.Entry<K, V> node = null;
                if (toEnd)
                {
                    node = hiInclusive ? this.m.findCeilingEntry(hi) : this.m.findHigherEntry(hi);
                }
                else
                {
                    node = this.m.findSmallestEntry();
                }
                if (node != null && fromStart && (loInclusive ? this.m.keyCompare(lo, node.key) < 0 : this.m.keyCompare(lo, node.key) <= 0))
                {
                    node = null;
                }
                if (node != null && toEnd && (hiInclusive ? this.m.keyCompare(hi, node.key) > 0 : this.m.keyCompare(hi, node.key) >= 0))
                {
                    node = null;
                }
                SimpleImmutableEntry<K, V> result = m
                        .newImmutableEntry(node);
                if (null != node)
                {
                    m.remove(node.key);
                }
                return result;
            }


            public override MapNS.Entry<K, V> higherEntry(K key)
            {
                TreeMap<K, V>.Entry<K, V> entry = this.m.findLowerEntry(key);
                if (null != entry && (fromStart && !checkLowerBound(entry.getKey())))
                {
                    entry = loInclusive ? this.m.findFloorEntry(this.lo) : this.m.findLowerEntry(this.lo);
                }
                if (null != entry && (!isInRange(entry.getKey())))
                {
                    entry = null;
                }
                return m.newImmutableEntry(entry);
            }


            public override MapNS.Entry<K, V> lowerEntry(K key)
            {
                TreeMap<K, V>.Entry<K, V> entry = this.m.findHigherEntry(key);
                if (null != entry && (toEnd && !checkUpperBound(entry.getKey())))
                {
                    entry = hiInclusive ? this.m.findCeilingEntry(this.hi) : this.m.findHigherEntry(this.hi);
                }
                if (null != entry && (!isInRange(entry.getKey())))
                {
                    entry = null;
                }
                return m.newImmutableEntry(entry);
            }


            public override MapNS.Entry<K, V> ceilingEntry(K key)
            {
                java.lang.Comparable<K> obj = m.comparatorJ == null ? toComparable((K)key)
                        : null;
                TreeMap<K, V>.Entry<K, V> entry = null;
                if (toEnd && m.cmp(obj, key, lo) >= 0)
                {
                    entry = loInclusive ? this.m.findFloorEntry(lo) : this.m.findLowerEntry(lo);
                }
                else
                {
                    entry = this.m.findFloorEntry(key);
                }
                if (null != entry && (toEnd && !checkUpperBound(entry.getKey())))
                {
                    entry = null;
                }
                return m.newImmutableEntry(entry);
            }


            public override MapNS.Entry<K, V> floorEntry(K key)
            {
                java.lang.Comparable<K> obj = m.comparatorJ == null ? toComparable((K)key)
                        : null;
                TreeMap<K, V>.Entry<K, V> entry = null;
                if (fromStart && m.cmp(obj, key, hi) <= 0)
                {
                    entry = hiInclusive ? this.m.findCeilingEntry(hi) : this.m.findHigherEntry(hi);
                }
                else
                {
                    entry = this.m.findCeilingEntry(key);
                }
                if (null != entry && (fromStart && !checkLowerBound(entry.getKey())))
                {
                    entry = null;
                }
                return m.newImmutableEntry(entry);
            }


            public override Set<MapNS.Entry<K, V>> entrySet()
            {
                return new DescendingSubMapEntrySet<K, V>(this);
            }



            public override NavigableSet<K> navigableKeySet()
            {
                return (NavigableSet<K>)new DescendingSubMapKeySet<K, V>(this);
            }

            public override NavigableMap<K, V> descendingMap()
            {
                if (fromStart && toEnd)
                {
                    return new AscendingSubMap<K, V>(hi, hiInclusive,
                            m, lo, loInclusive);
                }
                if (fromStart)
                {
                    return new AscendingSubMap<K, V>(m, lo, loInclusive);
                }
                if (toEnd)
                {
                    return new AscendingSubMap<K, V>(hi, hiInclusive,
                            m);
                }
                return new AscendingSubMap<K, V>(m);
            }

            int keyCompare(K left, K right)
            {
                return (null != reverseComparator) ? reverseComparator.compare(
                        left, right) : toComparable(left).compareTo(right);
            }


            public override NavigableMap<K, V> subMap(K start, bool startKeyInclusive,
                    K end, bool endKeyInclusive)
            {
                // special judgement, the same reason as subMap(K,K)
                if (!checkUpperBound(start))
                {
                    throw new java.lang.IllegalArgumentException();
                }
                if (fromStart
                        && ((!loInclusive && (startKeyInclusive || (endKeyInclusive && start.equals(end)))) ? keyCompare(start,
                                lo) <= 0 : keyCompare(start, lo) < 0)
                        || (toEnd && ((!hiInclusive && (endKeyInclusive)) ? keyCompare(
                                end, hi) >= 0
                                : keyCompare(end, hi) > 0)))
                {
                    throw new java.lang.IllegalArgumentException();
                }
                if (keyCompare(start, end) > 0)
                {
                    throw new java.lang.IllegalArgumentException();
                }
                return new DescendingSubMap<K, V>(start, startKeyInclusive, m, end,
                        endKeyInclusive);
            }


            public override NavigableMap<K, V> headMap(K end, bool inclusive)
            {
                // check for error
                this.keyCompare(end, end);
                K inclusiveEnd = end; //inclusive ? end : m.higherKey(end);
                bool isInRange = true;
                if (null != inclusiveEnd)
                {
                    int result;
                    if (toEnd)
                    {
                        result = (null != comparator()) ? comparator().compare(
                                inclusiveEnd, hi) : toComparable(inclusiveEnd)
                                .compareTo(hi);
                        isInRange = (hiInclusive || !inclusive) ? result <= 0 : result < 0;
                    }
                    if (fromStart)
                    {
                        result = (null != comparator()) ? comparator().compare(
                                inclusiveEnd, lo) : toComparable(inclusiveEnd)
                                .compareTo(lo);
                        isInRange = isInRange
                                && ((loInclusive || !inclusive) ? result >= 0 : result > 0);
                    }
                }
                if (isInRange)
                {
                    if (this.fromStart)
                    {
                        return new DescendingSubMap<K, V>(this.lo, this.loInclusive,
                                m, end, inclusive);
                    }
                    return new DescendingSubMap<K, V>(m, end, inclusive);
                }
                throw new java.lang.IllegalArgumentException();
            }


            public override NavigableMap<K, V> tailMap(K start, bool inclusive)
            {
                // check for error
                this.keyCompare(start, start);
                K inclusiveStart = start; // inclusive ? start : m.lowerKey(start);
                bool isInRange = true;
                int result;
                if (null != inclusiveStart)
                {
                    if (toEnd)
                    {
                        result = (null != comparator()) ? comparator().compare(
                                inclusiveStart, hi) : toComparable(inclusiveStart)
                                .compareTo(hi);
                        isInRange = (hiInclusive || !inclusive) ? result <= 0 : result < 0;
                    }
                    if (fromStart)
                    {
                        result = (null != comparator()) ? comparator().compare(
                                inclusiveStart, lo) : toComparable(inclusiveStart)
                                .compareTo(lo);
                        isInRange = isInRange
                                && ((loInclusive || !inclusive) ? result >= 0 : result > 0);
                    }
                }
                if (isInRange)
                {
                    if (this.toEnd)
                    {
                        return new DescendingSubMap<K, V>(start, inclusive, m, this.hi, this.hiInclusive);
                    }
                    return new DescendingSubMap<K, V>(start, inclusive, m);

                }
                throw new java.lang.IllegalArgumentException();
            }

            public override Collection<V> values()
            {
                if (valuesCollection == null)
                {
                    if (fromStart || toEnd)
                    {
                        return valuesCollection = new DescendingSubMapValuesCollection<K, V>(this);
                    }
                    valuesCollection = base.values();
                }
                return valuesCollection;
            }
            class DescendingSubMapValuesCollection<K, V> : AbstractCollection<V>
            {
                DescendingSubMap<K, V> subMap;

                public DescendingSubMapValuesCollection(DescendingSubMap<K, V> subMap)
                {
                    this.subMap = subMap;
                }


                public override bool isEmpty()
                {
                    return subMap.isEmpty();
                }


                public override Iterator<V> iterator()
                {
                    TreeMap<K, V>.Entry<K, V> from = subMap.m.find(subMap.firstKey());
                    TreeMap<K, V>.Entry<K, V> to = subMap.m.findLowerEntry(subMap.lastKey());
                    return new DescendingValueIterator<K, V>(from.node,
                            from == null ? 0 : from.index, subMap, to == null ? null : to.node,
                            to == null ? 0 : to.index);
                }

                class DescendingValueIterator<K, V> : BoundedMapIterator<K, V>
                 , Iterator<V>
                {

                    public DescendingValueIterator(TreeMap<K,V>.Node<K,V> startNode, int startOffset,
                            DescendingSubMap<K, V> map, TreeMap<K,V>.Node<K, V> finalNode, int finalOffset) :
                        base(startNode, startOffset, map.m, finalNode, finalOffset)
                    {
                        node = startNode;
                        offset = startOffset;
                    }

                    public V next()
                    {
                        if (!hasNext())
                        {
                            throw new NoSuchElementException();
                        }
                        if (node != null)
                        {
                            bool endOfIterator = lastNode == finalNode && lastOffset == finalOffset;
                            if (endOfIterator)
                            {
                                node = null;
                            }
                            else
                            {
                                if (expectedModCount != backingMap.modCount)
                                {
                                    throw new ConcurrentModificationException();
                                }
                                else if (node == null)
                                {
                                    throw new NoSuchElementException();
                                }
                                lastNode = node;
                                lastOffset = offset;
                                if (offset != node.left_idx)
                                {
                                    offset--;
                                }
                                else
                                {
                                    node = node.prev;
                                    if (node != null)
                                    {
                                        offset = node.right_idx;
                                    }
                                }
                            }
                        }
                        return lastNode.values[lastOffset];
                    }
                }


                public override int size()
                {
                    return subMap.size();
                }
            }


            protected internal override NavigableSubMap<K, V> descendingSubMap()
            {
                if (fromStart && toEnd)
                {
                    return new AscendingSubMap<K, V>(hi, hiInclusive,
                            m, lo, loInclusive);
                }
                if (fromStart)
                {
                    return new AscendingSubMap<K, V>(m, hi, hiInclusive);
                }
                if (toEnd)
                {
                    return new AscendingSubMap<K, V>(lo, loInclusive,
                            m);
                }
                return new AscendingSubMap<K, V>(m);
            }
        }

        /**
         * Constructs a new empty {@code TreeMap} instance.
         */
        public TreeMap()
            : base()
        {

        }

        /**
         * Constructs a new empty {@code TreeMap} instance with the specified
         * comparator.
         *
         * @param comparator
         *            the comparator to compare keys with.
         */
        public TreeMap(Comparator<K> comparator)
        {
            this.comparatorJ = comparator;
        }

        /**
         * Constructs a new {@code TreeMap} instance containing the mappings from
         * the specified map and using natural ordering.
         *
         * @param map
         *            the mappings to add.
         * @throws ClassCastException
         *             if a key in the specified map does not implement the
         *             Comparable interface, or if the keys in the map cannot be
         *             compared.
         */
        public TreeMap(Map<K, V> map)
            : this()
        {
            putAll(map);
        }

        /**
         * Constructs a new {@code TreeMap} instance containing the mappings from
         * the specified SortedMap and using the same comparator.
         *
         * @param map
         *            the mappings to add.
         */
        public TreeMap(SortedMap<K, V> map)
            : this(map.comparator())
        {

            TreeMap<K,V>.Node<K, V> lastNode = null;
            Iterator<MapNS.Entry<K, V>> it = map.entrySet()
                    .iterator();
            while (it.hasNext())
            {
                MapNS.Entry<K, V> entry = it.next();
                lastNode = addToLast(lastNode, entry.getKey(), entry.getValue());
            }
        }

        internal TreeMap<K,V>.Node<K, V> addToLast(TreeMap<K,V>.Node<K,V> last, K key, V value)
        {
            if (last == null)
            {
                root = last = createNode(key, value);
                sizeJ = 1;
            }
            else if (last.size == TreeMap<K,V>.Node<K, V>.NODE_SIZE)
            {
                TreeMap<K,V>.Node<K, V> newNode = createNode(key, value);
                attachToRight(last, newNode);
                balance(newNode);
                sizeJ++;
                last = newNode;
            }
            else
            {
                appendFromRight(last, key, value);
                sizeJ++;
            }
            return last;
        }

        /**
         * Removes all mappings from this TreeMap, leaving it empty.
         *
         * @see Map#isEmpty()
         * @see #size()
         */

        public override void clear()
        {
            root = null;
            sizeJ = 0;
            modCount++;
        }

        /**
         * Returns a new {@code TreeMap} with the same mappings, size and comparator
         * as this instance.
         *
         * @return a shallow copy of this instance.
         * @see java.lang.Cloneable
         */


        public virtual Object clone()
        {
            try
            {
                TreeMap<K, V> clone = (TreeMap<K, V>)base.MemberwiseClone();
                clone.entrySetJ = null;
                if (root != null)
                {
                    clone.root = root.clone(null);
                    // restore prev/next chain
                    TreeMap<K,V>.Node<K, V> node = minimum(clone.root);
                    while (true)
                    {
                        TreeMap<K,V>.Node<K, V> nxt = successor(node);
                        if (nxt == null)
                        {
                            break;
                        }
                        nxt.prev = node;
                        node.next = nxt;
                        node = nxt;
                    }
                }
                return clone;
            }
            catch (java.lang.CloneNotSupportedException e)
            {
                return null;
            }
        }

        /**
         * Returns the comparator used to compare elements in this map.
         *
         * @return the comparator or {@code null} if the natural ordering is used.
         */
        public virtual Comparator<K> comparator()
        {
            return comparatorJ;
        }

        /**
         * Returns whether this map contains the specified key.
         *
         * @param key
         *            the key to search for.
         * @return {@code true} if this map contains the specified key,
         *         {@code false} otherwise.
         * @throws ClassCastException
         *             if the specified key cannot be compared with the keys in this
         *             map.
         * @throws NullPointerException
         *             if the specified key is {@code null} and the comparator
         *             cannot handle {@code null} keys.
         */


        public override bool containsKey(Object key)
        {
            java.lang.Comparable<K> obj = comparatorJ == null ? toComparable((K)key)
                    : null;
            K keyK = (K)key;
            TreeMap<K,V>.Node<K, V> node = root;
            while (node != null)
            {
                K[] keys = node.keys;
                int left_idx = node.left_idx;
                int result = obj != null ? obj.compareTo(keys[left_idx])
                        : -comparatorJ.compare(keys[left_idx], keyK);
                if (result < 0)
                {
                    node = node.left;
                }
                else if (result == 0)
                {
                    return true;
                }
                else
                {
                    int right_idx = node.right_idx;
                    if (left_idx != right_idx)
                    {
                        result = cmp(obj, keyK, keys[right_idx]);
                    }
                    if (result > 0)
                    {
                        node = node.right;
                    }
                    else if (result == 0)
                    {
                        return true;
                    }
                    else
                    { /* search in node */
                        int low = left_idx + 1, mid = 0, high = right_idx - 1;
                        while (low <= high)
                        {
                            mid = java.dotnet.lang.Operator.shiftRightUnsignet((low + high), 1);
                            result = cmp(obj, keyK, keys[mid]);
                            if (result > 0)
                            {
                                low = mid + 1;
                            }
                            else if (result == 0)
                            {
                                return true;
                            }
                            else
                            {
                                high = mid - 1;
                            }
                        }
                        return false;
                    }
                }
            }
            return false;
        }

        /**
         * Returns whether this map contains the specified value.
         *
         * @param value
         *            the value to search for.
         * @return {@code true} if this map contains the specified value,
         *         {@code false} otherwise.
         */

        public override bool containsValue(Object value)
        {
            if (root == null)
            {
                return false;
            }
            TreeMap<K,V>.Node<K, V> node = minimum(root);
            if (value != null)
            {
                while (node != null)
                {
                    int to = node.right_idx;
                    V[] values = node.values;
                    for (int i = node.left_idx; i <= to; i++)
                    {
                        if (value.equals(values[i]))
                        {
                            return true;
                        }
                    }
                    node = node.next;
                }
            }
            else
            {
                while (node != null)
                {
                    int to = node.right_idx;
                    V[] values = node.values;
                    for (int i = node.left_idx; i <= to; i++)
                    {
                        if (values[i] == null)
                        {
                            return true;
                        }
                    }
                    node = node.next;
                }
            }
            return false;
        }

        private bool containsValue(Entry<K, V> node, Object value)
        {
            if (value == null ? node.value == null : value.equals(node.value))
            {
                return true;
            }
            if (node.left != null)
            {
                if (containsValue(node.left, value))
                {
                    return true;
                }
            }
            if (node.right != null)
            {
                if (containsValue(node.right, value))
                {
                    return true;
                }
            }
            return false;
        }


        TreeMap<K,V>.Entry<K,V> find(Object keyObj)
        {
            java.lang.Comparable<K> obj = comparatorJ == null ? toComparable((K)keyObj)
                    : null;
            K keyK = (K)keyObj;
            TreeMap<K,V>.Node<K, V> node = root;
            while (node != null)
            {
                K[] keys = node.keys;
                int left_idx = node.left_idx;
                int result = cmp(obj, keyK, keys[left_idx]);
                if (result < 0)
                {
                    node = node.left;
                }
                else if (result == 0)
                {
                    return createEntry(node, left_idx);
                }
                else
                {
                    int right_idx = node.right_idx;
                    if (left_idx != right_idx)
                    {
                        result = cmp(obj, keyK, keys[right_idx]);
                    }
                    if (result > 0)
                    {
                        node = node.right;
                    }
                    else if (result == 0)
                    {
                        return createEntry(node, right_idx);
                    }
                    else
                    { /* search in node */
                        int low = left_idx + 1, mid = 0, high = right_idx - 1;
                        while (low <= high)
                        {
                            mid = (low + high) >> 1;
                            result = cmp(obj, keyK, keys[mid]);
                            if (result > 0)
                            {
                                low = mid + 1;
                            }
                            else if (result == 0)
                            {
                                return createEntry(node, mid);
                            }
                            else
                            {
                                high = mid - 1;
                            }
                        }
                        return null;
                    }
                }
            }
            return null;
        }

        TreeMap<K,V>.Entry<K,V> createEntry(TreeMap<K,V>.Node<K,V> node, int index)
        {
            TreeMap<K, V>.Entry<K, V> entry = new TreeMap<K, V>.Entry<K, V>(node.keys[index], node.values[index]);
            entry.node = node;
            entry.index = index;
            return entry;
        }

        TreeMap<K, V>.Entry<K, V> findSmallestEntry()
        {
            if (null != root)
            {
                TreeMap<K,V>.Node<K, V> node = minimum(root);
                TreeMap<K, V>.Entry<K, V> ret = new TreeMap<K, V>.Entry<K, V>(node.keys[node.left_idx], node.values[node.left_idx]);
                ret.node = node;
                ret.index = node.left_idx;
                return ret;
            }
            return null;
        }

        TreeMap<K, V>.Entry<K, V> findBiggestEntry()
        {
            if (null != root)
            {
                TreeMap<K,V>.Node<K, V> node = maximum(root);
                TreeMap<K, V>.Entry<K, V> ret = new TreeMap<K, V>.Entry<K, V>(node.keys[node.right_idx], node.values[node.right_idx]);
                ret.node = node;
                ret.index = node.right_idx;
                return ret;
            }
            return null;
        }

        TreeMap<K, V>.Entry<K, V> findCeilingEntry(K key)
        {
            if (root == null)
            {
                return null;
            }
            java.lang.Comparable<K> obj = comparatorJ == null ? toComparable((K)key)
                    : null;
            K keyK = (K)key;
            TreeMap<K,V>.Node<K, V> node = root;
            TreeMap<K,V>.Node<K, V> foundNode = null;
            int foundIndex = 0;
            while (node != null)
            {
                K[] keys = node.keys;
                int left_idx = node.left_idx;
                int right_idx = node.right_idx;
                int result = cmp(obj, keyK, keys[left_idx]);
                if (result < 0)
                {
                    foundNode = node;
                    foundIndex = left_idx;
                    node = node.left;
                }
                else if (result == 0)
                {
                    foundNode = node;
                    foundIndex = left_idx;
                    break;
                }
                else
                {
                    if (left_idx != right_idx)
                    {
                        result = cmp(obj, key, keys[right_idx]);
                    }
                    if (result > 0)
                    {
                        node = node.right;
                    }
                    else
                    { /* search in node */
                        foundNode = node;
                        foundIndex = right_idx;
                        if (result == 0)
                        {
                            break;
                        }
                        int low = left_idx + 1, mid = 0, high = right_idx - 1;
                        while (low <= high && result != 0)
                        {
                            mid = (low + high) >> 1;
                            result = cmp(obj, key, keys[mid]);
                            if (result <= 0)
                            {
                                foundNode = node;
                                foundIndex = mid;
                                high = mid - 1;
                            }
                            else
                            {
                                low = mid + 1;
                            }
                            if (result == 0 || (low == high && high == mid))
                            {
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            if (foundNode != null
                    && cmp(obj, keyK, foundNode.keys[foundIndex]) > 0)
            {
                foundNode = null;
            }
            if (foundNode != null)
            {
                return createEntry(foundNode, foundIndex);
            }
            return null;
        }

        TreeMap<K, V>.Entry<K, V> findFloorEntry(K key)
        {
            if (root == null)
            {
                return null;
            }
            java.lang.Comparable<K> obj = comparatorJ == null ? toComparable((K)key)
                    : null;
            K keyK = (K)key;
            TreeMap<K,V>.Node<K, V> node = root;
            TreeMap<K,V>.Node<K, V> foundNode = null;
            int foundIndex = 0;
            while (node != null)
            {
                K[] keys = node.keys;
                int left_idx = node.left_idx;
                int result = cmp(obj, keyK, keys[left_idx]);
                if (result < 0)
                {
                    node = node.left;
                }
                else
                {
                    foundNode = node;
                    foundIndex = left_idx;
                    if (result == 0)
                    {
                        break;
                    }
                    int right_idx = node.right_idx;
                    if (left_idx != right_idx)
                    {
                        result = cmp(obj, key, keys[right_idx]);
                    }
                    if (result >= 0)
                    {
                        foundNode = node;
                        foundIndex = right_idx;
                        if (result == 0)
                        {
                            break;
                        }
                        node = node.right;
                    }
                    else
                    { /* search in node */
                        int low = left_idx + 1, mid = 0, high = right_idx - 1;
                        while (low <= high && result != 0)
                        {
                            mid = (low + high) >> 1;
                            result = cmp(obj, key, keys[mid]);
                            if (result >= 0)
                            {
                                foundNode = node;
                                foundIndex = mid;
                                low = mid + 1;
                            }
                            else
                            {
                                high = mid;
                            }
                            if (result == 0 || (low == high && high == mid))
                            {
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            if (foundNode != null
                    && cmp(obj, keyK, foundNode.keys[foundIndex]) < 0)
            {
                foundNode = null;
            }
            if (foundNode != null)
            {
                return createEntry(foundNode, foundIndex);
            }
            return null;
        }

        TreeMap<K, V>.Entry<K, V> findLowerEntry(K key)
        {
            if (root == null)
            {
                return null;
            }
            java.lang.Comparable<K> obj = comparatorJ == null ? toComparable((K)key)
                    : null;
            K keyK = (K)key;
            TreeMap<K,V>.Node<K, V> node = root;
            TreeMap<K,V>.Node<K, V> foundNode = null;
            int foundIndex = 0;
            while (node != null)
            {
                K[] keys = node.keys;
                int left_idx = node.left_idx;
                int result = cmp(obj, keyK, keys[left_idx]);
                if (result <= 0)
                {
                    node = node.left;
                }
                else
                {
                    foundNode = node;
                    foundIndex = left_idx;
                    int right_idx = node.right_idx;
                    if (left_idx != right_idx)
                    {
                        result = cmp(obj, key, keys[right_idx]);
                    }
                    if (result > 0)
                    {
                        foundNode = node;
                        foundIndex = right_idx;
                        node = node.right;
                    }
                    else
                    { /* search in node */
                        int low = left_idx + 1, mid = 0, high = right_idx - 1;
                        while (low <= high)
                        {
                            mid = (low + high) >> 1;
                            result = cmp(obj, key, keys[mid]);
                            if (result > 0)
                            {
                                foundNode = node;
                                foundIndex = mid;
                                low = mid + 1;
                            }
                            else
                            {
                                high = mid;
                            }
                            if (low == high && high == mid)
                            {
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            if (foundNode != null
                    && cmp(obj, keyK, foundNode.keys[foundIndex]) <= 0)
            {
                foundNode = null;
            }
            if (foundNode != null)
            {
                return createEntry(foundNode, foundIndex);
            }
            return null;
        }

        TreeMap<K, V>.Entry<K, V> findHigherEntry(K key)
        {
            if (root == null)
            {
                return null;
            }
            java.lang.Comparable<K> obj = comparatorJ == null ? toComparable((K)key)
                    : null;
            K keyK = (K)key;
            TreeMap<K,V>.Node<K, V> node = root;
            TreeMap<K,V>.Node<K, V> foundNode = null;
            int foundIndex = 0;
            while (node != null)
            {
                K[] keys = node.keys;
                int right_idx = node.right_idx;
                int result = cmp(obj, keyK, keys[right_idx]);
                if (result >= 0)
                {
                    node = node.right;
                }
                else
                {
                    foundNode = node;
                    foundIndex = right_idx;
                    int left_idx = node.left_idx;
                    if (left_idx != right_idx)
                    {
                        result = cmp(obj, key, keys[left_idx]);
                    }
                    if (result < 0)
                    {
                        foundNode = node;
                        foundIndex = left_idx;
                        node = node.left;
                    }
                    else
                    { /* search in node */
                        foundNode = node;
                        foundIndex = right_idx;
                        int low = left_idx + 1, mid = 0, high = right_idx - 1;
                        while (low <= high)
                        {
                            mid = (low + high) >> 1;
                            result = cmp(obj, key, keys[mid]);
                            if (result < 0)
                            {
                                foundNode = node;
                                foundIndex = mid;
                                high = mid - 1;
                            }
                            else
                            {
                                low = mid + 1;
                            }
                            if (low == high && high == mid)
                            {
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            if (foundNode != null
                    && cmp(obj, keyK, foundNode.keys[foundIndex]) >= 0)
            {
                foundNode = null;
            }
            if (foundNode != null)
            {
                return createEntry(foundNode, foundIndex);
            }
            return null;
        }

        /**
         * Returns the first key in this map.
         *
         * @return the first key in this map.
         * @throws NoSuchElementException
         *                if this map is empty.
         */
        public virtual K firstKey()
        {
            if (root != null)
            {
                TreeMap<K,V>.Node<K, V> node = minimum(root);
                return node.keys[node.left_idx];
            }
            throw new NoSuchElementException();
        }

        TreeMap<K,V>.Node<K, V> findNode(K key)
        {
            java.lang.Comparable<K> obj = comparatorJ == null ? toComparable((K)key)
                    : null;
            K keyK = (K)key;
            TreeMap<K,V>.Node<K, V> node = root;
            while (node != null)
            {
                K[] keys = node.keys;
                int left_idx = node.left_idx;
                int result = cmp(obj, keyK, keys[left_idx]);
                if (result < 0)
                {
                    node = node.left;
                }
                else if (result == 0)
                {
                    return node;
                }
                else
                {
                    int right_idx = node.right_idx;
                    if (left_idx != right_idx)
                    {
                        result = cmp(obj, keyK, keys[right_idx]);
                    }
                    if (result > 0)
                    {
                        node = node.right;
                    }
                    else
                    {
                        return node;
                    }
                }
            }
            return null;
        }

        /**
         * Returns the value of the mapping with the specified key.
         *
         * @param key
         *            the key.
         * @return the value of the mapping with the specified key.
         * @throws ClassCastException
         *             if the key cannot be compared with the keys in this map.
         * @throws NullPointerException
         *             if the key is {@code null} and the comparator cannot handle
         *             {@code null}.
         */


        public override V get(Object key)
        {
            java.lang.Comparable<K> obj = comparatorJ == null ? toComparable((K)key)
                    : null;
            K keyK = (K)key;
            TreeMap<K,V>.Node<K, V> node = root;
            while (node != null)
            {
                K[] keys = node.keys;
                int left_idx = node.left_idx;
                int result = cmp(obj, keyK, keys[left_idx]);
                if (result < 0)
                {
                    node = node.left;
                }
                else if (result == 0)
                {
                    return node.values[left_idx];
                }
                else
                {
                    int right_idx = node.right_idx;
                    if (left_idx != right_idx)
                    {
                        result = cmp(obj, keyK, keys[right_idx]);
                    }
                    if (result > 0)
                    {
                        node = node.right;
                    }
                    else if (result == 0)
                    {
                        return node.values[right_idx];
                    }
                    else
                    { /* search in node */
                        int low = left_idx + 1, mid = 0, high = right_idx - 1;
                        while (low <= high)
                        {
                            mid = java.dotnet.lang.Operator.shiftRightUnsignet((low + high), 1);
                            result = cmp(obj, keyK, keys[mid]);
                            if (result > 0)
                            {
                                low = mid + 1;
                            }
                            else if (result == 0)
                            {
                                return node.values[mid];
                            }
                            else
                            {
                                high = mid - 1;
                            }
                        }
                        return default(V);
                    }
                }
            }
            return default(V);
        }

        /**
         * Returns a set of the keys contained in this map. The set is backed by
         * this map so changes to one are reflected by the other. The set does not
         * support adding.
         *
         * @return a set of the keys.
         */

        public override Set<K> keySet()
        {
            if (keySetJ == null)
            {
                keySetJ = new IAC_KEYSET<K>(this);
            }
            return keySetJ;
        }
        private class IAC_KEYSET<K> : AbstractSet<K>
        {
            private TreeMap<K, V> root;
            public IAC_KEYSET(TreeMap<K, V> root)
            {
                this.root = root;
            }


            public override bool contains(Object obj)
            {
                return root.containsKey(obj);
            }


            public override int size()
            {
                return root.sizeJ;
            }


            public override void clear()
            {
                root.clear();
            }


            public override Iterator<K> iterator()
            {
                return new UnboundedKeyIterator<K, V>(root);
            }
        }

        /**
         * Returns the last key in this map.
         *
         * @return the last key in this map.
         * @throws NoSuchElementException
         *             if this map is empty.
         */
        public virtual K lastKey()
        {
            if (root != null)
            {
                TreeMap<K,V>.Node<K, V> node = maximum(root);
                return node.keys[node.right_idx];
            }
            throw new NoSuchElementException();
        }

        static TreeMap<K, V>.Entry<K, V> maximum<K, V>(TreeMap<K, V>.Entry<K, V> x)
        {
            while (x.right != null)
            {
                x = x.right;
            }
            return x;
        }

        static TreeMap<K, V>.Entry<K, V> minimum<K, V>(TreeMap<K, V>.Entry<K, V> x)
        {
            while (x.left != null)
            {
                x = x.left;
            }
            return x;
        }

        static TreeMap<K, V>.Entry<K, V> predecessor<K, V>(TreeMap<K, V>.Entry<K, V> x)
        {
            if (x.left != null)
            {
                return maximum(x.left);
            }
            TreeMap<K,V>.Entry<K,V> y = x.parent;
            while (y != null && x == y.left)
            {
                x = y;
                y = y.parent;
            }
            return y;
        }

        static private TreeMap<K,V>.Node<K, V> successor<K, V>(TreeMap<K,V>.Node<K,V> x)
        {
            if (x.right != null)
            {
                return minimum(x.right);
            }
            TreeMap<K,V>.Node<K, V> y = x.parent;
            while (y != null && x == y.right)
            {
                x = y;
                y = y.parent;
            }
            return y;
        }

        private int cmp(java.lang.Comparable<K> obj, K key1, K key2)
        {
            return obj != null ? obj.compareTo(key2) : comparatorJ.compare(
                    key1, key2);
        }

        /**
         * Maps the specified key to the specified value.
         *
         * @param key
         *            the key.
         * @param value
         *            the value.
         * @return the value of any previous mapping with the specified key or
         *         {@code null} if there was no mapping.
         * @throws ClassCastException
         *             if the specified key cannot be compared with the keys in this
         *             map.
         * @throws NullPointerException
         *             if the specified key is {@code null} and the comparator
         *             cannot handle {@code null} keys.
         */

        public override V put(K key, V value)
        {
            return putImpl(key, value);
        }

        private V putImpl(K key, V value)
        {
            if (root == null)
            {
                root = createNode(key, value);
                sizeJ = 1;
                modCount++;
                return default(V);
            }
            java.lang.Comparable<K> obj = comparatorJ == null ? toComparable((K)key)
                    : null;
            K keyK = (K)key;
            TreeMap<K,V>.Node<K, V> node = root;
            TreeMap<K,V>.Node<K, V> prevNode = null;
            int result = 0;
            while (node != null)
            {
                prevNode = node;
                K[] keys = node.keys;
                int left_idx = node.left_idx;
                result = cmp(obj, keyK, keys[left_idx]);
                if (result < 0)
                {
                    node = node.left;
                }
                else if (result == 0)
                {
                    V res = node.values[left_idx];
                    node.values[left_idx] = value;
                    return res;
                }
                else
                {
                    int right_idx = node.right_idx;
                    if (left_idx != right_idx)
                    {
                        result = cmp(obj, keyK, keys[right_idx]);
                    }
                    if (result > 0)
                    {
                        node = node.right;
                    }
                    else if (result == 0)
                    {
                        V res = node.values[right_idx];
                        node.values[right_idx] = value;
                        return res;
                    }
                    else
                    { /* search in node */
                        int low = left_idx + 1, mid = 0, high = right_idx - 1;
                        while (low <= high)
                        {
                            mid = java.dotnet.lang.Operator.shiftRightUnsignet((low + high), 1);
                            result = cmp(obj, keyK, keys[mid]);
                            if (result > 0)
                            {
                                low = mid + 1;
                            }
                            else if (result == 0)
                            {
                                V res = node.values[mid];
                                node.values[mid] = value;
                                return res;
                            }
                            else
                            {
                                high = mid - 1;
                            }
                        }
                        result = low;
                        break;
                    }
                }
            } /* while */
            /*
             * if(node == null) { if(prevNode==null) { - case of empty Tree } else {
             * result < 0 - prevNode.left==null - attach here result > 0 -
             * prevNode.right==null - attach here } } else { insert into node.
             * result - index where it should be inserted. }
             */
            sizeJ++;
            modCount++;
            if (node == null)
            {
                if (prevNode == null)
                {
                    // case of empty Tree
                    root = createNode(key, value);
                }
                else if (prevNode.size < TreeMap<K,V>.Node<K, V>.NODE_SIZE)
                {
                    // there is a place for insert
                    if (result < 0)
                    {
                        appendFromLeft(prevNode, key, value);
                    }
                    else
                    {
                        appendFromRight(prevNode, key, value);
                    }
                }
                else
                {
                    // create and link
                    TreeMap<K,V>.Node<K, V> newNode = createNode(key, value);
                    if (result < 0)
                    {
                        attachToLeft(prevNode, newNode);
                    }
                    else
                    {
                        attachToRight(prevNode, newNode);
                    }
                    balance(newNode);
                }
            }
            else
            {
                // insert into node.
                // result - index where it should be inserted.
                if (node.size < TreeMap<K,V>.Node<K, V>.NODE_SIZE)
                { // insert and ok
                    int left_idx = node.left_idx;
                    int right_idx = node.right_idx;
                    if (left_idx == 0
                            || ((right_idx != TreeMap<K,V>.Node<K, V>.NODE_SIZE - 1) && (right_idx
                                    - result <= result - left_idx)))
                    {
                        int right_idxPlus1 = right_idx + 1;
                        java.lang.SystemJ.arraycopy(node.keys, result, node.keys, result + 1,
                                right_idxPlus1 - result);
                        java.lang.SystemJ.arraycopy(node.values, result, node.values,
                                result + 1, right_idxPlus1 - result);
                        node.right_idx = right_idxPlus1;
                        node.keys[result] = key;
                        node.values[result] = value;
                    }
                    else
                    {
                        int left_idxMinus1 = left_idx - 1;
                        java.lang.SystemJ.arraycopy(node.keys, left_idx, node.keys,
                                left_idxMinus1, result - left_idx);
                        java.lang.SystemJ.arraycopy(node.values, left_idx, node.values,
                                left_idxMinus1, result - left_idx);
                        node.left_idx = left_idxMinus1;
                        node.keys[result - 1] = key;
                        node.values[result - 1] = value;
                    }
                    node.size++;
                }
                else
                {
                    // there are no place here
                    // insert and push old pair
                    TreeMap<K,V>.Node<K, V> previous = node.prev;
                    TreeMap<K,V>.Node<K, V> nextNode = node.next;
                    bool removeFromStart;
                    bool attachFromLeft = false;
                    TreeMap<K,V>.Node<K, V> attachHere = null;
                    if (previous == null)
                    {
                        if (nextNode != null && nextNode.size < TreeMap<K,V>.Node<K, V>.NODE_SIZE)
                        {
                            // move last pair to next
                            removeFromStart = false;
                        }
                        else
                        {
                            // next node doesn't exist or full
                            // left==null
                            // drop first pair to new node from left
                            removeFromStart = true;
                            attachFromLeft = true;
                            attachHere = node;
                        }
                    }
                    else if (nextNode == null)
                    {
                        if (previous.size < TreeMap<K,V>.Node<K, V>.NODE_SIZE)
                        {
                            // move first pair to prev
                            removeFromStart = true;
                        }
                        else
                        {
                            // right == null;
                            // drop last pair to new node from right
                            removeFromStart = false;
                            attachFromLeft = false;
                            attachHere = node;
                        }
                    }
                    else
                    {
                        if (previous.size < TreeMap<K,V>.Node<K, V>.NODE_SIZE)
                        {
                            if (nextNode.size < TreeMap<K,V>.Node<K, V>.NODE_SIZE)
                            {
                                // choose prev or next for moving
                                removeFromStart = previous.size < nextNode.size;
                            }
                            else
                            {
                                // move first pair to prev
                                removeFromStart = true;
                            }
                        }
                        else
                        {
                            if (nextNode.size < TreeMap<K,V>.Node<K, V>.NODE_SIZE)
                            {
                                // move last pair to next
                                removeFromStart = false;
                            }
                            else
                            {
                                // prev & next are full
                                // if node.right!=null then node.next.left==null
                                // if node.left!=null then node.prev.right==null
                                if (node.right == null)
                                {
                                    attachHere = node;
                                    attachFromLeft = false;
                                    removeFromStart = false;
                                }
                                else
                                {
                                    attachHere = nextNode;
                                    attachFromLeft = true;
                                    removeFromStart = false;
                                }
                            }
                        }
                    }
                    K movedKey;
                    V movedValue;
                    if (removeFromStart)
                    {
                        // node.left_idx == 0
                        movedKey = node.keys[0];
                        movedValue = node.values[0];
                        int resMunus1 = result - 1;
                        java.lang.SystemJ.arraycopy(node.keys, 1, node.keys, 0, resMunus1);
                        java.lang.SystemJ.arraycopy(node.values, 1, node.values, 0, resMunus1);
                        node.keys[resMunus1] = key;
                        node.values[resMunus1] = value;
                    }
                    else
                    {
                        // node.right_idx == Node<K,V>.NODE_SIZE - 1
                        movedKey = node.keys[Node<K, V>.NODE_SIZE - 1];
                        movedValue = node.values[Node<K, V>.NODE_SIZE - 1];
                        java.lang.SystemJ.arraycopy(node.keys, result, node.keys, result + 1,
                                TreeMap<K,V>.Node<K, V>.NODE_SIZE - 1 - result);
                        java.lang.SystemJ.arraycopy(node.values, result, node.values,
                                result + 1, TreeMap<K,V>.Node<K, V>.NODE_SIZE - 1 - result);
                        node.keys[result] = key;
                        node.values[result] = value;
                    }
                    if (attachHere == null)
                    {
                        if (removeFromStart)
                        {
                            appendFromRight(previous, movedKey, movedValue);
                        }
                        else
                        {
                            appendFromLeft(nextNode, movedKey, movedValue);
                        }
                    }
                    else
                    {
                        TreeMap<K,V>.Node<K, V> newNode = createNode(movedKey, movedValue);
                        if (attachFromLeft)
                        {
                            attachToLeft(attachHere, newNode);
                        }
                        else
                        {
                            attachToRight(attachHere, newNode);
                        }
                        balance(newNode);
                    }
                }
            }
            return default(V);
        }

        private void appendFromLeft(TreeMap<K,V>.Node<K,V> node, K keyObj, V value)
        {
            if (node.left_idx == 0)
            {
                int new_right = node.right_idx + 1;
                java.lang.SystemJ.arraycopy(node.keys, 0, node.keys, 1, new_right);
                java.lang.SystemJ.arraycopy(node.values, 0, node.values, 1, new_right);
                node.right_idx = new_right;
            }
            else
            {
                node.left_idx--;
            }
            node.size++;
            node.keys[node.left_idx] = keyObj;
            node.values[node.left_idx] = value;
        }

        private void attachToLeft(TreeMap<K,V>.Node<K,V> node, TreeMap<K,V>.Node<K, V> newNode)
        {
            newNode.parent = node;
            // node.left==null - attach here
            node.left = newNode;
            TreeMap<K,V>.Node<K, V> predecessor = node.prev;
            newNode.prev = predecessor;
            newNode.next = node;
            if (predecessor != null)
            {
                predecessor.next = newNode;
            }
            node.prev = newNode;
        }

        /*
         * add pair into node; existence free room in the node should be checked
         * before call
         */
        private void appendFromRight(TreeMap<K,V>.Node<K,V> node, K keyObj, V value)
        {
            if (node.right_idx == TreeMap<K,V>.Node<K, V>.NODE_SIZE - 1)
            {
                int left_idx = node.left_idx;
                int left_idxMinus1 = left_idx - 1;
                java.lang.SystemJ.arraycopy(node.keys, left_idx, node.keys, left_idxMinus1,
                        TreeMap<K,V>.Node<K, V>.NODE_SIZE - left_idx);
                java.lang.SystemJ.arraycopy(node.values, left_idx, node.values,
                        left_idxMinus1, TreeMap<K,V>.Node<K, V>.NODE_SIZE - left_idx);
                node.left_idx = left_idxMinus1;
            }
            else
            {
                node.right_idx++;
            }
            node.size++;
            node.keys[node.right_idx] = keyObj;
            node.values[node.right_idx] = value;
        }

        private void attachToRight(TreeMap<K,V>.Node<K,V> node, TreeMap<K,V>.Node<K, V> newNode)
        {
            newNode.parent = node;
            // - node.right==null - attach here
            node.right = newNode;
            newNode.prev = node;
            TreeMap<K,V>.Node<K, V> successor = node.next;
            newNode.next = successor;
            if (successor != null)
            {
                successor.prev = newNode;
            }
            node.next = newNode;
        }

        private TreeMap<K,V>.Node<K, V> createNode(K keyObj, V value)
        {
            TreeMap<K,V>.Node<K, V> node = new TreeMap<K,V>.Node<K, V>();
            node.keys[0] = keyObj;
            node.values[0] = value;
            node.left_idx = 0;
            node.right_idx = 0;
            node.size = 1;
            return node;
        }

        void balance(TreeMap<K,V>.Node<K,V> x)
        {
            TreeMap<K,V>.Node<K, V> y;
            x.color = true;
            while (x != root && x.parent.color)
            {
                if (x.parent == x.parent.parent.left)
                {
                    y = x.parent.parent.right;
                    if (y != null && y.color)
                    {
                        x.parent.color = false;
                        y.color = false;
                        x.parent.parent.color = true;
                        x = x.parent.parent;
                    }
                    else
                    {
                        if (x == x.parent.right)
                        {
                            x = x.parent;
                            leftRotate(x);
                        }
                        x.parent.color = false;
                        x.parent.parent.color = true;
                        rightRotate(x.parent.parent);
                    }
                }
                else
                {
                    y = x.parent.parent.left;
                    if (y != null && y.color)
                    {
                        x.parent.color = false;
                        y.color = false;
                        x.parent.parent.color = true;
                        x = x.parent.parent;
                    }
                    else
                    {
                        if (x == x.parent.left)
                        {
                            x = x.parent;
                            rightRotate(x);
                        }
                        x.parent.color = false;
                        x.parent.parent.color = true;
                        leftRotate(x.parent.parent);
                    }
                }
            }
            root.color = false;
        }

        private void rightRotate(TreeMap<K,V>.Node<K,V> x)
        {
            TreeMap<K,V>.Node<K, V> y = x.left;
            x.left = y.right;
            if (y.right != null)
            {
                y.right.parent = x;
            }
            y.parent = x.parent;
            if (x.parent == null)
            {
                root = y;
            }
            else
            {
                if (x == x.parent.right)
                {
                    x.parent.right = y;
                }
                else
                {
                    x.parent.left = y;
                }
            }
            y.right = x;
            x.parent = y;
        }

        private void leftRotate(TreeMap<K,V>.Node<K,V> x)
        {
            TreeMap<K,V>.Node<K, V> y = x.right;
            x.right = y.left;
            if (y.left != null)
            {
                y.left.parent = x;
            }
            y.parent = x.parent;
            if (x.parent == null)
            {
                root = y;
            }
            else
            {
                if (x == x.parent.left)
                {
                    x.parent.left = y;
                }
                else
                {
                    x.parent.right = y;
                }
            }
            y.left = x;
            x.parent = y;
        }

        /**
         * Copies all the mappings in the given map to this map. These mappings will
         * replace all mappings that this map had for any of the keys currently in
         * the given map.
         *
         * @param map
         *            the map to copy mappings from.
         * @throws ClassCastException
         *             if a key in the specified map cannot be compared with the
         *             keys in this map.
         * @throws NullPointerException
         *             if a key in the specified map is {@code null} and the
         *             comparator cannot handle {@code null} keys.
         */

        public override void putAll(Map<K, V> map)
        {
            Iterator<Object> it = (Iterator<Object>)map.entrySet().iterator();
            while (it.hasNext())
            {
                MapNS.Entry<K, V> entry = (MapNS.Entry<K, V>)it.next();
                this.putImpl(entry.getKey(), entry.getValue());
            }
        }

        /**
         * Removes the mapping with the specified key from this map.
         *
         * @param key
         *            the key of the mapping to remove.
         * @return the value of the removed mapping or {@code null} if no mapping
         *         for the specified key was found.
         * @throws ClassCastException
         *             if the specified key cannot be compared with the keys in this
         *             map.
         * @throws NullPointerException
         *             if the specified key is {@code null} and the comparator
         *             cannot handle {@code null} keys.
         */


        public override V remove(Object key)
        {
            java.lang.Comparable<K> obj = comparatorJ == null ? toComparable((K)key) : null;
            if (sizeJ == 0)
            {
                return default(V);
            }
            K keyK = (K)key;
            TreeMap<K,V>.Node<K, V> node = root;
            while (node != null)
            {
                K[] keys = node.keys;
                int left_idx = node.left_idx;
                int result = cmp(obj, keyK, keys[left_idx]);
                if (result < 0)
                {
                    node = node.left;
                }
                else if (result == 0)
                {
                    V value = node.values[left_idx];
                    removeLeftmost(node);
                    return value;
                }
                else
                {
                    int right_idx = node.right_idx;
                    if (left_idx != right_idx)
                    {
                        result = cmp(obj, keyK, keys[right_idx]);
                    }
                    if (result > 0)
                    {
                        node = node.right;
                    }
                    else if (result == 0)
                    {
                        V value = node.values[right_idx];
                        removeRightmost(node);
                        return value;
                    }
                    else
                    { /*search in node*/
                        int low = left_idx + 1, mid = 0, high = right_idx - 1;
                        while (low <= high)
                        {
                            mid = java.dotnet.lang.Operator.shiftRightUnsignet((low + high), 1);
                            result = cmp(obj, keyK, keys[mid]);
                            if (result > 0)
                            {
                                low = mid + 1;
                            }
                            else if (result == 0)
                            {
                                V value = node.values[mid];
                                removeMiddleElement(node, mid);
                                return value;
                            }
                            else
                            {
                                high = mid - 1;
                            }
                        }
                        return default(V);
                    }
                }
            }
            return default(V);
        }

        internal K removeLeftmost(TreeMap<K, V>.Node<K, V> node)
        {
            int index = node.left_idx;
            // record next key and record if the node is removed
            K key = (index + 1 <= node.right_idx) ? node.keys[index + 1] : default(K);
            if (node.size == 1)
            {
                deleteNode(node);
            }
            else if (node.prev != null && (Node<K, V>.NODE_SIZE - 1 - node.prev.right_idx) > node.size)
            {
                // move all to prev node and kill it
                TreeMap<K,V>.Node<K, V> prev = node.prev;
                int size = node.right_idx - index;
                java.lang.SystemJ.arraycopy(node.keys, index + 1, prev.keys, prev.right_idx + 1, size);
                java.lang.SystemJ.arraycopy(node.values, index + 1, prev.values, prev.right_idx + 1, size);
                prev.right_idx += size;
                prev.size += size;
                deleteNode(node);
            }
            else if (node.next != null && (node.next.left_idx) > node.size)
            {
                // move all to next node and kill it
                TreeMap<K,V>.Node<K, V> next = node.next;
                int size = node.right_idx - index;
                int next_new_left = next.left_idx - size;
                next.left_idx = next_new_left;
                java.lang.SystemJ.arraycopy(node.keys, index + 1, next.keys, next_new_left, size);
                java.lang.SystemJ.arraycopy(node.values, index + 1, next.values, next_new_left, size);
                next.size += size;
                deleteNode(node);
            }
            else
            {
                node.keys[index] = default(K);
                node.values[index] = default(V);
                node.left_idx++;
                node.size--;
                TreeMap<K,V>.Node<K, V> prev = node.prev;
                key = default(K);
                if (prev != null && prev.size == 1)
                {
                    node.size++;
                    node.left_idx--;
                    node.keys[node.left_idx] = prev.keys[prev.left_idx];
                    node.values[node.left_idx] = prev.values[prev.left_idx];
                    deleteNode(prev);
                }
            }
            modCount++;
            sizeJ--;
            return key;
        }

        K removeRightmost(TreeMap<K,V>.Node<K,V> node)
        {
            int index = node.right_idx;
            // store the next key, return if the node is deleted
            K key = (node != null && node.next != null) ? node.next.keys[node.next.left_idx] : default(K);
            if (node.size == 1)
            {
                deleteNode(node);
            }
            else if (node.prev != null && (Node<K, V>.NODE_SIZE - 1 - node.prev.right_idx) > node.size)
            {
                // move all to prev node and kill it
                TreeMap<K,V>.Node<K, V> prev = node.prev;
                int left_idx = node.left_idx;
                int size = index - left_idx;
                java.lang.SystemJ.arraycopy(node.keys, left_idx, prev.keys, prev.right_idx + 1, size);
                java.lang.SystemJ.arraycopy(node.values, left_idx, prev.values, prev.right_idx + 1, size);
                prev.right_idx += size;
                prev.size += size;
                deleteNode(node);
            }
            else if (node.next != null && (node.next.left_idx) > node.size)
            {
                // move all to next node and kill it
                TreeMap<K,V>.Node<K, V> next = node.next;
                int left_idx = node.left_idx;
                int size = index - left_idx;
                int next_new_left = next.left_idx - size;
                next.left_idx = next_new_left;
                java.lang.SystemJ.arraycopy(node.keys, left_idx, next.keys, next_new_left, size);
                java.lang.SystemJ.arraycopy(node.values, left_idx, next.values, next_new_left, size);
                next.size += size;
                deleteNode(node);
            }
            else
            {
                node.keys[index] = default(K);
                node.values[index] = default(V);
                node.right_idx--;
                node.size--;
                TreeMap<K,V>.Node<K, V> next = node.next;
                key = default(K);
                if (next != null && next.size == 1)
                {
                    node.size++;
                    node.right_idx++;
                    node.keys[node.right_idx] = next.keys[next.left_idx];
                    node.values[node.right_idx] = next.values[next.left_idx];
                    deleteNode(next);
                }
            }
            modCount++;
            sizeJ--;
            return key;
        }

        K removeMiddleElement(TreeMap<K,V>.Node<K,V> node, int index)
        {
            // this function is called iff index if some middle element;
            // so node.left_idx < index < node.right_idx
            // condition above assume that node.size > 1
            K ret = default(K);
            if (node.prev != null && (Node<K, V>.NODE_SIZE - 1 - node.prev.right_idx) > node.size)
            {
                // move all to prev node and kill it
                TreeMap<K,V>.Node<K, V> prev = node.prev;
                int left_idx = node.left_idx;
                int size = index - left_idx;
                java.lang.SystemJ.arraycopy(node.keys, left_idx, prev.keys, prev.right_idx + 1, size);
                java.lang.SystemJ.arraycopy(node.values, left_idx, prev.values, prev.right_idx + 1, size);
                prev.right_idx += size;
                size = node.right_idx - index;
                java.lang.SystemJ.arraycopy(node.keys, index + 1, prev.keys, prev.right_idx + 1, size);
                java.lang.SystemJ.arraycopy(node.values, index + 1, prev.values, prev.right_idx + 1, size);
                ret = prev.keys[prev.right_idx + 1];
                prev.right_idx += size;
                prev.size += (node.size - 1);
                deleteNode(node);
            }
            else if (node.next != null && (node.next.left_idx) > node.size)
            {
                // move all to next node and kill it
                TreeMap<K,V>.Node<K, V> next = node.next;
                int left_idx = node.left_idx;
                int next_new_left = next.left_idx - node.size + 1;
                next.left_idx = next_new_left;
                int size = index - left_idx;
                java.lang.SystemJ.arraycopy(node.keys, left_idx, next.keys, next_new_left, size);
                java.lang.SystemJ.arraycopy(node.values, left_idx, next.values, next_new_left, size);
                next_new_left += size;
                size = node.right_idx - index;
                java.lang.SystemJ.arraycopy(node.keys, index + 1, next.keys, next_new_left, size);
                java.lang.SystemJ.arraycopy(node.values, index + 1, next.values, next_new_left, size);
                ret = next.keys[next_new_left];
                next.size += (node.size - 1);
                deleteNode(node);
            }
            else
            {
                int moveFromRight = node.right_idx - index;
                int left_idx = node.left_idx;
                int moveFromLeft = index - left_idx;
                if (moveFromRight <= moveFromLeft)
                {
                    java.lang.SystemJ.arraycopy(node.keys, index + 1, node.keys, index, moveFromRight);
                    java.lang.SystemJ.arraycopy(node.values, index + 1, node.values, index, moveFromRight);
                    TreeMap<K,V>.Node<K, V> next = node.next;
                    if (next != null && next.size == 1)
                    {
                        node.keys[node.right_idx] = next.keys[next.left_idx];
                        node.values[node.right_idx] = next.values[next.left_idx];
                        ret = node.keys[index];
                        deleteNode(next);
                    }
                    else
                    {
                        node.keys[node.right_idx] = default(K);
                        node.values[node.right_idx] = default(V);
                        node.right_idx--;
                        node.size--;
                    }
                }
                else
                {
                    java.lang.SystemJ.arraycopy(node.keys, left_idx, node.keys, left_idx + 1, moveFromLeft);
                    java.lang.SystemJ.arraycopy(node.values, left_idx, node.values, left_idx + 1, moveFromLeft);
                    TreeMap<K,V>.Node<K, V> prev = node.prev;
                    if (prev != null && prev.size == 1)
                    {
                        node.keys[left_idx] = prev.keys[prev.left_idx];
                        node.values[left_idx] = prev.values[prev.left_idx];
                        ret = node.keys[index + 1];
                        deleteNode(prev);
                    }
                    else
                    {
                        node.keys[left_idx] = default(K);
                        node.values[left_idx] = default(V);
                        node.left_idx++;
                        node.size--;
                    }
                }
            }
            modCount++;
            sizeJ--;
            return ret;
        }

        private void deleteNode(TreeMap<K,V>.Node<K,V> node)
        {
            if (node.right == null)
            {
                if (node.left != null)
                {
                    attachToParent(node, node.left);
                }
                else
                {
                    attachNullToParent(node);
                }
                fixNextChain(node);
            }
            else if (node.left == null)
            { // node.right != null
                attachToParent(node, node.right);
                fixNextChain(node);
            }
            else
            {
                // Here node.left!=nul && node.right!=null
                // node.next should replace node in tree
                // node.next!=null by tree logic.
                // node.next.left==null by tree logic.
                // node.next.right may be null or non-null
                TreeMap<K,V>.Node<K, V> toMoveUp = node.next;
                fixNextChain(node);
                if (toMoveUp.right == null)
                {
                    attachNullToParent(toMoveUp);
                }
                else
                {
                    attachToParent(toMoveUp, toMoveUp.right);
                }
                // Here toMoveUp is ready to replace node
                toMoveUp.left = node.left;
                if (node.left != null)
                {
                    node.left.parent = toMoveUp;
                }
                toMoveUp.right = node.right;
                if (node.right != null)
                {
                    node.right.parent = toMoveUp;
                }
                attachToParentNoFixup(node, toMoveUp);
                toMoveUp.color = node.color;
            }
        }

        private void attachToParentNoFixup(TreeMap<K,V>.Node<K,V> toDelete, TreeMap<K,V>.Node<K, V> toConnect)
        {
            // assert toConnect!=null
            TreeMap<K,V>.Node<K, V> parent = toDelete.parent;
            toConnect.parent = parent;
            if (parent == null)
            {
                root = toConnect;
            }
            else if (toDelete == parent.left)
            {
                parent.left = toConnect;
            }
            else
            {
                parent.right = toConnect;
            }
        }

        private void attachToParent(TreeMap<K,V>.Node<K,V> toDelete, TreeMap<K,V>.Node<K, V> toConnect)
        {
            // assert toConnect!=null
            attachToParentNoFixup(toDelete, toConnect);
            if (!toDelete.color)
            {
                fixup(toConnect);
            }
        }

        private void attachNullToParent(TreeMap<K,V>.Node<K,V> toDelete)
        {
            TreeMap<K,V>.Node<K, V> parent = toDelete.parent;
            if (parent == null)
            {
                root = null;
            }
            else
            {
                if (toDelete == parent.left)
                {
                    parent.left = null;
                }
                else
                {
                    parent.right = null;
                }
                if (!toDelete.color)
                {
                    fixup(parent);
                }
            }
        }

        private void fixNextChain(TreeMap<K,V>.Node<K,V> node)
        {
            if (node.prev != null)
            {
                node.prev.next = node.next;
            }
            if (node.next != null)
            {
                node.next.prev = node.prev;
            }
        }

        private void fixup(TreeMap<K,V>.Node<K,V> x)
        {
            TreeMap<K,V>.Node<K, V> w;
            while (x != root && !x.color)
            {
                if (x == x.parent.left)
                {
                    w = x.parent.right;
                    if (w == null)
                    {
                        x = x.parent;
                        continue;
                    }
                    if (w.color)
                    {
                        w.color = false;
                        x.parent.color = true;
                        leftRotate(x.parent);
                        w = x.parent.right;
                        if (w == null)
                        {
                            x = x.parent;
                            continue;
                        }
                    }
                    if ((w.left == null || !w.left.color)
                        && (w.right == null || !w.right.color))
                    {
                        w.color = true;
                        x = x.parent;
                    }
                    else
                    {
                        if (w.right == null || !w.right.color)
                        {
                            w.left.color = false;
                            w.color = true;
                            rightRotate(w);
                            w = x.parent.right;
                        }
                        w.color = x.parent.color;
                        x.parent.color = false;
                        w.right.color = false;
                        leftRotate(x.parent);
                        x = root;
                    }
                }
                else
                {
                    w = x.parent.left;
                    if (w == null)
                    {
                        x = x.parent;
                        continue;
                    }
                    if (w.color)
                    {
                        w.color = false;
                        x.parent.color = true;
                        rightRotate(x.parent);
                        w = x.parent.left;
                        if (w == null)
                        {
                            x = x.parent;
                            continue;
                        }
                    }
                    if ((w.left == null || !w.left.color)
                        && (w.right == null || !w.right.color))
                    {
                        w.color = true;
                        x = x.parent;
                    }
                    else
                    {
                        if (w.left == null || !w.left.color)
                        {
                            w.right.color = false;
                            w.color = true;
                            leftRotate(w);
                            w = x.parent.left;
                        }
                        w.color = x.parent.color;
                        x.parent.color = false;
                        w.left.color = false;
                        rightRotate(x.parent);
                        x = root;
                    }
                }
            }
            x.color = false;
        }

        /**
         * Returns the number of mappings in this map.
         *
         * @return the number of mappings in this map.
         */
        public override int size()
        {
            return sizeJ;
        }

        /**
         * Returns a collection of the values contained in this map. The collection
         * is backed by this map so changes to one are reflected by the other. The
         * collection supports remove, removeAll, retainAll and clear operations,
         * and it does not support add or addAll operations.
         * <p>
         * This method returns a collection which is the subclass of
         * AbstractCollection. The iterator method of this subclass returns a
         * "wrapper object" over the iterator of map's entrySet(). The {@code size}
         * method wraps the map's size method and the {@code contains} method wraps
         * the map's containsValue method.
         * <p>
         * The collection is created when this method is called for the first time
         * and returned in response to all subsequent calls. This method may return
         * different collections when multiple concurrent calls occur, since no
         * synchronization is performed.
         *
         * @return a collection of the values contained in this map.
         */

        public override Collection<V> values()
        {
            if (valuesCollection == null)
            {
                valuesCollection = new IAC_VALUES_COLLECTION<V>(this);
            }
            return valuesCollection;
        }
        private class IAC_VALUES_COLLECTION<V> : AbstractCollection<V>
        {
            private TreeMap<K, V> root;
            public IAC_VALUES_COLLECTION(TreeMap<K, V> root)
            {
                this.root = root;
            }

            public override bool contains(Object obj)
            {
                return root.containsValue(obj);
            }


            public override int size()
            {
                return root.sizeJ;
            }


            public override void clear()
            {
                root.clear();
            }


            public override Iterator<V> iterator()
            {
                return new UnboundedValueIterator<K, V>(root);
            }
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#firstEntry()
         * @since 1.6
         */
        public MapNS.Entry<K, V> firstEntry()
        {
            return newImmutableEntry(findSmallestEntry());
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#lastEntry()
         * @since 1.6
         */
        public MapNS.Entry<K, V> lastEntry()
        {
            return newImmutableEntry(findBiggestEntry());
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#pollFirstEntry()
         * @since 1.6
         */
        public MapNS.Entry<K, V> pollFirstEntry()
        {
            TreeMap<K,V>.Entry<K,V> node = findSmallestEntry();
            SimpleImmutableEntry<K, V> result = newImmutableEntry(node);
            if (null != node)
            {
                remove(node.key);
            }
            return result;
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#pollLastEntry()
         * @since 1.6
         */
        public MapNS.Entry<K, V> pollLastEntry()
        {
            TreeMap<K,V>.Entry<K,V> node = findBiggestEntry();
            SimpleImmutableEntry<K, V> result = newImmutableEntry(node);
            if (null != node)
            {
                remove(node.key);
            }
            return result;
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#higherEntry(obj)
         * @since 1.6
         */
        public MapNS.Entry<K, V> higherEntry(K key)
        {
            return newImmutableEntry(findHigherEntry(key));
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#higherKey(obj)
         * @since 1.6
         */
        public K higherKey(K key)
        {
            MapNS.Entry<K, V> entry = higherEntry(key);
            return (null == entry) ? default(K) : entry.getKey();
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#lowerEntry(obj)
         * @since 1.6
         */
        public MapNS.Entry<K, V> lowerEntry(K key)
        {
            return newImmutableEntry(findLowerEntry(key));
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#lowerKey(obj)
         * @since 1.6
         */
        public K lowerKey(K key)
        {
            MapNS.Entry<K, V> entry = lowerEntry(key);
            return (null == entry) ? default(K) : entry.getKey();
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#ceilingEntry(java.lang.Object)
         * @since 1.6
         */
        public MapNS.Entry<K, V> ceilingEntry(K key)
        {
            return newImmutableEntry(findCeilingEntry(key));
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#ceilingKey(java.lang.Object)
         * @since 1.6
         */
        public K ceilingKey(K key)
        {
            MapNS.Entry<K, V> entry = ceilingEntry(key);
            return (null == entry) ? default(K) : entry.getKey();
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#floorEntry(obj)
         * @since 1.6
         */
        public MapNS.Entry<K, V> floorEntry(K key)
        {
            return newImmutableEntry(findFloorEntry(key));
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#floorKey(obj)
         * @since 1.6
         */
        public K floorKey(K key)
        {
            MapNS.Entry<K, V> entry = floorEntry(key);
            return (null == entry) ? default(K) : entry.getKey();
        }

        AbstractMap<K, V>.SimpleImmutableEntry<K, V> newImmutableEntry(
                TreeMap<K, V>.Entry<K, V> entry)
        {
            return (null == entry) ? null : new SimpleImmutableEntry<K, V>(entry);
        }


        private static java.lang.Comparable<T> toComparable<T>(T obj)
        {
            if (obj == null)
            {
                throw new java.lang.NullPointerException();
            }
            return (java.lang.Comparable<T>)obj;
        }

        int keyCompare(K left, K right)
        {
            return (null != comparator()) ? comparator().compare(left, right)
                    : toComparable(left).compareTo(right);
        }

        static TreeMap<K, V>.Node<K, V> minimum<K, V>(TreeMap<K, V>.Node<K, V> x)
        {
            if (x == null)
            {
                return null;
            }
            while (x.left != null)
            {
                x = x.left;
            }
            return x;
        }

        static TreeMap<K, V>.Node<K, V> maximum<K, V>(TreeMap<K, V>.Node<K, V> x)
        {
            if (x == null)
            {
                return null;
            }
            while (x.right != null)
            {
                x = x.right;
            }
            return x;
        }

        /**
         * Returns a set containing all of the mappings in this map. Each mapping is
         * an instance of {@link MapNS.Entry}. As the set is backed by this map,
         * changes in one will be reflected in the other. It does not support adding
         * operations.
         *
         * @return a set of the mappings.
         */

        public override Set<MapNS.Entry<K, V>> entrySet()
        {
            if (entrySetJ == null)
            {
                entrySetJ = new IAC_ENTRYSET<MapNS.Entry<K, V>>(this);
            }
            return entrySetJ;
        }
        private class IAC_ENTRYSET<E> : AbstractSet<MapNS.Entry<K, V>>
        {
            private TreeMap<K, V> root;
            public IAC_ENTRYSET(TreeMap<K, V> root)
            {
                this.root = root;
            }

            public override int size()
            {
                return root.sizeJ;
            }


            public override void clear()
            {
                root.clear();
            }



            public override bool contains(Object obj)
            {
                if (obj is MapNS.Entry<K, V>)
                {
                    MapNS.Entry<K, V> entry = (MapNS.Entry<K, V>)obj;
                    K key = entry.getKey();
                    Object v1 = root.get(key), v2 = entry.getValue();
                    return v1 == null ? (v2 == null && root.containsKey(key)) : v1.equals(v2);
                }
                return false;
            }


            public override Iterator<MapNS.Entry<K, V>> iterator()
            {
                return new UnboundedEntryIterator<K, V>(root);
            }
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#navigableKeySet()
         * @since 1.6
         */

        public virtual NavigableSet<K> navigableKeySet()
        {
            return (null != navigableKeySetJ) ? navigableKeySetJ : (navigableKeySetJ = (new AscendingSubMap<K, V>(this)).navigableKeySet());
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#descendingKeySet()
         * @since 1.6
         */
        public virtual NavigableSet<K> descendingKeySet()
        {
            return descendingMap().navigableKeySet();
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#descendingMap()
         * @since 1.6
         */
        public virtual NavigableMap<K, V> descendingMap()
        {
            return (null != descendingMapJ) ? descendingMapJ : (descendingMapJ = new DescendingSubMap<K, V>(this));
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#subMap(Object, boolean, Object, boolean)
         * @since 1.6
         */
        public virtual NavigableMap<K, V> subMap(K start, bool startInclusive, K end,
                bool endInclusive)
        {
            if (keyCompare(start, end) <= 0)
            {
                return new AscendingSubMap<K, V>(start, startInclusive, this, end,
                        endInclusive);
            }
            throw new java.lang.IllegalArgumentException();
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#headMap(Object, boolean)
         * @since 1.6
         */
        public virtual NavigableMap<K, V> headMap(K end, bool inclusive)
        {
            // check for error
            keyCompare(end, end);
            return new AscendingSubMap<K, V>(this, end, inclusive);
        }

        /**
         * {@inheritDoc}
         * 
         * @see java.util.NavigableMap#tailMap(Object, boolean)
         * @since 1.6
         */
        public virtual NavigableMap<K, V> tailMap(K start, bool inclusive)
        {
            // check for error
            keyCompare(start, start);
            return new AscendingSubMap<K, V>(start, inclusive, this);
        }

        /**
         * Returns a sorted map over a range of this sorted map with all keys
         * greater than or equal to the specified {@code startKey} and less than the
         * specified {@code endKey}. Changes to the returned sorted map are
         * reflected in this sorted map and vice versa.
         * <p>
         * Note: The returned map will not allow an insertion of a key outside the
         * specified range.
         *
         * @param startKey
         *            the low boundary of the range (inclusive).
         * @param endKey
         *            the high boundary of the range (exclusive),
         * @return a sorted map with the key from the specified range.
         * @throws ClassCastException
         *             if the start or end key cannot be compared with the keys in
         *             this map.
         * @throws NullPointerException
         *             if the start or end key is {@code null} and the comparator
         *             cannot handle {@code null} keys.
         * @throws IllegalArgumentException
         *             if the start key is greater than the end key, or if this map
         *             is itself a sorted map over a range of another sorted map and
         *             the specified range is outside of its range.
         */
        public virtual SortedMap<K, V> subMap(K startKey, K endKey)
        {
            if (comparatorJ == null)
            {
                if (toComparable(startKey).compareTo(endKey) <= 0)
                {
                    return new SubMap<K, V>(startKey, this, endKey);
                }
            }
            else
            {
                if (comparatorJ.compare(startKey, endKey) <= 0)
                {
                    return new SubMap<K, V>(startKey, this, endKey);
                }
            }
            throw new java.lang.IllegalArgumentException();
        }

        /**
         * Returns a sorted map over a range of this sorted map with all keys that
         * are less than the specified {@code endKey}. Changes to the returned
         * sorted map are reflected in this sorted map and vice versa.
         * <p>
         * Note: The returned map will not allow an insertion of a key outside the
         * specified range.
         *
         * @param endKey
         *            the high boundary of the range specified.
         * @return a sorted map where the keys are less than {@code endKey}.
         * @throws ClassCastException
         *             if the specified key cannot be compared with the keys in this
         *             map.
         * @throws NullPointerException
         *             if the specified key is {@code null} and the comparator
         *             cannot handle {@code null} keys.
         * @throws IllegalArgumentException
         *             if this map is itself a sorted map over a range of another
         *             map and the specified key is outside of its range.
         */
        public virtual SortedMap<K, V> headMap(K endKey)
        {
            return headMap(endKey, false);
        }

        /**
         * Returns a sorted map over a range of this sorted map with all keys that
         * are greater than or equal to the specified {@code startKey}. Changes to
         * the returned sorted map are reflected in this sorted map and vice versa.
         * <p>
         * Note: The returned map will not allow an insertion of a key outside the
         * specified range.
         *
         * @param startKey
         *            the low boundary of the range specified.
         * @return a sorted map where the keys are greater or equal to
         *         {@code startKey}.
         * @throws ClassCastException
         *             if the specified key cannot be compared with the keys in this
         *             map.
         * @throws NullPointerException
         *             if the specified key is {@code null} and the comparator
         *             cannot handle {@code null} keys.
         * @throws IllegalArgumentException
         *             if this map itself a sorted map over a range of another map
         *             and the specified key is outside of its range.
         */
        public virtual SortedMap<K, V> tailMap(K startKey)
        {
            return tailMap(startKey, true);
        }

        private void writeObject(java.io.ObjectOutputStream stream)
        {// throws IOException {
            stream.defaultWriteObject();
            stream.writeInt(sizeJ);
            if (sizeJ > 0)
            {
                TreeMap<K,V>.Node<K, V> node = minimum(root);
                while (node != null)
                {
                    int to = node.right_idx;
                    for (int i = node.left_idx; i <= to; i++)
                    {
                        stream.writeObject(node.keys[i]);
                        stream.writeObject(node.values[i]);
                    }
                    node = node.next;
                }
            }
        }


        private void readObject(java.io.ObjectInputStream stream)
        {//throws IOException,
            //ClassNotFoundException {
            stream.defaultReadObject();
            int size = stream.readInt();
            TreeMap<K,V>.Node<K, V> lastNode = null;
            for (int i = 0; i < size; i++)
            {
                lastNode = addToLast(lastNode, (K)stream.readObject(), (V)stream
                        .readObject());
            }
        }

        class AbstractMapIterator<K, V>
        {
            internal TreeMap<K, V> backingMap;

            internal int expectedModCount;

            internal TreeMap<K,V>.Node<K, V> node;

            internal TreeMap<K,V>.Node<K, V> lastNode;

            internal int offset;

            internal int lastOffset;

            internal AbstractMapIterator(TreeMap<K, V> map, TreeMap<K,V>.Node<K, V> startNode,
                    int startOffset)
            {
                backingMap = map;
                expectedModCount = map.modCount;
                if (startNode != null)
                {
                    node = startNode;
                    offset = startOffset;
                }
                else
                {
                    TreeMap<K,V>.Entry<K,V> entry = map.findSmallestEntry();
                    if (entry != null)
                    {
                        node = map.findSmallestEntry().node;
                        offset = node.left_idx;
                    }
                }
            }

            internal AbstractMapIterator(TreeMap<K, V> map, TreeMap<K,V>.Node<K, V> startNode) :
                this(map, startNode, startNode == null ? 0 : startNode.left_idx)
            {
            }

            internal AbstractMapIterator(TreeMap<K, V> map) :
                this(map, minimum(map.root))
            {
            }

            public virtual bool hasNext()
            {
                return node != null;
            }

            internal void makeNext()
            {
                if (expectedModCount != backingMap.modCount)
                {
                    throw new ConcurrentModificationException();
                }
                else if (node == null)
                {
                    throw new NoSuchElementException();
                }
                lastNode = node;
                lastOffset = offset;
                if (offset != node.right_idx)
                {
                    offset++;
                }
                else
                {
                    node = node.next;
                    if (node != null)
                    {
                        offset = node.left_idx;
                    }
                }
            }

            public virtual void remove()
            {
                if (expectedModCount == backingMap.modCount)
                {
                    if (lastNode != null)
                    {
                        int idx = lastOffset;
                        K key = default(K);
                        if (idx == lastNode.left_idx)
                        {
                            key = backingMap.removeLeftmost(lastNode);
                        }
                        else if (idx == lastNode.right_idx)
                        {
                            key = backingMap.removeRightmost(lastNode);
                        }
                        else
                        {
                            int lastRight = lastNode.right_idx;
                            key = backingMap.removeMiddleElement(node, idx);
                            if (null == key && lastRight > lastNode.right_idx)
                            {
                                // removed from right
                                offset--;
                            }
                        }
                        if (null != key)
                        {
                            // the node has been cleared, need to find new node
                            TreeMap<K,V>.Entry<K,V> entry = backingMap.find(key);
                            node = entry.node;
                            offset = entry.index;
                        }
                        lastNode = null;
                        expectedModCount++;
                    }
                    else
                    {
                        throw new java.lang.IllegalStateException();
                    }
                }
                else
                {
                    throw new ConcurrentModificationException();
                }
            }
        }

        class TreeMapEntry<K, V> : MapEntry<K, V>
        {
            TreeMap<K,V>.Node<K, V> node;
            int index;

            internal TreeMapEntry(K theKey, V theValue, TreeMap<K,V>.Node<K, V> node, int index)
                : base(theKey, theValue)
            {
                this.node = node;
                this.index = index;
            }

            // overwrite 
            public override V setValue(V obj)
            {
                V result = value;
                value = obj;
                // set back to TreeMap
                node.values[index] = obj;
                return result;
            }
        }

        class UnboundedEntryIterator<K, V> : AbstractMapIterator<K, V>
                , Iterator<MapNS.Entry<K, V>>
        {

            internal UnboundedEntryIterator(TreeMap<K, V> map, TreeMap<K,V>.Node<K, V> startNode,
                    int startOffset)
                : base(map, startNode, startOffset)
            {
            }

            internal UnboundedEntryIterator(TreeMap<K, V> map)
                : base(map)
            {
            }

            public virtual MapNS.Entry<K, V> next()
            {
                makeNext();
                int idx = lastOffset;
                return new TreeMapEntry<K, V>(lastNode.keys[idx], lastNode.values[idx], lastNode, idx);
            }
        }

        class UnboundedKeyIterator<K, V> : AbstractMapIterator<K, V>
                , Iterator<K>
        {

            internal UnboundedKeyIterator(TreeMap<K, V> map, TreeMap<K,V>.Node<K, V> startNode,
                    int startOffset)
                : base(map, startNode, startOffset)
            {
            }

            internal UnboundedKeyIterator(TreeMap<K, V> map)
                : base(map)
            {
            }

            public virtual K next()
            {
                makeNext();
                return lastNode.keys[lastOffset];
            }
        }

        class UnboundedValueIterator<K, V> : AbstractMapIterator<K, V>
                , Iterator<V>
        {

            internal UnboundedValueIterator(TreeMap<K, V> map, TreeMap<K,V>.Node<K, V> startNode,
                    int startOffset)
                : base(map, startNode, startOffset)
            {
            }

            internal UnboundedValueIterator(TreeMap<K, V> map)
                : base(map)
            {
            }

            public virtual V next()
            {
                makeNext();
                return lastNode.values[lastOffset];
            }
        }
    }
}