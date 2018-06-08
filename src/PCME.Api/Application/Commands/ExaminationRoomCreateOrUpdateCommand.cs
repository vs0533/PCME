using MediatR;
using PCME.Domain.AggregatesModel.ExaminationRoomAggregates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class ExaminationRoomCreateOrUpdateCommand: IRequest<ExaminationRoom>
    {
        public int Id { get; private set; }
        [Required]
        [RegularExpression(@"^0?[0-9]{1,2}$", ErrorMessage = "编号必须是两位数的数字字符01-99")]
        public string Num { get; private set; }
        public string Name { get; private set; }
        public int Galleryful { get; private set; }
        public string Description { get; private set; }
        public int TrainingCenterId { get; private set; }
        
        public ExaminationRoomCreateOrUpdateCommand(int id,string num,string name, int galleryful, int trainingcenterid, string description)
        {
            Id = id;
            Num = num;
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
