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
using System.Reflection;
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
    public class InvokerTransformer : Transformer, java.io.Serializable
    {

        /** The serial version */
        private static readonly long serialVersionUID = -8653385846894047688L;

        /** The method name to call */
        private readonly String iMethodName;
        /** The array of reflection parameter types */
        private readonly Type[] iParamTypes;
        /** The array of reflection arguments */
        private readonly Object[] iArgs;

        /**
         * Gets an instance of this transformer calling a specific method with no arguments.
         * 
         * @param methodName  the method name to call
         * @return an invoker transformer
         * @since Commons Collections 3.1
         */
        public static Transformer getInstance(String methodName)
        {
            if (methodName == null)
            {
                throw new java.lang.IllegalArgumentException("The method to invoke must not be null");
            }
            return new InvokerTransformer(methodName);
        }

        /**
         * Gets an instance of this transformer calling a specific method with specific values.
         * 
         * @param methodName  the method name to call
         * @param paramTypes  the parameter types of the method
         * @param args  the arguments to pass to the method
         * @return an invoker transformer
         */
        public static Transformer getInstance(String methodName, java.lang.Class[] paramTypes, Object[] args)
        {
            Type[] paramClassType = new Type[paramTypes.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                paramClassType[i] = paramTypes[i].GetType();
            }
            return getInstance(methodName, paramClassType, args);
        }

        /**
         * Gets an instance of this transformer calling a specific method with specific values.
         * 
         * @param methodName  the method name to call
         * @param paramTypes  the parameter types of the method
         * @param args  the arguments to pass to the method
         * @return an invoker transformer
         */
        internal static Transformer getInstance(String methodName, Type[] paramTypes, Object[] args)
        {
            if (methodName == null)
            {
                throw new java.lang.IllegalArgumentException("The method to invoke must not be null");
            }
            if (((paramTypes == null) && (args != null))
                || ((paramTypes != null) && (args == null))
                || ((paramTypes != null) && (args != null) && (paramTypes.Length != args.Length)))
            {
                throw new java.lang.IllegalArgumentException("The parameter types must match the arguments");
            }
            if (paramTypes == null || paramTypes.Length == 0)
            {
                return new InvokerTransformer(methodName);
            }
            else
            {
                paramTypes = (Type[])paramTypes.clone();
                args = (Object[])args.clone();
                return new InvokerTransformer(methodName, paramTypes, args);
            }
        }

        /**
         * Constructor for no arg instance.
         * 
         * @param methodName  the method to call
         */
        private InvokerTransformer(String methodName)
            : base()
        {
            iMethodName = methodName;
            iParamTypes = null;
            iArgs = null;
        }

        /**
         * Constructor that performs no validation.
         * Use <code>getInstance</code> if you want that.
         * 
         * @param methodName  the method to call
         * @param paramTypes  the constructor parameter types, not cloned
         * @param args  the constructor arguments, not cloned
         */
        public InvokerTransformer(String methodName, Type[] paramTypes, Object[] args)
            : base()
        {
            iMethodName = methodName;
            iParamTypes = paramTypes;
            iArgs = args;
        }

        /**
         * Transforms the input to result by invoking a method on the input.
         * 
         * @param input  the input object to transform
         * @return the transformed result, null if null input
         */
        public Object transform(Object input)
        {
            if (input == null)
            {
                return null;
            }
            try
            {
                // C#
                // Type c = input.GetType();
                // MethodInfo mi = c.GetMethod(iMethodName, iParamTypes);
                // if (null == mi) throw new FunctorException("InvokerTransformer: The method '" + iMethodName + "' on '" + input.GetType() + "' does not exist");
                // return mi.Invoke(input, iArgs); 
                // JavApi
                java.lang.Class cls = input.getClass();
                java.lang.reflect.Method method = cls.getMethod(iMethodName, iParamTypes);
                return method.invoke(input, iArgs);
            } 
            catch (java.lang.NoSuchMethodException ex) {
                throw new FunctorException("InvokerTransformer: The method '" + iMethodName + "' on '" + input.getClass() + "' does not exist");
            } catch (java.lang.IllegalAccessException ex) {
                throw new FunctorException("InvokerTransformer: The method '" + iMethodName + "' on '" + input.GetType() + "' cannot be accessed");
            }
            catch (Exception exc) {
                // HACK - How to add System.Exception in constructor...
                throw new FunctorException("InvokerTransformer: The method '" + iMethodName + "' on '" + input.GetType() + "' threw an exception "+exc.GetType());//, ex);
            }
        }

    }
}