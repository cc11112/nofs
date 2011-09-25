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

namespace org.apache.commons.collections.set
{
    /**
     * Decorates another <code>SortedSet</code> to provide additional behaviour.
     * <p>
     * Methods are forwarded directly to the decorated set.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    public abstract class AbstractSortedSetDecorator : AbstractSetDecorator, java.util.SortedSet<Object>
    {

        public override Object[] toArray()
        {
            return this.toArray(new Object[0]);
        }
        /*public override Object[] toArray(Object[] o)
        {
            Object[] result = o.Length < this.size() ? new Object[this.size()] : o;
            int i = 0;
            java.util.Iterator<Object> it = this.iterator();
            while (it.hasNext())
            {
                result[i] = it.next();
                i++;
            }
            for (; i < result.Length; i++)
            {
                result[i] = null;
            }

            return result;
        }
        */
        /**
         * Constructor only used in deserialization, do not use otherwise.
         * @since Commons Collections 3.1
         */
        protected AbstractSortedSetDecorator()
            : base()
        {
        }

        /**
         * Constructor that wraps (not copies).
         * 
         * @param set  the set to decorate, must not be null
         * @throws IllegalArgumentException if set is null
         */
        protected AbstractSortedSetDecorator(java.util.Set<Object> set)
            : base(set)
        {
        }

        /**
         * Gets the sorted set being decorated.
         * 
         * @return the decorated set
         */
        protected virtual java.util.SortedSet<Object> getSortedSet()
        {
            return (java.util.SortedSet<Object>)getCollection();
        }

        //-----------------------------------------------------------------------
        public virtual java.util.SortedSet<Object> subSet(Object fromElement, Object toElement)
        {
            return getSortedSet().subSet(fromElement, toElement);
        }

        public virtual java.util.SortedSet<Object> headSet(Object toElement)
        {
            return getSortedSet().headSet(toElement);
        }

        public virtual java.util.SortedSet<Object> tailSet(Object fromElement)
        {
            return getSortedSet().tailSet(fromElement);
        }

        public virtual Object first()
        {
            return getSortedSet().first();
        }

        public virtual Object last()
        {
            return getSortedSet().last();
        }

        public virtual java.util.Comparator<Object> comparator()
        {
            return getSortedSet().comparator();
        }

    }
}