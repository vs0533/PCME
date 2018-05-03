using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class ExamOrder
    {
        public ExamOrder()
        {
            ExamSerialNumber = new HashSet<ExamSerialNumber>();
        }

        public string OrderId { get; set; }
        public DateTime? ExamStartDate { get; set; }
        public DateTime? ExamEndDate { get; set; }

        public ICollection<ExamSerialNumber> ExamSerialNumber { get; set; }
    }
}
