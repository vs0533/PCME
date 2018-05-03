using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class BookPostNote
    {
        public int Id { get; set; }
        public int PostBookId { get; set; }
        public int PostbmdAccountId { get; set; }
        public string PostPersonId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsPrint { get; set; }
    }
}
