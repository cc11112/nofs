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
     * Decorates another <code>Set</code> to provide additional behaviour.
     * <p>
     * Methods are forwarded directly to the decorated set.
     *
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    public abstract class AbstractSetDecorator : AbstractCollectionDecorator, java.util.Set<Object>
    {

        /**
         * Constructor only used in deserialization, do not use otherwise.
         * @since Commons Collections 3.1
         */
        protected AbstractSetDecorator()
            : base()
        {
        }

        /**
         * Constructor that wraps (not copies).
         * 
         * @param set  the set to decorate, must not be null
         * @throws IllegalArgumentException if set is null
         */
        protected internal AbstractSetDecorator(java.util.Set<Object> set)
            : base(set)
        {
        }

        /**
         * Gets the set being decorated.
         * 
         * @return the decorated set
         */
        protected java.util.Set<Object> getSet()
        {
            return (java.util.Set<Object>)getCollection();
        }

        protected override java.util.Collection<object> getCollection()
        {
            return base.getCollection();
        }

    }
}
