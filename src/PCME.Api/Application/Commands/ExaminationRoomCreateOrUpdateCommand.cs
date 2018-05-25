using MediatR;
using PCME.Domain.AggregatesModel.ExaminationRoomAggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class ExaminationRoomCreateOrUpdateCommand: IRequest<ExaminationRoom>
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Galleryful { get; private set; }
        public string Description { get; private set; }
        public int TrainingCenterId { get; private set; }
        
        public ExaminationRoomCreateOrUpdateCommand(int id,string name, int galleryful, int trainingcenterid, string description)
        {
            Id = id;
            Name = name;
            Galleryful = galleryful;
            TrainingCenterId = trainingcenterid;
            Description = description;
        }
        public void SetId(int id) {
            Id = id;
        }
        public void SetTrainingCenterId(int trainingcenterId) {
            TrainingCenterId = trainingcenterId;
        }
    }
}
