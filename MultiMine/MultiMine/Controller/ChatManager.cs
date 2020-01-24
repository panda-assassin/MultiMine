using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiMine.Controller
{
    class ChatManager
    {
        private static ChatManager instance;
        private List<string> messages = new List<string>();
        private ChatListener listener;

        private ChatManager()
        {

        }

        public static ChatManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ChatManager();
            }
            return instance;
        }

        public void setListener(ChatListener listener)
        {
            if (instance != null)
            {
                this.listener = listener;
            }
        }

        public void setChat(string message)
        {
            if (instance != null)
            {
                this.messages.Add(message);
                listener.chatUpdated(message);
            }
        }

        public List<string> getChat()
        {
            return this.messages;
        }

        public void clearChat()
        {
            this.messages.Clear();
        }
    }
}
