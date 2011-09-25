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

namespace biz.ritter.javapi.util
{

    /**
     * An AbstractSet is an abstract implementation of the Set interface. This
     * implementation does not support adding. A subclass must implement the
     * abstract methods iterator() and size().
     * 
     * @since 1.2
     */
    public abstract class AbstractSet<E> : AbstractCollection<E>, Set<E> {

        /**
         * Constructs a new instance of this AbstractSet.
         */
        protected AbstractSet() : base (){
        }

        /**
         * Compares the specified object to this Set and returns true if they are
         * equal. The object must be an instance of Set and contain the same
         * objects.
         * 
         * @param object
         *            the object to compare with this set.
         * @return {@code true} if the specified object is equal to this set,
         *         {@code false} otherwise
         * @see #hashCode
         */
        public override bool Equals(Object obj) {
            if (this == obj) {
                return true;
            }
            if (obj is Set<E>) {
                Set<E> s = (Set<E>) obj;

                try {
                    return size() == s.size() && containsAll(s);
                } catch (java.lang.NullPointerException ignored) {
                    return false;
                } catch (java.lang.ClassCastException ignored) {
                    return false;
                }
            }
            return false;
        }

        /**
         * Returns the hash code for this set. Two set which are equal must return
         * the same value. This implementation calculates the hash code by adding
         * each element's hash code.
         * 
         * @return the hash code of this set.
         * @see #equals
         */
        public override int GetHashCode() {
            int result = 0;
            Iterator<E> it = iterator();
            while (it.hasNext()) {
                Object next = it.next();
                result += next == null ? 0 : next.GetHashCode();
            }
            return result;
        }

        /**
         * Removes all occurrences in this collection which are contained in the
         * specified collection.
         * 
         * @param collection
         *            the collection of objects to remove.
         * @return {@code true} if this collection was modified, {@code false}
         *         otherwise.
         * @throws UnsupportedOperationException
         *                if removing from this collection is not supported.
         */
        public override bool removeAll(Collection<E> collection) {
            bool result = false;
            if (size() <= collection.size()) {
                Iterator<E> it = iterator();
                while (it.hasNext()) {
                    if (collection.contains(it.next())) {
                        it.remove();
                        result = true;
                    }
                }
            } else {
                Iterator<E> it = collection.iterator();
                while (it.hasNext()) {
                    result = remove(it.next()) || result;
                }
            }
            return result;
        }
        /**
         * Searches this set for all objects in the specified collection.
         * 
         * @param collection
         *            the collection of objects.
         * @return {@code true} if all objects in the specified collection are
         *         elements of this set, {@code false} otherwise.
         */
        public override bool containsAll(Collection<E> collection)
        {
            Iterator<E> it = collection.iterator();
            while (it.hasNext())
            {
                if (!this.contains(it.next())) return false;
            }
            return true;
        }

        /**
         * Removes all objects from this set that are not contained in the specified
         * collection.
         * 
         * @param collection
         *            the collection of objects to retain.
         * @return {@code true} if this set was modified, {@code false} otherwise.
         * @throws UnsupportedOperationException
         *             when removing from this set is not supported.
         */
        public override bool retainAll(Collection<E> collection)
        {
            bool modified = false;
            Iterator<E> it = this.iterator();
            while (it.hasNext())
            {
                Object next =it.next();
                if (!collection.contains(next))
                {
                    this.remove(next);
                    modified = true;
                }
            }
            return modified;
        }
    }
}
