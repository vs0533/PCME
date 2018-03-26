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

        public int UnitNatureId { get; private set; }


        private readonly List<Unit> _childs;
        public IReadOnlyCollection<Unit> Childs => _childs;

        public Unit Parent { get; private set; }


        [Timestamp]
        public byte[] Version { get; set; }
    }
}
