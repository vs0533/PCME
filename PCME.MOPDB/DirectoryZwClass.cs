using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class DirectoryZwClass
    {
        public DirectoryZwClass()
        {
            DirectoryZwName = new HashSet<DirectoryZwName>();
        }

        public string ClassId { get; set; }
        public string ClassName { get; set; }
        public string Pxclass { get; set; }

        public ICollection<DirectoryZwName> DirectoryZwName { get; set; }
    }
}
