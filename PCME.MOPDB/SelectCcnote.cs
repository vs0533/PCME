using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class SelectCcnote
    {
        public int Id { get; set; }
        public string PersonId { get; set; }
        public string SubjectId { get; set; }
        public int Ccid { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsPrint { get; set; }
        public string Ksstate { get; set; }
        public DateTime? GoToTime { get; set; }
        public DateTime? StartKsdate { get; set; }
        public DateTime? OverKsdate { get; set; }
        public string Zkzcode { get; set; }
    }
}
