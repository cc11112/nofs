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
     * Runtime exception thrown from functors.
     * If required, a root cause error can be wrapped within this one.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Stephen Colebourne
     */
    [Serializable]
    public class FunctorException : java.lang.RuntimeException
    {

        /**
         * Does JDK support nested exceptions
         * Basties note: we provide it!
         */
        private const bool JDK_SUPPORTS_NESTED = true;

        /*
        static FunctorException() {
            bool flag = false;
            try {
                java.lang.Throwable.class.getDeclaredMethod("getCause", new Class[0]);
                flag = true;
            } catch (java.lang.NoSuchMethodException ex) {
                flag = false;
            }
            JDK_SUPPORTS_NESTED = flag;
        }*/

        /**
         * Root cause of the exception
         */
        private readonly java.lang.Throwable rootCause;

        /**
         * Constructs a new <code>FunctorException</code> without specified
         * detail message.
         */
        public FunctorException()
            : base()
        {
            this.rootCause = null;
        }

        /**
         * Constructs a new <code>FunctorException</code> with specified
         * detail message.
         *
         * @param msg  the error message.
         */
        public FunctorException(String msg)
            : base(msg)
        {
            this.rootCause = null;
        }

        /**
         * Constructs a new <code>FunctorException</code> with specified
         * nested <code>Throwable</code> root cause.
         *
         * @param rootCause  the exception or error that caused this exception
         *                   to be thrown.
         */
        public FunctorException(java.lang.Throwable rootCause) :
            base((rootCause == null ? null : rootCause.getMessage()))
        {
            this.rootCause = rootCause;
        }

        /**
         * Constructs a new <code>FunctorException</code> with specified
         * detail message and nested <code>Throwable</code> root cause.
         *
         * @param msg        the error message.
         * @param rootCause  the exception or error that caused this exception
         *                   to be thrown.
         */
        public FunctorException(String msg, java.lang.Throwable rootCause)
            : base(msg)
        {
            this.rootCause = rootCause;
        }

        /**
         * Gets the cause of this throwable.
         * 
         * @return  the cause of this throwable, or <code>null</code>
         */
        public override java.lang.Throwable getCause()
        {
            return rootCause;
        }

        /**
         * Prints the stack trace of this exception to the standard error stream.
         */
        public override void printStackTrace()
        {
            printStackTrace(java.lang.SystemJ.err);
        }

        /**
         * Prints the stack trace of this exception to the specified stream.
         *
         * @param out  the <code>PrintStream</code> to use for output
         */
        public override void printStackTrace(java.lang.PrintStream outJ)
        {
            lock (outJ)
            {
                java.io.PrintWriter pw = new java.io.PrintWriter(outJ, false);
                printStackTrace(pw);
                // Flush the PrintWriter before it's GC'ed.
                pw.flush();
            }
        }

        /**
         * Prints the stack trace of this exception to the specified writer.
         *
         * @param out  the <code>PrintWriter</code> to use for output
         */
        public override void printStackTrace(java.io.PrintWriter outJ)
        {
            lock (outJ)
            {
                base.printStackTrace(outJ);
                if (rootCause != null && JDK_SUPPORTS_NESTED == false)
                {
                    outJ.print("Caused by: ");
                    rootCause.printStackTrace(outJ);
                }
            }
        }

    }
}