using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class DirectoryZwName
    {
        public DirectoryZwName()
        {
            PersonTechnician = new HashSet<PersonTechnician>();
        }

        public int Id { get; set; }
        public string ZwName { get; set; }
        public string ClassName { get; set; }
        public string ZcJb { get; set; }
        public string Zy { get; set; }
        public string Promotionway { get; set; }

        public DirectoryZwClass ClassNameNavigation { get; set; }
        public ICollection<PersonTechnician> PersonTechnician { get; set; }
    }
}
