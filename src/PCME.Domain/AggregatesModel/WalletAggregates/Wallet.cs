using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.WalletAggregates
{
    public class Wallet:Entity,IAggregateRoot
    {
        public decimal Balance { get;private set; }
        public string Descript { get;private set; }
        public DateTime CreateTime { get;private set; }

        public int StudentId { get; private set; }

        public Wallet(decimal balance,string descript,int studentid)
        {
            Balance = balance;
            Descript = descript;
            CreateTime = DateTime.Now;
            StudentId = studentid;
        }
    }
}
