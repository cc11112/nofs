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

namespace biz.ritter.javapi.dotnet.lang
{

    /**
     * Default OutputStream to write something on Console.
     */
    public class ConsoleOutputPrintStream : java.lang.PrintStream
    {
        public ConsoleOutputPrintStream() : base(null){}
        
        protected override void printNewLine()
        {
            Console.WriteLine();
        }
        protected override void printString(string output)
        {
            Console.Write(output);
        }

        /// <summary>
        /// Override the PrintStream implementation with Console specific implementation
        /// </summary>
        /// <param name="output"></param>
        public override void println(string output)
        {
            Console.WriteLine(output);
        }
        protected override void printInt(int output)
        {
            Console.Write(output);
        }
        protected override void printLong(long output)
        {
            Console.Write(output);
        }
        public override void write(int b)
        {

            char toWrite = (char)b;
            Console.Out.Write(toWrite);
        }
        public override void close()
        {
            Console.Out.Close();
        }
        public override void flush()
        {
            Console.Out.Flush();
        }
        public override void print(int output)
        {
            Console.Out.Write(output);
        }
        public override void print(long output)
        {
            Console.Out.Write(output);
        }
        public override void print(string output)
        {
            Console.Out.Write(output);
        }
        public override void println(int output)
        {
            Console.Out.WriteLine(output);
        }
        public override void println(long output)
        {
            Console.Out.WriteLine(output);
        }
    }
}
