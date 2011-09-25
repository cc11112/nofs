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
 */
using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.lang{
    /**
     * Placeholder class for the Java keyword {@code void}.
     *
     * @since 1.1
     */
    public sealed class Void : Object
    {

        /**
         * The {@link Class} object that represents the primitive type {@code void}.
         */
        public static readonly java.lang.Class TYPE = lookupType();

        // Note: This can't be set to "void.class", since *that* is
        // defined to be "java.lang.Void.TYPE";

        private static java.lang.Class lookupType()
        {
            java.lang.Class voidType = null;
            try
            {
                java.lang.reflect.Method method = new java.lang.Class(typeof(java.lang.Runnable)).getMethod("run", new Class[0]); //$NON-NLS-1$
                voidType = method.getReturnType();
            }
            catch (Exception e)
            {
                throw new RuntimeException(e);
            }
            return voidType;
        }

        private Void()
        {
        }
    }
}