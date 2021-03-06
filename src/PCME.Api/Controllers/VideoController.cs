﻿using Microsoft.AspNetCore.Authorization;
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
        private readonly TestDBContext testContext;
        public VideoController(VideoDbContext videoDbContext, ApplicationDbContext context, TestDBContext testContext)
        {
            videoContext = videoDbContext;
            this.context = context;
            this.testContext = testContext;
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
                                  select examsubject;
            var itemsid = examsubjetItems.ToList();
            

            var videoItems = from video in videoContext.Video
                             let videocollection = from videocollection in videoContext.VideoCollection where videocollection.VideoId  == video.Id select videocollection
                                                   //let examsubjectcode = from examsubject in itemsid where examsubject.Id == video.ExamSubjectId select examsubject.Code
                             join examsubject in itemsid on video.ExamSubjectId equals examsubject.Id
                             where itemsid.Select(c=>c.Id == video.ExamSubjectId).Any()//.Contains(video.ExamSubjectId)
                             select new { video,videocollection, examsubject };

            var testConfig = testContext.TestConfig.ToList();
            var homeworkinfo = testContext.HomeWorkResult.Where(c => c.StudentId == studentid).ToList();

            var result = videoItems.Select(c => new Dictionary<string, object>
            {
                {"id",c.video.Id},
                {"name",c.video.Name},
                {"height",c.video.Height},
                {"width",c.video.Width},
                {"image",c.video.Image},
                {"collection",c.videocollection},
                {"examsubjectid",c.video.ExamSubjectId},
                {"ctrconfig",testConfig.FirstOrDefault(d=>d.CategoryCode == c.examsubject.Code) == null ? 0 : testConfig.FirstOrDefault(d=>d.CategoryCode == c.examsubject.Code).Ctr},
                {"ctr",homeworkinfo.Where(d=>d.CategoryCode == c.examsubject.Code).Count()},
                {"sumscore",homeworkinfo.Where(d=>d.CategoryCode == c.examsubject.Code).Sum(d=>d.Score)}
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
