using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PCME.Domain.AggregatesModel.UnitAggregates
{
    public class Unit:Entity,IAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }

        public int Level { get; private set; }

        public string LinkMan { get;private set; }

        public string LinkPhoto { get; private set; }

        public string Email { get;private set; }

        public string Address { get;private set; }

        public int? PID { get; private set; }

        public UnitNature UnitNature { get; private set; }


        private readonly List<Unit> _childs;
        public IReadOnlyCollection<Unit> Childs => _childs;

        public Unit Parent { get; private set; }


        [Timestamp]
        public byte[] Version { get; set; }

        public Unit(string code, string name, int level, string linkMan, 
            string linkPhoto, string email, string address, int? pID,
            UnitNature unitNature)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Level = level;
            LinkMan = linkMan ?? throw new ArgumentNullException(nameof(linkMan));
            LinkPhoto = linkPhoto ?? throw new ArgumentNullException(nameof(linkPhoto));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            PID = pID;
            UnitNature = unitNature ?? throw new ArgumentNullException(nameof(unitNature));
        }
    }
}
