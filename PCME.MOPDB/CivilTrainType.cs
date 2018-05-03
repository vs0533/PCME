using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class CivilTrainType
    {
        public CivilTrainType()
        {
            CivilTrainAudit = new HashSet<CivilTrainAudit>();
        }

        public int TypeId { get; set; }
        public string TypeName { get; set; }

        public ICollection<CivilTrainAudit> CivilTrainAudit { get; set; }
    }
}
