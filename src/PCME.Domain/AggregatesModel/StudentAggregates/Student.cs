using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.SeedWork;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCME.Domain.AggregatesModel.StudentAggregates
{
	public class Student : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
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
        [ForeignKey("WorkUnitId")]
        public WorkUnit WorkUnit { get; private set; }
        public int WorkUnitId { get; private set; }
        public StudentStatus StudentStatus{get;private set;}
        public int StudentStatusId { get; private set; }

        public Student()
        {

        }
        public Student(string name, string idcard, string password, int studentTypeId, int professionaltitleid, int workUnitId)
            : this(name, idcard, password, Sex.Unknown.Id, studentTypeId, null, string.Empty
                 , string.Empty, null, string.Empty, string.Empty, false,
                 string.Empty, false, string.Empty, Decimal.Zero,Decimal.Zero, workUnitId,StudentStatus.Normal.Id)
        {

        }
        public Student(string name, string idcard, string password, int sex, int studentTypeId, int professionaltitleid, int workUnitId)
            : this(name, idcard, password, sex, studentTypeId, null, string.Empty
                 , string.Empty, null, string.Empty, string.Empty, false,
                 string.Empty, false, string.Empty, Decimal.Zero,Decimal.Zero, workUnitId,StudentStatus.Normal.Id)
        {
        }
        public Student(string name, string idcard, string password, int sex, int studentTypeId,
            DateTime? birthDay, string graduationSchool, string specialty,
            DateTime? workdate, string officeName, string photo, bool photoIsValid,
            string email, bool emailIsValid, string address, decimal balanceActual,decimal balanceVirtual,
            int workUnitId,int studentStatus
            )
        {
            StudentTypeId = StudentType.From(studentTypeId).Id;
            Name = name;
            IDCard = idcard;
            Password = password;
            SexId = Sex.From(sex).Id;
            BirthDay = birthDay;
            GraduationSchool = graduationSchool;
            Specialty = specialty;
            WorkDate = workdate;
            OfficeName = officeName;
            Photo = photo;
            PhotoIsValid = photoIsValid;
            Email = email;
            EmailIsValid = EmailIsValid;
            Address = address;
            BalanceActual = balanceActual;
            BalanceVirtual = balanceVirtual;
            WorkUnitId = workUnitId;
            StudentStatusId = studentStatus;
        }

        public void Update(string name, string password, int sex, int studentTypeId,
            DateTime? birthDay, string graduationSchool, string specialty,
            DateTime? workdate, string officeName,
            string address, int studentStatus
            )
        {
            StudentTypeId = StudentType.From(studentTypeId).Id;
            Name = name;
            Password = password;
            SexId = Sex.From(sex).Id;
            BirthDay = birthDay;
            GraduationSchool = graduationSchool;
            Specialty = specialty;
            WorkDate = workdate;
            OfficeName = officeName;
            EmailIsValid = EmailIsValid;
            Address = address;
            StudentStatusId = studentStatus;
        }

        public void UpdateWorkUnit(int workUnitId) {
            WorkUnitId = WorkUnitId;
        }

        public void ChangeStudentStatus(int statusId) {
            StudentStatusId = StudentStatus.From(statusId).Id;
        }

    }
}
