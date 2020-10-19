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
    public class Room {

        List<Client> RoomClients;
        private bool threadRunning;
        private Thread runningThread;

        private GameBoard gameBoard;

        public int ID;

        private byte[] buffer = new byte[1000000];

        public Room(Client client, int ID)
        {
            this.RoomClients = new List<Client>();
            this.RoomClients.Add(client);
            this.ID = ID;
        }

        public void joinRoom(Client client)
        {
            this.RoomClients.Add(client);
        }

        public List<Client> getRoomClients()
        {
            return RoomClients;
        }

        public void StartRoomThread()
        {
            this.threadRunning = true;
            this.runningThread = new Thread(Run);
            this.runningThread.IsBackground = true;
            this.runningThread.Start();
        }

        public void Run()
        {
            while (threadRunning)
            {
                try
                {
                    foreach (Client client in RoomClients)
                    {
                        // Console.WriteLine("got data from client");
                        /*                    int receivedBytes = stream.EndRead(ar);*/
                        client.stream.Read(buffer, 0, buffer.Length);
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
                                    case MessageIDs.SendGameBoardMultiplayer:
                                        Console.WriteLine("GameBoard data sent");
                                        Console.WriteLine("Message data : " + message.Data);
                                        string gameBoardString = Encoding.ASCII.GetString(message.data);
                                        this.gameBoard = JsonConvert.DeserializeObject<GameBoard>(gameBoardString);
                                        this.SendMessage(new ServerMessage(message.MessageID, message.Data), client);
                                        break;
                                    case MessageIDs.SendChatMessageMultiplayer:
                                        Console.WriteLine("message data sent");
                                        Console.WriteLine("Message data : " + Encoding.ASCII.GetString(message.data));
                                        this.SendMessage(new ServerMessage(message.MessageID, message.Data), client);
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
                    }
                }

                catch (SocketException ex)
                {
                    //TODO: End the thread and stop the connection with the client.
                    Console.WriteLine(ex); ;
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex);
                }
            }

        }

        public void SendMessage(ServerMessage message, Client client)
        {
            string toSend = JsonConvert.SerializeObject((message)) + Util.END_MESSAGE_KEY;
            byte[] messageBytes = Encoding.Unicode.GetBytes(toSend);
            client.stream.Write(messageBytes, 0, messageBytes.Length);
        }

        public void sendGameRoomData(Client client)
        {
            string toSerialize = JsonConvert.SerializeObject(gameBoard);
            byte[] messageBytes = Encoding.ASCII.GetBytes(toSerialize);
            this.SendMessage(new ServerMessage(MessageIDs.SendGameBoardMultiplayer, messageBytes), client);
        }

    }
}

