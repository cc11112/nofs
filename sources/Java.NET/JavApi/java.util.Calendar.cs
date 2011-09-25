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
    public abstract class Calendar : java.io.Serializable, java.lang.Cloneable, java.lang.Comparable<Calendar>
    {

        #region Constant fields
        /**
         * Field number for {@code get} and {@code set} indicating the
         * era, e.g., AD or BC in the Julian calendar. This is a calendar-specific
         * value; see subclass documentation.
         *
         * @see GregorianCalendar#AD
         * @see GregorianCalendar#BC
         */
        public static readonly int ERA = 0;

        /**
         * Field number for {@code get} and {@code set} indicating the
         * year. This is a calendar-specific value; see subclass documentation.
         */
        public static readonly int YEAR = 1; 

        /**
         * Field number for {@code get} and {@code set} indicating the
         * month. This is a calendar-specific value. The first month of the year is
         * {@code JANUARY}; the last depends on the number of months in a
         * year.
         *
         * @see #JANUARY
         * @see #FEBRUARY
         * @see #MARCH
         * @see #APRIL
         * @see #MAY
         * @see #JUNE
         * @see #JULY
         * @see #AUGUST
         * @see #SEPTEMBER
         * @see #OCTOBER
         * @see #NOVEMBER
         * @see #DECEMBER
         * @see #UNDECIMBER
         */
        public static readonly int MONTH = 2;

        /**
         * Field number for {@code get} and {@code set} indicating the
         * week number within the current year. The first week of the year, as
         * defined by {@code getFirstDayOfWeek()} and
         * {@code getMinimalDaysInFirstWeek()}, has value 1. Subclasses
         * define the value of {@code WEEK_OF_YEAR} for days before the first
         * week of the year.
         *
         * @see #getFirstDayOfWeek
         * @see #getMinimalDaysInFirstWeek
         */
        public static readonly int WEEK_OF_YEAR = 3;

        /**
         * Field number for {@code get} and {@code set} indicating the
         * week number within the current month. The first week of the month, as
         * defined by {@code getFirstDayOfWeek()} and
         * {@code getMinimalDaysInFirstWeek()}, has value 1. Subclasses
         * define the value of {@code WEEK_OF_MONTH} for days before the
         * first week of the month.
         *
         * @see #getFirstDayOfWeek
         * @see #getMinimalDaysInFirstWeek
         */
        public static readonly int WEEK_OF_MONTH = 4;

        /**
         * Field number for {@code get} and {@code set} indicating the
         * day of the month. This is a synonym for {@code DAY_OF_MONTH}. The
         * first day of the month has value 1.
         *
         * @see #DAY_OF_MONTH
         */
        public static readonly int DATE = 5;

        /**
         * Field number for {@code get} and {@code set} indicating the
         * day of the month. This is a synonym for {@code DATE}. The first
         * day of the month has value 1.
         *
         * @see #DATE
         */
        public static readonly int DAY_OF_MONTH = 5;

        /**
         * Field number for {@code get} and {@code set} indicating the
         * day number within the current year. The first day of the year has value
         * 1.
         */
        public static readonly int DAY_OF_YEAR = 6;

        /**
         * Field number for {@code get} and {@code set} indicating the
         * day of the week. This field takes values {@code SUNDAY},
         * {@code MONDAY}, {@code TUESDAY}, {@code WEDNESDAY},
         * {@code THURSDAY}, {@code FRIDAY}, and
         * {@code SATURDAY}.
         *
         * @see #SUNDAY
         * @see #MONDAY
         * @see #TUESDAY
         * @see #WEDNESDAY
         * @see #THURSDAY
         * @see #FRIDAY
         * @see #SATURDAY
         */
        public static readonly int DAY_OF_WEEK = 7;

        /**
         * Field number for {@code get} and {@code set} indicating the
         * ordinal number of the day of the week within the current month. Together
         * with the {@code DAY_OF_WEEK} field, this uniquely specifies a day
         * within a month. Unlike {@code WEEK_OF_MONTH} and
         * {@code WEEK_OF_YEAR}, this field's value does <em>not</em>
         * depend on {@code getFirstDayOfWeek()} or
         * {@code getMinimalDaysInFirstWeek()}. {@code DAY_OF_MONTH 1}
         * through {@code 7} always correspond to <code>DAY_OF_WEEK_IN_MONTH
         * 1</code>;
         * {@code 8} through {@code 15} correspond to
         * {@code DAY_OF_WEEK_IN_MONTH 2}, and so on.
         * {@code DAY_OF_WEEK_IN_MONTH 0} indicates the week before
         * {@code DAY_OF_WEEK_IN_MONTH 1}. Negative values count back from
         * the end of the month, so the last Sunday of a month is specified as
         * {@code DAY_OF_WEEK = SUNDAY, DAY_OF_WEEK_IN_MONTH = -1}. Because
         * negative values count backward they will usually be aligned differently
         * within the month than positive values. For example, if a month has 31
         * days, {@code DAY_OF_WEEK_IN_MONTH -1} will overlap
         * {@code DAY_OF_WEEK_IN_MONTH 5} and the end of {@code 4}.
         *
         * @see #DAY_OF_WEEK
         * @see #WEEK_OF_MONTH
         */
        public static readonly int DAY_OF_WEEK_IN_MONTH = 8;

        /**
         * Field number for {@code get} and {@code set} indicating
         * whether the {@code HOUR} is before or after noon. E.g., at
         * 10:04:15.250 PM the {@code AM_PM} is {@code PM}.
         *
         * @see #AM
         * @see #PM
         * @see #HOUR
         */
        public static readonly int AM_PM = 9;
        /**
         * Value of the {@code AM_PM} field indicating the period of the day
         * from midnight to just before noon.
         */
        public static readonly int AM = 0;

        /**
         * Value of the {@code AM_PM} field indicating the period of the day
         * from noon to just before midnight.
         */
        public static readonly int PM = 1;

        /**
         * Field number for {@code get} and {@code set} indicating the
         * hour of the morning or afternoon. {@code HOUR} is used for the
         * 12-hour clock. E.g., at 10:04:15.250 PM the {@code HOUR} is 10.
         *
         * @see #AM_PM
         * @see #HOUR_OF_DAY
         */
        public static readonly int HOUR = 10;

        /**
         * Field number for {@code get} and {@code set} indicating the
         * hour of the day. {@code HOUR_OF_DAY} is used for the 24-hour
         * clock. E.g., at 10:04:15.250 PM the {@code HOUR_OF_DAY} is 22.
         *
         * @see #HOUR
         */
        public static readonly int HOUR_OF_DAY = 11;

        /**
         * Field number for {@code get} and {@code set} indicating the
         * minute within the hour. E.g., at 10:04:15.250 PM the {@code MINUTE}
         * is 4.
         */
        public static readonly int MINUTE = 12;

        /**
         * Field number for {@code get} and {@code set} indicating the
         * second within the minute. E.g., at 10:04:15.250 PM the
         * {@code SECOND} is 15.
         */
        public static readonly int SECOND = 13;

        /**
         * Field number for {@code get} and {@code set} indicating the
         * millisecond within the second. E.g., at 10:04:15.250 PM the
         * {@code MILLISECOND} is 250.
         */
        public static readonly int MILLISECOND = 14;
        #endregion

        protected DateTime delegateInstance;

        protected Calendar () {
            this.delegateInstance = new DateTime ();
        }

        public static Calendar getInstance () {
            return new GregorianCalendar ();
        }

        public virtual Object clone()
        {
            Calendar clone = getInstance();
            clone.delegateInstance = this.delegateInstance;
            return clone;
        }

        public int compareTo(Calendar cal)
        {
            return this.delegateInstance.CompareTo(cal.delegateInstance);
        }
        public virtual int get(int field)
        {
            if (YEAR == field)
            {
                return this.delegateInstance.Year;
            }
            else if (MONTH == field)
            {
                return this.delegateInstance.Month;
            }
            else if (DAY_OF_MONTH == field)
            {
                return this.delegateInstance.Day;
            }
            else if (DAY_OF_WEEK == field)
            {
                return this.delegateInstance.DayOfWeek.toInt();
            }
            else if (DAY_OF_YEAR == field)
            {
                return this.delegateInstance.DayOfYear;
            }
            else if (Calendar.AM_PM == field)
            {
                return this.delegateInstance.Hour > 11 ? Calendar.PM : Calendar.AM;
            }
            else if (Calendar.HOUR == field)
            {
                return this.delegateInstance.Hour > 11 ? this.delegateInstance.Hour - 12 : this.delegateInstance.Hour;
            }
            else if (Calendar.HOUR_OF_DAY == field)
            {
                return this.delegateInstance.Hour;
            }
            else if (Calendar.MILLISECOND == field)
            {
                return this.delegateInstance.Millisecond;
            }
            else if (Calendar.MINUTE == field)
            {
                return this.delegateInstance.Minute;
            }
            else if (Calendar.SECOND == field)
            {
                return this.delegateInstance.Second;
            }
            throw new java.lang.ArrayIndexOutOfBoundsException ();
        }

        public void set(int year, int month, int date, int hourOfDay, int minute)
        {
            this.set(year, month, date, hourOfDay, minute, 0);
        }
        public void set(int year, int month, int date)
        {
            this.set(year, month, date, 0, 0, 0);
        }
        public void set(int year, int month, int date, int hourOfDay, int minute, int second)
        {
            this.delegateInstance = new DateTime (year, month+1,date,hourOfDay,minute,second,0);
        }
        public void setTimeInMillis(long time)
        {
            this.delegateInstance = new DateTime(1970, 1, 1).AddMilliseconds(time);
        }
        public void setTime(Date d)
        {
            // Same as setTime in millis...
            // first we need recompute Java / Unix time to .net Time
            // then set the time to delegate object
            this.delegateInstance = new DateTime(1970, 1, 1).AddMilliseconds(d.milliseconds);
        }
        public Date getTime()
        {
            // first we get the time from delegate object (as Ticks)
            // 219,338,580,000,000,000 nanoseconds
            //   2,193,385,800,000,000 ticks
            //         219,338,580,000 milliseconds
            //             219,338,580.00 seconds
            // when we recompute this to Java / Unix time
            TimeSpan timeDiff = this.delegateInstance - new DateTime(1970, 1, 1);
            long unixTime = (long)timeDiff.TotalMilliseconds;

            return new Date(unixTime);
        }
    }
}
