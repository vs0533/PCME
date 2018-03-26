using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExaminationRoomAggregates
{
    public class ExaminationRoom:Entity,IAggregateRoot
    {
        public string Name { get; private set; }
        public int Galleryful { get; private set; }
        public string Description { get; private set; }
        public int TrainingCenterId { get; private set; }

        public ExaminationRoom(string name,int galleryful,int trainingcenterid,string description)
        {
            Name = name;
            Galleryful = galleryful;
            TrainingCenterId = trainingcenterid;
            Description = description;
        }
    }
}
