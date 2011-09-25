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
     * Predicate implementation that returns true if the input is null.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Stephen Colebourne
     */
    [Serializable]
    public sealed class NullPredicate : Predicate, java.io.Serializable
    {

        /** Serial version UID */
        private static readonly long serialVersionUID = 7533784454832764388L;

        /** Singleton predicate instance */
        public static readonly Predicate INSTANCE = new NullPredicate();

        /**
         * Factory returning the singleton instance.
         * 
         * @return the singleton instance
         * @since Commons Collections 3.1
         */
        public static Predicate getInstance()
        {
            return INSTANCE;
        }

        /**
         * Restricted constructor.
         */
        private NullPredicate()
            : base()
        {
        }

        /**
         * Evaluates the predicate returning true if the input is null.
         * 
         * @param object  the input object
         * @return true if input is null
         */
        public bool evaluate(Object obj)
        {
            return (obj == null);
        }

    }
}