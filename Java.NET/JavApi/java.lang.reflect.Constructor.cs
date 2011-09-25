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
using System.Reflection;

using java = biz.ritter.javapi;

namespace biz.ritter.javapi.lang.reflect
{
    public class Constructor
    {
        /// <summary>
        /// .net reflection type to delegate methods
        /// </summary>
        private ConstructorInfo delegateInstance;
        /// <summary>
        /// Creates a new constructor instance with .net reflection type.
        /// </summary>
        /// <param name="ci"></param>
        internal Constructor(ConstructorInfo ci)
        {
            this.delegateInstance = ci;
        }

        /// <summary>
        /// Create a new instance by calling representing constructor with given arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Object newInstance(Object[] args)
        {
            return this.delegateInstance.Invoke(args);
        }
    }
}
