using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExaminationRoomAggregates
{
    /// <summary>
    /// 教室管理
    /// </summary>
    public class ExaminationRoom:Entity,IAggregateRoot
    {
        public string Num { get; private set; }
        public string Name { get; private set; }
        /// <summary>
        /// 暂时弃用
        /// </summary>
        public int Galleryful { get; private set; }
        public string Description { get; private set; }
        public int TrainingCenterId { get; private set; }

        public ExaminationRoom()
        {

        }
        public ExaminationRoom(string num,string name,int galleryful,int trainingcenterid,string description)
        {
            Num = num;
            Name = name;
            Galleryful = galleryful;
            TrainingCenterId = trainingcenterid;
            Description = description;
        }

        public void Update(string name, int galleryful, string description) {
            Name = name;
            Galleryful = galleryful;
            Description = description;
        }
    }
}
