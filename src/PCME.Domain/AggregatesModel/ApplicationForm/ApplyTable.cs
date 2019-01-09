using System;
using System.Collections.Generic;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.SeedWork;
using System.Linq;
namespace PCME.Domain.AggregatesModel.ApplicationForm
{
    public class ApplyTable : Entity
    {
        public int WorkUnitId
        {
            get;
            private set;
        }
        public WorkUnit WorkUnit
        {
            get;
            private set;
        }

        private readonly List<StudentItem> _studentItems;

        public IReadOnlyCollection<StudentItem> StudentItems => _studentItems;
        public DateTime CreateTime
        {
            get;
            private set;
        }
        public string Num
        {
            get;
            private set;
        }

        public ApplyTable()
        {
            _studentItems = new List<StudentItem>();
        }

        public ApplyTable(int workUnitId, DateTime createTime, string num)
        {
            _studentItems = new List<StudentItem>();
            WorkUnitId = workUnitId;
            CreateTime = createTime;
            Num = num;
        }

        public void AddStudentItem(int studentId, int applyTableId)
        {
            var isExists = _studentItems.FirstOrDefault(c => c.StudentId == studentId && c.ApplyTableId == applyTableId);
            if (isExists != null)
            {
                throw new Exception("该人员已经存在申请表表内");
            }
            else {
                StudentItem item = new StudentItem(studentId, applyTableId);
                _studentItems.Add(item);
            }
        }
    }
}
