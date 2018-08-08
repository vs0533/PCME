using System;
using System.Collections.Generic;

namespace PCME.KSDB
{
    public partial class PersonCopy
    {
        public int Id { get; set; }
        public string PersonId { get; set; }
        public string PersonName { get; set; }
        public string Idcard { get; set; }
        public string PassWord { get; set; }
        public string Sex { get; set; }
        public string SubjectId { get; set; }
        public string SubjectName { get; set; }
        public DateTime? GoToTime { get; set; }
        public DateTime? StartKstime { get; set; }
        public DateTime? OverKstime { get; set; }
        public string Ksstate { get; set; }
        public string PersonPhoto { get; set; }
        public string PersonZkz { get; set; }
        public string Ccxh { get; set; }
        public string MagCard { get; set; }
        public int? RandomKey { get; set; }
        public int KdunitId { get; set; }
        public string WorkUnitName { get; set; }
        public bool IsZb { get; set; }
        public bool IsTk { get; set; }
        public int IsReSet { get; set; }
        public DateTime? PersonStartKstime { get; set; }
        public int? Workscore { get; set; }
    }
}
