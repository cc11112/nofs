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

namespace biz.ritter.javapi.util
{
    /**
     * A {@code MissingResourceException} is thrown by ResourceBundle when a
     * resource bundle cannot be found or a resource is missing from a resource
     * bundle.
     * 
     * @see ResourceBundle
     * @see java.lang.RuntimeException
     */
    public class MissingResourceException : java.lang.RuntimeException {

        private static long serialVersionUID = -4876345176062000401L;

        protected internal String className, key;

        /**
         * Constructs a new {@code MissingResourceException} with the stack trace,
         * message, the class name of the resource bundle and the name of the
         * missing resource filled in.
         * 
         * @param detailMessage
         *           the detail message for the exception.
         * @param className
         *           the class name of the resource bundle.
         * @param resourceName
         *           the name of the missing resource.
         */
        public MissingResourceException(String detailMessage, String className,
                String resourceName) : base (detailMessage) {
            this.className = className;
            this.key = resourceName;
        }

        /**
         * Returns the class name of the resource bundle from which a resource could
         * not be found, or in the case of a missing resource, the name of the
         * missing resource bundle.
         * 
         * @return the class name of the resource bundle.
         */
        public String getClassName() {
            return className;
        }

        /**
         * Returns the name of the missing resource, or an empty string if the
         * resource bundle is missing.
         * 
         * @return the name of the missing resource.
         */
        public String getKey() {
            return key;
        }

    }
}
