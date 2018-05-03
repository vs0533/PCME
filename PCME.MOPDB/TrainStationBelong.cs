using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class TrainStationBelong
    {
        public string SubjectId { get; set; }
        public string TrainStationId { get; set; }

        public ExamSubject Subject { get; set; }
        public TrainStation TrainStation { get; set; }
    }
}
