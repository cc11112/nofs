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
 */

using System;
using java = biz.ritter.javapi;
using org.apache.commons.collections;
using org.apache.commons.collections.collection;
using org.apache.commons.collections.iterators;

namespace org.apache.commons.collections.list
{

    /**
     * Decorates another <code>List</code> to validate that all additions
     * match a specified predicate.
     * <p>
     * This list exists to provide validation for the decorated list.
     * It is normally created to decorate an empty list.
     * If an object cannot be added to the list, an IllegalArgumentException is thrown.
     * <p>
     * One usage would be to ensure that no null entries are added to the list.
     * <pre>List list = PredicatedList.decorate(new ArrayList(), NotNullPredicate.INSTANCE);</pre>
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     * @author Paul Jack
     */
    public class PredicatedList : PredicatedCollection, java.util.List<Object>
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -5722039223898659102L;

        /**
         * Factory method to create a predicated (validating) list.
         * <p>
         * If there are any elements already in the list being decorated, they
         * are validated.
         * 
         * @param list  the list to decorate, must not be null
         * @param predicate  the predicate to use for validation, must not be null
         * @throws IllegalArgumentException if list or predicate is null
         * @throws IllegalArgumentException if the list contains invalid elements
         */
        public static java.util.List<Object> decorate(java.util.List<Object> list, Predicate predicate)
        {
            return new PredicatedList(list, predicate);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * <p>
         * If there are any elements already in the list being decorated, they
         * are validated.
         * 
         * @param list  the list to decorate, must not be null
         * @param predicate  the predicate to use for validation, must not be null
         * @throws IllegalArgumentException if list or predicate is null
         * @throws IllegalArgumentException if the list contains invalid elements
         */
        protected internal PredicatedList(java.util.List<Object> list, Predicate predicate)
            : base(list, predicate)
        {
        }

        /**
         * Gets the list being decorated.
         * 
         * @return the decorated list
         */
        protected java.util.List<Object> getList()
        {
            return (java.util.List<Object>)getCollection();
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
            validate(obj);
            getList().add(index, obj);
        }

        public bool addAll(int index, java.util.Collection<Object> coll)
        {
            for (java.util.Iterator<Object> it = coll.iterator(); it.hasNext(); )
            {
                validate(it.next());
            }
            return getList().addAll(index, coll);
        }

        public java.util.ListIterator<Object> listIterator()
        {
            return listIterator(0);
        }

        public java.util.ListIterator<Object> listIterator(int i)
        {
            return new PredicatedListIterator(getList().listIterator(i),this);
        }

        public Object set(int index, Object obj)
        {
            validate(obj);
            return getList().set(index, obj);
        }

        public java.util.List<Object> subList(int fromIndex, int toIndex)
        {
            java.util.List<Object> sub = getList().subList(fromIndex, toIndex);
            return new PredicatedList(sub, predicate);
        }
    }

    /**
     * Inner class Iterator for the PredicatedList
     */
    public class PredicatedListIterator : AbstractListIteratorDecorator
    {

        protected readonly PredicatedList root;

        protected internal PredicatedListIterator(java.util.ListIterator<Object> iterator, PredicatedList root)
            : base(iterator)
        {
            this.root = root;
        }

        public override void add(Object obj)
        {
            root.validate(obj);
            iterator.add(obj);
        }

        public override void set(Object obj)
        {
            root.validate(obj);
            iterator.set(obj);
        }
    }
}