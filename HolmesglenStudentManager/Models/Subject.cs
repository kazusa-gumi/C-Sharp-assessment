using System;
namespace HolmesglenStudentManager.Models
{
	
    public class Subject
    {
        public int SubjectId { get; set; }
        public string Title { get; set; }
        public int NumberOfSession { get; set; } 
        public int HourPerSession { get; set; }

    }
}

