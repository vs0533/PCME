using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.Exceptions
{
    public class ExaminationDomainException:Exception
    {
        public ExaminationDomainException()
        {

        }
        public ExaminationDomainException(string message):base(message)
        {

        }
        public ExaminationDomainException(string message,Exception exception):base(message,exception)
        {

        }
    }
}
