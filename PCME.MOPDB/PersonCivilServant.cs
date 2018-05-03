using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class PersonCivilServant
    {
        public PersonCivilServant()
        {
            CivilTrainAudit = new HashSet<CivilTrainAudit>();
        }

        public string PersonId { get; set; }
        public string ChiefDuty { get; set; }
        public string DutyLevel { get; set; }
        public DateTime RepresentDate { get; set; }
        public string RawQualification { get; set; }
        public string TopQualification { get; set; }
        public bool IsJoinPromotion { get; set; }
        public string AuditUnitId { get; set; }

        public CivilServantDutyLevel DutyLevelNavigation { get; set; }
        public Person Person { get; set; }
        public ICollection<CivilTrainAudit> CivilTrainAudit { get; set; }
    }
}
