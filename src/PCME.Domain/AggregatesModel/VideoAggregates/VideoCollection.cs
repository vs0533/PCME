using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.VideoAggregates
{
    public class VideoCollection:Entity,IAggregateRoot
    {
        public int VideoId { get; private set; }
        public string LocalPath { get; private set; }
        public string HtmlTag { get; private set; }
        public int WatchTimer { get; private set; }
        public DateTime CreateTime { get; private set; }
        public string Title { get; private set; }

        public VideoCollection()
        {

        }

        public VideoCollection(int videoId, string localPath, string htmlTag, int watchTimer, DateTime createTime, string title)
        {
            VideoId = videoId;
            LocalPath = localPath ?? throw new ArgumentNullException(nameof(localPath));
            HtmlTag = htmlTag ?? throw new ArgumentNullException(nameof(htmlTag));
            WatchTimer = watchTimer;
            CreateTime = createTime;
            Title = title ?? throw new ArgumentNullException(nameof(title));
        }
    }
}
