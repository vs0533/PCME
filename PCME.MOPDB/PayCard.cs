using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class PayCard
    {
        public string CardId { get; set; }
        public string SerialNumber { get; set; }
        public string Password { get; set; }
        public decimal? Cost { get; set; }
        public string Type { get; set; }
        public DateTime? CanUseDate { get; set; }
        public bool? IsUsed { get; set; }
        public string UserPersonId { get; set; }

        public Person UserPerson { get; set; }
    }
}
