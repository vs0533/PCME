using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.Exceptions
{
    public class ExamSubjectDomainException:Exception
    {
        public ExamSubjectDomainException()
        {

        }
        public ExamSubjectDomainException(string message):base(message)
        {

        }
        public ExamSubjectDomainException(string message,Exception exception):base(message,exception)
        {

        }
    }
}
