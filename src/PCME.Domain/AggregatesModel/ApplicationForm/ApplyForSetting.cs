using System;
using PCME.Domain.SeedWork;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using PCME.Domain.AggregatesModel.ExamSubjectAggregates;

namespace PCME.Domain.AggregatesModel.ApplicationForm
{
    public class ApplyForSetting : Entity,IAggregateRoot
    {
        public string ExamSubjectIdString
        {
            get;
            private set;
        }
        public ExamSubject ExamSubject
        {
            get;
            private set;
        }
        public DateTime StartTime
        {
            get;
            private set;
        }
        public DateTime EndTime
        {
            get;
            private set;
        }
        [NotMapped]
        public int[] SubjectIds
        {
            get {
                return ExamSubjectIdString.Split(',').Select(c => int.Parse(c)).ToArray();
            }
        }
        public string Title
        {
            get;
            private set;
        }

        public ApplyForSetting(string examSubjectIdString, DateTime startTime, DateTime endTime, string title)
        {
            ExamSubjectIdString = examSubjectIdString;
            StartTime = startTime;
            EndTime = endTime;
            Title = title;
        }
    }
}
