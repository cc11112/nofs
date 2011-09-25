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

namespace org.apache.commons.collections.iterators
{

    /** 
     * Provides an implementation of an empty ordered map iterator.
     *
     * @since Commons Collections 3.1
     * @version $Revision$ $Date$
     * 
     * @author Stephen Colebourne
     */
    public class EmptyOrderedMapIterator : AbstractEmptyIterator, OrderedMapIterator, ResettableIterator
    {

        /**
         * Singleton instance of the iterator.
         * @since Commons Collections 3.1
         */
        public static readonly OrderedMapIterator INSTANCE = new EmptyOrderedMapIterator();

        /**
         * Constructor.
         */
        protected EmptyOrderedMapIterator()
            : base()
        {
        }

    }
}