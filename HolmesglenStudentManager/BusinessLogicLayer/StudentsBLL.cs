// StudentBLL.cs - 修正版
using HolmesglenStudentManager.DataAccess;
using HolmesglenStudentManager.Models;
using System.Collections.Generic;

namespace HolmesglenStudentManager.BusinessLogicLayer
{
    public class StudentBLL
    {
        private readonly StudentDAL _studentDal;

        public StudentBLL(StudentDAL studentDal)
        {
            _studentDal = studentDal;
        }

        public bool ValidateStudentId(int studentId)
        {
            // 接続メソッドと型が一致するよう修正
            return _studentDal.ValidateStudentId(studentId);
        }

        public List<Student> GetAllStudents()
        {
            // 変更なし
            return _studentDal.GetAllStudents();
        }

        public Student GetStudentById(int id)
        {
            // id を int 型として扱うよう修正
            return _studentDal.GetStudentById(id);
        }

        public bool AddStudent(Student newStudent)
        {
            // Student クラスが int 型の StudentId を持っていればこのメソッドはそのままで正しい
            // それによって ValidateStudentId も正しい形式で呼び出される
            if (!_studentDal.ValidateStudentId(newStudent.StudentId))
            {
                _studentDal.AddStudent(newStudent);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateStudent(Student existingStudent)
        {
            // Student クラスとの整合性を取るために修正
            var studentToUpdate = _studentDal.GetStudentById(existingStudent.StudentId);
            if (studentToUpdate != null)
            {
                _studentDal.UpdateStudent(existingStudent);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteStudent(int studentId)
        {
            // studentId を int 型として扱うよう修正
            var studentToDelete = _studentDal.GetStudentById(studentId);
            if (studentToDelete != null)
            {
                _studentDal.DeleteStudent(studentId);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
