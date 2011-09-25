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
     * Predicate implementation that returns the result of a transformer.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Stephen Colebourne
     */
    [Serializable]
    public sealed class TransformerPredicate : Predicate, java.io.Serializable
    {

        /** Serial version UID */
        private static readonly long serialVersionUID = -2407966402920578741L;

        /** The transformer to call */
        private readonly Transformer iTransformer;

        /**
         * Factory to create the predicate.
         * 
         * @param transformer  the transformer to decorate
         * @return the predicate
         * @throws IllegalArgumentException if the transformer is null
         */
        public static Predicate getInstance(Transformer transformer)
        {
            if (transformer == null)
            {
                throw new java.lang.IllegalArgumentException("The transformer to call must not be null");
            }
            return new TransformerPredicate(transformer);
        }

        /**
         * Constructor that performs no validation.
         * Use <code>getInstance</code> if you want that.
         * 
         * @param transformer  the transformer to decorate
         */
        public TransformerPredicate(Transformer transformer)
            : base()
        {
            iTransformer = transformer;
        }

        /**
         * Evaluates the predicate returning the result of the decorated transformer.
         * 
         * @param object  the input object
         * @return true if decorated transformer returns Boolean.TRUE
         * @throws FunctorException if the transformer returns an invalid type
         */
        public bool evaluate(Object obj)
        {
            Object result = iTransformer.transform(obj);
            if (!(result is java.lang.Boolean))
            {
                throw new FunctorException(
                    "Transformer must return an is java.lang.Boolean, it was a "
                        + (result == null ? "null object" : result.GetType().Name));
            }
            return ((java.lang.Boolean)result).booleanValue();
        }

        /**
         * Gets the transformer.
         * 
         * @return the transformer
         * @since Commons Collections 3.1
         */
        public Transformer getTransformer()
        {
            return iTransformer;
        }

    }
}