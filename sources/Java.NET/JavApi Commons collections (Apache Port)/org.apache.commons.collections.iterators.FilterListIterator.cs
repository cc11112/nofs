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
     * Decorates another {@link ListIterator} using a predicate to filter elements.
     * <p>
     * This iterator decorates the underlying iterator, only allowing through
     * those elements that match the specified {@link Predicate Predicate}.
     *
     * @since Commons Collections 2.0
     * @version $Revision$ $Date$
     * 
     * @author Rodney Waldhoff
     */
    public class FilterListIterator : java.util.ListIterator<Object>
    {

        /** The iterator being used */
        private java.util.ListIterator<Object> iterator;

        /** The predicate being used */
        private Predicate predicate;

        /** 
         * The value of the next (matching) object, when 
         * {@link #nextObjectSet} is true. 
         */
        private Object nextObject;

        /** 
         * Whether or not the {@link #nextObject} has been set
         * (possibly to <code>null</code>). 
         */
        private bool nextObjectSet = false;

        /** 
         * The value of the previous (matching) object, when 
         * {@link #previousObjectSet} is true. 
         */
        private Object previousObject;

        /** 
         * Whether or not the {@link #previousObject} has been set
         * (possibly to <code>null</code>). 
         */
        private bool previousObjectSet = false;

        /** 
         * The index of the element that would be returned by {@link #next}.
         */
        private int nextIndexJ = 0;

        //-----------------------------------------------------------------------
        /**
         * Constructs a new <code>FilterListIterator</code> that will not function
         * until {@link #setListIterator(ListIterator) setListIterator}
         * and {@link #setPredicate(Predicate) setPredicate} are invoked.
         */
        public FilterListIterator()
            : base()
        {
        }

        /**
         * Constructs a new <code>FilterListIterator</code> that will not 
         * function until {@link #setPredicate(Predicate) setPredicate} is invoked.
         *
         * @param iterator  the iterator to use
         */
        public FilterListIterator(java.util.ListIterator<Object> iterator)
            : base()
        {
            this.iterator = iterator;
        }

        /**
         * Constructs a new <code>FilterListIterator</code>.
         *
         * @param iterator  the iterator to use
         * @param predicate  the predicate to use
         */
        public FilterListIterator(java.util.ListIterator<Object> iterator, Predicate predicate)
            : base()
        {
            this.iterator = iterator;
            this.predicate = predicate;
        }

        /**
         * Constructs a new <code>FilterListIterator</code> that will not function
         * until {@link #setListIterator(ListIterator) setListIterator} is invoked.
         *
         * @param predicate  the predicate to use.
         */
        public FilterListIterator(Predicate predicate)
            : base()
        {
            this.predicate = predicate;
        }

        //-----------------------------------------------------------------------
        /** Not supported. */
        public void add(Object o)
        {
            throw new java.lang.UnsupportedOperationException("FilterListIterator.add(Object) is not supported.");
        }

        public bool hasNext()
        {
            if (nextObjectSet)
            {
                return true;
            }
            else
            {
                return setNextObject();
            }
        }

        public bool hasPrevious()
        {
            if (previousObjectSet)
            {
                return true;
            }
            else
            {
                return setPreviousObject();
            }
        }

        public Object next()
        {
            if (!nextObjectSet)
            {
                if (!setNextObject())
                {
                    throw new java.util.NoSuchElementException();
                }
            }
            nextIndexJ++;
            Object temp = nextObject;
            clearNextObject();
            return temp;
        }

        public int nextIndex()
        {
            return nextIndexJ;
        }

        public Object previous()
        {
            if (!previousObjectSet)
            {
                if (!setPreviousObject())
                {
                    throw new java.util.NoSuchElementException();
                }
            }
            nextIndexJ--;
            Object temp = previousObject;
            clearPreviousObject();
            return temp;
        }

        public int previousIndex()
        {
            return (nextIndexJ - 1);
        }

        /** Not supported. */
        public void remove()
        {
            throw new java.lang.UnsupportedOperationException("FilterListIterator.remove() is not supported.");
        }

        /** Not supported. */
        public void set(Object o)
        {
            throw new java.lang.UnsupportedOperationException("FilterListIterator.set(Object) is not supported.");
        }

        //-----------------------------------------------------------------------
        /** 
         * Gets the iterator this iterator is using.
         * 
         * @return the iterator.
         */
        public java.util.ListIterator<Object> getListIterator()
        {
            return iterator;
        }

        /** 
         * Sets the iterator for this iterator to use.
         * If iteration has started, this effectively resets the iterator.
         * 
         * @param iterator  the iterator to use
         */
        public void setListIterator(java.util.ListIterator<Object> iterator)
        {
            this.iterator = iterator;
        }

        //-----------------------------------------------------------------------
        /** 
         * Gets the predicate this iterator is using.
         * 
         * @return the predicate.
         */
        public Predicate getPredicate()
        {
            return predicate;
        }

        /** 
         * Sets the predicate this the iterator to use.
         * 
         * @param predicate  the transformer to use
         */
        public void setPredicate(Predicate predicate)
        {
            this.predicate = predicate;
        }

        //-----------------------------------------------------------------------
        private void clearNextObject()
        {
            nextObject = null;
            nextObjectSet = false;
        }

        private bool setNextObject()
        {
            // if previousObjectSet,
            // then we've walked back one step in the 
            // underlying list (due to a hasPrevious() call)
            // so skip ahead one matching object
            if (previousObjectSet)
            {
                clearPreviousObject();
                if (!setNextObject())
                {
                    return false;
                }
                else
                {
                    clearNextObject();
                }
            }

            while (iterator.hasNext())
            {
                Object obj = iterator.next();
                if (predicate.evaluate(obj))
                {
                    nextObject = obj;
                    nextObjectSet = true;
                    return true;
                }
            }
            return false;
        }

        private void clearPreviousObject()
        {
            previousObject = null;
            previousObjectSet = false;
        }

        private bool setPreviousObject()
        {
            // if nextObjectSet,
            // then we've walked back one step in the 
            // underlying list (due to a hasNext() call)
            // so skip ahead one matching object
            if (nextObjectSet)
            {
                clearNextObject();
                if (!setPreviousObject())
                {
                    return false;
                }
                else
                {
                    clearPreviousObject();
                }
            }

            while (iterator.hasPrevious())
            {
                Object obj = iterator.previous();
                if (predicate.evaluate(obj))
                {
                    previousObject = obj;
                    previousObjectSet = true;
                    return true;
                }
            }
            return false;
        }

    }
}