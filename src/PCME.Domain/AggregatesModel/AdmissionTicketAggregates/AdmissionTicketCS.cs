using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.AdmissionTicketAggregates
{
    /// <summary>
    /// 准考证-(非集中考试)
    /// </summary>
    public class AdmissionTicketCS:Entity,IAggregateRoot
    {
        public string Num { get; private set; }
        public int StudentId { get; private set; }
        public int SignUpId { get; private set; }
        public int ExamSubjectId { get; private set; }
        public DateTime? LoginTime { get; private set; }
        public DateTime? PostPaperTime { get; private set; }
        public DateTime CreateTime { get; private set; }

        public AdmissionTicketCS()
        {

        }

        public void PostPaper() {
            PostPaperTime = DateTime.Now;
        }

        public AdmissionTicketCS(string num, int studentId, int signUpId, 
            int examSubjectId, DateTime? loginTime, DateTime? postPaperTime,
            DateTime createTime)
        {
            Num = num;
            StudentId = studentId;
            SignUpId = signUpId;
            ExamSubjectId = examSubjectId;
            LoginTime = loginTime;
            PostPaperTime = postPaperTime;
            CreateTime = createTime;
        }
        public void Login()
        {
            if (LoginTime == null) {
                LoginTime = DateTime.Now;
            }
        }
        public void AddLoginTime() {
            if (LoginTime != null)
            {
                LoginTime = LoginTime.GetValueOrDefault().AddMinutes(5);
            }
        }
    }
}
