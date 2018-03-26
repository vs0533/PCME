using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ExamSubjectAggregates
{
    public class ExamSubject:Entity,IAggregateRoot
    {
        public string Name { get; private set; }
        public StudentAggregates.StudentType OpenType { get; private set; }
    }
}