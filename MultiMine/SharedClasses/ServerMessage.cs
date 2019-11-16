namespace SharedClasses {
    public class ServerMessage {

        public string Data { get; set; }
        public MessageIDs Id { get; set; }

        public ServerMessage(MessageIDs Id, string Data)
        {
            this.Id = Id;
            this.Data = Data;
        }
    }
    public enum MessageIDs {
        //ALL EXAMPLES!!! Use Better names later on
        SendTestData
    }
}
