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
    public class Socket : java.io.Closeable
    {
        private System.Net.Sockets.Socket delegateInstance;

        public Socket (String host, int port){
            System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostEntry (host);
            System.Net.IPAddress serverIP =  hostEntry.AddressList [0];
            System.Net.IPEndPoint serverEP = new System.Net.IPEndPoint(serverIP,port);

            this.delegateInstance = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
        }
        /// <summary>
        /// Is used from ServerSocket to accept clients.
        /// </summary>
        /// <param name="newDelegateInstance">new C# Socket instance</param>
        internal Socket(System.Net.Sockets.Socket newDelegateInstance)
        {
            this.delegateInstance = newDelegateInstance;
        }

        public virtual java.io.InputStream getInputStream () {
            return new SocketInputStream(this.delegateInstance);
        }
        public virtual java.io.OutputStream getOutputStream () {
            return new SocketOutputStream(this.delegateInstance);
        }
        public void close()
        {
            this.delegateInstance.Close();
        }
    }
}
