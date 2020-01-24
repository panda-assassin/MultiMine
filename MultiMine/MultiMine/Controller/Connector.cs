using Newtonsoft.Json;
using Shared;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiMine.Controller
{
    class Connector
    {
        private static Connector instance;

        public List<string> clients { get; set; }
        private TcpClient client;
        private NetworkStream stream;
        private string firstHalfFromBuffer;
        private string secondHalfFromBuffer;
        private bool incompleteMessage;

        /// <summary>
        /// Makes connection to the server and saves the connection in the client and stream variable. 
        /// </summary>
        private Connector()
        {
            try
            {
                //client = new System.Net.Sockets.TcpClient("86.82.166.205", 1234); // Create a new connection
                client = new System.Net.Sockets.TcpClient("127.0.0.1", 1234); // Create a new connection
                stream = client.GetStream();

                clients = new List<string>();

                StartReading();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Start reading messages from the server on a new thread. 
        /// </summary>
        private void StartReading()
        {
            Thread t = new Thread(ReadMessage);
            t.IsBackground = false;
            t.Start();
        }

        /// <summary>
        /// Read a message from the stream
        /// </summary>
        private void ReadMessage()
        {
            byte[] buffer = new byte[1000000];
            stream.Read(buffer, 0, buffer.Length);

            string wholePacket = Encoding.Unicode.GetString(buffer);
            string stringMessage = wholePacket.Replace("\0", "");
            string[] messages = stringMessage.Split(new string[] { Util.END_MESSAGE_KEY }, StringSplitOptions.None);

            int length = messages.Length;

            if (incompleteMessage)
            {
                secondHalfFromBuffer = messages[0];
                messages[0] = firstHalfFromBuffer + secondHalfFromBuffer;
            }

            if (!messages[messages.Length - 1].Contains(Util.END_MESSAGE_KEY))
            {
                length -= 1;
                firstHalfFromBuffer = messages[messages.Length - 1];
                incompleteMessage = true;
            }


            for (int i = 0; i < length; i++)
            {
                if (messages[i] == "")
                    continue;

                HandleMessage(messages[i]);
            }

            this.ReadMessage();
        }

        private void HandleMessage(string stringMessage)
        {

            ServerMessage message = JsonConvert.DeserializeObject<ServerMessage>(stringMessage);

            byte[] byteArray = message.Data;
            string wholePacket = Encoding.ASCII.GetString(byteArray);

            switch (message.MessageID)
            {
                case MessageIDs.SendGameBoard:
                    GameBoard gameBoard = JsonConvert.DeserializeObject<GameBoard>(wholePacket);
                    GameBoardManager.GetInstance().setGameBoard(gameBoard);
                    break;
                case MessageIDs.SendAllClients:
                    string[] array = wholePacket.Split(new string[] { "--ID--" }, StringSplitOptions.None);
                    for (int i = 0; i < array.Length; i++)
                    {
                        clients.Add(array[i]);
                    }
                    break;
                case MessageIDs.SendChatMessage:
                    ChatManager.GetInstance().setChat(Encoding.ASCII.GetString(message.data));
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Send a message to the server.
        /// </summary>
        private void sendMessage(ServerMessage message)
        {
            string toSend = JsonConvert.SerializeObject(message) + Util.END_MESSAGE_KEY;
            byte[] messageBytes = Encoding.Unicode.GetBytes(toSend);

            try
            {
                stream.Write(messageBytes, 0, messageBytes.Length); // Write the bytes
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Connector GetInstance()
        {
            if (instance == null)
            {
                instance = new Connector();
            }
            return instance;
        }

        public void sendGameBoard(GameBoard gameBoard)
        {
            string gameboardString = JsonConvert.SerializeObject(gameBoard);
            byte[] byteArray = Encoding.ASCII.GetBytes(gameboardString);
            this.sendMessage(new ServerMessage(MessageIDs.SendGameBoard, byteArray));
        }

        public void sendChatMessage(string message)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(message);
            this.sendMessage(new ServerMessage(MessageIDs.SendChatMessage, byteArray));
        }

        public void connectClient()
        {
            string clientConnected = "true";
            byte[] byteArray = Encoding.ASCII.GetBytes(clientConnected);
            this.sendMessage(new ServerMessage(MessageIDs.ClientConnected, byteArray));
        }

        public void disconnectClient()
        {
            string clientConnected = "false";
            byte[] byteArray = Encoding.ASCII.GetBytes(clientConnected);
            this.sendMessage(new ServerMessage(MessageIDs.ClientDisconnected, byteArray));
        }

        public void requestClientList()
        {
            this.sendMessage(new ServerMessage(MessageIDs.RequestAllClients, null));
        }

        public void destroyInstance()
        {
            instance = null;
        }
    }


}

