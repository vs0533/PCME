using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExaminationRoomAccountAggregates
{
    public class ExaminationRoomAccount:Entity,IAggregateRoot
    {
        public string AccountName { get; private set; }
        public string Password { get; private set; }
        public int TrainingCenterId { get; private set; }
        public int ExaminationRoomId { get; private set; }
        public DateTime CreateTime { get; private set; }
        public ExaminationRoomAccount()
        {

        }
        public void Update(string accountName, string password, int trainingCenterId, int examinationRoomId)
        {
            AccountName = accountName ?? throw new ArgumentNullException(nameof(accountName));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            TrainingCenterId = trainingCenterId;
            ExaminationRoomId = examinationRoomId;
        }

        public ExaminationRoomAccount(string accountName, string password, int trainingCenterId, int examinationRoomId, DateTime createTime)
        {
            AccountName = accountName ?? throw new ArgumentNullException(nameof(accountName));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            TrainingCenterId = trainingCenterId;
            ExaminationRoomId = examinationRoomId;
            CreateTime = createTime;
        }
    }
}
