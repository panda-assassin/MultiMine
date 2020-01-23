using Newtonsoft.Json;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiMineCode
{
    class Connector
    {

        private static Connector instance;

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
            Thread mThread = new Thread(new ThreadStart(ConnectAsClient));
            mThread.Start();
        }

        private void ConnectAsClient()
        {
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse("192.168.1.124"), 1234);

            stream = client.GetStream();
            string s = "Hello from client";
            byte[] message = Encoding.ASCII.GetBytes(s);
            stream.Write(message, 0, message.Length);
            stream.Close();
            client.Close();

        }


        /// <summary>
        /// Read a message from the stream
        /// </summary>
        private void ReadMessage()
        {
            //TODO miss andere buffer size
            byte[] buffer = new byte[1024];
            stream.Read(buffer, 0, buffer.Length);

            string wholePacket = Encoding.ASCII.GetString(buffer);
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
            //TODO: add handleMessage

            ServerMessage message = JsonConvert.DeserializeObject<ServerMessage>(stringMessage);

            switch (message.Id)
            {
                case MessageIDs.SendTestData:
                    //TODO: Add logics here
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// Send a message to the server.
        /// </summary>
        private void sendMessage(object obj)
        {
            byte[] tosend = ObjectToByteArray(obj);

            try
            {
                //stream.write(name,0,name.lengt);//Write the name of the game
                stream.Write(tosend, 0, tosend.Length); // Write the bytes
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

        //creates object from bytearray
        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
           
        }

        //makes bytearray from object
        private object ByteArrayToObject(Byte[] byteArray)
        {
            if (byteArray == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                object obj = bf.Deserialize(ms);
                return obj;
            }
        }

    }


}
