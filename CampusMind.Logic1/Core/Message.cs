using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusMind.Logic1.Core
{

    // sender : User
    // reposne : AI
    public enum enRole { User = 1, AI = 2 };
    public class Message
    {
        public int Id { get; set; }
        public enRole Role { get; set; }
        public string Content { get; set; } 
        public int ConversationId { get; set; }

        public Message() { }

        public Message (int id, enRole role, string content, int conversationId)
        {
            Id = id;
            Role = role;
            Content = content;
            ConversationId = conversationId;
        }
    }
}
