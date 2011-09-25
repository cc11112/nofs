using System;
using java = biz.ritter.javapi;

namespace javapi.sample
{
    class SampleWorkingWithException
    {

        static void Main()
        {
            try
            {
                ExceptionSubclass thrower = new ExceptionSubclass();
                thrower.toString();
            }
            catch (java.lang.Throwable hereIAm)
            {
                hereIAm.printStackTrace();
                hereIAm.printStackTrace(java.lang.SystemJ.outJ);
            }
            catch (System.Exception whereAreYou)
            {
                Console.Error.WriteLine(whereAreYou.StackTrace);
            }
        }


    }

    class ExceptionSubclass
    {
        internal void method()
        {
            this.ToString();
        }

        public override String ToString()
        {
            return "Hashcode: " + GetHashCode();
        }
        public override int GetHashCode()
        {
            throw new java.lang.UnsupportedOperationException(new java.lang.RuntimeException("I am here!"));
        }
    }
}
