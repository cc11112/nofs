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
     * The {@code TimerTask} class represents a task to run at a specified time. The task
     * may be run once or repeatedly.
     * 
     * @see Timer
     * @see java.lang.Object#wait(long)
     */
    public abstract class TimerTask : java.lang.Runnable
    {
        /* Lock object for synchronization. It's also used by Timer class. */
        protected internal Object lockJ = new Object();

        /* If timer was cancelled */
        protected internal bool cancelled;

        /* Slots used by Timer */
        protected internal long when;

        protected internal long period;

        protected internal bool fixedRate;

        /*
         * The time when task will be executed, or the time when task was launched
         * if this is task in progress.
         */
        private long scheduledTime;

        /*
         * Method called from the Timer for synchronized getting of when field.
         */
        protected internal virtual long getWhen()
        {
            lock (lockJ)
            {
                return when;
            }
        }

        /*
         * Method called from the Timer object when scheduling an event @param time
         */
        protected internal virtual void setScheduledTime(long time)
        {
            lock (lockJ)
            {
                scheduledTime = time;
            }
        }

        /*
         * Is TimerTask scheduled into any timer?
         * 
         * @return {@code true} if the timer task is scheduled, {@code false}
         * otherwise.
         */
        protected internal virtual bool isScheduled()
        {
            lock (lockJ)
            {
                return when > 0 || scheduledTime > 0;
            }
        }

        /**
         * Creates a new {@code TimerTask}.
         */
        protected TimerTask():base()
        {
        }

        /**
         * Cancels the {@code TimerTask} and removes it from the {@code Timer}'s queue. Generally, it
         * returns {@code false} if the call did not prevent a {@code TimerTask} from running at
         * least once. Subsequent calls have no effect.
         * 
         * @return {@code true} if the call prevented a scheduled execution
         *         from taking place, {@code false} otherwise.
         */
        public virtual bool cancel()
        {
            lock (lockJ)
            {
                bool willRun = !cancelled && when > 0;
                cancelled = true;
                return willRun;
            }
        }

        /**
         * Returns the scheduled execution time. If the task execution is in
         * progress it returns the execution time of the ongoing task. Tasks which
         * have not yet run return an undefined value.
         * 
         * @return the most recent execution time.
         */
        public virtual long scheduledExecutionTime()
        {
            lock (lockJ)
            {
                return scheduledTime;
            }
        }

        /**
         * The task to run should be specified in the implementation of the {@code run()}
         * method.
         */
        public abstract void run();

    }
}

