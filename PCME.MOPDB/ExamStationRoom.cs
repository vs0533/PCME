using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class ExamStationRoom
    {
        public string SubjectId { get; set; }
        public string OrderId { get; set; }
        public string ExamStationId { get; set; }
        public string RoomId { get; set; }
        public int NumberOfSeatUsed { get; set; }

        public ExamStation ExamStation { get; set; }
        public ExamSubject Subject { get; set; }
    }
}
