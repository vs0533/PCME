using PCME.Domain.AggregatesModel.ExamSubjectAggregates;
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

        public int OpenTypeId { get; private set; }
		public OpenType OpenType { get; private set; }


		public TrainingCenter()
        {

        }

        public TrainingCenter(string logname,string logpassword,string name,string address,int openTypeId)
        {
            Name = name;
            LogPassWord = logpassword;
            LogName = logname;
            Address = address;
			OpenTypeId = openTypeId;
        }

		public void Update(string logname, string logpassword, string name, string address,int openTypeId)
        {
            Name = name;
            LogPassWord = logpassword;
            LogName = logname;
            Address = address;
			OpenTypeId = openTypeId;
        }
    }
}
