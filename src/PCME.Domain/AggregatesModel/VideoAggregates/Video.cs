using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.CoursewareAggregates
{
    public class Video:Entity
    {
        public string Name { get; private set; }
        public int ExamSubjectId { get; private set; }
        public string VideoPath { get; private set; }

        public Video(string name, int examSubjectId, string videoPath)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ExamSubjectId = examSubjectId;
            VideoPath = videoPath ?? throw new ArgumentNullException(nameof(videoPath));
        }
    }
}
