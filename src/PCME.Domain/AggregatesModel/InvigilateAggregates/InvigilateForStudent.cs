using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.InvigilateAggregates
{
    /// <summary>
    /// 学员监考情况
    /// </summary>
    public class InvigilateForStudent:Entity,IAggregateRoot
    {
        public string TicketNum { get; private set; }
        public string Type { get; private set; }
        public int ExaminationRoomPlantId { get; private set; }
        public DateTime CreateTime { get; private set; }

        public InvigilateForStudent()
        {

        }

        public InvigilateForStudent(string ticketNum, string type, int examinationRoomPlantId)
        {
            TicketNum = ticketNum ?? throw new ArgumentNullException(nameof(ticketNum));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            ExaminationRoomPlantId = examinationRoomPlantId;
            CreateTime = DateTime.Now;
        }
        public void SetZB(){
            Type = "作";
        }
        public void SetTI() {
            Type = "替";
        }
        public void SetCH() {
            Type = "重";
        }
    }
}
