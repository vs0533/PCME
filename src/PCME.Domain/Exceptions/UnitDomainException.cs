using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.Exceptions
{
    public class UnitDomainException:Exception
    {
        public UnitDomainException()
        {

        }
        public UnitDomainException(string message):base(message)
        {

        }
        public UnitDomainException(string message,Exception innerException)
            :base(message,innerException)
        {

        }
    }
}
