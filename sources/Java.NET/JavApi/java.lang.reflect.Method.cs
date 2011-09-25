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
 *  Copyright © 2011 Sebastian Ritter
 */
using System;
using System.Reflection;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.lang.reflect
{
    public class Method
    {
        private MethodInfo delegateInstance;

        internal Method (MethodInfo mi) {
            this.delegateInstance = mi;
        }

        public Object invoke(Object obj, params Object [] p)
        {
            try
            {
                return this.delegateInstance.Invoke(obj, p);
            }
            catch (MethodAccessException mae)
            {
                throw new java.lang.IllegalAccessException(mae.Message);
            }
            catch (TargetInvocationException tie)
            {
                throw new InvocationTargetException(tie);
            }
        }

        public Class getReturnType()
        {
            if (typeof(void).equals(this.delegateInstance.ReturnType))
            {
                return new java.lang.Class(typeof(java.lang.Void));
            }
            else
                return new java.lang.Class(this.delegateInstance.ReturnType);
        }

        public int getModifiers()
        {
            int returnValue = 0;
            returnValue += this.delegateInstance.IsPublic? Modifier.PUBLIC :0;
            returnValue += this.delegateInstance.IsAbstract ? Modifier.ABSTRACT : 0;
            returnValue += this.delegateInstance.IsFinal ? Modifier.FINAL : 0;
            returnValue += this.delegateInstance.IsPrivate ? Modifier.PRIVATE : 0;
            returnValue += this.delegateInstance.IsStatic ? Modifier.STATIC : 0;

            return returnValue;
        }

        public java.lang.Class[] getParameterTypes()
        {
            ParameterInfo[] pi = this.delegateInstance.GetParameters();
            java.lang.Class[] returnValue = new java.lang.Class[pi.Length];
            for (int i = 0; i < pi.Length; i++)
            {
                returnValue[i] = new java.lang.Class(pi[i].ParameterType);
            }
            return returnValue;
        }

        public String getName()
        {
            return delegateInstance.Name;
        }
    }
}
