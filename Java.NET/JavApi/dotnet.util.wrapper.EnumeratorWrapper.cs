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
using System.Collections.Generic;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.dotnet.util.wrapper
{
    /// <summary>
    /// This class take a .net IEnumerator instance inside and use this to delegate the
    /// java.util.Enumeration method calls.
    /// </summary>
    /// <typeparam name="K">Type to enumerate</typeparam>
    public class EnumeratorWrapper<K> : java.util.Enumeration<K>
    {
        private readonly IEnumerator<K> delegateInstance;
        private bool nextElementIsReaded = false;
        public EnumeratorWrapper(IEnumerator<K> enumerator)
        {
            this.delegateInstance = enumerator;
        }
        public EnumeratorWrapper(IEnumerable<K> enumerable)
        {
            this.delegateInstance = enumerable.GetEnumerator();
        }
        public bool hasMoreElements()
        {
            lock (delegateInstance)
            {
                if (nextElementIsReaded)
                {
                    nextElementIsReaded = this.delegateInstance.MoveNext();
                    return nextElementIsReaded;
                }
                return true;
            }
        }
        public K nextElement()
        {
            lock (delegateInstance)
            {
                if (nextElementIsReaded)
                {
                    nextElementIsReaded = false;
                    return delegateInstance.Current;
                }
                else
                {
                    nextElementIsReaded = this.delegateInstance.MoveNext();
                    if (!nextElementIsReaded)
                    {
                        throw new java.util.NoSuchElementException();
                    }
                    else
                    {
                        return delegateInstance.Current;
                    }
                }
            }
        }

    }
}
