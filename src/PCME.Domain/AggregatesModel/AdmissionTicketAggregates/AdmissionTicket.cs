using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.AdmissionTicketAggregates
{
    /// <summary>
    /// 准考证-考试完毕后会删除
    /// </summary>
    public class AdmissionTicket:Entity,IAggregateRoot
    {
        public string Num { get; private set; }
        public int StudentId { get; private set; }
        public int ExaminationRoomId { get; private set; }
        public int SignUpId { get; private set; }
        public int ExamSubjectId { get; private set; }
        public DateTime? SignInTime { get; private set; }
        public DateTime? LoginTime { get; private set; }
        public DateTime? PostPaperTime { get; private set; }
        public DateTime CreateTime { get; private set; }
        public int ExaminationRoomPlanId { get; private set; }
        public int ExamRoomPlanTicketId { get; private set; }

        public AdmissionTicket()
        {

        }
        public void SignIn() {
            SignInTime = DateTime.Now;
        }

        public AdmissionTicket(string num, int studentId, int examinationRoomId, int signUpId, 
            int examSubjectId, DateTime? signInTime, DateTime? loginTime, DateTime? postPaperTime,
            DateTime createTime, int examinationRoomPlanId,int examroomplanticketid)
        {
            Num = num;
            StudentId = studentId;
            ExaminationRoomId = examinationRoomId;
            SignUpId = signUpId;
            ExamSubjectId = examSubjectId;
            SignInTime = signInTime;
            LoginTime = loginTime;
            PostPaperTime = postPaperTime;
            CreateTime = createTime;
            ExaminationRoomPlanId = examinationRoomPlanId;
            ExamRoomPlanTicketId = examroomplanticketid;
        }
    }
}
