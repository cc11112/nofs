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
 *  Copyright © 2011 Sebastian Ritter
 */
using System;
using System.Collections.Generic;
using System.Collections;

using java = biz.ritter.javapi;

namespace biz.ritter.javapi.dotnet.util.wrapper
{
    /// <summary>
    /// This class wrapps the IEnumerator / IEnumerable / Enumeration above an Iterator instance. So foreach loop
    /// and default .net functionality can be used.
    /// </summary>
    public sealed class EnumerableIterator<E> : IEnumerable<E>, java.util.Enumeration<E>
    {
        private java.lang.Iterable<E> iterableInstance;
        /// <summary>
        /// Create new instance for given Iterable object
        /// </summary>
        /// <param name="it"></param>
        public EnumerableIterator (java.lang.Iterable<E> it) {
            this.iterableInstance = it;
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
        public IEnumerator<E> GetEnumerator()
        {
            java.util.Iterator<E> it = this.iterableInstance.iterator();
            while (it.hasNext())
            {
                yield return it.next();
            }
        }
        public bool hasMoreElements()
        {
            return this.iterableInstance.iterator().hasNext();
        }

        public E nextElement()
        {
            return this.iterableInstance.iterator().next();
        }
    }
}
