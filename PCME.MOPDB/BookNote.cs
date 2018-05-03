using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class BookNote
    {
        public int Id { get; set; }
        public string PersonId { get; set; }
        public int BookId { get; set; }
        public int PxdId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsPost { get; set; }
        public string SubjectId { get; set; }
        public bool? IsEnable { get; set; }
    }
}
