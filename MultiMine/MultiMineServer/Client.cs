using Newtonsoft.Json;
using Shared;
using Shared.Model;
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
        public NetworkStream stream;

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
           
        }

        public async void StartClientThread()
        {
            this.threadRunning = true;
            await Run2();
        }

        public void StopClientThread()
        {
            this.threadRunning = false;
            //this.runningThread.Abort();
        }

        private async Task Run2()
        {
            await Task.Run(() =>
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
                                    case MessageIDs.SendChatMessage:
                                        Console.WriteLine("message data sent");
                                        Console.WriteLine("Message data : " + Encoding.ASCII.GetString(message.Data));
                                        this.SendChatMessage(new ServerMessage(message.MessageID, message.Data));
                                        break;
                                    case MessageIDs.StartMultiplayerServer:
                                        Room room = new Room(this, (server.Rooms.Count + 1));
                                        room.StartRoomThread();
                                        server.CreateRoom(room);
                                        Console.WriteLine("Created Room with ID:" + server.getRoom(this).ID);
                                        byte[] roomIDbyteArray = Encoding.ASCII.GetBytes(room.ID.ToString());
                                        this.SendMessage(new ServerMessage(MessageIDs.RoomCreated, roomIDbyteArray));
                                        server.Clients.Remove(this);
                                        break;
                                    case MessageIDs.JoinMultiPlayerServer:
                                        string roomHost = Encoding.ASCII.GetString(message.data);
                                        server.joinRoom(this, roomHost);
                                        Console.WriteLine("Client [" + this.uniqueID + "] joined room [" + roomHost + "]");
                                        server.Clients.Remove(this);
                                        break;
                                    case MessageIDs.SaveGame:
                                        writeToFile(Encoding.ASCII.GetString(message.data));
                                        break;
                                    case MessageIDs.GetRooms:
                                        List<string> rooms = new List<string>();
                                        string roomsToArray = "";

                                        foreach (Room room2 in server.Rooms)
                                        {
                                            roomsToArray = roomsToArray + room2.ID + "+";
                                        }

                                        byte[] byteArray2 = Encoding.ASCII.GetBytes(roomsToArray);
                                        Console.WriteLine("Rooms Requested");

                                        this.SendMessage(new ServerMessage(MessageIDs.SendRooms, byteArray2));
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
            });

        }

        public void SendMessage(ServerMessage message)
        {
           
                    foreach (Client client in server.Clients)
                    {
                        stream = client.tcpClient.GetStream();
                        string toSend = JsonConvert.SerializeObject((message)) + Util.END_MESSAGE_KEY;
                        byte[] messageBytes = Encoding.Unicode.GetBytes(toSend);
                        this.stream.Write(messageBytes, 0, messageBytes.Length);
                    }

                
            

        }

        public void SendChatMessage(ServerMessage message)
        {
            string totalMessageString = Encoding.ASCII.GetString(message.Data);
            string[] totalMessageArray = totalMessageString.Split('-');

            string messageRoomID = totalMessageArray[0];
            string messageString = totalMessageArray[1];

            foreach (Room room in server.Rooms)
            {
                if (room.ID.ToString() == messageRoomID)
                {
                    foreach (Client client in room.getRoomClients())
                    {
                        stream = client.tcpClient.GetStream();
                        byte[] messageStringBytes = Encoding.Unicode.GetBytes(messageString);
                        ServerMessage newMessage = new ServerMessage(MessageIDs.SendChatMessage, messageStringBytes);
                        string toSend = JsonConvert.SerializeObject((newMessage)) + Util.END_MESSAGE_KEY;
                        byte[] messageBytes = Encoding.Unicode.GetBytes(toSend);
                        this.stream.Write(messageBytes, 0, messageBytes.Length);
                    }

                }
            }
        }


        public void Disconnect()
        {
         //   this.server.RemoveClient(this);
            this.tcpClient = null;
            this.stream = null;

            Console.WriteLine("Client disconnected!");
        }

        private void writeToFile(string gameBoard)
        {
            File.WriteAllText(getPath() + "/Data/Data.txt", gameBoard);
        }

        private GameBoard readFromFile()
        {
            return (GameBoard)JsonConvert.DeserializeObject(System.IO.File.ReadAllText(getPath() + "/Data/Data.txt"));
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
