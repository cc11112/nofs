using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.lang {
    /**
     * Thrown when a waiting thread is activated before the condition it was waiting
     * for has been satisfied.
     */
    [Serializable]
    public class InterruptedException : Exception {

        private static readonly long serialVersionUID = 6700697376100628473L;

        /**
         * Constructs a new {@code InterruptedException} that includes the current
         * stack trace.
         */
        public InterruptedException() : base(){
        }

        /**
         * Constructs a new {@code InterruptedException} with the current stack
         * trace and the specified detail message.
         * 
         * @param detailMessage
         *            the detail message for this exception.
         */
        public InterruptedException(String detailMessage) : base (detailMessage) {
        }
    }
}
