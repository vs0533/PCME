using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class Person
    {
        public Person()
        {
            PayCard = new HashSet<PayCard>();
        }

        public string PersonId { get; set; }
        public string PersonName { get; set; }
        public string Idcard { get; set; }
        public string Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string GraduateSchool { get; set; }
        public DateTime? GraduateDate { get; set; }
        public string GraduateSpecialty { get; set; }
        public string WorkSpecialty { get; set; }
        public DateTime? WorkDate { get; set; }
        public string WorkUnitId { get; set; }
        public int? DeptId { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public decimal? AccountMoney { get; set; }
        public string PersonIdentityId { get; set; }
        public int? PxdId { get; set; }
        public int? Pxdidgwy { get; set; }

        public PersonIdentity PersonIdentity { get; set; }
        public Unit WorkUnit { get; set; }
        public PersonCivilServant PersonCivilServant { get; set; }
        public PersonTechnician PersonTechnician { get; set; }
        public ICollection<PayCard> PayCard { get; set; }
    }
}
