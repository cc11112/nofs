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
     * Predicate implementation that returns true if the input is the same object
     * as the one stored in this predicate.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Stephen Colebourne
     */
    [Serializable]
    public sealed class IdentityPredicate : Predicate, java.io.Serializable
    {

        /** Serial version UID */
        private static readonly long serialVersionUID = -89901658494523293L;


        /** The value to compare to */
        private readonly Object iValue;

        /**
         * Factory to create the identity predicate.
         * 
         * @param object  the object to compare to
         * @return the predicate
         * @throws IllegalArgumentException if the predicate is null
         */
        public static Predicate getInstance(Object obj)
        {
            if (obj == null)
            {
                return NullPredicate.INSTANCE;
            }
            return new IdentityPredicate(obj);
        }

        /**
         * Constructor that performs no validation.
         * Use <code>getInstance</code> if you want that.
         * 
         * @param object  the object to compare to
         */
        public IdentityPredicate(Object obj)
            : base()
        {
            iValue = obj;
        }

        /**
         * Evaluates the predicate returning true if the input object is identical to
         * the stored object.
         * 
         * @param object  the input object
         * @return true if input is the same object as the stored value
         */
        public bool evaluate(Object obj)
        {
            return (iValue == obj);
        }

        /**
         * Gets the value.
         * 
         * @return the value
         * @since Commons Collections 3.1
         */
        public Object getValue()
        {
            return iValue;
        }

    }
}