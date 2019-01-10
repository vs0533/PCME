using System;
using System.Collections.Generic;
using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.SeedWork;
using System.Linq;
namespace PCME.Domain.AggregatesModel.ApplicationForm
{
    public class ApplyTable : Entity
    {

        public bool IsPay { get; private set; }

        public bool IsLock { get; private set; }
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

        public int ApplyForSettingId { get; set; }
        public ApplyForSetting ApplyForSetting
        {
            get;
            private set;
        }

        public ApplyTable(int workUnitId, DateTime createTime, string num,int applyForSettingId)
        {
            _studentItems = new List<StudentItem>();
            WorkUnitId = workUnitId;
            CreateTime = createTime;
            Num = num;
            ApplyForSettingId = applyForSettingId;
            IsPay = false;
            IsLock = false;
        }

        public void AddStudentItem(int studentId)
        {
            var isExists = _studentItems.FirstOrDefault(c => c.StudentId == studentId);
            if (isExists != null)
            {
                throw new Exception("该人员已经存在申请表表内");
            }
            else {
                StudentItem item = new StudentItem(studentId);
                _studentItems.Add(item);
            }
        }

        public void ChangeIsLock(bool islock)
        {
            IsLock = islock;
        }
        /// <summary>
        /// 支付置为成功
        /// </summary>
        public void PayToSuccess()
        {
            IsPay = true;
        }
    }
}
