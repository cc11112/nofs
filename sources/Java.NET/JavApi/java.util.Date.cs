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
using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util
{
    public class Date : java.io.Serializable, java.lang.Cloneable, java.lang.Comparable<Date>
    {
        [NonSerialized]
        internal long milliseconds;

        public Date()
        {
            this.milliseconds = java.lang.SystemJ.currentTimeMillis();
        }
        public Date (long timeInMillisSince_1970_01_01) {
            this.milliseconds = timeInMillisSince_1970_01_01;
        }
        public virtual bool after (Date date) {
            return this.milliseconds > date.milliseconds;
        }
        public virtual bool before(Date date)
        {
            return this.milliseconds < date.milliseconds;
        }
        public int compareTo(Date date)
        {
            if (this.after(date)) return 1;
            if (this.before(date)) return -1;
            return 0;
        }
        public override bool Equals(object obj)
        {
            return (this == obj) || ((obj is Date) && (this.milliseconds == ((Date)obj).milliseconds));
        }
        public virtual long getTime()
        {
            return this.milliseconds;
        }
        public virtual void setTime(long timeInMillisSince_1970_01_01)
        {
            this.milliseconds = timeInMillisSince_1970_01_01;
        }
        private String toTwoDigits(int digit)
        {
            if (10 > digit)
            {
                return "0" + digit;
            }
            else {
                return ""+digit;
            }
        }
        private static int zone(String text)
        {
            if (text.equals("EST"))
            { 
                return -5;
            }
            if (text.equals("EDT"))
            { 
                return -4;
            }
            if (text.equals("CST"))
            { 
                return -6;
            }
            if (text.equals("CDT"))
            { 
                return -5;
            }
            if (text.equals("MST"))
            { 
                return -7;
            }
            if (text.equals("MDT"))
            { 
                return -6;
            }
            if (text.equals("PST"))
            { 
                return -8;
            }
            if (text.equals("PDT"))
            { 
                return -7;
            }
            return 0;
        }

        public Object clone()
        {
            Date clone = new Date(this.milliseconds);
            return clone;
        }

        public override int GetHashCode()
        {
            return (int)this.milliseconds;
        }
    }
}
