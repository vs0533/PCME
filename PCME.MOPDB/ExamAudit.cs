using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class ExamAudit
    {
        public string ExamId { get; set; }
        public string PersonId { get; set; }
        public decimal SumResult { get; set; }
        public int? CreditHour { get; set; }
        public int ResultState { get; set; }
        public int Id { get; set; }
        public DateTime? ExamDate { get; set; }
    }
}
