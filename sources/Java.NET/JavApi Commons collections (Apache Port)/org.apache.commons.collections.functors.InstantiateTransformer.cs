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
     * Transformer implementation that creates a new object instance by reflection.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Stephen Colebourne
     */
    [Serializable]
    public class InstantiateTransformer : Transformer, java.io.Serializable
    {

        /** The serial version */
        private static readonly long serialVersionUID = 3786388740793356347L;

        /** Singleton instance that uses the no arg constructor */
        public static readonly Transformer NO_ARG_INSTANCE = new InstantiateTransformer();

        /** The constructor parameter types */
        private readonly Type[] iParamTypes;
        /** The constructor arguments */
        private readonly Object[] iArgs;

        /**
         * Transformer method that performs validation.
         * 
         * @param paramTypes  the constructor parameter types
         * @param args  the constructor arguments
         * @return an instantiate transformer
         */
        public static Transformer getInstance(java.lang.Class[] paramTypes, Object[] args)
        {
            if (((paramTypes == null) && (args != null))
                || ((paramTypes != null) && (args == null))
                || ((paramTypes != null) && (args != null) && (paramTypes.Length != args.Length)))
            {
                throw new java.lang.IllegalArgumentException("Parameter types must match the arguments");
            }

            if (paramTypes == null || paramTypes.Length == 0)
            {
                return NO_ARG_INSTANCE;
            }
            else
            {
                paramTypes = (java.lang.Class[])paramTypes.clone();
                args = (Object[])args.clone();
            }
            return new InstantiateTransformer(paramTypes, args);
        }

        /**
         * Constructor for no arg instance.
         */
        private InstantiateTransformer()
            : base()
        {
            iParamTypes = null;
            iArgs = null;
        }

        /**
         * Constructor that performs no validation.
         * Use <code>getInstance</code> if you want that.
         * 
         * @param paramTypes  the constructor parameter types, not cloned
         * @param args  the constructor arguments, not cloned
         */
        public InstantiateTransformer(java.lang.Class[] paramTypes, Object[] args)
            : base()
        {
            System.Type[] types = new System.Type[paramTypes.Length];
            for (int i = 0; i < types.Length; i++)
            {
                types[i] = paramTypes[i].getDelegateInstance();
            }
            iParamTypes = types;
            iArgs = args;
        }

        /**
         * Transforms the input Class object to a result by instantiation.
         * 
         * @param input  the input object to transform
         * @return the transformed result
         */
        public Object transform(Object input)
        {
            try
            {
                if (input is java.lang.Class == false)
                {
                    throw new FunctorException(
                        "InstantiateTransformer: Input object was not an is Class, it was a "
                            + (input == null ? "null object" : input.getClass().getName()));
                }
                java.lang.reflect.Constructor con = ((java.lang.Class)input).getConstructor(iParamTypes);
                return con.newInstance(iArgs);

            }
            catch (java.lang.NoSuchMethodException ex)
            {
                throw new FunctorException("InstantiateTransformer: The constructor must exist and be public ");
            }
            catch (java.lang.InstantiationException ex)
            {
                throw new FunctorException("InstantiateTransformer: InstantiationException", ex);
            }
            catch (java.lang.IllegalAccessException ex)
            {
                throw new FunctorException("InstantiateTransformer: Constructor must be public", ex);
            }
            catch (java.lang.reflect.InvocationTargetException ex)
            {
                throw new FunctorException("InstantiateTransformer: Constructor threw an exception", ex);
            }
        }

    }
}