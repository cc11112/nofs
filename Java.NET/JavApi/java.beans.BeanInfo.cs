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

    public interface BeanInfo
    {
        /*
        public const int ICON_COLOR_16x16 = 1;

        public const int ICON_COLOR_32x32 = 2;

        public const int ICON_MONO_16x16 = 3;

        public const int ICON_MONO_32x32 = 4;
        */
        PropertyDescriptor[] getPropertyDescriptors();
        /*
            public MethodDescriptor[] getMethodDescriptors();

            public EventSetDescriptor[] getEventSetDescriptors();

            public BeanInfo[] getAdditionalBeanInfo();
        */
        BeanDescriptor getBeanDescriptor();
        /*
            public Image getIcon(int iconKind);

            public int getDefaultPropertyIndex();

            public int getDefaultEventIndex();
        */
    }
}