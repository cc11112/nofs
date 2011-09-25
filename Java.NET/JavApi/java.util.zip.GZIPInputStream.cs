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
using System.Text;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util.zip
{
    public class GZIPInputStream : InflaterInputStream
    {
        private readonly System.IO.Compression.GZipStream delegateInstance;
        public GZIPInputStream(java.io.InputStream inJ)
            : base(inJ)
        {
            delegateInstance = new System.IO.Compression.GZipStream(new dotnet.util.wrapper.StreamInputWrapper (inJ),System.IO.Compression.CompressionMode.Decompress);
        }

        public override int available()
        {
            return delegateInstance.CanRead ? 1 : -1;
        }

        public override int read()
        {
            return delegateInstance.ReadByte();
        }
        public override int read(byte[] buffer, int beginOffset, int length)
        {
            return delegateInstance.Read(buffer, beginOffset, length);
        }
        public override long skip(long n)
        {
            return this.delegateInstance.Seek(n, System.IO.SeekOrigin.Current);
        }
        public override bool markSupported()
        {
            return false;
        }
        public override void mark(int readlimit)
        {
            throw new java.io.IOException ("mark/reset not supported");
        }
        public override void close()
        {
            this.delegateInstance.Close();
            base.close();
        }
        public override void reset()
        {
            throw new java.io.IOException("mark/reset not supported");
        }
    }
}
