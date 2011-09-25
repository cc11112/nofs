using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util
{
    [Serializable]
    public class GregorianCalendar : Calendar
    {
        /**
         * Value for the BC era.
         */
        public static readonly int BC = 0;

        /**
         * Value for the AD era.
         */
        public static readonly int AD = 1;

        public GregorianCalendar() : base() { }

        public GregorianCalendar (int year, int month, int day) : base()
        {
            this.delegateInstance = new DateTime (year,month,day);
        }
        public GregorianCalendar(int year, int month, int day, int hour, int minute, int second) : base()
        {
            this.delegateInstance = new DateTime(year, month, day, hour,minute,second);
        }
        public GregorianCalendar(int year, int month, int day, int hour, int minute) : base()
        {
            this.delegateInstance = new DateTime(year, month, day, hour, minute,0);
        }

        [NonSerialized]
        private int changeYear = 1582;
        public bool isLeapYear(int year)
        {
            if (year > changeYear)
            {
                return 0 == year % 4 && (0 != year % 100 || 0 == year % 400);
            }
            else
            {
                return 0 == year % 4;
            }
        }
        public override int get(int field)
        {
            if (Calendar.ERA == field)
            {
                return this.delegateInstance.Year < 0 ? GregorianCalendar.BC : GregorianCalendar.AD;
            }
            else
            {
                return base.get(field);
            }
        }

    }

}
