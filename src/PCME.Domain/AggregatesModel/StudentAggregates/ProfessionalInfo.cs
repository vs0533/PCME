using System;
using PCME.Domain.AggregatesModel.ProfessionalTitleAggregates;
using PCME.Domain.SeedWork;

namespace PCME.Domain.AggregatesModel.StudentAggregates
{
	public class ProfessionalInfo:Entity
    {
		public ProfessionalTitle ProfessionalTitle { get; private set; }
        public int ProfessionalTitleId { get; private set; }
		public DateTime GetDate { get; private set; }
        public DateTime CalculateDate { get; private set; }
        public string Education { get; private set; }

		public int PromoteTypeId { get; private set; }
        public PromoteType PromoteType { get; private set; }
        
        public Student Student { get; private set; }
		public int? StudentId { get; private set; }

		public ProfessionalInfo(int professionalTitleId, DateTime getDate, string education, int promoteTypeId,int? studentId)
		{
			ProfessionalTitleId = professionalTitleId;
			GetDate = getDate;
			Education = education;
			PromoteTypeId = promoteTypeId;
			StudentId = studentId;

            SetCalculateDate();
		}

		public void Update(int professionalTitleId, DateTime getDate, string education, int promoteTypeId)
		{
            ProfessionalTitleId = professionalTitleId;
            GetDate = getDate;
            Education = education;
            PromoteTypeId = promoteTypeId;

            SetCalculateDate();

        }

        public void SetCalculateDate(DateTime calculateDate) {
            CalculateDate = calculateDate;
        }

        private void SetCalculateDate()
        {
            CalculateDate = GetDate;
        }

    }
}
