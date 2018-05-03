using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class ExamAuditTemp
    {
        public string ExamId { get; set; }
        public string Idcard { get; set; }
        public string PersonName { get; set; }
        public decimal SumResult { get; set; }
        public int ResultState { get; set; }
        public int InsertState { get; set; }
        public int Id { get; set; }
        public DateTime? ExamDate { get; set; }
    }
}
