using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCME.Domain.AggregatesModel.CertificateAggregates
{
    public class CertificateCategory : Enumeration
    {
        public static CertificateCategory Cert1 = new CertificateCategory(1, "继续教育合格证书");
        public static CertificateCategory Cert2 = new CertificateCategory(2, "参加职称评审合格证书");
        public static CertificateCategory Cert3 = new CertificateCategory(3, "参加职称聘任合格证书");

        public static IEnumerable<CertificateCategory> List() => new[] { Cert1, Cert2,Cert3 };
        public CertificateCategory()
        {

        }
        public CertificateCategory(int id, string name) :
            base(id, name)
        {

        }

        public static CertificateCategory FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCulture));

            if (state == null)
            {
                throw new Exception($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static CertificateCategory From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for type: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
