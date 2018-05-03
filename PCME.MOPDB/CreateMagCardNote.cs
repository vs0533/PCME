using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class CreateMagCardNote
    {
        public int Id { get; set; }
        public string PersonId { get; set; }
        public string MagCard { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateAdmin { get; set; }
        public bool Istechnician { get; set; }
        public bool Isunit { get; set; }
        public bool IscCivilServant { get; set; }
        public int RandomKey { get; set; }
    }
}
