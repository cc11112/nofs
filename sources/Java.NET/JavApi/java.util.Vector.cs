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
using System.Text;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util
{

    /**
     * Vector is a variable size contiguous indexable array of objects. The size of
     * the vector is the number of objects it contains. The capacity of the vector
     * is the number of objects it can hold.
     * <p>
     * Objects may be inserted at any position up to the size of the vector, thus
     * increasing the size of the vector. Objects at any position in the vector may
     * be removed, thus shrinking the size of the Vector. Objects at any position in
     * the Vector may be replaced, which does not affect the vector's size.
     * <p>
     * The capacity of a vector may be specified when the vector is created. If the
     * capacity of the vector is exceeded, the capacity is increased (doubled by
     * default).
     * 
     * @see java.lang.StringBuffer
     */
    [Serializable]
    public class Vector<E> : AbstractList<E>, List<E>, RandomAccess, java.lang.Cloneable, java.io.Serializable {

        private static readonly long serialVersionUID = -2767605614048989439L;

        /**
         * The number of elements or the size of the vector.
         */
        protected internal int elementCount;

        /**
         * The elements of the vector.
         */
        protected internal E[] elementData;

        /**
         * How many elements should be added to the vector when it is detected that
         * it needs to grow to accommodate extra entries. If this value is zero or
         * negative the size will be doubled if an increase is needed.
         */
        protected int capacityIncrement;

        private static readonly int DEFAULT_SIZE = 10;

        /**
         * Constructs a new vector using the default capacity.
         */
        public Vector() :this(DEFAULT_SIZE, 0){
        }

        /**
         * Constructs a new vector using the specified capacity.
         * 
         * @param capacity
         *            the initial capacity of the new vector.
         * @throws IllegalArgumentException
         *             if {@code capacity} is negative.
         */
        public Vector(int capacity) :this(capacity, 0){
        }

        /**
         * Constructs a new vector using the specified capacity and capacity
         * increment.
         * 
         * @param capacity
         *            the initial capacity of the new vector.
         * @param capacityIncrement
         *            the amount to increase the capacity when this vector is full.
         * @throws IllegalArgumentException
         *             if {@code capacity} is negative.
         */
        public Vector(int capacity, int capacityIncrement) {
            if (capacity < 0) {
                throw new java.lang.IllegalArgumentException();
            }
            elementData = newElementArray(capacity);
            elementCount = 0;
            this.capacityIncrement = capacityIncrement;
        }

        /**
         * Constructs a new instance of {@code Vector} containing the elements in
         * {@code collection}. The order of the elements in the new {@code Vector}
         * is dependent on the iteration order of the seed collection.
         * 
         * @param collection
         *            the collection of elements to add.
         */
        public Vector(Collection<E> collection) :this(collection.size(), 0){
            Iterator<E> it = collection.iterator();
            while (it.hasNext()) {
                elementData[elementCount++] = it.next();
            }
        }

        private E[] newElementArray(int size) {
            return new E[size];
        }

        /**
         * Adds the specified object into this vector at the specified location. The
         * object is inserted before any element with the same or a higher index
         * increasing their index by 1. If the location is equal to the size of this
         * vector, the object is added at the end.
         * 
         * @param location
         *            the index at which to insert the element.
         * @param object
         *            the object to insert in this vector.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code location < 0 || location > size()}.
         * @see #addElement
         * @see #size
         */
        
        public override void add(int location, E obj) {
            insertElementAt(obj, location);
        }

        /**
         * Adds the specified object at the end of this vector.
         * 
         * @param object
         *            the object to add to the vector.
         * @return {@code true}
         */
        
        public override bool add(E obj) {
            lock (this) {
                if (elementCount == elementData.Length) {
                    growByOne();
                }
                elementData[elementCount++] = obj;
                modCount++;
                return true;
            }
        }

        /**
         * Inserts the objects in the specified collection at the specified location
         * in this vector. The objects are inserted in the order in which they are
         * returned from the Collection iterator. The elements with an index equal
         * or higher than {@code location} have their index increased by the size of
         * the added collection.
         * 
         * @param location
         *            the location to insert the objects.
         * @param collection
         *            the collection of objects.
         * @return {@code true} if this vector is modified, {@code false} otherwise.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code location < 0} or {@code location > size()}.
         */
        
        public override bool addAll(int location, Collection<E> collection) {
            lock (this){
                if (0 <= location && location <= elementCount) {
                    int size = collection.size();
                    if (size == 0) {
                        return false;
                    }
                    int required = size - (elementData.Length - elementCount);
                    if (required > 0) {
                        growBy(required);
                    }
                    int count = elementCount - location;
                    if (count > 0) {
                        java.lang.SystemJ.arraycopy(elementData, location, elementData, location
                                + size, count);
                    }
                    Iterator<E> it = collection.iterator();
                    while (it.hasNext()) {
                        elementData[location++] = it.next();
                    }
                    elementCount += size;
                    modCount++;
                    return true;
                }
                throw new java.lang.ArrayIndexOutOfBoundsException(location);
            }
        }

        /**
         * Adds the objects in the specified collection to the end of this vector.
         * 
         * @param collection
         *            the collection of objects.
         * @return {@code true} if this vector is modified, {@code false} otherwise.
         */
        
        public override bool addAll(Collection<E> collection) {
            lock (this) {
                return addAll(elementCount, collection);
            }
        }

        /**
         * Adds the specified object at the end of this vector.
         * 
         * @param object
         *            the object to add to the vector.
         */
        public virtual void addElement(E obj) {
            lock (this) {
                if (elementCount == elementData.Length) {
                    growByOne();
                }
                elementData[elementCount++] = obj;
                modCount++;
            }
        }

        /**
         * Returns the number of elements this vector can hold without growing.
         * 
         * @return the capacity of this vector.
         * @see #ensureCapacity
         * @see #size
         */
        public virtual int capacity() {
            lock (this) {
                return elementData.Length;
            }
        }

        /**
         * Removes all elements from this vector, leaving it empty.
         * 
         * @see #isEmpty
         * @see #size
         */
        
        public override void clear() {
            removeAllElements();
        }

        /**
         * Returns a new vector with the same elements, size, capacity and capacity
         * increment as this vector.
         * 
         * @return a shallow copy of this vector.
         * @see java.lang.Cloneable
         */
        public Object clone() {
            lock (this) {
                try {
                    Vector<E> vector = (Vector<E>) base.MemberwiseClone();
                    vector.elementData = (E[])elementData.Clone();
                    return vector;
                } catch (java.lang.CloneNotSupportedException e) {
                    return null;
                }
            }
        }

        /**
         * Searches this vector for the specified object.
         * 
         * @param object
         *            the object to look for in this vector.
         * @return {@code true} if object is an element of this vector,
         *         {@code false} otherwise.
         * @see #indexOf(Object)
         * @see #indexOf(Object, int)
         * @see java.lang.Object#equals
         */
        
        public override bool contains(Object obj) {
            lock (this) {
                return indexOf(obj, 0) != -1;
            }
        }

        /**
         * Searches this vector for all objects in the specified collection.
         * 
         * @param collection
         *            the collection of objects.
         * @return {@code true} if all objects in the specified collection are
         *         elements of this vector, {@code false} otherwise.
         */
        public override bool containsAll(Collection<E> collection) {
            lock (this) {
                return base.containsAll(collection);
            }
        }

        /**
         * Attempts to copy elements contained by this {@code Vector} into the
         * corresponding elements of the supplied {@code Object} array.
         * 
         * @param elements
         *            the {@code Object} array into which the elements of this
         *            vector are copied.
         * @throws IndexOutOfBoundsException
         *             if {@code elements} is not big enough.
         * @see #clone
         */
        public virtual void copyInto(Object[] elements) {
            lock (this) {
                java.lang.SystemJ.arraycopy(elementData, 0, elements, 0, elementCount);
            }
        }

        /**
         * Returns the element at the specified location in this vector.
         * 
         * @param location
         *            the index of the element to return in this vector.
         * @return the element at the specified location.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code location < 0 || location >= size()}.
         * @see #size
         */
        public virtual E elementAt(int location) {
            lock (this) {
                if (location < elementCount) {
                    return (E) elementData[location];
                }
                throw new java.lang.ArrayIndexOutOfBoundsException(location);
            }
        }

        /**
         * Returns an enumeration on the elements of this vector. The results of the
         * enumeration may be affected if the contents of this vector is modified.
         * 
         * @return an enumeration of the elements of this vector.
         * @see #elementAt
         * @see Enumeration
         */
        public virtual Enumeration<E> elements() {
            return new IAC_VECTOR_ENUMERATION<E> (this);
        }

        /**
         * Ensures that this vector can hold the specified number of elements
         * without growing.
         * 
         * @param minimumCapacity
         *            the minimum number of elements that this vector will hold
         *            before growing.
         * @see #capacity
         */
        public virtual void ensureCapacity(int minimumCapacity) {
            lock (this) {
                if (elementData.Length < minimumCapacity) {
                    int next = (capacityIncrement <= 0 ? elementData.Length
                            : capacityIncrement)
                            + elementData.Length;
                    grow(minimumCapacity > next ? minimumCapacity : next);
                }
            }
        }

        /**
         * Compares the specified object to this vector and returns if they are
         * equal. The object must be a List which contains the same objects in the
         * same order.
         * 
         * @param object
         *            the object to compare with this object
         * @return {@code true} if the specified object is equal to this vector,
         *         {@code false} otherwise.
         * @see #hashCode
         */
        
        public override bool Equals(Object obj) {
            lock (this) {
                if (this == obj) {
                    return true;
                }
                if (obj is java.util.List<Object>) {
                    java.util.List<Object> list = (java.util.List<Object>) obj;
                    if (list.size() != elementCount) {
                        return false;
                    }

                    int index = 0;
                    Iterator<Object> it = list.iterator();
                    while (it.hasNext()) {
                        Object e1 = elementData[index++], e2 = it.next();
                        if (!(e1 == null ? e2 == null : e1.equals(e2))) {
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            }
        }

        /**
         * Returns the first element in this vector.
         * 
         * @return the element at the first position.
         * @throws NoSuchElementException
         *                if this vector is empty.
         * @see #elementAt
         * @see #lastElement
         * @see #size
         */
        public virtual E firstElement() {
            lock (this) {
                if (elementCount > 0) {
                    return (E) elementData[0];
                }
                throw new NoSuchElementException();
            }
        }

        /**
         * Returns the element at the specified location in this vector.
         * 
         * @param location
         *            the index of the element to return in this vector.
         * @return the element at the specified location.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code location < 0 || location >= size()}.
         * @see #size
         */
        
        public override E get(int location) {
            return elementAt(location);
        }

        private void grow(int newCapacity) {
            E[] newData = newElementArray(newCapacity);
            // Assumes elementCount is <= newCapacity
            if (elementCount > newCapacity) throw new java.lang.IllegalArgumentException ("Assumes elementCount is <= newCapacity");//assert elementCount <= newCapacity;
            java.lang.SystemJ.arraycopy(elementData, 0, newData, 0, elementCount);
            elementData = newData;
        }

        /**
         * JIT optimization
         */
        private void growByOne() {
            int adding = 0;
            if (capacityIncrement <= 0) {
                if ((adding = elementData.Length) == 0) {
                    adding = 1;
                }
            } else {
                adding = capacityIncrement;
            }

            E[] newData = newElementArray(elementData.Length + adding);
            java.lang.SystemJ.arraycopy(elementData, 0, newData, 0, elementCount);
            elementData = newData;
        }

        private void growBy(int required) {
            int adding = 0;
            if (capacityIncrement <= 0) {
                if ((adding = elementData.Length) == 0) {
                    adding = required;
                }
                while (adding < required) {
                    adding += adding;
                }
            } else {
                adding = (required / capacityIncrement) * capacityIncrement;
                if (adding < required) {
                    adding += capacityIncrement;
                }
            }
            E[] newData = newElementArray(elementData.Length + adding);
            java.lang.SystemJ.arraycopy(elementData, 0, newData, 0, elementCount);
            elementData = newData;
        }

        /**
         * Returns an integer hash code for the receiver. Objects which are equal
         * return the same value for this method.
         * 
         * @return the receiver's hash.
         * @see #equals
         */
        public override int GetHashCode() {
            lock (this) {
                int result = 1;
                for (int i = 0; i < elementCount; i++) {
                    result = (31 * result)
                            + (elementData[i] == null ? 0 : elementData[i].GetHashCode());
                }
                return result;
            }
        }

        /**
         * Searches in this vector for the index of the specified object. The search
         * for the object starts at the beginning and moves towards the end of this
         * vector.
         * 
         * @param object
         *            the object to find in this vector.
         * @return the index in this vector of the specified element, -1 if the
         *         element isn't found.
         * @see #contains
         * @see #lastIndexOf(Object)
         * @see #lastIndexOf(Object, int)
         */
        public override int indexOf(Object obj) {
            return indexOf(obj, 0);
        }

        /**
         * Searches in this vector for the index of the specified object. The search
         * for the object starts at the specified location and moves towards the end
         * of this vector.
         * 
         * @param object
         *            the object to find in this vector.
         * @param location
         *            the index at which to start searching.
         * @return the index in this vector of the specified element, -1 if the
         *         element isn't found.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code location < 0}.
         * @see #contains
         * @see #lastIndexOf(Object)
         * @see #lastIndexOf(Object, int)
         */
        public virtual int indexOf(Object obj, int location) {
            lock (this) {
                if (obj != null) {
                    for (int i = location; i < elementCount; i++) {
                        if (obj.equals(elementData[i])) {
                            return i;
                        }
                    }
                } else {
                    for (int i = location; i < elementCount; i++) {
                        if (elementData[i] == null) {
                            return i;
                        }
                    }
                }
                return -1;
            }
        }

        /**
         * Inserts the specified object into this vector at the specified location.
         * This object is inserted before any previous element at the specified
         * location. All elements with an index equal or greater than
         * {@code location} have their index increased by 1. If the location is
         * equal to the size of this vector, the object is added at the end.
         * 
         * @param object
         *            the object to insert in this vector.
         * @param location
         *            the index at which to insert the element.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code location < 0 || location > size()}.
         * @see #addElement
         * @see #size
         */
        public virtual void insertElementAt(E obj, int location) {
            lock (this) {
                if (0 <= location && location <= elementCount) {
                    if (elementCount == elementData.Length) {
                        growByOne();
                    }
                    int count = elementCount - location;
                    if (count > 0) {
                        java.lang.SystemJ.arraycopy(elementData, location, elementData,
                                location + 1, count);
                    }
                    elementData[location] = obj;
                    elementCount++;
                    modCount++;
                } else {
                    throw new java.lang.ArrayIndexOutOfBoundsException(location);
                }
}
        }

        /**
         * Returns if this vector has no elements, a size of zero.
         * 
         * @return {@code true} if this vector has no elements, {@code false}
         *         otherwise.
         * @see #size
         */
        public override bool isEmpty() {
            lock (this) {
                return elementCount == 0;
            }
        }

        /**
         * Returns the last element in this vector.
         * 
         * @return the element at the last position.
         * @throws NoSuchElementException
         *                if this vector is empty.
         * @see #elementAt
         * @see #firstElement
         * @see #size
         */
        public virtual E lastElement() {
            lock (this) {
                try {
                    return (E) elementData[elementCount - 1];
                } catch (java.lang.IndexOutOfBoundsException e) {
                    throw new NoSuchElementException();
                }
            }
        }

        /**
         * Searches in this vector for the index of the specified object. The search
         * for the object starts at the end and moves towards the start of this
         * vector.
         * 
         * @param object
         *            the object to find in this vector.
         * @return the index in this vector of the specified element, -1 if the
         *         element isn't found.
         * @see #contains
         * @see #indexOf(Object)
         * @see #indexOf(Object, int)
         */
        
        public override int lastIndexOf(Object obj) {
            lock (this) {
                return lastIndexOf(obj, elementCount - 1);
            }
        }

        /**
         * Searches in this vector for the index of the specified object. The search
         * for the object starts at the specified location and moves towards the
         * start of this vector.
         * 
         * @param object
         *            the object to find in this vector.
         * @param location
         *            the index at which to start searching.
         * @return the index in this vector of the specified element, -1 if the
         *         element isn't found.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code location >= size()}.
         * @see #contains
         * @see #indexOf(Object)
         * @see #indexOf(Object, int)
         */
        public virtual int lastIndexOf(Object obj, int location) {
            lock (this) {
                if (location < elementCount) {
                    if (obj != null) {
                        for (int i = location; i >= 0; i--) {
                            if (obj.equals(elementData[i])) {
                                return i;
                            }
                        }
                    } else {
                        for (int i = location; i >= 0; i--) {
                            if (elementData[i] == null) {
                                return i;
                            }
                        }
                    }
                    return -1;
                }
                throw new java.lang.ArrayIndexOutOfBoundsException(location);
            }
        }

        /**
         * Removes the object at the specified location from this vector. All
         * elements with an index bigger than {@code location} have their index
         * decreased by 1.
         * 
         * @param location
         *            the index of the object to remove.
         * @return the removed object.
         * @throws IndexOutOfBoundsException
         *                if {@code location < 0 || location >= size()}.
         */
        public override E remove(int location) {
            lock (this) {
                if (location < elementCount) {
                    E result = (E) elementData[location];
                    elementCount--;
                    int size = elementCount - location;
                    if (size > 0) {
                        java.lang.SystemJ.arraycopy(elementData, location + 1, elementData,
                                location, size);
                    }
                    elementData[elementCount] = default(E);
                    modCount++;
                    return result;
                }
                throw new java.lang.ArrayIndexOutOfBoundsException(location);
            }
        }

        /**
         * Removes the first occurrence, starting at the beginning and moving
         * towards the end, of the specified object from this vector. All elements
         * with an index bigger than the element that gets removed have their index
         * decreased by 1.
         * 
         * @param object
         *            the object to remove from this vector.
         * @return {@code true} if the specified object was found, {@code false}
         *         otherwise.
         * @see #removeAllElements
         * @see #removeElementAt
         * @see #size
         */
        public override bool remove(Object obj) {
            return removeElement(obj);
        }

        /**
         * Removes all occurrences in this vector of each object in the specified
         * Collection.
         * 
         * @param collection
         *            the collection of objects to remove.
         * @return {@code true} if this vector is modified, {@code false} otherwise.
         * @see #remove(Object)
         * @see #contains(Object)
         */
        public override bool removeAll(Collection<E> collection) {
            lock (this) {
                return base.removeAll(collection);
            }
        }

        /**
         * Removes all elements from this vector, leaving the size zero and the
         * capacity unchanged.
         * 
         * @see #isEmpty
         * @see #size
         */
        public virtual void removeAllElements() {
            lock (this) {
                for (int i = 0; i < elementCount; i++) {
                    elementData[i] = default(E);
                }
                modCount++;
                elementCount = 0;
            }
        }

        /**
         * Removes the first occurrence, starting at the beginning and moving
         * towards the end, of the specified object from this vector. All elements
         * with an index bigger than the element that gets removed have their index
         * decreased by 1.
         * 
         * @param object
         *            the object to remove from this vector.
         * @return {@code true} if the specified object was found, {@code false}
         *         otherwise.
         * @see #removeAllElements
         * @see #removeElementAt
         * @see #size
         */
        public virtual bool removeElement(Object obj) {
            lock (this) {
                int index;
                if ((index = indexOf(obj, 0)) == -1) {
                    return false;
                }
                removeElementAt(index);
                return true;
            }
        }

        /**
         * Removes the element found at index position {@code location} from
         * this {@code Vector}. All elements with an index bigger than
         * {@code location} have their index decreased by 1.
         * 
         * @param location
         *            the index of the element to remove.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code location < 0 || location >= size()}.
         * @see #removeElement
         * @see #removeAllElements
         * @see #size
         */
        public virtual void removeElementAt(int location) {
            lock (this) {
                if (0 <= location && location < elementCount) {
                    elementCount--;
                    int size = elementCount - location;
                    if (size > 0) {
                        java.lang.SystemJ.arraycopy(elementData, location + 1, elementData,
                                location, size);
                    }
                    elementData[elementCount] = default(E);
                    modCount++;
                } else {
                    throw new java.lang.ArrayIndexOutOfBoundsException(location);
                }
            }
        }

        /**
         * Removes the objects in the specified range from the start to the, but not
         * including, end index. All elements with an index bigger than or equal to
         * {@code end} have their index decreased by {@code end - start}.
         * 
         * @param start
         *            the index at which to start removing.
         * @param end
         *            the index one past the end of the range to remove.
         * @throws IndexOutOfBoundsException
         *                if {@code start < 0, start > end} or
         *                {@code end > size()}.
         */
        protected internal override void removeRange(int start, int end) {
            if (start >= 0 && start <= end && end <= elementCount) {
                if (start == end) {
                    return;
                }
                if (end != elementCount) {
                    java.lang.SystemJ.arraycopy(elementData, end, elementData, start,
                            elementCount - end);
                    int newCount = elementCount - (end - start);
                    Arrays<E>.fill(elementData, newCount, elementCount, default(E));
                    elementCount = newCount;
                } else {
                    Arrays<E>.fill(elementData, start, elementCount, default(E));
                    elementCount = start;
                }
                modCount++;
            } else {
                throw new java.lang.IndexOutOfBoundsException();
            }
        }

        /**
         * Removes all objects from this vector that are not contained in the
         * specified collection.
         * 
         * @param collection
         *            the collection of objects to retain.
         * @return {@code true} if this vector is modified, {@code false} otherwise.
         * @see #remove(Object)
         */
        public override bool retainAll(Collection<E> collection) {
            lock (this) {
                return base.retainAll(collection);
            }
        }

        /**
         * Replaces the element at the specified location in this vector with the
         * specified object.
         * 
         * @param location
         *            the index at which to put the specified object.
         * @param object
         *            the object to add to this vector.
         * @return the previous element at the location.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code location < 0 || location >= size()}.
         * @see #size
         */
        public override E set(int location, E obj) {
            lock (this) {
                if (location < elementCount) {
                    E result = (E) elementData[location];
                    elementData[location] = obj;
                    return result;
                }
                throw new java.lang.ArrayIndexOutOfBoundsException(location);
            }
        }

        /**
         * Replaces the element at the specified location in this vector with the
         * specified object.
         * 
         * @param object
         *            the object to add to this vector.
         * @param location
         *            the index at which to put the specified object.
         * @throws ArrayIndexOutOfBoundsException
         *                if {@code location < 0 || location >= size()}.
         * @see #size
         */
        public virtual void setElementAt(E obj, int location) {
            lock (this) {
                if (location < elementCount) {
                    elementData[location] = obj;
                } else {
                    throw new java.lang.ArrayIndexOutOfBoundsException(location);
                }
            }  
        }

        /**
         * Sets the size of this vector to the specified size. If there are more
         * than length elements in this vector, the elements at end are lost. If
         * there are less than length elements in the vector, the additional
         * elements contain null.
         * 
         * @param length
         *            the new size of this vector.
         * @see #size
         */
        public virtual void setSize(int length) {
            lock (this) {
                if (length == elementCount) {
                    return;
                }
                ensureCapacity(length);
                if (elementCount > length) {
                    Arrays<E>.fill(elementData, length, elementCount, default(E));
                }
                elementCount = length;
                modCount++;
            }
        }

        /**
         * Returns the number of elements in this vector.
         * 
         * @return the number of elements in this vector.
         * @see #elementCount
         * @see #lastElement
         */
        
        public override int size() {
            lock (this) {
                return elementCount;
            }
        }

        /**
         * Returns a List of the specified portion of this vector from the start
         * index to one less than the end index. The returned List is backed by this
         * vector so changes to one are reflected by the other.
         * 
         * @param start
         *            the index at which to start the sublist.
         * @param end
         *            the index one past the end of the sublist.
         * @return a List of a portion of this vector.
         * @throws IndexOutOfBoundsException
         *                if {@code start < 0} or {@code end > size()}.
         * @throws IllegalArgumentException
         *                if {@code start > end}.
         */
        
        public override List<E> subList(int start, int end) {
            lock (this) {
                return new SynchronizedRandomAccessList<E>(base.subList(
                        start, end), this);
            }
        }

        /**
         * Returns a new array containing all elements contained in this vector.
         * 
         * @return an array of the elements from this vector.
         */
        public override Object[] toArray() {
            lock (this) {
                Object[] result = new Object[elementCount];
                java.lang.SystemJ.arraycopy(elementData, 0, result, 0, elementCount);
                return result;
            }
        }

        /**
         * Returns an array containing all elements contained in this vector. If the
         * specified array is large enough to hold the elements, the specified array
         * is used, otherwise an array of the same type is created. If the specified
         * array is used and is larger than this vector, the array element following
         * the collection elements is set to null.
         * 
         * @param contents
         *            the array to fill.
         * @return an array of the elements from this vector.
         * @throws ArrayStoreException
         *                if the type of an element in this vector cannot be
         *                stored in the type of the specified array.
         */
        public override  T[] toArray<T>(T[] contents) {
            lock (this) {
                if (elementCount > contents.Length) {
                    contents = new T[elementCount];
                }
                java.lang.SystemJ.arraycopy(elementData, 0, contents, 0, elementCount);
                if (elementCount < contents.Length) {
                    contents[elementCount] = default(T);
                }
                return contents;
            }
        }

        /**
         * Returns the string representation of this vector.
         * 
         * @return the string representation of this vector.
         * @see #elements
         */
        public override String ToString() {
            lock (this) {
                if (elementCount == 0) {
                    return "[]"; //$NON-NLS-1$
                }
                int length = elementCount - 1;
                StringBuilder buffer = new StringBuilder(elementCount * 16);
                buffer.append('[');
                for (int i = 0; i < length; i++) {
                    if (((Object)this) == ((Object)elementData[i])) {
                        buffer.append("(this Collection)"); //$NON-NLS-1$
                    } else {
                        buffer.append(elementData[i]);
                    }
                    buffer.append(", "); //$NON-NLS-1$
                }
                if (((Object)elementData[length]) == ((Object)this)) {
                    buffer.append("(this Collection)"); //$NON-NLS-1$
                } else {
                    buffer.append(elementData[length]);
                }
                buffer.append(']');
                return buffer.toString();
            }
        }

        /**
         * Sets the capacity of this vector to be the same as the size.
         * 
         * @see #capacity
         * @see #ensureCapacity
         * @see #size
         */
        public virtual void trimToSize() {
            lock (this) {
                if (elementData.Length != elementCount) {
                    grow(elementCount);
                }
            }
        }

/*        private  void writeObject(ObjectOutputStream stream)
                throws IOException {
            stream.defaultWriteObject();
        }*/
    }
#region IAC_VECTOR_ENUMERATION
    internal class IAC_VECTOR_ENUMERATION<E> : Enumeration<E>{
        internal int pos = 0;
        private readonly Vector<E> root;
        public IAC_VECTOR_ENUMERATION (Vector<E> v) {
            this.root = v;
        }
        public bool hasMoreElements() {
            return pos < root.elementCount;
        }

        public E nextElement() {
            lock (this.root) {
                if (pos < root.elementCount) {
                    return (E) root.elementData[pos++];
                }
            }
            throw new java.util.NoSuchElementException();
        }
    }
#endregion
}
