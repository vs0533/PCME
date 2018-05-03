using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class TrainApply2
    {
        public int Id { get; set; }
        public string PersonId { get; set; }
        public string SubjectId { get; set; }
        public int TrainStationId { get; set; }
        public bool? IsConfirm { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Isenable { get; set; }
        public int WorkScore { get; set; }
        public decimal? Arrearage { get; set; }
        public bool? IsEnable2 { get; set; }
        public int? Yx { get; set; }
    }
}
