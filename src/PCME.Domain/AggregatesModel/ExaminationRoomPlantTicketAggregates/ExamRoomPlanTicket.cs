using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExaminationRoomPlantTicketAggregates
{
    /// <summary>
    /// 考试券 用于选场 生成->准考证
    /// </summary>
    public class ExamRoomPlanTicket:Entity,IAggregateRoot
    {
        public string Num { get; private set; }
        public int StudentId { get; private set; }
        public int TrainingCenterId { get; private set; }
        /// <summary>
        /// 是否被花掉
        /// </summary>
        public bool IsExpense { get; private set; }
        public string ReMark { get; private set; }
        public DateTime CreateTime { get; private set; }
        public DateTime? ExpenseTime { get; private set; }
        public ExamRoomPlanTicket()
        {

        }

        public ExamRoomPlanTicket(string num,int studentId, int trainingCenterId,string reMark="报名创建")
        {
            Num = num.PadRight(4,'0') + DateTime.Now.ToString("yyMMddhhssmm");
            StudentId = studentId;
            TrainingCenterId = trainingCenterId;
            IsExpense = false;
            ReMark = reMark ?? throw new ArgumentNullException(nameof(reMark));
            CreateTime = DateTime.Now;
        }

        public void DoExpense(string reMark="选场消费") {
            IsExpense = false;
            ExpenseTime = DateTime.Now;
        }
    }
}
