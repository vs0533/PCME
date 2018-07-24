using MediatR;
using Newtonsoft.Json;
using PCME.Api.Infrastructure.Validation;
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
        [StringAndCharacter(ErrorMessage ="不能包含非法字符")]
        public string Name { get; private set; }
        [Required(ErrorMessage ="身份证号必须填写")]
        [RegularExpression(@"(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)", ErrorMessage = "身份证号码格式错误")]
        public string IDCard { get; private set; }
        public Sex Sex { get; private set; }
        [Required(ErrorMessage = "性别必须选择")]
        [JsonProperty("Sex.Id")]
        public int SexId { get; private set; }
        public StudentType StudentType { get; private set; }
        [Required(ErrorMessage = "学员类型必须选择")]
        [JsonProperty("StudentType.Id")]
        public int StudentTypeId { get; private set; }
        public string Password { get; private set; }
        public string Favicon { get; private set; }

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
        [Required(ErrorMessage = "学员状态必须选择")]
        [JsonProperty("StudentStatus.Id")]
        public int StudentStatusId { get; private set; }
        public bool JoinEdu { get; private set; }
        public void SetPhoto(string photo) {
            Photo = photo;
        }
        public StudentCreateOrUpdateCommand(int id, string name, string iDCard, int sexId, int studentTypeId,
            string password, DateTime? birthDay, string graduationSchool, string specialty,
            DateTime? workDate, string officeName,  string email, string address, int workUnitId, int studentStatusId, string favicon,bool joinEdu)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            IDCard = iDCard ?? throw new ArgumentNullException(nameof(iDCard));
            SexId = sexId;
            StudentTypeId = studentTypeId;
            Password = password ?? throw new ArgumentNullException(nameof(password));
            BirthDay = birthDay ?? throw new ArgumentNullException(nameof(birthDay));
            GraduationSchool = graduationSchool;
            Specialty = specialty;
            WorkDate = workDate;
            OfficeName = officeName ?? throw new ArgumentNullException(nameof(officeName));
            Email = email;
            Address = address;
            WorkUnitId = workUnitId;
            StudentStatusId = studentStatusId;
            Favicon = favicon;
            JoinEdu = joinEdu;
        }

        public void SetWorkUnitId(int workUnitId) {
            WorkUnitId = workUnitId;
        }
        public void SetId(int id) {
            Id = id;
        }
    }
}
