using AutoMapper;
using PCME.Api.Application.Commands;
using PCME.Domain.AggregatesModel.UnitAggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCME.Api.Infrastructure.AutoMapperMapping
{
    public static class MappingConfiguration
    {
        public static void Configure(IMapperConfigurationExpression cfg)

        {

            cfg.CreateMap<CreateWorkUnitCommand, WorkUnit>();

        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateWorkUnitCommand, WorkUnit>();
        }
    }
}
