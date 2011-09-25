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
    public class BeanDescriptor : FeatureDescriptor
    {
        private java.lang.Class beanClass;

        private java.lang.Class customizerClass;

        /**
         * <p>
         * Constructs an instance with the bean's {@link Class} and a customizer
         * {@link Class}. The descriptor's {@link #getName()} is set as the
         * unqualified name of the <code>beanClass</code>.
         * </p>
         * 
         * @param beanClass
         *            The bean's Class.
         * @param customizerClass
         *            The bean's customizer Class.
         */
        public BeanDescriptor(java.lang.Class beanClass, java.lang.Class customizerClass)
        {
            if (beanClass == null)
            {
                throw new java.lang.NullPointerException();
            }
            setName(getShortClassName(beanClass));
            this.beanClass = beanClass;
            this.customizerClass = customizerClass;
        }

        /**
         * <p>
         * Constructs an instance with the bean's {@link Class}. The descriptor's
         * {@link #getName()} is set as the unqualified name of the
         * <code>beanClass</code>.
         * </p>
         * 
         * @param beanClass
         *            The bean's Class.
         */
        public BeanDescriptor(java.lang.Class beanClass) :
            this(beanClass, null)
        {
        }

        /**
         * <p>
         * Gets the bean's customizer {@link Class}/
         * </p>
         * 
         * @return A {@link Class} instance or <code>null</code>.
         */
        public java.lang.Class getCustomizerClass()
        {
            return customizerClass;
        }

        /**
         * <p>
         * Gets the bean's {@link Class}.
         * </p>
         * 
         * @return A {@link Class} instance.
         */
        public java.lang.Class getBeanClass()
        {
            return beanClass;
        }

        /**
         * <p>
         * Utility method for getting the unqualified name of a {@link Class}.
         * </p>
         * 
         * @param leguminaClass
         *            The Class to get the name from.
         * @return A String instance or <code>null</code>.
         */
        private String getShortClassName(java.lang.Class leguminaClass)
        {
            if (leguminaClass == null)
            {
                return null;
            }
            String beanClassName = leguminaClass.getName();
            int lastIndex = beanClassName.lastIndexOf("."); //$NON-NLS-1$
            return (lastIndex == -1) ? beanClassName : beanClassName.substring(lastIndex + 1);
        }

    }
}