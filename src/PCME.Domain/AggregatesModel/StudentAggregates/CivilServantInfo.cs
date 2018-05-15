using System;
using PCME.Domain.SeedWork;

namespace PCME.Domain.AggregatesModel.StudentAggregates
{
	public class CivilServantInfo:Entity
    {
		public string Duty { get; private set; }
        public DutyLevel DutyLevel { get; private set; }
		public int DutyLevelId { get; private set; }
		public DateTime TakeDate { get; private set; }
		public string EducationFirst { get; private set; }

        public string EducationHeight { get; private set; }
        /// <summary>
        /// 是否参加职称晋升 true是
        /// </summary>
        /// <value><c>true</c> if join promote; otherwise, <c>false</c>.</value>
		public bool JoinPromote { get; private set; }

		public int StudentId { get; private set; }
		public Student Student { get; private set; }

		public CivilServantInfo(string duty, int dutyLevelId, DateTime takeDate, string educationFirst, string educationHeight, bool joinPromote, int studentId)
		{
			Duty = duty;
			DutyLevelId = dutyLevelId;
			TakeDate = takeDate;
			EducationFirst = educationFirst;
			EducationHeight = educationHeight;
			JoinPromote = joinPromote;
			StudentId = studentId;
		}

		public void Update(string duty, int dutyLevelId, DateTime takeDate, string educationFirst, string educationHeight, bool joinPromote)
        {
            Duty = duty;
            DutyLevelId = dutyLevelId;
            TakeDate = takeDate;
            EducationFirst = educationFirst;
            EducationHeight = educationHeight;
            JoinPromote = joinPromote;
        }
	}
}
