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

namespace org.apache.commons.collections.set
{

    /**
     * Decorates another <code>SortedSet</code> to validate that all additions
     * match a specified predicate.
     * <p>
     * This set exists to provide validation for the decorated set.
     * It is normally created to decorate an empty set.
     * If an object cannot be added to the set, an IllegalArgumentException is thrown.
     * <p>
     * One usage would be to ensure that no null entries are added to the set.
     * <pre>SortedSet set = PredicatedSortedSet.decorate(new TreeSet(), NotNullPredicate.INSTANCE);</pre>
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     * @author Paul Jack
     */
    [Serializable]
    public class PredicatedSortedSet : PredicatedSet, java.util.SortedSet<Object>
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -9110948148132275052L;

        /**
         * Factory method to create a predicated (validating) sorted set.
         * <p>
         * If there are any elements already in the set being decorated, they
         * are validated.
         * 
         * @param set  the set to decorate, must not be null
         * @param predicate  the predicate to use for validation, must not be null
         * @throws IllegalArgumentException if set or predicate is null
         * @throws IllegalArgumentException if the set contains invalid elements
         */
        public static java.util.SortedSet<Object> decorate(java.util.SortedSet<Object> set, Predicate predicate)
        {
            return new PredicatedSortedSet(set, predicate);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * <p>
         * If there are any elements already in the set being decorated, they
         * are validated.
         * 
         * @param set  the set to decorate, must not be null
         * @param predicate  the predicate to use for validation, must not be null
         * @throws IllegalArgumentException if set or predicate is null
         * @throws IllegalArgumentException if the set contains invalid elements
         */
        protected internal PredicatedSortedSet(java.util.SortedSet<Object> set, Predicate predicate)
            : base(set, predicate)
        {
        }

        /**
         * Gets the sorted set being decorated.
         * 
         * @return the decorated sorted set
         */
        private java.util.SortedSet<Object> getSortedSet()
        {
            return (java.util.SortedSet<Object>)getCollection();
        }

        //-----------------------------------------------------------------------
        public java.util.SortedSet<Object> subSet(Object fromElement, Object toElement)
        {
            java.util.SortedSet<Object> sub = getSortedSet().subSet(fromElement, toElement);
            return new PredicatedSortedSet(sub, predicate);
        }

        public java.util.SortedSet<Object> headSet(Object toElement)
        {
            java.util.SortedSet<Object> sub = getSortedSet().headSet(toElement);
            return new PredicatedSortedSet(sub, predicate);
        }

        public java.util.SortedSet<Object> tailSet(Object fromElement)
        {
            java.util.SortedSet<Object> sub = getSortedSet().tailSet(fromElement);
            return new PredicatedSortedSet(sub, predicate);
        }

        public Object first()
        {
            return getSortedSet().first();
        }

        public Object last()
        {
            return getSortedSet().last();
        }

        public java.util.Comparator<Object> comparator()
        {
            return getSortedSet().comparator();
        }

    }
}