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

namespace org.apache.commons.collections.set
{

    /**
     * Decorates another <code>Set</code> to synchronize its behaviour for a
     * multi-threaded environment.
     * <p>
     * Methods are synchronized, then forwarded to the decorated set.
     * <p>
     * This class is java.io.Serializable from Commons Collections 3.1.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    [Serializable]
    public class SynchronizedSet : SynchronizedCollection, java.util.Set<Object>
    {

        /** Serialization version */
        private static readonly long serialVersionUID = -8304417378626543635L;

        /**
         * Factory method to create a synchronized set.
         * 
         * @param set  the set to decorate, must not be null
         * @throws IllegalArgumentException if set is null
         */
        public static java.util.Set<Object> decorate(java.util.Set<Object> set)
        {
            return new SynchronizedSet(set);
        }

        //-----------------------------------------------------------------------
        /**
         * Constructor that wraps (not copies).
         * 
         * @param set  the set to decorate, must not be null
         * @throws IllegalArgumentException if set is null
         */
        protected SynchronizedSet(java.util.Set<Object> set)
            : base(set)
        {
        }

        /**
         * Constructor that wraps (not copies).
         * 
         * @param set  the set to decorate, must not be null
         * @param lock  the lock object to use, must not be null
         * @throws IllegalArgumentException if set is null
         */
        protected SynchronizedSet(java.util.Set<Object> set, Object lockJ)
            : base(set, lockJ)
        {
        }

        /**
         * Gets the decorated set.
         * 
         * @return the decorated set
         */
        protected java.util.Set<Object> getSet()
        {
            return (java.util.Set<Object>)collection;
        }

    }
}