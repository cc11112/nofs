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
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util
{

    /// <summary>
    /// Abstract wrapper class for <code>EventListener</code> interface.
    /// </summary>
    /// <see cref="EventListener"/>
    /// <see cref="EventObject"/>
    public abstract class EventListenerProxy : EventListener {

        private readonly EventListener delegateInstance;

        ///<summary>
        /// Create a new wrapper instance with given object.
        ///</summary>
        ///<param name="objectToProxy">EventListener for proxy instance.</param>
        public EventListenerProxy(EventListener objectToProxy) {
            if (null == objectToProxy) throw new java.lang.NullPointerException();
            this.delegateInstance = objectToProxy;
        }

        /// <summary>
        /// Returns the wrapped EventListener instance
        /// </summary>
        /// <returns>EventListener instance</returns>
        public EventListener getListener() {
            return this.delegateInstance;
        }
    }
}
