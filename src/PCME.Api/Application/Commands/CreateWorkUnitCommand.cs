using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class CreateWorkUnitCommand:IRequest<bool>
    {
        public int Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }

        public int Level { get; private set; }

        public string LinkMan { get; private set; }

        public string LinkPhone { get; private set; }

        public string Email { get; private set; }

        public string Address { get; private set; }

        public int? PID { get; private set; }
        
        public int WorkUnitNatureId { get; private set; }


        //private readonly List<CreateWorkUnitCommand> _childs;
        //public IReadOnlyCollection<CreateWorkUnitCommand> Childs => _childs;

        //public CreateWorkUnitCommand Parent { get; private set; }

        public CreateWorkUnitCommand(int id,string code, string name, int level, string linkMan, string linkPhone,
            string email, string address, int? pID, int workUnitNatureId)
        {
            Id = id;
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Level = level;
            LinkMan = linkMan ?? throw new ArgumentNullException(nameof(linkMan));
            LinkPhone = linkPhone ?? throw new ArgumentNullException(nameof(linkPhone));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Address = address;
            PID = pID;
            WorkUnitNatureId = workUnitNatureId;

            //_childs = new List<CreateWorkUnitCommand>();
        }
    }
}
