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
     * Predicate implementation that returns true if both the predicates return true.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Stephen Colebourne
     */
    [Serializable]
    public sealed class AndPredicate : Predicate, PredicateDecorator, java.io.Serializable
    {

        /** Serial version UID */
        private static readonly long serialVersionUID = 4189014213763186912L;

        /** The array of predicates to call */
        private readonly Predicate iPredicate1;
        /** The array of predicates to call */
        private readonly Predicate iPredicate2;

        /**
         * Factory to create the predicate.
         * 
         * @param predicate1  the first predicate to check, not null
         * @param predicate2  the second predicate to check, not null
         * @return the <code>and</code> predicate
         * @throws IllegalArgumentException if either predicate is null
         */
        public static Predicate getInstance(Predicate predicate1, Predicate predicate2)
        {
            if (predicate1 == null || predicate2 == null)
            {
                throw new java.lang.IllegalArgumentException("Predicate must not be null");
            }
            return new AndPredicate(predicate1, predicate2);
        }

        /**
         * Constructor that performs no validation.
         * Use <code>getInstance</code> if you want that.
         * 
         * @param predicate1  the first predicate to check, not null
         * @param predicate2  the second predicate to check, not null
         */
        public AndPredicate(Predicate predicate1, Predicate predicate2)
            : base()
        {
            iPredicate1 = predicate1;
            iPredicate2 = predicate2;
        }

        /**
         * Evaluates the predicate returning true if both predicates return true.
         * 
         * @param object  the input object
         * @return true if both decorated predicates return true
         */
        public bool evaluate(Object obj)
        {
            return (iPredicate1.evaluate(obj) && iPredicate2.evaluate(obj));
        }

        /**
         * Gets the two predicates being decorated as an array.
         * 
         * @return the predicates
         * @since Commons Collections 3.1
         */
        public Predicate[] getPredicates()
        {
            return new Predicate[] { iPredicate1, iPredicate2 };
        }

    }
}