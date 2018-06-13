using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.SignUpAggregates
{
    /// <summary>
    /// 正式报名表 生成场次后删除
    /// </summary>
    public class SignUp:Entity,IAggregateRoot
    {
        #region 符合主键不允许重复
        public int StudentId { get; private set; }
        public int ExamSubjectId { get; private set; }
        #endregion

        /// <summary>
        /// null 的话是个人报名
        /// </summary>
        public int? SignUpForUnitId { get; private set; }
        public int TrainingCenterId { get; private set; }

        /// <summary>
        /// 是否生成了准考证
        /// </summary>
        public bool TicketIsCreate { get; private set; }

        public DateTime CreateTime { get; private set; }
        public SignUp()
        {

        }
        public void TicketChangeCreate() {
            TicketIsCreate = true;
        }

        public SignUp(int studentId, int examSubjectId, int? signUpForUnitId, int trainingCenterId, bool ticketIsCreate, DateTime createTime)
        {
            StudentId = studentId;
            ExamSubjectId = examSubjectId;
            SignUpForUnitId = signUpForUnitId;
            TrainingCenterId = trainingCenterId;
            TicketIsCreate = ticketIsCreate;
            CreateTime = createTime;
        }
    }
}
