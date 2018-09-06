using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PCME.Api.Application.ParameBinder;
using PCME.Api.Extensions;
using PCME.Domain.AggregatesModel.CertificateAggregates;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/PrintedData")]
    public class PrintedDataController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public PrintedDataController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Save([FromBody]JObject jObject, int type)
        {
            int studentid = (int)jObject["studentid"];
            int cf = (int)jObject["cf"];
            if (cf != 0)
            {
                return Ok(new { success = false, message = "当前学分不合格，生成失败。" });
            }
            var isexists = dbContext.PrintedData.Where(c => c.StudentId == studentid && c.CertificateCategoryId == type);
            bool insert = true;
            int count = isexists.Count();
            PrintedData last = isexists.OrderByDescending(c => c.Id).FirstOrDefault();
            if (!isexists.Any())
            {
                insert = true;
            }
            else
            {
                var time = isexists.OrderByDescending(c => c.Id).FirstOrDefault().CreateTime;
                if (DateTime.Now < time.AddDays(180) && DateTime.Now > time.AddDays(-180))
                {
                    insert = false;
                }
            }
            if (insert)
            {
                PrintedData printedData = new PrintedData(studentid, CreateNum(count, studentid,type), jObject.ToString(), type, DateTime.Now);
                dbContext.PrintedData.Add(printedData);
                dbContext.SaveChanges();
                return Ok(new { success=true,message="成功生成"});
            }
            return Ok(new { success=false,message="上一次证书生成时间小于系统设置的180天，生成失败。"});
        }
        private string CreateNum(int count, int studentid,int type)
        {
            var ctr = count + 1;
            var student = dbContext.Students.Find(studentid);
            if (student == null)
            {
                throw new Exception("生成合格证ID时出错 原因：人员信息不存在");
            }
            return string.Format("{0}{1}{2}", type.ToString(),student.IDCard, ctr.ToString().PadLeft(5, '0'));
        }
        [HttpPost]
        [Route("read")]
        [Authorize(Roles = "Student")]
        public IActionResult StoreRead(int start, int limit, string filter, string query, string navigates)
        {
            int studentid = int.Parse(User.FindFirstValue("AccountId"));
            var search = from printedData in dbContext.PrintedData
                         join certificatecategory in dbContext.CertificateCategory on printedData.CertificateCategoryId equals certificatecategory.Id
                         where printedData.StudentId == studentid
                         orderby printedData.Id descending
                         select new { printedData, certificatecategory };
            search = search
                .FilterAnd(filter.ToObject<Filter>())
                .FilterOr(query.ToObject<Filter>());
            var item = search.Skip(start).Take(limit);
            var result = item.Select(c => new Dictionary<string, object>
            {
                {"id",c.printedData.Id},
                {"printedData.Num",c.printedData.Num},
                {"printedData.Data",c.printedData.Data},
                {"printedData.CreateTime",c.printedData.CreateTime},
                {"printedData.StudentId",c.printedData.StudentId},
                {"certificatecategory.Id",c.certificatecategory.Id},
                {"certificatecategory.Name",c.certificatecategory.Name}
            });
            var total = search.Count();
            return Ok(new { total, data = result });
        }
    }
}
