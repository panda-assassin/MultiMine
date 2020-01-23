using System;

namespace Shared
{
    public class ServerMessage
    {

        public byte[] Data { get; set; }
        public MessageIDs Id { get; set; }

        public ServerMessage(MessageIDs Id, byte[] Data)
        {
            this.Id = Id;
            this.Data = Data;
        }
    }

    public enum MessageIDs
    {
        //ALL EXAMPLES!!! Use Better names later on
        SendTestData
    }
}
