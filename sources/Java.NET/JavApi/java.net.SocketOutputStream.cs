using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.net
{
    internal class SocketOutputStream : java.io.OutputStream 
    {
        private System.Net.Sockets.NetworkStream stream;
        public SocketOutputStream(System.Net.Sockets.Socket baseSocket)
        {
            this.stream = new System.Net.Sockets.NetworkStream (baseSocket);
        }

        public override void write(byte[] value, int beginOffset, int length)
        {
            stream.Write(value, beginOffset, length);
        }
        public override void write(int value)
        {
            stream.WriteByte((byte)value);
        }
        public override void flush()
        {
            stream.Flush();
        }

    }
}
