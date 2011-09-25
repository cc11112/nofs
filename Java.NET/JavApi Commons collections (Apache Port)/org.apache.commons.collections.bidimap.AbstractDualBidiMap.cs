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
using org.apache.commons.collections.collection;
using org.apache.commons.collections.iterators;
using org.apache.commons.collections.keyvalue;

namespace org.apache.commons.collections.bidimap
{

    /**
 * Abstract <code>BidiMap</code> implemented using two maps.
 * <p>
 * An implementation can be written simply by implementing the
 * <code>createMap</code> method.
 * 
 * @see DualHashBidiMap
 * @see DualTreeBidiMap
 * @since Commons Collections 3.0
 * @version $Id$
 * 
 * @author Matthew Hawthorne
 * @author Stephen Colebourne
 */
    public abstract class AbstractDualBidiMap : BidiMap
    {

        /**
         * Delegate map array.  The first map contains standard entries, and the 
         * second contains inverses.
         */
        [NonSerialized]
        protected internal readonly java.util.Map<Object, Object>[] maps = new java.util.Map<Object, Object>[2];
        /**
         * Inverse view of this map.
         */
        [NonSerialized]
        protected BidiMap inverseBidiMapJ = null;
        /**
         * View of the keys.
         */
        [NonSerialized]
        protected java.util.Set<Object> keySetJ = null;
        /**
         * View of the values.
         */
        [NonSerialized]
        protected java.util.Collection<Object> valuesJ = null;
        /**
         * View of the entries.
         */
        [NonSerialized]
        protected java.util.Set<Object> entrySetJ = null;

        /**
         * Creates an empty map, initialised by <code>createMap</code>.
         * <p>
         * This constructor remains in place for deserialization.
         * All other usage is deprecated in favour of
         * {@link #AbstractDualBidiMap(Map, Map)}.
         */
        protected AbstractDualBidiMap()
            : base()
        {
            maps[0] = createMap();
            maps[1] = createMap();
        }

        /**
         * Creates an empty map using the two maps specified as storage.
         * <p>
         * The two maps must be a matching pair, normal and reverse.
         * They will typically both be empty.
         * <p>
         * Neither map is validated, so nulls may be passed in.
         * If you choose to do this then the subclass constructor must populate
         * the <code>maps[]</code> instance variable itself.
         * 
         * @param normalMap  the normal direction map
         * @param reverseMap  the reverse direction map
         * @since Commons Collections 3.1
         */
        protected AbstractDualBidiMap(java.util.Map<Object, Object> normalMap, java.util.Map<Object, Object> reverseMap)
            : base()
        {
            maps[0] = normalMap;
            maps[1] = reverseMap;
        }

        /** 
         * Constructs a map that decorates the specified maps,
         * used by the subclass <code>createBidiMap</code> implementation.
         *
         * @param normalMap  the normal direction map
         * @param reverseMap  the reverse direction map
         * @param inverseBidiMap  the inverse BidiMap
         */
        protected AbstractDualBidiMap(java.util.Map<Object, Object> normalMap, java.util.Map<Object, Object> reverseMap, BidiMap inverseBidiMap)
            : base()
        {
            maps[0] = normalMap;
            maps[1] = reverseMap;
            this.inverseBidiMapJ = inverseBidiMap;
        }

        /**
         * Creates a new instance of the map used by the subclass to store data.
         * <p>
         * This design is deeply flawed and has been deprecated.
         * It relied on subclass data being used during a superclass constructor.
         * 
         * @return the map to be used for internal storage
         * @deprecated For constructors, use the new two map constructor.
         * For deserialization, populate the maps array directly in readObject.
         */
        protected virtual java.util.Map<Object, Object> createMap()
        {
            return null;
        }

        /**
         * Creates a new instance of the subclass.
         * 
         * @param normalMap  the normal direction map
         * @param reverseMap  the reverse direction map
         * @param inverseMap  this map, which is the inverse in the new map
         * @return the inverse map
         */
        protected abstract BidiMap createBidiMap(java.util.Map<Object, Object> normalMap, java.util.Map<Object, Object> reverseMap, BidiMap inverseMap);

        // Map delegation
        //-----------------------------------------------------------------------
        public Object get(Object key)
        {
            return maps[0].get(key);
        }

        public virtual int size()
        {
            return maps[0].size();
        }

        public virtual bool isEmpty()
        {
            return maps[0].isEmpty();
        }

        public virtual bool containsKey(Object key)
        {
            return maps[0].containsKey(key);
        }

        public override bool Equals(Object obj)
        {
            return maps[0].equals(obj);
        }

        public override int GetHashCode()
        {
            return maps[0].GetHashCode();
        }

        public override String ToString()
        {
            return maps[0].toString();
        }

        // BidiMap changes
        //-----------------------------------------------------------------------
        public virtual Object put(Object key, Object value)
        {
            if (maps[0].containsKey(key))
            {
                maps[1].remove(maps[0].get(key));
            }
            if (maps[1].containsKey(value))
            {
                maps[0].remove(maps[1].get(value));
            }
            Object obj = maps[0].put(key, value);
            maps[1].put(value, key);
            return obj;
        }

        public virtual void putAll(java.util.Map<Object, Object> map)
        {
            for (java.util.Iterator<Object> it = (java.util.Iterator<Object>)map.entrySet().iterator(); it.hasNext(); )
            {
                biz.ritter.javapi.util.MapNS.Entry<Object, Object> entry = (biz.ritter.javapi.util.MapNS.Entry<Object, Object>)it.next();
                put(entry.getKey(), entry.getValue());
            }
        }

        public virtual Object remove(Object key)
        {
            Object value = null;
            if (maps[0].containsKey(key))
            {
                value = maps[0].remove(key);
                maps[1].remove(value);
            }
            return value;
        }

        public virtual void clear()
        {
            maps[0].clear();
            maps[1].clear();
        }

        public virtual bool containsValue(Object value)
        {
            return maps[1].containsKey(value);
        }

        // BidiMap
        //-----------------------------------------------------------------------
        /**
         * Obtains a <code>MapIterator</code> over the map.
         * The iterator implements <code>ResetableMapIterator</code>.
         * This implementation relies on the entrySet iterator.
         * <p>
         * The setValue() methods only allow a new value to be set.
         * If the value being set is already in the map, an IllegalArgumentException
         * is thrown (as setValue cannot change the size of the map).
         * 
         * @return a map iterator
         */
        public virtual MapIterator mapIterator()
        {
            return new BidiMapIterator(this);
        }

        public virtual Object getKey(Object value)
        {
            return maps[1].get(value);
        }

        public virtual Object removeValue(Object value)
        {
            Object key = null;
            if (maps[1].containsKey(value))
            {
                key = maps[1].remove(value);
                maps[0].remove(key);
            }
            return key;
        }

        public virtual BidiMap inverseBidiMap()
        {
            if (inverseBidiMapJ == null)
            {
                inverseBidiMapJ = createBidiMap(maps[1], maps[0], this);
            }
            return inverseBidiMapJ;
        }

        // Map views
        //-----------------------------------------------------------------------
        /**
         * Gets a keySet view of the map.
         * Changes made on the view are reflected in the map.
         * The set supports remove and clear but not add.
         * 
         * @return the keySet view
         */
        public virtual java.util.Set<Object> keySet()
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
         * @param iterator  the iterator to decorate
         * @return the keySet iterator
         */
        protected internal virtual java.util.Iterator<Object> createKeySetIterator(java.util.Iterator<Object> iterator)
        {
            return new KeySetIterator(iterator, this);
        }

        /**
         * Gets a values view of the map.
         * Changes made on the view are reflected in the map.
         * The set supports remove and clear but not add.
         * 
         * @return the values view
         */
        public virtual java.util.Collection<Object> values()
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
         * @param iterator  the iterator to decorate
         * @return the values iterator
         */
        protected internal virtual java.util.Iterator<Object> createValuesIterator(java.util.Iterator<Object> iterator)
        {
            return new ValuesIterator(iterator, this);
        }

        /**
         * Gets an entrySet view of the map.
         * Changes made on the set are reflected in the map.
         * The set supports remove and clear but not add.
         * <p>
         * The Map Entry setValue() method only allow a new value to be set.
         * If the value being set is already in the map, an IllegalArgumentException
         * is thrown (as setValue cannot change the size of the map).
         * 
         * @return the entrySet view
         */
        public virtual java.util.Set<java.util.MapNS.Entry<Object,Object>> entrySet()
        {
            if (entrySetJ == null)
            {
                entrySetJ = new EntrySet(this);
            }
            return (java.util.Set<java.util.MapNS.Entry<Object,Object>>) entrySetJ;
        }

        /**
         * Creates an entry set iterator.
         * Subclasses can override this to return iterators with different properties.
         * 
         * @param iterator  the iterator to decorate
         * @return the entrySet iterator
         */
        protected internal java.util.Iterator<Object> createEntrySetIterator(java.util.Iterator<Object> iterator)
        {
            return new EntrySetIterator(iterator, this);
        }


    }
    //-----------------------------------------------------------------------
    /**
     * Inner class View.
     */
    public abstract class View : AbstractCollectionDecorator
    {

        /** The parent map */
        protected readonly AbstractDualBidiMap parent;

        /**
         * Constructs a new view of the BidiMap.
         * 
         * @param coll  the collection view being decorated
         * @param parent  the parent BidiMap
         */
        protected internal View(java.util.Collection<Object> coll, AbstractDualBidiMap parent)
            : base(coll)
        {
            this.parent = parent;
        }

        public override bool removeAll(java.util.Collection<Object> coll)
        {
            if (parent.isEmpty() || coll.isEmpty())
            {
                return false;
            }
            bool modified = false;
            java.util.Iterator<Object> it = iterator();
            while (it.hasNext())
            {
                if (coll.contains(it.next()))
                {
                    it.remove();
                    modified = true;
                }
            }
            return modified;
        }

        public override bool retainAll(java.util.Collection<Object> coll)
        {
            if (parent.isEmpty())
            {
                return false;
            }
            if (coll.isEmpty())
            {
                parent.clear();
                return true;
            }
            bool modified = false;
            java.util.Iterator<Object> it = iterator();
            while (it.hasNext())
            {
                if (coll.contains(it.next()) == false)
                {
                    it.remove();
                    modified = true;
                }
            }
            return modified;
        }

        public override void clear()
        {
            parent.clear();
        }
    }

    //-----------------------------------------------------------------------
    /**
     * Inner class KeySet.
     */
    public class KeySet : View, java.util.Set<Object>
    {

        /**
         * Constructs a new view of the BidiMap.
         * 
         * @param parent  the parent BidiMap
         */
        protected internal KeySet(AbstractDualBidiMap parent)
            : base(parent.maps[0].keySet(), parent)
        {
        }

        public override java.util.Iterator<Object> iterator()
        {
            return parent.createKeySetIterator(base.iterator());
        }

        public override bool contains(Object key)
        {
            return parent.maps[0].containsKey(key);
        }

        public override bool remove(Object key)
        {
            if (parent.maps[0].containsKey(key))
            {
                Object value = parent.maps[0].remove(key);
                parent.maps[1].remove(value);
                return true;
            }
            return false;
        }
    }

    /**
     * Inner class KeySetIterator.
     */
    public class KeySetIterator : AbstractIteratorDecorator
    {

        /** The parent map */
        protected readonly AbstractDualBidiMap parent;
        /** The last returned key */
        protected Object lastKey = null;
        /** Whether remove is allowed at present */
        protected bool canRemove = false;

        /**
         * Constructor.
         * @param iterator  the iterator to decorate
         * @param parent  the parent map
         */
        protected internal KeySetIterator(java.util.Iterator<Object> iterator, AbstractDualBidiMap parent)
            : base(iterator)
        {
            this.parent = parent;
        }

        public override Object next()
        {
            lastKey = base.next();
            canRemove = true;
            return lastKey;
        }

        public override void remove()
        {
            if (canRemove == false)
            {
                throw new java.lang.IllegalStateException("Iterator remove() can only be called once after next()");
            }
            Object value = parent.maps[0].get(lastKey);
            base.remove();
            parent.maps[1].remove(value);
            lastKey = null;
            canRemove = false;
        }
    }

    //-----------------------------------------------------------------------
    /**
     * Inner class Values.
     */
    public class Values : View, java.util.Set<Object>
    {

        /**
         * Constructs a new view of the BidiMap.
         * 
         * @param parent  the parent BidiMap
         */
        protected internal Values(AbstractDualBidiMap parent)
            : base(parent.maps[0].values(), parent)
        {
        }

        public override java.util.Iterator<Object> iterator()
        {
            return parent.createValuesIterator(base.iterator());
        }

        public override bool contains(Object value)
        {
            return parent.maps[1].containsKey(value);
        }

        public override bool remove(Object value)
        {
            if (parent.maps[1].containsKey(value))
            {
                Object key = parent.maps[1].remove(value);
                parent.maps[0].remove(key);
                return true;
            }
            return false;
        }
    }

    /**
     * Inner class ValuesIterator.
     */
    public class ValuesIterator : AbstractIteratorDecorator
    {

        /** The parent map */
        protected readonly AbstractDualBidiMap parent;
        /** The last returned value */
        protected Object lastValue = null;
        /** Whether remove is allowed at present */
        protected bool canRemove = false;

        /**
         * Constructor.
         * @param iterator  the iterator to decorate
         * @param parent  the parent map
         */
        protected internal ValuesIterator(java.util.Iterator<Object> iterator, AbstractDualBidiMap parent)
            : base(iterator)
        {
            this.parent = parent;
        }

        public override Object next()
        {
            lastValue = base.next();
            canRemove = true;
            return lastValue;
        }

        public override void remove()
        {
            if (canRemove == false)
            {
                throw new java.lang.IllegalStateException("Iterator remove() can only be called once after next()");
            }
            base.remove(); // removes from maps[0]
            parent.maps[1].remove(lastValue);
            lastValue = null;
            canRemove = false;
        }
    }

    //-----------------------------------------------------------------------
    /**
     * Inner class EntrySet.
     */
    public class EntrySet : View, java.util.Set<Object>
    {

        /**
         * Constructs a new view of the BidiMap.
         * 
         * @param parent  the parent BidiMap
         */
        protected internal EntrySet(AbstractDualBidiMap parent)
            : base(
            (java.util.Collection<Object>)parent.maps[0].entrySet(),
            parent)
        {
        }

        public override java.util.Iterator<Object> iterator()
        {
            return parent.createEntrySetIterator(base.iterator());
        }

        public override bool remove(Object obj)
        {
            if (obj is java.util.MapNS.Entry<Object, Object> == false)
            {
                return false;
            }
            java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)obj;
            Object key = entry.getKey();
            if (parent.containsKey(key))
            {
                Object value = parent.maps[0].get(key);
                if (value == null ? entry.getValue() == null : value.equals(entry.getValue()))
                {
                    parent.maps[0].remove(key);
                    parent.maps[1].remove(value);
                    return true;
                }
            }
            return false;
        }
    }

    /**
     * Inner class EntrySetIterator.
     */
    public class EntrySetIterator : AbstractIteratorDecorator
    {

        /** The parent map */
        protected readonly AbstractDualBidiMap parent;
        /** The last returned entry */
        protected java.util.MapNS.Entry<Object, Object> last = null;
        /** Whether remove is allowed at present */
        protected bool canRemove = false;

        /**
         * Constructor.
         * @param iterator  the iterator to decorate
         * @param parent  the parent map
         */
        protected internal EntrySetIterator(java.util.Iterator<Object> iterator, AbstractDualBidiMap parent)
            : base(iterator)
        {
            this.parent = parent;
        }

        public override Object next()
        {
            last = new MapEntry((java.util.MapNS.Entry<Object, Object>)base.next(), parent);
            canRemove = true;
            return last;
        }

        public override void remove()
        {
            if (canRemove == false)
            {
                throw new java.lang.IllegalStateException("Iterator remove() can only be called once after next()");
            }
            // store value as remove may change the entry in the decorator (eg.TreeMap)
            Object value = last.getValue();
            base.remove();
            parent.maps[1].remove(value);
            last = null;
            canRemove = false;
        }
    }

    /**
     * Inner class MapEntry.
     */
    public class MapEntry : AbstractMapEntryDecorator
    {

        /** The parent map */
        protected readonly AbstractDualBidiMap parent;
        /**
         * Constructor.
         * @param entry  the entry to decorate
         * @param parent  the parent map
         */
        protected internal MapEntry(java.util.MapNS.Entry<Object, Object> entry, AbstractDualBidiMap parent)
            : base(entry)
        {
            this.parent = parent;
        }

        public override Object setValue(Object value)
        {
            Object key =  /*MapEntry.*/this.getKey();
            if (parent.maps[1].containsKey(value) &&
                parent.maps[1].get(value) != key)
            {
                throw new java.lang.IllegalArgumentException("Cannot use setValue() when the object being set is already in the map");
            }
            parent.put(key, value);
            Object oldValue = base.setValue(value);
            return oldValue;
        }
    }

    /**
     * Inner class MapIterator.
     */
    public class BidiMapIterator : MapIterator, ResettableIterator
    {

        /** The parent map */
        protected readonly AbstractDualBidiMap parent;
        /** The iterator being wrapped */
        protected java.util.Iterator<Object> iterator;
        /** The last returned entry */
        protected java.util.MapNS.Entry<Object, Object> last = null;
        /** Whether remove is allowed at present */
        protected bool canRemove = false;

        /**
         * Constructor.
         * @param parent  the parent map
         */
        protected internal BidiMapIterator(AbstractDualBidiMap parent)
            : base()
        {
            this.parent = parent;
            this.iterator = (java.util.Iterator<Object>)parent.maps[0].entrySet().iterator();
        }

        public bool hasNext()
        {
            return iterator.hasNext();
        }

        public Object next()
        {
            last = (java.util.MapNS.Entry<Object, Object>)iterator.next();
            canRemove = true;
            return last.getKey();
        }

        public void remove()
        {
            if (canRemove == false)
            {
                throw new java.lang.IllegalStateException("Iterator remove() can only be called once after next()");
            }
            // store value as remove may change the entry in the decorator (eg.TreeMap)
            Object value = last.getValue();
            iterator.remove();
            parent.maps[1].remove(value);
            last = null;
            canRemove = false;
        }

        public Object getKey()
        {
            if (last == null)
            {
                throw new java.lang.IllegalStateException("Iterator getKey() can only be called after next() and before remove()");
            }
            return last.getKey();
        }

        public Object getValue()
        {
            if (last == null)
            {
                throw new java.lang.IllegalStateException("Iterator getValue() can only be called after next() and before remove()");
            }
            return last.getValue();
        }

        public Object setValue(Object value)
        {
            if (last == null)
            {
                throw new java.lang.IllegalStateException("Iterator setValue() can only be called after next() and before remove()");
            }
            if (parent.maps[1].containsKey(value) &&
                parent.maps[1].get(value) != last.getKey())
            {
                throw new java.lang.IllegalArgumentException("Cannot use setValue() when the object being set is already in the map");
            }
            return parent.put(last.getKey(), value);
        }

        public void reset()
        {
            iterator = (java.util.Iterator<Object>)parent.maps[0].entrySet().iterator();
            last = null;
            canRemove = false;
        }

        public String toString()
        {
            if (last != null)
            {
                return "MapIterator[" + getKey() + "=" + getValue() + "]";
            }
            else
            {
                return "MapIterator[]";
            }
        }
    }
}