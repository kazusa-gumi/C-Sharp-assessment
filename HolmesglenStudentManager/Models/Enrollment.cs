using System;
namespace HolmesglenStudentManager.Models
{
	public class Enrollment
	{   
            public int Id { get; set; }
            public int StudentID_FK { get; set; }
            public int SubjectID_FK { get; set; }
        
    }
}

