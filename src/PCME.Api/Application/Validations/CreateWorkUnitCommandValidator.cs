using FluentValidation;
using PCME.Api.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Validations
{
    public class CreateWorkUnitCommandValidator: AbstractValidator<CreateOrUpdateWorkUnitCommand>
    {
        public CreateWorkUnitCommandValidator()
        {
            RuleFor(command => command.Name).NotEmpty().Length(1, 50);
            RuleFor(command => command.PassWord).NotEmpty().Length(6, 50).WithMessage("密码必须大于6位");
            RuleFor(command => command.Email).NotEmpty();
            RuleFor(command => command.Address).NotEmpty();
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Level).NotEmpty();
            RuleFor(command => command.WorkUnitNatureId).NotEmpty();
        }

    }
}
