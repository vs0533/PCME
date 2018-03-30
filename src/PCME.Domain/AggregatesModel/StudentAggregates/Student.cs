using Microsoft.AspNetCore.Identity;
using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace PCME.Domain.AggregatesModel.StudentAggregates
{
    public class Student : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string IDCard { get; private set; }
        public Sex Sex { get; private set; }
        public StudentType Type { get; private set; }
        public string Password { get; private set; }

        public DateTime? BirthDay { get; private set; }
        /// <summary>
        /// 毕业院校
        /// </summary>
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
        public string Address { get; private set; }

        public decimal Balance { get; private set; }

        

        public Student()
        {

        }
        public Student(string name, string idcard, string password,StudentType studentType)
            :this(name, idcard, password, Sex.Unknown,studentType,null,string.Empty
                 ,string.Empty,null,string.Empty,string.Empty,false,
                 string.Empty,false,string.Empty,Decimal.Zero)
        {

        }
        public Student(string name, string idcard, string password, Sex sex,StudentType type)
            :this(name, idcard, password, sex,type,null,string.Empty
                 ,string.Empty,null,string.Empty,string.Empty,false,
                 string.Empty,false,string.Empty,Decimal.Zero)
        {
        }
        public Student(string name,string idcard,string password,Sex sex,StudentType type,
            DateTime? birthDay,string graduationSchool,string specialty,
            DateTime? workdate,string officeName,string photo,bool photoIsValid,
            string email,bool emailIsValid,string address,decimal balance
            )
        {
            Type = type;
            Name = name;
            IDCard = idcard;
            Password = password;
            Sex = sex;
            BirthDay = birthDay;
            GraduationSchool = graduationSchool;
            Specialty = specialty;
            WorkDate = workdate;
            OfficeName = officeName;
            Photo = photo;
            PhotoIsValid = PhotoIsValid;
            Email = email;
            EmailIsValid = EmailIsValid;
            Address = address;
            Balance = balance;
        }
        
    }
}
