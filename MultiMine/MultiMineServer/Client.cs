using Newtonsoft.Json;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace MultiMineServer {
    public class Client {

        private TcpClient tcpClient;
        private Server server;
        private NetworkStream stream;

        private byte[] buffer = new byte[1024];

        string totalBuffer = "";
        public TcpClient TcpClient { get => tcpClient; set => tcpClient = value; }

        public Client(TcpClient tcpClient, Server server)
        {
            //setup initial connection
            this.tcpClient = tcpClient;
            this.server = server;

            this.stream = tcpClient.GetStream();
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }

        private void OnRead(IAsyncResult ar)
        {
            try
            {
               // Console.WriteLine("got data from client");
                int receivedBytes = stream.EndRead(ar);
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

                        switch (message.Id)
                        {
                            case MessageIDs.SendTestData:
                                Console.WriteLine("TEST MESSAGE = " + message.Data);
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
                if (this.stream != null)
                {
                    buffer = new byte[1024];
                    stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
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

        public void Disconnect()
        {
            this.server.RemoveClient(this);
            this.tcpClient = null;
            this.stream = null;

            Console.WriteLine("Client disconnected!");
        }
    }
}
