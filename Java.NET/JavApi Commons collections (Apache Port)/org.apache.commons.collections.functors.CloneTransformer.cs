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
     * Transformer implementation that returns a clone of the input object.
     * <p>
     * Clone is performed using <code>PrototypeFactory.getInstance(input).create()</code>.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Stephen Colebourne
     */
    public class CloneTransformer : Transformer, java.io.Serializable
    {

        /** Serial version UID */
        private static readonly long serialVersionUID = -8188742709499652567L;

        /** Singleton predicate instance */
        public static readonly Transformer INSTANCE = new CloneTransformer();

        /**
         * Factory returning the singleton instance.
         * 
         * @return the singleton instance
         * @since Commons Collections 3.1
         */
        public static Transformer getInstance()
        {
            return INSTANCE;
        }

        /**
         * Constructor
         */
        private CloneTransformer()
            : base()
        {
        }

        /**
         * Transforms the input to result by cloning it.
         * 
         * @param input  the input object to transform
         * @return the transformed result
         */
        public Object transform(Object input)
        {
            if (input == null)
            {
                return null;
            }
            return PrototypeFactory.getInstance(input).create();
        }

    }
}