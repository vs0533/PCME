using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class ExamOrderBelongExamStation
    {
        public string ExamStationId { get; set; }
        public string ExamOrderId { get; set; }
        public string SubjectId { get; set; }
        public string StartRoomNumber { get; set; }
        public string EndRoomNumber { get; set; }
    }
}
