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

namespace biz.ritter.javapi.lang.refj
{

    /**
     * The {@code ReferenceQueue} is the container on which reference objects are
     * enqueued when the garbage collector detects the reachability type specified
     * for the referent.
     *
     * @since 1.2
     */
    public class ReferenceQueue<T> {
    
        private static readonly int DEFAULT_QUEUE_SIZE = 128;
    
        private Reference<T>[] references;

        private int head;
    
        private int tail;

        private bool empty;
    
        /**
         * Constructs a new instance of this class.
         */
        public ReferenceQueue() {
            references = newArray(DEFAULT_QUEUE_SIZE);
            head = 0;
            tail = 0;
            empty = true;
        }
    
        private Reference<T>[] newArray(int size) {
            return new Reference<T>[size];
        }

        /**
         * Returns the next available reference from the queue, removing it in the
         * process. Does not wait for a reference to become available.
         *
         * @return the next available reference, or {@code null} if no reference is
         *         immediately available
         */
        public Reference<T> poll() {
            Reference<T> refJ;

            lock (this) {
                if (empty) {
                    return null;
                }
                refJ = references[head++];
                refJ.dequeue();
                if (head == references.Length) {
                    head = 0;
                }
                if (head == tail) {
                    empty = true;
                }
            }
            return refJ;
        }

        /**
         * Returns the next available reference from the queue, removing it in the
         * process. Waits indefinitely for a reference to become available.
         *
         * @return the next available reference
         *
         * @throws InterruptedException
         *             if the blocking call was interrupted for some reason
         */
        public Reference<T> remove() //throws InterruptedException 
        {
            return remove(0L);
        }

        /**
         * Returns the next available reference from the queue, removing it in the
         * process. Waits for a reference to become available or the given timeout
         * period to elapse, whichever happens first.
         *
         * @param timeout
         *            maximum time (in ms) to spend waiting for a reference object
         *            to become available. A value of zero results in the method
         *            waiting indefinitely.
         * @return the next available reference, or {@code null} if no reference
         *         becomes available within the timeout period
         * @throws IllegalArgumentException
         *             if the wait period is negative.
         * @throws InterruptedException
         *             if the blocking call was interrupted for some reason
         */
        public Reference<T> remove(long timeout) //throws IllegalArgumentException,InterruptedException 
        {
            if (timeout < 0) {
                throw new IllegalArgumentException();
            }

            Reference<T> refJ;
            lock (this) {
                if (empty) {
                    System.Threading.Monitor.Wait(timeout);// wait(timeout);
                    if (empty) {
                        return null;
                    }
                }
                refJ = references[head++];
                refJ.dequeue();
                if (head == references.Length) {
                    head = 0;
                }
                if (head == tail) {
                    empty = true;
                } else {
                    System.Threading.Monitor.PulseAll(this);// notifyAll();
                }
            }
            return refJ;
        }

        /**
         * Enqueue the reference object on the receiver.
         *
         * @param reference
         *            reference object to be enqueued.
         * @return boolean true if reference is enqueued. false if reference failed
         *         to enqueue.
         */
        protected internal bool enqueue(Reference<T> reference) {
            lock (this) {
                if (!empty && head == tail) {
                    /* Queue is full - grow */
                    int newQueueSize = (int) (references.Length * 1.10);
                    Reference<T>[] newQueue = newArray(newQueueSize);
                    java.lang.SystemJ.arraycopy(references, head, newQueue, 0, references.Length - head);
                    if (tail > 0) {
                        java.lang.SystemJ.arraycopy(references, 0, newQueue, references.Length - head, tail);
                    }
                    head = 0;
                    tail = references.Length;
                    references = newQueue;
                }
                references[tail++] = reference;
                if (tail == references.Length) {
                    tail = 0;
                }
                empty = false;
                System.Threading.Monitor.PulseAll(this); //notifyAll();
            }
            return true;
        }
    }
}
