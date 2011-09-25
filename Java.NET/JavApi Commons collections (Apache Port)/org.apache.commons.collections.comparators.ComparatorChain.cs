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

namespace org.apache.commons.collections.comparators
{

    /**
     * <p>A ComparatorChain is a Comparator that wraps one or
     * more Comparators in sequence.  The ComparatorChain
     * calls each Comparator in sequence until either 1)
     * any single Comparator returns a non-zero result
     * (and that result is then returned),
     * or 2) the ComparatorChain is exhausted (and zero is
     * returned).  This type of sorting is very similar
     * to multi-column sorting in SQL, and this class
     * allows Java classes to emulate that kind of behaviour
     * when sorting a List.</p>
     * 
     * <p>To further facilitate SQL-like sorting, the order of
     * any single Comparator in the list can be reversed.</p>
     * 
     * <p>Calling a method that adds new Comparators or
     * changes the ascend/descend sort <i>after compare(Object,
     * Object) has been called</i> will result in an
     * UnsupportedOperationException.  However, <i>take care</i>
     * to not alter the underlying List of Comparators
     * or the BitSet that defines the sort order.</p>
     * 
     * <p>Instances of ComparatorChain are not synchronized.
     * The class is not thread-safe at construction time, but
     * it <i>is</i> thread-safe to perform multiple comparisons
     * after all the setup operations are complete.</p>
     * 
     * @since Commons Collections 2.0
     * @author Morgan Delagrange
     * @version $Revision$ $Date$
     */
    [Serializable]
    public class ComparatorChain : java.util.Comparator<Object>, java.io.Serializable
    {

        /** Serialization version from Collections 2.0. */
        private static readonly long serialVersionUID = -721644942746081630L;

        /** The list of comparators in the chain. */
        protected java.util.List<Object> comparatorChain = null;
        /** Order - false (clear) = ascend; true (set) = descend. */
        protected java.util.BitSet orderingBits = null;
        /** Whether the chain has been "locked". */
        protected bool isLockedJ = false;

        //-----------------------------------------------------------------------
        /**
         * Construct a ComparatorChain with no Comparators.
         * You must add at least one Comparator before calling
         * the compare(Object,Object) method, or an 
         * UnsupportedOperationException is thrown
         */
        public ComparatorChain() :
            this(new java.util.ArrayList<Object>(), new java.util.BitSet())
        {
        }

        /**
         * Construct a ComparatorChain with a single Comparator,
         * sorting in the forward order
         * 
         * @param comparator First comparator in the Comparator chain
         */
        public ComparatorChain(java.util.Comparator<Object> comparator) :
            this(comparator, false)
        {
        }

        /**
         * Construct a Comparator chain with a single Comparator,
         * sorting in the given order
         * 
         * @param comparator First Comparator in the ComparatorChain
         * @param reverse    false = forward sort; true = reverse sort
         */
        public ComparatorChain(java.util.Comparator<Object> comparator, bool reverse)
        {
            comparatorChain = new java.util.ArrayList<Object>();
            comparatorChain.add(comparator);
            orderingBits = new java.util.BitSet(1);
            if (reverse == true)
            {
                orderingBits.set(0);
            }
        }

        /**
         * Construct a ComparatorChain from the Comparators in the
         * List.  All Comparators will default to the forward 
         * sort order.
         * 
         * @param list   List of Comparators
         * @see #ComparatorChain(List,BitSet)
         */
        public ComparatorChain(java.util.List<Object> list) :
            this(list, new java.util.BitSet(list.size()))
        {
        }

        /**
         * Construct a ComparatorChain from the Comparators in the
         * given List.  The sort order of each column will be
         * drawn from the given BitSet.  When determining the sort
         * order for Comparator at index <i>i</i> in the List,
         * the ComparatorChain will call BitSet.get(<i>i</i>).
         * If that method returns <i>false</i>, the forward
         * sort order is used; a return value of <i>true</i>
         * indicates reverse sort order.
         * 
         * @param list   List of Comparators.  NOTE: This constructor does not perform a
         *               defensive copy of the list
         * @param bits   Sort order for each Comparator.  Extra bits are ignored,
         *               unless extra Comparators are added by another method.
         */
        public ComparatorChain(java.util.List<Object> list, java.util.BitSet bits)
        {
            comparatorChain = list;
            orderingBits = bits;
        }

        //-----------------------------------------------------------------------
        /**
         * Add a Comparator to the end of the chain using the
         * forward sort order
         * 
         * @param comparator Comparator with the forward sort order
         */
        public void addComparator(java.util.Comparator<Object> comparator)
        {
            addComparator(comparator, false);
        }

        /**
         * Add a Comparator to the end of the chain using the
         * given sort order
         * 
         * @param comparator Comparator to add to the end of the chain
         * @param reverse    false = forward sort order; true = reverse sort order
         */
        public void addComparator(java.util.Comparator<Object> comparator, bool reverse)
        {
            checkLocked();

            comparatorChain.add(comparator);
            if (reverse == true)
            {
                orderingBits.set(comparatorChain.size() - 1);
            }
        }

        /**
         * Replace the Comparator at the given index, maintaining
         * the existing sort order.
         * 
         * @param index      index of the Comparator to replace
         * @param comparator Comparator to place at the given index
         * @exception IndexOutOfBoundsException
         *                   if index &lt; 0 or index &gt;= size()
         */
        public void setComparator(int index, java.util.Comparator<Object> comparator)
        {//throws IndexOutOfBoundsException {
            setComparator(index, comparator, false);
        }

        /**
         * Replace the Comparator at the given index in the
         * ComparatorChain, using the given sort order
         * 
         * @param index      index of the Comparator to replace
         * @param comparator Comparator to set
         * @param reverse    false = forward sort order; true = reverse sort order
         */
        public void setComparator(int index, java.util.Comparator<Object> comparator, bool reverse)
        {
            checkLocked();

            comparatorChain.set(index, comparator);
            if (reverse == true)
            {
                orderingBits.set(index);
            }
            else
            {
                orderingBits.clear(index);
            }
        }


        /**
         * Change the sort order at the given index in the
         * ComparatorChain to a forward sort.
         * 
         * @param index  Index of the ComparatorChain
         */
        public void setForwardSort(int index)
        {
            checkLocked();
            orderingBits.clear(index);
        }

        /**
         * Change the sort order at the given index in the
         * ComparatorChain to a reverse sort.
         * 
         * @param index  Index of the ComparatorChain
         */
        public void setReverseSort(int index)
        {
            checkLocked();
            orderingBits.set(index);
        }

        /**
         * Number of Comparators in the current ComparatorChain.
         * 
         * @return Comparator count
         */
        public int size()
        {
            return comparatorChain.size();
        }

        /**
         * Determine if modifications can still be made to the
         * ComparatorChain.  ComparatorChains cannot be modified
         * once they have performed a comparison.
         * 
         * @return true = ComparatorChain cannot be modified; false = 
         *         ComparatorChain can still be modified.
         */
        public bool isLocked()
        {
            return isLockedJ;
        }

        // throw an exception if the ComparatorChain is locked
        private void checkLocked()
        {
            if (isLockedJ == true)
            {
                throw new java.lang.UnsupportedOperationException("Comparator ordering cannot be changed after the first comparison is performed");
            }
        }

        private void checkChainIntegrity()
        {
            if (comparatorChain.size() == 0)
            {
                throw new java.lang.UnsupportedOperationException("ComparatorChains must contain at least one Comparator");
            }
        }

        //-----------------------------------------------------------------------
        /**
         * Perform comparisons on the Objects as per
         * Comparator.compare(o1,o2).
         * 
         * @param o1  the first object to compare
         * @param o2  the second object to compare
         * @return -1, 0, or 1
         * @exception UnsupportedOperationException
         *                   if the ComparatorChain does not contain at least one
         *                   Comparator
         */
        public int compare(Object o1, Object o2)
        {//throws UnsupportedOperationException {
            if (isLockedJ == false)
            {
                checkChainIntegrity();
                isLockedJ = true;
            }

            // iterate over all comparators in the chain
            java.util.Iterator<Object> comparators = comparatorChain.iterator();
            for (int comparatorIndex = 0; comparators.hasNext(); ++comparatorIndex)
            {

                java.util.Comparator<Object> comparator = (java.util.Comparator<Object>)comparators.next();
                int retval = comparator.compare(o1, o2);
                if (retval != 0)
                {
                    // invert the order if it is a reverse sort
                    if (orderingBits.get(comparatorIndex) == true)
                    {
                        if (java.lang.Integer.MIN_VALUE == retval)
                        {
                            retval = java.lang.Integer.MAX_VALUE;
                        }
                        else
                        {
                            retval *= -1;
                        }
                    }

                    return retval;
                }

            }

            // if comparators are exhausted, return 0
            return 0;
        }

        //-----------------------------------------------------------------------
        /**
         * Implement a hash code for this comparator that is consistent with
         * {@link #equals(Object) equals}.
         * 
         * @return a suitable hash code
         * @since Commons Collections 3.0
         */
        public override int GetHashCode()
        {
            int hash = 0;
            if (null != comparatorChain)
            {
                hash ^= comparatorChain.GetHashCode();
            }
            if (null != orderingBits)
            {
                hash ^= orderingBits.GetHashCode();
            }
            return hash;
        }

        /**
         * Returns <code>true</code> iff <i>that</i> Object is 
         * is a {@link Comparator} whose ordering is known to be 
         * equivalent to mine.
         * <p>
         * This implementation returns <code>true</code>
         * iff <code><i>object</i>.{@link Object#getClass() getClass()}</code>
         * equals <code>this.getClass()</code>, and the underlying 
         * comparators and order bits are equal.
         * Subclasses may want to override this behavior to remain consistent
         * with the {@link Comparator#equals(Object)} contract.
         * 
         * @param object  the object to compare with
         * @return true if equal
         * @since Commons Collections 3.0
         */
        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }
            else if (null == obj)
            {
                return false;
            }
            else if (obj.getClass().Equals(this.getClass()))
            {
                ComparatorChain chain = (ComparatorChain)obj;
                return ((null == orderingBits ? null == chain.orderingBits : orderingBits.equals(chain.orderingBits))
                       && (null == comparatorChain ? null == chain.comparatorChain : comparatorChain.equals(chain.comparatorChain)));
            }
            else
            {
                return false;
            }
        }
        public virtual bool equals(Object obj)
        {
            return Equals(obj);
        }
    }
}