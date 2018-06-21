using MediatR;
using Newtonsoft.Json;
using PCME.Domain.AggregatesModel.BookAggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class BookCreateOrUpdateCommand:IRequest<Dictionary<string,object>>
    {
        public int Id { get; private set; }
        [JsonProperty("books.Num")]
        public string Num { get; private set; }
        [JsonProperty("books.Name")]
        public string Name { get; private set; }
        [JsonProperty("books.PublishingHouse")]
        public string PublishingHouse { get; private set; }
        [JsonProperty("examsubjects.Id")]
        public int ExamSubjectId { get; private set; }
        [JsonProperty("books.Pirce")]
        public decimal Pirce { get; private set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public float Discount { get; private set; }

        public void SetId(int id) {
            Id = id;
        }

        public BookCreateOrUpdateCommand(int id,string num, string name, string publishingHouse, int examSubjectId, decimal pirce = 0, float discount = 0)
        {
            Id = id;
            Num = num ?? throw new ArgumentNullException(nameof(num));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            PublishingHouse = publishingHouse ?? throw new ArgumentNullException(nameof(publishingHouse));
            ExamSubjectId = examSubjectId;
            Pirce = pirce;
            Discount = discount;
        }
    }
}
