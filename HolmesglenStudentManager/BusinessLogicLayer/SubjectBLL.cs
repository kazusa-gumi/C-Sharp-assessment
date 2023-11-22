using HolmesglenStudentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HolmesglenStudentManager.BusinessLogicLayer
{
    public class SubjectBLL
    {
        private AppDBContext _db;

        public SubjectBLL(AppDBContext appDBContext)
        {
            _db = appDBContext;
        }

        // すべての科目を取得するメソッド
        public List<Subject> GetAllSubjects()
        {
            return _db.Subject.ToList();
        }

        // IDによって特定の科目を取得するメソッド
        public Subject GetSubjectById(string id)
        {
            return _db.Subject.FirstOrDefault(s => s.SubjectId == id);
        }

        // 新しい科目を追加するメソッド
        public bool AddSubject(Subject newSubject)
        {
            var existingSubject = _db.Subject.FirstOrDefault(s => s.SubjectId == newSubject.SubjectId);
            if (existingSubject == null)
            {
                _db.Subject.Add(newSubject);
                _db.SaveChanges();
                return true; // 追加成功
            }
            return false; // すでに同じIDの科目が存在するため追加失敗
        }

        // 科目情報を更新するメソッド
        public bool UpdateSubject(Subject subject)
        {
            var subjectToUpdate = _db.Subject.FirstOrDefault(s => s.SubjectId == subject.SubjectId);
            if (subjectToUpdate != null)
            {
                subjectToUpdate.Title = subject.Title;
                subjectToUpdate.NumberOfSession = subject.NumberOfSession; // 更新
                subjectToUpdate.HourPerSession = subject.HourPerSession; // 更新
                _db.SaveChanges();
                return true; // 更新成功
            }
            return false; // 科目が見つからないため更新失敗
        }

        // 科目を削除するメソッド
        public bool DeleteSubject(string id)
        {
            var subjectToDelete = _db.Subject.FirstOrDefault(s => s.SubjectId == id);
            if (subjectToDelete != null)
            {
                _db.Subject.Remove(subjectToDelete);
                _db.SaveChanges();
                return true; // 削除成功
            }
            return false; // 科目が見つからないため削除失敗
        }
    }

}