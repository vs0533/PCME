using MediatR;
using PCME.Domain.AggregatesModel.StudentAggregates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class StudentCreateOrUpdateCommand:IRequest<Student>
    {
        public int Id { get; private set; }
        [Required(ErrorMessage ="人员姓名必须填写")]
        [RegularExpression(@"^[A-Za-z0-9|_\u4e00-\u9fa5]+$", ErrorMessage = "人员姓名不允许非法字符")]
        public string Name { get; private set; }
        [Required(ErrorMessage ="身份证号必须填写")]
        [RegularExpression(@"\d{17}[\d|x]|\d{15}", ErrorMessage = "身份证号码格式错误")]
        public string IDCard { get; private set; }
        public Sex Sex { get; private set; }
        public int SexId { get; private set; }
        public StudentType StudentType { get; private set; }
        public int StudentTypeId { get; private set; }
        public string Password { get; private set; }

        public DateTime? BirthDay { get; private set; }
        /// <summary>
        /// 毕业院校
        /// </summary>
        [MaxLength(60)]
        public string GraduationSchool { get; private set; }
        /// <summary>
        /// 所学专业
        /// </summary>
        public string Specialty { get; private set; }
        /// <summary>
        /// 参加工作时间
        /// </summary>
        public DateTime? WorkDate { get; private set; }
        public string OfficeName { get; private set; }
        public string Photo { get; private set; }
        public bool PhotoIsValid { get; private set; }
        public string Email { get; private set; }
        public bool EmailIsValid { get; private set; }
        [MaxLength(60)]
        public string Address { get; private set; }

        public decimal BalanceActual { get; private set; }
        public decimal BalanceVirtual { get; private set; }
        public int WorkUnitId { get; private set; }
        public int StudentStatusId { get; private set; }
    }
}
