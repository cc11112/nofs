using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.dotnet.util.wrapper
{
    public sealed class StreamInputWrapper : System.IO.Stream
    {
        private java.io.InputStream delegateInstance;
        public StreamInputWrapper(java.io.InputStream inputStream)
        {
            this.delegateInstance = inputStream;
        }

        public override void Close()
        {
            this.delegateInstance.close();
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }
        public override void WriteByte(byte value)
        {
            throw new NotImplementedException();
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }
        public override bool CanRead
        {
            get { return true; }
        }

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
            if (origin == System.IO.SeekOrigin.Current)
                return this.delegateInstance.skip(offset);
            else
                throw new NotImplementedException();
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.delegateInstance.read(buffer, offset, count);
        }
        public override long Length
        {
            get { return this.delegateInstance.available(); }
        }
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
        public override bool CanSeek
        {
            get { return true; }
        }
    }
}
