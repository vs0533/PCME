using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PCME.Api.Infrastructure.ModelBinder;
using PCME.Domain.AggregatesModel.UnitAggregates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class CreateOrUpdateWorkUnitCommand:IRequest<WorkUnit>
    {
        public int Id { get;  set; }
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
        public int? PID { get; set; }
        [Required(ErrorMessage = "单位类型必须选择")]
        [JsonProperty("WorkUnitNature.Id")]
        public int WorkUnitNatureId { get; private set; }

        public int WorkUnitAccountTypeId { get; private set; }


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

        public void NewInitData(int pid,int curLevel)
        {
            Id = 0;
            PID = pid;
            Level = curLevel+1;
        }
    }
}
