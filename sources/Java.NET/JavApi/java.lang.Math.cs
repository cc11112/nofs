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

namespace biz.ritter.javapi.lang
{
    public sealed class Math 
    {
        public static long min(long a, long b)
        {
            return System.Math.Min(a, b);
        }
        public static int min(int a, int b)
        {
            return System.Math.Min(a, b);
        }
        public static long max(long a, long b)
        {
            return System.Math.Max(a, b);
        }
        public static int max(int a, int b)
        {
            return System.Math.Max(a, b);
        }
        public static double log(double a)
        {
            return System.Math.Log(a);
        }
        public static double sqrt(double a)
        {
            return System.Math.Sqrt(a);
        }
        public static long abs(long a)
        {
            return System.Math.Abs(a);
        }
        public static int abs(int a)
        {
            return System.Math.Abs(a);
        }
        public static float abs(float a)
        {
            return System.Math.Abs(a);
        }
        public static double abs(double a)
        {
            return System.Math.Abs(a);
        }
        public static double log10(double a)
        {
            return System.Math.Log10(a);
        } 
    }
}
