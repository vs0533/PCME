using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class PxdExamSubjectRoom
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Pxtime { get; set; }
        public int PxdUnitId { get; set; }
        public string SubjectId { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsActive { get; set; }
        public string Pxcontent { get; set; }
        public string Pxaddress { get; set; }
        public int PersonCount { get; set; }
    }
}
