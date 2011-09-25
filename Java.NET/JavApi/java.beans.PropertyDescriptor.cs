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

namespace biz.ritter.javapi.beans
{
    public class PropertyDescriptor : FeatureDescriptor
    {
        private java.lang.reflect.Method getter;

        private java.lang.reflect.Method setter;

        public java.lang.reflect.Method getWriteMethod()
        {
            return setter;
        }

        public java.lang.reflect.Method getReadMethod()
        {
            return getter;
        }

        public PropertyDescriptor(String propertyName, java.lang.Class beanClass,
                String getterName, String setterName)
            : base()
        {//throws IntrospectionException {
            if (beanClass == null)
            {
                throw new IntrospectionException("Target Bean class is null");//Messages.getString("beans.03")); //$NON-NLS-1$
            }
            if (propertyName == null || propertyName.length() == 0)
            {
                throw new IntrospectionException("bad property name");//Messages.getString("beans.04")); //$NON-NLS-1$
            }
            this.setName(propertyName);
            if (getterName != null)
            {
                if (getterName.length() == 0)
                {
                    throw new IntrospectionException(
                            "read or write method cannot be empty."); //$NON-NLS-1$    
                }
                try
                {
                    setReadMethod(beanClass, getterName);
                }
                catch (IntrospectionException e)
                {
                    setReadMethod(beanClass, createDefaultMethodName(propertyName,
                            "get")); //$NON-NLS-1$
                }
            }
            if (setterName != null)
            {
                if (setterName.length() == 0)
                {
                    throw new IntrospectionException(
                            "read or write method cannot be empty."); //$NON-NLS-1$    
                }
                setWriteMethod(beanClass, setterName);
            }
        }


        void setWriteMethod(java.lang.Class beanClass, String setterName)
        {//throws IntrospectionException {
            java.lang.reflect.Method writeMethod = null;
            try
            {
                if (getter != null)
                {
                    writeMethod = beanClass.getMethod(setterName,
                            new java.lang.Class[] { getter.getReturnType() });
                }
                else
                {
                    java.lang.Class clazz = beanClass;
                    java.lang.reflect.Method[] methods = null;
                    while (clazz != null && writeMethod == null)
                    {
                        methods = clazz.getDeclaredMethods();
                        foreach (java.lang.reflect.Method method in methods)
                        {
                            if (setterName.equals(method.getName()))
                            {
                                if (method.getParameterTypes().Length == 1)
                                {
                                    writeMethod = method;
                                    break;
                                }
                            }
                        }
                        clazz = clazz.getSuperclass();
                    }
                }
            }
            catch (java.lang.Exception e)
            {
                throw new IntrospectionException(e.getLocalizedMessage());
            }
            catch (System.Exception e)
            {
                throw new IntrospectionException(e.Message);
            }
            if (writeMethod == null)
            {
                throw new IntrospectionException("Method not found: " + setterName); //$NON-NLS-1$
            }
            setWriteMethod(writeMethod);
        }

        public void setWriteMethod(java.lang.reflect.Method setter)
        {//throws IntrospectionException {
            if (setter != null)
            {
                int modifiers = setter.getModifiers();
                if (!java.lang.reflect.Modifier.isPublic(modifiers))
                {
                    throw new IntrospectionException("Modifier for setter method should be public.");//Messages.getString("beans.05")); //$NON-NLS-1$
                }
                java.lang.Class[] parameterTypes = setter.getParameterTypes();
                if (parameterTypes.Length != 1)
                {
                    throw new IntrospectionException("Number of parameters in setter method is not equal to 1.");//Messages.getString("beans.06")); //$NON-NLS-1$
                }
                java.lang.Class parameterType = parameterTypes[0];
                java.lang.Class propertyType = getPropertyType();
                if (propertyType != null && !propertyType.equals(parameterType))
                {
                    throw new IntrospectionException("Parameter type in setter method does not corresponds to predefined.");//Messages.getString("beans.07")); //$NON-NLS-1$
                }
            }
            this.setter = setter;
        }

        String createDefaultMethodName(String propertyName, String prefix)
        {
            String result = null;
            if (propertyName != null)
            {
                result = prefix
                    + propertyName.Substring(0, 1).ToUpperInvariant()
                    + propertyName.Substring(1);
            }
            return result;
        }
        void setReadMethod(java.lang.Class beanClass, String getterName)
        {//throws IntrospectionException {
            try
            {
                java.lang.reflect.Method readMethod = beanClass.getMethod(getterName, new java.lang.Class[] { });
                setReadMethod(readMethod);
            }
            catch (java.lang.Exception e)
            {
                throw new IntrospectionException(e.getLocalizedMessage());
            }
            catch (System.Exception e)
            {
                throw new IntrospectionException(e.getMessage());
            }
        }
        public java.lang.Class getPropertyType()
        {
            java.lang.Class result = null;
            if (getter != null)
            {
                result = getter.getReturnType();
            }
            else if (setter != null)
            {
                java.lang.Class[] parameterTypes = setter.getParameterTypes();
                result = parameterTypes[0];
            }
            return result;
        }

        public void setReadMethod(java.lang.reflect.Method getter)
        {//throws IntrospectionException {
            if (getter != null)
            {
                int modifiers = getter.getModifiers();
                if (!java.lang.reflect.Modifier.isPublic(modifiers))
                {
                    throw new IntrospectionException("Modifier for getter method should be public.");//Messages.getString("beans.0A")); //$NON-NLS-1$
                }
                java.lang.Class[] parameterTypes = getter.getParameterTypes();
                if (parameterTypes.Length != 0)
                {
                    throw new IntrospectionException("Number of parameters in getter method is not equal to 0.");//Messages.getString("beans.08")); //$NON-NLS-1$
                }
                java.lang.Class returnType = getter.getReturnType();
                if (returnType.equals(java.lang.Void.TYPE))
                {
                    throw new IntrospectionException("{0} does not return <void>");//Messages.getString("beans.33")); //$NON-NLS-1$
                }
                java.lang.Class propertyType = getPropertyType();
                if ((propertyType != null) && !returnType.equals(propertyType))
                {
                    throw new IntrospectionException("Parameter type in getter method does not corresponds to predefined.");//Messages.getString("beans.09")); //$NON-NLS-1$
                }
            }
            this.getter = getter;
        }

    }
}