using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.net
{
    internal class SocketInputStream : java.io.InputStream
    {
        private System.Net.Sockets.NetworkStream stream;
        public SocketInputStream(System.Net.Sockets.Socket baseSocket)
        {
            this.stream = new System.Net.Sockets.NetworkStream (baseSocket);
        }

        public override int read()
        {
            return this.stream.ReadByte();
        }
        public override int read(byte[] buffer, int beginOffset, int length)
        {
            return stream.Read(buffer, beginOffset, length);
        }
        public override int available()
        {
            return stream.DataAvailable ? 1 : -1;
        }
    }
}
