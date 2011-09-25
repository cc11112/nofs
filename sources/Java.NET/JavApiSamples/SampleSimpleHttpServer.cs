using System;
using java = biz.ritter.javapi;

namespace javapi.sample.net
{
    /// <summary>
    /// Sample for working with threads and sockets.
    /// </summary>
    public class SampleSimpleHttpServer
    {
        private bool running = false;
        private int port;

        static void Main()
        {
            SampleSimpleHttpServer server = new SampleSimpleHttpServer();
            server.port = 51000;
            server.start();
        }

        public void start()
        {
            java.net.ServerSocket server = new java.net.ServerSocket(this.port);
            // Create a receiver main loop...
            this.running = true;
            while (this.running)
            {
                // waiting for connect...
                java.net.Socket client = server.accept();

                // create new thread for work
                java.lang.Thread worker = new java.lang.Thread(new SampleSimpleHttpServerWorker(client));
                worker.start();
            }
        }

        public void stop()
        {
            this.running = false;
        }
    }

    public class SampleSimpleHttpServerWorker : java.lang.Runnable
    {
        private java.net.Socket client;
        public SampleSimpleHttpServerWorker(java.net.Socket newClient)
        {
            this.client = newClient;
        } 
        public void run()
        {
            java.io.OutputStream os = this.client.getOutputStream();
            os.write("Hello JavApi User".getBytes());
            os.flush();
            client.close();
        }
    }
}
