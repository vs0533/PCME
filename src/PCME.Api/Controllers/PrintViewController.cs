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
using PCME.Domain.AggregatesModel.ExaminationRoomPlantTicketAggregates;
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
        private readonly IRepository<ExamRoomPlanTicket> examRoomPlanTicketRepository;
        private readonly ApplicationDbContext context;
        public PrintViewController(IUnitOfWork<ApplicationDbContext> unitOfWork, ApplicationDbContext context)
        {
            this.unitOfWork = unitOfWork;
            signUpForUnitRepository = unitOfWork.GetRepository<SignUpForUnit>();
            signUpCollectionRepository = unitOfWork.GetRepository<SignUpCollection>();
            signUpRepository = unitOfWork.GetRepository<SignUp>();
            examRoomPlanTicketRepository = unitOfWork.GetRepository<ExamRoomPlanTicket>();
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
            //var items_child = await search_child.ToListAsync();

            var result = new Dictionary<string, object>
            {
                { "id",items.Id},
                { "code",items.Code},
                { "workunitname",items.WorkUnit.Name},
                { "trainingcentername",items.TrainingCenter.Name},
                { "islock",items.IsLock},
                { "ispay",items.IsPay},
                { "gotovaldatetime",examsubjectOpenInfo.GoToValDateTime},
                { "signupcollectioncount",search_child.Count()},
                { "signupcollection",search_child.Select(c=>new {
                    id = c.Id,
                    idcard = c.Student.IDCard,
                    studentname = c.Student.Name,
                    examsubjectname = c.ExamSubject.Name,
                    signupforunitid = c.SignUpForUnitId
                })},
                {"address",items.TrainingCenter.Address},
                {"tel",items.TrainingCenter.Tel}
            };
            try
            {
                return Ok(new { total = search_child.Count(), data = result });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("book")]
        [Authorize(Roles ="Unit,TrainingCenter")]
        public async Task<IActionResult> Book(int signUpForUnitId)
        {
            var signUpForUnit = await signUpForUnitRepository.Query(c => c.Id == signUpForUnitId, include: s =>
              s.Include(c => c.WorkUnit)
              .Include(c => c.TrainingCenter)
            ).FirstOrDefaultAsync();

            var signUpCollection = context.SignUpCollections
                .Include(s => s.ExamSubject)
                .Include(s => s.Student)
                .OrderBy(o => o.ExamSubjectId)
                .Where(c => c.SignUpForUnitId == signUpForUnit.Id);

            var result_linq = from signupcollection in signUpCollection
                               join book in context.Books on signupcollection.ExamSubjectId equals book.ExamSubjectId into left1
                               from book in left1.DefaultIfEmpty()
                               group book by book into g
                               select new { g.Key.Name,count = g.Count(),sum = g.Sum(c=>c.Pirce) };

            var result = new Dictionary<string, object>
            {
                { "id",signUpForUnit.Id},
                { "code",signUpForUnit.Code},
                { "workunitname",signUpForUnit.WorkUnit.Name},
                { "trainingcentername",signUpForUnit.TrainingCenter.Name},
                { "books",result_linq.Select(c=>new {
                    name = c.Name,
                    c.count,
                    c.sum
                })},
                {"tel",signUpForUnit.TrainingCenter.Tel},
                {"address",signUpForUnit.TrainingCenter.Address }
            };
            return Ok(new { total = result.Count(), data = result });
        }

        [HttpPost]
        [Route("bookStudent")]
        [Authorize(Roles = "Student,TrainingCenter")]
        public async Task<IActionResult> BookStudent(int signUpforstudentid)
        {
            var signupforstudent = await (from signupstudent in context.SignUpStudent
                                   join trainingcenter in context.TrainingCenter on signupstudent.TrainingCenterId equals trainingcenter.Id
                                   join student in context.Students on signupstudent.StudentId equals student.Id
                                   where signupstudent.Id == signUpforstudentid
                                   select new { signupstudent, trainingcenter, student }).FirstOrDefaultAsync();

            var collection = from signupstudentcollection in context.SignUpStudentCollection.Include(s => s.SignUpStudent)
                             join examsubject in context.ExamSubjects on signupstudentcollection.ExamSubjectId equals examsubject.Id
                             join book in context.Books on examsubject.Id equals book.ExamSubjectId into books
                             from book in books.DefaultIfEmpty()
                             where signupstudentcollection.SignUpStudentId == signUpforstudentid
                             select new { signupstudentcollection, examsubject,book };

            

            var result = new Dictionary<string, object>
            {
                { "id",signupforstudent.signupstudent.Id},
                { "code",signupforstudent.signupstudent.Code},
                { "studentname",signupforstudent.student.Name},
                {"studentidcard",signupforstudent.student.IDCard},
                { "trainingcentername",signupforstudent.trainingcenter.Name},
                {"sum",collection.Sum(c=>c.book.Pirce)},
                { "books",collection.Select(c=>new {
                    name = c.book == null ? "" : c.book.Name,
                    Pirce = c.book == null ? 0 : c.book.Pirce
                })},
                {"tel",signupforstudent.trainingcenter.Tel},
                {"address",signupforstudent.trainingcenter.Address}
            };
            return Ok(new { total = result.Count(), data = result });
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

            var signUpCollection = signUpCollectionRepository.Query(c => c.SignUpForUnitId == signUpForUnit.Id, include: s => s
                  .Include(c => c.ExamSubject)
                  .Include(c => c.Student)
            );//.ToListAsync();

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
        /// 培训点扫描报名(个人)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("readsignupdetailsbyqr_student")]
        [Authorize(Roles = "TrainingCenter")]
        public async Task<IActionResult> ReadSignUpForStudentAndSignUpCollectionByQR(string code)
        {
            var loginTrainingCenterId = int.Parse(User.FindFirstValue("WorkUnitId"));

            var search = from signupstudent in context.SignUpStudent
                         join trainingcenter in context.TrainingCenter on signupstudent.TrainingCenterId equals trainingcenter.Id
                         join student in context.Students on signupstudent.StudentId equals student.Id
                         where signupstudent.Code == code && signupstudent.TrainingCenterId == loginTrainingCenterId
                         select new { signupstudent, trainingcenter, student };

            var items = await search.FirstOrDefaultAsync();
            
            var collection = context.SignUpStudentCollection.Where(c => c.SignUpStudentId == items.signupstudent.Id)
                .Join(context.ExamSubjects, l => l.ExamSubjectId, r => r.Id, (l, r) => new { l, r });

            var result = new Dictionary<string, object>
            {
                { "id",items.signupstudent.Id},
                { "code",items.signupstudent.Code},
                { "studentname",items.student.Name},
                { "studentidcard",items.student.IDCard},
                { "trainingcentername",items.trainingcenter.Name},
                { "ispay",items.signupstudent.IsPay},
                { "signupcollectioncount",collection.Count()},
                { "signupcollection",collection.Select(c=>new {
                    id = c.l.Id,
                    examsubjectname = c.r.Name,
                    signupstudentid = c.l.SignUpStudentId
                })}
            };
            try
            {
                return Ok(new { total = collection.Count(), data = result });
            }
            catch (Exception)
            {
                throw;
            }
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
            List<dynamic> exists = new List<dynamic>();
            List<SignUp> signUps = new List<SignUp>();
            List<ExamRoomPlanTicket> examRoomPlanTickeds = new List<ExamRoomPlanTicket>();
            foreach (var item in signUpCollection)
            {
                var isExists = signUpRepository.Query(c => c.StudentId == item.StudentId && c.ExamSubjectId == item.ExamSubjectId).Any();
                if (isExists)//如果报名表详情中人员和科目存在正式报名表中了 则添加到存在列表中用于返回客户端显示
                {
                    exists.Add(new { studentname = item.Student.Name,examsubjectname= item.ExamSubject.Name });
                }
                else//否则添加到正式报名表中 并增加人员生成准考证权限
                {
                    SignUp signUp = new SignUp(item.StudentId, item.ExamSubjectId, signUpForUnit.Id, loginTrainingCenterId, false, DateTime.Now);
                    signUps.Add(signUp);
                    //item.Student.AddaTicketCtr();
                    //string idcard = item.Student.IDCard;
                    //string num = idcard.Substring(idcard.Length - 4, 4);
                    ExamRoomPlanTicket ticket = new ExamRoomPlanTicket(Guid.NewGuid().ToString().Replace("-",""), item.StudentId, loginTrainingCenterId);
                    examRoomPlanTickeds.Add(ticket);
                }
            }
            if (exists.Any())
            {
                var existsliststr = string.Join("<br >", exists.Select(c => string.Format("姓名:{0} - 科目:{1}", c.studentname, c.examsubjectname)));
                return Ok(string.Format("报名表中包含已经报名成功的人员<br>{0}", existsliststr));
            }
            //if (!signUps.Any())
            //{
            //    return Ok("发生错误，本次表报名失败");
            //}
            signUpForUnit.PayToSuccess();
            signUpForUnitRepository.Update(signUpForUnit);
            await signUpRepository.InsertAsync(signUps);
            //signUpCollectionRepository.Update(signUpCollection);
            await examRoomPlanTicketRepository.InsertAsync(examRoomPlanTickeds);
            await unitOfWork.SaveChangesAsync();
            return Ok("报名成功");
        }

        /// <summary>
        /// 正式报名(个人)
        /// </summary>
        /// <param name="signupforunitid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("signupofficial_student")]
        [Authorize(Roles = "TrainingCenter")]
        public async Task<IActionResult> SignUp_Student(int signupforstudentid)
        {
            var loginTrainingCenterId = int.Parse(User.FindFirstValue("WorkUnitId"));
            var signUpForStudent = await (from signupstudent in context.SignUpStudent
                                    join student in context.Students on signupstudent.StudentId equals student.Id
                                    where signupstudent.Id == signupforstudentid
                                    select new { signupstudent, student }).FirstOrDefaultAsync();
            
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

            if (signUpForStudent == null)
            {
                return Ok("未找到报名表");
            }
            if (signUpForStudent.signupstudent.IsPay)
            {
                return Ok("报名表已经扫描过了");
            }
            if (signUpForStudent.signupstudent.TrainingCenterId != loginTrainingCenterId)
            {
                return Ok("该报名表不属于该培训点，不能进行表报名");
            }

            var signUpStudentCollection = await (from signupstudentcollection in context.SignUpStudentCollection
                                           join examsubject in context.ExamSubjects on signupstudentcollection.ExamSubjectId equals examsubject.Id
                                           where signupstudentcollection.SignUpStudentId == signUpForStudent.signupstudent.Id
                                                 select new { signupstudentcollection,examsubject}).ToListAsync();

                                         
                

            if (!signUpStudentCollection.Any())
            {
                return Ok("该报名表是空的，并不能进行报名");
            }
            //Dictionary<string, string> exists = new Dictionary<string, string>();
            List<dynamic> exists = new List<dynamic>();
            List<SignUp> signUps = new List<SignUp>();
            List<ExamRoomPlanTicket> examRoomPlanTickeds = new List<ExamRoomPlanTicket>();
            foreach (var item in signUpStudentCollection)
            {
                var isExists = await context.SignUp.Where(c => c.StudentId == signUpForStudent.signupstudent.StudentId && c.ExamSubjectId == item.signupstudentcollection.ExamSubjectId).AnyAsync();
                if (isExists)//如果报名表详情中人员和科目存在正式报名表中了 则添加到存在列表中用于返回客户端显示
                {
                    exists.Add(new { studentname = signUpForStudent.student.Name, examsubjectname= item.examsubject.Name });
                }
                else//否则添加到正式报名表中 并增加人员生成准考证权限
                {
                    SignUp signUp = new SignUp(signUpForStudent.signupstudent.StudentId, item.examsubject.Id, null, loginTrainingCenterId, false, DateTime.Now);
                    signUps.Add(signUp);
                    //signUpForStudent.student.AddaTicketCtr();
                    
                    ExamRoomPlanTicket ticket = new ExamRoomPlanTicket(Guid.NewGuid().ToString().Replace("-", ""), signUpForStudent.signupstudent.StudentId, loginTrainingCenterId);
                    examRoomPlanTickeds.Add(ticket);
                }
            }
            if (exists.Any())
            {
                var existsliststr = string.Join("<br >", exists.Select(c => string.Format("姓名:{0} - 科目:{1}", c.studentname, c.examsubjectname)));
                return Ok(string.Format("报名表中包含已经报名成功的人员<br>{0}", existsliststr));
            }
            //if (!signUps.Any())
            //{
            //    return Ok("发生错误，本次表报名失败");
            //}
            signUpForStudent.signupstudent.PayToSuccess();
            context.SignUpStudent.Update(signUpForStudent.signupstudent);
            await context.SignUp.AddRangeAsync(signUps);
            await context.ExamRoomPlanTicket.AddRangeAsync(examRoomPlanTickeds);
            await context.SaveChangesAsync();
            return Ok("报名成功");
        }


        /// <summary>
        /// 个人显示报名表
        /// </summary>
        /// <param name="signUpForUnitId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("signupstudent")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SignUpForStudentAndCollectionView(int signupforstudentid)
        {
            var search = from signupstudent in context.SignUpStudent
                         join trainingcenter in context.TrainingCenter on signupstudent.TrainingCenterId equals trainingcenter.Id
                         join student in context.Students on signupstudent.StudentId equals student.Id
                         where signupstudent.Id == signupforstudentid
                         select new { signupstudent, trainingcenter, student };

            var items = await search.FirstOrDefaultAsync();

            ///取得科目开设信息
            var examsubjectOpenInfo = context.ExamSubjectOpenInfo.FirstOrDefault(c => c.TrainingCenterId == items.trainingcenter.Id);

            var collection = context.SignUpStudentCollection.Where(c => c.SignUpStudentId == items.signupstudent.Id)
                .Join(context.ExamSubjects, l => l.ExamSubjectId, r => r.Id, (l, r) => new { l, r });
            
            //var items_child = await search_child.ToListAsync();

            var result = new Dictionary<string, object>
            {
                { "id",items.signupstudent.Id},
                { "code",items.signupstudent.Code},
                { "studentname",items.student.Name},
                { "studentidcard",items.student.IDCard},
                { "trainingcentername",items.trainingcenter.Name},
                { "ispay",items.signupstudent.IsPay},
                { "gotovaldatetime",examsubjectOpenInfo.GoToValDateTime},
                { "signupcollectioncount",collection.Count()},
                { "signupcollection",collection.Select(c=>new {
                    id = c.l.Id,
                    examsubjectname = c.r.Name,
                    signupstudentid = c.l.SignUpStudentId
                })},
                {"address",items.trainingcenter.Address},
                {"tel",items.trainingcenter.Tel}
            };
            try
            {
                return Ok(new { total = collection.Count(), data = result });
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}