using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using PCME.Domain.AggregatesModel.AdmissionTicketAggregates;
using PCME.Domain.AggregatesModel.ApplicationForm;

namespace PCME.Api.Application.Commands
{
    public class StudentItemDto
    {
        public int StudentId
        {
            get;
            private set;
        }
        public int ApplyTableId
        {
            get;
            private set;
        }

        public StudentItemDto(int studentId, int applyTableId)
        {
            StudentId = studentId;
            ApplyTableId = applyTableId;
        }
    }
    public class ApplyTableCreateOrUpdateCommand:IRequest<Dictionary<string,object>>
    {
        public int Id { get; private set; }
        public int WorkUnitId
        {
            get;
            private set;
        }

        private readonly List<StudentItemDto> _studentItems;

        public IReadOnlyCollection<StudentItemDto> StudentItems => _studentItems;
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

        public ApplyTableCreateOrUpdateCommand()
        {
            _studentItems = new List<StudentItemDto>();
        }

        public ApplyTableCreateOrUpdateCommand(int workUnitId, DateTime createTime, string num)
        {
            _studentItems = new List<StudentItemDto>();
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
            else
            {
                StudentItemDto item = new StudentItemDto(studentId, applyTableId);
                _studentItems.Add(item);
            }
        }
    }
}
