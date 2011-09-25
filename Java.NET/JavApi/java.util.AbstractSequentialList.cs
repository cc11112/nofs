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
using System.Collections;
using System.Collections.Generic;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util
{

    /**
     * AbstractSequentialList is an abstract implementation of the List interface.
     * This implementation does not support adding. A subclass must implement the
     * abstract method listIterator().
     * 
     * @since 1.2
     */
    public abstract class AbstractSequentialList<E> : AbstractList<E>, IEnumerable {

        /**
         * Constructs a new instance of this AbstractSequentialList.
         */
        protected AbstractSequentialList() : base () {
        }

        public override void add(int location, E obj) {
            listIterator(location).add(obj);
        }

        public override bool addAll(int location, Collection<E> collection) {
            ListIterator<E> it = listIterator(location);
            Iterator<E> colIt = collection.iterator();
            int next = it.nextIndex();
            while (colIt.hasNext()) {
                it.add(colIt.next());
            }
            return next != it.nextIndex();
        }

        public override E get(int location) {
            try {
                return listIterator(location).next();
            } catch (NoSuchElementException e) {
                throw new java.lang.IndexOutOfBoundsException();
            }
        }

        public override Iterator<E> iterator() {
            return listIterator(0);
        }

        //public abstract ListIterator<E> listIterator(int location);

        public override E remove(int location) {
            try {
                ListIterator<E> it = listIterator(location);
                E result = it.next();
                it.remove();
                return result;
            } catch (NoSuchElementException e) {
                throw new java.lang.IndexOutOfBoundsException();
            }
        }

        
        public override E set(int location, E obj) {
            ListIterator<E> it = listIterator(location);
            if (!it.hasNext()) {
                throw new java.lang.IndexOutOfBoundsException();
            }
            E result = it.next();
            it.set(obj);
            return result;
        }

        /// <summary>
        /// Get the IEnumerator instance.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        /// <summary>
        /// Get the IEnemurator instance for this wrapped java.util.Iterator.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<E> GetEnumerator()
        {
            java.util.Iterator<E> it = this.iterator();
            while (it.hasNext())
            {
                yield return it.next();
            }
        }
    }
}
