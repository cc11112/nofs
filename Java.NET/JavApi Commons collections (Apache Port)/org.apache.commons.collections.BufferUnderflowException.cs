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

namespace org.apache.commons.collections
{

    /**
     * The BufferUnderflowException is used when the buffer is already empty.
     * <p>
     * NOTE: From version 3.0, this exception extends NoSuchElementException.
     * 
     * @since Commons Collections 2.1
     * @version $Revision$ $Date$
     *
     * @author Avalon
     * @author Berin Loritsch
     * @author Jeff Turner
     * @author Paul Jack
     * @author Stephen Colebourne
     */
    public class BufferUnderflowException : java.util.NoSuchElementException
    {

        /** The root cause throwable */
        private readonly java.lang.Throwable throwable;

        /**
         * Constructs a new <code>BufferUnderflowException</code>.
         */
        public BufferUnderflowException()
            : base()
        {
            throwable = null;
        }

        /** 
         * Construct a new <code>BufferUnderflowException</code>.
         * 
         * @param message  the detail message for this exception
         */
        public BufferUnderflowException(String message)
            : this(message, null)
        {
        }

        /** 
         * Construct a new <code>BufferUnderflowException</code>.
         * 
         * @param message  the detail message for this exception
         * @param exception  the root cause of the exception
         */
        public BufferUnderflowException(String message, java.lang.Throwable exception)
            : base(message)
        {
            throwable = exception;
        }

        /**
         * Gets the root cause of the exception.
         *
         * @return the root cause
         */
        public override java.lang.Throwable getCause()
        {
            return throwable;
        }

    }
}