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
        public string PassWord { get; private set; }
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

        public virtual WorkUnit Parent { get; private set; }


        [Timestamp]
        public byte[] Version { get; set; }

        public WorkUnit()
        {
            this._childs = new List<WorkUnit>();
        }

        public WorkUnit(string code,string passWord, string name, int level, string linkMan, 
            string linkPhoto, string email, string address, int? pID,
            int workUnitNatureId)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            PassWord = passWord ?? throw new ArgumentNullException(nameof(PassWord));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Level = level;
            LinkMan = linkMan ?? throw new ArgumentNullException(nameof(linkMan));
            LinkPhone = linkPhoto ?? throw new ArgumentNullException(nameof(linkPhoto));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Address = address;
            PID = pID;
            WorkUnitNatureId = workUnitNatureId;
        }

        public void Update(string name,string linkman,string email,string linkphone,string address)
        {
            Name = name;
            LinkMan = linkman;
            Email = email;
            LinkPhone = linkphone;
            Address = address;
        }

        public void ChangePassWord(string passWord) {
            PassWord = passWord ?? throw new ArgumentNullException(nameof(PassWord));
        }
    }
}
