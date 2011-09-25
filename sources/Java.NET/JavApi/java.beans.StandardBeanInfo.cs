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
    public class StandardBeanInfo : SimpleBeanInfo
    {
        private int defaultPropertyIndex = -1;

        private static PropertyComparator comparator = new PropertyComparator();
        private BeanDescriptor beanDescriptor = null;

        internal BeanInfo[] additionalBeanInfo = null;
        private java.lang.Class beanClass;

        private bool explicitMethods = false;

        private bool explicitProperties = false;

        private bool explicitEvents = false;

        private BeanInfo explicitBeanInfo = null;

        private EventSetDescriptor[] events = null;

        private MethodDescriptor[] methods = null;

        private PropertyDescriptor[] properties = null;

        private Object[] icon = new Object[4];

        internal StandardBeanInfo(java.lang.Class beanClass, BeanInfo explicitBeanInformations, java.lang.Class stopClass)
        {//throws IntrospectionException {
            this.beanClass = beanClass;
        }
        internal void mergeBeanInfo(BeanInfo beanInfo, bool force) { }

        public override BeanDescriptor getBeanDescriptor()
        {
            if (beanDescriptor == null)
            {
                if (explicitBeanInfo != null)
                {
                    beanDescriptor = explicitBeanInfo.getBeanDescriptor();
                }
                if (beanDescriptor == null)
                {
                    beanDescriptor = new BeanDescriptor(beanClass);
                }
            }
            return beanDescriptor;
        }
    class PropertyComparator :
            java.util.Comparator<PropertyDescriptor> {
        public int compare(PropertyDescriptor object1,
                PropertyDescriptor object2) {
            return object1.getName().compareTo(object2.getName());
        }
        public bool equals(Object o)
        {
            return this == o;
        }
    }

        // TODO
        internal void init()
        {
            if (this.events == null)
            {
                events = new EventSetDescriptor[0];
            }
            if (this.properties == null)
            {
                this.properties = new PropertyDescriptor[0];
            }

            if (properties != null)
            {
                String defaultPropertyName = (defaultPropertyIndex != -1 ? properties[defaultPropertyIndex]
                        .getName()
                        : null);
                java.util.Arrays<PropertyDescriptor>.sort(properties, comparator);
                if (null != defaultPropertyName)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        if (defaultPropertyName.equals(properties[i].getName()))
                        {
                            defaultPropertyIndex = i;
                            break;
                        }
                    }
                }
            }
        }
    }
}
