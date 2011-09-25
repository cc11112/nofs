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
 * Thanks for original source code, (class LinkedDictionary) written by Scott Garland.
 * see also NOTICE.LinkedHashMap
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util
{
    public enum LinkOrder
    {
        Insert,
        InsertReverse,
        Access,
        AccessReverse
    }
    /// <summary>
    /// LinkedHashMap implementation
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class LinkedHashMap<TKey, TValue> : HashMap<TKey,TValue>, IDictionary<TKey, TValue>, IDictionary, java.util.Map<TKey, TValue>
    {
        private const int HashMask = 0x7fffffff;
        private readonly IEqualityComparer<TKey> _comparer;
        private readonly bool _isKeyValueType;
        private readonly Action<HashNode<TKey, TValue>, bool> _touch;
        private HashNode<TKey, TValue>[] _buckets;
        private int _count;
        private HashNode<TKey, TValue> _head;
        private int _rehashThreshold;
        private object _syncRoot;
        private HashNode<TKey, TValue> _tail;
        /// <summary>
        /// Create a new LinkedHashMap
        /// </summary>
        public LinkedHashMap()
            : this(16, LinkOrder.Insert, EqualityComparer<TKey>.Default)
        {
        }

        public LinkedHashMap(IEqualityComparer<TKey> comparer)
            : this(16, LinkOrder.Insert, comparer)
        {
        }

        public LinkedHashMap(int initialCapacity)
            : this(initialCapacity, LinkOrder.Insert, EqualityComparer<TKey>.Default)
        {
        }

#region Java like constructors
        /// <summary>
        /// Create a new LinkedHashMap.
        /// </summary>
        /// <param name="initialCapacity"></param>
        /// <param name="ignoredLoadFactor">ignored</param>
        public LinkedHashMap(int initialCapacity, float ignoredLoadFactor)
            : this(initialCapacity, LinkOrder.Insert, EqualityComparer<TKey>.Default)
        {
        }
#endregion

        public LinkedHashMap(int initialCapacity, IEqualityComparer<TKey> comparer)
            : this(initialCapacity, LinkOrder.Insert, comparer)
        {
        }

        public LinkedHashMap(LinkOrder linkOrder)
            : this(16, linkOrder, EqualityComparer<TKey>.Default)
        {
        }

        public LinkedHashMap(int initialCapacity, LinkOrder linkOrder, IEqualityComparer<TKey> comparer)
        {
            LinkOrder = linkOrder;
            switch (linkOrder)
            {
                case LinkOrder.Insert:
                    _touch = LinkOrderInsert;
                    break;
                case LinkOrder.InsertReverse:
                    _touch = LinkOrderInsertReverse;
                    break;
                case LinkOrder.Access:
                    _touch = LinkOrderAccess;
                    break;
                case LinkOrder.AccessReverse:
                    _touch = LinkOrderAccessReverse;
                    break;
            }

            _buckets = new HashNode<TKey, TValue>[initialCapacity];
            _rehashThreshold = (int)(_buckets.Length * 1.75f);
            _comparer = comparer;
            _isKeyValueType = typeof(TKey).IsValueType;
        }

        public bool IsAccessOrdered
        {
            get { return LinkOrder == LinkOrder.Access || LinkOrder == LinkOrder.AccessReverse; }
        }

        public LinkOrder LinkOrder { get; private set; }

        public new IEqualityComparer<TKey> Comparer
        {
            get { return _comparer; }
        }

        public TKey FirstKey
        {
            get
            {
                if (_head == null)
                {
                    throw new InvalidOperationException("Collection is empty");
                }
                return _head.Key;
            }
        }

        public TKey LastKey
        {
            get
            {
                if (_tail == null)
                {
                    throw new InvalidOperationException("Collection is empty");
                }
                return _tail.Key;
            }
        }

        #region java.util.Map

        public override void clear()
        {
            this.Clear();
        }
        public override bool containsKey(Object key)
        {
            return this.ContainsKey((TKey)key);
        }
        public override bool containsValue(Object value)
        {
            return this.Values.Contains((TValue)value);
        }
        public override TValue get(Object key)
        {
            TValue returnValue = default(TValue);
            this.TryGetValue((TKey)key, out returnValue);
            return returnValue;
        }
        public override bool isEmpty()
        {
            return 0 < this.size();
        }
        public override TValue put(TKey key, TValue value)
        {
            TValue oldValue = default(TValue);
            oldValue = this.get(key);
            this.Add(key, value);
            return oldValue;

        }
        public override void putAll(Map<TKey, TValue> map)
        {
            java.util.Iterator<TKey> it = map.keySet().iterator();
            while (it.hasNext())
            {
                TKey k = it.next();
                this.put(k, map.get(k));
            }
        }
        public override TValue remove(Object key)
        {
            TValue oldValue = this.get(key);
            this.Remove((TKey)key);
            return oldValue;
        }
        public override int size()
        {
            return this.Count;
        }
        public override Collection<TValue> values()
        {
            ArrayList<TValue> valueCollection = new ArrayList<TValue>(this.size());
            ICollection<TValue> dotNetCollection = this.Values;
            foreach (TValue value in dotNetCollection)
            {
                valueCollection.add(value);
            }
            return valueCollection;
        }
        public override Set<TKey> keySet()
        {
            DefaultSet<TKey> setCollection = new DefaultSet<TKey>();
            foreach (TKey key in this.Keys)
            {
                setCollection.add(key);
            }
            return setCollection;
        }
        public override Set<MapNS.Entry<TKey, TValue>> entrySet()
        {
            DefaultSet<MapNS.DefaultEntry<TKey, TValue>> setCollection = new DefaultSet<MapNS.DefaultEntry<TKey, TValue>>();
            foreach (TKey key in this.Keys)
            {
                TValue v = default(TValue);
                this.TryGetValue(key, out v);
                MapNS.DefaultEntry<TKey, TValue> e = new MapNS.DefaultEntry<TKey, TValue>(key, v);
                setCollection.add(e);
            }
            Set<MapNS.Entry<TKey, TValue>> returnValue = (Set<MapNS.Entry<TKey, TValue>>)setCollection;
            return returnValue;
        }
    
        #endregion

        #region IDictionary Members

        void IDictionary.Add(object key, object value)
        {
            Add((TKey)key, (TValue)value);
        }

        void IDictionary.Clear()
        {
            Clear();
        }

        bool IDictionary.Contains(object key)
        {
            return ContainsKey((TKey)key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new KeyValueEnumerator(this);
        }

        bool IDictionary.IsFixedSize
        {
            get { return false; }
        }

        bool IDictionary.IsReadOnly
        {
            get { return IsReadOnly; }
        }

        ICollection IDictionary.Keys
        {
            get { return new KeyCollection(this); }
        }

        void IDictionary.Remove(object key)
        {
            Remove((TKey)key);
        }

        ICollection IDictionary.Values
        {
            get { return new ValueCollection(this); }
        }

        object IDictionary.this[object key]
        {
            get { return this[(TKey)key]; }
            set { this[(TKey)key] = (TValue)value; }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if ((index < 0) || (index > array.Length))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if ((array.Length - index) < Count)
            {
                throw new ArgumentException("Array too small");
            }
            var keyValuePairArray = array as KeyValuePair<TKey, TValue>[];
            if (keyValuePairArray != null)
            {
                CopyTo(keyValuePairArray, index);
            }
            else if (array is DictionaryEntry[])
            {
                var entryArray = array as DictionaryEntry[];
                foreach (var entry in this)
                {
                    entryArray[index++] = new DictionaryEntry(entry.Key, entry.Value);
                }
            }
            else
            {
                var objArray = array as object[];
                if (objArray == null)
                {
                    throw new ArgumentException("Invalid Array Type", "array");
                }
                try
                {
                    foreach (var entry in this)
                    {
                        objArray[index++] = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException("Invalid Array Type", "array");
                }
            }
        }

        int ICollection.Count
        {
            get { return Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                }
                return _syncRoot;
            }
        }

        #endregion

        #region KeyValueEnumerator

        private struct KeyValueEnumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
        {
            private readonly LinkedHashMap<TKey, TValue> _dictionary;
            private HashNode<TKey, TValue> _current;

            public KeyValueEnumerator(LinkedHashMap<TKey, TValue> dictionary)
            {
                if (dictionary == null)
                {
                    throw new ArgumentNullException("dictionary");
                }
                _dictionary = dictionary;
                _current = null;
            }

            #region IDictionaryEnumerator Members

            DictionaryEntry IDictionaryEnumerator.Entry
            {
                get
                {
                    if (_current == null)
                    {
                        throw new InvalidOperationException();
                    }
                    return new DictionaryEntry(_current.Key, _current.Value);
                }
            }

            object IDictionaryEnumerator.Key
            {
                get
                {
                    if (_current == null)
                    {
                        throw new InvalidOperationException();
                    }
                    return _current.Key;
                }
            }

            object IDictionaryEnumerator.Value
            {
                get
                {
                    if (_current == null)
                    {
                        throw new InvalidOperationException();
                    }
                    return _current.Value;
                }
            }

            #endregion

            #region IEnumerator<KeyValuePair<TKey,TValue>> Members

            public KeyValuePair<TKey, TValue> Current
            {
                get
                {
                    if (_current == null)
                    {
                        throw new InvalidOperationException();
                    }
                    return new KeyValuePair<TKey, TValue>(_current.Key, _current.Value);
                }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return new DictionaryEntry(_current.Key, _current.Value); }
            }

            bool IEnumerator.MoveNext()
            {
                return MoveNext();
            }

            void IEnumerator.Reset()
            {
                Reset();
            }

            #endregion

            public bool MoveNext()
            {
                if (_dictionary._head == null)
                {
                    return false;
                }
                if (_current == null)
                {
                    _current = _dictionary._head;
                    return true;
                }
                if (_current.NextInOrder != null)
                {
                    _current = _current.NextInOrder;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _current = null;
            }
        }

        #endregion

        #region KeyCollection

        private new class KeyCollection : ICollection<TKey>, ICollection
        {
            private readonly LinkedHashMap<TKey, TValue> _dictionary;

            public KeyCollection(LinkedHashMap<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
            }

            #region ICollection Members

            void ICollection.CopyTo(Array array, int index)
            {
                CopyTo((TKey[])array, index);
            }

            int ICollection.Count
            {
                get { return Count; }
            }

            bool ICollection.IsSynchronized
            {
                get { return false; }
            }

            object ICollection.SyncRoot
            {
                get { return this; }
            }

            #endregion

            #region ICollection<TKey> Members

            public void Add(TKey item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(TKey item)
            {
                return _dictionary.ContainsKey(item);
            }

            public void CopyTo(TKey[] array, int arrayIndex)
            {
                foreach (var k in this)
                {
                    array[arrayIndex++] = k;
                }
            }

            public int Count
            {
                get { return _dictionary.Count; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public bool Remove(TKey item)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<TKey> GetEnumerator()
            {
                return new KeyEnumerator(_dictionary);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        #endregion

        #region KeyEnumerator

        private struct KeyEnumerator : IEnumerator<TKey>
        {
            private readonly LinkedHashMap<TKey, TValue> _dictionary;
            private HashNode<TKey, TValue> _current;

            public KeyEnumerator(LinkedHashMap<TKey, TValue> dictionary)
            {
                if (dictionary == null)
                {
                    throw new ArgumentNullException("dictionary");
                }
                _dictionary = dictionary;
                _current = null;
            }

            #region IEnumerator<TKey> Members

            public TKey Current
            {
                get
                {
                    if (_current == null)
                    {
                        throw new InvalidOperationException();
                    }
                    return _current.Key;
                }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (_dictionary._head == null)
                {
                    return false;
                }
                if (_current == null)
                {
                    _current = _dictionary._head;
                    return true;
                }
                if (_current.NextInOrder != null)
                {
                    _current = _current.NextInOrder;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _current = null;
            }

            #endregion
        }

        #endregion

        #region ValueCollection

        private new class ValueCollection : ICollection<TValue>, ICollection
        {
            private readonly LinkedHashMap<TKey, TValue> _dictionary;

            public ValueCollection(LinkedHashMap<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
            }

            #region ICollection Members

            void ICollection.CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            int ICollection.Count
            {
                get { return Count; }
            }

            bool ICollection.IsSynchronized
            {
                get { return false; }
            }

            object ICollection.SyncRoot
            {
                get { return this; }
            }

            #endregion

            #region ICollection<TValue> Members

            public void Add(TValue item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(TValue item)
            {
                return (from v in this where v.Equals(item) select v).Any();
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                foreach (var value in _dictionary.Values)
                {
                    array[arrayIndex++] = value;
                }
            }

            public int Count
            {
                get { return _dictionary.Count; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public bool Remove(TValue item)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                return new ValueEnumerator(_dictionary);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        #endregion

        #region ValueEnumerator

        private struct ValueEnumerator : IEnumerator<TValue>
        {
            private readonly LinkedHashMap<TKey, TValue> _dictionary;
            private HashNode<TKey, TValue> _current;
            private TValue _lastValue;
            private bool _lastValuePresent;

            public ValueEnumerator(LinkedHashMap<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _current = null;
                _lastValuePresent = false;
                _lastValue = default(TValue);
            }

            #region IEnumerator<TValue> Members

            public TValue Current
            {
                get
                {
                    if (_current == null)
                    {
                        throw new InvalidOperationException();
                    }
                    if (_lastValuePresent)
                    {
                        return _lastValue;
                    }
                    // TODO: this could be optimized by caching the value.
                    int hash = _dictionary._comparer.GetHashCode(_current.Key);
                    int bucketIndex = (_dictionary._comparer.GetHashCode(_current.Key) & HashMask) % _dictionary._buckets.Length;
                    for (var node = _dictionary._buckets[bucketIndex]; node != null; node = node.Next)
                    {
                        if (hash == (_dictionary._comparer.GetHashCode(node.Key)) && _dictionary._comparer.Equals(node.Key, _current.Key))
                        {
                            _lastValuePresent = true;
                            _lastValue = node.Value;
                            return _lastValue;
                        }
                    }
                    throw new InvalidOperationException();
                }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                _lastValuePresent = false;
                if (_dictionary._head == null)
                {
                    return false;
                }
                if (_current == null)
                {
                    _current = _dictionary._head;
                    return true;
                }
                if (_current.NextInOrder != null)
                {
                    _current = _current.NextInOrder;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _lastValuePresent = false;
                _current = null;
            }

            #endregion
        }

        #endregion

        #region IDictionary<TKey,TValue> Members

        public new void Add(TKey key, TValue value)
        {
            if (!_isKeyValueType && ReferenceEquals(key, null))
            {
                throw new ArgumentNullException("key");
            }
            Insert(key, value, true);
        }

        public new bool ContainsKey(TKey key)
        {
            if (!_isKeyValueType && ReferenceEquals(key, null))
            {
                throw new ArgumentNullException("key");
            }

            int hash = _comparer.GetHashCode(key);
            int bucketIndex = (hash & HashMask) % _buckets.Length;
            for (var node = _buckets[bucketIndex]; node != null; node = node.Next)
            {
                if (hash == (_comparer.GetHashCode(node.Key)) && _comparer.Equals(node.Key, key))
                {
                    if (IsAccessOrdered)
                    {
                        _touch(node, IsAccessOrdered);
                    }
                    return true;
                }
            }
            return false;
        }

        public new ICollection<TKey> Keys
        {
            get { return new KeyCollection(this); }
        }

        public new bool Remove(TKey key)
        {
            if (!_isKeyValueType && ReferenceEquals(key, null))
            {
                throw new ArgumentNullException("key");
            }

            int bucketIndex = (_comparer.GetHashCode(key) & HashMask) % _buckets.Length;

            int startCount = _count;
            HashNode<TKey, TValue> previous = null;
            int hash = _comparer.GetHashCode(key);
            var node = _buckets[bucketIndex];
            while (node != null)
            {
                // Supports duplicate key deletion, even though I don't support dup key insertions
                if (hash == (_comparer.GetHashCode(node.Key)) && _comparer.Equals(node.Key, key))
                {
                    UnlinkNode(node);
                    if (previous == null)
                    {
                        _buckets[bucketIndex] = node.Next;
                    }
                    else
                    {
                        previous.Next = node.Next;
                    }
                    _count--;
                }
                previous = node;
                node = node.Next;
            }
            return startCount != _count;
        }

        public new bool TryGetValue(TKey key, out TValue value)
        {
            if (!_isKeyValueType && ReferenceEquals(key, null))
            {
                throw new ArgumentNullException("key");
            }

            int hash = _comparer.GetHashCode(key);
            int bucketIndex = (hash & HashMask) % _buckets.Length;
            for (var node = _buckets[bucketIndex]; node != null; node = node.Next)
            {
                if (hash == (_comparer.GetHashCode(node.Key)) && _comparer.Equals(node.Key, key))
                {
                    value = node.Value;
                    if (IsAccessOrdered)
                    {
                        _touch(node, IsAccessOrdered);
                    }
                    return true;
                }
            }
            value = default(TValue);
            return false;
        }

        public new ICollection<TValue> Values
        {
            get { return new ValueCollection(this); }
        }

        public new TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (TryGetValue(key, out value))
                {
                    return value;
                }
                return default(TValue);
            }
            set { Insert(key, value, false); }
        }

        public virtual void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public new void Clear()
        {
            _buckets = new HashNode<TKey, TValue>[_buckets.Length];
            _count = 0;
            _head = _tail = null;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if ((array.Length - index) < Count)
            {
                throw new ArgumentException("Array too small");
            }
            foreach (var pair in this)
            {
                if (index < array.Length)
                {
                    array[index++] = new KeyValuePair<TKey, TValue>(pair.Key, pair.Value);
                }
            }
        }

        public new int Count
        {
            get { return _count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public new IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new KeyValueEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            foreach (var kp in collection)
            {
                Add(kp.Key, kp.Value);
            }
        }

        private void Insert(TKey key, TValue value, bool addOnly)
        {
            int hash = _comparer.GetHashCode(key);
            int bucketIndex = (hash & HashMask) % _buckets.Length;
            for (var node = _buckets[bucketIndex]; node != null; node = node.Next)
            {
                if (hash == (_comparer.GetHashCode(node.Key)) && _comparer.Equals(node.Key, key))
                {
                    if (addOnly)
                    {
                        throw new ArgumentException("An item with the same key has already been added.", "key");
                    }
                    node.Key = key;
                    node.Value = value;
                    _touch(node, IsAccessOrdered);
                    return;
                }
            }

            var newNode = new HashNode<TKey, TValue> { Key = key, Value = value, Next = _buckets[bucketIndex] };
            _buckets[bucketIndex] = newNode;
            _touch(newNode, false);

            if (++_count > _rehashThreshold)
            {
                GrowCapacity();
            }
        }

        private void GrowCapacity()
        {
            var newBuckets = new HashNode<TKey, TValue>[(_buckets.Length * 2) + 1];
            _rehashThreshold = (int)(_buckets.Length * 1.75f);

            for (int i = 0; i < _buckets.Length; i++)
            {
                var current = _buckets[i];
                while (current != null)
                {
                    int bucketIndex = (_comparer.GetHashCode(current.Key) & HashMask) % newBuckets.Length;
                    var dest = newBuckets[bucketIndex];

                    // Have we already used this new bucket?
                    if (dest != null)
                    {
                        // Append current node to the end
                        while (dest.Next != null)
                        {
                            dest = dest.Next;
                        }
                        dest.Next = current;
                    }
                    else
                    {
                        newBuckets[bucketIndex] = current;
                    }

                    // Advance to next in current items, rehashing may move some items off existing bucket lists.
                    var next = current.Next;
                    current.Next = null;
                    current = next;
                }
            }
            _buckets = newBuckets;
        }

        private void InsertAtTail(HashNode<TKey, TValue> node)
        {
            if (_head == null)
            {
                _head = _tail = node;
                return;
            }

            if (_tail == node)
            {
                return;
            }

            var tempTail = _tail;
            node.NextInOrder = null;
            node.PreviousInOrder = tempTail;
            tempTail.NextInOrder = node;
            _tail = node;
        }

        private void InsertAtHead(HashNode<TKey, TValue> node)
        {
            if (_head == null)
            {
                _head = _tail = node;
                return;
            }

            if (_head == node)
            {
                return;
            }

            node.NextInOrder = _head;
            _head.PreviousInOrder = node;
            _head = node;
        }

        private void MoveToHead(HashNode<TKey, TValue> node)
        {
            if (_head == node)
            {
                return;
            }

            UnlinkNode(node);

            var temp = _head;
            _head = node;
            _head.NextInOrder = temp;
            temp.PreviousInOrder = _head;
        }

        private void MoveToTail(HashNode<TKey, TValue> node)
        {
            if (node == _tail)
            {
                return;
            }

            UnlinkNode(node);

            // Now attach to the tail.
            node.NextInOrder = null;
            node.PreviousInOrder = _tail;
            _tail.NextInOrder = node;
            _tail = node;
        }

        private void LinkOrderInsert(HashNode<TKey, TValue> node, bool touch)
        {
            if (!touch)
            {
                InsertAtTail(node);
            }
        }

        private void LinkOrderInsertReverse(HashNode<TKey, TValue> node, bool touch)
        {
            if (!touch)
            {
                InsertAtHead(node);
            }
        }

        private void LinkOrderAccess(HashNode<TKey, TValue> node, bool touch)
        {
            if (!touch)
            {
                InsertAtTail(node);
            }
            else
            {
                MoveToTail(node);
            }
        }

        private void LinkOrderAccessReverse(HashNode<TKey, TValue> node, bool touch)
        {
            if (!touch)
            {
                InsertAtHead(node);
            }
            else
            {
                MoveToHead(node);
            }
        }

        private void UnlinkNode(HashNode<TKey, TValue> node)
        {
            if (node == null || (node.PreviousInOrder == null && node.NextInOrder == null))
            {
                return;
            }
            if (node == _head && node == _tail)
            {
                _head = _tail = null;
            }
            else if (node == _head)
            {
                _head = _head.NextInOrder;
                _head.PreviousInOrder = null;
            }
            else if (node == _tail)
            {
                _tail = _tail.PreviousInOrder;
                _tail.NextInOrder = null;
            }
            else
            {
                if (node.NextInOrder != null)
                {
                    node.NextInOrder.PreviousInOrder = node.PreviousInOrder;
                }
                if (node.PreviousInOrder != null)
                {
                    node.PreviousInOrder.NextInOrder = node.NextInOrder;
                }
            }
            node.NextInOrder = node.PreviousInOrder = null;
        }

        #region Nested type: HashNode

        private class HashNode<TK, TV>
        {
            public TK Key;
            public HashNode<TK, TV> Next;
            public HashNode<TK, TV> NextInOrder;
            public HashNode<TK, TV> PreviousInOrder;
            public TV Value;
        }

        #endregion
    }
}
