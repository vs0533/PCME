using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class PersonTechnician
    {
        public PersonTechnician()
        {
            FruitAudit = new HashSet<FruitAudit>();
            TrainAudit = new HashSet<TrainAudit>();
        }

        public string PersonId { get; set; }
        public int DutyId { get; set; }
        public DateTime? RepresentDate { get; set; }
        public DateTime? CountDate { get; set; }
        public string TopQualification { get; set; }
        public string AuditCategory { get; set; }
        public string AuditUnitId { get; set; }
        public int? CreditHours { get; set; }

        public Unit AuditUnit { get; set; }
        public DirectoryZwName Duty { get; set; }
        public Person Person { get; set; }
        public ICollection<FruitAudit> FruitAudit { get; set; }
        public ICollection<TrainAudit> TrainAudit { get; set; }
    }
}
