using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCME.Infrastructure;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Video")]
    public class VideoController:Controller
    {
        private readonly VideoDbContext videoContext;
        private readonly ApplicationDbContext context;
        public VideoController(VideoDbContext videoDbContext, ApplicationDbContext context)
        {
            videoContext = videoDbContext;
            this.context = context;
        }
        [HttpPost]
        [Route("getvideo")]
        //[Authorize(Roles = "Student")]
        public IActionResult getVideo()
        {
            var studentid = int.Parse(User.FindFirstValue("AccountId"));
            var examsubjetItems = from signup in context.SignUp
                                  join examsubject in context.ExamSubjects on signup.ExamSubjectId equals examsubject.Id
                                  where signup.StudentId == studentid
                                  select examsubject.Id;
            var itemsid = examsubjetItems.ToList();
            

            var videoItems = from video in videoContext.Video
                             let videocollection = from videocollection in videoContext.VideoCollection where videocollection.VideoId  == video.Id select videocollection
                             where itemsid.Contains(video.ExamSubjectId)
                             select new { video,videocollection };
            var result = videoItems.Select(c => new Dictionary<string, object>
            {
                {"id",c.video.Id},
                {"name",c.video.Name},
                {"image",c.video.Image},
                {"collection",c.videocollection},
                {"examsubjectid",c.video.ExamSubjectId}
            });

            return Ok(new { data = result });
        }
        [HttpPost]
        [Route("playvideo")]
        public IActionResult PlayVideo(int id) {
            var video = from videocollection in videoContext.VideoCollection
                        where videocollection.Id == id
                        select videocollection;
            return Ok(video.FirstOrDefault());
        }
    }
}
