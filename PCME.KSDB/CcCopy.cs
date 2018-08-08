using System;
using System.Collections.Generic;

namespace PCME.KSDB
{
    public partial class CcCopy
    {
        public int Id { get; set; }
        public string Xh { get; set; }
        public DateTime IsSelectDate { get; set; }
        public DateTime IsOverSelectDate { get; set; }
        public DateTime Ksdate { get; set; }
        public int Rs { get; set; }
        public int State { get; set; }
        public string IsOpen { get; set; }
        public DateTime? IsOpenDate { get; set; }
        public int KdunitId { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateAccount { get; set; }
        public DateTime? KsoverDate { get; set; }
        public bool? IsRelax { get; set; }
    }
}
