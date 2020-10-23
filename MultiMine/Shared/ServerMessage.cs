﻿using System;

namespace Shared
{
    public class ServerMessage
    {

        public byte[] data;
        public MessageIDs id;

        public ServerMessage(MessageIDs id, byte[] data)
        {
            this.id = id;
            this.data = data;
        }

        public MessageIDs MessageID
        {
            get { return id; }
        }

        public byte[] Data
        {
            get { return data; }
        }
    }


    public enum MessageIDs
    {
        StartMultiplayerServer,
        JoinMultiPlayerServer,
        SendAllClients,
        RequestAllClients,
        SendGameBoard,
        SendGameBoardMultiplayer,
        SendChatMessage,
        SendChatMessageMultiplayer,
        SaveGame,
        GetRooms,
        SendRooms,
        GetRoomID,
        RoomCreated
    }
}
