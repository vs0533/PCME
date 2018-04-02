using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PCME.Domain.AggregatesModel.UnitAggregates
{
    public class WorkUnit:Entity,IAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }

        public int Level { get; private set; }

        public string LinkMan { get;private set; }

        public string LinkPhone { get; private set; }

        public string Email { get;private set; }

        public string Address { get;private set; }

        public int? PID { get; private set; }

        public WorkUnitNature UnitNature { get; private set; }
        public int WorkUnitNatureId { get; private set; }


        private readonly List<WorkUnit> _childs;
        public IReadOnlyCollection<WorkUnit> Childs => _childs;

        public WorkUnit Parent { get; private set; }


        [Timestamp]
        public byte[] Version { get; set; }

        public WorkUnit(string code, string name, int level, string linkMan, 
            string linkPhoto, string email, string address, int? pID,
            int workUnitNatureId)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Level = level;
            LinkMan = linkMan ?? throw new ArgumentNullException(nameof(linkMan));
            LinkPhone = linkPhoto ?? throw new ArgumentNullException(nameof(linkPhoto));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Address = address;
            PID = pID;
            WorkUnitNatureId = WorkUnitNatureId;
        }
    }
}
