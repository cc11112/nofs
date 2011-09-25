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
     * HashSet is an implementation of a Set. All optional operations (adding and
     * removing) are supported. The elements can be any objects.
     */
    [Serializable]
    public class HashSet<E> : AbstractSet<E>, Set<E>, java.lang.Cloneable, java.io.Serializable {

        private static readonly long serialVersionUID = -5024744406713321676L;

        protected internal HashMap<E, HashSet<E>> backingMap;

        /**
         * Constructs a new empty instance of {@code HashSet}.
         */
        public HashSet()
            // using specialized LinkedHashMap instead HashMap, but all LinkedHashMaps are HashMaps
            : this(new LinkedHashMap<E, HashSet<E>>())
        {
        }

        /**
         * Constructs a new instance of {@code HashSet} with the specified capacity.
         * 
         * @param capacity
         *            the initial capacity of this {@code HashSet}.
         */
        public HashSet(int capacity)
            // using specialized LinkedHashMap instead HashMap, but all LinkedHashMaps are HashMaps
            : this(new LinkedHashMap<E, HashSet<E>>(capacity))
        {
        }

        /**
         * Constructs a new instance of {@code HashSet} with the specified capacity
         * and load factor.
         * 
         * @param capacity
         *            the initial capacity.
         * @param loadFactor
         *            the initial load factor.
         */
        // using specialized LinkedHashMap instead HashMap
        public HashSet(int capacity, float loadFactor)
            // using specialized LinkedHashMap instead HashMap, but all LinkedHashMaps are HashMaps
            : this(new LinkedHashMap<E, HashSet<E>>(capacity, loadFactor))
        {
        }

        /**
         * Constructs a new instance of {@code HashSet} containing the unique
         * elements in the specified collection.
         * 
         * @param collection
         *            the collection of elements to add.
         */
        public HashSet(Collection< E> collection) :
            // using specialized LinkedHashMap instead HashMap, but all LinkedHashMaps are HashMaps
            this(new LinkedHashMap <E, HashSet<E>>(collection.size() < 6 ? 11 : collection.size() * 2)){
                java.util.Iterator<E> it = collection.iterator();
                while (it.hasNext())
                {
                    add(it.next());
                }
            }

        protected internal HashSet(HashMap<E, HashSet<E>> backingMap) {
            this.backingMap = backingMap;
        }

        /**
         * Adds the specified object to this {@code HashSet} if not already present.
         * 
         * @param object
         *            the object to add.
         * @return {@code true} when this {@code HashSet} did not already contain
         *         the object, {@code false} otherwise
         */
        public override bool add(E obj) {
            return backingMap.put(obj, this) == null;
        }

        /**
         * Removes all elements from this {@code HashSet}, leaving it empty.
         * 
         * @see #isEmpty
         * @see #size
         */
        public override void clear() {
            backingMap.clear();
        }

        /**
         * Returns a new {@code HashSet} with the same elements and size as this
         * {@code HashSet}.
         * 
         * @return a shallow copy of this {@code HashSet}.
         * @see java.lang.Cloneable
         */
        public Object clone() {
            try {
                HashSet<E> clone = (HashSet<E>) MemberwiseClone();
                clone.backingMap = (HashMap<E, HashSet<E>>) MemberwiseClone();
                return clone;
            } catch (java.lang.CloneNotSupportedException e) {
                return null;
            }
        }

        /**
         * Searches this {@code HashSet} for the specified object.
         * 
         * @param object
         *            the object to search for.
         * @return {@code true} if {@code object} is an element of this
         *         {@code HashSet}, {@code false} otherwise.
         */
        public override bool contains(Object obj) {
            return backingMap.containsKey(obj);
        }

        /**
         * Returns true if this {@code HashSet} has no elements, false otherwise.
         * 
         * @return {@code true} if this {@code HashSet} has no elements,
         *         {@code false} otherwise.
         * @see #size
         */
        public override bool isEmpty() {
            return backingMap.isEmpty();
        }

        /**
         * Returns an Iterator on the elements of this {@code HashSet}.
         * 
         * @return an Iterator on the elements of this {@code HashSet}.
         * @see Iterator
         */
        public override Iterator<E> iterator() {
            return backingMap.keySet().iterator();
        }

        /**
         * Removes the specified object from this {@code HashSet}.
         * 
         * @param object
         *            the object to remove.
         * @return {@code true} if the object was removed, {@code false} otherwise.
         */
        public override bool remove(Object obj) {
            return backingMap.remove(obj) != null;
        }

        /**
         * Returns the number of elements in this {@code HashSet}.
         * 
         * @return the number of elements in this {@code HashSet}.
         */
        public override int size() {
            return backingMap.size();
        }
/*
        private void writeObject(ObjectOutputStream stream) throws IOException {
            stream.defaultWriteObject();
            stream.writeInt(backingMap.elementData.length);
            stream.writeFloat(backingMap.loadFactor);
            stream.writeInt(backingMap.elementCount);
            for (int i = backingMap.elementData.length; --i >= 0;) {
                HashMap.Entry<E, HashSet<E>> entry = backingMap.elementData[i];
                while (entry != null) {
                    stream.writeObject(entry.key);
                    entry = entry.next;
                }
            }
        }

        @SuppressWarnings("unchecked")
        private void readObject(ObjectInputStream stream) throws IOException,
                ClassNotFoundException {
            stream.defaultReadObject();
            int length = stream.readInt();
            float loadFactor = stream.readFloat();
            backingMap = createBackingMap(length, loadFactor);
            int elementCount = stream.readInt();
            for (int i = elementCount; --i >= 0;) {
                E key = (E) stream.readObject();
                backingMap.put(key, this);
            }
        }
*/
        protected internal virtual HashMap<E, HashSet<E>> createBackingMap(int capacity, float loadFactor) {
            return new LinkedHashMap<E, HashSet<E>>(capacity, loadFactor);
        }
    }
}
