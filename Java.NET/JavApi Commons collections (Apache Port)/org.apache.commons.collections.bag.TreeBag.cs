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

namespace org.apache.commons.collections.bag
{

    /**
     * Implements <code>SortedBag</code>, using a <code>TreeMap</code> to provide
     * the data storage. This is the standard implementation of a sorted bag.
     * <p>
     * Order will be maintained among the bag members and can be viewed through the
     * iterator.
     * <p>
     * A <code>Bag</code> stores each object in the collection together with a
     * count of occurrences. Extra methods on the interface allow multiple copies
     * of an object to be added or removed at once. It is important to read the
     * interface javadoc carefully as several methods violate the
     * <code>Collection</code> interface specification.
     *
     * @since Commons Collections 3.0 (previously in main package v2.0)
     * @version $Revision$ $Date$
     * 
     * @author Chuck Burdick
     * @author Stephen Colebourne
     */
    [Serializable]
    public class TreeBag
            : AbstractMapBag, SortedBag, java.io.Serializable
    {

        /** Serial version lock */
        private static readonly long serialVersionUID = -7740146511091606676L;

        /**
         * Constructs an empty <code>TreeBag</code>.
         */
        public TreeBag()
            : base(new java.util.TreeMap<Object, Object>())
        {
        }

        /**
         * Constructs an empty bag that maintains order on its unique
         * representative members according to the given {@link Comparator}.
         * 
         * @param comparator  the comparator to use
         */
        public TreeBag(java.util.Comparator<Object> comparator)
            : base(new java.util.TreeMap<Object, Object>(comparator))
        {
        }

        /**
         * Constructs a <code>TreeBag</code> containing all the members of the
         * specified collection.
         * 
         * @param coll  the collection to copy into the bag
         */
        public TreeBag(java.util.Collection<Object> coll)
            : this()
        {
            addAll(coll);
        }

        //-----------------------------------------------------------------------
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

        //-----------------------------------------------------------------------
        /**
         * Write the bag out using a custom routine.
         */
        private void writeObject(java.io.ObjectOutputStream outJ)
        {//throws IOException {
            outJ.defaultWriteObject();
            outJ.writeObject(comparator());
            base.doWriteObject(outJ);
        }

        /**
         * Read the bag in using a custom routine.
         */
        private void readObject(java.io.ObjectInputStream inJ)
        {//throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            java.util.Comparator<Object> comp = (java.util.Comparator<Object>)inJ.readObject();
            base.doReadObject(new java.util.TreeMap<Object, Object>(comp), inJ);
        }

    }
}