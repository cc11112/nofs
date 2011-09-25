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
     * Factory implementation that creates a new instance each time based on a prototype.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Stephen Colebourne
     */
    public class PrototypeFactory
    {

        /**
         * Factory method that performs validation.
         * <p>
         * Creates a Factory that will return a clone of the same prototype object
         * each time the factory is used. The prototype will be cloned using one of these
         * techniques (in order):
         * <ul>
         * <li>public clone method
         * <li>public copy constructor
         * <li>serialization clone
         * <ul>
         *
         * @param prototype  the object to clone each time in the factory
         * @return the <code>prototype</code> factory
         * @throws IllegalArgumentException if the prototype is null
         * @throws IllegalArgumentException if the prototype cannot be cloned
         */
        public static Factory getInstance(Object prototype)
        {
            if (prototype == null)
            {
                return ConstantFactory.NULL_INSTANCE;
            }
            try
            {
                java.lang.reflect.Method method = prototype.getClass().getMethod("clone", (java.lang.Class[])null);
                return new PrototypeCloneFactory(prototype, method);

            }
            catch (java.lang.NoSuchMethodException ex)
            {
                try
                {
                    prototype.getClass().getConstructor(new java.lang.Class[] { prototype.getClass() });
                    return new InstantiateFactory(
                        prototype.getClass(),
                        new java.lang.Class[] { prototype.getClass() },
                        new Object[] { prototype });

                }
                catch (java.lang.NoSuchMethodException ex2)
                {
                    if (prototype is java.io.Serializable)
                    {
                        return new PrototypeSerializationFactory((java.io.Serializable)prototype);
                    }
                }
            }
            throw new java.lang.IllegalArgumentException("The prototype must be cloneable via a public clone method");
        }

        /**
         * Constructor that performs no validation.
         * Use <code>getInstance</code> if you want that.
         */
        private PrototypeFactory()
            : base()
        {
        }

        // PrototypeCloneFactory
        //-----------------------------------------------------------------------
        /**
         * PrototypeCloneFactory creates objects by copying a prototype using the clone method.
         */
        [Serializable]
        class PrototypeCloneFactory : Factory, java.io.Serializable
        {

            /** The serial version */
            private static readonly long serialVersionUID = 5604271422565175555L;

            /** The object to clone each time */
            private readonly Object iPrototype;
            /** The method used to clone */
            [NonSerialized]
            private java.lang.reflect.Method iCloneMethod;

            /**
             * Constructor to store prototype.
             */
            internal PrototypeCloneFactory(Object prototype, java.lang.reflect.Method method)
                : base()
            {
                iPrototype = prototype;
                iCloneMethod = method;
            }

            /**
             * Find the Clone method for the class specified.
             */
            private void findCloneMethod()
            {
                try
                {
                    iCloneMethod = iPrototype.getClass().getMethod("clone", (java.lang.Class[])null);

                }
                catch (java.lang.NoSuchMethodException ex)
                {
                    throw new java.lang.IllegalArgumentException("PrototypeCloneFactory: The clone method must exist and be public ");
                }
            }

            /**
             * Creates an object by calling the clone method.
             * 
             * @return the new object
             */
            public Object create()
            {
                // needed for post-serialization
                if (iCloneMethod == null)
                {
                    findCloneMethod();
                }

                try
                {
                    return iCloneMethod.invoke(iPrototype, (Object[])null);

                }
                catch (java.lang.IllegalAccessException ex)
                {
                    throw new FunctorException("PrototypeCloneFactory: Clone method must be public", ex);
                }
                catch (java.lang.reflect.InvocationTargetException ex)
                {
                    throw new FunctorException("PrototypeCloneFactory: Clone method threw an exception", ex);
                }
            }
        }

        // PrototypeSerializationFactory
        //-----------------------------------------------------------------------
        /**
         * PrototypeSerializationFactory creates objects by cloning a prototype using serialization.
         */
        [Serializable]
        class PrototypeSerializationFactory : Factory, java.io.Serializable
        {

            /** The serial version */
            private static readonly long serialVersionUID = -8704966966139178833L;

            /** The object to clone via serialization each time */
            private readonly java.io.Serializable iPrototype;

            /**
             * Constructor to store prototype
             */
            internal PrototypeSerializationFactory(java.io.Serializable prototype)
                : base()
            {
                iPrototype = prototype;
            }

            /**
             * Creates an object using serialization.
             * 
             * @return the new object
             */
            public Object create()
            {
                java.io.ByteArrayOutputStream baos = new java.io.ByteArrayOutputStream(512);
                java.io.ByteArrayInputStream bais = null;
                try
                {
                    java.io.ObjectOutputStream outJ = new java.io.ObjectOutputStream(baos);
                    outJ.writeObject(iPrototype);

                    bais = new java.io.ByteArrayInputStream(baos.toByteArray());
                    java.io.ObjectInputStream inJ = new java.io.ObjectInputStream(bais);
                    return inJ.readObject();

                }
                catch (java.lang.ClassNotFoundException ex)
                {
                    throw new FunctorException(ex);
                }
                catch (java.io.IOException ex)
                {
                    throw new FunctorException(ex);
                }
                finally
                {
                    try
                    {
                        if (bais != null)
                        {
                            bais.close();
                        }
                    }
                    catch (java.io.IOException ex)
                    {
                        // ignore
                    }
                    try
                    {
                        if (baos != null)
                        {
                            baos.close();
                        }
                    }
                    catch (java.io.IOException ex)
                    {
                        // ignore
                    }
                }
            }
        }

    }
}