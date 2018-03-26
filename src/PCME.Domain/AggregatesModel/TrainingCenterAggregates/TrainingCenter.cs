using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.TrainingCenterAggregates
{
    public class TrainingCenter:Entity,IAggregateRoot
    {
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string LogName { get; private set; }
        public string LogPassWord { get; private set; }

        public TrainingCenter(string logname,string logpassword,string name,string address)
        {
            Name = logname;
            LogPassWord = logpassword;
            Name = name;
            Address = address;
        }
    }
}
