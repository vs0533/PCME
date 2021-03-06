﻿using PCME.Domain.AggregatesModel.StudentAggregates;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.ProfessionalTitleAggregates
{
    public class ProfessionalTitle:Entity,IAggregateRoot
    {
        public string Name { get; private set; }

        public Specialty Specialty { get; private set; }
        public Series Series { get; private set; }
        public Level Level { get; private set; }

        public PromoteType PromoteType { get; private set; }
        public ProfessionalTitle()
        {

        }
        public ProfessionalTitle(string name, Specialty specialty, Series series, Level level,PromoteType promoteType)
        {
            Name = name;
            Specialty = specialty ?? throw new ArgumentNullException(nameof(specialty));
            Series = series ?? throw new ArgumentNullException(nameof(series));
            Level = level ?? throw new ArgumentNullException(nameof(level));
            PromoteType = promoteType ?? throw new ArgumentNullException(nameof(PromoteType));
        }
    }
}
