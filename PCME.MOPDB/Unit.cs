using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class Unit
    {
        public Unit()
        {
            Account = new HashSet<Account>();
            Person = new HashSet<Person>();
            PersonTechnician = new HashSet<PersonTechnician>();
            UnitDept = new HashSet<UnitDept>();
        }

        public string UnitId { get; set; }
        public string UnitName { get; set; }
        public int? TypeId { get; set; }
        public string Linkman { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public UnitType Type { get; set; }
        public ICollection<Account> Account { get; set; }
        public ICollection<Person> Person { get; set; }
        public ICollection<PersonTechnician> PersonTechnician { get; set; }
        public ICollection<UnitDept> UnitDept { get; set; }
    }
}
