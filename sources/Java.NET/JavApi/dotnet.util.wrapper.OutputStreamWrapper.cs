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

namespace biz.ritter.javapi.dotnet.util.wrapper
{
    public sealed class OutputStreamWrapper : System.IO.Stream
    {
        private java.io.OutputStream delegateInstance;
        public OutputStreamWrapper (java.io.OutputStream outputStream) {
            this.delegateInstance = outputStream;
        }

        public override void Close()
        {
            this.delegateInstance.close();
        }

        public override void Flush()
        {
            this.delegateInstance.flush();
        }
        public override void WriteByte(byte value)
        {
            this.delegateInstance.write(value);
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            this.delegateInstance.write(buffer, offset, count);
        }
        public override bool CanWrite
        {
            get {
                return true;
            }
        }

        #region not implemented methods
        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            throw new NotImplementedException();
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
        public override long Length
        {
            get { throw new NotImplementedException(); }
        }
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
        public override bool CanRead
        {
            get { throw new NotImplementedException(); }
        }
        public override bool CanSeek
        {
            get { throw new NotImplementedException(); }
        }
        #endregion
    }
}
