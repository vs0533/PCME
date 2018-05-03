using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class PersonPhoto
    {
        public int Id { get; set; }
        public string PersonId { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime Createdate { get; set; }
        public bool IsOk { get; set; }
    }
}
