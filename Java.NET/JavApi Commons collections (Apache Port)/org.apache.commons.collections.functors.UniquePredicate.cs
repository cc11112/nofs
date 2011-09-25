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
     * Predicate implementation that returns true the first time an object is
     * passed into the predicate.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Stephen Colebourne
     */
    public sealed class UniquePredicate : Predicate, java.io.Serializable
    {

        /** Serial version UID */
        private static readonly long serialVersionUID = -3319417438027438040L;

        /** The set of previously seen objects */
        private readonly java.util.Set<Object> iSet = new java.util.HashSet<Object>();

        /**
         * Factory to create the predicate.
         * 
         * @return the predicate
         * @throws IllegalArgumentException if the predicate is null
         */
        public static Predicate getInstance()
        {
            return new UniquePredicate();
        }

        /**
         * Constructor that performs no validation.
         * Use <code>getInstance</code> if you want that.
         */
        public UniquePredicate()
            : base()
        {
        }

        /**
         * Evaluates the predicate returning true if the input object hasn't been
         * received yet.
         * 
         * @param object  the input object
         * @return true if this is the first time the object is seen
         */
        public bool evaluate(Object obj)
        {
            return iSet.add(obj);
        }

    }
}