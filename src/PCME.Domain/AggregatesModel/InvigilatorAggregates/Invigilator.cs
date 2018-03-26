using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.InvigilatorAggregates
{
    public class Invigilator:Entity,IAggregateRoot
    {
        public string Name { get; private set; }
        public int ExaminationRoomId { get; private set; }
        public string PassWord { get; private set; }

        public Invigilator()
        {

        }
        public Invigilator(string name,string password,int examinationRoomId)
        {
            Name = name;
            PassWord = password;
            ExaminationRoomId = examinationRoomId;
        }
    }
}
