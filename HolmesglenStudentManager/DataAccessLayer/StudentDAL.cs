// StudentDAL.cs
using HolmesglenStudentManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace HolmesglenStudentManager.DataAccess
{
    public class StudentDAL
    {
        private readonly AppDBContext _context;

        public StudentDAL(AppDBContext context)
        {
            _context = context;
        }

        public bool ValidateStudentId(int studentId)
        {
            return _context.Student.Any(s => s.StudentId == studentId);
        }

        public List<Student> GetAllStudents()
        {
            return _context.Student.ToList();
        }

        public Student GetStudentById(int id)
        {
            return _context.Student.FirstOrDefault(s => s.StudentId == id);
        }

        public bool AddStudent(Student student)
        {
            if (!ValidateStudentId(student.StudentId))
            {
                _context.Student.Add(student);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateStudent(Student student)
        {
            var studentToUpdate = _context.Student.FirstOrDefault(s => s.StudentId == student.StudentId);
            if (studentToUpdate != null)
            {
                studentToUpdate.FirstName = student.FirstName;
                studentToUpdate.LastName = student.LastName;
                studentToUpdate.Email = student.Email;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteStudent(int studentId) // 引数のデータ型を int に変更しました
        {
            var studentToDelete = _context.Student.FirstOrDefault(s => s.StudentId == studentId);
            if (studentToDelete != null)
            {
                _context.Student.Remove(studentToDelete);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}