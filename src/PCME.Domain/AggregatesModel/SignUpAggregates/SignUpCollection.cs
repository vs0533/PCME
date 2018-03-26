using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.SignUpAggregates
{
    public class SignUpCollection:Entity
    {
        public string Num { get; private set; }
        public bool IsUnit { get; private set; }
        public int UnitId { get; private set; }
        public decimal Price { get; private set; }
        public bool IsClock { get; private set; }
        public int TrainingCenterId { get; private set; }

        public SignUpCollection(string num, bool isUnit, int unitId, decimal price, bool isClock, int trainingCenterId)
        {
            Num = num ?? throw new ArgumentNullException(nameof(num));
            IsUnit = isUnit;
            UnitId = unitId;
            Price = price;
            IsClock = isClock;
            TrainingCenterId = trainingCenterId;
        }
    }
}
