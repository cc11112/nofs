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

namespace javapi.sample.util
{
    public class SampleUsingCollection
    {
        static void Main()
        {
            java.util.ArrayList<int> primeNumbers = new java.util.ArrayList<int>();
            primeNumbers.add(1);
            primeNumbers.add(2);
            primeNumbers.add(3);
            primeNumbers.add(5);
            primeNumbers.add(7);

            java.lang.SystemJ.outJ.println("Work with all collection elements");

            java.lang.SystemJ.outJ.println("Using collection with for loop");
            for (int i = 0; i < primeNumbers.size(); i++)
                java.lang.SystemJ.outJ.println(primeNumbers.get(i));

            java.lang.SystemJ.outJ.println("Using collection with iterator while loop");
            java.util.Iterator<int> it = primeNumbers.iterator();
            while (it.hasNext())
                java.lang.SystemJ.outJ.println(it.next());

            java.lang.SystemJ.outJ.println("Using Java collection with .NET foreach loop");
            foreach (int prime in primeNumbers)
                java.lang.SystemJ.outJ.println(prime);

            java.lang.SystemJ.outJ.print("");
        }
    }
}
