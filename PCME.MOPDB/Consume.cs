using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class Consume
    {
        public int Id { get; set; }
        public string ConsumeName { get; set; }
        public DateTime ConsumeTime { get; set; }
        public string ConsumePersonId { get; set; }
        public decimal ConsumeMoney { get; set; }
        public string ConsumeHandle { get; set; }
        public string ConsumeType { get; set; }
        public int? Type { get; set; }
    }
}
