using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class SignUpStudentCreateOrUpdateCommand:IRequest<Dictionary<string,object>>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int StudentId { get; set; }
        [JsonProperty("trainingcenter")]
        public int TrainingCenterId { get; private set; }
        public DateTime CreateTime { get; private set; }
        [JsonProperty("examsubject")]
        public IEnumerable<SignUpStudentCollectionDTO> CollectionDTO { get; private set; }
        public SignUpStudentCreateOrUpdateCommand()
        {
            CollectionDTO = new List<SignUpStudentCollectionDTO>();
        }

        public SignUpStudentCreateOrUpdateCommand(int id, int trainingCenterId, List<SignUpStudentCollectionDTO> collectionDTO)
        {
            Id = id;
            TrainingCenterId = trainingCenterId;
            CollectionDTO = collectionDTO;// ?? throw new ArgumentNullException(nameof(collectionDTO));
        }
    }
    public class SignUpStudentCollectionDTO {
        [JsonProperty("examsubjectid")]
        public int ExamSubjectId { get; private set; }
        public SignUpStudentCollectionDTO(int examsubjectid)
        {
            ExamSubjectId = examsubjectid;
        }
    }
}
