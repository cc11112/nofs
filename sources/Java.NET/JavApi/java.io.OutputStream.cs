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
    public abstract class OutputStream : Closeable, Flushable
    {
        public abstract void write(int value);// throws IOException;
        public virtual void write(byte[] value) // throws IOException;
        {
            this.write(value, 0, value.Length);
        }
        public virtual void write(byte[] value, int beginOffset, int length) // throws IOException; 
        {
            if (0 > beginOffset || beginOffset + length > value.Length || 0 > length)
                throw new java.lang.IndexOutOfBoundsException();
            for (int index = beginOffset; index < beginOffset + length; index++)
            {
                this.write(value[index]);
            }
        }
        public virtual void flush () {}
        public virtual void close () {}

        
    }
}
