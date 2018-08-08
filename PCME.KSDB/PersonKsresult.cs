using System;
using System.Collections.Generic;

namespace PCME.KSDB
{
    public partial class PersonKsresult
    {
        public int Id { get; set; }
        public string Zkzcode { get; set; }
        public string SubjectId { get; set; }
        public string Ccxh { get; set; }
        public DateTime Createdate { get; set; }
        public string PersonId { get; set; }
        public int Score { get; set; }
        public DateTime ExamDate { get; set; }
    }
}
