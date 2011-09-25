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
using org.apache.commons.collections.map;

namespace org.apache.commons.collections.bidimap
{

    /**
     * Implementation of <code>BidiMap</code> that uses two <code>java.util.TreeMap<Object,Object></code> instances.
     * <p>
     * The setValue() method on iterators will succeed only if the new value being set is
     * not already in the bidimap.
     * <p>
     * When considering whether to use this class, the {@link TreeBidiMap} class should
     * also be considered. It implements the interface using a dedicated design, and does
     * not store each object twice, which can save on memory use.
     * <p>
     * NOTE: From Commons Collections 3.1, all subclasses will use <code>java.util.TreeMap<Object,Object></code>
     * and the flawed <code>createMap</code> method is ignored.
     * 
     * @since Commons Collections 3.0
     * @version $Id$
     * 
     * @author Matthew Hawthorne
     * @author Stephen Colebourne
     */
    [Serializable]
    public class DualTreeBidiMap
            : AbstractDualBidiMap, SortedBidiMap, java.io.Serializable
    {

        /** Ensure serialization compatibility */
        private static readonly long serialVersionUID = 721969328361809L;
        /** The comparator to use */
        protected readonly java.util.Comparator<Object> comparatorJ;

        /**
         * Creates an empty <code>DualTreeBidiMap</code>
         */
        public DualTreeBidiMap() :
            base(new java.util.TreeMap<Object, Object>(), new java.util.TreeMap<Object, Object>())
        {
            this.comparatorJ = null;
        }

        /** 
         * Constructs a <code>DualTreeBidiMap</code> and copies the mappings from
         * specified <code>Map</code>.  
         *
         * @param map  the map whose mappings are to be placed in this map
         */
        public DualTreeBidiMap(java.util.Map<Object, Object> map) :
            base(new java.util.TreeMap<Object, Object>(), new java.util.TreeMap<Object, Object>())
        {
            putAll(map);
            this.comparatorJ = null;
        }

        /** 
         * Constructs a <code>DualTreeBidiMap</code> using the specified Comparator.
         *
         * @param comparator  the Comparator
         */
        public DualTreeBidiMap(java.util.Comparator<Object> comparator) :
            base(new java.util.TreeMap<Object, Object>(comparator), new java.util.TreeMap<Object, Object>(comparator))
        {
            this.comparatorJ = comparator;
        }

        /** 
         * Constructs a <code>DualTreeBidiMap</code> that decorates the specified maps.
         *
         * @param normalMap  the normal direction map
         * @param reverseMap  the reverse direction map
         * @param inverseBidiMap  the inverse BidiMap
         */
        protected DualTreeBidiMap(java.util.Map<Object, Object> normalMap, java.util.Map<Object, Object> reverseMap, BidiMap inverseBidiMap) :
            base(normalMap, reverseMap, inverseBidiMap)
        {
            this.comparatorJ = ((java.util.SortedMap<Object, Object>)normalMap).comparator();
        }

        /**
         * Creates a new instance of this object.
         * 
         * @param normalMap  the normal direction map
         * @param reverseMap  the reverse direction map
         * @param inverseMap  the inverse BidiMap
         * @return new bidi map
         */
        protected override BidiMap createBidiMap(java.util.Map<Object, Object> normalMap, java.util.Map<Object, Object> reverseMap, BidiMap inverseMap)
        {
            return new DualTreeBidiMap(normalMap, reverseMap, inverseMap);
        }

        //-----------------------------------------------------------------------
        public virtual java.util.Comparator<Object> comparator()
        {
            return ((java.util.SortedMap<Object, Object>)maps[0]).comparator();
        }

        public virtual Object firstKey()
        {
            return ((java.util.SortedMap<Object, Object>)maps[0]).firstKey();
        }

        public virtual Object lastKey()
        {
            return ((java.util.SortedMap<Object, Object>)maps[0]).lastKey();
        }

        public virtual Object nextKey(Object key)
        {
            if (isEmpty())
            {
                return null;
            }
            if (maps[0] is OrderedMap)
            {
                return ((OrderedMap)maps[0]).nextKey(key);
            }
            java.util.SortedMap<Object, Object> sm = (java.util.SortedMap<Object, Object>)maps[0];
            java.util.Iterator<Object> it = sm.tailMap(key).keySet().iterator();
            it.next();
            if (it.hasNext())
            {
                return it.next();
            }
            return null;
        }

        public virtual Object previousKey(Object key)
        {
            if (isEmpty())
            {
                return null;
            }
            if (maps[0] is OrderedMap)
            {
                return ((OrderedMap)maps[0]).previousKey(key);
            }
            java.util.SortedMap<Object, Object> sm = (java.util.SortedMap<Object, Object>)maps[0];
            java.util.SortedMap<Object, Object> hm = sm.headMap(key);
            if (hm.isEmpty())
            {
                return null;
            }
            return hm.lastKey();
        }

        //-----------------------------------------------------------------------
        /**
         * Obtains an ordered map iterator.
         * <p>
         * This implementation copies the elements to an ArrayList in order to
         * provide the forward/backward behaviour.
         * 
         * @return a new ordered map iterator
         */
        public virtual OrderedMapIterator orderedMapIterator()
        {
            return new BidiOrderedMapIterator(this);
        }

        public virtual SortedBidiMap inverseSortedBidiMap()
        {
            return (SortedBidiMap)inverseBidiMap();
        }

        public virtual OrderedBidiMap inverseOrderedBidiMap()
        {
            return (OrderedBidiMap)inverseBidiMap();
        }

        //-----------------------------------------------------------------------
        public virtual java.util.SortedMap<Object, Object> headMap(Object toKey)
        {
            java.util.SortedMap<Object, Object> sub = ((java.util.SortedMap<Object, Object>)maps[0]).headMap(toKey);
            return new ViewMap(this, sub);
        }

        public virtual java.util.SortedMap<Object, Object> tailMap(Object fromKey)
        {
            java.util.SortedMap<Object, Object> sub = ((java.util.SortedMap<Object, Object>)maps[0]).tailMap(fromKey);
            return new ViewMap(this, sub);
        }

        public virtual java.util.SortedMap<Object, Object> subMap(Object fromKey, Object toKey)
        {
            java.util.SortedMap<Object, Object> sub = ((java.util.SortedMap<Object, Object>)maps[0]).subMap(fromKey, toKey);
            return new ViewMap(this, sub);
        }

        //-----------------------------------------------------------------------
        /**
         * Internal sorted map view.
         */
        protected class ViewMap : AbstractSortedMapDecorator
        {
            /** The parent bidi map. */
            readonly DualTreeBidiMap bidi;

            /**
             * Constructor.
             * @param bidi  the parent bidi map
             * @param sm  the subMap sorted map
             */
            protected internal ViewMap(DualTreeBidiMap bidi, java.util.SortedMap<Object, Object> sm) :
                // the implementation is not great here...
                // use the maps[0] as the filtered map, but maps[1] as the full map
                // this forces containsValue and clear to be overridden
                base((java.util.SortedMap<Object, Object>)bidi.createBidiMap(sm, bidi.maps[1], bidi.inverseBidiMapJ))
            {
                this.bidi = (DualTreeBidiMap)map;
            }

            public override bool containsValue(Object value)
            {
                // override as default implementation jumps to [1]
                return bidi.maps[0].containsValue(value);
            }

            public override void clear()
            {
                // override as default implementation jumps to [1]
                for (java.util.Iterator<Object> it = keySet().iterator(); it.hasNext(); )
                {
                    it.next();
                    it.remove();
                }
            }

            public override java.util.SortedMap<Object, Object> headMap(Object toKey)
            {
                return new ViewMap(bidi, base.headMap(toKey));
            }

            public override java.util.SortedMap<Object, Object> tailMap(Object fromKey)
            {
                return new ViewMap(bidi, base.tailMap(fromKey));
            }

            public override java.util.SortedMap<Object, Object> subMap(Object fromKey, Object toKey)
            {
                return new ViewMap(bidi, base.subMap(fromKey, toKey));
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Inner class MapIterator.
         */
        protected class BidiOrderedMapIterator : OrderedMapIterator, ResettableIterator
        {

            /** The parent map */
            protected readonly AbstractDualBidiMap parent;
            /** The iterator being decorated */
            protected java.util.ListIterator<Object> iterator;
            /** The last returned entry */
            private java.util.MapNS.Entry<Object, Object> last = null;

            /**
             * Constructor.
             * @param parent  the parent map
             */
            protected internal BidiOrderedMapIterator(AbstractDualBidiMap parent)
                : base()
            {
                this.parent = parent;
                iterator = new java.util.ArrayList<Object>((java.util.Collection<Object>)parent.entrySet()).listIterator();
            }

            public virtual bool hasNext()
            {
                return iterator.hasNext();
            }

            public virtual Object next()
            {
                last = (java.util.MapNS.Entry<Object, Object>)iterator.next();
                return last.getKey();
            }

            public virtual bool hasPrevious()
            {
                return iterator.hasPrevious();
            }

            public virtual Object previous()
            {
                last = (java.util.MapNS.Entry<Object, Object>)iterator.previous();
                return last.getKey();
            }

            public virtual void remove()
            {
                iterator.remove();
                parent.remove(last.getKey());
                last = null;
            }

            public virtual Object getKey()
            {
                if (last == null)
                {
                    throw new java.lang.IllegalStateException("Iterator getKey() can only be called after next() and before remove()");
                }
                return last.getKey();
            }

            public virtual Object getValue()
            {
                if (last == null)
                {
                    throw new java.lang.IllegalStateException("Iterator getValue() can only be called after next() and before remove()");
                }
                return last.getValue();
            }

            public virtual Object setValue(Object value)
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

            public virtual void reset()
            {
                iterator = new java.util.ArrayList<Object>((java.util.Collection<Object>)parent.entrySet()).listIterator();
                last = null;
            }

            public override String ToString()
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

        // Serialization
        //-----------------------------------------------------------------------
        private void writeObject(java.io.ObjectOutputStream outJ)
        {//throws IOException {
            outJ.defaultWriteObject();
            outJ.writeObject(maps[0]);
        }

        private void readObject(java.io.ObjectInputStream inJ)
        {//throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            maps[0] = new java.util.TreeMap<Object, Object>(comparatorJ);
            maps[1] = new java.util.TreeMap<Object, Object>(comparatorJ);
            java.util.Map<Object, Object> map = (java.util.Map<Object, Object>)inJ.readObject();
            putAll(map);
        }
    }
}