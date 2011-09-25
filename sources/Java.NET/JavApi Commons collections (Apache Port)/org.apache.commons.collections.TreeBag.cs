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

namespace org.apache.commons.collections
{

    /**
     * A {@link Bag} that is backed by a {@link TreeMap}. 
     * Order will be maintained among the unique representative
     * members.
     *
     * @deprecated Moved to bag subpackage and rewritten internally. Due to be removed in v4.0.
     * @since Commons Collections 2.0
     * @version $Revision$ $Date$
     * 
     * @author Chuck Burdick
     */
    [Obsolete]
    public class TreeBag : DefaultMapBag, SortedBag
    {

        /**
         * Constructs an empty <code>TreeBag</code>.
         */
        public TreeBag()
            : base(new java.util.TreeMap<Object, Object>())
        {
        }

        /**
         * Constructs an empty {@link Bag} that maintains order on its unique
         * representative members according to the given {@link Comparator}.
         * 
         * @param comparator  the comparator to use
         */
        public TreeBag(java.util.Comparator<Object> comparator)
            : base(new java.util.TreeMap<Object, Object>(comparator))
        {
        }

        /**
         * Constructs a {@link Bag} containing all the members of the given
         * collection.
         * 
         * @param coll  the collection to copy into the bag
         */
        public TreeBag(java.util.Collection<Object> coll)
            : this()
        {
            addAll(coll);
        }

        public Object first()
        {
            return ((java.util.SortedMap<Object, Object>)getMap()).firstKey();
        }

        public Object last()
        {
            return ((java.util.SortedMap<Object, Object>)getMap()).lastKey();
        }

        public java.util.Comparator<Object> comparator()
        {
            return ((java.util.SortedMap<Object, Object>)getMap()).comparator();
        }

    }
}