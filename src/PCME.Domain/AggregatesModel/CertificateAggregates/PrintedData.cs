using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.CertificateAggregates
{
    public class PrintedData:Entity,IAggregateRoot
    {
        public int StudentId { get; private set; }
        public string Data { get; private set; }
        public CertificateCategory Category { get; private set; }
        public DateTime CreateTime { get; private set; }
        public PrintedData()
        {

        }

        public PrintedData(int studentId, string data, CertificateCategory category, DateTime createTime)
        {
            StudentId = studentId;
            Data = data ?? throw new ArgumentNullException(nameof(data));
            Category = category ?? throw new ArgumentNullException(nameof(category));
            CreateTime = createTime;
        }
    }
}
