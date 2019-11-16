using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MultiMineServer {
    public class Server {

        TcpListener listener;
        private List<Client> clients = new List<Client>();

        internal List<Client> Clients { get => clients; set => clients = value; }

        public Server()
        {
            listener = new TcpListener(IPAddress.Any, 1234);
            listener.Start();
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
            listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }

        public void RemoveClient(Client client)
        {
            clients.Remove(client);
        }
        private void OnConnect(IAsyncResult ar)
        {
            var newTcpClient = listener.EndAcceptTcpClient(ar);
            Console.WriteLine("New client connected");

            clients.Add(new Client(newTcpClient, this));

            listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }

    }
}
