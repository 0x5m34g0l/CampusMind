using CampusMind.Data1.DataAccess;
using CampusMind.Logic1.Core;
using System.Collections.Generic;

namespace CampusMind.Logic1.Services
{
    public static class MessageService
    {

        public static Message SendUserMessage(int conversationId, string content)
        {
            int messageId = MessageDataAccess.Create(
                conversationId,
                (int)enRole.User,
                content);

            return new Message(
                messageId,
                enRole.User,
                content,
                conversationId);
        }


        public static Message SendAiMessage(int conversationId, string content)
        {
            int messageId = MessageDataAccess.Create(
                conversationId,
                (int)enRole.AI,
                content);

            return new Message(
                messageId,
                enRole.AI,
                content,
                conversationId);
        }


        public static List<Message> GetConversationMessages(int conversationId)
        {
            List<int> ids = MessageDataAccess.GetMessagesByConversationId(conversationId);

            List<Message> messages = new List<Message>();

            foreach (int id in ids)
            {
                int role = 0;
                string content = "";
                int convoId = 0;

                bool found = MessageDataAccess.GetMessageById(
                    id,
                    ref role,
                    ref content,
                    ref convoId);

                if (found)
                {
                    messages.Add(
                        new Message(
                            id,
                            (enRole)role,
                            content,
                            convoId));
                }
            }

            return messages;
        }

    }
}