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
using org.apache.commons.collections;

namespace org.apache.commons.collections.functors
{

    /**
     * Closure implementation that executes a closure repeatedly until a condition is met,
     * like a do-while or while loop.
     * 
     * @since Commons Collections 3.0
     * @version $Revision$ $Date$
     *
     * @author Stephen Colebourne
     */
    public class WhileClosure : Closure, java.io.Serializable
    {

        /** Serial version UID */
        private static readonly long serialVersionUID = -3110538116913760108L;

        /** The test condition */
        private readonly Predicate iPredicate;
        /** The closure to call */
        private readonly Closure iClosure;
        /** The flag, true is a do loop, false is a while */
        private readonly bool iDoLoop;

        /**
         * Factory method that performs validation.
         * 
         * @param predicate  the predicate used to evaluate when the loop terminates, not null
         * @param closure  the closure the execute, not null
         * @param doLoop  true to act as a do-while loop, always executing the closure once
         * @return the <code>while</code> closure
         * @throws IllegalArgumentException if the predicate or closure is null
         */
        public static Closure getInstance(Predicate predicate, Closure closure, bool doLoop)
        {
            if (predicate == null)
            {
                throw new java.lang.IllegalArgumentException("Predicate must not be null");
            }
            if (closure == null)
            {
                throw new java.lang.IllegalArgumentException("Closure must not be null");
            }
            return new WhileClosure(predicate, closure, doLoop);
        }

        /**
         * Constructor that performs no validation.
         * Use <code>getInstance</code> if you want that.
         * 
         * @param predicate  the predicate used to evaluate when the loop terminates, not null
         * @param closure  the closure the execute, not null
         * @param doLoop  true to act as a do-while loop, always executing the closure once
         */
        public WhileClosure(Predicate predicate, Closure closure, bool doLoop)
            : base()
        {
            iPredicate = predicate;
            iClosure = closure;
            iDoLoop = doLoop;
        }

        /**
         * Executes the closure until the predicate is false.
         * 
         * @param input  the input object
         */
        public void execute(Object input)
        {
            if (iDoLoop)
            {
                iClosure.execute(input);
            }
            while (iPredicate.evaluate(input))
            {
                iClosure.execute(input);
            }
        }

        /**
         * Gets the predicate in use.
         * 
         * @return the predicate
         * @since Commons Collections 3.1
         */
        public Predicate getPredicate()
        {
            return iPredicate;
        }

        /**
         * Gets the closure.
         * 
         * @return the closure
         * @since Commons Collections 3.1
         */
        public Closure getClosure()
        {
            return iClosure;
        }

        /**
         * Is the loop a do-while loop.
         * 
         * @return true is do-while, false if while
         * @since Commons Collections 3.1
         */
        public bool isDoLoop()
        {
            return iDoLoop;
        }

    }
}