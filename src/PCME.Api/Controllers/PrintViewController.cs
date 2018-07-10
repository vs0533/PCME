using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PCME.Domain.AggregatesModel.SignUpAggregates;
using PCME.Domain.SeedWork;
using PCME.Infrastructure;
using QRCoder;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/PrintView")]
    public class PrintViewController : Controller
    {
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IRepository<SignUpForUnit> signUpForUnitRepository;
        private readonly IRepository<SignUpCollection> signUpCollectionRepository;
        private readonly IRepository<SignUp> signUpRepository;
        private readonly ApplicationDbContext context;
        public PrintViewController(IUnitOfWork<ApplicationDbContext> unitOfWork, ApplicationDbContext context)
        {
            this.unitOfWork = unitOfWork;
            signUpForUnitRepository = unitOfWork.GetRepository<SignUpForUnit>();
            signUpCollectionRepository = unitOfWork.GetRepository<SignUpCollection>();
            signUpRepository = unitOfWork.GetRepository<SignUp>();
            this.context = context;
        }
        /// <summary>
        /// 单位显示报名表
        /// </summary>
        /// <param name="signUpForUnitId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("signup")]
        [Authorize(Roles = "Unit")]
        public async Task<IActionResult> SignUpForUnitAndCollectionView(int signUpForUnitId)
        {
            var search = signUpForUnitRepository.Query(c => c.Id == signUpForUnitId, include: s =>
              s.Include(c => c.WorkUnit)
              .Include(c => c.TrainingCenter)
            );
            var items = await search.FirstOrDefaultAsync();

            ///取得科目开设信息
            var examsubjectOpenInfo = context.ExamSubjectOpenInfo.FirstOrDefault(c => c.TrainingCenterId == items.TrainingCenterId);


            var search_child = signUpCollectionRepository.Query(c => c.SignUpForUnitId == items.Id, include: s => s
                  .Include(c => c.ExamSubject)
                  .Include(c => c.Student)
            ).OrderBy(c=>c.ExamSubjectId);
            var items_child = await search_child.ToListAsync();

            var result = new Dictionary<string, object>
            {
                { "id",items.Id},
                { "code",items.Code},
                { "workunitname",items.WorkUnit.Name},
                { "trainingcentername",items.TrainingCenter.Name},
                { "islock",items.IsLock},
                { "ispay",items.IsPay},
                { "gotovaldatetime",examsubjectOpenInfo.GoToValDateTime},
                { "signupcollectioncount",items_child.Count()},
                { "signupcollection",items_child.Select(c=>new {
                    id = c.Id,
                    idcard = c.Student.IDCard,
                    studentname = c.Student.Name,
                    examsubjectname = c.ExamSubject.Name,
                    signupforunitid = c.SignUpForUnitId
                })}
            };
            try
            {
                return Ok(new { total = items_child.Count(), data = result });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Route("qrcode")]
        public IActionResult QRCode(string code)
        {
            Bitmap bmp = null;
            Graphics g = null;
            MemoryStream ms = new MemoryStream();

            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                bmp = qrCode.GetGraphic(20);
                g = Graphics.FromImage(bmp);
                bmp.Save(ms, ImageFormat.Jpeg);
                return File(ms.ToArray(), "image/jpeg");
            }
            catch (Exception)
            {
                return BadRequest();
            }
            finally
            {
                g.Dispose();
                bmp.Dispose();
            }
        }

        /// <summary>
        /// 培训点扫描报名
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("readsignupdetailsbyqr")]
        [Authorize(Roles = "TrainingCenter")]
        public async Task<IActionResult> ReadSignUpForUnitAndSignUpCollectionByQR(string code)
        {
            var loginTrainingCenterId = int.Parse(User.FindFirstValue("WorkUnitId"));

            var signUpForUnit = await signUpForUnitRepository.Query(c => c.Code == code && c.TrainingCenterId == loginTrainingCenterId, include: s =>
              s.Include(c => c.WorkUnit)
              .Include(c => c.TrainingCenter)
            ).FirstOrDefaultAsync();

            var signUpCollection = await signUpCollectionRepository.Query(c => c.SignUpForUnitId == signUpForUnit.Id, include: s => s
                  .Include(c => c.ExamSubject)
                  .Include(c => c.Student)
            ).ToListAsync();

            var result = new Dictionary<string, object>
            {
                { "id",signUpForUnit.Id},
                { "code",signUpForUnit.Code},
                { "workunitname",signUpForUnit.WorkUnit.Name},
                { "trainingcentername",signUpForUnit.TrainingCenter.Name},
                { "islock",signUpForUnit.IsLock},
                { "ispay",signUpForUnit.IsPay},
                { "signupcollectioncount",signUpCollection.Count()},
                { "signupcollection",signUpCollection.Select(c=>new {
                    id = c.Id,
                    idcard = c.Student.IDCard,
                    studentname = c.Student.Name,
                    examsubjectname = c.ExamSubject.Name,
                    signupforunitid = c.SignUpForUnitId
                })}
            };
            return Ok(new { total = 1, data = result });
        }

        /// <summary>
        /// 培训点解锁
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("saveorupdatesignupforunit")]
        [Authorize(Roles = "TrainingCenter")]
        public IActionResult SaveOrUpdate([FromBody]JToken data)
        {
            //int s = int.TryParse(data.islock);
            int key = (int)data["id"];
            bool islock = (bool)data["islock"];
            var signUpForUnit = signUpForUnitRepository.Find(key);
            signUpForUnit.ChangeIsLock(islock);
            signUpForUnitRepository.Update(signUpForUnit);
            unitOfWork.SaveChanges();
            return Ok(new { message = "解锁定成功", success = true });
        }
        /// <summary>
        /// 正式报名
        /// </summary>
        /// <param name="signupforunitid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("signupofficial")]
        [Authorize(Roles = "TrainingCenter")]
        public async Task<IActionResult> SignUp(int signupforunitid)
        {
            var loginTrainingCenterId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var signUpForUnit = await signUpForUnitRepository.Query(c => c.Id == signupforunitid)
                .FirstOrDefaultAsync();


            ///取得科目开设信息
            var examsubjectOpenInfo = context.ExamSubjectOpenInfo.FirstOrDefault(c => c.TrainingCenterId == loginTrainingCenterId);
            if (examsubjectOpenInfo == null)
            {
                return Ok("未取得任何开设科目信息，报名失败");
            }
            if (DateTime.Now > examsubjectOpenInfo.SignUpFinishTime.AddDays(examsubjectOpenInfo.SignUpFinishOffset))
            {
                return Ok("当前时间大于报名截止时间了，报名失败");
            }

            if (signUpForUnit == null)
            {
                return Ok("未找到报名表");
            }
            if (signUpForUnit.IsPay)
            {
                return Ok("报名表已经扫描过了");
            }
            if (signUpForUnit.TrainingCenterId != loginTrainingCenterId)
            {
                return Ok("该报名表不属于该培训点，不能进行表报名");
            }

            if (!signUpForUnit.IsLock)
            {
                return Ok("该报名表未锁定，不能进行报名");
            }

            var signUpCollection = await signUpCollectionRepository.Query(c => c.SignUpForUnitId == signUpForUnit.Id, include: s => s
              .Include(c => c.ExamSubject)
              .Include(c => c.Student)
            ).ToListAsync();
            if (!signUpCollection.Any())
            {
                return Ok("该报名表是空的，并不能进行报名");
            }
            Dictionary<string, string> exists = new Dictionary<string, string>();
            List<SignUp> signUps = new List<SignUp>();
            foreach (var item in signUpCollection)
            {
                var isExists = signUpRepository.Query(c => c.StudentId == item.StudentId && c.ExamSubjectId == item.ExamSubjectId).Any();
                if (isExists)//如果报名表详情中人员和科目存在正式报名表中了 则添加到存在列表中用于返回客户端显示
                {
                    exists.Add(item.Student.Name, item.ExamSubject.Name);
                }
                else//否则添加到正式报名表中 并增加人员生成准考证权限
                {
                    SignUp signUp = new SignUp(item.StudentId, item.ExamSubjectId, signUpForUnit.Id, loginTrainingCenterId, false, DateTime.Now);
                    signUps.Add(signUp);
                    item.Student.AddaTicketCtr();
                }
            }
            if (exists.Any())
            {
                var existsliststr = string.Join("<br >", exists.Select(c => string.Format("姓名:{0} - 科目:{1}", c.Key, c.Value)));
                return Ok(string.Format("报名表中包含已经报名成功的人员<br>{0}", existsliststr));
            }
            //if (!signUps.Any())
            //{
            //    return Ok("发生错误，本次表报名失败");
            //}
            signUpForUnit.PayToSuccess();
            signUpForUnitRepository.Update(signUpForUnit);
            await signUpRepository.InsertAsync(signUps);
            signUpCollectionRepository.Update(signUpCollection);
            await unitOfWork.SaveChangesAsync();
            return Ok("报名成功");
        }

    }
}