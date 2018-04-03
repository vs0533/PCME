using Microsoft.EntityFrameworkCore;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.StudentAggregates.StudentAggregates
{
    public interface IStudentRepository : IRepository<Student>
    {

    }
}