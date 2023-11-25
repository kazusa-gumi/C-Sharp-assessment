using HolmesglenStudentManager.DataAccess;
using HolmesglenStudentManager.Models;
using System;
using System.Collections.Generic;

namespace HolmesglenStudentManager.BusinessLogicLayer
{
    public class SubjectBLLConnectedMode
    {
        private readonly SubjectDALConnectedMode _subjectDal;

        public SubjectBLLConnectedMode(SubjectDALConnectedMode subjectDal)
        {
            _subjectDal = subjectDal;
        }

        public List<Subject> GetAllSubjects()
        {
            try
            {
                return _subjectDal.GetAllSubjects();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllSubjects: {ex.Message}");
                return new List<Subject>();
            }
        }

        public Subject GetSubjectById(int id)
        {
            try
            {
                return _subjectDal.GetSubjectById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetSubjectById: {ex.Message}");
                return null;
            }
        }

        public bool AddSubject(Subject newSubject)
        {
            try
            {
                if (_subjectDal.GetSubjectById(newSubject.SubjectId) == null)
                {
                    return _subjectDal.AddSubject(newSubject);
                }
                else
                {
                    Console.WriteLine("Subjet already exists with this ID.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddSubject: {ex.Message}");
                return false;
            }
        }

        public bool UpdateSubject(Subject existingSubject)
        {
            try
            {
                return _subjectDal.UpdateSubject(existingSubject);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateSubject: {ex.Message}");
                return false;
            }
        }

        public bool DeleteSubject(int subjectId)
        {
            try
            {
                return _subjectDal.DeleteSubject(subjectId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteSubject: {ex.Message}");
                return false;
            }
        }
    }
}
