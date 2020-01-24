using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace MultiMineServer {
    public class Client {

        private TcpClient tcpClient;
        private Server server;
        private NetworkStream stream;

        private Thread runningThread;

        private bool threadRunning;

        public int uniqueID { get; set; }

        private byte[] buffer = new byte[1000000];

        string totalBuffer = "";
        public TcpClient TcpClient { get => tcpClient; set => tcpClient = value; }

        public Client(TcpClient tcpClient, Server server, int uniqueID)
        {
            //setup initial connection
            this.tcpClient = tcpClient;
            this.server = server;
            this.uniqueID = uniqueID;

            this.stream = tcpClient.GetStream();
            /*stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);*/
        }

        public void StartClientThread()
        {
            this.threadRunning = true;
            this.runningThread = new Thread(Run);
            this.runningThread.IsBackground = true;
            this.runningThread.Start();
        }

        public void StopClientThread()
        {
            this.threadRunning = false;
            this.runningThread.Abort();
        }

        private void Run()
        {
            while (threadRunning)
            {
                try
                {
                    // Console.WriteLine("got data from client");
/*                    int receivedBytes = stream.EndRead(ar);*/
                    this.stream.Read(buffer, 0, buffer.Length);
                    string wholePacket = Encoding.ASCII.GetString(buffer);
                    string stringMessage = wholePacket.Replace("\0", "");

                    string[] messages = stringMessage.Split(new string[] { Util.END_MESSAGE_KEY }, StringSplitOptions.None);
                    for (int i = 0; i < messages.Length; i++)
                    {
                        // Console.WriteLine(messages[i]);
                        try
                        {
                            if (messages[i] == "")
                            {
                                // Console.WriteLine("Message Length is 0");
                                continue;


                            }

                            // Below: Example of how to use the message class in order to filter out the data send by the other applications.
                            ServerMessage message = JsonConvert.DeserializeObject<ServerMessage>(messages[i]);
                            // Console.WriteLine(message.Data);

                            switch (message.MessageID)
                            {
                                case MessageIDs.SendGameBoard:
                                    Console.WriteLine("GameBoard data sent");
                                    Console.WriteLine("Message data : " + message.Data);
                                    this.SendMessage(new ServerMessage(message.MessageID, message.Data));
                                    break;
                                case MessageIDs.RequestAllClients:
                                    List<string> clientIDs = new List<string>();
                                    string toArray = "";

                                    foreach (Client client in server.Clients)
                                    {
                                        toArray = toArray + client.uniqueID + "+";
                                    }

                                    byte[] byteArray = Encoding.ASCII.GetBytes(toArray);
                                    Console.WriteLine("Clients Requested");
                                    this.SendMessage(new ServerMessage((MessageIDs.SendAllClients), byteArray));
                                    break;
                                default:
                                    break;
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }


                    }
                    if (this.stream == null)
                    {
                        this.server.DisconnectClient(this);
                        this.StopClientThread();
                        this.Disconnect();
                    }

                }
                catch (SocketException ex)
                {
                    //TODO: End the thread and stop the connection with the client.
                    this.Disconnect();
                }
                catch (IOException ex)
                {
                    this.Disconnect();
                }
            }
        }


        public void SendMessage(ServerMessage message)
        {
            string toSend = JsonConvert.SerializeObject((message)) + Util.END_MESSAGE_KEY;
            byte[] messageBytes = Encoding.Unicode.GetBytes(toSend);
            this.stream.Write(messageBytes, 0, messageBytes.Length);
        }


        public void Disconnect()
        {
         //   this.server.RemoveClient(this);
            this.tcpClient = null;
            this.stream = null;

            Console.WriteLine("Client disconnected!");
        }
    }
}
