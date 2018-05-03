using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class ExamStation
    {
        public ExamStation()
        {
            ExamSerialNumber = new HashSet<ExamSerialNumber>();
            ExamStationRoom = new HashSet<ExamStationRoom>();
        }

        public string StationId { get; set; }
        public string StationName { get; set; }
        public string StationAddress { get; set; }
        public int NumberOfRoom { get; set; }
        public string TrainStationId { get; set; }

        public ICollection<ExamSerialNumber> ExamSerialNumber { get; set; }
        public ICollection<ExamStationRoom> ExamStationRoom { get; set; }
    }
}
