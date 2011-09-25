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

namespace org.apache.commons.collections
{
    /**
     * Red-Black tree-based implementation of Map. This class guarantees
     * that the map will be in both ascending key order and ascending
     * value order, sorted according to the natural order for the key's
     * and value's classes.
     * <p>
     * This Map is intended for applications that need to be able to look
     * up a key-value pairing by either key or value, and need to do so
     * with equal efficiency.
     * <p>
     * While that goal could be accomplished by taking a pair of TreeMaps
     * and redirecting requests to the appropriate TreeMap (e.g.,
     * containsKey would be directed to the TreeMap that maps values to
     * keys, containsValue would be directed to the TreeMap that maps keys
     * to values), there are problems with that implementation,
     * particularly when trying to keep the two TreeMaps synchronized with
     * each other. And if the data contained in the TreeMaps is large, the
     * cost of redundant storage becomes significant. (See also the new
     * {@link org.apache.commons.collections.bidimap.DualTreeBidiMap DualTreeBidiMap} and
     * {@link org.apache.commons.collections.bidimap.DualHashBidiMap DualHashBidiMap}
     * implementations.)
     * <p>
     * This solution keeps the data properly synchronized and minimizes
     * the data storage. The red-black algorithm is based on TreeMap's,
     * but has been modified to simultaneously map a tree node by key and
     * by value. This doubles the cost of put operations (but so does
     * using two TreeMaps), and nearly doubles the cost of remove
     * operations (there is a savings in that the lookup of the node to be
     * removed only has to be performed once). And since only one node
     * contains the key and value, storage is significantly less than that
     * required by two TreeMaps.
     * <p>
     * There are some limitations placed on data kept in this Map. The
     * biggest one is this:
     * <p>
     * When performing a put operation, neither the key nor the value may
     * already exist in the Map. In the java.util Map implementations
     * (HashMap, TreeMap), you can perform a put with an already mapped
     * key, and neither cares about duplicate values at all ... but this
     * implementation's put method with throw an IllegalArgumentException
     * if either the key or the value is already in the Map.
     * <p>
     * Obviously, that same restriction (and consequence of failing to
     * heed that restriction) applies to the putAll method.
     * <p>
     * The java.util.MapNS.Entry<Object,Object> instances returned by the appropriate methods will
     * not allow setValue() and will throw an
     * UnsupportedOperationException on attempts to call that method.
     * <p>
     * New methods are added to take advantage of the fact that values are
     * kept sorted independently of their keys:
     * <p>
     * Object getKeyForValue(Object value) is the opposite of get; it
     * takes a value and returns its key, if any.
     * <p>
     * Object removeValue(Object value) finds and removes the specified
     * value and returns the now un-used key.
     * <p>
     * Set entrySetByValue() returns the java.util.MapNS.Entry<Object,Object>'s in a Set whose
     * iterator will iterate over the java.util.MapNS.Entry<Object,Object>'s in ascending order by
     * their corresponding values.
     * <p>
     * Set keySetByValue() returns the keys in a Set whose iterator will
     * iterate over the keys in ascending order by their corresponding
     * values.
     * <p>
     * Collection valuesByValue() returns the values in a Collection whose
     * iterator will iterate over the values in ascending order.
     *
     * @deprecated Replaced by TreeBidiMap in bidimap subpackage. Due to be removed in v4.0.
     * @see BidiMap
     * @see org.apache.commons.collections.bidimap.DualTreeBidiMap
     * @see org.apache.commons.collections.bidimap.DualHashBidiMap
     * @since Commons Collections 2.0
     * @version $Revision$ $Date$
     * 
     * @author Marc Johnson
     */
    public sealed class DoubleOrderedMap : java.util.AbstractMap<Object, Object>
    {
        //  final for performance

        internal const int KEY = 0;
        internal const int VALUE = 1;
        private const int SUM_OF_INDICES = KEY + VALUE;
        private const int FIRST_INDEX = 0;
        private const int NUMBER_OF_INDICES = 2;
        private static readonly String[] dataName = new String[] { "key", "value" };

        private Node[] rootNode = new Node[] { null, null };
        private int nodeCount = 0;
        private int modifications = 0;
        private java.util.Set<Object>[] setOfKeys = new java.util.Set<Object>[] { null, null };
        private java.util.Set<Object>[] setOfEntries = new java.util.Set<Object>[] { null, null };
        private java.util.Collection<Object>[] collectionOfValues = new java.util.Collection<Object>[] { null, null };

        /**
         * Construct a new DoubleOrderedMap
         */
        public DoubleOrderedMap()
        {
        }

        /**
         * Constructs a new DoubleOrderedMap from an existing Map, with keys and
         * values sorted
         *
         * @param map the map whose mappings are to be placed in this map.
         *
         * @throws ClassCastException if the keys in the map are not
         *                               java.lang.Comparable<Object>, or are not mutually
         *                               java.lang.Comparable<Object>; also if the values in
         *                               the map are not java.lang.Comparable<Object>, or
         *                               are not mutually java.lang.Comparable<Object>
         * @throws NullPointerException if any key or value in the map
         *                                 is null
         * @throws IllegalArgumentException if there are duplicate keys
         *                                     or duplicate values in the
         *                                     map
         */
        public DoubleOrderedMap(java.util.Map<Object, Object> map)
        {//throws ClassCastException, NullPointerException,IllegalArgumentException {
            putAll(map);
        }

        /**
         * Returns the key to which this map maps the specified value.
         * Returns null if the map contains no mapping for this value.
         *
         * @param value value whose associated key is to be returned.
         *
         * @return the key to which this map maps the specified value, or
         *         null if the map contains no mapping for this value.
         *
         * @throws ClassCastException if the value is of an
         *                               inappropriate type for this map.
         * @throws NullPointerException if the value is null
         */
        public Object getKeyForValue(Object value)
        {//throws ClassCastException, NullPointerException {
            return doGet((java.lang.Comparable<Object>)value, VALUE);
        }

        /**
         * Removes the mapping for this value from this map if present
         *
         * @param value value whose mapping is to be removed from the map.
         *
         * @return previous key associated with specified value, or null
         *         if there was no mapping for value.
         */
        public Object removeValue(Object value)
        {
            return doRemove((java.lang.Comparable<Object>)value, VALUE);
        }

        /**
         * Returns a set view of the mappings contained in this map. Each
         * element in the returned set is a java.util.MapNS.Entry<Object,Object>. The set is backed
         * by the map, so changes to the map are reflected in the set, and
         * vice-versa.  If the map is modified while an iteration over the
         * set is in progress, the results of the iteration are
         * undefined. The set supports element removal, which removes the
         * corresponding mapping from the map, via the Iterator.remove,
         * Set.remove, removeAll, retainAll and clear operations.  It does
         * not support the add or addAll operations.<p>
         *
         * The difference between this method and entrySet is that
         * entrySet's iterator() method returns an iterator that iterates
         * over the mappings in ascending order by key. This method's
         * iterator method iterates over the mappings in ascending order
         * by value.
         *
         * @return a set view of the mappings contained in this map.
         */
        public java.util.Set<Object> entrySetByValue()
        {

            if (setOfEntries[VALUE] == null)
            {
                setOfEntries[VALUE] = new IAC_EntrySetByValue(this);
            }

            return setOfEntries[VALUE];
        }
        class IAC_EntrySetByValue : java.util.AbstractSet<Object>
        {
            private DoubleOrderedMap root;
            public IAC_EntrySetByValue(DoubleOrderedMap root)
            {
                this.root = root;
            }
            public override java.util.Iterator<Object> iterator()
            {

                return new IAC_Iterator(root);
            }
            class IAC_Iterator : DoubleOrderedMapIterator
            {
                public IAC_Iterator(DoubleOrderedMap root)
                    : base(VALUE, root)
                {
                    this.root = root;
                }
                protected override Object doGetNext()
                {
                    return lastReturnedNode;
                }
            }

            public override bool contains(Object o)
            {

                if (!(o is java.util.MapNS.Entry<Object, Object>))
                {
                    return false;
                }

                java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)o;
                Object key = entry.getKey();
                Node node = root.lookup((java.lang.Comparable<Object>)entry.getValue(),
                                         VALUE);

                return (node != null) && node.getData(KEY).equals(key);
            }

            public override bool remove(Object o)
            {

                if (!(o is java.util.MapNS.Entry<Object, Object>))
                {
                    return false;
                }

                java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)o;
                Object key = entry.getKey();
                Node node = root.lookup((java.lang.Comparable<Object>)entry.getValue(),
                                         VALUE);

                if ((node != null) && node.getData(KEY).equals(key))
                {
                    root.doRedBlackDelete(node);

                    return true;
                }

                return false;
            }

            public override int size()
            {
                return root.size();
            }

            public override void clear()
            {
                root.clear();
            }
        }

        /**
         * Returns a set view of the keys contained in this map.  The set
         * is backed by the map, so changes to the map are reflected in
         * the set, and vice-versa. If the map is modified while an
         * iteration over the set is in progress, the results of the
         * iteration are undefined. The set supports element removal,
         * which removes the corresponding mapping from the map, via the
         * Iterator.remove, Set.remove, removeAll, retainAll, and clear
         * operations. It does not support the add or addAll
         * operations.<p>
         *
         * The difference between this method and keySet is that keySet's
         * iterator() method returns an iterator that iterates over the
         * keys in ascending order by key. This method's iterator method
         * iterates over the keys in ascending order by value.
         *
         * @return a set view of the keys contained in this map.
         */
        public java.util.Set<Object> keySetByValue()
        {

            if (setOfKeys[VALUE] == null)
            {
                setOfKeys[VALUE] = new IAC_KeySetByValue(this);
            }

            return setOfKeys[VALUE];
        }
        class IAC_KeySetByValue : java.util.AbstractSet<Object>
        {
            private DoubleOrderedMap root;
            public IAC_KeySetByValue(DoubleOrderedMap root)
            {
                this.root = root;
            }

            public override java.util.Iterator<Object> iterator()
            {

                return new IAC_Iterator(root);
            }
            class IAC_Iterator : DoubleOrderedMapIterator
            {
                public IAC_Iterator(DoubleOrderedMap root) : base(VALUE, root) { this.root = root; }
                protected override Object doGetNext()
                {
                    return lastReturnedNode.getData(KEY);
                }
            }

            public override int size()
            {
                return root.size();
            }

            public override bool contains(Object o)
            {
                return root.containsKey(o);
            }

            public override bool remove(Object o)
            {

                int oldnodeCount = root.nodeCount;

                root.remove(o);

                return root.nodeCount != oldnodeCount;
            }

            public override void clear()
            {
                root.clear();
            }
        }

        /**
         * Returns a collection view of the values contained in this
         * map. The collection is backed by the map, so changes to the map
         * are reflected in the collection, and vice-versa. If the map is
         * modified while an iteration over the collection is in progress,
         * the results of the iteration are undefined. The collection
         * supports element removal, which removes the corresponding
         * mapping from the map, via the Iterator.remove,
         * Collection.remove, removeAll, retainAll and clear operations.
         * It does not support the add or addAll operations.<p>
         *
         * The difference between this method and values is that values's
         * iterator() method returns an iterator that iterates over the
         * values in ascending order by key. This method's iterator method
         * iterates over the values in ascending order by key.
         *
         * @return a collection view of the values contained in this map.
         */
        public java.util.Collection<Object> valuesByValue()
        {

            if (collectionOfValues[VALUE] == null)
            {
                collectionOfValues[VALUE] = new IAC_ValuesByValue(this);
            }

            return collectionOfValues[VALUE];
        }
        class IAC_ValuesByValue : java.util.AbstractCollection<Object>
        {
            private DoubleOrderedMap root; public IAC_ValuesByValue(DoubleOrderedMap root) { this.root = root; }

            public override java.util.Iterator<Object> iterator()
            {
                return new IAC_Iterator(root);
            }
            class IAC_Iterator : DoubleOrderedMapIterator
            {
                public IAC_Iterator(DoubleOrderedMap root) : base(VALUE, root) { this.root = root; }
                protected override Object doGetNext()
                {
                    return lastReturnedNode.getData(KEY);
                }
            }


            public override int size()
            {
                return root.size();
            }

            public override bool contains(Object o)
            {
                return root.containsValue(o);
            }

            public override bool remove(Object o)
            {

                int oldnodeCount = root.nodeCount;

                root.removeValue(o);

                return root.nodeCount != oldnodeCount;
            }

            public override bool removeAll(java.util.Collection<Object> c)
            {

                bool modified = false;
                java.util.Iterator<Object> iter = c.iterator();

                while (iter.hasNext())
                {
                    if (root.removeValue(iter.next()) != null)
                    {
                        modified = true;
                    }
                }

                return modified;
            }

            public override void clear()
            {
                root.clear();
            }
        }

        /**
         * common remove logic (remove by key or remove by value)
         *
         * @param o the key, or value, that we're looking for
         * @param index KEY or VALUE
         *
         * @return the key, if remove by value, or the value, if remove by
         *         key. null if the specified key or value could not be
         *         found
         */
        private Object doRemove(java.lang.Comparable<Object> o, int index)
        {

            Node node = lookup(o, index);
            Object rval = null;

            if (node != null)
            {
                rval = node.getData(oppositeIndex(index));

                doRedBlackDelete(node);
            }

            return rval;
        }

        /**
         * common get logic, used to get by key or get by value
         *
         * @param o the key or value that we're looking for
         * @param index KEY or VALUE
         *
         * @return the key (if the value was mapped) or the value (if the
         *         key was mapped); null if we couldn't find the specified
         *         object
         */
        private Object doGet(java.lang.Comparable<Object> o, int index)
        {

            checkNonNullComparable(o, index);

            Node node = lookup(o, index);

            return ((node == null)
                    ? null
                    : node.getData(oppositeIndex(index)));
        }

        /**
         * Get the opposite index of the specified index
         *
         * @param index KEY or VALUE
         *
         * @return VALUE (if KEY was specified), else KEY
         */
        private int oppositeIndex(int index)
        {

            // old trick ... to find the opposite of a value, m or n,
            // subtract the value from the sum of the two possible
            // values. (m + n) - m = n; (m + n) - n = m
            return SUM_OF_INDICES - index;
        }

        /**
         * do the actual lookup of a piece of data
         *
         * @param data the key or value to be looked up
         * @param index KEY or VALUE
         *
         * @return the desired Node, or null if there is no mapping of the
         *         specified data
         */
        private Node lookup(java.lang.Comparable<Object> data, int index)
        {

            Node rval = null;
            Node node = rootNode[index];

            while (node != null)
            {
                int cmp = compare(data, node.getData(index));

                if (cmp == 0)
                {
                    rval = node;

                    break;
                }
                else
                {
                    node = (cmp < 0)
                           ? node.getLeft(index)
                           : node.getRight(index);
                }
            }

            return rval;
        }

        /**
         * Compare two objects
         *
         * @param o1 the first object
         * @param o2 the second object
         *
         * @return negative value if o1 < o2; 0 if o1 == o2; positive
         *         value if o1 > o2
         */
        private static int compare(java.lang.Comparable<Object> o1, java.lang.Comparable<Object> o2)
        {
            return o1.compareTo(o2);
        }

        /**
         * find the least node from a given node. very useful for starting
         * a sorting iterator ...
         *
         * @param node the node from which we will start searching
         * @param index KEY or VALUE
         *
         * @return the smallest node, from the specified node, in the
         *         specified mapping
         */
        private static Node leastNode(Node node, int index)
        {

            Node rval = node;

            if (rval != null)
            {
                while (rval.getLeft(index) != null)
                {
                    rval = rval.getLeft(index);
                }
            }

            return rval;
        }

        /**
         * get the next larger node from the specified node
         *
         * @param node the node to be searched from
         * @param index KEY or VALUE
         *
         * @return the specified node
         */
        private Node nextGreater(Node node, int index)
        {

            Node rval = null;

            if (node == null)
            {
                rval = null;
            }
            else if (node.getRight(index) != null)
            {

                // everything to the node's right is larger. The least of
                // the right node's descendants is the next larger node
                rval = leastNode(node.getRight(index), index);
            }
            else
            {

                // traverse up our ancestry until we find an ancestor that
                // is null or one whose left child is our ancestor. If we
                // find a null, then this node IS the largest node in the
                // tree, and there is no greater node. Otherwise, we are
                // the largest node in the subtree on that ancestor's left
                // ... and that ancestor is the next greatest node
                Node parent = node.getParent(index);
                Node child = node;

                while ((parent != null) && (child == parent.getRight(index)))
                {
                    child = parent;
                    parent = parent.getParent(index);
                }

                rval = parent;
            }

            return rval;
        }

        /**
         * copy the color from one node to another, dealing with the fact
         * that one or both nodes may, in fact, be null
         *
         * @param from the node whose color we're copying; may be null
         * @param to the node whose color we're changing; may be null
         * @param index KEY or VALUE
         */
        private static void copyColor(Node from, Node to,
                                      int index)
        {

            if (to != null)
            {
                if (from == null)
                {

                    // by default, make it black
                    to.setBlack(index);
                }
                else
                {
                    to.copyColor(from, index);
                }
            }
        }

        /**
         * is the specified node red? if the node does not exist, no, it's
         * black, thank you
         *
         * @param node the node (may be null) in question
         * @param index KEY or VALUE
         */
        private static bool isRed(Node node, int index)
        {

            return ((node == null)
                    ? false
                    : node.isRed(index));
        }

        /**
         * is the specified black red? if the node does not exist, sure,
         * it's black, thank you
         *
         * @param node the node (may be null) in question
         * @param index KEY or VALUE
         */
        private static bool isBlack(Node node, int index)
        {

            return ((node == null)
                    ? true
                    : node.isBlack(index));
        }

        /**
         * force a node (if it exists) red
         *
         * @param node the node (may be null) in question
         * @param index KEY or VALUE
         */
        private static void makeRed(Node node, int index)
        {

            if (node != null)
            {
                node.setRed(index);
            }
        }

        /**
         * force a node (if it exists) black
         *
         * @param node the node (may be null) in question
         * @param index KEY or VALUE
         */
        private static void makeBlack(Node node, int index)
        {

            if (node != null)
            {
                node.setBlack(index);
            }
        }

        /**
         * get a node's grandparent. mind you, the node, its parent, or
         * its grandparent may not exist. no problem
         *
         * @param node the node (may be null) in question
         * @param index KEY or VALUE
         */
        private static Node getGrandParent(Node node, int index)
        {
            return getParent(getParent(node, index), index);
        }

        /**
         * get a node's parent. mind you, the node, or its parent, may not
         * exist. no problem
         *
         * @param node the node (may be null) in question
         * @param index KEY or VALUE
         */
        private static Node getParent(Node node, int index)
        {

            return ((node == null)
                    ? null
                    : node.getParent(index));
        }

        /**
         * get a node's right child. mind you, the node may not exist. no
         * problem
         *
         * @param node the node (may be null) in question
         * @param index KEY or VALUE
         */
        private static Node getRightChild(Node node, int index)
        {

            return (node == null)
                   ? null
                   : node.getRight(index);
        }

        /**
         * get a node's left child. mind you, the node may not exist. no
         * problem
         *
         * @param node the node (may be null) in question
         * @param index KEY or VALUE
         */
        private static Node getLeftChild(Node node, int index)
        {

            return (node == null)
                   ? null
                   : node.getLeft(index);
        }

        /**
         * is this node its parent's left child? mind you, the node, or
         * its parent, may not exist. no problem. if the node doesn't
         * exist ... it's its non-existent parent's left child. If the
         * node does exist but has no parent ... no, we're not the
         * non-existent parent's left child. Otherwise (both the specified
         * node AND its parent exist), check.
         *
         * @param node the node (may be null) in question
         * @param index KEY or VALUE
         */
        private static bool isLeftChild(Node node, int index)
        {

            return (node == null)
                   ? true
                   : ((node.getParent(index) == null)
                      ? false
                      : (node == node.getParent(index).getLeft(index)));
        }

        /**
         * is this node its parent's right child? mind you, the node, or
         * its parent, may not exist. no problem. if the node doesn't
         * exist ... it's its non-existent parent's right child. If the
         * node does exist but has no parent ... no, we're not the
         * non-existent parent's right child. Otherwise (both the
         * specified node AND its parent exist), check.
         *
         * @param node the node (may be null) in question
         * @param index KEY or VALUE
         */
        private static bool isRightChild(Node node, int index)
        {

            return (node == null)
                   ? true
                   : ((node.getParent(index) == null)
                      ? false
                      : (node == node.getParent(index).getRight(index)));
        }

        /**
         * do a rotate left. standard fare in the world of balanced trees
         *
         * @param node the node to be rotated
         * @param index KEY or VALUE
         */
        private void rotateLeft(Node node, int index)
        {

            Node rightChild = node.getRight(index);

            node.setRight(rightChild.getLeft(index), index);

            if (rightChild.getLeft(index) != null)
            {
                rightChild.getLeft(index).setParent(node, index);
            }

            rightChild.setParent(node.getParent(index), index);

            if (node.getParent(index) == null)
            {

                // node was the root ... now its right child is the root
                rootNode[index] = rightChild;
            }
            else if (node.getParent(index).getLeft(index) == node)
            {
                node.getParent(index).setLeft(rightChild, index);
            }
            else
            {
                node.getParent(index).setRight(rightChild, index);
            }

            rightChild.setLeft(node, index);
            node.setParent(rightChild, index);
        }

        /**
         * do a rotate right. standard fare in the world of balanced trees
         *
         * @param node the node to be rotated
         * @param index KEY or VALUE
         */
        private void rotateRight(Node node, int index)
        {

            Node leftChild = node.getLeft(index);

            node.setLeft(leftChild.getRight(index), index);

            if (leftChild.getRight(index) != null)
            {
                leftChild.getRight(index).setParent(node, index);
            }

            leftChild.setParent(node.getParent(index), index);

            if (node.getParent(index) == null)
            {

                // node was the root ... now its left child is the root
                rootNode[index] = leftChild;
            }
            else if (node.getParent(index).getRight(index) == node)
            {
                node.getParent(index).setRight(leftChild, index);
            }
            else
            {
                node.getParent(index).setLeft(leftChild, index);
            }

            leftChild.setRight(node, index);
            node.setParent(leftChild, index);
        }

        /**
         * complicated red-black insert stuff. Based on Sun's TreeMap
         * implementation, though it's barely recognizable any more
         *
         * @param insertedNode the node to be inserted
         * @param index KEY or VALUE
         */
        private void doRedBlackInsert(Node insertedNode, int index)
        {

            Node currentNode = insertedNode;

            makeRed(currentNode, index);

            while ((currentNode != null) && (currentNode != rootNode[index])
                    && (isRed(currentNode.getParent(index), index)))
            {
                if (isLeftChild(getParent(currentNode, index), index))
                {
                    Node y = getRightChild(getGrandParent(currentNode, index),
                                           index);

                    if (isRed(y, index))
                    {
                        makeBlack(getParent(currentNode, index), index);
                        makeBlack(y, index);
                        makeRed(getGrandParent(currentNode, index), index);

                        currentNode = getGrandParent(currentNode, index);
                    }
                    else
                    {
                        if (isRightChild(currentNode, index))
                        {
                            currentNode = getParent(currentNode, index);

                            rotateLeft(currentNode, index);
                        }

                        makeBlack(getParent(currentNode, index), index);
                        makeRed(getGrandParent(currentNode, index), index);

                        if (getGrandParent(currentNode, index) != null)
                        {
                            rotateRight(getGrandParent(currentNode, index),
                                        index);
                        }
                    }
                }
                else
                {

                    // just like clause above, except swap left for right
                    Node y = getLeftChild(getGrandParent(currentNode, index),
                                          index);

                    if (isRed(y, index))
                    {
                        makeBlack(getParent(currentNode, index), index);
                        makeBlack(y, index);
                        makeRed(getGrandParent(currentNode, index), index);

                        currentNode = getGrandParent(currentNode, index);
                    }
                    else
                    {
                        if (isLeftChild(currentNode, index))
                        {
                            currentNode = getParent(currentNode, index);

                            rotateRight(currentNode, index);
                        }

                        makeBlack(getParent(currentNode, index), index);
                        makeRed(getGrandParent(currentNode, index), index);

                        if (getGrandParent(currentNode, index) != null)
                        {
                            rotateLeft(getGrandParent(currentNode, index), index);
                        }
                    }
                }
            }

            makeBlack(rootNode[index], index);
        }

        /**
         * complicated red-black delete stuff. Based on Sun's TreeMap
         * implementation, though it's barely recognizable any more
         *
         * @param deletedNode the node to be deleted
         */
        private void doRedBlackDelete(Node deletedNode)
        {

            for (int index = FIRST_INDEX; index < NUMBER_OF_INDICES; index++)
            {

                // if deleted node has both left and children, swap with
                // the next greater node
                if ((deletedNode.getLeft(index) != null)
                        && (deletedNode.getRight(index) != null))
                {
                    swapPosition(nextGreater(deletedNode, index), deletedNode,
                                 index);
                }

                Node replacement = ((deletedNode.getLeft(index) != null)
                                    ? deletedNode.getLeft(index)
                                    : deletedNode.getRight(index));

                if (replacement != null)
                {
                    replacement.setParent(deletedNode.getParent(index), index);

                    if (deletedNode.getParent(index) == null)
                    {
                        rootNode[index] = replacement;
                    }
                    else if (deletedNode
                             == deletedNode.getParent(index).getLeft(index))
                    {
                        deletedNode.getParent(index).setLeft(replacement, index);
                    }
                    else
                    {
                        deletedNode.getParent(index).setRight(replacement, index);
                    }

                    deletedNode.setLeft(null, index);
                    deletedNode.setRight(null, index);
                    deletedNode.setParent(null, index);

                    if (isBlack(deletedNode, index))
                    {
                        doRedBlackDeleteFixup(replacement, index);
                    }
                }
                else
                {

                    // replacement is null
                    if (deletedNode.getParent(index) == null)
                    {

                        // empty tree
                        rootNode[index] = null;
                    }
                    else
                    {

                        // deleted node had no children
                        if (isBlack(deletedNode, index))
                        {
                            doRedBlackDeleteFixup(deletedNode, index);
                        }

                        if (deletedNode.getParent(index) != null)
                        {
                            if (deletedNode
                                    == deletedNode.getParent(index)
                                        .getLeft(index))
                            {
                                deletedNode.getParent(index).setLeft(null, index);
                            }
                            else
                            {
                                deletedNode.getParent(index).setRight(null,
                                                      index);
                            }

                            deletedNode.setParent(null, index);
                        }
                    }
                }
            }

            shrink();
        }

        /**
         * complicated red-black delete stuff. Based on Sun's TreeMap
         * implementation, though it's barely recognizable any more. This
         * rebalances the tree (somewhat, as red-black trees are not
         * perfectly balanced -- perfect balancing takes longer)
         *
         * @param replacementNode the node being replaced
         * @param index KEY or VALUE
         */
        private void doRedBlackDeleteFixup(Node replacementNode,
                                           int index)
        {

            Node currentNode = replacementNode;

            while ((currentNode != rootNode[index])
                    && (isBlack(currentNode, index)))
            {
                if (isLeftChild(currentNode, index))
                {
                    Node siblingNode =
                        getRightChild(getParent(currentNode, index), index);

                    if (isRed(siblingNode, index))
                    {
                        makeBlack(siblingNode, index);
                        makeRed(getParent(currentNode, index), index);
                        rotateLeft(getParent(currentNode, index), index);

                        siblingNode = getRightChild(getParent(currentNode, index), index);
                    }

                    if (isBlack(getLeftChild(siblingNode, index), index)
                            && isBlack(getRightChild(siblingNode, index),
                                       index))
                    {
                        makeRed(siblingNode, index);

                        currentNode = getParent(currentNode, index);
                    }
                    else
                    {
                        if (isBlack(getRightChild(siblingNode, index), index))
                        {
                            makeBlack(getLeftChild(siblingNode, index), index);
                            makeRed(siblingNode, index);
                            rotateRight(siblingNode, index);

                            siblingNode =
                                getRightChild(getParent(currentNode, index), index);
                        }

                        copyColor(getParent(currentNode, index), siblingNode,
                                  index);
                        makeBlack(getParent(currentNode, index), index);
                        makeBlack(getRightChild(siblingNode, index), index);
                        rotateLeft(getParent(currentNode, index), index);

                        currentNode = rootNode[index];
                    }
                }
                else
                {
                    Node siblingNode = getLeftChild(getParent(currentNode, index), index);

                    if (isRed(siblingNode, index))
                    {
                        makeBlack(siblingNode, index);
                        makeRed(getParent(currentNode, index), index);
                        rotateRight(getParent(currentNode, index), index);

                        siblingNode = getLeftChild(getParent(currentNode, index), index);
                    }

                    if (isBlack(getRightChild(siblingNode, index), index)
                            && isBlack(getLeftChild(siblingNode, index), index))
                    {
                        makeRed(siblingNode, index);

                        currentNode = getParent(currentNode, index);
                    }
                    else
                    {
                        if (isBlack(getLeftChild(siblingNode, index), index))
                        {
                            makeBlack(getRightChild(siblingNode, index), index);
                            makeRed(siblingNode, index);
                            rotateLeft(siblingNode, index);

                            siblingNode =
                                getLeftChild(getParent(currentNode, index), index);
                        }

                        copyColor(getParent(currentNode, index), siblingNode,
                                  index);
                        makeBlack(getParent(currentNode, index), index);
                        makeBlack(getLeftChild(siblingNode, index), index);
                        rotateRight(getParent(currentNode, index), index);

                        currentNode = rootNode[index];
                    }
                }
            }

            makeBlack(currentNode, index);
        }

        /**
         * swap two nodes (except for their content), taking care of
         * special cases where one is the other's parent ... hey, it
         * happens.
         *
         * @param x one node
         * @param y another node
         * @param index KEY or VALUE
         */
        private void swapPosition(Node x, Node y, int index)
        {

            // Save initial values.
            Node xFormerParent = x.getParent(index);
            Node xFormerLeftChild = x.getLeft(index);
            Node xFormerRightChild = x.getRight(index);
            Node yFormerParent = y.getParent(index);
            Node yFormerLeftChild = y.getLeft(index);
            Node yFormerRightChild = y.getRight(index);
            bool xWasLeftChild =
                (x.getParent(index) != null)
                && (x == x.getParent(index).getLeft(index));
            bool yWasLeftChild =
                (y.getParent(index) != null)
                && (y == y.getParent(index).getLeft(index));

            // Swap, handling special cases of one being the other's parent.
            if (x == yFormerParent)
            {    // x was y's parent
                x.setParent(y, index);

                if (yWasLeftChild)
                {
                    y.setLeft(x, index);
                    y.setRight(xFormerRightChild, index);
                }
                else
                {
                    y.setRight(x, index);
                    y.setLeft(xFormerLeftChild, index);
                }
            }
            else
            {
                x.setParent(yFormerParent, index);

                if (yFormerParent != null)
                {
                    if (yWasLeftChild)
                    {
                        yFormerParent.setLeft(x, index);
                    }
                    else
                    {
                        yFormerParent.setRight(x, index);
                    }
                }

                y.setLeft(xFormerLeftChild, index);
                y.setRight(xFormerRightChild, index);
            }

            if (y == xFormerParent)
            {    // y was x's parent
                y.setParent(x, index);

                if (xWasLeftChild)
                {
                    x.setLeft(y, index);
                    x.setRight(yFormerRightChild, index);
                }
                else
                {
                    x.setRight(y, index);
                    x.setLeft(yFormerLeftChild, index);
                }
            }
            else
            {
                y.setParent(xFormerParent, index);

                if (xFormerParent != null)
                {
                    if (xWasLeftChild)
                    {
                        xFormerParent.setLeft(y, index);
                    }
                    else
                    {
                        xFormerParent.setRight(y, index);
                    }
                }

                x.setLeft(yFormerLeftChild, index);
                x.setRight(yFormerRightChild, index);
            }

            // Fix children's parent pointers
            if (x.getLeft(index) != null)
            {
                x.getLeft(index).setParent(x, index);
            }

            if (x.getRight(index) != null)
            {
                x.getRight(index).setParent(x, index);
            }

            if (y.getLeft(index) != null)
            {
                y.getLeft(index).setParent(y, index);
            }

            if (y.getRight(index) != null)
            {
                y.getRight(index).setParent(y, index);
            }

            x.swapColors(y, index);

            // Check if root changed
            if (rootNode[index] == x)
            {
                rootNode[index] = y;
            }
            else if (rootNode[index] == y)
            {
                rootNode[index] = x;
            }
        }

        /**
         * check if an object is fit to be proper input ... has to be
         * java.lang.Comparable<Object> and non-null
         *
         * @param o the object being checked
         * @param index KEY or VALUE (used to put the right word in the
         *              exception message)
         *
         * @throws NullPointerException if o is null
         * @throws ClassCastException if o is not java.lang.Comparable<Object>
         */
        private static void checkNonNullComparable(Object o,
                                                   int index)
        {

            if (o == null)
            {
                throw new java.lang.NullPointerException(dataName[index]
                                               + " cannot be null");
            }

            if (!(o is java.lang.Comparable<Object>))
            {
                throw new java.lang.ClassCastException(dataName[index]
                                             + " must be java.lang.Comparable<Object>");
            }
        }

        /**
         * check a key for validity (non-null and implements java.lang.Comparable<Object>)
         *
         * @param key the key to be checked
         *
         * @throws NullPointerException if key is null
         * @throws ClassCastException if key is not java.lang.Comparable<Object>
         */
        private static void checkKey(Object key)
        {
            checkNonNullComparable(key, KEY);
        }

        /**
         * check a value for validity (non-null and implements java.lang.Comparable<Object>)
         *
         * @param value the value to be checked
         *
         * @throws NullPointerException if value is null
         * @throws ClassCastException if value is not java.lang.Comparable<Object>
         */
        private static void checkValue(Object value)
        {
            checkNonNullComparable(value, VALUE);
        }

        /**
         * check a key and a value for validity (non-null and implements
         * java.lang.Comparable<Object>)
         *
         * @param key the key to be checked
         * @param value the value to be checked
         *
         * @throws NullPointerException if key or value is null
         * @throws ClassCastException if key or value is not java.lang.Comparable<Object>
         */
        private static void checkKeyAndValue(Object key,
                                             Object value)
        {
            checkKey(key);
            checkValue(value);
        }

        /**
         * increment the modification count -- used to check for
         * concurrent modification of the map through the map and through
         * an Iterator from one of its Set or Collection views
         */
        private void modify()
        {
            modifications++;
        }

        /**
         * bump up the size and note that the map has changed
         */
        private void grow()
        {

            modify();

            nodeCount++;
        }

        /**
         * decrement the size and note that the map has changed
         */
        private void shrink()
        {

            modify();

            nodeCount--;
        }

        /**
         * insert a node by its value
         *
         * @param newNode the node to be inserted
         *
         * @throws IllegalArgumentException if the node already exists
         *                                     in the value mapping
         */
        private void insertValue(Node newNode)
        {//throws IllegalArgumentException {

            Node node = rootNode[VALUE];

            while (true)
            {
                int cmp = compare(newNode.getData(VALUE), node.getData(VALUE));

                if (cmp == 0)
                {
                    throw new java.lang.IllegalArgumentException(
                        "Cannot store a duplicate value (\""
                        + newNode.getData(VALUE) + "\") in this Map");
                }
                else if (cmp < 0)
                {
                    if (node.getLeft(VALUE) != null)
                    {
                        node = node.getLeft(VALUE);
                    }
                    else
                    {
                        node.setLeft(newNode, VALUE);
                        newNode.setParent(node, VALUE);
                        doRedBlackInsert(newNode, VALUE);

                        break;
                    }
                }
                else
                {    // cmp > 0
                    if (node.getRight(VALUE) != null)
                    {
                        node = node.getRight(VALUE);
                    }
                    else
                    {
                        node.setRight(newNode, VALUE);
                        newNode.setParent(node, VALUE);
                        doRedBlackInsert(newNode, VALUE);

                        break;
                    }
                }
            }
        }

        /* ********** START implementation of Map ********** */

        /**
         * Returns the number of key-value mappings in this map. If the
         * map contains more than Integer.MAXVALUE elements, returns
         * Integer.MAXVALUE.
         *
         * @return the number of key-value mappings in this map.
         */
        public override int size()
        {
            return nodeCount;
        }

        /**
         * Returns true if this map contains a mapping for the specified
         * key.
         *
         * @param key key whose presence in this map is to be tested.
         *
         * @return true if this map contains a mapping for the specified
         *         key.
         *
         * @throws ClassCastException if the key is of an inappropriate
         *                               type for this map.
         * @throws NullPointerException if the key is null
         */
        public override bool containsKey(Object key)
        {//throws ClassCastException, NullPointerException {

            checkKey(key);

            return lookup((java.lang.Comparable<Object>)key, KEY) != null;
        }

        /**
         * Returns true if this map maps one or more keys to the
         * specified value.
         *
         * @param value value whose presence in this map is to be tested.
         *
         * @return true if this map maps one or more keys to the specified
         *         value.
         */
        public override bool containsValue(Object value)
        {

            checkValue(value);

            return lookup((java.lang.Comparable<Object>)value, VALUE) != null;
        }

        /**
         * Returns the value to which this map maps the specified
         * key. Returns null if the map contains no mapping for this key.
         *
         * @param key key whose associated value is to be returned.
         *
         * @return the value to which this map maps the specified key, or
         *         null if the map contains no mapping for this key.
         *
         * @throws ClassCastException if the key is of an inappropriate
         *                               type for this map.
         * @throws NullPointerException if the key is null
         */
        public override Object get(Object key)
        {//throws ClassCastException, NullPointerException {
            return doGet((java.lang.Comparable<Object>)key, KEY);
        }

        /**
         * Associates the specified value with the specified key in this
         * map.
         *
         * @param key key with which the specified value is to be
         *            associated.
         * @param value value to be associated with the specified key.
         *
         * @return null
         *
         * @throws ClassCastException if the class of the specified key
         *                               or value prevents it from being
         *                               stored in this map.
         * @throws NullPointerException if the specified key or value
         *                                 is null
         * @throws IllegalArgumentException if the key duplicates an
         *                                     existing key, or if the
         *                                     value duplicates an
         *                                     existing value
         */
        public override Object put(Object key, Object value)
        {//throws ClassCastException, NullPointerException,IllegalArgumentException {

            checkKeyAndValue(key, value);

            Node node = rootNode[KEY];

            if (node == null)
            {
                Node root = new Node((java.lang.Comparable<Object>)key, (java.lang.Comparable<Object>)value);

                rootNode[KEY] = root;
                rootNode[VALUE] = root;

                grow();
            }
            else
            {
                while (true)
                {
                    int cmp = compare((java.lang.Comparable<Object>)key, node.getData(KEY));

                    if (cmp == 0)
                    {
                        throw new java.lang.IllegalArgumentException(
                            "Cannot store a duplicate key (\"" + key
                            + "\") in this Map");
                    }
                    else if (cmp < 0)
                    {
                        if (node.getLeft(KEY) != null)
                        {
                            node = node.getLeft(KEY);
                        }
                        else
                        {
                            Node newNode = new Node((java.lang.Comparable<Object>)key,
                                                    (java.lang.Comparable<Object>)value);

                            insertValue(newNode);
                            node.setLeft(newNode, KEY);
                            newNode.setParent(node, KEY);
                            doRedBlackInsert(newNode, KEY);
                            grow();

                            break;
                        }
                    }
                    else
                    {    // cmp > 0
                        if (node.getRight(KEY) != null)
                        {
                            node = node.getRight(KEY);
                        }
                        else
                        {
                            Node newNode = new Node((java.lang.Comparable<Object>)key,
                                                    (java.lang.Comparable<Object>)value);

                            insertValue(newNode);
                            node.setRight(newNode, KEY);
                            newNode.setParent(node, KEY);
                            doRedBlackInsert(newNode, KEY);
                            grow();

                            break;
                        }
                    }
                }
            }

            return null;
        }

        /**
         * Removes the mapping for this key from this map if present
         *
         * @param key key whose mapping is to be removed from the map.
         *
         * @return previous value associated with specified key, or null
         *         if there was no mapping for key.
         */
        public override Object remove(Object key)
        {
            return doRemove((java.lang.Comparable<Object>)key, KEY);
        }

        /**
         * Removes all mappings from this map
         */
        public override void clear()
        {

            modify();

            nodeCount = 0;
            rootNode[KEY] = null;
            rootNode[VALUE] = null;
        }

        /**
         * Returns a set view of the keys contained in this map.  The set
         * is backed by the map, so changes to the map are reflected in
         * the set, and vice-versa. If the map is modified while an
         * iteration over the set is in progress, the results of the
         * iteration are undefined. The set supports element removal,
         * which removes the corresponding mapping from the map, via the
         * Iterator.remove, Set.remove, removeAll, retainAll, and clear
         * operations.  It does not support the add or addAll operations.
         *
         * @return a set view of the keys contained in this map.
         */
        public override java.util.Set<Object> keySet()
        {

            if (setOfKeys[KEY] == null)
            {
                setOfKeys[KEY] = new IAC_KeySet(this);
            }

            return setOfKeys[KEY];
        }
        class IAC_KeySet : java.util.AbstractSet<Object>
        {
            private DoubleOrderedMap root; public IAC_KeySet(DoubleOrderedMap root) { this.root = root; }

            public override java.util.Iterator<Object> iterator()
            {

                return new IAC_Iterator(root);
            }
            class IAC_Iterator : DoubleOrderedMapIterator
            {
                public IAC_Iterator(DoubleOrderedMap root) : base(KEY, root) { this.root = root; }
                protected override Object doGetNext()
                {
                    return lastReturnedNode.getData(KEY);
                }
            }

            public override int size()
            {
                return root.size();
            }

            public override bool contains(Object o)
            {
                return root.containsKey(o);
            }

            public override bool remove(Object o)
            {

                int oldNodeCount = root.nodeCount;

                root.remove(o);

                return root.nodeCount != oldNodeCount;
            }

            public override void clear()
            {
                root.clear();
            }
        }

        /**
         * Returns a collection view of the values contained in this
         * map. The collection is backed by the map, so changes to the map
         * are reflected in the collection, and vice-versa. If the map is
         * modified while an iteration over the collection is in progress,
         * the results of the iteration are undefined. The collection
         * supports element removal, which removes the corresponding
         * mapping from the map, via the Iterator.remove,
         * Collection.remove, removeAll, retainAll and clear operations.
         * It does not support the add or addAll operations.
         *
         * @return a collection view of the values contained in this map.
         */
        public override java.util.Collection<Object> values()
        {

            if (collectionOfValues[KEY] == null)
            {
                collectionOfValues[KEY] = new IAC_Values(this);
            }

            return collectionOfValues[KEY];
        }
        class IAC_Values : java.util.AbstractCollection<Object>
        {
            private DoubleOrderedMap root;
            public IAC_Values(DoubleOrderedMap root) { this.root = root; }

            public override java.util.Iterator<Object> iterator()
            {

                return new IAC_Iterator(root);
            }
            class IAC_Iterator : DoubleOrderedMapIterator
            {
                public IAC_Iterator(DoubleOrderedMap root) : base(KEY, root) { this.root = root; }
                protected override Object doGetNext()
                {
                    return lastReturnedNode.getData(VALUE);
                }
            }

            public override int size()
            {
                return root.size();
            }

            public override bool contains(Object o)
            {
                return root.containsValue(o);
            }

            public override bool remove(Object o)
            {

                int oldNodeCount = root.nodeCount;

                root.removeValue(o);

                return root.nodeCount != oldNodeCount;
            }

            public override bool removeAll(java.util.Collection<Object> c)
            {

                bool modified = false;
                java.util.Iterator<Object> iter = c.iterator();

                while (iter.hasNext())
                {
                    if (root.removeValue(iter.next()) != null)
                    {
                        modified = true;
                    }
                }

                return modified;
            }

            public override void clear()
            {
                root.clear();
            }
        }

        /**
         * Returns a set view of the mappings contained in this map. Each
         * element in the returned set is a java.util.MapNS.Entry<Object,Object>. The set is backed
         * by the map, so changes to the map are reflected in the set, and
         * vice-versa.  If the map is modified while an iteration over the
         * set is in progress, the results of the iteration are
         * undefined.
         * <p>
         * The set supports element removal, which removes the corresponding
         * mapping from the map, via the Iterator.remove, Set.remove, removeAll,
         * retainAll and clear operations.
         * It does not support the add or addAll operations.
         * The setValue method is not supported on the Map Entry.
         *
         * @return a set view of the mappings contained in this map.
         */
        public new java.util.Set<Object> entrySet()
        {

            if (setOfEntries[KEY] == null)
            {
                setOfEntries[KEY] = new IAC_EntrySet(this);
            }

            return setOfEntries[KEY];
        }
        class IAC_EntrySet : java.util.AbstractSet<Object>
        {
            private DoubleOrderedMap root; public IAC_EntrySet(DoubleOrderedMap root) { this.root = root; }

            public override java.util.Iterator<Object> iterator()
            {

                return new IAC_Iterator(root);
            }
            class IAC_Iterator : DoubleOrderedMapIterator
            {
                public IAC_Iterator(DoubleOrderedMap root) : base(KEY, root) { this.root = root; }
                protected override Object doGetNext()
                {
                    return lastReturnedNode;
                }
            }

            public override bool contains(Object o)
            {

                if (!(o is java.util.MapNS.Entry<Object, Object>))
                {
                    return false;
                }

                java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)o;
                Object value = entry.getValue();
                Node node = root.lookup((java.lang.Comparable<Object>)entry.getKey(),
                                         KEY);

                return (node != null)
                       && node.getData(VALUE).equals(value);
            }

            public override bool remove(Object o)
            {

                if (!(o is java.util.MapNS.Entry<Object, Object>))
                {
                    return false;
                }

                java.util.MapNS.Entry<Object, Object> entry = (java.util.MapNS.Entry<Object, Object>)o;
                Object value = entry.getValue();
                Node node = root.lookup((java.lang.Comparable<Object>)entry.getKey(),
                                         KEY);

                if ((node != null) && node.getData(VALUE).equals(value))
                {
                    root.doRedBlackDelete(node);

                    return true;
                }

                return false;
            }

            public override int size()
            {
                return root.size();
            }

            public override void clear()
            {
                root.clear();
            }
        }

        /* **********  END  implementation of Map ********** */
        private abstract class DoubleOrderedMapIterator : java.util.Iterator<Object>
        {
            protected DoubleOrderedMap root;
            private int expectedModifications;
            protected Node lastReturnedNode;
            private Node nextNode;
            private int iteratorType;

            /**
             * Constructor
             *
             * @param type
             */
            internal DoubleOrderedMapIterator(int type, DoubleOrderedMap root)
            {
                this.root = root;
                iteratorType = type;
                expectedModifications = root.modifications;
                lastReturnedNode = null;
                nextNode = leastNode(root.rootNode[iteratorType],
                                                  iteratorType);
            }

            /**
             * @return 'next', whatever that means for a given kind of
             *         DoubleOrderedMapIterator
             */
            protected abstract Object doGetNext();

            /* ********** START implementation of Iterator ********** */

            /**
             * @return true if the iterator has more elements.
             */
            public bool hasNext()
            {
                return nextNode != null;
            }

            /**
             * @return the next element in the iteration.
             *
             * @throws NoSuchElementException if iteration has no more
             *                                   elements.
             * @throws ConcurrentModificationException if the
             *                                            DoubleOrderedMap is
             *                                            modified behind
             *                                            the iterator's
             *                                            back
             */
            public Object next()
            {//throws NoSuchElementException,ConcurrentModificationException {

                if (nextNode == null)
                {
                    throw new java.util.NoSuchElementException();
                }

                if (root.modifications != expectedModifications)
                {
                    throw new java.util.ConcurrentModificationException();
                }

                lastReturnedNode = nextNode;
                nextNode = root.nextGreater(nextNode, iteratorType);

                return doGetNext();
            }

            /**
             * Removes from the underlying collection the last element
             * returned by the iterator. This method can be called only
             * once per call to next. The behavior of an iterator is
             * unspecified if the underlying collection is modified while
             * the iteration is in progress in any way other than by
             * calling this method.
             *
             * @throws IllegalStateException if the next method has not
             *                                  yet been called, or the
             *                                  remove method has already
             *                                  been called after the last
             *                                  call to the next method.
             * @throws ConcurrentModificationException if the
             *                                            DoubleOrderedMap is
             *                                            modified behind
             *                                            the iterator's
             *                                            back
             */
            public void remove()
            {//throws IllegalStateException,ConcurrentModificationException {

                if (lastReturnedNode == null)
                {
                    throw new java.lang.IllegalStateException();
                }

                if (root.modifications != expectedModifications)
                {
                    throw new java.util.ConcurrentModificationException();
                }

                root.doRedBlackDelete(lastReturnedNode);

                expectedModifications++;

                lastReturnedNode = null;
            }

            /* **********  END  implementation of Iterator ********** */
        }    // end private abstract class DoubleOrderedMapIterator

        // final for performance
        private sealed class Node : java.util.MapNS.Entry<Object, Object>, KeyValue
        {

            private java.lang.Comparable<Object>[] data;
            private Node[] leftNode;
            private Node[] rightNode;
            private Node[] parentNode;
            private bool[] blackColor;
            private int hashcodeValue;
            private bool calculatedHashCode;

            /**
             * Make a new cell with given key and value, and with null
             * links, and black (true) colors.
             *
             * @param key
             * @param value
             */
            internal Node(java.lang.Comparable<Object> key, java.lang.Comparable<Object> value)
            {

                data = new java.lang.Comparable<Object>[] { key, value };
                leftNode = new Node[] { null, null };
                rightNode = new Node[] { null, null };
                parentNode = new Node[] { null, null };
                blackColor = new bool[] { true, true };
                calculatedHashCode = false;
            }

            /**
             * get the specified data
             *
             * @param index KEY or VALUE
             *
             * @return the key or value
             */
            internal java.lang.Comparable<Object> getData(int index)
            {
                return data[index];
            }

            /**
             * Set this node's left node
             *
             * @param node the new left node
             * @param index KEY or VALUE
             */
            internal void setLeft(Node node, int index)
            {
                leftNode[index] = node;
            }

            /**
             * get the left node
             *
             * @param index KEY or VALUE
             *
             * @return the left node -- may be null
             */
            internal Node getLeft(int index)
            {
                return leftNode[index];
            }

            /**
             * Set this node's right node
             *
             * @param node the new right node
             * @param index KEY or VALUE
             */
            internal void setRight(Node node, int index)
            {
                rightNode[index] = node;
            }

            /**
             * get the right node
             *
             * @param index KEY or VALUE
             *
             * @return the right node -- may be null
             */
            internal Node getRight(int index)
            {
                return rightNode[index];
            }

            /**
             * Set this node's parent node
             *
             * @param node the new parent node
             * @param index KEY or VALUE
             */
            internal void setParent(Node node, int index)
            {
                parentNode[index] = node;
            }

            /**
             * get the parent node
             *
             * @param index KEY or VALUE
             *
             * @return the parent node -- may be null
             */
            internal Node getParent(int index)
            {
                return parentNode[index];
            }

            /**
             * exchange colors with another node
             *
             * @param node the node to swap with
             * @param index KEY or VALUE
             */
            internal void swapColors(Node node, int index)
            {

                // Swap colors -- old hacker's trick
                blackColor[index] ^= node.blackColor[index];
                node.blackColor[index] ^= blackColor[index];
                blackColor[index] ^= node.blackColor[index];
            }

            /**
             * is this node black?
             *
             * @param index KEY or VALUE
             *
             * @return true if black (which is represented as a true boolean)
             */
            internal bool isBlack(int index)
            {
                return blackColor[index];
            }

            /**
             * is this node red?
             *
             * @param index KEY or VALUE
             *
             * @return true if non-black
             */
            internal bool isRed(int index)
            {
                return !blackColor[index];
            }

            /**
             * make this node black
             *
             * @param index KEY or VALUE
             */
            internal void setBlack(int index)
            {
                blackColor[index] = true;
            }

            /**
             * make this node red
             *
             * @param index KEY or VALUE
             */
            internal void setRed(int index)
            {
                blackColor[index] = false;
            }

            /**
             * make this node the same color as another
             *
             * @param node the node whose color we're adopting
             * @param index KEY or VALUE
             */
            internal void copyColor(Node node, int index)
            {
                blackColor[index] = node.blackColor[index];
            }

            /* ********** START implementation of java.util.MapNS.Entry<Object,Object> ********** */

            /**
             * @return the key corresponding to this entry.
             */
            public Object getKey()
            {
                return data[KEY];
            }

            /**
             * @return the value corresponding to this entry.
             */
            public Object getValue()
            {
                return data[VALUE];
            }

            /**
             * Optional operation that is not permitted in this
             * implementation
             *
             * @param ignored
             *
             * @return does not return
             *
             * @throws UnsupportedOperationException
             */
            public Object setValue(Object ignored)
            {//throws UnsupportedOperationException {
                throw new java.lang.UnsupportedOperationException(
                    "java.util.MapNS.Entry<Object,Object>.setValue is not supported");
            }

            /**
             * Compares the specified object with this entry for equality.
             * Returns true if the given object is also a map entry and
             * the two entries represent the same mapping.
             *
             * @param o object to be compared for equality with this map
             *          entry.
             * @return true if the specified object is equal to this map
             *         entry.
             */
            public override bool Equals(Object o)
            {

                if (this == o)
                {
                    return true;
                }

                if (!(o is java.util.MapNS.Entry<Object, Object>))
                {
                    return false;
                }

                java.util.MapNS.Entry<Object, Object> e = (java.util.MapNS.Entry<Object, Object>)o;

                return data[KEY].equals(e.getKey())
                       && data[VALUE].equals(e.getValue());
            }

            /**
             * @return the hash code value for this map entry.
             */
            public override int GetHashCode()
            {

                if (!calculatedHashCode)
                {
                    hashcodeValue = data[KEY].GetHashCode()
                                         ^ data[VALUE].GetHashCode();
                    calculatedHashCode = true;
                }

                return hashcodeValue;
            }

            /* **********  END  implementation of java.util.MapNS.Entry<Object,Object> ********** */
        }
    }    // end public class DoubleOrderedMap
}