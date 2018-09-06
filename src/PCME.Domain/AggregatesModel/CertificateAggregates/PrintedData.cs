using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.CertificateAggregates
{
    public class PrintedData : Entity, IAggregateRoot
    {
        public int StudentId { get; private set; }
        public string Num { get; private set; }
        public string Data { get; private set; }
        public int CertificateCategoryId{get;private set;}
        public DateTime CreateTime { get; private set; }
        public PrintedData()
        {

        }

        public PrintedData(int studentId,string num, string data, int certificateCategoryId, DateTime createTime)
        {
            StudentId = studentId;
            Num = num ?? throw new ArgumentNullException(nameof(num));
            Data = data ?? throw new ArgumentNullException(nameof(data));
            CertificateCategoryId = certificateCategoryId;
            CreateTime = createTime;
        }
    }
}
