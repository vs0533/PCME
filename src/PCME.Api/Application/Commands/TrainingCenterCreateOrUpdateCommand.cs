using MediatR;
using PCME.Api.Infrastructure.Validation;
using PCME.Domain.AggregatesModel.TrainingCenterAggregates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Commands
{
    public class TrainingCenterCreateOrUpdateCommand: IRequest<TrainingCenter>
    {
        public int Id { get; private set; }
        [Required(ErrorMessage ="培训点名称必须填写")]
        [StringAndCharacter(ErrorMessage = "名称不能包含特殊字符")]
        public string Name { get; private set; }
        [Required(ErrorMessage = "培训点地址必须填写")]
        [StringAndCharacter(ErrorMessage = "地址不能包含特殊字符")]
        public string Address { get; private set; }
        [Required(ErrorMessage = "培训点账号必须填写")]
        [String(ErrorMessage = "账号不能包含特殊字符")]
        public string LogName { get; private set; }
        [Required(ErrorMessage = "登陆密码必须填写")]
        public string LogPassWord { get; private set; }

        public TrainingCenterCreateOrUpdateCommand(int id,string logname, string logpassword, string name, string address)
        {
            Id = id;         
            LogName = logname;
            LogPassWord = logpassword;
            Name = name;
            Address = address;
        }

    }
}
