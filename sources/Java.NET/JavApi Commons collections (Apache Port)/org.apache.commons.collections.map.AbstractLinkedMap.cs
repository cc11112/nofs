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
     * An abstract implementation of a hash-based map that links entries to create an
     * ordered map and which provides numerous points for subclasses to override.
     * <p>
     * This class implements all the features necessary for a subclass linked
     * hash-based map. Key-value entries are stored in instances of the
     * <code>LinkEntry</code> class which can be overridden and replaced.
     * The iterators can similarly be replaced, without the need to replace the KeySet,
     * EntrySet and Values view classes.
     * <p>
     * Overridable methods are provided to change the default hashing behaviour, and
     * to change how entries are added to and removed from the map. Hopefully, all you
     * need for unusual subclasses is here.
     * <p>
     * This implementation maintains order by original insertion, but subclasses
     * may work differently. The <code>OrderedMap</code> interface is implemented
     * to provide access to bidirectional iteration and extra convenience methods.
     * <p>
     * The <code>orderedMapIterator()</code> method provides direct access to a
     * bidirectional iterator. The iterators from the other views can also be cast
     * to <code>OrderedIterator</code> if required.
     * <p>
     * All the available iterators can be reset back to the start by casting to
     * <code>ResettableIterator</code> and calling <code>reset()</code>.
     * <p>
     * The implementation is also designed to be subclassed, with lots of useful
     * methods exposed.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author java util LinkedHashMap
     * @author Stephen Colebourne
     */
    [Serializable]
    public class AbstractLinkedMap : AbstractHashedMap, OrderedMap
    {

        /** Header in the linked list */
        [NonSerialized]
        protected LinkEntry header;

        /**
         * Constructor only used in deserialization, do not use otherwise.
         */
        protected AbstractLinkedMap()
            : base()
        {
        }

        /**
         * Constructor which performs no validation on the passed in parameters.
         * 
         * @param initialCapacity  the initial capacity, must be a power of two
         * @param loadFactor  the load factor, must be > 0.0f and generally < 1.0f
         * @param threshold  the threshold, must be sensible
         */
        protected AbstractLinkedMap(int initialCapacity, float loadFactor, int threshold)
            : base(initialCapacity, loadFactor, threshold)
        {
        }

        /**
         * Constructs a new, empty map with the specified initial capacity. 
         *
         * @param initialCapacity  the initial capacity
         * @throws IllegalArgumentException if the initial capacity is less than one
         */
        protected AbstractLinkedMap(int initialCapacity)
            : base(initialCapacity)
        {
        }

        /**
         * Constructs a new, empty map with the specified initial capacity and
         * load factor. 
         *
         * @param initialCapacity  the initial capacity
         * @param loadFactor  the load factor
         * @throws IllegalArgumentException if the initial capacity is less than one
         * @throws IllegalArgumentException if the load factor is less than zero
         */
        protected AbstractLinkedMap(int initialCapacity, float loadFactor)
            : base(initialCapacity, loadFactor)
        {
        }

        /**
         * Constructor copying elements from another map.
         *
         * @param map  the map to copy
         * @throws NullPointerException if the map is null
         */
        protected AbstractLinkedMap(java.util.Map<Object, Object> map)
            : base(map)
        {
        }

        /**
         * Initialise this subclass during construction.
         * <p>
         * NOTE: As from v3.2 this method calls
         * {@link #createEntry(HashEntry, int, Object, Object)} to create
         * the map entry object.
         */
        protected override void init()
        {
            header = (LinkEntry)createEntry(null, -1, null, null);
            header.before = header.after = header;
        }

        //-----------------------------------------------------------------------
        /**
         * Checks whether the map contains the specified value.
         * 
         * @param value  the value to search for
         * @return true if the map contains the value
         */
        public override bool containsValue(Object value)
        {
            // override uses faster iterator
            if (value == null)
            {
                for (LinkEntry entry = header.after; entry != header; entry = entry.after)
                {
                    if (entry.getValue() == null)
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (LinkEntry entry = header.after; entry != header; entry = entry.after)
                {
                    if (isEqualValue(value, entry.getValue()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /**
         * Clears the map, resetting the size to zero and nullifying references
         * to avoid garbage collection issues.
         */
        public override void clear()
        {
            // override to reset the linked list
            base.clear();
            header.before = header.after = header;
        }

        //-----------------------------------------------------------------------
        /**
         * Gets the first key in the map, which is the most recently inserted.
         * 
         * @return the most recently inserted key
         */
        public virtual Object firstKey()
        {
            if (sizeJ == 0)
            {
                throw new java.util.NoSuchElementException("Map is empty");
            }
            return header.after.getKey();
        }

        /**
         * Gets the last key in the map, which is the first inserted.
         * 
         * @return the eldest key
         */
        public virtual Object lastKey()
        {
            if (sizeJ == 0)
            {
                throw new java.util.NoSuchElementException("Map is empty");
            }
            return header.before.getKey();
        }

        /**
         * Gets the next key in sequence.
         * 
         * @param key  the key to get after
         * @return the next key
         */
        public virtual Object nextKey(Object key)
        {
            LinkEntry entry = (LinkEntry)getEntry(key);
            return (entry == null || entry.after == header ? null : entry.after.getKey());
        }

        /**
         * Gets the previous key in sequence.
         * 
         * @param key  the key to get before
         * @return the previous key
         */
        public virtual Object previousKey(Object key)
        {
            LinkEntry entry = (LinkEntry)getEntry(key);
            return (entry == null || entry.before == header ? null : entry.before.getKey());
        }

        //-----------------------------------------------------------------------    
        /**
         * Gets the key at the specified index.
         * 
         * @param index  the index to retrieve
         * @return the key at the specified index
         * @throws IndexOutOfBoundsException if the index is invalid
         */
        protected virtual LinkEntry getEntry(int indexJ)
        {
            if (indexJ < 0)
            {
                throw new java.lang.IndexOutOfBoundsException("Index " + indexJ + " is less than zero");
            }
            if (indexJ >= sizeJ)
            {
                throw new java.lang.IndexOutOfBoundsException("Index " + indexJ + " is invalid for size " + sizeJ);
            }
            LinkEntry entry;
            if (indexJ < (sizeJ / 2))
            {
                // Search forwards
                entry = header.after;
                for (int currentIndex = 0; currentIndex < indexJ; currentIndex++)
                {
                    entry = entry.after;
                }
            }
            else
            {
                // Search backwards
                entry = header;
                for (int currentIndex = sizeJ; currentIndex > indexJ; currentIndex--)
                {
                    entry = entry.before;
                }
            }
            return entry;
        }

        /**
         * Adds an entry into this map, maintaining insertion order.
         * <p>
         * This implementation adds the entry to the data storage table and
         * to the end of the linked list.
         * 
         * @param entry  the entry to add
         * @param hashIndex  the index into the data array to store at
         */
        protected override void addEntry(HashEntry entry, int hashIndex)
        {
            LinkEntry link = (LinkEntry)entry;
            link.after = header;
            link.before = header.before;
            header.before.after = link;
            header.before = link;
            data[hashIndex] = entry;
        }

        /**
         * Creates an entry to store the data.
         * <p>
         * This implementation creates a new LinkEntry instance.
         * 
         * @param next  the next entry in sequence
         * @param hashCode  the hash code to use
         * @param key  the key to store
         * @param value  the value to store
         * @return the newly created entry
         */
        protected override HashEntry createEntry(HashEntry next, int hashCode, Object key, Object value)
        {
            return new LinkEntry(next, hashCode, key, value);
        }

        /**
         * Removes an entry from the map and the linked list.
         * <p>
         * This implementation removes the entry from the linked list chain, then
         * calls the superclass implementation.
         * 
         * @param entry  the entry to remove
         * @param hashIndex  the index into the data structure
         * @param previous  the previous entry in the chain
         */
        protected override void removeEntry(HashEntry entry, int hashIndex, HashEntry previous)
        {
            LinkEntry link = (LinkEntry)entry;
            link.before.after = link.after;
            link.after.before = link.before;
            link.after = null;
            link.before = null;
            base.removeEntry(entry, hashIndex, previous);
        }

        //-----------------------------------------------------------------------
        /**
         * Gets the <code>before</code> field from a <code>LinkEntry</code>.
         * Used in subclasses that have no visibility of the field.
         * 
         * @param entry  the entry to query, must not be null
         * @return the <code>before</code> field of the entry
         * @throws NullPointerException if the entry is null
         * @since Commons Collections 3.1
         */
        protected virtual LinkEntry entryBefore(LinkEntry entry)
        {
            return entry.before;
        }

        /**
         * Gets the <code>after</code> field from a <code>LinkEntry</code>.
         * Used in subclasses that have no visibility of the field.
         * 
         * @param entry  the entry to query, must not be null
         * @return the <code>after</code> field of the entry
         * @throws NullPointerException if the entry is null
         * @since Commons Collections 3.1
         */
        protected virtual LinkEntry entryAfter(LinkEntry entry)
        {
            return entry.after;
        }

        //-----------------------------------------------------------------------
        /**
         * Gets an iterator over the map.
         * Changes made to the iterator affect this map.
         * <p>
         * A MapIterator returns the keys in the map. It also provides convenient
         * methods to get the key and value, and set the value.
         * It avoids the need to create an entrySet/keySet/values object.
         * 
         * @return the map iterator
         */
        public override MapIterator mapIterator()
        {
            if (sizeJ == 0)
            {
                return EmptyOrderedMapIterator.INSTANCE;
            }
            return new LinkMapIterator(this);
        }

        /**
         * Gets a bidirectional iterator over the map.
         * Changes made to the iterator affect this map.
         * <p>
         * A MapIterator returns the keys in the map. It also provides convenient
         * methods to get the key and value, and set the value.
         * It avoids the need to create an entrySet/keySet/values object.
         * 
         * @return the map iterator
         */
        public virtual OrderedMapIterator orderedMapIterator()
        {
            if (sizeJ == 0)
            {
                return EmptyOrderedMapIterator.INSTANCE;
            }
            return new LinkMapIterator(this);
        }

        /**
         * MapIterator implementation.
         */
        protected class LinkMapIterator : LinkIterator, OrderedMapIterator
        {

            protected internal LinkMapIterator(AbstractLinkedMap parent)
                : base(parent)
            {
            }

            public override Object next()
            {
                return base.nextEntry().getKey();
            }

            public override Object previous()
            {
                return base.previousEntry().getKey();
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

            public Object setValue(Object value)
            {
                HashEntry current = currentEntry();
                if (current == null)
                {
                    throw new java.lang.IllegalStateException(AbstractHashedMap.SETVALUE_INVALID);
                }
                return current.setValue(value);
            }
        }

        //-----------------------------------------------------------------------    
        /**
         * Creates an entry set iterator.
         * Subclasses can override this to return iterators with different properties.
         * 
         * @return the entrySet iterator
         */
        protected internal override java.util.Iterator<Object> createEntrySetIterator()
        {
            if (size() == 0)
            {
                return EmptyOrderedIterator.INSTANCE;
            }
            return new EntrySetIterator(this);
        }

        /**
         * EntrySet iterator.
         */
        protected new class EntrySetIterator : LinkIterator
        {

            protected internal EntrySetIterator(AbstractLinkedMap parent)
                : base(parent)
            {
            }

            public override Object next()
            {
                return base.nextEntry();
            }

            public override Object previous()
            {
                return base.previousEntry();
            }
        }

        //-----------------------------------------------------------------------    
        /**
         * Creates a key set iterator.
         * Subclasses can override this to return iterators with different properties.
         * 
         * @return the keySet iterator
         */
        protected internal override java.util.Iterator<Object> createKeySetIterator()
        {
            if (size() == 0)
            {
                return EmptyOrderedIterator.INSTANCE;
            }
            return new KeySetIterator(this);
        }

        /**
         * KeySet iterator.
         */
        protected new class KeySetIterator : EntrySetIterator
        {

            protected internal KeySetIterator(AbstractLinkedMap parent)
                : base(parent)
            {
            }

            public override Object next()
            {
                return base.nextEntry().getKey();
            }

            public override Object previous()
            {
                return base.previousEntry().getKey();
            }
        }

        //-----------------------------------------------------------------------    
        /**
         * Creates a values iterator.
         * Subclasses can override this to return iterators with different properties.
         * 
         * @return the values iterator
         */
        protected internal override java.util.Iterator<Object> createValuesIterator()
        {
            if (size() == 0)
            {
                return EmptyOrderedIterator.INSTANCE;
            }
            return new ValuesIterator(this);
        }

        /**
         * Values iterator.
         */
        protected new class ValuesIterator : LinkIterator
        {

            protected internal ValuesIterator(AbstractLinkedMap parent)
                : base(parent)
            {
            }

            public override Object next()
            {
                return base.nextEntry().getValue();
            }

            public override Object previous()
            {
                return base.previousEntry().getValue();
            }
        }

        //-----------------------------------------------------------------------
        /**
         * LinkEntry that stores the data.
         * <p>
         * If you subclass <code>AbstractLinkedMap</code> but not <code>LinkEntry</code>
         * then you will not be able to access the protected fields.
         * The <code>entryXxx()</code> methods on <code>AbstractLinkedMap</code> exist
         * to provide the necessary access.
         */
        protected class LinkEntry : HashEntry
        {
            /** The entry before this one in the order */
            protected internal LinkEntry before;
            /** The entry after this one in the order */
            protected internal LinkEntry after;

            /**
             * Constructs a new entry.
             * 
             * @param next  the next entry in the hash bucket sequence
             * @param hashCode  the hash code
             * @param key  the key
             * @param value  the value
             */
            protected internal LinkEntry(HashEntry next, int hashCode, Object key, Object value)
                : base(next, hashCode, key, value)
            {
            }
        }

        /**
         * Base Iterator that iterates in link order.
         */
        protected abstract class LinkIterator
                : OrderedIterator, ResettableIterator
        {

            public abstract Object previous();
            public abstract Object next();

            /** The parent map */
            protected readonly AbstractLinkedMap parent;
            /** The current (last returned) entry */
            protected LinkEntry last;
            /** The next entry */
            protected LinkEntry nextJ;
            /** The modification count expected */
            protected int expectedModCount;

            protected LinkIterator(AbstractLinkedMap parent)
                : base()
            {
                this.parent = parent;
                this.nextJ = parent.header.after;
                this.expectedModCount = parent.modCount;
            }

            public virtual bool hasNext()
            {
                return (nextJ != parent.header);
            }

            public virtual bool hasPrevious()
            {
                return (nextJ.before != parent.header);
            }

            protected virtual LinkEntry nextEntry()
            {
                if (parent.modCount != expectedModCount)
                {
                    throw new java.util.ConcurrentModificationException();
                }
                if (nextJ == parent.header)
                {
                    throw new java.util.NoSuchElementException(AbstractHashedMap.NO_NEXT_ENTRY);
                }
                last = nextJ;
                nextJ = nextJ.after;
                return last;
            }

            protected virtual LinkEntry previousEntry()
            {
                if (parent.modCount != expectedModCount)
                {
                    throw new java.util.ConcurrentModificationException();
                }
                LinkEntry previous = nextJ.before;
                if (previous == parent.header)
                {
                    throw new java.util.NoSuchElementException(AbstractHashedMap.NO_PREVIOUS_ENTRY);
                }
                nextJ = previous;
                last = previous;
                return last;
            }

            protected virtual LinkEntry currentEntry()
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

            public virtual void reset()
            {
                last = null;
                nextJ = parent.header.after;
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

    }
}