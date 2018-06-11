using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public IActionResult Post()
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);

            //size > 100MB refuse upload !

            if (size > 104857600)
            {
                return BadRequest(new { success = false,message="文件大小超出限制" });
            }
            List<string> filePathResultList = new List<string>();
            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                string filePath = hostingEnv.WebRootPath + $@"\Files";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                fileName = Guid.NewGuid() + "." + fileName.Split('.')[1];
                string fileFullName = Path.Combine(filePath,fileName);
                using (FileStream fs = System.IO.File.Create(fileFullName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                filePathResultList.Add($"/src/Files/{fileName}");
            }
            string message = $"{files.Count} file(s) /{size} bytes uploaded successfully!";

            return Json(new { success=true,paths=filePathResultList,count=filePathResultList.Count()});
        }
    }
}
