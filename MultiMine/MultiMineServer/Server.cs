using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiMineServer {
    public class Server {

        TcpListener listener;
        private List<Client> clients = new List<Client>();

        internal List<Client> Clients { get => clients; set => clients = value; }

        public Server()
        { 
            StartServer();

            Console.WriteLine("Server started and listening.");
            Console.WriteLine("Type quit to exit.");

             bool running = true;
             while (running)
                {
             string userInput = Console.ReadLine();
             if (userInput.ToLower() == "quit")
             {
                  running = false;
              }
              else
               {
                   Console.WriteLine("Unknown command: {0}", userInput);
             }
            }
        }

        public void StartServer()
        {
            listener = new TcpListener(IPAddress.Any, 1234);
            listener.Start();
            listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }

        private void OnConnect(IAsyncResult ar)
        {
            TcpClient tcpClient = listener.AcceptTcpClient();
            Thread tcpHandlerThread = new Thread(new ParameterizedThreadStart(tcpHandler));
            tcpHandlerThread.Start(tcpClient);
            Console.WriteLine("New client connected");
            Console.WriteLine(tcpClient.GetStream());
            
            clients.Add(new Client(tcpClient, this));

            //listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }

        private void tcpHandler(object client)
        {
            TcpClient mclient = (TcpClient)client;
            NetworkStream stream = mclient.GetStream();
            byte[] message = new byte[1024];
            while (true)
            {
                stream.Read(message,0,message.Length);
            }
            stream.Close();
            mclient.Close();
        }

        private void writeToFile()
        {

        }

    }
}
