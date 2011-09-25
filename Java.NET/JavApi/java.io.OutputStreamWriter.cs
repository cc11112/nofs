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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using java = biz.ritter.javapi;

namespace biz.ritter.javapi.io
{
    public class OutputStreamWriter : Writer 
    {
        private OutputStream delegateInstance;
        private java.nio.charset.Charset cs;

        public OutputStreamWriter(OutputStream outJ) : this (outJ, "UTF-8")
        {
        }

        public OutputStreamWriter(OutputStream outJ, String charsetName)
        {
            cs = java.nio.charset.Charset.forName(charsetName);
            this.delegateInstance = outJ;
        }


        public override void flush()
        {
            delegateInstance.flush();
        }
        public override void close()
        {
            delegateInstance.close();
        }
        public override void write(char[] cbuf, int off, int len)
        {
            char [] toWrite = new char[len-off];
            java.lang.SystemJ.arraycopy (cbuf,off,toWrite,0,len);
            byte[] result = (byte[])cs.encode(new java.nio.ReadWriteCharArrayBuffer(toWrite)).array();
            delegateInstance.write(result);
        }
        public override void write(string str, int off, int len)
        {
            base.write(str, off, len);
        }
        public override void write(int c)
        {
            base.write(c);
        }
    }
}
