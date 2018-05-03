using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class PersonSignIn
    {
        public int Id { get; set; }
        public int PersonClassCardId { get; set; }
        public DateTime SignInTime { get; set; }
        public DateTime? SignOutTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
