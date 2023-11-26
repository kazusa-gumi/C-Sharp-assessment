// DisconnectedStudentBLL.cs
using System.Data;
using HolmesglenStudentManager.DataAccess;

namespace HolmesglenStudentManager.BusinessLogic
{
    public class DisconnectedStudentBLL
    {
        private readonly DisconnectedStudentDAL _studentDAL;

        public DisconnectedStudentBLL(DisconnectedStudentDAL studentDAL)
        {
            _studentDAL = studentDAL;
        }

        public DataSet GetAllStudents()
        {
            return _studentDAL.GetAllStudents();
        }

        public void AddStudent(DataSet dataSet, DataRow newStudent)
        {
            _studentDAL.InsertStudent(dataSet, newStudent);
        }

        public void UpdateStudent(DataSet dataSet, DataRow studentToUpdate)
        {
            _studentDAL.UpdateStudent(dataSet, studentToUpdate);
        }

        public void DeleteStudent(int studentId)
        {
            _studentDAL.DeleteStudent(studentId);
        }
    }
}