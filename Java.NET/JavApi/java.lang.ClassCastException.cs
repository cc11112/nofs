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

namespace biz.ritter.javapi.lang
{

    /**
     * Thrown when a program attempts to cast a an object to a type with which it is
     * not compatible.
     */
    [Serializable]
    public class ClassCastException : RuntimeException {
        private static readonly long serialVersionUID = -9223365651070458532L;

        /**
         * Constructs a new {@code ClassCastException} that includes the current
         * stack trace.
         */
        public ClassCastException() : base () {
        }

        /**
         * Constructs a new {@code ClassCastException} with the current stack trace
         * and the specified detail message.
         * 
         * @param detailMessage
         *            the detail message for this exception.
         */
        public ClassCastException(String detailMessage) : base (detailMessage) {
        }

        /**
         * Constructs a new {@code ClassCastException} with the current stack trace
         * and a detail message based on the source and target class.
         * 
         * @param instanceClass
         *            the class being cast from.
         * @param castClass
         *            the class being cast to.
         */
        protected internal ClassCastException(Type instanceClass, Type castClass) : base ("Class "+instanceClass.FullName+" cannot cast to "+castClass.FullName) {
        }
    }
}
