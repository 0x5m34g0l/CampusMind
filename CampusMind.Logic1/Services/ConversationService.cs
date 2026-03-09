using CampusMind.Data1.DataAccess;
using CampusMind.Logic1.Core;
using System.Collections.Generic;

namespace CampusMind.Logic1.Services
{
    public static class ConversationService
    {

        public static Conversation StartConversation(int userId, enToolType toolType, string title)
        {
            int conversationId = ConversationDataAccess.Create((int)toolType, title, userId);

            if (conversationId == -1)
                return null;

            return new Conversation(conversationId, toolType, title, userId);
        }


        public static Conversation GetConversation(int conversationId)
        {
            int toolType = 0;
            string title = "";
            int userId = 0;

            bool found = ConversationDataAccess.GetConversationById(
                conversationId,
                ref toolType,
                ref title,
                ref userId);

            if (!found)
                return null;

            return new Conversation(
                conversationId,
                (enToolType)toolType,
                title,
                userId);
        }


        public static List<Conversation> GetUserConversations(int userId)
        {
            List<int> ids = ConversationDataAccess.GetConversationsByUserId(userId);

            List<Conversation> conversations = new List<Conversation>();

            foreach (int id in ids)
            {
                int toolType = 0;
                string title = "";
                int uid = 0;

                bool found = ConversationDataAccess.GetConversationById(
                    id,
                    ref toolType,
                    ref title,
                    ref uid);

                if (found)
                {
                    conversations.Add(
                        new Conversation(
                            id,
                            (enToolType)toolType,
                            title,
                            uid));
                }
            }

            return conversations;
        }


        public static bool UpdateTitle(int conversationId, string newTitle)
        {
            return ConversationDataAccess.UpdateTitle(conversationId, newTitle);
        }


        public static bool DeleteConversation(int conversationId)
        {
            return ConversationDataAccess.DeleteConversation(conversationId);
        }

    }
}