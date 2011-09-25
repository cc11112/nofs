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
     * Implements <code>Bag</code>, using a <code>HashMap</code> to provide the
     * data storage. This is the standard implementation of a bag.
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
    public class HashBag
            : AbstractMapBag, Bag, java.io.Serializable
    {

        /** Serial version lock */
        private static readonly long serialVersionUID = -6561115435802554013L;

        /**
         * Constructs an empty <code>HashBag</code>.
         */
        public HashBag()
            : base(new java.util.HashMap<Object, Object>())
        {
        }

        /**
         * Constructs a bag containing all the members of the given collection.
         * 
         * @param coll  a collection to copy into this bag
         */
        public HashBag(java.util.Collection<Object> coll)
            : this()
        {
            addAll(coll);
        }

        //-----------------------------------------------------------------------
        /**
         * Write the bag out using a custom routine.
         */
        private void writeObject(java.io.ObjectOutputStream outJ)
        {//throws IOException {
            outJ.defaultWriteObject();
            base.doWriteObject(outJ);
        }

        /**
         * Read the bag in using a custom routine.
         */
        private void readObject(java.io.ObjectInputStream inJ)
        {//throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            base.doReadObject(new java.util.HashMap<Object, Object>(), inJ);
        }

    }
}