﻿using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.AdmissionTicketLogAggregates
{
    public class AdmissionTicketLogs:Entity,IAggregateRoot
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
        public AdmissionTicketLogs()
        {

        }

        public AdmissionTicketLogs(string num, int studentId, int examinationRoomId, int signUpId, int examSubjectId, DateTime? signInTime, DateTime? loginTime, DateTime? postPaperTime, DateTime createTime, int examinationRoomPlanId)
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
        }
    }
}
