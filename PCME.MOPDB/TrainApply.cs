using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class TrainApply
    {
        public string PersonId { get; set; }
        public string SubjectId { get; set; }
        public string TrainStationId { get; set; }
        public bool IsConfirm { get; set; }

        public ExamSubject Subject { get; set; }
    }
}
