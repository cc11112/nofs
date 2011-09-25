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
using System.Text;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util
{
    /**
     * {@code Stack} is a Last-In/First-Out(LIFO) data structure which represents a
     * stack of objects. It enables users to pop to and push from the stack,
     * including null objects. There is no limit to the size of the stack.
     */
    [Serializable]
    public class Stack<E> : Vector<E> {

        private static readonly long serialVersionUID = 1224463164541339165L;

        /**
         * Constructs a stack with the default size of {@code Vector}.
         */
        public Stack() :base() {
        }

        /**
         * Returns whether the stack is empty or not.
         * 
         * @return {@code true} if the stack is empty, {@code false} otherwise.
         */
        public bool empty() {
            return isEmpty();
        }

        /**
         * Returns the element at the top of the stack without removing it.
         * 
         * @return the element at the top of the stack.
         * @throws EmptyStackException
         *             if the stack is empty.
         * @see #pop
         */
        public virtual E peek() {
            lock (this) {
                try {
                    return (E) elementData[elementCount - 1];
                } catch (java.lang.IndexOutOfBoundsException e) {
                    throw new EmptyStackException();
                }
            }
        }

        /**
         * Returns the element at the top of the stack and removes it.
         * 
         * @return the element at the top of the stack.
         * @throws EmptyStackException
         *             if the stack is empty.
         * @see #peek
         * @see #push
         */
        public virtual E pop() {
            lock (this) {
                if (elementCount == 0) {
                    throw new EmptyStackException();
                }
                int index = --elementCount;
                E obj = (E) elementData[index];
                elementData[index] = default(E);
                modCount++;
                return obj;
            }
        }

        /**
         * Pushes the specified object onto the top of the stack.
         * 
         * @param object
         *            The object to be added on top of the stack.
         * @return the object argument.
         * @see #peek
         * @see #pop
         */
        public E push(E obj) {
            addElement(obj);
            return obj;
        }

        /**
         * Returns the index of the first occurrence of the object, starting from
         * the top of the stack.
         * 
         * @return the index of the first occurrence of the object, assuming that
         *         the topmost object on the stack has a distance of one.
         * @param o
         *            the object to be searched.
         */
        public int search(Object o) {
            lock (this) {
                E[] dumpArray = elementData;
                int size = elementCount;
                if (o != null) {
                    for (int i = size - 1; i >= 0; i--) {
                        if (o.equals(dumpArray[i])) {
                            return size - i;
                        }
                    }
                } else {
                    for (int i = size - 1; i >= 0; i--) {
                        if (dumpArray[i] == null) {
                            return size - i;
                        }
                    }
                }
                return -1;
            }
        }
    }
}
