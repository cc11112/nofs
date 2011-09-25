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

namespace org.apache.commons.collections.iterators
{

    /**
     * Implements a <code>MapIterator</code> using a Map entrySet.
     * Reverse iteration is not supported.
     * <pre>
     * MapIterator it = map.mapIterator();
     * while (it.hasNext()) {
     *   Object key = it.next();
     *   Object value = it.getValue();
     *   it.setValue(newValue);
     * }
     * </pre>
     *  
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Stephen Colebourne
     */
    public class EntrySetMapIterator : MapIterator, ResettableIterator
    {

        private readonly java.util.Map<Object, Object> map;
        private java.util.Iterator<Object> iterator;
        private java.util.MapNS.Entry<Object, Object> last;
        private bool canRemove = false;

        /**
         * Constructor.
         * 
         * @param map  the map to iterate over
         */
        public EntrySetMapIterator(java.util.Map<Object, Object> map)
            : base()
        {
            this.map = map;
            this.iterator = (java.util.Iterator<Object>)map.entrySet().iterator();
        }

        //-----------------------------------------------------------------------    
        /**
         * Checks to see if there are more entries still to be iterated.
         *
         * @return <code>true</code> if the iterator has more elements
         */
        public bool hasNext()
        {
            return iterator.hasNext();
        }

        /**
         * Gets the next <em>key</em> from the <code>Map</code>.
         *
         * @return the next key in the iteration
         * @throws java.util.NoSuchElementException if the iteration is finished
         */
        public Object next()
        {
            last = (java.util.MapNS.Entry<Object, Object>)iterator.next();
            canRemove = true;
            return last.getKey();
        }

        //-----------------------------------------------------------------------
        /**
         * Removes the last returned key from the underlying <code>Map</code>.
         * <p>
         * This method can be called once per call to <code>next()</code>.
         *
         * @throws UnsupportedOperationException if remove is not supported by the map
         * @throws IllegalStateException if <code>next()</code> has not yet been called
         * @throws IllegalStateException if <code>remove()</code> has already been called
         *  since the last call to <code>next()</code>
         */
        public void remove()
        {
            if (canRemove == false)
            {
                throw new java.lang.IllegalStateException("Iterator remove() can only be called once after next()");
            }
            iterator.remove();
            last = null;
            canRemove = false;
        }

        //-----------------------------------------------------------------------
        /**
         * Gets the current key, which is the key returned by the last call
         * to <code>next()</code>.
         *
         * @return the current key
         * @throws IllegalStateException if <code>next()</code> has not yet been called
         */
        public Object getKey()
        {
            if (last == null)
            {
                throw new java.lang.IllegalStateException("Iterator getKey() can only be called after next() and before remove()");
            }
            return last.getKey();
        }

        /**
         * Gets the current value, which is the value associated with the last key
         * returned by <code>next()</code>.
         *
         * @return the current value
         * @throws IllegalStateException if <code>next()</code> has not yet been called
         */
        public Object getValue()
        {
            if (last == null)
            {
                throw new java.lang.IllegalStateException("Iterator getValue() can only be called after next() and before remove()");
            }
            return last.getValue();
        }

        /**
         * Sets the value associated with the current key.
         *
         * @param value  the new value
         * @return the previous value
         * @throws UnsupportedOperationException if setValue is not supported by the map
         * @throws IllegalStateException if <code>next()</code> has not yet been called
         * @throws IllegalStateException if <code>remove()</code> has been called since the
         *  last call to <code>next()</code>
         */
        public Object setValue(Object value)
        {
            if (last == null)
            {
                throw new java.lang.IllegalStateException("Iterator setValue() can only be called after next() and before remove()");
            }
            return last.setValue(value);
        }

        //-----------------------------------------------------------------------
        /**
         * Resets the state of the iterator.
         */
        public void reset()
        {
            iterator = (java.util.Iterator<Object>)map.entrySet().iterator();
            last = null;
            canRemove = false;
        }

        /**
         * Gets the iterator as a String.
         * 
         * @return a string version of the iterator
         */
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