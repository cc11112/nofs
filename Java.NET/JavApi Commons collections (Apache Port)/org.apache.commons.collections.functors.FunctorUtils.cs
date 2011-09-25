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
     * Internal utilities for functors.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Stephen Colebourne
     * @author Matt Benson
     */
    internal class FunctorUtils
    {

        /**
         * Restricted constructor.
         */
        private FunctorUtils()
            : base()
        {
        }

        /**
         * Clone the predicates to ensure that the internal reference can't be messed with.
         * 
         * @param predicates  the predicates to copy
         * @return the cloned predicates
         */
        internal static Predicate[] copy(Predicate[] predicates)
        {
            if (predicates == null)
            {
                return null;
            }
            return (Predicate[])predicates.clone();
        }

        /**
         * Validate the predicates to ensure that all is well.
         * 
         * @param predicates  the predicates to validate
         */
        internal static void validate(Predicate[] predicates)
        {
            if (predicates == null)
            {
                throw new java.lang.IllegalArgumentException("The predicate array must not be null");
            }
            for (int i = 0; i < predicates.Length; i++)
            {
                if (predicates[i] == null)
                {
                    throw new java.lang.IllegalArgumentException("The predicate array must not contain a null predicate, index " + i + " was null");
                }
            }
        }

        /**
         * Validate the predicates to ensure that all is well.
         * 
         * @param predicates  the predicates to validate
         * @return predicate array
         */
        internal static Predicate[] validate(java.util.Collection<Predicate> predicates)
        {
            if (predicates == null)
            {
                throw new java.lang.IllegalArgumentException("The predicate collection must not be null");
            }
            // convert to array like this to guarantee iterator() ordering
            Predicate[] preds = new Predicate[predicates.size()];
            int i = 0;
            for (java.util.Iterator<Predicate> it = predicates.iterator(); it.hasNext(); )
            {
                preds[i] = it.next();
                if (preds[i] == null)
                {
                    throw new java.lang.IllegalArgumentException("The predicate collection must not contain a null predicate, index " + i + " was null");
                }
                i++;
            }
            return preds;
        }

        /**
         * Clone the closures to ensure that the internal reference can't be messed with.
         * 
         * @param closures  the closures to copy
         * @return the cloned closures
         */
        internal static Closure[] copy(Closure[] closures)
        {
            if (closures == null)
            {
                return null;
            }
            return (Closure[])closures.clone();
        }

        /**
         * Validate the closures to ensure that all is well.
         * 
         * @param closures  the closures to validate
         */
        internal static void validate(Closure[] closures)
        {
            if (closures == null)
            {
                throw new java.lang.IllegalArgumentException("The closure array must not be null");
            }
            for (int i = 0; i < closures.Length; i++)
            {
                if (closures[i] == null)
                {
                    throw new java.lang.IllegalArgumentException("The closure array must not contain a null closure, index " + i + " was null");
                }
            }
        }

        /**
         * Copy method
         * 
         * @param transformers  the transformers to copy
         * @return a clone of the transformers
         */
        internal static Transformer[] copy(Transformer[] transformers)
        {
            if (transformers == null)
            {
                return null;
            }
            return (Transformer[])transformers.clone();
        }

        /**
         * Validate method
         * 
         * @param transformers  the transformers to validate
         */
        internal static void validate(Transformer[] transformers)
        {
            if (transformers == null)
            {
                throw new java.lang.IllegalArgumentException("The transformer array must not be null");
            }
            for (int i = 0; i < transformers.Length; i++)
            {
                if (transformers[i] == null)
                {
                    throw new java.lang.IllegalArgumentException(
                        "The transformer array must not contain a null transformer, index " + i + " was null");
                }
            }
        }

    }
}