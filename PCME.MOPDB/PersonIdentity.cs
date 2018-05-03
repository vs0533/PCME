using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class PersonIdentity
    {
        public PersonIdentity()
        {
            Person = new HashSet<Person>();
        }

        public string IdentityId { get; set; }
        public string IdentityName { get; set; }

        public ICollection<Person> Person { get; set; }
    }
}
