using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Files")]
    public class FilesController:Controller
    {
        private IHostingEnvironment hostingEnv;
        public FilesController(IHostingEnvironment hostingEnv)
        {
            this.hostingEnv = hostingEnv;
        }
        [HttpPost]
        [Route("favicon")]
        public IActionResult Post()
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);

            //size > 100MB refuse upload !
            string base64 = string.Empty;
            if (size > 307200)
            {
                return BadRequest(new { success = false,msg="文件大小超出300KB限制" });
            }
            try
            {
                using (var ms = new MemoryStream())
                {
                    Image image = Image.FromStream(files[0].OpenReadStream(), true, true);
                    Bitmap bmp = new Bitmap(image);
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);
                    ms.Close();
                    base64 = Convert.ToBase64String(arr);
                }
                return Content(JsonConvert.SerializeObject(new { msg = base64, success = true }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message, success = true }); ;
            }
            //List<string> filePathResultList = new List<string>();
            //foreach (var file in files)
            //{

            //var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

            //string filePath = hostingEnv.WebRootPath + $@"\Files";

            //if (!Directory.Exists(filePath))
            //{
            //    Directory.CreateDirectory(filePath);
            //}

            //fileName = Guid.NewGuid() + "." + fileName.Split('.')[1];
            //string fileFullName = Path.Combine(filePath,fileName);
            //using (FileStream fs = System.IO.File.Create(fileFullName))
            //{
            //    file.CopyTo(fs);
            //    fs.Flush();
            //}
            //filePathResultList.Add($"/src/Files/{fileName}");
            //}
            //string message = $"{files.Count} file(s) /{size} bytes uploaded successfully!";

            //return Json(new { success=true,paths=filePathResultList,count=filePathResultList.Count()});
        }
    }
}
