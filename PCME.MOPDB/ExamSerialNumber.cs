using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class ExamSerialNumber
    {
        public string ExamSerialNumber1 { get; set; }
        public string SubjectId { get; set; }
        public string ExamStationId { get; set; }
        public string OrderId { get; set; }
        public string RoomNumber { get; set; }
        public string SeatNumber { get; set; }
        public string PersonId { get; set; }

        public ExamStation ExamStation { get; set; }
        public ExamOrder Order { get; set; }
    }
}
