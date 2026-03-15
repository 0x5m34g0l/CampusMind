using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusMind.Logic1.Core
{
    public enum enToolType
    {
        None = 0,
        CourseExplainer = 1,
        PolicyAssistant = 2,
        StudyPlanner = 3,
        FileConverter = 4,
    }
    public class Conversation
    {
        public int Id { get; set; }
        public enToolType ToolType { get; set; }
        public string Title { get; set; } 
        public int UserId { get; set; }

        public Conversation()
        {

        }

        public Conversation(int id, enToolType toolType, string title, int userId)
        {
            Id = id;
            ToolType = toolType;
            Title = title;
            UserId = userId;
        }
    }
}
