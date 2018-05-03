using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class TeachBook
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Agio { get; set; }
        public string Publisher { get; set; }
        public decimal? Dmoney { get; set; }
        public decimal TrueMoney { get; set; }
        public string BookCode { get; set; }
        public string SubjectId { get; set; }
        public bool? IsSelect { get; set; }
    }
}
