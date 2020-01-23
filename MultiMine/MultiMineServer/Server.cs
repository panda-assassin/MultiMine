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
                stream.Read(message, 0, message.Length);
            }
            stream.Close();
            mclient.Close();
        }

        private void writeToFile(GameBoard gameBoard)
        {
            string output = JsonConvert.SerializeObject(gameBoard);
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(getPath() + "Data/Data.txt", true))
            {
                file.WriteLine(output);
            }
        }

        private GameBoard readFromFile()
        {
            return (GameBoard)JsonConvert.DeserializeObject(System.IO.File.ReadAllText(getPath() + "Data/Data.txt"));
        }
        public static string getPath()
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string Startsplit = startupPath.Substring(0, startupPath.LastIndexOf("bin"));
            string split = Startsplit.Replace(@"\", "/");
            return split;
        }

    }
}
