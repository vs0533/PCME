using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<SignUpCollection> signUpCollection;
        public PrintViewController(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            signUpForUnitRepository = unitOfWork.GetRepository<SignUpForUnit>();
            signUpCollection = unitOfWork.GetRepository<SignUpCollection>();
        }
        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUpForUnitAndCollectionView(int signUpForUnitId)
        {
            var search = signUpForUnitRepository.Query(c => c.Id == signUpForUnitId, include: s =>
              s.Include(c => c.WorkUnit)
              .Include(c => c.TrainingCenter)
            );
            var items = await search.FirstOrDefaultAsync();

            var search_child = signUpCollection.Query(c => c.SignUpForUnitId == items.Id, include: s => s
                  .Include(c => c.ExamSubject)
                  .Include(c => c.Student)
            );
            var items_child = await search_child.ToListAsync();

            var result = new Dictionary<string, object>
            {
                { "id",items.Id},
                { "code",items.Code},
                { "workunitname",items.WorkUnit.Name},
                { "trainingcentername",items.TrainingCenter.Name},
                { "islock",items.IsLock},
                { "ispay",items.IsPay},
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
            catch (Exception ex)
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
                bmp = qrCode.GetGraphic(40);
                g = Graphics.FromImage(bmp);
                bmp.Save(ms, ImageFormat.Jpeg);
                return File(ms.ToArray(), "image/jpeg");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            finally
            {
                g.Dispose();
                bmp.Dispose();
            }
        }
        
    }
}