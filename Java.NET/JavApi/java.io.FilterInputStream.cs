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
    [biz.ritter.develop.ReleaseNote(jVersion = 1.0, port = 1)]
    public class FilterInputStream : InputStream
    {

        protected volatile InputStream inJ;

        public FilterInputStream (InputStream inJ) {
            this.inJ = inJ;
        }

        public override int read()
        {
            return this.inJ.read();
        }
        public override int read(byte[] buffer)
        {
            return inJ.read(buffer);
        }
        public override int read(byte[] buffer, int beginOffset, int length)
        {
            return inJ.read(buffer, beginOffset, length);
        }
        public override void close()
        {
            inJ.close();
        }
        public override int available()
        {
            return inJ.available();
        }
        public override long skip(long n)
        {
            return inJ.skip(n);
        }
        public override void mark(int readlimit)
        {
            inJ.mark(readlimit);
        }
        public override bool markSupported()
        {
            return inJ.markSupported();
        }
        public override void reset()
        {
            inJ.reset();
        }
    }
}
