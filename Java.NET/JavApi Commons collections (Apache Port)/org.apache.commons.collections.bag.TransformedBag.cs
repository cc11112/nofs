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
using org.apache.commons.collections.collection;
using org.apache.commons.collections.set;

namespace org.apache.commons.collections.bag
{

    /**
     * Decorates another <code>Bag</code> to transform objects that are added.
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
    public class TransformedBag
            : TransformedCollection, Bag
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 5421170911299074185L;

        /**
         * Factory method to create a transforming bag.
         * <p>
         * If there are any elements already in the bag being decorated, they
         * are NOT transformed.
         * 
         * @param bag  the bag to decorate, must not be null
         * @param transformer  the transformer to use for conversion, must not be null
         * @return a new transformed Bag
         * @throws IllegalArgumentException if bag or transformer is null
         */
        public static Bag decorate(Bag bag, Transformer transformer)
        {
            return new TransformedBag(bag, transformer);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * <p>
         * If there are any elements already in the bag being decorated, they
         * are NOT transformed.
         * 
         * @param bag  the bag to decorate, must not be null
         * @param transformer  the transformer to use for conversion, must not be null
         * @throws IllegalArgumentException if bag or transformer is null
         */
        protected TransformedBag(Bag bag, Transformer transformer)
            : base(bag, transformer)
        {
        }

        /**
         * Gets the decorated bag.
         * 
         * @return the decorated bag
         */
        protected Bag getBag()
        {
            return (Bag)collection;
        }

        //-----------------------------------------------------------------------
        public int getCount(Object obj)
        {
            return getBag().getCount(obj);
        }

        public bool remove(Object obj, int nCopies)
        {
            return getBag().remove(obj, nCopies);
        }

        //-----------------------------------------------------------------------
        public bool add(Object obj, int nCopies)
        {
            obj = transform(obj);
            return getBag().add(obj, nCopies);
        }

        public java.util.Set<Object> uniqueSet()
        {
            java.util.Set<Object> set = getBag().uniqueSet();
            return TransformedSet.decorate(set, transformer);
        }

    }
}