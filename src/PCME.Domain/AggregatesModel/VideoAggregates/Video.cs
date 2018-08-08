using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.CoursewareAggregates
{
    public class Video:Entity,IAggregateRoot
    {
        public string Name { get; private set; }
        public int ExamSubjectId { get; private set; }
        public string Image { get; private set; }
        public DateTime CreateTime { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Video()
        {

        }

        public Video(string name, int examSubjectId, string image, DateTime createTime, int width=510, int height=498)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ExamSubjectId = examSubjectId;
            Image = image ?? throw new ArgumentNullException(nameof(image));
            CreateTime = createTime;
            Width = width;
            Height = height;
        }
    }
}
