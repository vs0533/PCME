using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.Exceptions
{
    public class ExaminationRoomPlanDomainException:Exception
    {
        public ExaminationRoomPlanDomainException()
        {

        }
        public ExaminationRoomPlanDomainException(string message):base(message)
        {

        }
        public ExaminationRoomPlanDomainException(string message,, Exception exception) :base(message,exception)
        {

        }
    }
}
