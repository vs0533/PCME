using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class CivilTrainAudit
    {
        public int TrainId { get; set; }
        public string PersonId { get; set; }
        public string TrainName { get; set; }
        public int? TrainTypeId { get; set; }
        public string FrontUnit { get; set; }
        public DateTime? TrainDate { get; set; }
        public int? NumberOfDay { get; set; }
        public decimal? Result { get; set; }
        public string TrainDuty { get; set; }
        public int? CreditHour { get; set; }
        public int? AuditState { get; set; }

        public PersonCivilServant Person { get; set; }
        public CivilTrainType TrainType { get; set; }
    }
}
