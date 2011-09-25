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

namespace org.apache.commons.collections.set
{

    /**
     * java.io.Serializable subclass of AbstractSetDecorator.
     * 
     * @author Stephen Colebourne
     * @since Commons Collections 3.1
     */
    [Serializable]
    public abstract class AbstractSerializableSetDecorator
            : AbstractSetDecorator
            , java.io.Serializable
    {

        /** Serialization version */
        private static readonly long serialVersionUID = 1229469966212206107L;

        /**
         * Constructor.
         */
        protected AbstractSerializableSetDecorator(java.util.Set<Object> set)
            : base(set)
        {
        }

        //-----------------------------------------------------------------------
        /**
         * Write the set out using a custom routine.
         * 
         * @param out  the output stream
         * @throws IOException
         */
        private void writeObject(java.io.ObjectOutputStream outJ)
        {//throws IOException {
            outJ.defaultWriteObject();
            outJ.writeObject(collection);
        }

        /**
         * Read the set in using a custom routine.
         * 
         * @param in  the input stream
         * @throws IOException
         * @throws ClassNotFoundException
         */
        private void readObject(java.io.ObjectInputStream inJ)
        {// throws IOException, ClassNotFoundException {
            inJ.defaultReadObject();
            collection = (java.util.Collection<Object>)inJ.readObject();
        }

    }
}