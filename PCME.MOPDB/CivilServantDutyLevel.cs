using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class CivilServantDutyLevel
    {
        public CivilServantDutyLevel()
        {
            PersonCivilServant = new HashSet<PersonCivilServant>();
        }

        public string DutyId { get; set; }
        public string DutyName { get; set; }

        public ICollection<PersonCivilServant> PersonCivilServant { get; set; }
    }
}
