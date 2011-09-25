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

namespace org.apache.commons.collections.list
{

    /**
     * An abstract implementation of a linked list which provides numerous points for
     * subclasses to override.
     * <p>
     * Overridable methods are provided to change the storage node and to change how
     * nodes are added to and removed. Hopefully, all you need for unusual subclasses
     * is here.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Rich Dougherty
     * @author Phil Steitz
     * @author Stephen Colebourne
     */
    public abstract class AbstractLinkedList : java.util.List<Object>
    {

        /*
         * Implementation notes:
         * - a standard circular doubly-linked list
         * - a marker node is stored to mark the start and the end of the list
         * - node creation and removal always occurs through createNode() and
         *   removeNode().
         * - a modification count is kept, with the same semantics as
         * {@link java.util.LinkedList}.
         * - respects {@link AbstractList#modCount}
         */

        /**
         * A {@link Node} which indicates the start and end of the list and does not
         * hold a value. The value of <code>next</code> is the first item in the
         * list. The value of of <code>previous</code> is the last item in the list.
         */
        [NonSerialized]
        protected internal Node header;
        /** The size of the list */
        [NonSerialized]
        protected int sizeJ;
        /** Modification count for iterators */
        [NonSerialized]
        protected internal int modCount;

        /**
         * Constructor that does nothing intended for deserialization.
         * <p>
         * If this constructor is used by a java.io.Serializable subclass then the init()
         * method must be called.
         */
        protected AbstractLinkedList()
            : base()
        {
        }

        /**
         * Constructs a list copying data from the specified collection.
         * 
         * @param coll  the collection to copy
         */
        protected AbstractLinkedList(java.util.Collection<Object> coll)
            : base()
        {
            init();
            addAll(coll);
        }

        /**
         * The equivalent of a default constructor, broken out so it can be called
         * by any constructor and by <code>readObject</code>.
         * Subclasses which override this method should make sure they call super,
         * so the list is initialised properly.
         */
        protected virtual void init()
        {
            header = createHeaderNode();
        }

        //-----------------------------------------------------------------------
        public virtual int size()
        {
            return sizeJ;
        }

        public virtual bool isEmpty()
        {
            return (size() == 0);
        }

        public virtual Object get(int index)
        {
            Node node = getNode(index, false);
            return node.getValue();
        }

        //-----------------------------------------------------------------------
        public virtual java.util.Iterator<Object> iterator()
        {
            return listIterator();
        }

        public virtual java.util.ListIterator<Object> listIterator()
        {
            return new LinkedListIterator(this, 0);
        }

        public virtual java.util.ListIterator<Object> listIterator(int fromIndex)
        {
            return new LinkedListIterator(this, fromIndex);
        }

        //-----------------------------------------------------------------------
        public virtual int indexOf(Object value)
        {
            int i = 0;
            for (Node node = header.next; node != header; node = node.next)
            {
                if (isEqualValue(node.getValue(), value))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        public virtual int lastIndexOf(Object value)
        {
            int i = sizeJ - 1;
            for (Node node = header.previous; node != header; node = node.previous)
            {
                if (isEqualValue(node.getValue(), value))
                {
                    return i;
                }
                i--;
            }
            return -1;
        }

        public virtual bool contains(Object value)
        {
            return indexOf(value) != -1;
        }

        public virtual bool containsAll(java.util.Collection<Object> coll)
        {
            java.util.Iterator<Object> it = coll.iterator();
            while (it.hasNext())
            {
                if (contains(it.next()) == false)
                {
                    return false;
                }
            }
            return true;
        }

        //-----------------------------------------------------------------------
        public virtual Object[] toArray()
        {
            return toArray(new Object[sizeJ]);
        }

        public virtual Object[] toArray<Object>(Object[] array)
        {
            // Extend the array if needed
            if (array.Length < sizeJ)
            {
                // Basties note: Why this next two lines, we only return an array of object instances
                //Class componentType = array.getClass().getComponentType();
                //array = (Object[]) java.lang.reflect.Array.newInstance(componentType, size);
                array = new Object[sizeJ];
            }
            // Copy the values into the array
            int i = 0;
            for (Node node = header.next; node != header; node = node.next, i++)
            {
                array[i] = (Object)node.getValue();
            }
            // Set the value after the last value to null
            if (array.Length > sizeJ)
            {
                array[sizeJ] = default(Object);
            }
            return array;
        }

        /**
         * Gets a sublist of the main list.
         * 
         * @param fromIndexInclusive  the index to start from
         * @param toIndexExclusive  the index to end at
         * @return the new sublist
         */
        public virtual java.util.List<Object> subList(int fromIndexInclusive, int toIndexExclusive)
        {
            return new LinkedSubList(this, fromIndexInclusive, toIndexExclusive);
        }

        //-----------------------------------------------------------------------
        public virtual bool add(Object value)
        {
            addLast(value);
            return true;
        }

        public virtual void add(int index, Object value)
        {
            Node node = getNode(index, true);
            addNodeBefore(node, value);
        }

        public virtual bool addAll(java.util.Collection<Object> coll)
        {
            return addAll(sizeJ, coll);
        }

        public virtual bool addAll(int index, java.util.Collection<Object> coll)
        {
            Node node = getNode(index, true);
            for (java.util.Iterator<Object> itr = coll.iterator(); itr.hasNext(); )
            {
                Object value = itr.next();
                addNodeBefore(node, value);
            }
            return true;
        }

        //-----------------------------------------------------------------------
        public virtual Object remove(int index)
        {
            Node node = getNode(index, false);
            Object oldValue = node.getValue();
            removeNode(node);
            return oldValue;
        }

        public virtual bool remove(Object value)
        {
            for (Node node = header.next; node != header; node = node.next)
            {
                if (isEqualValue(node.getValue(), value))
                {
                    removeNode(node);
                    return true;
                }
            }
            return false;
        }

        public virtual bool removeAll(java.util.Collection<Object> coll)
        {
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

        //-----------------------------------------------------------------------
        public virtual bool retainAll(java.util.Collection<Object> coll)
        {
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

        public virtual Object set(int index, Object value)
        {
            Node node = getNode(index, false);
            Object oldValue = node.getValue();
            updateNode(node, value);
            return oldValue;
        }

        public virtual void clear()
        {
            removeAllNodes();
        }

        //-----------------------------------------------------------------------
        public virtual Object getFirst()
        {
            Node node = header.next;
            if (node == header)
            {
                throw new java.util.NoSuchElementException();
            }
            return node.getValue();
        }

        public virtual Object getLast()
        {
            Node node = header.previous;
            if (node == header)
            {
                throw new java.util.NoSuchElementException();
            }
            return node.getValue();
        }

        public virtual bool addFirst(Object o)
        {
            addNodeAfter(header, o);
            return true;
        }

        public virtual bool addLast(Object o)
        {
            addNodeBefore(header, o);
            return true;
        }

        public virtual Object removeFirst()
        {
            Node node = header.next;
            if (node == header)
            {
                throw new java.util.NoSuchElementException();
            }
            Object oldValue = node.getValue();
            removeNode(node);
            return oldValue;
        }

        public virtual Object removeLast()
        {
            Node node = header.previous;
            if (node == header)
            {
                throw new java.util.NoSuchElementException();
            }
            Object oldValue = node.getValue();
            removeNode(node);
            return oldValue;
        }

        //-----------------------------------------------------------------------
        public override bool Equals(Object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (obj is java.util.List<Object> == false)
            {
                return false;
            }
            java.util.List<Object> other = (java.util.List<Object>)obj;
            if (other.size() != size())
            {
                return false;
            }
            java.util.ListIterator<Object> it1 = listIterator();
            java.util.ListIterator<Object> it2 = other.listIterator();
            while (it1.hasNext() && it2.hasNext())
            {
                Object o1 = it1.next();
                Object o2 = it2.next();
                if (!(o1 == null ? o2 == null : o1.equals(o2)))
                    return false;
            }
            return !(it1.hasNext() || it2.hasNext());
        }

        public override int GetHashCode()
        {
            int hashCode = 1;
            java.util.Iterator<Object> it = iterator();
            while (it.hasNext())
            {
                Object obj = it.next();
                hashCode = 31 * hashCode + (obj == null ? 0 : obj.GetHashCode());
            }
            return hashCode;
        }

        public override String ToString()
        {
            if (size() == 0)
            {
                return "[]";
            }
            java.lang.StringBuffer buf = new java.lang.StringBuffer(16 * size());
            buf.append("[");

            java.util.Iterator<Object> it = iterator();
            bool hasNext = it.hasNext();
            while (hasNext)
            {
                Object value = it.next();
                buf.append(value == this ? "(this Collection)" : value);
                hasNext = it.hasNext();
                if (hasNext)
                {
                    buf.append(", ");
                }
            }
            buf.append("]");
            return buf.toString();
        }

        //-----------------------------------------------------------------------
        /**
         * Compares two values for equals.
         * This implementation uses the equals method.
         * Subclasses can override this to match differently.
         * 
         * @param value1  the first value to compare, may be null
         * @param value2  the second value to compare, may be null
         * @return true if equal
         */
        protected virtual bool isEqualValue(Object value1, Object value2)
        {
            return (value1 == value2 || (value1 == null ? false : value1.equals(value2)));
        }

        /**
         * Updates the node with a new value.
         * This implementation sets the value on the node.
         * Subclasses can override this to record the change.
         * 
         * @param node  node to update
         * @param value  new value of the node
         */
        protected virtual void updateNode(Node node, Object value)
        {
            node.setValue(value);
        }

        /**
         * Creates a new node with previous, next and element all set to null.
         * This implementation creates a new empty Node.
         * Subclasses can override this to create a different class.
         * 
         * @return  newly created node
         */
        protected virtual Node createHeaderNode()
        {
            return new Node();
        }

        /**
         * Creates a new node with the specified properties.
         * This implementation creates a new Node with data.
         * Subclasses can override this to create a different class.
         * 
         * @param value  value of the new node
         */
        protected virtual Node createNode(Object value)
        {
            return new Node(value);
        }

        /**
         * Creates a new node with the specified object as its 
         * <code>value</code> and inserts it before <code>node</code>.
         * <p>
         * This implementation uses {@link #createNode(Object)} and
         * {@link #addNode(AbstractLinkedList.Node,AbstractLinkedList.Node)}.
         *
         * @param node  node to insert before
         * @param value  value of the newly added node
         * @throws NullPointerException if <code>node</code> is null
         */
        protected internal virtual void addNodeBefore(Node node, Object value)
        {
            Node newNode = createNode(value);
            addNode(newNode, node);
        }

        /**
         * Creates a new node with the specified object as its 
         * <code>value</code> and inserts it after <code>node</code>.
         * <p>
         * This implementation uses {@link #createNode(Object)} and
         * {@link #addNode(AbstractLinkedList.Node,AbstractLinkedList.Node)}.
         * 
         * @param node  node to insert after
         * @param value  value of the newly added node
         * @throws NullPointerException if <code>node</code> is null
         */
        protected virtual void addNodeAfter(Node node, Object value)
        {
            Node newNode = createNode(value);
            addNode(newNode, node.next);
        }

        /**
         * Inserts a new node into the list.
         *
         * @param nodeToInsert  new node to insert
         * @param insertBeforeNode  node to insert before
         * @throws NullPointerException if either node is null
         */
        protected virtual void addNode(Node nodeToInsert, Node insertBeforeNode)
        {
            nodeToInsert.next = insertBeforeNode;
            nodeToInsert.previous = insertBeforeNode.previous;
            insertBeforeNode.previous.next = nodeToInsert;
            insertBeforeNode.previous = nodeToInsert;
            sizeJ++;
            modCount++;
        }

        /**
         * Removes the specified node from the list.
         *
         * @param node  the node to remove
         * @throws NullPointerException if <code>node</code> is null
         */
        protected internal virtual void removeNode(Node node)
        {
            node.previous.next = node.next;
            node.next.previous = node.previous;
            sizeJ--;
            modCount++;
        }

        /**
         * Removes all nodes by resetting the circular list marker.
         */
        protected virtual void removeAllNodes()
        {
            header.next = header;
            header.previous = header;
            sizeJ = 0;
            modCount++;
        }

        /**
         * Gets the node at a particular index.
         * 
         * @param index  the index, starting from 0
         * @param endMarkerAllowed  whether or not the end marker can be returned if
         * startIndex is set to the list's size
         * @throws IndexOutOfBoundsException if the index is less than 0; equal to
         * the size of the list and endMakerAllowed is false; or greater than the
         * size of the list
         */
        protected internal virtual Node getNode(int index, bool endMarkerAllowed)
        {//throws IndexOutOfBoundsException {
            // Check the index is within the bounds
            if (index < 0)
            {
                throw new java.lang.IndexOutOfBoundsException("Couldn't get the node: " +
                        "index (" + index + ") less than zero.");
            }
            if (!endMarkerAllowed && index == sizeJ)
            {
                throw new java.lang.IndexOutOfBoundsException("Couldn't get the node: " +
                        "index (" + index + ") is the size of the list.");
            }
            if (index > sizeJ)
            {
                throw new java.lang.IndexOutOfBoundsException("Couldn't get the node: " +
                        "index (" + index + ") greater than the size of the " +
                        "list (" + sizeJ + ").");
            }
            // Search the list and get the node
            Node node;
            if (index < (sizeJ / 2))
            {
                // Search forwards
                node = header.next;
                for (int currentIndex = 0; currentIndex < index; currentIndex++)
                {
                    node = node.next;
                }
            }
            else
            {
                // Search backwards
                node = header;
                for (int currentIndex = sizeJ; currentIndex > index; currentIndex--)
                {
                    node = node.previous;
                }
            }
            return node;
        }

        //-----------------------------------------------------------------------
        /**
         * Creates an iterator for the sublist.
         * 
         * @param subList  the sublist to get an iterator for
         */
        protected internal virtual java.util.Iterator<Object> createSubListIterator(LinkedSubList subList)
        {
            return createSubListListIterator(subList, 0);
        }

        /**
         * Creates a list iterator for the sublist.
         * 
         * @param subList  the sublist to get an iterator for
         * @param fromIndex  the index to start from, relative to the sublist
         */
        protected internal virtual java.util.ListIterator<Object> createSubListListIterator(LinkedSubList subList, int fromIndex)
        {
            return new LinkedSubListIterator(subList, fromIndex);
        }

        //-----------------------------------------------------------------------
        /**
         * Serializes the data held in this object to the stream specified.
         * <p>
         * The first java.io.Serializable subclass must call this method from
         * <code>writeObject</code>.
         */
        protected virtual void doWriteObject(java.io.ObjectOutputStream outputStream)
        {//throws IOException {
            // Write the size so we know how many nodes to read back
            outputStream.writeInt(size());
            for (java.util.Iterator<Object> itr = iterator(); itr.hasNext(); )
            {
                outputStream.writeObject(itr.next());
            }
        }

        /**
         * Deserializes the data held in this object to the stream specified.
         * <p>
         * The first java.io.Serializable subclass must call this method from
         * <code>readObject</code>.
         */
        protected virtual void doReadObject(java.io.ObjectInputStream inputStream)
        {//throws IOException, ClassNotFoundException {
            init();
            int size = inputStream.readInt();
            for (int i = 0; i < size; i++)
            {
                add(inputStream.readObject());
            }
        }


    }

    //-----------------------------------------------------------------------
    /**
     * A node within the linked list.
     * <p>
     * From Commons Collections 3.1, all access to the <code>value</code> property
     * is via the methods on this class.
     */
    public class Node
    {

        /** A pointer to the node before this node */
        protected internal Node previous;
        /** A pointer to the node after this node */
        protected internal Node next;
        /** The object contained within this node */
        protected Object value;

        /**
         * Constructs a new header node.
         */
        protected internal Node()
            : base()
        {
            previous = this;
            next = this;
        }

        /**
         * Constructs a new node.
         * 
         * @param value  the value to store
         */
        protected internal Node(Object value)
            : base()
        {
            this.value = value;
        }

        /**
         * Constructs a new node.
         * 
         * @param previous  the previous node in the list
         * @param next  the next node in the list
         * @param value  the value to store
         */
        protected internal Node(Node previous, Node next, Object value)
            : base()
        {
            this.previous = previous;
            this.next = next;
            this.value = value;
        }

        /**
         * Gets the value of the node.
         * 
         * @return the value
         * @since Commons Collections 3.1
         */
        protected internal virtual Object getValue()
        {
            return value;
        }

        /**
         * Sets the value of the node.
         * 
         * @param value  the value
         * @since Commons Collections 3.1
         */
        protected internal virtual void setValue(Object value)
        {
            this.value = value;
        }

        /**
         * Gets the previous node.
         * 
         * @return the previous node
         * @since Commons Collections 3.1
         */
        protected internal virtual Node getPreviousNode()
        {
            return previous;
        }

        /**
         * Sets the previous node.
         * 
         * @param previous  the previous node
         * @since Commons Collections 3.1
         */
        protected internal virtual void setPreviousNode(Node previous)
        {
            this.previous = previous;
        }

        /**
         * Gets the next node.
         * 
         * @return the next node
         * @since Commons Collections 3.1
         */
        protected virtual Node getNextNode()
        {
            return next;
        }

        /**
         * Sets the next node.
         * 
         * @param next  the next node
         * @since Commons Collections 3.1
         */
        protected virtual void setNextNode(Node next)
        {
            this.next = next;
        }
    }

    //-----------------------------------------------------------------------
    /**
     * A list iterator over the linked list.
     */
    public class LinkedListIterator : java.util.ListIterator<Object>, OrderedIterator
    {

        /** The parent list */
        protected readonly AbstractLinkedList parent;

        /**
         * The node that will be returned by {@link #next()}. If this is equal
         * to {@link AbstractLinkedList#header} then there are no more values to return.
         */
        protected Node nextJ;

        /**
         * The index of {@link #next}.
         */
        protected int nextIndexJ;

        /**
         * The last node that was returned by {@link #next()} or {@link
         * #previous()}. Set to <code>null</code> if {@link #next()} or {@link
         * #previous()} haven't been called, or if the node has been removed
         * with {@link #remove()} or a new node added with {@link #add(Object)}.
         * Should be accessed through {@link #getLastNodeReturned()} to enforce
         * this behaviour.
         */
        protected Node current;

        /**
         * The modification count that the list is expected to have. If the list
         * doesn't have this count, then a
         * {@link java.util.ConcurrentModificationException} may be thrown by
         * the operations.
         */
        protected int expectedModCount;

        /**
         * Create a ListIterator for a list.
         * 
         * @param parent  the parent list
         * @param fromIndex  the index to start at
         */
        protected internal LinkedListIterator(AbstractLinkedList parent, int fromIndex)
            : base()
        {// throws IndexOutOfBoundsException {
            this.parent = parent;
            this.expectedModCount = parent.modCount;
            this.nextJ = parent.getNode(fromIndex, true);
            this.nextIndexJ = fromIndex;
        }

        /**
         * Checks the modification count of the list is the value that this
         * object expects.
         * 
         * @throws ConcurrentModificationException If the list's modification
         * count isn't the value that was expected.
         */
        protected virtual void checkModCount()
        {
            if (parent.modCount != expectedModCount)
            {
                throw new java.util.ConcurrentModificationException();
            }
        }

        /**
         * Gets the last node returned.
         * 
         * @throws IllegalStateException If {@link #next()} or
         * {@link #previous()} haven't been called, or if the node has been removed
         * with {@link #remove()} or a new node added with {@link #add(Object)}.
         */
        protected virtual Node getLastNodeReturned()
        {//throws IllegalStateException {
            if (current == null)
            {
                throw new java.lang.IllegalStateException();
            }
            return current;
        }

        public virtual bool hasNext()
        {
            return nextJ != parent.header;
        }

        public virtual Object next()
        {
            checkModCount();
            if (!hasNext())
            {
                throw new java.util.NoSuchElementException("No element at index " + nextIndexJ + ".");
            }
            Object value = nextJ.getValue();
            current = nextJ;
            nextJ = nextJ.next;
            nextIndexJ++;
            return value;
        }

        public virtual bool hasPrevious()
        {
            return nextJ.previous != parent.header;
        }

        public virtual Object previous()
        {
            checkModCount();
            if (!hasPrevious())
            {
                throw new java.util.NoSuchElementException("Already at start of list.");
            }
            nextJ = nextJ.previous;
            Object value = nextJ.getValue();
            current = nextJ;
            nextIndexJ--;
            return value;
        }

        public virtual int nextIndex()
        {
            return nextIndexJ;
        }

        public virtual int previousIndex()
        {
            // not normally overridden, as relative to nextIndex()
            return nextIndex() - 1;
        }

        public virtual void remove()
        {
            checkModCount();
            if (current == nextJ)
            {
                // remove() following previous()
                nextJ = nextJ.next;
                parent.removeNode(getLastNodeReturned());
            }
            else
            {
                // remove() following next()
                parent.removeNode(getLastNodeReturned());
                nextIndexJ--;
            }
            current = null;
            expectedModCount++;
        }

        public virtual void set(Object obj)
        {
            checkModCount();
            getLastNodeReturned().setValue(obj);
        }

        public virtual void add(Object obj)
        {
            checkModCount();
            parent.addNodeBefore(nextJ, obj);
            current = null;
            nextIndexJ++;
            expectedModCount++;
        }

    }

    //-----------------------------------------------------------------------
    /**
     * A list iterator over the linked sub list.
     */
    public class LinkedSubListIterator : LinkedListIterator
    {

        /** The parent list */
        protected readonly LinkedSubList sub;

        protected internal LinkedSubListIterator(LinkedSubList sub, int startIndex)
            : base(sub.parent, startIndex + sub.offset)
        {
            this.sub = sub;
        }

        public override bool hasNext()
        {
            return (nextIndex() < sub.sizeJ);
        }

        public override bool hasPrevious()
        {
            return (previousIndex() >= 0);
        }

        public override int nextIndex()
        {
            return (base.nextIndex() - sub.offset);
        }

        public override void add(Object obj)
        {
            base.add(obj);
            sub.expectedModCount = parent.modCount;
            sub.sizeJ++;
        }

        public override void remove()
        {
            base.remove();
            sub.expectedModCount = parent.modCount;
            sub.sizeJ--;
        }
    }
    //-----------------------------------------------------------------------
    /**
     * The sublist implementation for AbstractLinkedList.
     */
    public class LinkedSubList : java.util.AbstractList<Object>
    {
        /** The main list */
        internal AbstractLinkedList parent;
        /** Offset from the main list */
        internal int offset;
        /** Sublist size */
        internal int sizeJ;
        /** Sublist modCount */
        internal int expectedModCount;

        protected internal LinkedSubList(AbstractLinkedList parent, int fromIndex, int toIndex)
        {
            if (fromIndex < 0)
            {
                throw new java.lang.IndexOutOfBoundsException("fromIndex = " + fromIndex);
            }
            if (toIndex > parent.size())
            {
                throw new java.lang.IndexOutOfBoundsException("toIndex = " + toIndex);
            }
            if (fromIndex > toIndex)
            {
                throw new java.lang.IllegalArgumentException("fromIndex(" + fromIndex + ") > toIndex(" + toIndex + ")");
            }
            this.parent = parent;
            this.offset = fromIndex;
            this.sizeJ = toIndex - fromIndex;
            this.expectedModCount = parent.modCount;
        }

        public override int size()
        {
            checkModCount();
            return sizeJ;
        }

        public override Object get(int index)
        {
            rangeCheck(index, sizeJ);
            checkModCount();
            return parent.get(index + offset);
        }

        public override void add(int index, Object obj)
        {
            rangeCheck(index, sizeJ + 1);
            checkModCount();
            parent.add(index + offset, obj);
            expectedModCount = parent.modCount;
            sizeJ++;
            modCount++;
        }

        public override Object remove(int index)
        {
            rangeCheck(index, sizeJ);
            checkModCount();
            Object result = parent.remove(index + offset);
            expectedModCount = parent.modCount;
            sizeJ--;
            modCount++;
            return result;
        }

        public override bool addAll(java.util.Collection<Object> coll)
        {
            return addAll(sizeJ, coll);
        }

        public override bool addAll(int index, java.util.Collection<Object> coll)
        {
            rangeCheck(index, sizeJ + 1);
            int cSize = coll.size();
            if (cSize == 0)
            {
                return false;
            }

            checkModCount();
            parent.addAll(offset + index, coll);
            expectedModCount = parent.modCount;
            sizeJ += cSize;
            modCount++;
            return true;
        }

        public override Object set(int index, Object obj)
        {
            rangeCheck(index, sizeJ);
            checkModCount();
            return parent.set(index + offset, obj);
        }

        public override void clear()
        {
            checkModCount();
            java.util.Iterator<Object> it = iterator();
            while (it.hasNext())
            {
                it.next();
                it.remove();
            }
        }

        public override java.util.Iterator<Object> iterator()
        {
            checkModCount();
            return parent.createSubListIterator(this);
        }

        public override java.util.ListIterator<Object> listIterator(int index)
        {
            rangeCheck(index, sizeJ + 1);
            checkModCount();
            return parent.createSubListListIterator(this, index);
        }

        public override java.util.List<Object> subList(int fromIndexInclusive, int toIndexExclusive)
        {
            return new LinkedSubList(parent, fromIndexInclusive + offset, toIndexExclusive + offset);
        }

        protected virtual void rangeCheck(int index, int beyond)
        {
            if (index < 0 || index >= beyond)
            {
                throw new java.lang.IndexOutOfBoundsException("Index '" + index + "' out of bounds for size '" + sizeJ + "'");
            }
        }

        protected virtual void checkModCount()
        {
            if (parent.modCount != expectedModCount)
            {
                throw new java.util.ConcurrentModificationException();
            }
        }
    }
    //! @see org.apache.commons.collections.listCursorableLinkedList - Subclass Public... adding for work!
    public class PublicLinkedListIterator : LinkedListIterator
    { 
        protected internal PublicLinkedListIterator(AbstractLinkedList parent, int fromIndex) : base (parent,fromIndex){}
    }
}