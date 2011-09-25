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

namespace org.apache.commons.collections.list
{

    /**
     * A <code>List</code> implementation that stores a cache of internal Node objects
     * in an effort to reduce wasteful object creation.
     * <p>
     * A linked list creates one Node for each item of data added. This can result in
     * a lot of object creation and garbage collection. This implementation seeks to
     * avoid that by maintaining a store of cached nodes.
     * <p>
     * This implementation is suitable for long-lived lists where both add and remove
     * are used. Short-lived lists, or lists which only grow will have worse performance
     * using this class.
     * <p>
     * <b>Note that this implementation is not synchronized.</b>
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Jeff Varszegi
     * @author Rich Dougherty
     * @author Phil Steitz
     * @author Stephen Colebourne
     */
    [Serializable]
    public class NodeCachingLinkedList : AbstractLinkedList, java.io.Serializable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 6897789178562232073L;

        /**
         * The default value for {@link #maximumCacheSize}.
         */
        protected static readonly int DEFAULT_MAXIMUM_CACHE_SIZE = 20;

        /**
         * The first cached node, or <code>null</code> if no nodes are cached.
         * Cached nodes are stored in a singly-linked list with
         * <code>next</code> pointing to the next element.
         */
        [NonSerialized]
        protected Node firstCachedNode;

        /**
         * The size of the cache.
         */
        [NonSerialized]
        protected int cacheSize;

        /**
         * The maximum size of the cache.
         */
        protected int maximumCacheSize;

        //-----------------------------------------------------------------------
        /**
         * Constructor that creates.
         */
        public NodeCachingLinkedList()
            : this(DEFAULT_MAXIMUM_CACHE_SIZE)
        {
        }

        /**
         * Constructor that copies the specified collection
         * 
         * @param coll  the collection to copy
         */
        public NodeCachingLinkedList(java.util.Collection<Object> coll)
            : base(coll)
        {
            this.maximumCacheSize = DEFAULT_MAXIMUM_CACHE_SIZE;
        }

        /**
         * Constructor that species the maximum cache size.
         *
         * @param maximumCacheSize  the maximum cache size
         */
        public NodeCachingLinkedList(int maximumCacheSize)
            : base()
        {
            this.maximumCacheSize = maximumCacheSize;
            init();  // must call init() as use super();
        }

        //-----------------------------------------------------------------------
        /**
         * Gets the maximum size of the cache.
         * 
         * @return the maximum cache size
         */
        protected virtual int getMaximumCacheSize()
        {
            return maximumCacheSize;
        }

        /**
         * Sets the maximum size of the cache.
         * 
         * @param maximumCacheSize  the new maximum cache size
         */
        protected virtual void setMaximumCacheSize(int maximumCacheSize)
        {
            this.maximumCacheSize = maximumCacheSize;
            shrinkCacheToMaximumSize();
        }

        /**
         * Reduce the size of the cache to the maximum, if necessary.
         */
        protected virtual void shrinkCacheToMaximumSize()
        {
            // Rich Dougherty: This could be more efficient.
            while (cacheSize > maximumCacheSize)
            {
                getNodeFromCache();
            }
        }

        /**
         * Gets a node from the cache. If a node is returned, then the value of
         * {@link #cacheSize} is decreased accordingly. The node that is returned
         * will have <code>null</code> values for next, previous and element.
         *
         * @return a node, or <code>null</code> if there are no nodes in the cache.
         */
        protected virtual Node getNodeFromCache()
        {
            if (cacheSize == 0)
            {
                return null;
            }
            Node cachedNode = firstCachedNode;
            firstCachedNode = cachedNode.next;
            cachedNode.next = null; // This should be changed anyway, but defensively
            // set it to null.                    
            cacheSize--;
            return cachedNode;
        }

        /**
         * Checks whether the cache is full.
         * 
         * @return true if the cache is full
         */
        protected virtual bool isCacheFull()
        {
            return cacheSize >= maximumCacheSize;
        }

        /**
         * Adds a node to the cache, if the cache isn't full.
         * The node's contents are cleared to so they can be garbage collected.
         * 
         * @param node  the node to add to the cache
         */
        protected virtual void addNodeToCache(Node node)
        {
            if (isCacheFull())
            {
                // don't cache the node.
                return;
            }
            // clear the node's contents and add it to the cache.
            Node nextCachedNode = firstCachedNode;
            node.previous = null;
            node.next = nextCachedNode;
            node.setValue(null);
            firstCachedNode = node;
            cacheSize++;
        }

        //-----------------------------------------------------------------------    
        /**
         * Creates a new node, either by reusing one from the cache or creating
         * a new one.
         * 
         * @param value  value of the new node
         * @return the newly created node
         */
        protected override Node createNode(Object value)
        {
            Node cachedNode = getNodeFromCache();
            if (cachedNode == null)
            {
                return base.createNode(value);
            }
            else
            {
                cachedNode.setValue(value);
                return cachedNode;
            }
        }

        /**
         * Removes the node from the list, storing it in the cache for reuse
         * if the cache is not yet full.
         * 
         * @param node  the node to remove
         */
        protected internal override void removeNode(Node node)
        {
            base.removeNode(node);
            addNodeToCache(node);
        }

        /**
         * Removes all the nodes from the list, storing as many as required in the
         * cache for reuse.
         * 
         */
        protected override void removeAllNodes()
        {
            // Add the removed nodes to the cache, then remove the rest.
            // We can add them to the cache before removing them, since
            // {@link AbstractLinkedList.removeAllNodes()} removes the
            // nodes by removing references directly from {@link #header}.
            int numberOfNodesToCache = java.lang.Math.min(sizeJ, maximumCacheSize - cacheSize);
            Node node = header.next;
            for (int currentIndex = 0; currentIndex < numberOfNodesToCache; currentIndex++)
            {
                Node oldNode = node;
                node = node.next;
                addNodeToCache(oldNode);
            }
            base.removeAllNodes();
        }

        //-----------------------------------------------------------------------
        /**
         * Serializes the data held in this object to the stream specified.
         */
        private void writeObject(java.io.ObjectOutputStream outJ)
        {//throws IOException {
            outJ.defaultWriteObject();
            doWriteObject(outJ);
        }

        /**
         * Deserializes the data held in this object to the stream specified.
         */
        private void readObject(java.io.ObjectInputStream inJ)
        {// throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            doReadObject(inJ);
        }

    }
}
