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

namespace biz.ritter.javapi.lang
{
    public abstract class PrintStream : java.io.FilterOutputStream 
    {

        public PrintStream (java.io.OutputStream outJ) : base(outJ){}

        public virtual void println()
        {
            newLine();
        }

        public virtual void println(String output)
        {
            lock (this)
            {
                print(output);
                newLine();
            }
        }
        public virtual void print(String output)
        {
            lock (this)
            {
                printString(output);
            }
        }
        public virtual void println(int output)
        {
            lock (this)
            {
                print(output);
                newLine();
            }
        }

        public virtual void print(int output)
        {
            lock (this)
            {
                printInt(output);
            }
        }

        public virtual void println(long output)
        {
            lock (this)
            {
                print(output);
                newLine();
            }
        }

        public virtual void print(long output)
        {
            lock (this)
            {
                printLong(output);
            }
        }

        public virtual void print(Object obj)
        {
            this.printString(null != obj ? obj.ToString() : "null");
        }

        protected abstract void printString(String output);
        protected abstract void printInt(int output);
        protected abstract void printLong(long output);
        protected virtual void newLine()
        {
            this.printNewLine();
        }

        protected abstract void printNewLine();
    }

}
