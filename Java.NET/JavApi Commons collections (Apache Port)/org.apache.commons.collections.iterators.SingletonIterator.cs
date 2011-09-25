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
     * <code>SingletonIterator</code> is an {@link Iterator} over a single 
     * object instance.
     *
     * @since Commons Collections 2.0
     * @version $Revision$ $Date$
     * 
     * @author James Strachan
     * @author Stephen Colebourne
     * @author Rodney Waldhoff
     */
    public class SingletonIterator
            : java.util.Iterator<Object>, ResettableIterator
    {

        /** Whether remove is allowed */
        private readonly bool removeAllowed;
        /** Is the cursor before the first element */
        private bool beforeFirst = true;
        /** Has the element been removed */
        private bool removed = false;
        /** The object */
        private Object obj;

        /**
         * Constructs a new <code>SingletonIterator</code> where <code>remove</code>
         * is a permitted operation.
         *
         * @param object  the single object to return from the iterator
         */
        public SingletonIterator(Object obj)
            : this(obj, true)
        {
        }

        /**
         * Constructs a new <code>SingletonIterator</code> optionally choosing if
         * <code>remove</code> is a permitted operation.
         *
         * @param object  the single object to return from the iterator
         * @param removeAllowed  true if remove is allowed
         * @since Commons Collections 3.1
         */
        public SingletonIterator(Object obj, bool removeAllowed)
            : base()
        {
            this.obj = obj;
            this.removeAllowed = removeAllowed;
        }

        //-----------------------------------------------------------------------
        /**
         * Is another object available from the iterator?
         * <p>
         * This returns true if the single object hasn't been returned yet.
         * 
         * @return true if the single object hasn't been returned yet
         */
        public bool hasNext()
        {
            return (beforeFirst && !removed);
        }

        /**
         * Get the next object from the iterator.
         * <p>
         * This returns the single object if it hasn't been returned yet.
         *
         * @return the single object
         * @throws NoSuchElementException if the single object has already 
         *    been returned
         */
        public Object next()
        {
            if (!beforeFirst || removed)
            {
                throw new java.util.NoSuchElementException();
            }
            beforeFirst = false;
            return obj;
        }

        /**
         * Remove the object from this iterator.
         * 
         * @throws IllegalStateException if the <tt>next</tt> method has not
         *        yet been called, or the <tt>remove</tt> method has already
         *        been called after the last call to the <tt>next</tt>
         *        method.
         * @throws UnsupportedOperationException if remove is not supported
         */
        public void remove()
        {
            if (removeAllowed)
            {
                if (removed || beforeFirst)
                {
                    throw new java.lang.IllegalStateException();
                }
                else
                {
                    obj = null;
                    removed = true;
                }
            }
            else
            {
                throw new java.lang.UnsupportedOperationException();
            }
        }

        /**
         * Reset the iterator to the start.
         */
        public void reset()
        {
            beforeFirst = true;
        }

    }
}