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
    public class GZIPOutputStream : DeflaterOutputStream
    {
        protected CRC32 crc = new CRC32();

        private System.IO.Compression.GZipStream delegateInstance;

        public GZIPOutputStream(java.io.OutputStream stream) : base (stream)
        {
            dotnet.util.wrapper.OutputStreamWrapper wrapperStream = new dotnet.util.wrapper.OutputStreamWrapper(stream);
            this.delegateInstance = new System.IO.Compression.GZipStream(wrapperStream, System.IO.Compression.CompressionMode.Compress);
        }

        public override void write(byte[] buf, int p, int len)
        {
            crc.update(buf, p, len);
            this.delegateInstance.Write(buf, p, len);
        }
        public override void write(int b)
        {
            this.delegateInstance.WriteByte((byte)b);
        }
        public override void flush()
        {
            this.delegateInstance.Flush();
        }
        public override void close()
        {
            this.delegateInstance.Close();
        }

        public override void finish()
        {
            base.finish();
            writeLong(crc.getValue());
            writeLong(crc.tbytes);
        }

        private long writeLong(long value)  {
            // Write out the long value as an unsigned int
            int unsigned = (int) value;
            this.delegateInstance.WriteByte ((byte) (unsigned & 0xFF));
            this.delegateInstance.WriteByte ((byte) ((unsigned >> 8) & 0xFF));
            this.delegateInstance.WriteByte ((byte) ((unsigned >> 16) & 0xFF));
            this.delegateInstance.WriteByte ((byte) ((unsigned >> 24) & 0xFF));
            return value;
        }

        private int writeShort(int value) {
            this.delegateInstance.WriteByte ((byte) (value & 0xFF));
            this.delegateInstance.WriteByte ((byte) ((value >> 8) & 0xFF));
            return value;
        }
    }
}
