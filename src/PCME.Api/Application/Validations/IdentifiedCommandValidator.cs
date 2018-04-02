using FluentValidation;
using PCME.Api.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Application.Validations
{
    public class IdentifiedCommandValidator:AbstractValidator<IdentifiedCommand<CreateWorkUnitCommand,bool>>
    {
        public IdentifiedCommandValidator()
    {
            RuleFor(command => command.Id).NotEmpty();
    }
}
}
