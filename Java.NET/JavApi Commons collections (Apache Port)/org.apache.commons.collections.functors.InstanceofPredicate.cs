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

namespace org.apache.commons.collections.functors
{

    /**
     * Predicate implementation that returns true if the input is an instanceof
     * the type stored in this predicate.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Stephen Colebourne
     */
    [Serializable]
    public sealed class InstanceofPredicate : Predicate, java.io.Serializable
    {

        /** Serial version UID */
        private static readonly long serialVersionUID = -6682656911025165584L;

        /** The type to compare to */
        private readonly Type iType;

        /**
         * Factory to create the identity predicate.
         * 
         * @param type  the type to check for, may not be null
         * @return the predicate
         * @throws IllegalArgumentException if the class is null
         */
        internal static Predicate getInstance(Type type)
        {
            if (type == null)
            {
                throw new java.lang.IllegalArgumentException("The type to check is must not be null");
            }
            return new InstanceofPredicate(type);
        }

        /**
         * Factory to create the identity predicate.
         * 
         * @param type  the type to check for, may not be null
         * @return the predicate
         * @throws IllegalArgumentException if the class is null
         */
        public static Predicate getInstance(java.lang.Class type)
        {
            return getInstance(type.getDelegateInstance());
        }

        /**
         * Constructor that performs no validation.
         * Use <code>getInstance</code> if you want that.
         * 
         * @param type  the type to check for
         */
        public InstanceofPredicate(Type type)
        {
            iType = type;
        }

        /**
         * Evaluates the predicate returning true if the input object is of the correct type.
         * 
         * @param object  the input object
         * @return true if input is of stored type
         */
        public bool evaluate(Object obj)
        {
            return (iType.IsInstanceOfType(obj));
        }

        /**
         * Gets the type to compare to.
         * 
         * @return the type
         * @since Commons Collections 3.1
         */
        public Type getType()
        {
            return iType;
        }

    }
}