using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class PaperCount
    {
        public int Id { get; set; }
        public string AreaLevel { get; set; }
        public string AwardLevel { get; set; }
        public string PaperType { get; set; }
        public int? CreditHour { get; set; }
    }
}
