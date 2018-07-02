using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Login")]
    [Authorize]
    public class LoginController : Controller
    {
        //[HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public IActionResult GetSystemInfo()
        {
            Dictionary<string, object> cfg = new Dictionary<string, object>();
            Dictionary<string, object> company = new Dictionary<string, object>();
            Dictionary<string, object> systeminfo = new Dictionary<string, object>();
            company.Add("companyid", "1");
            company.Add("companyname", "卫盛科技");
            company.Add("companylongname", "淄博卫盛科技有限公司");
            company.Add("address", "人民西路45号");
            company.Add("linkmen", "技术支持");
            company.Add("telnumber", "0533-2791836");
            company.Add("servicedepartment", "研发部");
            company.Add("servicemen", "技术支持");
            company.Add("servicetelnumber", "18653311771");
            company.Add("serviceqq", "3447063");
            company.Add("serviceemail", "3447063@qq.com");
            company.Add("servicehomepage", "http://www.zbpe.gov.cn");

            systeminfo.Add("systemname", "继续教育管理系统");
            systeminfo.Add("systemversion", "ver 1.0.0.0");
            systeminfo.Add("iconurl", "");
            systeminfo.Add("iconcls", "");
            systeminfo.Add("systemaddition", "");
            systeminfo.Add("copyrightowner", "淄博市人力资源和社会保障局");
            systeminfo.Add("copyrightinfo", "淄博市人力资源和社会保障局");
            systeminfo.Add("allowsavepassword", "");
            systeminfo.Add("savepassworddays", "");
            systeminfo.Add("needidentifingcode", "");
            systeminfo.Add("alwaysneedidentifingcode", "");
            systeminfo.Add("forgetpassword", "忘记密码找管理员");

            cfg.Add("systeminfo", systeminfo);
            cfg.Add("company", company);
            return Ok(cfg);
        }
        [Route("[action]")]
        public IActionResult GetUserInfo() {
            var result = GetDict(User.Claims);
            //return Ok(from c in User.Claims select new { c.Type, c.Value });
            return Ok(result);
        }
        private Dictionary<string, string> GetDict(IEnumerable<Claim> claims)
        {
            var d = new Dictionary<string, string>();
            foreach (var item in claims)
            {
                if (!d.Any(c=>c.Key == item.Type))
                {
                    d.Add(item.Type, item.Value);
                }
                
            }
            return d;
        }
    }
}
