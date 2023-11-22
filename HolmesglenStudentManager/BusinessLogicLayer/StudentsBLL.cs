using HolmesglenStudentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolmesglenStudentManager.BusinessLogicLayer
{
    public class StudentBLL
    {
        private AppDBContext _db;

        public StudentBLL(AppDBContext appDBContext)
        {
            _db = appDBContext;
        }

        // すべての学生を取得するメソッド
        public List<Student> GetAllStudents()
        {
            return _db.Student.ToList();
        }

        // IDによって特定の学生を取得するメソッド
        public Student GetStudentById(string id)
        {
            return _db.Student.FirstOrDefault(s => s.StudentId == id); // IdをStudentIdに変更
        }

        // 新しい学生を追加するメソッド
        public bool AddStudent(Student newStudent)
        {
            var existingStudent = _db.Student.FirstOrDefault(s => s.StudentId == newStudent.StudentId);
            if (existingStudent == null)
            {
                _db.Student.Add(newStudent);
                _db.SaveChanges();
                return true; // 追加成功
            }
            return false; // すでに同じIDの学生が存在するため追加失敗
        }

        // 学生情報を更新するメソッド
        public bool UpdateStudent(Student student)
        {
            var studentToUpdate = _db.Student.FirstOrDefault(s => s.StudentId == student.StudentId);
            if (studentToUpdate != null)
            {
                studentToUpdate.FirstName = student.FirstName;
                studentToUpdate.LastName = student.LastName;
                studentToUpdate.Email = student.Email;
                _db.SaveChanges();
                return true; // 更新成功
            }
            return false; // 学生が見つからないため更新失敗
        }

        // 学生を削除するメソッド
        public bool DeleteStudent(string id)
        {
            var studentToDelete = _db.Student.FirstOrDefault(s => s.StudentId == id);
            if (studentToDelete != null)
            {
                _db.Student.Remove(studentToDelete);
                _db.SaveChanges();
                return true; // 削除成功
            }
            return false; // 学生が見つからないため削除失敗
        }

    }
}
