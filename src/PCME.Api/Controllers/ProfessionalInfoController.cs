using Microsoft.AspNetCore.Mvc;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using PCME.Domain.AggregatesModel.StudentAggregates;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/ProfessionalInfo")]

    public class ProfessionalInfoController:Controller
    {
        private readonly ApplicationDbContext context;
        public ProfessionalInfoController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        [Route("gettitle")]
        public async Task<IActionResult> GetTitle()
        {
            var query = await context.ProfessionalTitles
                .Include(s => s.Series)
                .Include(s => s.Specialty)
                .Include(s => s.Level).ToListAsync();
            var result = query.Select(c => new Dictionary<string, object>
            {
                {"Series.Id",c.Series.Id},
                {"Series.Name",c.Series.Name},
                {"Specialty.Id",c.Specialty.Id},
                {"Specialty.Name",c.Specialty.Name},
                {"Name",c.Name},
                {"Level.Id",c.Level.Id},
                {"Level.Name",c.Level.Name},
                {"Id",c.Id}

            });

            return Ok(new { data = result,total = query.Count()});
        }
        [HttpPost]
        [Route("saveorupdate")]
        [Authorize(Roles ="Unit")]
        public async Task<IActionResult> Save(int studentid,int titleid,int promotetype,string education,DateTime getdate) {
            var pinfo_ = await context.ProfessionalInfos.Where(c => c.StudentId == studentid).FirstOrDefaultAsync();
            if (pinfo_ == null)
            {
                pinfo_ = new ProfessionalInfo(titleid, getdate, education, promotetype, studentid);
                context.ProfessionalInfos.Add(pinfo_);
                await context.SaveChangesAsync();
            }
            else
            {
                pinfo_.Update(titleid, getdate, education, promotetype);
                await context.SaveChangesAsync();
            }
            return Ok(new {success=true,message="保存成功" });
        }
        [HttpGet]
        [Route("getpinfo")]
        public async Task<IActionResult> read(int studentid)
        {
            var pinfo = await context.ProfessionalInfos.Where(c => c.StudentId == studentid)
                .Include(s=>s.ProfessionalTitle)
                .Include(s=>s.PromoteType)
                .FirstOrDefaultAsync();

            //var type = PromoteType.From(pinfo.PromoteTypeId).GetType();
            return Ok(new Dictionary<string,object> {
                { "title",pinfo.ProfessionalTitle.Name },
                { "titleid",pinfo.ProfessionalTitleId },
                {"promotetype",pinfo.PromoteTypeId},
                {"getdate",pinfo.GetDate.ToString("yyyy-MM-dd")},
                { "studentid",studentid},
                { "education",pinfo.Education}
            });
        }
    }
}
