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
     * <p>
     * An implementation of a Map which has a maximum size and uses a Least Recently Used
     * algorithm to remove items from the Map when the maximum size is reached and new items are added.
     * </p>
     * 
     * <p>
     * A synchronized version can be obtained with:
     * <code>Collections.synchronizedMap( theMapToSynchronize )</code>
     * If it will be accessed by multiple threads, you _must_ synchronize access
     * to this Map.  Even concurrent get(Object) operations produce indeterminate
     * behaviour.
     * </p>
     * 
     * <p>
     * Unlike the Collections 1.0 version, this version of LRUMap does use a true
     * LRU algorithm.  The keys for all gets and puts are moved to the front of
     * the list.  LRUMap is now a subclass of SequencedHashMap, and the "LRU"
     * key is now equivalent to LRUMap.getFirst().
     * </p>
     * 
     * @deprecated Moved to map subpackage. Due to be removed in v4.0.
     * @since Commons Collections 1.0
     * @version $Revision$ $Date$
     * 
     * @author <a href="mailto:jstrachan@apache.org">James Strachan</a>
     * @author <a href="mailto:morgand@apache.org">Morgan Delagrange</a>
     */
    [Obsolete]
    [Serializable]
    public class LRUMap : SequencedHashMap, java.io.Externalizable
    {

        private int maximumSize = 0;

        /**
         * Default constructor, primarily for the purpose of
         * de-externalization.  This constructors sets a default
         * LRU limit of 100 keys, but this value may be overridden
         * internally as a result of de-externalization.
         */
        public LRUMap() :
            this(100)
        {
        }

        /**
         * Create a new LRUMap with a maximum capacity of <i>i</i>.
         * Once <i>i</i> capacity is achieved, subsequent gets
         * and puts will push keys out of the map.  See .
         * 
         * @param i      Maximum capacity of the LRUMap
         */
        public LRUMap(int i)
            : base(i)
        {
            maximumSize = i;
        }

        /**
         * <p>Get the value for a key from the Map.  The key
         * will be promoted to the Most Recently Used position.
         * Note that get(Object) operations will modify
         * the underlying Collection.  Calling get(Object)
         * inside of an iteration over keys, values, etc. is
         * currently unsupported.</p>
         * 
         * @param key    Key to retrieve
         * @return Returns the value.  Returns null if the key has a
         *         null value <i>or</i> if the key has no value.
         */
        public override Object get(Object key)
        {
            if (!containsKey(key)) return null;

            Object value = remove(key);
            base.put(key, value);
            return value;
        }

        /**
         * <p>Removes the key and its Object from the Map.</p>
         * 
         * <p>(Note: this may result in the "Least Recently Used"
         * object being removed from the Map.  In that case,
         * the removeLRU() method is called.  See javadoc for
         * removeLRU() for more details.)</p>
         * 
         * @param key    Key of the Object to add.
         * @param value  Object to add
         * @return Former value of the key
         */
        public override Object put(Object key, Object value)
        {

            int mapSize = size();
            Object retval = null;

            if (mapSize >= maximumSize)
            {

                // don't retire LRU if you are just
                // updating an existing key
                if (!containsKey(key))
                {
                    // lets retire the least recently used item in the cache
                    removeLRU();
                }
            }

            retval = base.put(key, value);

            return retval;
        }

        /**
         * This method is used internally by the class for 
         * finding and removing the LRU Object.
         */
        protected virtual void removeLRU()
        {
            Object key = getFirstKey();
            // be sure to call super.get(key), or you're likely to 
            // get infinite promotion recursion
            Object value = base.get(key);

            remove(key);

            processRemovedLRU(key, value);
        }

        /**
         * Subclasses of LRUMap may hook into this method to
         * provide specialized actions whenever an Object is
         * automatically removed from the cache.  By default,
         * this method does nothing.
         * 
         * @param key    key that was removed
         * @param value  value of that key (can be null)
         */
        protected virtual void processRemovedLRU(Object key, Object value)
        {
        }

        // Externalizable interface
        //-------------------------------------------------------------------------        
        public override void readExternal(java.io.ObjectInput inJ)
        {//  throws IOException, ClassNotFoundException {
            maximumSize = inJ.readInt();
            int size = inJ.readInt();

            for (int i = 0; i < size; i++)
            {
                Object key = inJ.readObject();
                Object value = inJ.readObject();
                put(key, value);
            }
        }

        public override void writeExternal(java.io.ObjectOutput outJ)
        {//throws IOException {
            outJ.writeInt(maximumSize);
            outJ.writeInt(size());
            for (java.util.Iterator<Object> iterator = keySet().iterator(); iterator.hasNext(); )
            {
                Object key = iterator.next();
                outJ.writeObject(key);
                // be sure to call super.get(key), or you're likely to 
                // get infinite promotion recursion
                Object value = base.get(key);
                outJ.writeObject(value);
            }
        }


        // Properties
        //-------------------------------------------------------------------------        
        /** Getter for property maximumSize.
         * @return Value of property maximumSize.
         */
        public virtual int getMaximumSize()
        {
            return maximumSize;
        }
        /** Setter for property maximumSize.
         * @param maximumSize New value of property maximumSize.
         */
        public virtual void setMaximumSize(int maximumSize)
        {
            this.maximumSize = maximumSize;
            while (size() > maximumSize)
            {
                removeLRU();
            }
        }


        // add a serial version uid, so that if we change things in the future
        // without changing the format, we can still deserialize properly.
        private static readonly long serialVersionUID = 2197433140769957051L;
    }
}