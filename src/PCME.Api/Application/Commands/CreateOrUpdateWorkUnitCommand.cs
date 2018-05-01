using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class CreateOrUpdateWorkUnitCommand:IRequest<bool>
    {
        public int Id { get; private set; }
        [Required(ErrorMessage ="单位编码必须填写")]
        public string Code { get; private set; }
        [Required(ErrorMessage = "单位密码必须填写")]
        public string PassWord { get; private set; }
        [Required(ErrorMessage = "单位名称必须填写")]
        public string Name { get; private set; }
        [Required(ErrorMessage = "单位级别必须填写")]
        public int Level { get; private set; }
        [Required(ErrorMessage = "单位联系人必须填写")]
        public string LinkMan { get; private set; }
        [Required(ErrorMessage = "单位联系人电话必须填写")]
        public string LinkPhone { get; private set; }
        public string Email { get; private set; }

        public string Address { get; private set; }
        public int? PID { get; private set; }
        [Required(ErrorMessage = "单位类型必须选择")]
        public int WorkUnitNatureId { get; private set; }


        //private readonly List<CreateWorkUnitCommand> _childs;
        //public IReadOnlyCollection<CreateWorkUnitCommand> Childs => _childs;

        //public CreateWorkUnitCommand Parent { get; private set; }

        public CreateOrUpdateWorkUnitCommand(int id,string code,string passWord, string name, int level, string linkMan, string linkPhone,
            string email, string address, int? pID, int workUnitNatureId)
        {
            Id = id;
            PassWord = passWord;
            Code = code;
            Name = name;
            Level = level;
            LinkMan = linkMan;
            LinkPhone = linkPhone;
            Email = email;
            Address = address;
            PID = pID;
            WorkUnitNatureId = workUnitNatureId;

            //_childs = new List<CreateWorkUnitCommand>();
        }
    }
}
