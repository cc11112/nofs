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
using org.apache.commons.collections.list;
using org.apache.commons.collections.keyvalue;
using org.apache.commons.collections.set;

namespace org.apache.commons.collections
{

    /** 
     * An implementation of Map for JavaBeans which uses introspection to
     * get and put properties in the bean.
     * <p>
     * If an exception occurs during attempts to get or set a property then the
     * property is considered non existent in the Map
     *
     * @since Commons Collections 1.0
     * @version $Revision$ $Date$
     * 
     * @author James Strachan
     * @author Stephen Colebourne
     * @author Dimiter Dimitrov
     * 
     * @deprecated Identical class now available in commons-beanutils (full jar version).
     * This version is due to be removed in collections v4.0.
     */
    [Obsolete]
    [Serializable]
    public class BeanMap : java.util.AbstractMap<Object, Object>, java.lang.Cloneable
    {
        [NonSerialized]
        private Object bean;

        [NonSerialized]
        private java.util.HashMap<Object, Object> readMethods = new java.util.HashMap<Object, Object>();
        [NonSerialized]
        private java.util.HashMap<Object, Object> writeMethods = new java.util.HashMap<Object, Object>();
        [NonSerialized]
        private java.util.HashMap<Object, Object> types = new java.util.HashMap<Object, Object>();

        /**
         * An empty array.  Used to invoke accessors via reflection.
         */
        public static readonly Object[] NULL_ARGUMENTS = { };

        /**
         * Maps primitive Class types to transformers.  The transformer
         * transform strings into the appropriate primitive wrapper.
         */
        public static java.util.HashMap<Object, Object> defaultTransformers = new java.util.HashMap<Object, Object>();

        static BeanMap()
        {
            defaultTransformers.put(java.lang.Boolean.TYPE, new IAC_BooleanTransformer());
            defaultTransformers.put(java.lang.Character.TYPE, new IAC_CharacterTransformer());
            defaultTransformers.put(java.lang.Byte.TYPE, new IAC_ByteTransformer());
            defaultTransformers.put(java.lang.Short.TYPE, new IAC_ShortTransformer());
            defaultTransformers.put(java.lang.Integer.TYPE, new IAC_IntegerTransformer());
            defaultTransformers.put(java.lang.Long.TYPE, new IAC_LongTransformer());
            defaultTransformers.put(java.lang.Float.TYPE, new IAC_FloatTransformer());
            defaultTransformers.put(java.lang.Double.TYPE, new IAC_DoubleTransformer());
        }
        #region TransformerImpl
        class IAC_BooleanTransformer : Transformer
        {
            public Object transform(Object input)
            {
                return java.lang.Boolean.valueOf(input.toString());
            }
        }
        class IAC_CharacterTransformer : Transformer
        {
            public Object transform(Object input)
            {
                return new java.lang.Character(input.toString().charAt(0));
            }
        }
        class IAC_ByteTransformer : Transformer
        {
            public Object transform(Object input)
            {
                return java.lang.Byte.valueOf(input.toString());
            }
        }
        class IAC_ShortTransformer : Transformer
        {
            public Object transform(Object input)
            {
                return java.lang.Short.valueOf(input.toString());
            }
        }
        class IAC_IntegerTransformer : Transformer
        {
            public Object transform(Object input)
            {
                return java.lang.Integer.valueOf(input.toString());
            }
        }
        class IAC_LongTransformer : Transformer
        {
            public Object transform(Object input)
            {
                return java.lang.Long.valueOf(input.toString());
            }
        }
        class IAC_FloatTransformer : Transformer
        {
            public Object transform(Object input)
            {
                return java.lang.Float.valueOf(input.toString());
            }
        }
        class IAC_DoubleTransformer : Transformer
        {
            public Object transform(Object input)
            {
                return java.lang.Double.valueOf(input.toString());
            }
        }
        #endregion

        // Constructors
        //-------------------------------------------------------------------------

        /**
         * Constructs a new empty <code>BeanMap</code>.
         */
        public BeanMap()
        {
        }

        /**
         * Constructs a new <code>BeanMap</code> that operates on the 
         * specified bean.  If the given bean is <code>null</code>, then
         * this map will be empty.
         *
         * @param bean  the bean for this map to operate on
         */
        public BeanMap(Object bean)
        {
            this.bean = bean;
            initialise();
        }

        // Map interface
        //-------------------------------------------------------------------------

        public override String ToString()
        {
            return "BeanMap<" + java.lang.StringJ.valueOf(bean) + ">";
        }

        /**
         * Clone this bean map using the following process: 
         *
         * <ul>
         * <li>If there is no underlying bean, return a cloned BeanMap without a
         * bean.
         *
         * <li>Since there is an underlying bean, try to instantiate a new bean of
         * the same type using Class.newInstance().
         * 
         * <li>If the instantiation fails, throw a CloneNotSupportedException
         *
         * <li>Clone the bean map and set the newly instantiated bean as the
         * underlying bean for the bean map.
         *
         * <li>Copy each property that is both readable and writable from the
         * existing object to a cloned bean map.  
         *
         * <li>If anything fails along the way, throw a
         * CloneNotSupportedException.
         *
         * <ul>
         */
        public Object clone()
        {//throws CloneNotSupportedException {
            BeanMap newMap = (BeanMap)base.MemberwiseClone();

            if (bean == null)
            {
                // no bean, just an empty bean map at the moment.  return a newly
                // cloned and empty bean map.
                return newMap;
            }

            Object newBean = null;
            java.lang.Class beanClass = null;
            try
            {
                beanClass = bean.getClass();
                newBean = beanClass.newInstance();
            }
            catch (Exception e)
            {
                // unable to instantiate
                throw new java.lang.CloneNotSupportedException
                    ("Unable to instantiate the underlying bean \"" +
                     beanClass.getName() + "\": " + e);
            }

            try
            {
                newMap.setBean(newBean);
            }
            catch (Exception exception)
            {
                throw new java.lang.CloneNotSupportedException
                    ("Unable to set bean in the cloned bean map: " +
                     exception);
            }

            try
            {
                // copy only properties that are readable and writable.  If its
                // not readable, we can't get the value from the old map.  If
                // its not writable, we can't write a value into the new map.
                java.util.Iterator<Object> readableKeys = readMethods.keySet().iterator();
                while (readableKeys.hasNext())
                {
                    Object key = readableKeys.next();
                    if (getWriteMethod(key) != null)
                    {
                        newMap.put(key, get(key));
                    }
                }
            }
            catch (Exception exception)
            {
                throw new java.lang.CloneNotSupportedException
                    ("Unable to copy bean values to cloned bean map: " +
                     exception);
            }

            return newMap;
        }

        /**
         * Puts all of the writable properties from the given BeanMap into this
         * BeanMap. Read-only and Write-only properties will be ignored.
         *
         * @param map  the BeanMap whose properties to put
         */
        public void putAllWriteable(BeanMap map)
        {
            java.util.Iterator<Object> readableKeys = map.readMethods.keySet().iterator();
            while (readableKeys.hasNext())
            {
                Object key = readableKeys.next();
                if (getWriteMethod(key) != null)
                {
                    this.put(key, map.get(key));
                }
            }
        }


        /**
         * This method reinitializes the bean map to have default values for the
         * bean's properties.  This is accomplished by constructing a new instance
         * of the bean which the map uses as its underlying data source.  This
         * behavior for <code>clear()</code> differs from the Map contract in that
         * the mappings are not actually removed from the map (the mappings for a
         * BeanMap are fixed).
         */
        public override void clear()
        {
            if (bean == null) return;

            java.lang.Class beanClass = null;
            try
            {
                beanClass = bean.getClass();
                bean = beanClass.newInstance();
            }
            catch (Exception e)
            {
                throw new java.lang.UnsupportedOperationException("Could not create new instance of class: " + beanClass);
            }
        }

        /**
         * Returns true if the bean defines a property with the given name.
         * <p>
         * The given name must be a <code>String</code>; if not, this method
         * returns false. This method will also return false if the bean
         * does not define a property with that name.
         * <p>
         * Write-only properties will not be matched as the test operates against
         * property read methods.
         *
         * @param name  the name of the property to check
         * @return false if the given name is null or is not a <code>String</code>;
         *   false if the bean does not define a property with that name; or
         *   true if the bean does define a property with that name
         */
        public override bool containsKey(Object name)
        {
            java.lang.reflect.Method method = getReadMethod(name);
            return method != null;
        }

        /**
         * Returns true if the bean defines a property whose current value is
         * the given object.
         *
         * @param value  the value to check
         * @return false  true if the bean has at least one property whose 
         *   current value is that object, false otherwise
         */
        public override bool containsValue(Object value)
        {
            // use default implementation
            return base.containsValue(value);
        }

        /**
         * Returns the value of the bean's property with the given name.
         * <p>
         * The given name must be a {@link String} and must not be 
         * null; otherwise, this method returns <code>null</code>.
         * If the bean defines a property with the given name, the value of
         * that property is returned.  Otherwise, <code>null</code> is 
         * returned.
         * <p>
         * Write-only properties will not be matched as the test operates against
         * property read methods.
         *
         * @param name  the name of the property whose value to return
         * @return  the value of the property with that name
         */
        public override Object get(Object name)
        {
            if (bean != null)
            {
                java.lang.reflect.Method method = getReadMethod(name);
                if (method != null)
                {
                    try
                    {
                        return method.invoke(bean, NULL_ARGUMENTS);
                    }
                    catch (java.lang.IllegalAccessException e)
                    {
                        logWarn(e);
                    }
                    catch (java.lang.IllegalArgumentException e)
                    {
                        logWarn(e);
                    }
                    catch (java.lang.reflect.InvocationTargetException e)
                    {
                        logWarn(e);
                    }
                    catch (java.lang.NullPointerException e)
                    {
                        logWarn(e);
                    }
                }
            }
            return null;
        }

        /**
         * Sets the bean property with the given name to the given value.
         *
         * @param name  the name of the property to set
         * @param value  the value to set that property to
         * @return  the previous value of that property
         * @throws IllegalArgumentException  if the given name is null;
         *   if the given name is not a {@link String}; if the bean doesn't
         *   define a property with that name; or if the bean property with
         *   that name is read-only
         */
        public override Object put(Object name, Object value)
        {//throws IllegalArgumentException, ClassCastException {
            if (bean != null)
            {
                Object oldValue = get(name);
                java.lang.reflect.Method method = getWriteMethod(name);
                if (method == null)
                {
                    throw new java.lang.IllegalArgumentException("The bean of type: " + bean.getClass().getName() + " has no property called: " + name);
                }
                try
                {
                    Object[] arguments = createWriteMethodArguments(method, value);
                    method.invoke(bean, arguments);

                    Object newValue = get(name);
                    firePropertyChange(name, oldValue, newValue);
                }
                catch (java.lang.reflect.InvocationTargetException e)
                {
                    logInfo(e);
                    throw new java.lang.IllegalArgumentException(e.getMessage());
                }
                catch (java.lang.IllegalAccessException e)
                {
                    logInfo(e);
                    throw new java.lang.IllegalArgumentException(e.getMessage());
                }
                return oldValue;
            }
            return null;
        }

        /**
         * Returns the number of properties defined by the bean.
         *
         * @return  the number of properties defined by the bean
         */
        public override int size()
        {
            return readMethods.size();
        }


        /**
         * Get the keys for this BeanMap.
         * <p>
         * Write-only properties are <b>not</b> included in the returned set of
         * property names, although it is possible to set their value and to get 
         * their type.
         * 
         * @return BeanMap keys.  The Set returned by this method is not
         *        modifiable.
         */
        public override java.util.Set<Object> keySet()
        {
            return UnmodifiableSet.decorate(readMethods.keySet());
        }

        /**
         * Gets a Set of MapEntry objects that are the mappings for this BeanMap.
         * <p>
         * Each MapEntry can be set but not removed.
         * 
         * @return the unmodifiable set of mappings
         */
        public new java.util.Set<Object> entrySet()
        {
            return UnmodifiableSet.decorate(new IAC_EntrySet(this));
        }
        class IAC_EntrySet : java.util.AbstractSet<Object>
        {
            private readonly BeanMap root;
            public IAC_EntrySet(BeanMap root)
            {
                this.root = root;
            }
            public override java.util.Iterator<Object> iterator()
            {
                return root.entryIterator();
            }
            public override int size()
            {
                return root.readMethods.size();
            }
        }

        /**
         * Returns the values for the BeanMap.
         * 
         * @return values for the BeanMap.  The returned collection is not
         *        modifiable.
         */
        public override java.util.Collection<Object> values()
        {
            java.util.ArrayList<Object> answer = new java.util.ArrayList<Object>(readMethods.size());
            for (java.util.Iterator<Object> iter = valueIterator(); iter.hasNext(); )
            {
                answer.add(iter.next());
            }
            return UnmodifiableList.decorate(answer);
        }


        // Helper methods
        //-------------------------------------------------------------------------

        /**
         * Returns the type of the property with the given name.
         *
         * @param name  the name of the property
         * @return  the type of the property, or <code>null</code> if no such
         *  property exists
         */
        public virtual java.lang.Class getType(String name)
        {
            return (java.lang.Class)types.get(name);
        }

        /**
         * Convenience method for getting an iterator over the keys.
         * <p>
         * Write-only properties will not be returned in the iterator.
         *
         * @return an iterator over the keys
         */
        public virtual java.util.Iterator<Object> keyIterator()
        {
            return readMethods.keySet().iterator();
        }

        /**
         * Convenience method for getting an iterator over the values.
         *
         * @return an iterator over the values
         */
        public virtual java.util.Iterator<Object> valueIterator()
        {
            return new IAC_ValueIterator(this);
        }
        public class IAC_ValueIterator : java.util.Iterator<Object>
        {
            private readonly BeanMap root;
            private readonly java.util.Iterator<Object> iter;
            public IAC_ValueIterator(BeanMap root)
            {
                this.root = root;
                iter = root.keyIterator();
            }
            public bool hasNext()
            {
                return iter.hasNext();
            }
            public Object next()
            {
                Object key = iter.next();
                return root.get(key);
            }
            public void remove()
            {
                throw new java.lang.UnsupportedOperationException("remove() not supported for BeanMap");
            }
        }

        /**
         * Convenience method for getting an iterator over the entries.
         *
         * @return an iterator over the entries
         */
        public java.util.Iterator<Object> entryIterator()
        {
            return new IAC_EntryIterator(this);
        }
        class IAC_EntryIterator : java.util.Iterator<Object>
        {
            private readonly BeanMap root;
            private readonly java.util.Iterator<Object> iter;
            public IAC_EntryIterator(BeanMap root)
            {
                this.root = root;
                this.iter = root.keyIterator();
            }
            public bool hasNext()
            {
                return iter.hasNext();
            }
            public Object next()
            {
                Object key = iter.next();
                Object value = root.get(key);
                return new MyMapEntry(root, key, value);
            }
            public void remove()
            {
                throw new java.lang.UnsupportedOperationException("remove() not supported for BeanMap");
            }
        }


        // Properties
        //-------------------------------------------------------------------------

        /**
         * Returns the bean currently being operated on.  The return value may
         * be null if this map is empty.
         *
         * @return the bean being operated on by this map
         */
        public virtual Object getBean()
        {
            return bean;
        }

        /**
         * Sets the bean to be operated on by this map.  The given value may
         * be null, in which case this map will be empty.
         *
         * @param newBean  the new bean to operate on
         */
        public virtual void setBean(Object newBean)
        {
            bean = newBean;
            reinitialise();
        }

        /**
         * Returns the accessor for the property with the given name.
         *
         * @param name  the name of the property 
         * @return the accessor method for the property, or null
         */
        public virtual java.lang.reflect.Method getReadMethod(String name)
        {
            return (java.lang.reflect.Method)readMethods.get(name);
        }

        /**
         * Returns the mutator for the property with the given name.
         *
         * @param name  the name of the property
         * @return the mutator method for the property, or null
         */
        public virtual java.lang.reflect.Method getWriteMethod(String name)
        {
            return (java.lang.reflect.Method)writeMethods.get(name);
        }


        // Implementation methods
        //-------------------------------------------------------------------------

        /**
         * Returns the accessor for the property with the given name.
         *
         * @param name  the name of the property 
         * @return null if the name is null; null if the name is not a 
         * {@link String}; null if no such property exists; or the accessor
         *  method for that property
         */
        protected virtual java.lang.reflect.Method getReadMethod(Object name)
        {
            return (java.lang.reflect.Method)readMethods.get(name);
        }

        /**
         * Returns the mutator for the property with the given name.
         *
         * @param name  the name of the 
         * @return null if the name is null; null if the name is not a 
         * {@link String}; null if no such property exists; null if the 
         * property is read-only; or the mutator method for that property
         */
        protected virtual java.lang.reflect.Method getWriteMethod(Object name)
        {
            return (java.lang.reflect.Method)writeMethods.get(name);
        }

        /**
         * Reinitializes this bean.  Called during {@link #setBean(Object)}.
         * Does introspection to find properties.
         */
        protected virtual void reinitialise()
        {
            readMethods.clear();
            writeMethods.clear();
            types.clear();
            initialise();
        }

        private void initialise()
        {
            if (getBean() == null) return;

            java.lang.Class beanClass = getBean().getClass();
            try
            {
                //BeanInfo beanInfo = Introspector.getBeanInfo( bean, null );
                java.beans.BeanInfo beanInfo = java.beans.Introspector.getBeanInfo(beanClass);
                java.beans.PropertyDescriptor[] propertyDescriptors = beanInfo.getPropertyDescriptors();
                if (propertyDescriptors != null)
                {
                    for (int i = 0; i < propertyDescriptors.Length; i++)
                    {
                        java.beans.PropertyDescriptor propertyDescriptor = propertyDescriptors[i];
                        if (propertyDescriptor != null)
                        {
                            String name = propertyDescriptor.getName();
                            java.lang.reflect.Method readMethod = propertyDescriptor.getReadMethod();
                            java.lang.reflect.Method writeMethod = propertyDescriptor.getWriteMethod();
                            java.lang.Class aType = propertyDescriptor.getPropertyType();

                            if (readMethod != null)
                            {
                                readMethods.put(name, readMethod);
                            }
                            if (writeMethod != null)
                            {
                                writeMethods.put(name, writeMethod);
                            }
                            types.put(name, aType);
                        }
                    }
                }
            }
            catch (java.beans.IntrospectionException e)
            {
                logWarn(e);
            }
        }

        /**
         * Called during a successful {@link #put(Object,Object)} operation.
         * Default implementation does nothing.  Override to be notified of
         * property changes in the bean caused by this map.
         *
         * @param key  the name of the property that changed
         * @param oldValue  the old value for that property
         * @param newValue  the new value for that property
         */
        protected virtual void firePropertyChange(Object key, Object oldValue, Object newValue)
        {
        }

        // Implementation classes
        //-------------------------------------------------------------------------

        /**
         * Map entry used by {@link BeanMap}.
         */
        protected internal class MyMapEntry : AbstractMapEntry
        {
            private BeanMap owner;

            /**
             * Constructs a new <code>MyMapEntry</code>.
             *
             * @param owner  the BeanMap this entry belongs to
             * @param key  the key for this entry
             * @param value  the value for this entry
             */
            protected internal MyMapEntry(BeanMap owner, Object key, Object value)
                : base(key, value)
            {
                this.owner = owner;
            }

            /**
             * Sets the value.
             *
             * @param value  the new value for the entry
             * @return the old value for the entry
             */
            public override Object setValue(Object value)
            {
                Object key = getKey();
                Object oldValue = owner.get(key);

                owner.put(key, value);
                Object newValue = owner.get(key);
                base.setValue(newValue);
                return oldValue;
            }
        }

        /**
         * Creates an array of parameters to pass to the given mutator method.
         * If the given object is not the right type to pass to the method 
         * directly, it will be converted using {@link #convertType(Class,Object)}.
         *
         * @param method  the mutator method
         * @param value  the value to pass to the mutator method
         * @return an array containing one object that is either the given value
         *   or a transformed value
         * @throws IllegalAccessException if {@link #convertType(Class,Object)}
         *   raises it
         * @throws IllegalArgumentException if any other exception is raised
         *   by {@link #convertType(Class,Object)}
         */
        protected Object[] createWriteMethodArguments(java.lang.reflect.Method method, Object value)
        {//throws IllegalAccessException, ClassCastException {            
            try
            {
                if (value != null)
                {
                    java.lang.Class[] types = method.getParameterTypes();
                    if (types != null && types.Length > 0)
                    {
                        java.lang.Class paramType = types[0];
                        if (!paramType.isAssignableFrom(value.getClass()))
                        {
                            value = convertType(paramType, value);
                        }
                    }
                }
                Object[] answer = { value };
                return answer;
            }
            catch (java.lang.reflect.InvocationTargetException e)
            {
                logInfo(e);
                throw new java.lang.IllegalArgumentException(e.getMessage());
            }
            catch (java.lang.InstantiationException e)
            {
                logInfo(e);
                throw new java.lang.IllegalArgumentException(e.getMessage());
            }
        }

        /**
         * Converts the given value to the given type.  First, reflection is
         * is used to find a public constructor declared by the given class 
         * that takes one argument, which must be the precise type of the 
         * given value.  If such a constructor is found, a new object is
         * created by passing the given value to that constructor, and the
         * newly constructed object is returned.<P>
         *
         * If no such constructor exists, and the given type is a primitive
         * type, then the given value is converted to a string using its 
         * {@link Object#toString() toString()} method, and that string is
         * parsed into the correct primitive type using, for instance, 
         * {@link Integer#valueOf(String)} to convert the string into an
         * <code>int</code>.<P>
         *
         * If no special constructor exists and the given type is not a 
         * primitive type, this method returns the original value.
         *
         * @param newType  the type to convert the value to
         * @param value  the value to convert
         * @return the converted value
         * @throws NumberFormatException if newType is a primitive type, and 
         *  the string representation of the given value cannot be converted
         *  to that type
         * @throws InstantiationException  if the constructor found with 
         *  reflection raises it
         * @throws InvocationTargetException  if the constructor found with
         *  reflection raises it
         * @throws IllegalAccessException  never
         * @throws IllegalArgumentException  never
         */
        protected Object convertType(java.lang.Class newType, Object value)
        {
            //throws InstantiationException, IllegalAccessException, IllegalArgumentException, InvocationTargetException {

            // try call constructor
            java.lang.Class[] types = { value.getClass() };
            try
            {
                java.lang.reflect.Constructor constructor = newType.getConstructor(types);
                Object[] arguments = { value };
                return constructor.newInstance(arguments);
            }
            catch (java.lang.NoSuchMethodException e)
            {
                // try using the transformers
                Transformer transformer = getTypeTransformer(newType);
                if (transformer != null)
                {
                    return transformer.transform(value);
                }
                return value;
            }
        }

        /**
         * Returns a transformer for the given primitive type.
         *
         * @param aType  the primitive type whose transformer to return
         * @return a transformer that will convert strings into that type,
         *  or null if the given type is not a primitive type
         */
        protected Transformer getTypeTransformer(java.lang.Class aType)
        {
            return (Transformer)defaultTransformers.get(aType);
        }

        /**
         * Logs the given exception to <code>java.lang.SystemJ.outJ</code>.  Used to display
         * warnings while accessing/mutating the bean.
         *
         * @param ex  the exception to log
         */
        protected void logInfo(Exception ex)
        {
            // Deliberately do not use LOG4J or Commons Logging to avoid dependencies
            java.lang.SystemJ.outJ.println("INFO: Exception: " + ex);
        }

        /**
         * Logs the given exception to <code>System.err</code>.  Used to display
         * errors while accessing/mutating the bean.
         *
         * @param ex  the exception to log
         */
        protected void logWarn(java.lang.Exception ex)
        {
            // Deliberately do not use LOG4J or Commons Logging to avoid dependencies
            java.lang.SystemJ.outJ.println("WARN: Exception: " + ex);
            ex.printStackTrace();
        }
    }
}