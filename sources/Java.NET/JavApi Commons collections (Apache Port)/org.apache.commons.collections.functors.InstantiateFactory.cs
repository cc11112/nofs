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
     * Factory implementation that creates a new object instance by reflection.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Stephen Colebourne
     */
    [Serializable]
    public class InstantiateFactory : Factory, java.io.Serializable
    {

        /** The serial version */
        private static readonly long serialVersionUID = -7732226881069447957L;

        /** The class to create */
        private readonly java.lang.Class iClassToInstantiate;
        /** The constructor parameter types */
        private readonly java.lang.Class[] iParamTypes;
        /** The constructor arguments */
        private readonly Object[] iArgs;
        /** The constructor */
        [NonSerialized]
        private java.lang.reflect.Constructor iConstructor = null;

        /**
         * Factory method that performs validation.
         * 
         * @param classToInstantiate  the class to instantiate, not null
         * @param paramTypes  the constructor parameter types
         * @param args  the constructor arguments
         * @return a new instantiate factory
         */
        public static Factory getInstance(java.lang.Class classToInstantiate, java.lang.Class[] paramTypes, Object[] args)
        {
            if (classToInstantiate == null)
            {
                throw new java.lang.IllegalArgumentException("Class to instantiate must not be null");
            }
            if (((paramTypes == null) && (args != null))
                || ((paramTypes != null) && (args == null))
                || ((paramTypes != null) && (args != null) && (paramTypes.Length != args.Length)))
            {
                throw new java.lang.IllegalArgumentException("Parameter types must match the arguments");
            }

            if (paramTypes == null || paramTypes.Length == 0)
            {
                return new InstantiateFactory(classToInstantiate);
            }
            else
            {
                paramTypes = (java.lang.Class[])paramTypes.clone();
                args = (Object[])args.clone();
                return new InstantiateFactory(classToInstantiate, paramTypes, args);
            }
        }

        /**
         * Constructor that performs no validation.
         * Use <code>getInstance</code> if you want that.
         * 
         * @param classToInstantiate  the class to instantiate
         */
        public InstantiateFactory(java.lang.Class classToInstantiate)
            : base()
        {
            iClassToInstantiate = classToInstantiate;
            iParamTypes = null;
            iArgs = null;
            findConstructor();
        }

        /**
         * Constructor that performs no validation.
         * Use <code>getInstance</code> if you want that.
         * 
         * @param classToInstantiate  the class to instantiate
         * @param paramTypes  the constructor parameter types, not cloned
         * @param args  the constructor arguments, not cloned
         */
        public InstantiateFactory(java.lang.Class classToInstantiate, java.lang.Class[] paramTypes, Object[] args)
            : base()
        {
            iClassToInstantiate = classToInstantiate;
            iParamTypes = paramTypes;
            iArgs = args;
            findConstructor();
        }

        /**
         * Find the Constructor for the class specified.
         */
        private void findConstructor()
        {
            try
            {
                iConstructor = iClassToInstantiate.getConstructor(iParamTypes);

            }
            catch (java.lang.NoSuchMethodException ex)
            {
                throw new java.lang.IllegalArgumentException("InstantiateFactory: The constructor must exist and be public ");
            }
        }

        /**
         * Creates an object using the stored constructor.
         * 
         * @return the new object
         */
        public Object create()
        {
            // needed for post-serialization
            if (iConstructor == null)
            {
                findConstructor();
            }

            try
            {
                return iConstructor.newInstance(iArgs);

            }
            catch (java.lang.InstantiationException ex)
            {
                throw new FunctorException("InstantiateFactory: InstantiationException", ex);
            }
            catch (java.lang.IllegalAccessException ex)
            {
                throw new FunctorException("InstantiateFactory: Constructor must be public", ex);
            }
            catch (java.lang.reflect.InvocationTargetException ex)
            {
                throw new FunctorException("InstantiateFactory: Constructor threw an exception", ex);
            }
        }

    }
}