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
using org.apache.commons.collections.iterators;

namespace org.apache.commons.collections.list
{

    /**
     * Decorates another <code>List</code> to transform objects that are added.
     * <p>
     * The add and set methods are affected by this class.
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
    public class TransformedList : TransformedCollection, java.util.List<Object>
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 1077193035000013141L;

        /**
         * Factory method to create a transforming list.
         * <p>
         * If there are any elements already in the list being decorated, they
         * are NOT transformed.
         * 
         * @param list  the list to decorate, must not be null
         * @param transformer  the transformer to use for conversion, must not be null
         * @throws IllegalArgumentException if list or transformer is null
         */
        public static java.util.List<Object> decorate(java.util.List<Object> list, Transformer transformer)
        {
            return new TransformedList(list, transformer);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * <p>
         * If there are any elements already in the list being decorated, they
         * are NOT transformed.
         * 
         * @param list  the list to decorate, must not be null
         * @param transformer  the transformer to use for conversion, must not be null
         * @throws IllegalArgumentException if list or transformer is null
         */
        protected TransformedList(java.util.List<Object> list, Transformer transformer)
            : base(list, transformer)
        {
        }

        /**
         * Gets the decorated list.
         * 
         * @return the decorated list
         */
        protected java.util.List<Object> getList()
        {
            return (java.util.List<Object>)collection;
        }

        //-----------------------------------------------------------------------
        public Object get(int index)
        {
            return getList().get(index);
        }

        public int indexOf(Object obj)
        {
            return getList().indexOf(obj);
        }

        public int lastIndexOf(Object obj)
        {
            return getList().lastIndexOf(obj);
        }

        public Object remove(int index)
        {
            return getList().remove(index);
        }

        //-----------------------------------------------------------------------
        public void add(int index, Object obj)
        {
            obj = transform(obj);
            getList().add(index, obj);
        }

        public bool addAll(int index, java.util.Collection<Object> coll)
        {
            coll = transform(coll);
            return getList().addAll(index, coll);
        }

        public java.util.ListIterator<Object> listIterator()
        {
            return listIterator(0);
        }

        public java.util.ListIterator<Object> listIterator(int i)
        {
            return new TransformedListIterator(this, getList().listIterator(i));
        }

        public Object set(int index, Object obj)
        {
            obj = transform(obj);
            return getList().set(index, obj);
        }

        public java.util.List<Object> subList(int fromIndex, int toIndex)
        {
            java.util.List<Object> sub = getList().subList(fromIndex, toIndex);
            return new TransformedList(sub, transformer);
        }
    }
    /**
     * Inner class Iterator for the TransformedList
     */
    internal class TransformedListIterator : AbstractListIteratorDecorator
    {

        private TransformedList root;
        protected internal TransformedListIterator(TransformedList root, java.util.ListIterator<Object> iterator)
            : base(iterator)
        {
            this.root = root;
        }

        public override void add(Object obj)
        {
            obj = root.transform(obj);
            iterator.add(obj);
        }

        public override void set(Object obj)
        {
            obj = root.transform(obj);
            iterator.set(obj);
        }
    }
}