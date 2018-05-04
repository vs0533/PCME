﻿using PCME.Domain.AggregatesModel.UnitAggregates;
using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PCME.Domain.AggregatesModel.WorkUnitAccountAggregates
{
    public class WorkUnitAccount:Entity,IAggregateRoot
    {
        public string AccountName { get; private set; }
        public string HolderName { get; private set; }
        public WorkUnitAccountType WorkUnitAccountType { get; private set; }
        public int WorkUnitAccountTypeId { get; private set; }
        public string PassWord { get; private set; }
        [ForeignKey("WorkUnitId")]
        public WorkUnit WorkUnit { get; private set; }
        public int WorkUnitId { get; private set; }
        public WorkUnitAccount()
        {

        }

        public WorkUnitAccount(string accountName, int workAccountTypeId,string passWord,string holderName)
        {
            AccountName = accountName ?? throw new ArgumentNullException(nameof(accountName));
            WorkUnitAccountTypeId = workAccountTypeId;
            PassWord = passWord ?? throw new ArgumentNullException(nameof(passWord));
            HolderName = holderName ?? throw new ArgumentException(nameof(holderName));
        }
    }
}
