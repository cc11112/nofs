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
     * A Comparator which imposes a specific order on a specific set of Objects.
     * Objects are presented to the FixedOrderComparator in a specified order and
     * subsequent calls to {@link #compare(Object, Object) compare} yield that order.
     * For example:
     * <pre>
     * String[] planets = {"Mercury", "Venus", "Earth", "Mars"};
     * FixedOrderComparator distanceFromSun = new FixedOrderComparator(planets);
     * Arrays.sort(planets);                     // Sort to alphabetical order
     * Arrays.sort(planets, distanceFromSun);    // Back to original order
     * </pre>
     * <p>
     * Once <code>compare</code> has been called, the FixedOrderComparator is locked
     * and attempts to modify it yield an UnsupportedOperationException.
     * <p>
     * Instances of FixedOrderComparator are not synchronized.  The class is not
     * thread-safe at construction time, but it is thread-safe to perform
     * multiple comparisons  after all the setup operations are complete.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author David Leppik
     * @author Stephen Colebourne
     * @author Janek Bogucki
     */
    public class FixedOrderComparator : java.util.Comparator<Object>
    {

        /** 
         * Behavior when comparing unknown Objects:
         * unknown objects compare as before known Objects.
         */
        public const int UNKNOWN_BEFORE = 0;

        /** 
         * Behavior when comparing unknown Objects:
         * unknown objects compare as after known Objects.
         */
        public const int UNKNOWN_AFTER = 1;

        /** 
         * Behavior when comparing unknown Objects:
         * unknown objects cause a IllegalArgumentException to be thrown.
         * This is the default behavior.
         */
        public const int UNKNOWN_THROW_EXCEPTION = 2;

        /** Internal map of object to position */
        private readonly java.util.HashMap<Object, Object> map = new java.util.HashMap<Object, Object>(); // TODO use correct classes for this internal genreic instance
        /** Counter used in determining the position in the map */
        private int counter = 0;
        /** Is the comparator locked against further change */
        private bool isLockedJ = false;
        /** The behaviour in the case of an unknown object */
        private int unknownObjectBehavior = UNKNOWN_THROW_EXCEPTION;

        // Constructors
        //-----------------------------------------------------------------------
        /** 
         * Constructs an empty FixedOrderComparator.
         */
        public FixedOrderComparator()
            : base()
        {

        }

        /** 
         * Constructs a FixedOrderComparator which uses the order of the given array
         * to compare the objects.
         * <p>
         * The array is copied, so later changes will not affect the comparator.
         * 
         * @param items  the items that the comparator can compare in order
         * @throws IllegalArgumentException if the array is null
         */
        public FixedOrderComparator(Object[] items)
            : base()
        {

            if (items == null)
            {
                throw new java.lang.IllegalArgumentException("The list of items must not be null");
            }
            for (int i = 0; i < items.Length; i++)
            {
                add(items[i]);
            }
        }

        /** 
         * Constructs a FixedOrderComparator which uses the order of the given list
         * to compare the objects.
         * <p>
         * The list is copied, so later changes will not affect the comparator.
         * 
         * @param items  the items that the comparator can compare in order
         * @throws IllegalArgumentException if the list is null
         */
        public FixedOrderComparator(java.util.List<Object> items)
            : base()
        {

            if (items == null)
            {
                throw new java.lang.IllegalArgumentException("The list of items must not be null");
            }
            for (java.util.Iterator<Object> it = items.iterator(); it.hasNext(); )
            {
                add(it.next());
            }
        }

        // Bean methods / state querying methods
        //-----------------------------------------------------------------------
        /**
         * Returns true if modifications cannot be made to the FixedOrderComparator.
         * FixedOrderComparators cannot be modified once they have performed a comparison.
         * 
         * @return true if attempts to change the FixedOrderComparator yield an
         *  UnsupportedOperationException, false if it can be changed.
         */
        public bool isLocked()
        {
            return isLockedJ;
        }

        /**
         * Checks to see whether the comparator is now locked against further changes.
         * 
         * @throws UnsupportedOperationException if the comparator is locked
         */
        protected void checkLocked()
        {
            if (isLocked())
            {
                throw new java.lang.UnsupportedOperationException("Cannot modify a FixedOrderComparator after a comparison");
            }
        }

        /** 
         * Gets the behavior for comparing unknown objects.
         * 
         * @return the flag for unknown behaviour - UNKNOWN_AFTER,
         * UNKNOWN_BEFORE or UNKNOWN_THROW_EXCEPTION
         */
        public int getUnknownObjectBehavior()
        {
            return unknownObjectBehavior;
        }

        /** 
         * Sets the behavior for comparing unknown objects.
         * 
         * @param unknownObjectBehavior  the flag for unknown behaviour -
         * UNKNOWN_AFTER, UNKNOWN_BEFORE or UNKNOWN_THROW_EXCEPTION
         * @throws UnsupportedOperationException if a comparison has been performed
         * @throws IllegalArgumentException if the unknown flag is not valid
         */
        public void setUnknownObjectBehavior(int unknownObjectBehavior)
        {
            checkLocked();
            if (unknownObjectBehavior != UNKNOWN_AFTER
                && unknownObjectBehavior != UNKNOWN_BEFORE
                && unknownObjectBehavior != UNKNOWN_THROW_EXCEPTION)
            {
                throw new java.lang.IllegalArgumentException("Unrecognised value for unknown behaviour flag");
            }
            this.unknownObjectBehavior = unknownObjectBehavior;
        }

        // Methods for adding items
        //-----------------------------------------------------------------------
        /** 
         * Adds an item, which compares as after all items known to the Comparator.
         * If the item is already known to the Comparator, its old position is
         * replaced with the new position.
         * 
         * @param obj  the item to be added to the Comparator.
         * @return true if obj has been added for the first time, false if
         *  it was already known to the Comparator.
         * @throws UnsupportedOperationException if a comparison has already been made
         */
        public bool add(Object obj)
        {
            checkLocked();
            Object position = map.put(obj, new java.lang.Integer(counter++));
            return (position == null);
        }

        /**
         * Adds a new item, which compares as equal to the given existing item.
         * 
         * @param existingObj  an item already in the Comparator's set of 
         *  known objects
         * @param newObj  an item to be added to the Comparator's set of
         *  known objects
         * @return true if newObj has been added for the first time, false if
         *  it was already known to the Comparator.
         * @throws IllegalArgumentException if existingObject is not in the 
         *  Comparator's set of known objects.
         * @throws UnsupportedOperationException if a comparison has already been made
         */
        public bool addAsEqual(Object existingObj, Object newObj)
        {
            checkLocked();
            java.lang.Integer position = (java.lang.Integer)map.get(existingObj);
            if (position == null)
            {
                throw new java.lang.IllegalArgumentException(existingObj + " not known to " + this);
            }
            Object result = map.put(newObj, position);
            return (result == null);
        }

        // Comparator methods
        //-----------------------------------------------------------------------
        /** 
         * Compares two objects according to the order of this Comparator.
         * <p>
         * It is important to note that this class will throw an IllegalArgumentException
         * in the case of an unrecognised object. This is not specified in the 
         * Comparator interface, but is the most appropriate exception.
         * 
         * @param obj1  the first object to compare
         * @param obj2  the second object to compare
         * @return negative if obj1 is less, positive if greater, zero if equal
         * @throws IllegalArgumentException if obj1 or obj2 are not known 
         *  to this Comparator and an alternative behavior has not been set
         *  via {@link #setUnknownObjectBehavior(int)}.
         */
        public int compare(Object obj1, Object obj2)
        {
            isLockedJ = true;
            java.lang.Integer position1 = (java.lang.Integer)map.get(obj1);
            java.lang.Integer position2 = (java.lang.Integer)map.get(obj2);
            if (position1 == null || position2 == null)
            {
                switch (unknownObjectBehavior)
                {
                    case UNKNOWN_BEFORE:
                        if (position1 == null)
                        {
                            return (position2 == null) ? 0 : -1;
                        }
                        else
                        {
                            return 1;
                        }
                    case UNKNOWN_AFTER:
                        if (position1 == null)
                        {
                            return (position2 == null) ? 0 : 1;
                        }
                        else
                        {
                            return -1;
                        }
                    case UNKNOWN_THROW_EXCEPTION:
                        Object unknownObj = (position1 == null) ? obj1 : obj2;
                        throw new java.lang.IllegalArgumentException("Attempting to compare unknown object " + unknownObj);
                    default:
                        throw new java.lang.UnsupportedOperationException("Unknown unknownObjectBehavior: " + unknownObjectBehavior);
                }
            }
            else
            {
                return position1.compareTo(position2);
            }
        }
        public virtual bool equals(Object obj)
        {
            return Equals(obj);
        }
    }

}