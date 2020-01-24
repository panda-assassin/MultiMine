using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using Shared.Model;

namespace MultiMineServer
{
    public class Server
    {

        TcpListener listener;
        private List<Client> clients = new List<Client>();
        private List<Room> rooms = new List<Room>();

        internal List<Client> Clients { get => clients; set => clients = value; }
        internal List<Room> Rooms { get => rooms; set => rooms = value; }

        public Server()
        {
            Console.WriteLine("Server started and listening.");
            Console.WriteLine("Type quit to exit.");
            StartServer();
        }

        public void StartServer()
        {
            listener = new TcpListener(IPAddress.Any, 1234);
            listener.Start();
            Start();
        }

        private void Start()
        {

            int random = 0;
            while (true)
            {
                TcpClient tcpClient = listener.AcceptTcpClient();
                Random rand = new Random();
                while (true)
                {
                    random = rand.Next(1000);
                    Client stubClient = getClient(random);
                    if (!clients.Contains(stubClient))
                    {
                        break;
                    }
                }
               

                
                Client client = new Client(tcpClient, this, random);
                client.StartClientThread();
                /*Thread tcpHandlerThread = new Thread(new ParameterizedThreadStart(tcpHandler));
                tcpHandlerThread.Start(tcpClient);*/
                Console.WriteLine("New client connected");
                Console.WriteLine(tcpClient.GetStream());

                clients.Add(client);
            }
        }

        public Client getClient(int UniqueID)
        {
            foreach (Client client in clients){
                if (client.uniqueID == UniqueID)
                {
                    return client;
                }
            }
            return null;
        }

        public Room getRoom(Client client)
        {
            foreach (Room room in rooms)
            {
                if (room.getRoomClients().Contains(client))
                {
                    return room;
                }
            }
            return null;
        }

        public void DisconnectClient(Client client)
        {
            RemoveClient(client);
        }

        private void RemoveClient(Client client)
        {
            clients.Remove(client);
        }

    }
}
