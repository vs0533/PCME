using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.Exceptions
{
    public class StudentDomainException : Exception
    {
        public StudentDomainException()
        { }

        public StudentDomainException(string message)
            : base(message)
        { }

        public StudentDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
