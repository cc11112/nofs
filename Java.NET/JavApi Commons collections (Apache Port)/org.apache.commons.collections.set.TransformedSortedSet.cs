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
     * Decorates another <code>SortedSet</code> to transform objects that are added.
     * <p>
     * The add methods are affected by this class.
     * Thus objects must be removed or searched for using their transformed form.
     * For example, if the transformation converts Strings to Integers, you must
     * use the Integer form to remove objects.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    [Serializable]
    public class TransformedSortedSet : TransformedSet, java.util.SortedSet<Object>
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -1675486811351124386L;

        /**
         * Factory method to create a transforming sorted set.
         * <p>
         * If there are any elements already in the set being decorated, they
         * are NOT transformed.
         * 
         * @param set  the set to decorate, must not be null
         * @param transformer  the transformer to use for conversion, must not be null
         * @throws IllegalArgumentException if set or transformer is null
         */
        public static java.util.SortedSet<Object> decorate(java.util.SortedSet<Object> set, Transformer transformer)
        {
            return new TransformedSortedSet(set, transformer);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * <p>
         * If there are any elements already in the set being decorated, they
         * are NOT transformed.
         * 
         * @param set  the set to decorate, must not be null
         * @param transformer  the transformer to use for conversion, must not be null
         * @throws IllegalArgumentException if set or transformer is null
         */
        protected TransformedSortedSet(java.util.SortedSet<Object> set, Transformer transformer)
            : base(set, transformer)
        {
        }

        /**
         * Gets the decorated set.
         * 
         * @return the decorated set
         */
        protected java.util.SortedSet<Object> getSortedSet()
        {
            return (java.util.SortedSet<Object>)collection;
        }

        //-----------------------------------------------------------------------
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

        //-----------------------------------------------------------------------
        public java.util.SortedSet<Object> subSet(Object fromElement, Object toElement)
        {
            java.util.SortedSet<Object> set = getSortedSet().subSet(fromElement, toElement);
            return new TransformedSortedSet(set, transformer);
        }

        public java.util.SortedSet<Object> headSet(Object toElement)
        {
            java.util.SortedSet<Object> set = getSortedSet().headSet(toElement);
            return new TransformedSortedSet(set, transformer);
        }

        public java.util.SortedSet<Object> tailSet(Object fromElement)
        {
            java.util.SortedSet<Object> set = getSortedSet().tailSet(fromElement);
            return new TransformedSortedSet(set, transformer);
        }

    }
}