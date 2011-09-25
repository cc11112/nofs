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

namespace org.apache.commons.collections.bidimap
{

    /**
     * Implementation of <code>BidiMap</code> that uses two <code>HashMap</code> instances.
     * <p>
     * Two <code>HashMap</code> instances are used in this class.
     * This provides fast lookups at the expense of storing two sets of map entries.
     * Commons Collections would welcome the addition of a direct hash-based
     * implementation of the <code>BidiMap</code> interface.
     * <p>
     * NOTE: From Commons Collections 3.1, all subclasses will use <code>HashMap</code>
     * and the flawed <code>createMap</code> method is ignored.
     * 
     * @since Commons Collections 3.0
     * @version $Id$
     * 
     * @author Matthew Hawthorne
     * @author Stephen Colebourne
     */
    [Serializable]
    public class DualHashBidiMap
            : AbstractDualBidiMap, java.io.Serializable
    {

        /** Ensure serialization compatibility */
        private static readonly long serialVersionUID = 721969328361808L;

        /**
         * Creates an empty <code>HashBidiMap</code>.
         */
        public DualHashBidiMap() :
            base(new java.util.HashMap<Object, Object>(), new java.util.HashMap<Object, Object>())
        {
        }

        /** 
         * Constructs a <code>HashBidiMap</code> and copies the mappings from
         * specified <code>Map</code>.  
         *
         * @param map  the map whose mappings are to be placed in this map
         */
        public DualHashBidiMap(java.util.Map<Object, Object> map) :
            base(new java.util.HashMap<Object, Object>(), new java.util.HashMap<Object, Object>())
        {
            putAll(map);
        }

        /** 
         * Constructs a <code>HashBidiMap</code> that decorates the specified maps.
         *
         * @param normalMap  the normal direction map
         * @param reverseMap  the reverse direction map
         * @param inverseBidiMap  the inverse BidiMap
         */
        protected DualHashBidiMap(java.util.Map<Object, Object> normalMap, java.util.Map<Object, Object> reverseMap, BidiMap inverseBidiMap) :
            base(normalMap, reverseMap, inverseBidiMap)
        {
        }

        /**
         * Creates a new instance of this object.
         * 
         * @param normalMap  the normal direction map
         * @param reverseMap  the reverse direction map
         * @param inverseBidiMap  the inverse BidiMap
         * @return new bidi map
         */
        protected override BidiMap createBidiMap(java.util.Map<Object, Object> normalMap, java.util.Map<Object, Object> reverseMap, BidiMap inverseBidiMap)
        {
            return new DualHashBidiMap(normalMap, reverseMap, inverseBidiMap);
        }

        // Serialization
        //-----------------------------------------------------------------------
        private void writeObject(java.io.ObjectOutputStream outJ)
        {//throws IOException {
            outJ.defaultWriteObject();
            outJ.writeObject(maps[0]);
        }

        private void readObject(java.io.ObjectInputStream inJ)
        {//throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            maps[0] = new java.util.HashMap<Object, Object>();
            maps[1] = new java.util.HashMap<Object, Object>();
            java.util.Map<Object, Object> map = (java.util.Map<Object, Object>)inJ.readObject();
            putAll(map);
        }

    }
}