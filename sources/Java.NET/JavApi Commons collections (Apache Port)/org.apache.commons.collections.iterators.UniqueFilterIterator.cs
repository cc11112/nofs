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
using org.apache.commons.collections.functors;

namespace org.apache.commons.collections.iterators
{

    /** 
     * A FilterIterator which only returns "unique" Objects.  Internally,
     * the Iterator maintains a Set of objects it has already encountered,
     * and duplicate Objects are skipped.
     *
     * @since Commons Collections 2.1
     * @version $Revision$ $Date$
     * 
     * @author Morgan Delagrange
     */
    public class UniqueFilterIterator : FilterIterator
    {

        //-------------------------------------------------------------------------

        /**
         *  Constructs a new <code>UniqueFilterIterator</code>.
         *
         *  @param iterator  the iterator to use
         */
        public UniqueFilterIterator(java.util.Iterator<Object> iterator) : base(iterator, UniquePredicate.getInstance()) { }


    }
}