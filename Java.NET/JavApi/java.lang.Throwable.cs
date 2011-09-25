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
// some implementation parts comes from Apache Harmony project
using System;
using System.Diagnostics;
using System.Text;

using java = biz.ritter.javapi;

namespace biz.ritter.javapi.lang
{
    public class Throwable : System.Exception, java.io.Serializable
    {

        /**
         * A fully-expanded representation of the stack trace.
         */
        private StackTraceElement[] stackTrace;
        /// <summary>
        /// Cause for this exception, if other it is.
        /// </summary>
        private Throwable cause = null;
        /// <summary>
        /// Create new Throwable with empty message and no cause
        /// </summary>
        public Throwable() : this ("")
        {
        }
        public Throwable (String message) : this (message, null) {}
        public Throwable(Throwable cause)
            : this(cause == null ? "" : cause.getMessage(), cause)
        {
        }
        public Throwable(String message, Throwable cause) : base(message) {
            this.cause = cause;
        }
        public virtual String getMessage()
        {
            return Message;
        }
        public virtual Throwable initCause(Throwable throwable)
        {
            if (cause == this)
            {
                if (throwable != this)
                {
                    cause = throwable;
                    return this;
                }
                throw new IllegalArgumentException("Cause cannot be the receiver");
            }
            throw new IllegalStateException("Cause already initialized");
        }
        public virtual String getLocalizedMessage()
        {
            return this.getMessage();
        }

        public virtual void printStackTrace()
        {
            this.printStackTrace(SystemJ.err);
//            Console.Error.WriteLine(Environment.StackTrace);
        }

        /**
         * Writes a printable representation of this {@code Throwable}'s stack trace
         * to the specified print stream. If the {@code Throwable} contains a
         * {@link #getCause() cause}, the method will be invoked recursively for
         * the nested {@code Throwable}.
         * 
         * @param err
         *            the stream to write the stack trace on.
         */
        public virtual void printStackTrace(PrintStream err) {
            // we want better OO and do not implement two time same, so we delegate to next
            this.printStackTrace(new java.io.PrintWriter(err,true));
        }

        /**
            * Writes a printable representation of this {@code Throwable}'s stack trace
            * to the specified print writer. If the {@code Throwable} contains a
            * {@link #getCause() cause}, the method will be invoked recursively for the
            * nested {@code Throwable}.
            * 
            * @param err
            *            the writer to write the stack trace on.
            */
        public virtual void printStackTrace(java.io.PrintWriter err) {
            err.println(this.ToString());
            // Don't use getStackTrace() as it calls clone()
            // Get stackTrace, in case stackTrace is reassigned
            StackTraceElement[] stack = getInternalStackTrace();
            foreach (StackTraceElement element in stack) {
                err.println("\tat " + element);
            }

            StackTraceElement[] parentStack = stack;
            Throwable throwable = getCause();
            while (throwable != null) {
                err.print("Caused by: ");
                err.println(throwable);
                StackTraceElement[] currentStack = throwable.getInternalStackTrace();
                int duplicates = countDuplicates(currentStack, parentStack);
                for (int i = 0; i < currentStack.Length - duplicates; i++) {
                    err.println("\tat " + currentStack[i]);
                }
                if (duplicates > 0) {
                    err.println("\t... " + duplicates + " more");
                }
                parentStack = currentStack;
                throwable = throwable.getCause();
            }
        }
        
        public override String ToString() {
            String msg = getLocalizedMessage();
            String name = this.GetType().Name;
            if (msg == null) {
                return name;
            }
            return new StringBuilder(name.length() + 2 + msg.length()).append(name).append(": ")
                    .append(msg).toString();
        }

        /**
         * Returns an array of StackTraceElements. Each StackTraceElement represents
         * a entry on the stack. Cache the stack trace in the stackTrace field,
         * returning the cached field when it has already been initialized.
         * 
         * @return an array of StackTraceElement representing the stack
         */
        private StackTraceElement[] getInternalStackTrace()
        {
            if (stackTrace == null)
            {
                stackTrace = getStackTraceImpl();
            }
            return stackTrace;
        }

        /// <summary>
        /// Create a new StackTraceElement [] 
        /// </summary>
        /// <returns></returns>
        private StackTraceElement[] getStackTraceImpl()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);
            
            StackTraceElement[] result = new StackTraceElement[st.FrameCount];
            for (int i = 0; i < result.Length; i++)
            {
                StackFrame sfe = st.GetFrame(i);
                String type = null;
                if (null != sfe.GetFileName())
                {
                    int startIndex = sfe.GetFileName().LastIndexOf(java.lang.SystemJ.getProperty("file.separator")) == -1 ? 0 : sfe.GetFileName().LastIndexOf(java.lang.SystemJ.getProperty("file.separator")) + 1;
                    int endIndex = sfe.GetFileName().LastIndexOf(".") == -1 ? 0 : sfe.GetFileName().LastIndexOf(".");
                    type = sfe.GetFileName().substring(startIndex, endIndex);
                }
                else
                {
                    type = "Unknown";
                }
                StackTraceElement ste = new StackTraceElement(
                    type,
                    sfe.GetMethod().Name,
                    sfe.GetFileName(),
                    sfe.GetFileLineNumber()
                    );
                result[i] = ste;
            }
            return result;
        }
        /**
         * Returns the cause of this {@code Throwable}, or {@code null} if there is
         * no cause.
         * 
         * @return Throwable this {@code Throwable}'s cause.
         */
        public virtual Throwable getCause()
        {
            if (cause == this)
            {
                return null;
            }
            return cause;
        }

        /**
         * Counts the number of duplicate stack frames, starting from the
         * end of the stack.
         * 
         * @param currentStack a stack to compare
         * @param parentStack a stack to compare
         * 
         * @return the number of duplicate stack frames.
         */
        private static int countDuplicates(StackTraceElement[] currentStack,
                StackTraceElement[] parentStack)
        {
            int duplicates = 0;
            int parentIndex = parentStack.Length;
            for (int i = currentStack.Length; --i >= 0 && --parentIndex >= 0; )
            {
                StackTraceElement parentFrame = parentStack[parentIndex];
                if (parentFrame.equals(currentStack[i]))
                {
                    duplicates++;
                }
                else
                {
                    break;
                }
            }
            return duplicates;
        }
    }
}

