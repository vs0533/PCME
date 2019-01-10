using System;
using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.SeedWork;

namespace PCME.Domain.AggregatesModel.ApplicationForm
{
    public class StudentItem : Entity
    {
        public int StudentId
        {
            get;
            private set;
        }
        public Student Student
        {
            get;
            private set;
        }
        public int ApplyTableId
        {
            get;
            private set;
        }
        public ApplyTable ApplyTable
        {
            get;
            private set;
        }

        public StudentItem(int studentId, int applyTableId)
        {
            StudentId = studentId;
            ApplyTableId = applyTableId;
        }
        public StudentItem(int studentId)
        {
            StudentId = studentId;
        }
    }
}
