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

namespace biz.ritter.javapi.util
{
    /**
     * {@code EventObject}s represent events. Typically applications subclass this class to
     * add event specific information.
     * 
     * @see EventListener
     */
    [Serializable]
    public class EventObject : java.io.Serializable {
    
        private static readonly long serialVersionUID = 5516075349620653480L;

        /**
         * The event source.
         */
        [NonSerialized]
        protected Object source;

        /**
         * Constructs a new instance of this class.
         * 
         * @param source
         *            the object which fired the event.
         */
        public EventObject(Object source) {
            if (source != null) {
                this.source = source;
            } else {
                throw new java.lang.IllegalArgumentException(new java.lang.NullPointerException());
            }
        }

        /**
         * Returns the event source.
         * 
         * @return the object which fired the event.
         */
        public Object getSource() {
            return source;
        }

        /**
         * Returns the string representation of this {@code EventObject}.
         * 
         * @return the string representation of this {@code EventObject}.
         */
        public override String ToString() {
            return this.GetType().Name + "[source=" + this.getSource() + ']'; //$NON-NLS-1$
        }
    }
}
