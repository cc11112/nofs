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
using org.apache.commons.collections;
using org.apache.commons.collections.iterators;

namespace org.apache.commons.collections.map
{

    /**
     * An abstract implementation of a hash-based map which provides numerous points for
     * subclasses to override.
     * <p>
     * This class implements all the features necessary for a subclass hash-based map.
     * Key-value entries are stored in instances of the <code>HashEntry</code> class,
     * which can be overridden and replaced. The iterators can similarly be replaced,
     * without the need to replace the KeySet, EntrySet and Values view classes.
     * <p>
     * Overridable methods are provided to change the default hashing behaviour, and
     * to change how entries are added to and removed from the map. Hopefully, all you
     * need for unusual subclasses is here.
     * <p>
     * NOTE: From Commons Collections 3.1 this class extends AbstractMap.
     * This is to provide backwards compatibility for ReferenceMap between v3.0 and v3.1.
     * This extends clause will be removed in v4.0.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author java util HashMap
     * @author Stephen Colebourne
     * @author Christian Siefkes
     */
    public class AbstractHashedMap : java.util.AbstractMap<Object, Object>, IterableMap
    {

        protected internal static readonly String NO_NEXT_ENTRY = "No next() entry in the iteration";
        protected internal static readonly String NO_PREVIOUS_ENTRY = "No previous() entry in the iteration";
        protected internal static readonly String REMOVE_INVALID = "remove() can only be called once after next()";
        protected internal static readonly String GETKEY_INVALID = "getKey() can only be called after next() and before remove()";
        protected internal static readonly String GETVALUE_INVALID = "getValue() can only be called after next() and before remove()";
        protected internal static readonly String SETVALUE_INVALID = "setValue() can only be called after next() and before remove()";

        /** The default capacity to use */
        protected static readonly int DEFAULT_CAPACITY = 16;
        /** The default threshold to use */
        protected static readonly int DEFAULT_THRESHOLD = 12;
        /** The default load factor to use */
        protected static readonly float DEFAULT_LOAD_FACTOR = 0.75f;
        /** The maximum capacity allowed */
        protected static readonly int MAXIMUM_CAPACITY = 1 << 30;
        /** An object for masking null */
        protected internal static readonly Object NULL = new Object();

        /** Load factor, normally 0.75 */
        [NonSerialized]
        protected float loadFactor;
        /** The size of the map */
        [NonSerialized]
        protected int sizeJ;
        /** Map entries */
        [NonSerialized]
        protected internal HashEntry[] data;
        /** Size at which to rehash */
        [NonSerialized]
        protected int threshold;
        /** Modification count for iterators */
        [NonSerialized]
        protected internal int modCount;
        /** Entry set */
        [NonSerialized]
        protected EntrySet entrySetJ;
        /** Key set */
        [NonSerialized]
        protected new KeySet keySetJ;
        /** Values */
        [NonSerialized]
        protected Values valuesJ;

        /**
         * Constructor only used in deserialization, do not use otherwise.
         */
        protected AbstractHashedMap()
            : base()
        {
        }

        /**
         * Constructor which performs no validation on the passed in parameters.
         * 
         * @param initialCapacity  the initial capacity, must be a power of two
         * @param loadFactor  the load factor, must be &gt; 0.0f and generally &lt; 1.0f
         * @param threshold  the threshold, must be sensible
         */
        protected AbstractHashedMap(int initialCapacity, float loadFactor, int threshold)
            : base()
        {
            this.loadFactor = loadFactor;
            this.data = new HashEntry[initialCapacity];
            this.threshold = threshold;
            init();
        }

        /**
         * Constructs a new, empty map with the specified initial capacity and
         * default load factor. 
         *
         * @param initialCapacity  the initial capacity
         * @throws IllegalArgumentException if the initial capacity is less than one
         */
        protected AbstractHashedMap(int initialCapacity)
            : this(initialCapacity, DEFAULT_LOAD_FACTOR)
        {
        }

        /**
         * Constructs a new, empty map with the specified initial capacity and
         * load factor. 
         *
         * @param initialCapacity  the initial capacity
         * @param loadFactor  the load factor
         * @throws IllegalArgumentException if the initial capacity is less than one
         * @throws IllegalArgumentException if the load factor is less than or equal to zero
         */
        protected AbstractHashedMap(int initialCapacity, float loadFactor)
            : base()
        {
            if (initialCapacity < 1)
            {
                throw new java.lang.IllegalArgumentException("Initial capacity must be greater than 0");
            }
            if (loadFactor <= 0.0f || java.lang.Float.isNaN(loadFactor))
            {
                throw new java.lang.IllegalArgumentException("Load factor must be greater than 0");
            }
            this.loadFactor = loadFactor;
            initialCapacity = calculateNewCapacity(initialCapacity);
            this.threshold = calculateThreshold(initialCapacity, loadFactor);
            this.data = new HashEntry[initialCapacity];
            init();
        }

        /**
         * Constructor copying elements from another map.
         *
         * @param map  the map to copy
         * @throws NullPointerException if the map is null
         */
        protected AbstractHashedMap(java.util.Map<Object, Object> map)
            : this(java.lang.Math.max(2 * map.size(), DEFAULT_CAPACITY), DEFAULT_LOAD_FACTOR)
        {
            putAll(map);
        }

        /**
         * Initialise subclasses during construction, cloning or deserialization.
         */
        protected virtual void init()
        {
        }

        //-----------------------------------------------------------------------
        /**
         * Gets the value mapped to the key specified.
         * 
         * @param key  the key
         * @return the mapped value, null if no match
         */
        public override Object get(Object key)
        {
            key = convertKey(key);
            int hashCode = hash(key);
            HashEntry entry = data[hashIndex(hashCode, data.Length)]; // no local for hash index
            while (entry != null)
            {
                if (entry.hashCode == hashCode && isEqualKey(key, entry.key))
                {
                    return entry.getValue();
                }
                entry = entry.nextJ;
            }
            return null;
        }

        /**
         * Gets the size of the map.
         * 
         * @return the size
         */
        public override int size()
        {
            return sizeJ;
        }

        /**
         * Checks whether the map is currently empty.
         * 
         * @return true if the map is currently size zero
         */
        public override bool isEmpty()
        {
            return (sizeJ == 0);
        }

        //-----------------------------------------------------------------------
        /**
         * Checks whether the map contains the specified key.
         * 
         * @param key  the key to search for
         * @return true if the map contains the key
         */
        public override bool containsKey(Object key)
        {
            key = convertKey(key);
            int hashCode = hash(key);
            HashEntry entry = data[hashIndex(hashCode, data.Length)]; // no local for hash index
            while (entry != null)
            {
                if (entry.hashCode == hashCode && isEqualKey(key, entry.key))
                {
                    return true;
                }
                entry = entry.nextJ;
            }
            return false;
        }

        /**
         * Checks whether the map contains the specified value.
         * 
         * @param value  the value to search for
         * @return true if the map contains the value
         */
        public override bool containsValue(Object value)
        {
            if (value == null)
            {
                for (int i = 0, isize = data.Length; i < isize; i++)
                {
                    HashEntry entry = data[i];
                    while (entry != null)
                    {
                        if (entry.getValue() == null)
                        {
                            return true;
                        }
                        entry = entry.nextJ;
                    }
                }
            }
            else
            {
                for (int i = 0, isize = data.Length; i < isize; i++)
                {
                    HashEntry entry = data[i];
                    while (entry != null)
                    {
                        if (isEqualValue(value, entry.getValue()))
                        {
                            return true;
                        }
                        entry = entry.nextJ;
                    }
                }
            }
            return false;
        }

        //-----------------------------------------------------------------------
        /**
         * Puts a key-value mapping into this map.
         * 
         * @param key  the key to add
         * @param value  the value to add
         * @return the value previously mapped to this key, null if none
         */
        public override Object put(Object key, Object value)
        {
            key = convertKey(key);
            int hashCode = hash(key);
            int index = hashIndex(hashCode, data.Length);
            HashEntry entry = data[index];
            while (entry != null)
            {
                if (entry.hashCode == hashCode && isEqualKey(key, entry.key))
                {
                    Object oldValue = entry.getValue();
                    updateEntry(entry, value);
                    return oldValue;
                }
                entry = entry.nextJ;
            }

            addMapping(index, hashCode, key, value);
            return null;
        }

        /**
         * Puts all the values from the specified map into this map.
         * <p>
         * This implementation iterates around the specified map and
         * uses {@link #put(Object, Object)}.
         * 
         * @param map  the map to add
         * @throws NullPointerException if the map is null
         */
        public override void putAll(java.util.Map<Object, Object> map)
        {
            int mapSize = map.size();
            if (mapSize == 0)
            {
                return;
            }
            int newSize = (int)((sizeJ + mapSize) / loadFactor + 1);
            ensureCapacity(calculateNewCapacity(newSize));
            for (java.util.Iterator<java.util.MapNS.Entry<Object, Object>> it = map.entrySet().iterator(); it.hasNext(); )
            {
                java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)it.next();
                put(entry.getKey(), entry.getValue());
            }
        }

        /**
         * Removes the specified mapping from this map.
         * 
         * @param key  the mapping to remove
         * @return the value mapped to the removed key, null if key not in map
         */
        public override Object remove(Object key)
        {
            key = convertKey(key);
            int hashCode = hash(key);
            int index = hashIndex(hashCode, data.Length);
            HashEntry entry = data[index];
            HashEntry previous = null;
            while (entry != null)
            {
                if (entry.hashCode == hashCode && isEqualKey(key, entry.key))
                {
                    Object oldValue = entry.getValue();
                    removeMapping(entry, index, previous);
                    return oldValue;
                }
                previous = entry;
                entry = entry.nextJ;
            }
            return null;
        }

        /**
         * Clears the map, resetting the size to zero and nullifying references
         * to avoid garbage collection issues.
         */
        public override void clear()
        {
            modCount++;
            HashEntry[] data = this.data;
            for (int i = data.Length - 1; i >= 0; i--)
            {
                data[i] = null;
            }
            sizeJ = 0;
        }

        //-----------------------------------------------------------------------
        /**
         * Converts input keys to another object for storage in the map.
         * This implementation masks nulls.
         * Subclasses can override this to perform alternate key conversions.
         * <p>
         * The reverse conversion can be changed, if required, by overriding the
         * getKey() method in the hash entry.
         * 
         * @param key  the key convert
         * @return the converted key
         */
        protected virtual Object convertKey(Object key)
        {
            return (key == null ? NULL : key);
        }

        /**
         * Gets the hash code for the key specified.
         * This implementation uses the additional hashing routine from JDK1.4.
         * Subclasses can override this to return alternate hash codes.
         * 
         * @param key  the key to get a hash code for
         * @return the hash code
         */
        protected virtual int hash(Object key)
        {
            // same as JDK 1.4
            int h = key.GetHashCode();
            h += ~(h << 9);
            h ^= java.dotnet.lang.Operator.shiftRightUnsignet(h, 14);
            h += (h << 4);
            h ^= java.dotnet.lang.Operator.shiftRightUnsignet(h, 10);
            return h;
        }

        /**
         * Compares two keys, in internal converted form, to see if they are equal.
         * This implementation uses the equals method and assumes neither key is null.
         * Subclasses can override this to match differently.
         * 
         * @param key1  the first key to compare passed in from outside
         * @param key2  the second key extracted from the entry via <code>entry.key</code>
         * @return true if equal
         */
        protected virtual bool isEqualKey(Object key1, Object key2)
        {
            return (key1 == key2 || key1.equals(key2));
        }

        /**
         * Compares two values, in external form, to see if they are equal.
         * This implementation uses the equals method and assumes neither value is null.
         * Subclasses can override this to match differently.
         * 
         * @param value1  the first value to compare passed in from outside
         * @param value2  the second value extracted from the entry via <code>getValue()</code>
         * @return true if equal
         */
        protected virtual bool isEqualValue(Object value1, Object value2)
        {
            return (value1 == value2 || value1.equals(value2));
        }

        /**
         * Gets the index into the data storage for the hashCode specified.
         * This implementation uses the least significant bits of the hashCode.
         * Subclasses can override this to return alternate bucketing.
         * 
         * @param hashCode  the hash code to use
         * @param dataSize  the size of the data to pick a bucket from
         * @return the bucket index
         */
        protected internal virtual int hashIndex(int hashCode, int dataSize)
        {
            return hashCode & (dataSize - 1);
        }

        //-----------------------------------------------------------------------
        /**
         * Gets the entry mapped to the key specified.
         * <p>
         * This method exists for subclasses that may need to perform a multi-step
         * process accessing the entry. The public methods in this class don't use this
         * method to gain a small performance boost.
         * 
         * @param key  the key
         * @return the entry, null if no match
         */
        protected internal virtual HashEntry getEntry(Object key)
        {
            key = convertKey(key);
            int hashCode = hash(key);
            HashEntry entry = data[hashIndex(hashCode, data.Length)]; // no local for hash index
            while (entry != null)
            {
                if (entry.hashCode == hashCode && isEqualKey(key, entry.key))
                {
                    return entry;
                }
                entry = entry.nextJ;
            }
            return null;
        }

        //-----------------------------------------------------------------------
        /**
         * Updates an existing key-value mapping to change the value.
         * <p>
         * This implementation calls <code>setValue()</code> on the entry.
         * Subclasses could override to handle changes to the map.
         * 
         * @param entry  the entry to update
         * @param newValue  the new value to store
         */
        protected internal virtual void updateEntry(HashEntry entry, Object newValue)
        {
            entry.setValue(newValue);
        }

        /**
         * Reuses an existing key-value mapping, storing completely new data.
         * <p>
         * This implementation sets all the data fields on the entry.
         * Subclasses could populate additional entry fields.
         * 
         * @param entry  the entry to update, not null
         * @param hashIndex  the index in the data array
         * @param hashCode  the hash code of the key to add
         * @param key  the key to add
         * @param value  the value to add
         */
        protected virtual void reuseEntry(HashEntry entry, int hashIndex, int hashCode, Object key, Object value)
        {
            entry.nextJ = data[hashIndex];
            entry.hashCode = hashCode;
            entry.key = key;
            entry.value = value;
        }

        //-----------------------------------------------------------------------
        /**
         * Adds a new key-value mapping into this map.
         * <p>
         * This implementation calls <code>createEntry()</code>, <code>addEntry()</code>
         * and <code>checkCapacity()</code>.
         * It also handles changes to <code>modCount</code> and <code>size</code>.
         * Subclasses could override to fully control adds to the map.
         * 
         * @param hashIndex  the index into the data array to store at
         * @param hashCode  the hash code of the key to add
         * @param key  the key to add
         * @param value  the value to add
         */
        protected internal virtual void addMapping(int hashIndex, int hashCode, Object key, Object value)
        {
            modCount++;
            HashEntry entry = createEntry(data[hashIndex], hashCode, key, value);
            addEntry(entry, hashIndex);
            sizeJ++;
            checkCapacity();
        }

        /**
         * Creates an entry to store the key-value data.
         * <p>
         * This implementation creates a new HashEntry instance.
         * Subclasses can override this to return a different storage class,
         * or implement caching.
         * 
         * @param next  the next entry in sequence
         * @param hashCode  the hash code to use
         * @param key  the key to store
         * @param value  the value to store
         * @return the newly created entry
         */
        protected virtual HashEntry createEntry(HashEntry next, int hashCode, Object key, Object value)
        {
            return new HashEntry(next, hashCode, key, value);
        }

        /**
         * Adds an entry into this map.
         * <p>
         * This implementation adds the entry to the data storage table.
         * Subclasses could override to handle changes to the map.
         *
         * @param entry  the entry to add
         * @param hashIndex  the index into the data array to store at
         */
        protected virtual void addEntry(HashEntry entry, int hashIndex)
        {
            data[hashIndex] = entry;
        }

        //-----------------------------------------------------------------------
        /**
         * Removes a mapping from the map.
         * <p>
         * This implementation calls <code>removeEntry()</code> and <code>destroyEntry()</code>.
         * It also handles changes to <code>modCount</code> and <code>size</code>.
         * Subclasses could override to fully control removals from the map.
         * 
         * @param entry  the entry to remove
         * @param hashIndex  the index into the data structure
         * @param previous  the previous entry in the chain
         */
        protected internal virtual void removeMapping(HashEntry entry, int hashIndex, HashEntry previous)
        {
            modCount++;
            removeEntry(entry, hashIndex, previous);
            sizeJ--;
            destroyEntry(entry);
        }

        /**
         * Removes an entry from the chain stored in a particular index.
         * <p>
         * This implementation removes the entry from the data storage table.
         * The size is not updated.
         * Subclasses could override to handle changes to the map.
         * 
         * @param entry  the entry to remove
         * @param hashIndex  the index into the data structure
         * @param previous  the previous entry in the chain
         */
        protected virtual void removeEntry(HashEntry entry, int hashIndex, HashEntry previous)
        {
            if (previous == null)
            {
                data[hashIndex] = entry.nextJ;
            }
            else
            {
                previous.nextJ = entry.nextJ;
            }
        }

        /**
         * Kills an entry ready for the garbage collector.
         * <p>
         * This implementation prepares the HashEntry for garbage collection.
         * Subclasses can override this to implement caching (override clear as well).
         * 
         * @param entry  the entry to destroy
         */
        protected virtual void destroyEntry(HashEntry entry)
        {
            entry.nextJ = null;
            entry.key = null;
            entry.value = null;
        }

        //-----------------------------------------------------------------------
        /**
         * Checks the capacity of the map and enlarges it if necessary.
         * <p>
         * This implementation uses the threshold to check if the map needs enlarging
         */
        protected virtual void checkCapacity()
        {
            if (sizeJ >= threshold)
            {
                int newCapacity = data.Length * 2;
                if (newCapacity <= MAXIMUM_CAPACITY)
                {
                    ensureCapacity(newCapacity);
                }
            }
        }

        /**
         * Changes the size of the data structure to the capacity proposed.
         * 
         * @param newCapacity  the new capacity of the array (a power of two, less or equal to max)
         */
        protected virtual void ensureCapacity(int newCapacity)
        {
            int oldCapacity = data.Length;
            if (newCapacity <= oldCapacity)
            {
                return;
            }
            if (sizeJ == 0)
            {
                threshold = calculateThreshold(newCapacity, loadFactor);
                data = new HashEntry[newCapacity];
            }
            else
            {
                HashEntry[] oldEntries = data;
                HashEntry[] newEntries = new HashEntry[newCapacity];

                modCount++;
                for (int i = oldCapacity - 1; i >= 0; i--)
                {
                    HashEntry entry = oldEntries[i];
                    if (entry != null)
                    {
                        oldEntries[i] = null;  // gc
                        do
                        {
                            HashEntry next = entry.nextJ;
                            int index = hashIndex(entry.hashCode, newCapacity);
                            entry.nextJ = newEntries[index];
                            newEntries[index] = entry;
                            entry = next;
                        } while (entry != null);
                    }
                }
                threshold = calculateThreshold(newCapacity, loadFactor);
                data = newEntries;
            }
        }

        /**
         * Calculates the new capacity of the map.
         * This implementation normalizes the capacity to a power of two.
         * 
         * @param proposedCapacity  the proposed capacity
         * @return the normalized new capacity
         */
        protected virtual int calculateNewCapacity(int proposedCapacity)
        {
            int newCapacity = 1;
            if (proposedCapacity > MAXIMUM_CAPACITY)
            {
                newCapacity = MAXIMUM_CAPACITY;
            }
            else
            {
                while (newCapacity < proposedCapacity)
                {
                    newCapacity <<= 1;  // multiply by two
                }
                if (newCapacity > MAXIMUM_CAPACITY)
                {
                    newCapacity = MAXIMUM_CAPACITY;
                }
            }
            return newCapacity;
        }

        /**
         * Calculates the new threshold of the map, where it will be resized.
         * This implementation uses the load factor.
         * 
         * @param newCapacity  the new capacity
         * @param factor  the load factor
         * @return the new resize threshold
         */
        protected virtual int calculateThreshold(int newCapacity, float factor)
        {
            return (int)(newCapacity * factor);
        }

        //-----------------------------------------------------------------------
        /**
         * Gets the <code>next</code> field from a <code>HashEntry</code>.
         * Used in subclasses that have no visibility of the field.
         * 
         * @param entry  the entry to query, must not be null
         * @return the <code>next</code> field of the entry
         * @throws NullPointerException if the entry is null
         * @since Commons Collections 3.1
         */
        protected virtual HashEntry entryNext(HashEntry entry)
        {
            return entry.nextJ;
        }

        /**
         * Gets the <code>hashCode</code> field from a <code>HashEntry</code>.
         * Used in subclasses that have no visibility of the field.
         * 
         * @param entry  the entry to query, must not be null
         * @return the <code>hashCode</code> field of the entry
         * @throws NullPointerException if the entry is null
         * @since Commons Collections 3.1
         */
        protected virtual int entryHashCode(HashEntry entry)
        {
            return entry.hashCode;
        }

        /**
         * Gets the <code>key</code> field from a <code>HashEntry</code>.
         * Used in subclasses that have no visibility of the field.
         * 
         * @param entry  the entry to query, must not be null
         * @return the <code>key</code> field of the entry
         * @throws NullPointerException if the entry is null
         * @since Commons Collections 3.1
         */
        protected virtual Object entryKey(HashEntry entry)
        {
            return entry.key;
        }

        /**
         * Gets the <code>value</code> field from a <code>HashEntry</code>.
         * Used in subclasses that have no visibility of the field.
         * 
         * @param entry  the entry to query, must not be null
         * @return the <code>value</code> field of the entry
         * @throws NullPointerException if the entry is null
         * @since Commons Collections 3.1
         */
        protected virtual Object entryValue(HashEntry entry)
        {
            return entry.value;
        }

        //-----------------------------------------------------------------------
        /**
         * Gets an iterator over the map.
         * Changes made to the iterator affect this map.
         * <p>
         * A MapIterator returns the keys in the map. It also provides convenient
         * methods to get the key and value, and set the value.
         * It avoids the need to create an entrySet/keySet/values object.
         * It also avoids creating the Map.Entry object.
         * 
         * @return the map iterator
         */
        public virtual MapIterator mapIterator()
        {
            if (sizeJ == 0)
            {
                return EmptyMapIterator.INSTANCE;
            }
            return new HashMapIterator(this);
        }


        //-----------------------------------------------------------------------    
        /**
         * Gets the entrySet view of the map.
         * Changes made to the view affect this map.
         * To simply iterate through the entries, use {@link #mapIterator()}.
         * 
         * @return the entrySet view
         */
        public virtual new java.util.Set<Object> entrySet()
        {
            if (entrySetJ == null)
            {
                entrySetJ = new EntrySet(this);
            }
            return entrySetJ;
        }

        /**
         * Creates an entry set iterator.
         * Subclasses can override this to return iterators with different properties.
         * 
         * @return the entrySet iterator
         */
        protected internal virtual java.util.Iterator<Object> createEntrySetIterator()
        {
            if (size() == 0)
            {
                return EmptyIterator.INSTANCE;
            }
            return new EntrySetIterator(this);
        }

        //-----------------------------------------------------------------------    
        /**
         * Gets the keySet view of the map.
         * Changes made to the view affect this map.
         * To simply iterate through the keys, use {@link #mapIterator()}.
         * 
         * @return the keySet view
         */
        public override java.util.Set<Object> keySet()
        {
            if (keySetJ == null)
            {
                keySetJ = new KeySet(this);
            }
            return keySetJ;
        }

        /**
         * Creates a key set iterator.
         * Subclasses can override this to return iterators with different properties.
         * 
         * @return the keySet iterator
         */
        protected internal virtual java.util.Iterator<Object> createKeySetIterator()
        {
            if (size() == 0)
            {
                return EmptyIterator.INSTANCE;
            }
            return new KeySetIterator(this);
        }



        //-----------------------------------------------------------------------    
        /**
         * Gets the values view of the map.
         * Changes made to the view affect this map.
         * To simply iterate through the values, use {@link #mapIterator()}.
         * 
         * @return the values view
         */
        public override java.util.Collection<Object> values()
        {
            if (valuesJ == null)
            {
                valuesJ = new Values(this);
            }
            return valuesJ;
        }

        /**
         * Creates a values iterator.
         * Subclasses can override this to return iterators with different properties.
         * 
         * @return the values iterator
         */
        protected internal virtual java.util.Iterator<Object> createValuesIterator()
        {
            if (size() == 0)
            {
                return EmptyIterator.INSTANCE;
            }
            return new ValuesIterator(this);
        }



        //-----------------------------------------------------------------------
        /**
         * Writes the map data to the stream. This method must be overridden if a
         * subclass must be setup before <code>put()</code> is used.
         * <p>
         * Serialization is not one of the JDK's nicest topics. Normal serialization will
         * initialise the superclass before the subclass. Sometimes however, this isn't
         * what you want, as in this case the <code>put()</code> method on read can be
         * affected by subclass state.
         * <p>
         * The solution adopted here is to serialize the state data of this class in
         * this protected method. This method must be called by the
         * <code>writeObject()</code> of the first java.io.Serializable subclass.
         * <p>
         * Subclasses may override if they have a specific field that must be present
         * on read before this implementation will work. Generally, the read determines
         * what must be serialized here, if anything.
         * 
         * @param out  the output stream
         */
        protected virtual void doWriteObject(java.io.ObjectOutputStream outJ)
        {//throws IOException {
            outJ.writeFloat(loadFactor);
            outJ.writeInt(data.Length);
            outJ.writeInt(sizeJ);
            for (MapIterator it = mapIterator(); it.hasNext(); )
            {
                outJ.writeObject(it.next());
                outJ.writeObject(it.getValue());
            }
        }

        /**
         * Reads the map data from the stream. This method must be overridden if a
         * subclass must be setup before <code>put()</code> is used.
         * <p>
         * Serialization is not one of the JDK's nicest topics. Normal serialization will
         * initialise the superclass before the subclass. Sometimes however, this isn't
         * what you want, as in this case the <code>put()</code> method on read can be
         * affected by subclass state.
         * <p>
         * The solution adopted here is to deserialize the state data of this class in
         * this protected method. This method must be called by the
         * <code>readObject()</code> of the first java.io.Serializable subclass.
         * <p>
         * Subclasses may override if the subclass has a specific field that must be present
         * before <code>put()</code> or <code>calculateThreshold()</code> will work correctly.
         * 
         * @param in  the input stream
         */
        protected virtual void doReadObject(java.io.ObjectInputStream inJ)
        {// throws IOException, ClassNotFoundException {
            loadFactor = inJ.readFloat();
            int capacity = inJ.readInt();
            int size = inJ.readInt();
            init();
            threshold = calculateThreshold(capacity, loadFactor);
            data = new HashEntry[capacity];
            for (int i = 0; i < size; i++)
            {
                Object key = inJ.readObject();
                Object value = inJ.readObject();
                put(key, value);
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Clones the map without cloning the keys or values.
         * <p>
         * To implement <code>clone()</code>, a subclass must implement the
         * <code>Cloneable</code> interface and make this method public.
         *
         * @return a shallow clone
         */
        protected virtual Object clone()
        {
            try
            {
                AbstractHashedMap cloned = (AbstractHashedMap)MemberwiseClone();
                cloned.data = new HashEntry[data.Length];
                cloned.entrySetJ = null;
                cloned.keySetJ = null;
                cloned.valuesJ = null;
                cloned.modCount = 0;
                cloned.sizeJ = 0;
                cloned.init();
                cloned.putAll(this);
                return cloned;

            }
            catch (java.lang.CloneNotSupportedException ex)
            {
                ex.getClass();
                return null;  // should never happen
            }
        }

        /**
         * Compares this map with another.
         * 
         * @param obj  the object to compare to
         * @return true if equal
         */
        public override bool Equals(Object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (obj is java.util.Map<Object, Object> == false)
            {
                return false;
            }
            java.util.Map<Object, Object> map = (java.util.Map<Object, Object>)obj;
            if (map.size() != size())
            {
                return false;
            }
            MapIterator it = mapIterator();
            try
            {
                while (it.hasNext())
                {
                    Object key = it.next();
                    Object value = it.getValue();
                    if (value == null)
                    {
                        if (map.get(key) != null || map.containsKey(key) == false)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (value.equals(map.get(key)) == false)
                        {
                            return false;
                        }
                    }
                }
            }
            catch (java.lang.ClassCastException ignored)
            {
                ignored.getClass();
                return false;
            }
            catch (java.lang.NullPointerException ignored)
            {
                ignored.getClass();
                return false;
            }
            return true;
        }

        /**
         * Gets the standard Map hashCode.
         * 
         * @return the hash code defined in the Map interface
         */
        public override int GetHashCode()
        {
            int total = 0;
            java.util.Iterator<Object> it = createEntrySetIterator();
            while (it.hasNext())
            {
                total += it.next().GetHashCode();
            }
            return total;
        }

        /**
         * Gets the map as a String.
         * 
         * @return a string version of the map
         */
        public override String ToString()
        {
            if (size() == 0)
            {
                return "{}";
            }
            java.lang.StringBuffer buf = new java.lang.StringBuffer(32 * size());
            buf.append('{');

            MapIterator it = mapIterator();
            bool hasNext = it.hasNext();
            while (hasNext)
            {
                Object key = it.next();
                Object value = it.getValue();
                buf.append(key == this ? "(this Map)" : key)
                   .append('=')
                   .append(value == this ? "(this Map)" : value);

                hasNext = it.hasNext();
                if (hasNext)
                {
                    buf.append(',').append(' ');
                }
            }

            buf.append('}');
            return buf.toString();
        }
        //-----------------------------------------------------------------------
        /**
         * HashEntry used to store the data.
         * <p>
         * If you subclass <code>AbstractHashedMap</code> but not <code>HashEntry</code>
         * then you will not be able to access the protected fields.
         * The <code>entryXxx()</code> methods on <code>AbstractHashedMap</code> exist
         * to provide the necessary access.
         */
        public class HashEntry : java.util.MapNS.Entry<Object, Object>, KeyValue
        {
            /** The next entry in the hash chain */
            protected internal HashEntry nextJ;
            /** The hash code of the key */
            protected internal int hashCode;
            /** The key */
            protected internal Object key;
            /** The value */
            protected internal Object value;

            protected internal HashEntry(HashEntry next, int hashCode, Object key, Object value)
                : base()
            {
                this.nextJ = next;
                this.hashCode = hashCode;
                this.key = key;
                this.value = value;
            }

            public virtual Object getKey()
            {
                return (key == AbstractHashedMap.NULL ? null : key);
            }

            public virtual Object getValue()
            {
                return value;
            }

            public virtual Object setValue(Object value)
            {
                Object old = this.value;
                this.value = value;
                return old;
            }

            public override bool Equals(Object obj)
            {
                if (obj == this)
                {
                    return true;
                }
                if (obj is java.util.MapNS.Entry<Object, Object> == false)
                {
                    return false;
                }
                java.util.MapNS.Entry<Object, Object> other = (java.util.MapNS.Entry<Object, Object>)obj;
                return
                    (getKey() == null ? other.getKey() == null : getKey().equals(other.getKey())) &&
                    (getValue() == null ? other.getValue() == null : getValue().equals(other.getValue()));
            }

            public override int GetHashCode()
            {
                return (getKey() == null ? 0 : getKey().GetHashCode()) ^
                       (getValue() == null ? 0 : getValue().GetHashCode());
            }

            public override String ToString()
            {
                return new java.lang.StringBuffer().append(getKey()).append('=').append(getValue()).toString();
            }
        }


        /**
         * MapIterator implementation.
         */
        protected internal class HashMapIterator : HashIterator, MapIterator
        {

            protected internal HashMapIterator(AbstractHashedMap parent)
                : base(parent)
            {
            }

            public override Object next()
            {
                return base.nextEntry().getKey();
            }

            public virtual Object getKey()
            {
                HashEntry current = currentEntry();
                if (current == null)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.GETKEY_INVALID);
                }
                return current.getKey();
            }

            public virtual Object getValue()
            {
                HashEntry current = currentEntry();
                if (current == null)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.GETVALUE_INVALID);
                }
                return current.getValue();
            }

            public virtual Object setValue(Object value)
            {
                HashEntry current = currentEntry();
                if (current == null)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.SETVALUE_INVALID);
                }
                return current.setValue(value);
            }
        }
        /**
         * Values iterator.
         */
        protected internal class ValuesIterator : HashIterator
        {

            protected internal ValuesIterator(AbstractHashedMap parent)
                : base(parent)
            {
            }

            public override Object next()
            {
                return base.nextEntry().getValue();
            }
        }

        /**
         * KeySet iterator.
         */
        protected internal class KeySetIterator : EntrySetIterator
        {

            protected internal KeySetIterator(AbstractHashedMap parent)
                : base(parent)
            {
            }

            public override Object next()
            {
                return base.nextEntry().getKey();
            }
        }

        /**
         * Base Iterator
         */
        public abstract class HashIterator : java.util.Iterator<Object>
        {
            public abstract Object next();

            /** The parent map */
            protected readonly AbstractHashedMap parent;
            /** The current index into the array of buckets */
            protected int hashIndex;
            /** The last returned entry */
            protected HashEntry last;
            /** The next entry */
            protected HashEntry nextJ;
            /** The modification count expected */
            protected int expectedModCount;

            protected HashIterator(AbstractHashedMap parent)
                : base()
            {
                this.parent = parent;
                HashEntry[] data = parent.data;
                int i = data.Length;
                HashEntry next = null;
                while (i > 0 && next == null)
                {
                    next = data[--i];
                }
                this.nextJ = next;
                this.hashIndex = i;
                this.expectedModCount = parent.modCount;
            }

            public virtual bool hasNext()
            {
                return (nextJ != null);
            }

            protected virtual HashEntry nextEntry()
            {
                if (parent.modCount != expectedModCount)
                {
                    throw new java.util.ConcurrentModificationException();
                }
                HashEntry newCurrent = nextJ;
                if (newCurrent == null)
                {
                    throw new java.util.NoSuchElementException(AbstractHashedMap.NO_NEXT_ENTRY);
                }
                HashEntry[] data = parent.data;
                int i = hashIndex;
                HashEntry n = newCurrent.nextJ;
                while (n == null && i > 0)
                {
                    n = data[--i];
                }
                nextJ = n;
                hashIndex = i;
                last = newCurrent;
                return newCurrent;
            }

            protected virtual HashEntry currentEntry()
            {
                return last;
            }

            public virtual void remove()
            {
                if (last == null)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.REMOVE_INVALID);
                }
                if (parent.modCount != expectedModCount)
                {
                    throw new java.util.ConcurrentModificationException();
                }
                parent.remove(last.getKey());
                last = null;
                expectedModCount = parent.modCount;
            }

            public override String ToString()
            {
                if (last != null)
                {
                    return "Iterator[" + last.getKey() + "=" + last.getValue() + "]";
                }
                else
                {
                    return "Iterator[]";
                }
            }
        }
        /**
         * Values implementation.
         */
        protected internal new class Values : java.util.AbstractCollection<Object>
        {
            /** The parent map */
            protected readonly AbstractHashedMap parent;

            protected internal Values(AbstractHashedMap parent)
                : base()
            {
                this.parent = parent;
            }

            public override int size()
            {
                return parent.size();
            }

            public override void clear()
            {
                parent.clear();
            }

            public override bool contains(Object value)
            {
                return parent.containsValue(value);
            }

            public override java.util.Iterator<Object> iterator()
            {
                return parent.createValuesIterator();
            }
        }
        /**
         * KeySet implementation.
         */
        public class KeySet : java.util.AbstractSet<Object>
        {
            /** The parent map */
            protected readonly AbstractHashedMap parent;

            protected internal KeySet(AbstractHashedMap parent)
                : base()
            {
                this.parent = parent;
            }

            public override int size()
            {
                return parent.size();
            }

            public override void clear()
            {
                parent.clear();
            }

            public override bool contains(Object key)
            {
                return parent.containsKey(key);
            }

            public override bool remove(Object key)
            {
                bool result = parent.containsKey(key);
                parent.remove(key);
                return result;
            }

            public override java.util.Iterator<Object> iterator()
            {
                return parent.createKeySetIterator();
            }
        }

        /**
         * EntrySet implementation.
         */
        protected internal class EntrySet : java.util.AbstractSet<Object>
        {
            /** The parent map */
            protected readonly AbstractHashedMap parent2;

            protected internal EntrySet(AbstractHashedMap parent)
                : base()
            {
                this.parent2 = parent;
            }

            public override int size()
            {
                return parent2.size();
            }

            public override void clear()
            {
                parent2.clear();
            }

            public override bool contains(Object entry)
            {
                if (entry is java.util.MapNS.Entry<Object, Object>)
                {
                    java.util.MapNS.Entry<Object, Object> e = (java.util.MapNS.Entry<Object, Object>)entry;
                    HashEntry match = parent2.getEntry(e.getKey());
                    return (match != null && match.equals(e));
                }
                return false;
            }

            public override bool remove(Object obj)
            {
                if (obj is java.util.MapNS.Entry<Object, Object> == false)
                {
                    return false;
                }
                if (contains(obj) == false)
                {
                    return false;
                }
                java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)obj;
                Object key = entry.getKey();
                parent2.remove(key);
                return true;
            }

            public override java.util.Iterator<Object> iterator()
            {
                return parent2.createEntrySetIterator();
            }
        }

        /**
         * EntrySet iterator.
         */
        protected internal class EntrySetIterator : HashIterator
        {

            protected internal EntrySetIterator(AbstractHashedMap parent)
                : base(parent)
            {
            }

            public override Object next()
            {
                return base.nextEntry();
            }
        }
    }
}

