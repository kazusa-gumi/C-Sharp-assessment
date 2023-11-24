﻿using HolmesglenStudentManager.DataAccess;
using HolmesglenStudentManager.Models;
using System.Collections.Generic;

namespace HolmesglenStudentManager.BusinessLogicLayer
{
    public class SubjectBLL
    {
        private readonly SubjectDAL _subjectDal;

        public SubjectBLL(SubjectDAL subjectDal)
        {
            _subjectDal = subjectDal;
        }

        public bool ValidateSubjectId(int subjectId) // stringからintへ変更
        {
            return _subjectDal.ValidateSubjectId(subjectId);
        }

        public List<Subject> GetAllSubjects()
        {
            return _subjectDal.GetAllSubjects();
        }

        public Subject GetSubjectById(int id)
        {
            // id を int 型として扱うよう修正
            return _subjectDal.GetSubjectById(id);
        }

        public bool AddSubject(Subject newSubject)
        {
            if (!ValidateSubjectId(newSubject.SubjectId))
            {
                _subjectDal.AddSubject(newSubject);
                return true;
            }
            return false;
        }

        public bool UpdateSubject(Subject subject)
        {
            if (_subjectDal.GetSubjectById(subject.SubjectId) != null) // stringからintへ変更
            {
                _subjectDal.UpdateSubject(subject);
                return true;
            }
            return false;
        }

        public bool DeleteSubject(int id) // stringからintへ変更
        {
            if (_subjectDal.GetSubjectById(id) != null)
            {
                _subjectDal.DeleteSubject(id);
                return true;
            }
            return false;
        }
    }
}