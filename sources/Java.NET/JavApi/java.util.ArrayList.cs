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
     * ArrayList is an implementation of {@link List}, backed by an array. All
     * optional operations adding, removing, and replacing are supported. The
     * elements can be any objects.
     * 
     * @since 1.2
     */
    [Serializable]
    public class ArrayList<E> : AbstractList<E>, List<E>, java.lang.Cloneable, java.io.Serializable, RandomAccess {

        private static readonly long serialVersionUID = 8683452581122892189L;

        [NonSerialized]
        private int firstIndex;
        [NonSerialized]
        private int sizeJ;
        [NonSerialized]
        private E[] array;

        /**
         * Constructs a new instance of {@code ArrayList} with ten capacity.
         */
        public ArrayList() : base (){
            firstIndex = sizeJ = 0;
            array = newElementArray(10);
        }

        internal ArrayList(E[] array) :base()
        {
            this.array = array;
            firstIndex = 0;
            sizeJ = this.array.Length;
        }

        /**
         * Constructs a new instance of {@code ArrayList} with the specified
         * capacity.
         * 
         * @param capacity
         *            the initial capacity of this {@code ArrayList}.
         */
        public ArrayList(int capacity) :base() {
            if (capacity < 0) {
                throw new java.lang.IllegalArgumentException();
            }
            firstIndex = sizeJ = 0;
            array = newElementArray(capacity);
        }

        /**
         * Constructs a new instance of {@code ArrayList} containing the elements of
         * the specified collection. The initial size of the {@code ArrayList} will
         * be 10% larger than the size of the specified collection.
         * 
         * @param collection
         *            the collection of elements to add.
         */
        public ArrayList(Collection<E> collection) {
            firstIndex = 0;
            Object[] objects = collection.toArray();
            sizeJ = objects.Length;

            // REVIEW: Created 2 array copies of the original collection here
            //         Could be better to use the collection iterator and
            //         copy once?
            array = newElementArray(sizeJ + (sizeJ / 10));
            java.lang.SystemJ.arraycopy(objects, 0, array, 0, sizeJ);
            modCount = 1;
        }

        private E[] newElementArray(int length) {
            return (E[]) new E[length];
        }

        /**
         * Inserts the specified object into this {@code ArrayList} at the specified
         * location. The object is inserted before any previous element at the
         * specified location. If the location is equal to the size of this
         * {@code ArrayList}, the object is added at the end.
         * 
         * @param location
         *            the index at which to insert the object.
         * @param object
         *            the object to add.
         * @throws IndexOutOfBoundsException
         *             when {@code location < 0 || > size()}
         */
        public override void add(int location, E obj) {
            if (location < 0 || location > sizeJ) {
                throw new java.lang.IndexOutOfBoundsException(
                        // luni.0A=Index: {0}, Size: {1}
                        "Index: "+location+" Size: "+sizeJ //$NON-NLS-1$
                        );
            }
            if (location == 0) {
                if (firstIndex == 0) {
                    growAtFront(1);
                }
                array[--firstIndex] = obj;
            } else if (location == sizeJ) {
                if (firstIndex + sizeJ == array.Length) {
                    growAtEnd(1);
                }
                array[firstIndex + sizeJ] = obj;
            } else { // must be case: (0 < location && location < sizeJ)
                if (sizeJ == array.Length) {
                    growForInsert(location, 1);
                } else if (firstIndex + sizeJ == array.Length
                        || (firstIndex > 0 && location < sizeJ / 2)) {
                    java.lang.SystemJ.arraycopy(array, firstIndex, array, --firstIndex,
                            location);
                } else {
                    int index = location + firstIndex;
                    java.lang.SystemJ.arraycopy(array, index, array, index + 1, sizeJ
                            - location);
                }
                array[location + firstIndex] = obj;
            }

            sizeJ++;
            modCount++;
        }

        /**
         * Adds the specified object at the end of this {@code ArrayList}.
         * 
         * @param object
         *            the object to add.
         * @return always true
         */
        
        public override bool add(E obj) {
            if (firstIndex + sizeJ == array.Length) {
                growAtEnd(1);
            }
            array[firstIndex + sizeJ] = obj;
            sizeJ++;
            modCount++;
            return true;
        }

        /**
         * Inserts the objects in the specified collection at the specified location
         * in this List. The objects are added in the order they are returned from
         * the collection's iterator.
         * 
         * @param location
         *            the index at which to insert.
         * @param collection
         *            the collection of objects.
         * @return {@code true} if this {@code ArrayList} is modified, {@code false}
         *         otherwise.
         * @throws IndexOutOfBoundsException
         *             when {@code location < 0 || > size()}
         */
        
        public override bool addAll(int location, Collection<E> collection) {
            if (location < 0 || location > sizeJ) {
                throw new java.lang.IndexOutOfBoundsException(
                        // luni.0A=Index: {0}, Size: {1}
                        "Index: "+location+", Size: "+sizeJ);
            }

            Object[] dumparray = collection.toArray();
            int growSize = dumparray.Length;
            // REVIEW: Why do this check here rather than check
            //         collection.size() earlier? RI behaviour?
            if (growSize == 0) {
                return false;
            }

            if (location == 0) {
                growAtFront(growSize);
                firstIndex -= growSize;
            } else if (location == sizeJ) {
                if (firstIndex + sizeJ > array.Length - growSize) {
                    growAtEnd(growSize);
                }
            } else { // must be case: (0 < location && location < sizeJ)
                if (array.Length - sizeJ < growSize) {
                    growForInsert(location, growSize);
                } else if (firstIndex + sizeJ > array.Length - growSize
                           || (firstIndex > 0 && location < sizeJ / 2)) {
                    int newFirst = firstIndex - growSize;
                    if (newFirst < 0) {
                        int index = location + firstIndex;
                        java.lang.SystemJ.arraycopy(array, index, array, index - newFirst,
                                sizeJ - location);
                        newFirst = 0;
                    }
                    java.lang.SystemJ.arraycopy(array, firstIndex, array, newFirst, location);
                    firstIndex = newFirst;
                } else {
                    int index = location + firstIndex;
                    java.lang.SystemJ.arraycopy(array, index, array, index + growSize, sizeJ
                            - location);
                }
            }

            java.lang.SystemJ.arraycopy(dumparray, 0, this.array, location + firstIndex,
                    growSize);
            sizeJ += growSize;
            modCount++;
            return true;
        }

        /**
         * Adds the objects in the specified collection to this {@code ArrayList}.
         * 
         * @param collection
         *            the collection of objects.
         * @return {@code true} if this {@code ArrayList} is modified, {@code false}
         *         otherwise.
         */
        
        public override bool addAll(Collection<E> collection) {
            Object[] dumpArray = collection.toArray();
            if (dumpArray.Length == 0) {
                return false;
            }
            if (dumpArray.Length > array.Length - (firstIndex + sizeJ)) {
                growAtEnd(dumpArray.Length);
            }
            java.lang.SystemJ.arraycopy(dumpArray, 0, this.array, firstIndex + sizeJ,
                             dumpArray.Length);
            sizeJ += dumpArray.Length;
            modCount++;
            return true;
        }

        /**
         * Removes all elements from this {@code ArrayList}, leaving it empty.
         * 
         * @see #isEmpty
         * @see #size
         */
        public override void clear() {
            if (sizeJ != 0) {
                // REVIEW: Should we use Arrays.fill() instead of just
                //         allocating a new array?  Should we use the same
                //         sized array?
                Arrays<E>.fill(array, firstIndex, firstIndex + sizeJ, default(E));
                // REVIEW: Should the indexes point into the middle of the
                //         array rather than 0?
                firstIndex = sizeJ = 0;
                modCount++;
            }
        }

        /**
         * Returns a new {@code ArrayList} with the same elements, the same size and
         * the same capacity as this {@code ArrayList}.
         * 
         * @return a shallow copy of this {@code ArrayList}
         * @see java.lang.Cloneable
         */
        public virtual Object clone() {
            try {
                ArrayList<E> newList = new ArrayList<E> (this.sizeJ);
                newList.addAll(this);
                return newList;
            } catch (java.lang.CloneNotSupportedException e) {
                return null;
            }
        }

        /**
         * Searches this {@code ArrayList} for the specified object.
         * 
         * @param object
         *            the object to search for.
         * @return {@code true} if {@code object} is an element of this
         *         {@code ArrayList}, {@code false} otherwise
         */
        public override bool contains(Object obj) {
            int lastIndex = firstIndex + sizeJ;
            if (obj != null) {
                for (int i = firstIndex; i < lastIndex; i++) {
                    if (obj.Equals(array[i])) {
                        return true;
                    }
                }
            } else {
                for (int i = firstIndex; i < lastIndex; i++) {
                    if (array[i] == null) {
                        return true;
                    }
                }
            }
            return false;
        }

        /**
         * Ensures that after this operation the {@code ArrayList} can hold the
         * specified number of elements without further growing.
         * 
         * @param minimumCapacity
         *            the minimum capacity asked for.
         */
        public virtual void ensureCapacity(int minimumCapacity) {
            int required = minimumCapacity - array.Length;
            if (required > 0) {
                // REVIEW: Why do we check the firstIndex first? Growing
                //         the end makes more sense
                if (firstIndex > 0) {
                    growAtFront(required);
                } else {
                    growAtEnd(required);
                }
            }
        }

        
        public override E get(int location) {
            if (location < 0 || location >= sizeJ) {
                throw new java.lang.IndexOutOfBoundsException(
                    "Index: "+location+", Size: "+sizeJ);
            }
            return array[firstIndex + location];
        }

        private void growAtEnd(int required) {
            if (array.Length - sizeJ >= required) {
                // REVIEW: as growAtEnd, why not move size == 0 out as
                //         special case
                if (sizeJ != 0) {
                    java.lang.SystemJ.arraycopy(array, firstIndex, array, 0, sizeJ);
                    int start = sizeJ < firstIndex ? firstIndex : sizeJ;
                    // REVIEW: I think we null too much
                    //         array.length should be lastIndex ?
                    Arrays<E>.fill(array, start, array.Length, default(E));
                }
                firstIndex = 0;
            } else {
                // REVIEW: If size is 0?
                //         Does size/2 seems a little high!
                int increment = sizeJ / 2;
                if (required > increment) {
                    increment = required;
                }
                if (increment < 12) {
                    increment = 12;
                }
                E[] newArray = newElementArray(sizeJ + increment);
                if (sizeJ != 0) {
                    java.lang.SystemJ.arraycopy(array, firstIndex, newArray, 0, sizeJ);
                    firstIndex = 0;
                }
                array = newArray;
            }
        }

        private void growAtFront(int required) {
            if (array.Length - sizeJ >= required) {
                int newFirst = array.Length - sizeJ;
                // REVIEW: as growAtEnd, why not move size == 0 out as
                //         special case
                if (sizeJ != 0) {
                    java.lang.SystemJ.arraycopy(array, firstIndex, array, newFirst, sizeJ);
                    int lastIndex = firstIndex + sizeJ;
                    int length = lastIndex > newFirst ? newFirst : lastIndex;
                    Arrays<E>.fill(array, firstIndex, length, default(E));
                }
                firstIndex = newFirst;
            } else {
                int increment = sizeJ / 2;
                if (required > increment) {
                    increment = required;
                }
                if (increment < 12) {
                    increment = 12;
                }
                E[] newArray = newElementArray(sizeJ + increment);
                if (sizeJ != 0) {
                    java.lang.SystemJ.arraycopy(array, firstIndex, newArray, increment, sizeJ);
                }
                firstIndex = newArray.Length - sizeJ;
                array = newArray;
            }
        }

        private void growForInsert(int location, int required) {
            // REVIEW: we grow too quickly because we are called with the
            //         size of the new collection to add without taking in
            //         to account the free space we already have
            int increment = sizeJ / 2;
            if (required > increment) {
                increment = required;
            }
            if (increment < 12) {
                increment = 12;
            }
            E[] newArray = newElementArray(sizeJ + increment);
            // REVIEW: biased towards leaving space at the beginning?
            //         perhaps newFirst should be (increment-required)/2?
            int newFirst = increment - required;
            // Copy elements after location to the new array skipping inserted
            // elements
            java.lang.SystemJ.arraycopy(array, location + firstIndex, newArray, newFirst
                    + location + required, sizeJ - location);
            // Copy elements before location to the new array from firstIndex
            java.lang.SystemJ.arraycopy(array, firstIndex, newArray, newFirst, location);
            firstIndex = newFirst;
            array = newArray;
        }

        public override int indexOf(Object obj) {
            // REVIEW: should contains call this method?
            int lastIndex = firstIndex + sizeJ;
            if (obj != null) {
                for (int i = firstIndex; i < lastIndex; i++) {
                    if (obj.Equals(array[i])) {
                        return i - firstIndex;
                    }
                }
            } else {
                for (int i = firstIndex; i < lastIndex; i++) {
                    if (array[i] == null) {
                        return i - firstIndex;
                    }
                }
            }
            return -1;
        }

        
        public override bool isEmpty() {
            return sizeJ == 0;
        }

        
        public override int lastIndexOf(Object obj) {
            int lastIndex = firstIndex + sizeJ;
            if (obj != null) {
                for (int i = lastIndex - 1; i >= firstIndex; i--) {
                    if (obj.Equals(array[i])) {
                        return i - firstIndex;
                    }
                }
            } else {
                for (int i = lastIndex - 1; i >= firstIndex; i--) {
                    if (array[i] == null) {
                        return i - firstIndex;
                    }
                }
            }
            return -1;
        }

        /**
         * Removes the object at the specified location from this list.
         * 
         * @param location
         *            the index of the object to remove.
         * @return the removed object.
         * @throws IndexOutOfBoundsException
         *             when {@code location < 0 || >= size()}
         */
        
        public override E remove(int location) {
            E result;
            if (location < 0 || location >= sizeJ) {
                throw new java.lang.IndexOutOfBoundsException(
                        "Index: "+location+", Size: "+sizeJ);
            }
            if (location == 0) {
                result = array[firstIndex];
                array[firstIndex++] = default(E);
            } else if (location == sizeJ - 1) {
                int lastIndex = firstIndex + sizeJ - 1;
                result = array[lastIndex];
                array[lastIndex] = default(E);
            } else {
                int elementIndex = firstIndex + location;
                result = array[elementIndex];
                if (location < sizeJ / 2) {
                    java.lang.SystemJ.arraycopy(array, firstIndex, array, firstIndex + 1,
                                     location);
                    array[firstIndex++] = default(E);
                } else {
                    java.lang.SystemJ.arraycopy(array, elementIndex + 1, array,
                                     elementIndex, sizeJ - location - 1);
                    array[firstIndex+sizeJ-1] = default(E);
                }
            }
            sizeJ--;

            // REVIEW: we can move this to the first if case since it
            //         can only occur when size==1
            if (sizeJ == 0) {
                firstIndex = 0;
            }

            modCount++;
            return result;
        }

        
        public override bool remove(Object obj) {
            int location = indexOf(obj);
            if (location >= 0) {
                remove(location);
                return true;
            }
            return false;
        }

        /**
         * Removes the objects in the specified range from the start to the end, but
         * not including the end index.
         * 
         * @param start
         *            the index at which to start removing.
         * @param end
         *            the index one after the end of the range to remove.
         * @throws IndexOutOfBoundsException
         *             when {@code start < 0, start > end} or {@code end > size()}
         */
        
        protected internal override void removeRange(int start, int end) {
            // REVIEW: does RI call this from remove(location)
            if (start < 0) {
                // REVIEW: message should indicate which index is out of range
                throw new java.lang.IndexOutOfBoundsException(
                        "Array index out of range: "+start);
            } else if (end > sizeJ) {
                // REVIEW: message should indicate which index is out of range
                throw new java.lang.IndexOutOfBoundsException(
                        "Index: "+end+", Size: "+sizeJ);
            } else if (start > end) {
                throw new java.lang.IndexOutOfBoundsException(
                        "Start index ("+start+") is greater than end index ("+end+")");
            }

            if (start == end) {
                return;
            }
            if (end == sizeJ) {
                Arrays<E>.fill(array, firstIndex + start, firstIndex + sizeJ, default(E));
            } else if (start == 0) {
                Arrays<E>.fill(array, firstIndex, firstIndex + end, default(E));
                firstIndex += end;
            } else {
                // REVIEW: should this optimize to do the smallest copy?
                java.lang.SystemJ.arraycopy(array, firstIndex + end, array, firstIndex
                                 + start, sizeJ - end);
                int lastIndex = firstIndex + sizeJ;
                int newLast = lastIndex + start - end;
                Arrays<E>.fill(array, newLast, lastIndex, default(E));
            }
            sizeJ -= end - start;
            modCount++;
        }

        /**
         * Replaces the element at the specified location in this {@code ArrayList}
         * with the specified object.
         * 
         * @param location
         *            the index at which to put the specified object.
         * @param object
         *            the object to add.
         * @return the previous element at the index.
         * @throws IndexOutOfBoundsException
         *             when {@code location < 0 || >= size()}
         */
        
        public override E set(int location, E obj) {
            if (location < 0 || location >= sizeJ) {
                throw new java.lang.IndexOutOfBoundsException(
                        "Index: "+location+", Size: "+sizeJ);
            }
            E result = array[firstIndex + location];
            array[firstIndex + location] = obj;
            return result;
        }

        /**
         * Returns the number of elements in this {@code ArrayList}.
         * 
         * @return the number of elements in this {@code ArrayList}.
         */
        
        public override int size() {
            return sizeJ;
        }

        /**
         * Returns a new array containing all elements contained in this
         * {@code ArrayList}.
         * 
         * @return an array of the elements from this {@code ArrayList}
         */
        
        public override Object[] toArray() {
            Object[] result = new Object[sizeJ];
            java.lang.SystemJ.arraycopy(array, firstIndex, result, 0, sizeJ);
            return result;
        }

        /** --Bastie: why not use the base clase method? --
         * Returns an array containing all elements contained in this
         * {@code ArrayList}. If the specified array is large enough to hold the
         * elements, the specified array is used, otherwise an array of the same
         * type is created. If the specified array is used and is larger than this
         * {@code ArrayList}, the array element following the collection elements
         * is set to null.
         * 
         * @param contents
         *            the array.
         * @return an array of the elements from this {@code ArrayList}.
         * @throws ArrayStoreException
         *             when the type of an element in this {@code ArrayList} cannot
         *             be stored in the type of the specified array.
         *
        public override T[] toArray<T> (T[] contents) {
            if (sizeJ > contents.Length) {
                Class<?> ct = contents.getClass().getComponentType();
                contents = (T[]) Array.newInstance(ct, sizeJ);
            }
            System.arraycopy(array, firstIndex, contents, 0, sizeJ);
            if (sizeJ < contents.length) {
                // REVIEW: do we use this incorrectly - i.e. do we null
                //         the rest out?
                contents[sizeJ] = null;
            }
            return contents;
        }*/

        /**
         * Sets the capacity of this {@code ArrayList} to be the same as the current
         * size.
         * 
         * @see #size
         */
        public virtual void trimToSize() {
            E[] newArray = newElementArray(sizeJ);
            java.lang.SystemJ.arraycopy(array, firstIndex, newArray, 0, sizeJ);
            array = newArray;
            firstIndex = 0;
            modCount = 0;
        }
/*
        private static ObjectStreamField[] serialPersistentFields = { new ObjectStreamField(
                "size", Integer.TYPE) }; //$NON-NLS-1$

        private void writeObject(ObjectOutputStream stream) throws IOException {
            ObjectOutputStream.PutField fields = stream.putFields();
            fields.put("size", sizeJ); //$NON-NLS-1$
            stream.writeFields();
            stream.writeInt(array.length);
            Iterator<?> it = iterator();
            while (it.hasNext()) {
                stream.writeObject(it.next());
            }
        }

        private void readObject(ObjectInputStream stream) throws IOException,
                ClassNotFoundException {
            ObjectInputStream.GetField fields = stream.readFields();
            size = fields.get("size", 0); //$NON-NLS-1$
            array = newElementArray(stream.readInt());
            for (int i = 0; i < size; i++) {
                array[i] = (E) stream.readObject();
            }
        }
*/
    }
}
