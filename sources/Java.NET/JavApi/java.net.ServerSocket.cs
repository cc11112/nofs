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

namespace biz.ritter.javapi.net
{
    public class ServerSocket : java.io.Closeable
    {
        private System.Net.Sockets.Socket delegateInstance;

        public ServerSocket(int port)
        {
            this.delegateInstance = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            this.delegateInstance.Bind(new System.Net.IPEndPoint(System.Net.IPAddress.Any, port));
            this.delegateInstance.Listen(50); // Why 50? See Javadoc!
        }

        public virtual Socket accept()
        {
            return new Socket(this.delegateInstance.Accept());
        }

        public virtual void close()
        {
            delegateInstance.Close();
        }
    }
}
