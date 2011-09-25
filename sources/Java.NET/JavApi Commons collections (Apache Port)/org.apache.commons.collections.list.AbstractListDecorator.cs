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
using org.apache.commons.collections.collection;

namespace org.apache.commons.collections.list
{

    /**
     * Decorates another <code>List</code> to provide additional behaviour.
     * <p>
     * Methods are forwarded directly to the decorated list.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    public abstract class AbstractListDecorator : AbstractCollectionDecorator, java.util.List<Object>
    {

        /**
         * Constructor only used in deserialization, do not use otherwise.
         * @since Commons Collections 3.1
         */
        protected AbstractListDecorator()
            : base()
        {

        }

        /**
         * Constructor that wraps (not copies).
         * 
         * @param list  the list to decorate, must not be null
         * @throws IllegalArgumentException if list is null
         */
        protected AbstractListDecorator(java.util.List<Object> list)
            : base(list)
        {

        }

        /**
         * Gets the list being decorated.
         * 
         * @return the decorated list
         */
        protected virtual java.util.List<Object> getList()
        {
            return (java.util.List<Object>)getCollection();
        }

        //-----------------------------------------------------------------------
        public virtual void add(int index, Object obj)
        {
            getList().add(index, obj);
        }

        public virtual bool addAll(int index, java.util.Collection<Object> coll)
        {
            return getList().addAll(index, coll);
        }

        public virtual Object get(int index)
        {
            return getList().get(index);
        }

        public virtual int indexOf(Object obj)
        {
            return getList().indexOf(obj);
        }

        public virtual int lastIndexOf(Object obj)
        {
            return getList().lastIndexOf(obj);
        }

        public virtual java.util.ListIterator<Object> listIterator()
        {
            return getList().listIterator();
        }

        public virtual java.util.ListIterator<Object> listIterator(int index)
        {
            return getList().listIterator(index);
        }

        public virtual Object remove(int index)
        {
            return getList().remove(index);
        }

        public virtual Object set(int index, Object obj)
        {
            return getList().set(index, obj);
        }

        public virtual java.util.List<Object> subList(int fromIndex, int toIndex)
        {
            return getList().subList(fromIndex, toIndex);
        }

    }
}