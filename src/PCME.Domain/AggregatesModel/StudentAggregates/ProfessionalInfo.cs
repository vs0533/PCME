using System;
using PCME.Domain.AggregatesModel.ProfessionalTitleAggregates;
using PCME.Domain.SeedWork;

namespace PCME.Domain.AggregatesModel.StudentAggregates
{
	public class ProfessionalInfo:Entity
    {
		public int SeriesId { get; private set; }
		public ProfessionalTitle ProfessionalTitle { get; private set; }
        public int ProfessionalTitleId { get; private set; }
		public DateTime GetDate { get; private set; }
		public string Education { get; private set; }

		public int PromoteTypeId { get; private set; }
        public PromoteType PromoteType { get; private set; }
        
        public Student Student { get; private set; }
		public int? StudentId { get; private set; }

		public ProfessionalInfo(int seriesId, int professionalTitleId, DateTime getDate, string education, int promoteTypeId,int? studentId)
		{
			SeriesId = seriesId;
			ProfessionalTitleId = professionalTitleId;
			GetDate = getDate;
			Education = education;
			PromoteTypeId = promoteTypeId;
			StudentId = studentId;
		}

		public void Update(int seriesId, int professionalTitleId, DateTime getDate, string education, int promoteTypeId)
		{
			SeriesId = seriesId;
            ProfessionalTitleId = professionalTitleId;
            GetDate = getDate;
            Education = education;
            PromoteTypeId = promoteTypeId;
		}
	}
}
