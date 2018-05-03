using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class PersonClassCard
    {
        public int Id { get; set; }
        public int PxdExamSubjectRoomId { get; set; }
        public string PersonId { get; set; }
        public string TicketCode { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
