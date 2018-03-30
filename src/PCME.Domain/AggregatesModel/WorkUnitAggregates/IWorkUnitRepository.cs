using Microsoft.EntityFrameworkCore;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.UnitAggregates
{
    public interface IWorkUnitRepository:IRepository<WorkUnit>
    {
    }
}
