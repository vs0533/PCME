using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.InvigilateAggregates
{
    public class InvigilateForRoomPlant:Entity,IAggregateRoot
    {
        public int ExaminationRoomPlantId { get; private set; }
        public string ReMark { get; private set; }
        public string TecherName { get; private set; }
        public DateTime CreateTime { get; private set; }
        /// <summary>
        /// 违纪
        /// </summary>
        public int WJCtr { get; private set; }
        /// <summary>
        /// 缺考
        /// </summary>
        public int QKCtr { get; private set; }
        /// <summary>
        /// 实考
        /// </summary>
        public int SKCtr { get; private set; }
        public InvigilateForRoomPlant()
        {

        }

        public InvigilateForRoomPlant(int examinationRoomPlantId, string reMark, string techerName, DateTime createTime, int wJCtr, int qKCtr, int sKCtr)
        {
            ExaminationRoomPlantId = examinationRoomPlantId;
            ReMark = reMark;
            TecherName = techerName ?? throw new ArgumentNullException(nameof(techerName));
            CreateTime = DateTime.Now;
            WJCtr = wJCtr;
            QKCtr = qKCtr;
            SKCtr = sKCtr;
        }
    }
}
